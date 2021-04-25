using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFunc.Maths.Expressions;
using xFunc.Maths.Tokens;
using xueqiao.trade.hosting;
using XueQiaoFoundation.Shared.Helper;
using XueQiaoWaf.Trade.Modules.Domain.Trades;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    public static class AddComposeHelper
    {
        /// <summary>
        /// 将<see cref="AddCompose"/>数据转化为<see cref="HostingComposeGraph"/>数据。同时获取到别名
        /// </summary>
        /// <param name="domainCompose"></param>
        /// <param name="outTargetCompose"></param>
        /// <param name="outTargetAliasName"></param>
        public static void DomainAddComposeToServerCompose(AddCompose domainCompose, out HostingComposeGraph outTargetCompose, out string outTargetAliasName)
        {
            if (domainCompose == null)
            {
                outTargetCompose = null;
                outTargetAliasName = null;
                return;
            }

            var composeUnits = domainCompose.ComposeUnits.ToArray();
            var serverCompose = new HostingComposeGraph
            {
                Formular = domainCompose.Formular,
                Legs = new Dictionary<string, HostingComposeLeg>()
            };

            for (int i = 0; i < composeUnits.Length; i++)
            {
                var unit = composeUnits[i];
                var variableName = CommonConvert.IndexUnder23ToLetter(i, true, null, null);
                var leg = new HostingComposeLeg
                {
                    SledContractId = unit.ContractId,
                    VariableName = variableName,
                    Quantity = unit.Quantity,
                    LegTradeDirection = unit.Direction == ClientTradeDirection.BUY ? HostingComposeLegTradeDirection.COMPOSE_LEG_BUY : HostingComposeLegTradeDirection.COMPOSE_LEG_SELL,
                };
                serverCompose.Legs[variableName] = leg;
            }

            outTargetCompose = serverCompose;
            outTargetAliasName = domainCompose.ComposeName;
        }

        public static void TrialCalculateComposeAskBidPrices(
            string formula,
            int precisionNumber,
            Func<string, double?> legCommodityTickSizeByVariableGetter,
            Func<string, ClientTradeDirection?> legDirectionByVariableGetter,
            out double? trialAskPrice, out double? trialBidPrice, out string trialErrorMsg)
        {
            trialAskPrice = null;
            trialBidPrice = null;
            trialErrorMsg = null;

            Debug.Assert(!string.IsNullOrEmpty(formula));
            Debug.Assert(legCommodityTickSizeByVariableGetter != null);
            Debug.Assert(legDirectionByVariableGetter != null);

            var formalaProcessor = new xFunc.Maths.Processor();
            IExpression formulaExp = null;
            try
            {
                formulaExp = formalaProcessor.Parse(formula);
            }
            catch (Exception)
            {
            }
            if (formulaExp == null)
            {
                trialErrorMsg = "请检查公式正确性";
                return;
            }

            var variableTokens =  formalaProcessor.Lexer.Tokenize(formula)?.OfType<VariableToken>()?.ToArray();
            if (variableTokens?.Any() != true)
            {
                trialErrorMsg = "公式未提供任何变量，请检查公式正确性";
                return;
            }
            
            var combTrialAskPriceProcessor = new xFunc.Maths.Processor();
            var combTrialBidPriceProcessor = new xFunc.Maths.Processor();
            foreach (var variableToken in variableTokens)
            {
                var legVariableName = variableToken.Variable;
                var legTickSize = legCommodityTickSizeByVariableGetter(legVariableName);
                var legDir = legDirectionByVariableGetter(legVariableName);

                if (!legTickSize.HasValue || !legDir.HasValue)
                    continue;

                // 人为给定腿的试算买卖价，再去算组合的买卖价
                var legTrialBidPrice = legTickSize.Value;
                var legTrialAskPrice = legTickSize.Value + legTickSize.Value;
                
                // 试算combBidPrice 买价, combAskPrice 卖价
                if (legDir == ClientTradeDirection.BUY)
                {
                    combTrialBidPriceProcessor.Parameters.Variables.Add(legVariableName, legTrialBidPrice);
                    combTrialAskPriceProcessor.Parameters.Variables.Add(legVariableName, legTrialAskPrice);
                }
                else
                {
                    combTrialBidPriceProcessor.Parameters.Variables.Add(legVariableName, legTrialAskPrice);
                    combTrialAskPriceProcessor.Parameters.Variables.Add(legVariableName, legTrialBidPrice);
                }
                
            }


            object combTrialBidPrice = null;
            object combTrialAskPrice = null;
            try
            {
                combTrialBidPrice = combTrialBidPriceProcessor.Solve(formula).Result;
                combTrialAskPrice = combTrialAskPriceProcessor.Solve(formula).Result;
            }
            catch (Exception)
            {
                trialErrorMsg = "请检查公式正确性";
                return;
            }

            if (!(combTrialBidPrice is double doubleTrialBid) || !(combTrialAskPrice is double doubleTrialAsk))
            {
                trialErrorMsg = "请检查公式正确性";
                return;
            }

            string strTrialBid = $"{doubleTrialBid}", strTrialAsk = $"{doubleTrialAsk}";
            if (strTrialBid == strTrialAsk)
            {
                trialErrorMsg = "试算买卖价相同，请修改小数位数或公式";
                return;
            }

            var tickSizedTrialBid = MathHelper.MakeValuePrecise(doubleTrialBid, precisionNumber);
            var tickSizedTrialAsk = MathHelper.MakeValuePrecise(doubleTrialAsk, precisionNumber);
            if (tickSizedTrialBid == tickSizedTrialAsk)
            {
                var trialBidPieceTuple = MathHelper.GetDecimalPieces(new decimal(doubleTrialBid));
                var trialAskPieceTuple = MathHelper.GetDecimalPieces(new decimal(doubleTrialAsk));
                if (trialBidPieceTuple.Item1 == trialAskPieceTuple.Item1)
                {
                    if (trialBidPieceTuple.Item2 != trialAskPieceTuple.Item2)
                    {
                        string bidPriceDecimalPiece = trialBidPieceTuple.Item2, askPriceDecimalPiece = trialAskPieceTuple.Item2;

                        // 计算开始不同的位数
                        // 使两者位数一致
                        var decimalPieceLengthDelta = bidPriceDecimalPiece.Length - askPriceDecimalPiece.Length;
                        if (decimalPieceLengthDelta > 0)
                        {
                            askPriceDecimalPiece += new string(Enumerable.Repeat('0', decimalPieceLengthDelta).ToArray());
                        }
                        else
                        {
                            bidPriceDecimalPiece += new string(Enumerable.Repeat('0', -decimalPieceLengthDelta).ToArray());
                        }

                        // 计算两者的开始出现不同数字的地方
                        int? diffCharIdx = null;
                        for (var i = 0; i < bidPriceDecimalPiece.Length; i++)
                        {
                            if (bidPriceDecimalPiece[i] != askPriceDecimalPiece[i])
                            {
                                diffCharIdx = i;
                            }
                        }

                        if (diffCharIdx != null)
                        {
                            var magnify = ((diffCharIdx.Value + 1) - precisionNumber) * 10;

                            trialErrorMsg = $"试算买卖价相同，建议将公式放大{magnify}倍，\n或修改小数位数或公式";
                            return;
                        }
                        else
                        {
                            trialErrorMsg = "试算买卖价相同，请修改小数位数或公式";
                            return;
                        }
                    }
                    else
                    {
                        trialErrorMsg = "试算买卖价相同，请修改小数位数或公式";
                        return;
                    }
                }
            }

            trialAskPrice = tickSizedTrialBid;
            trialBidPrice = tickSizedTrialAsk;
        }
    }
}
