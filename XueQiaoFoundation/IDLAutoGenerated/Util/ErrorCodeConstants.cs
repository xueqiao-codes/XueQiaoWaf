﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDLAutoGenerated.Util
{
    public struct ErrorCodeConstants
    {
        // 出入金重复票据，操作重复
        public const int ERROR_SUB_ACCOUNT_OP_MONEY_DUPLICATE_TICKET = 1702;

        // 组合图不存在
        public const int ERROR_COMPOSE_GRAPH_NOT_EXISTED = 3003;

        // 组合视图不存在
        public const int ERROR_COMPOSE_VIEW_NOT_EXISTED = 3050;

        // 订阅的组合行情过多
        public const int ERROR_COMPOSE_VIEW_SUBSCRIBE_TOO_MANY = 3056;
        
        // 配置版本号过低
        public const int ERROR_CONFIG_VERSION_LOW = 3100;

        // 配置内部原因导致丢失
        public const int ERROR_CONFIG_LOST = 3101;

        // 配置内容具备和服务端版本相同的MD5值
        public const int ERROR_CONFIG_SAME_CONTENT_MD5 = 3102;
    }
}