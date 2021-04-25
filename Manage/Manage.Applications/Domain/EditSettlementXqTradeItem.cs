using Manage.Applications.Properties;
using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Model;

namespace Manage.Applications.Domain
{
    public class EditSettlementXqTradeItem : ValidationModel, ICloneable
    {
        private DateTime? tradeDate;
        [Required(ErrorMessageResourceName = "EditSettlementXqTradeItem_TradeDateRequired", ErrorMessageResourceType = typeof(Resources))]
        public DateTime? TradeDate
        {
            get { return tradeDate; }
            set { SetProperty(ref tradeDate, value); }
        }

        private int? contractId;
        [Required(ErrorMessageResourceName = "EditSettlementXqTradeItem_ContractIdRequired", ErrorMessageResourceType = typeof(Resources))]
        public int? ContractId
        {
            get { return contractId; }
            set { SetProperty(ref contractId, value); }
        }

        private ClientTradeDirection? direction;
        [Required(ErrorMessageResourceName = "EditSettlementXqTradeItem_DirectionRequired", ErrorMessageResourceType = typeof(Resources))]
        public ClientTradeDirection? Direction
        {
            get { return direction; }
            set { SetProperty(ref direction, value); }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }

        private int quantity = 1;
        [Range(1, int.MaxValue, ErrorMessageResourceName = "EditSettlementXqTradeItem_QuantityEqualOrLarger", ErrorMessageResourceType = typeof(Resources))]
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        public object Clone()
        {
            return new EditSettlementXqTradeItem
            {
                TradeDate = this.TradeDate,
                ContractId = this.ContractId,
                Direction = this.Direction,
                Price = this.price,
                Quantity = this.quantity,
            };
        }
    }
}
