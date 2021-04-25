using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    /// <summary>
    /// 邮件客户端
    /// </summary>
    public class EmailClient
    {
        /// <summary>
        /// 发送者
        /// </summary>
        public string mailFrom { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] mailToArray { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] mailCcArray { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string mailSubject { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string mailBody { get; set; }

        /// <summary>
        /// 发件人密码
        /// </summary>
        public string mailPwd { get; set; }

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 正文是否是html格式
        /// </summary>
        public bool isbodyHtml { get; set; }
        
        /// <summary>
        /// 附件。Item1：附件文件名称，Item2：文件流
        /// </summary>
        public Tuple<string, Stream>[] attachmentsNameStream { get; set; }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <returns>是否发送成功</returns>
        public bool Send(out Exception exception)
        {
            exception = null;

            //使用指定的邮件地址初始化MailAddress实例
            MailAddress maddr = new MailAddress(mailFrom);
            //初始化MailMessage实例
            MailMessage myMail = new MailMessage();


            //向收件人地址集合添加邮件地址
            if (mailToArray != null)
            {
                for (int i = 0; i < mailToArray.Length; i++)
                {
                    myMail.To.Add(mailToArray[i].ToString());
                }
            }

            //向抄送收件人地址集合添加邮件地址
            if (mailCcArray != null)
            {
                for (int i = 0; i < mailCcArray.Length; i++)
                {
                    myMail.CC.Add(mailCcArray[i].ToString());
                }
            }
            //发件人地址
            myMail.From = maddr;

            //电子邮件的标题
            myMail.Subject = mailSubject;

            //电子邮件的主题内容使用的编码
            myMail.SubjectEncoding = Encoding.UTF8;

            //电子邮件正文
            myMail.Body = mailBody;

            //电子邮件正文的编码
            myMail.BodyEncoding = Encoding.Default;

            myMail.Priority = MailPriority.High;

            myMail.IsBodyHtml = isbodyHtml;

            //在有附件的情况下添加附件
            try
            {
                if (attachmentsNameStream != null && attachmentsNameStream.Length > 0)
                {
                    Attachment attachFile = null;
                    foreach (var a in attachmentsNameStream)
                    {
                        attachFile = new Attachment(a.Item2, a.Item1);
                        myMail.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }

            using (var smtp = new SmtpClient())
            {
                //指定发件人的邮件地址和密码以验证发件人身份
                smtp.Credentials = new System.Net.NetworkCredential(mailFrom, mailPwd);
                //设置SMTP邮件服务器
                smtp.Host = host;

                try
                {
                    //将邮件发送到SMTP邮件服务器
                    smtp.Send(myMail);
                    return true;
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    exception = ex;
                    return false;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
            }
        }
    }
}
