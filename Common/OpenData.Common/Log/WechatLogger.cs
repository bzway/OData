using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using System.Text;
using System.Reflection;


namespace OpenData.Log
{
    public class WechatLogger
    {
        public Int64 MsgId { get; set; }
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string MsgType { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public string ContentXML { get; set; }

        static ILog log = log4net.LogManager.GetLogger("Bzway.Wechat", typeof(WechatLogger));
        static void Log(Int64 MsgId, string ToUserName,
            string FromUserName, string MsgType, string Content, DateTime CreateTime, string ContentXML)
        {
            var wsLog = new WechatLogger()
            {
                Content = Content,
                ContentXML = ContentXML,
                CreateTime = CreateTime,
                FromUserName = FromUserName,
                MsgId = MsgId,
                MsgType = MsgType,
                ToUserName = ToUserName
            };
            log.Info(wsLog);
        }
    }


    public class WechatLog
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("WechatLog:");
            foreach (PropertyInfo info in this.GetType().GetProperties())
            {
                if (info.CanRead)
                {
                    //object o = info.GetValue( this, null );
                    object o = null;
                    try { o = info.GetValue(this, null); }
                    catch { }
                    sb.Append(string.Format("[{0}:{1}]", info.Name, o == null ? "<NULL>" : o.ToString()));
                }
            }
            return sb.ToString();
        }
    }
}