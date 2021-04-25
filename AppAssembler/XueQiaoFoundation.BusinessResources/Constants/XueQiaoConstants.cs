using NativeModel.Trade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XueQiaoFoundation.Shared.Helper;

namespace XueQiaoFoundation.BusinessResources.Constants
{
    public static class XueQiaoConstants
    {
        /// <summary>
        /// 可撤单的的订单状态列表
        /// </summary>
        public readonly static ClientXQOrderState[] RevokeEnabledOrderStates
            = new ClientXQOrderState[]
            {
                ClientXQOrderState.ClientInaccurate_Unkown,
                ClientXQOrderState.Client_RequestCreating,
                ClientXQOrderState.Client_RequestRevoking,
                ClientXQOrderState.XQ_ORDER_CREATED,
                ClientXQOrderState.XQ_ORDER_SUSPENDED,
                ClientXQOrderState.XQ_ORDER_SUSPENDING,
                ClientXQOrderState.XQ_ORDER_RUNNING,
                ClientXQOrderState.XQ_ORDER_STARTING
            };

        /// <summary>
        /// 可暂停的订单状态列表
        /// </summary>
        public readonly static ClientXQOrderState[] SuspendEnabledOrderStates
            = new ClientXQOrderState[]
            {
                ClientXQOrderState.ClientInaccurate_Unkown,
                ClientXQOrderState.XQ_ORDER_CREATED,
                ClientXQOrderState.XQ_ORDER_RUNNING,
                ClientXQOrderState.XQ_ORDER_STARTING
            };

        /// <summary>
        /// 未结束的订单状态列表
        /// </summary>
        public readonly static ClientXQOrderState[] UnfinishedOrderStates
            = new ClientXQOrderState[]
            {
                ClientXQOrderState.ClientInaccurate_Unkown,
                ClientXQOrderState.Client_RequestCreating,
                ClientXQOrderState.Client_RequestRevoking,
                ClientXQOrderState.Client_RequestSuspending,
                ClientXQOrderState.Client_RequestResuming,
                ClientXQOrderState.Client_RequestStrongChasing,
                ClientXQOrderState.XQ_ORDER_CREATED,
                ClientXQOrderState.XQ_ORDER_SUSPENDED,
                ClientXQOrderState.XQ_ORDER_SUSPENDING,
                ClientXQOrderState.XQ_ORDER_RUNNING,
                ClientXQOrderState.XQ_ORDER_STARTING
            };

        /// <summary>
        /// 不明确状态的订单状态列表
        /// </summary>
        public readonly static ClientXQOrderState[] AmbiguousOrderStates
            = new ClientXQOrderState[] 
            {
                ClientXQOrderState.ClientInaccurate_Unkown,
                ClientXQOrderState.Client_RequestCreating,
                ClientXQOrderState.Client_RequestRevoking,
                ClientXQOrderState.Client_RequestSuspending,
                ClientXQOrderState.Client_RequestResuming,
                ClientXQOrderState.Client_RequestStrongChasing
            };

        /// <summary>
        /// 应用的公司名称
        /// </summary>
        public const string AppEnglishCompanyName = "XueQiao";

        public const string AppEnglishName = "XueQiaoTrading";
        
        /// <summary>
        /// 当前应用的非漫游数据存放目录完整路径
        /// </summary>
        public readonly static string AppLocalDataDirectoryFullName 
            = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                AppEnglishCompanyName, AppEnglishName);
        
        /// <summary>
        /// 雪橇标的的价格准确度容忍度值。用于判断浮点型价格是否相等
        /// </summary>
        public const double XQTargetPriceAccuracyTolerance = 0.00000001;

        /// <summary>
        /// 雪橇乘(包括乘除等复杂计算)计算后的Tick价格率。如平均价格就是价格的除计算结果，它的 tick 就是要在价格 tick 上乘以<see cref="XQMultipleCalculatedPriceTickRate"/>
        /// </summary>
        public const double XQMultipleCalculatedPriceTickRate = 0.1;

        /// <summary>
        /// 雪橇合约价格的最小 tick，用于精确价格显示
        /// </summary>
        public const double XQContractPriceMinimumPirceTick = 0.000001;
        
        /// <summary>
        /// 雪橇合约价格的最小 tick <see cref="XQContractPriceMinimumPirceTick"/> 的小数位数
        /// </summary>
        public static readonly short XQContractPriceMinimumPirceTick_DecimalCount = (short)MathHelper.GetDecimalCount(new decimal(XQContractPriceMinimumPirceTick));

        /// <summary>
        /// 雪橇合约价格的最大 tick，用于精确价格显示
        /// </summary>
        public const double XQComposePriceMaximumPirceTick = 0.01;

        /// <summary>
        /// 雪橇组合价格支持的最少小数位数
        /// </summary>
        public static readonly short XQComposePrice_LowerDecimalCount = (short)MathHelper.GetDecimalCount(new decimal(XQComposePriceMaximumPirceTick));

        public static readonly string XQComposePrice_LowerDecimalCount_String = $"{XQComposePrice_LowerDecimalCount}";

        /// <summary>
        /// 雪橇组合价格支持的最多小数位数
        /// </summary>
        public readonly static short XQComposePrice_UpperDecimalCount = (short)MathHelper.GetDecimalCount(new decimal(XQContractPriceMinimumPirceTick));


        #region UserSettingKey (不超过32位长度)

        // 用户设置 key:用户的订阅数据
        public const string UserSettingKey_UserSubscribeData = "kUserSubscribeData";
        // 用户设置 key:交易工作空间窗口树
        public const string UserSettingKey_TradeWorkspaceWindowTree = "kUserWorkspaceData";
        // 用户设置 key:工作空间模板数据
        public const string UserSettingKey_TradeWorkspaceTemplateData = "ca8d56f31b424b5aae1b9b4e748f5845";
        // 用户设置 key:交易组件中的列表列信息数据
        public const string UserSettingKey_TradeComponentListColumnInfosData = "70bb202af5664f939dd0daa28a330e55";

        // 用户设置 key:雪橇成交预录入(Xq Trade Preview Input)数据
        public const string UserSettingKey_XqTPIData = "XqTPIData";

        // 用户设置 key:持仓预分配(Position Preview Assign)数据
        public const string UserSettingKey_PPAData = "PPAData";

        // 用户设置 key:雪橇组合订单执行参数模板(Execute Parameters Template)数据
        public const string UserSettingKey_XQComposeOrderEPTData = "XQComposeOrderEPTData";

        // 用户设置 key:投研工作空间窗口树
        public const string UserSettingKey_ResearchWorkspaceWindowTree = "kResearchWorkspaceWindowData";

        // 交易模块工作空间用户设置 key 的前缀
        public const string UserSettingKey_TradeWorkspaceKeyPrefix = "TRWS_";
        
        // 投研模块工作空间用户设置 key 的前缀
        public const string UserSettingKey_ResearchWorkspaceKeyPrefix = "REWS_";

        #endregion

        // 登录密码的 RSA public key xml形式文本
        public const string LoginPWDRSAPublicKeyXml = @"<RSAKeyValue><Modulus>py2Gv5gGq9oVSiTxyvyhiTWqtjQV/Syz0Q+vigFoeb2SMLt7QOri32+v/xQtr2ozWMHosOYKY32YD2zvFm9qjsqYYCa6lpHti/cAVpeUKPhdlIfkOwGqFw7Ic3COk/cQ6xuBeowp0Fco41bwgdq1wOubb641LsuSERaFVj+cYRU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";


        #region 交易组件类型

        // 合约列表
        public const int TradeCompType_CONTRACT_LIST = 0;
        // 组合列表
        public const int TradeCompType_COMPOSE_LIST = 1;

        // 下单
        public const int TradeCompType_PLACE_ORDER = 10;

        // 委托单列表
        public const int TradeCompType_ENTRUSTED_ORDER_LIST = 20;
        // 预埋单列表
        public const int TradeCompType_PARKED_ORDER_LIST = 21;
        // 预埋单列表
        public const int TradeCompType_CONDITION_ORDER_LIST = 22;

        public const int TradeCompType_TRADE_LIST = 30;                 // 成交列表

        public const int TradeCompType_POSITION_LIST = 40;              // 持仓列表
        public const int TradeCompType_POSITION_ASSISTANT = 41;         // 持仓助手


        public const int TradeCompType_ORDER_HISTORY = 50;              // 订单历史
        public const int TradeCompType_TRADE_HISTORY = 51;              // 成交历史
        public const int TradeCompType_POSITION_ASSIGN_HISTORY = 52;    // （持仓）历史分配

        public const int TradeCompType_FUND = 60;                       // 资金
        
        #endregion


        #region 投研组件类型

        // url 类型
        public const int ResearchCompType_URL = 1;

        #endregion


        #region 工作空间类型

        public const int WORKSPACE_TRADE_MAIN = 0;    // 交易主工作空间
        public const int WORKSPACE_TRADE_SUB = 1;     // 交易子工作空间
        public const int WORKSPACE_RESEARCH = 10;     // 投研分析工作空间

        #endregion

        /// <summary>
        /// 查询应用更新信息的 app key
        /// </summary>
        public const string QueryUpdateInfoAppKey = "xueqiao_trade";

        #region 列表列内容对齐方式类型

        public const int ListColumnContentAlignment_Left = 0;
        public const int ListColumnContentAlignment_Center = 1;
        public const int ListColumnContentAlignment_Right = 2;

        public static readonly int[] AllListColumnContentAlignments
            = new int[] { ListColumnContentAlignment_Left, ListColumnContentAlignment_Center, ListColumnContentAlignment_Right };

        #endregion


        #region 发布资源目录名称
        // 发布资源需要在发布时拷贝到输出目录下，然后一起打包成安装文件。

        // 发布资源的根目录名称
        public const string PUBLISH_RESOURCE_ROOT_DIR_NAME = "publish_res";

        // 发布资源子目录名称-音频
        public const string PUBLISH_RESOUCE_CHILD_DIR_NAME_SOUNDS = "sounds";

        #endregion


        #region 应用的注册表相关

        // 应用的注册表基本路径 
        public static readonly string ApplicationRegistryKeyBasePath = $"SOFTWARE\\{AppEnglishCompanyName}\\{AppEnglishName}";

        // IsDevelopOpen(是否打开开发模式) 注册表 key
        public const string RegistryKey_IsDevelopOpen = "IsDevelopOpen";

        // (Dev 环境下个人用户 company code) 注册表 key
        public const string RegistryKey_PersonalUserCompanyCode_DEV = "PersonalUserCompanyCode_DEV";

        // (Gamma 环境下个人用户 company code) 注册表 key
        public const string RegistryKey_PersonalUserCompanyCode_GAMMA = "PersonalUserCompanyCode_GAMMA";

        // (IDC 环境下个人用户 company code) 注册表 key
        public const string RegistryKey_PersonalUserCompanyCode_IDC = "PersonalUserCompanyCode_IDC";

        public const string DefaultValue_PersonalUserCompanyCode_DEV = "雪橇dev";
        public const string DefaultValue_PersonalUserCompanyCode_GAMMA = "雪橇云服务个人版";
        // TODO: DefaultValue_PersonalUserCompanyCode_IDC???
        public const string DefaultValue_PersonalUserCompanyCode_IDC = "雪橇测试";

        #endregion

        #region 意见反馈相关
        public const string FeedbackMailFromAddress = "feedbackuser@xueqiao.cn";
        public const string FeedbackMailToAddress = "feedback@xueqiao.cn";
        public const string FeedbackMailSendHost = "smtp.qiye.aliyun.com";
        #endregion
    }
}
