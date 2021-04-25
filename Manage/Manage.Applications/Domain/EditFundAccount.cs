using Manage.Applications.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Waf.Foundation;
using xueqiao.broker;

namespace Manage.Applications.Domain
{
    public class EditFundAccount : ValidatableModel
    {
        private string loginUserName;
        [Required(ErrorMessageResourceName = "FundAccountLoginUserNameRequired", ErrorMessageResourceType = typeof(Resources))]
        public string LoginUserName
        {
            get { return loginUserName; }
            set { SetPropertyAndValidate(ref loginUserName, value); }
        }
        
        private string accountAlias;
        public string AccountAlias
        {
            get { return accountAlias; }
            set { SetPropertyAndValidate(ref accountAlias, value); }
        }
        
        private string es9_AuthCode;
        /// <summary>
        /// 易盛9.0 auth code 
        /// </summary>
        public string Es9_AuthCode
        {
            get { return es9_AuthCode; }
            set { SetPropertyAndValidate(ref es9_AuthCode, value); }
        }
        
        private string es3_AppId;
        /// <summary>
        /// 易盛3.0 app id
        /// </summary>
        public string Es3_AppId
        {
            get { return es3_AppId; }
            set { SetPropertyAndValidate(ref es3_AppId, value); }
        }
        
        private string es3_CertInfo;
        /// <summary>
        /// 易盛3.0 cert info
        /// </summary>
        public string Es3_CertInfo
        {
            get { return es3_CertInfo; }
            set { SetPropertyAndValidate(ref es3_CertInfo, value); }
        }

        private string ctp_AppId;
        /// <summary>
        /// CTP APPID 
        /// </summary>
        public string Ctp_AppId
        {
            get { return ctp_AppId; }
            set { SetPropertyAndValidate(ref ctp_AppId, value); }
        }

        private string ctp_AuthCode;
        /// <summary>
        /// CTP auth code
        /// </summary>
        public string Ctp_AuthCode
        {
            get { return ctp_AuthCode; }
            set { SetPropertyAndValidate(ref ctp_AuthCode, value); }
        }

    }
}
