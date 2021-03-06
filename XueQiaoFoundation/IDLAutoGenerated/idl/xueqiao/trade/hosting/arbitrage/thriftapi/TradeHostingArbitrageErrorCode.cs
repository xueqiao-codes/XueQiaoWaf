/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */

namespace xueqiao.trade.hosting.arbitrage.thriftapi
{
  public enum TradeHostingArbitrageErrorCode
  {
    ERROR_XQ_ORDER_ID_FORMAT = 4000,
    ERROR_XQ_ORDER_TYPE_NOT_SUPPORTED = 4001,
    ERROR_XQ_ORDER_NOT_EXISTS = 4002,
    ERROR_XQ_ORDER_EXISTED = 4003,
    ERROR_XQ_ORDER_EFFECT_DATE_TYPE_NOT_SUPPORTED = 4004,
    ERROR_XQ_ORDER_PARAM_ILLEGAL_TO_SYSTEM_SETTINGS = 4005,
    ERROR_XQ_ORDER_TRADETIME_CONSTRUCT_FAILED = 4100,
    ERROR_XQ_ORDER_TRADETIME_NO_RECENT = 4101,
    ERROR_XQ_ORDER_SUSPENDED_NOTIN_TRADETIME = 4150,
    ERROR_XQ_ORDER_SUSPENDED_BEFORE_EFFECT_TIME_PERIOD = 4151,
    ERROR_XQ_ORDER_SUSPENDED_LEG_ORDER_VERIFY_FAILED = 4199,
    ERROR_XQ_ORDER_SUSPENDED_LEG_ORDER_SEND_FAILED = 4200,
    ERROR_XQ_ORDER_SUSPENDED_LEG_ORDER_CANCELLED_TOO_MANY = 4201,
    ERROR_XQ_ORDER_SUSPENDED_LEG_ORDER_PRICE_PROTECTED = 4202,
    ERROR_XQ_ORDER_SUSPENDED_LEG_LEAK_BUT_TRADE_CLOSE = 4203,
    ERROR_XQ_ORDER_SUSPENDED_INNER_STATE_ERROR = 4204,
    ERROR_XQ_ORDER_SUSPENDED_INNER_EXCEPTION_OCCURS = 4205,
    ERROR_XQ_ORDER_SUSPENDED_OPERATION_INTENT_UNKOWN = 4206,
    ERROR_XQ_ORDER_SUSPENDED_EXEC_ORDER_VERIFY_FAILED = 4207,
    ERROR_XQ_ORDER_SUSPENDED_EXEC_ORDER_SEND_FAILED = 4208,
    ERROR_XQ_ORDER_SUSPENDED_EXEC_ORDER_UPSIDE_REJECTED = 4209,
    ERROR_XQ_ORDER_SUSPENDED_EXEC_ORDER_UPSIDE_CANCELLED = 4210,
    ERROR_XQ_ORDER_SUSPENDED_EXEC_ORDER_UPSIDE_EXPIRED = 4211,
    ERROR_XQ_ORDER_TYPE_SUSPEND_NOT_SUPPORTED = 4600,
    ERROR_XQ_ORDER_STATE_CANNOT_OPERATION = 4601,
    ERROR_XQ_ORDER_NOT_EFFECTIVE_CANNOT_OPERATION = 4602,
    ERROR_XQ_ORDER_CANCELLED_STARTED_BUT_AFTER_EFFECT_TIME_PERIOD = 4698,
    ERROR_XQ_ORDER_CANCELLED_AFTER_EFFECT_TIME_PERIOD = 4699,
    ERROR_XQ_ORDER_CANCELLED_BY_USER = 4700,
    ERROR_XQ_ORDER_CANCELLED_EXEC_ORDER_VERIFY_FAILED = 4701,
    ERROR_XQ_ORDER_CANCELLED_EXEC_ORDER_SEND_FAILED = 4702,
    ERROR_XQ_ORDER_CANCELLED_EXEC_ORDER_UPSIDE_REJECTED = 4703,
    ERROR_XQ_ORDER_CANCELLED_EXEC_ORDER_UPSIDE_CANCELLED = 4704,
    ERROR_XQ_ORDER_CANCELLED_MARKET_CLOSED = 4707,
    ERROR_XQ_ORDER_CANCELLED_ANALYSIS_TRADETIME_FAILED = 4708,
    ERROR_XQ_ORDER_CANCELLED_MARKET_OPENDED_FOR_PARKED = 4709,
    ERROR_XQ_ORDER_CANCELLED_STARTING_TOO_LONG_FOR_PARKED = 4710,
    ERROR_XQ_ORDER_CANCELLED_TRIGGER_RUNNING_TOO_LONG_FOR_PARKED = 4711,
    ERROR_XQ_ORDER_CANCELLED_COMPOSE_EXEC_TYPE_NOT_SUPPORTED = 4712,
    ERROR_XQ_ORDER_CANCELLED_COMPOSE_NO_ACCEPTABLE_FIRSTLEG = 4713,
    ERROR_XQ_ORDER_CANCELLED_INNER_EXCEPTION_OCCURS = 4714,
  }
}
