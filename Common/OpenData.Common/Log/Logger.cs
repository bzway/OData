using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace OpenData.Log
{
    public class Logger
    {
        public static void Register(string ConnectionString)
        {
            //Config ADONetAppender with live connection string
            log4net.Repository.ILoggerRepository hier = log4net.LogManager.GetRepository();
            if (hier != null)
            {
                log4net.Appender.AdoNetAppender adoAppender = new log4net.Appender.AdoNetAppender();
                adoAppender.Name = "AdoNetAppender";
                adoAppender.CommandType = CommandType.Text;
                adoAppender.BufferSize = 1;
                adoAppender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                adoAppender.ConnectionString = ConnectionString;
                adoAppender.CommandText = "INSERT INTO [Log4Net] ([Thread], [Level], [Logger], [Source], [Message], [Exception], [Date]) VALUES (@Thread, @Level, @Logger, @Source, @Message, @Exception, @Date)";
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Thread", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) });
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Source", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%file:%method:%line")) });
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Message", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message")) });
                adoAppender.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Exception", DbType = System.Data.DbType.String, Size = 4000, Layout = new Layout2RawLayoutAdapter(new ExceptionLayout()) });
                adoAppender.ActivateOptions();

                log4net.Appender.AdoNetAppender adoAppenderEmail = new log4net.Appender.AdoNetAppender();
                adoAppenderEmail.Name = "AdoNetAppenderEmail";
                adoAppenderEmail.CommandType = CommandType.Text;
                adoAppenderEmail.BufferSize = 1;
                adoAppenderEmail.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                adoAppenderEmail.ConnectionString = ConnectionString;
                adoAppenderEmail.CommandText = "INSERT INTO [Log4Email] ([Thread], [Level], [Logger], [Source], [Date], [From], [To], [Subject], [Body]) VALUES (@Thread, @Level, @Logger, @Source, @Date, @From, @To, @Subject, @Body)";
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Thread", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
                //adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Source", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%file:%method:%line")) });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Source", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("Source") });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@From", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("From") });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@To", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("To") });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Body", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("Body") });
                adoAppenderEmail.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Subject", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("Subject") });
                adoAppenderEmail.ActivateOptions();
                var repositoryEmail = log4net.LogManager.CreateRepository("Bzway.Email");

                log4net.Appender.AdoNetAppender adoAppenderWechat = new log4net.Appender.AdoNetAppender();
                adoAppenderWechat.Name = "AdoNetAppender";
                adoAppenderWechat.CommandType = CommandType.Text;
                adoAppenderWechat.BufferSize = 1;
                adoAppenderWechat.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                adoAppenderWechat.ConnectionString = ConnectionString;
                adoAppenderWechat.CommandText = "INSERT INTO [Log4Wechat] ([Thread], [Level], [Logger], [Source], [Date], [MsgId], [ToUserName], [FromUserName], [MsgType], [Content], [CreateTime], [ContentXML]) VALUES (@Thread, @Level, @Logger, @Source, @Date, @MsgId, @ToUserName, @FromUserName, @MsgType, @Content, @CreateTime, @ContentXML)";
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Thread", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%thread")) });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Level", DbType = System.Data.DbType.String, Size = 50, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level")) });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Logger", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger")) });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Date", DbType = System.Data.DbType.DateTime, Layout = new log4net.Layout.RawTimeStampLayout() });
                //adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Source", DbType = System.Data.DbType.String, Size = 255, Layout = new Layout2RawLayoutAdapter(new PatternLayout("%file:%method:%line")) });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Source", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("Source") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MsgId", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("MsgId") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ToUserName", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("ToUserName") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@FromUserName", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("FromUserName") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@MsgType", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("MsgType") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@Content", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("Content") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@CreateTime", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("CreateTime") });
                adoAppenderWechat.AddParameter(new AdoNetAppenderParameter { ParameterName = "@ContentXML", DbType = System.Data.DbType.String, Size = 255, Layout = new OpenData.Log.PropertyLayout("ContentXML") });
                adoAppenderWechat.ActivateOptions();

                var repositoryWechat = log4net.LogManager.CreateRepository("Bzway.Wechat");
                BasicConfigurator.Configure(adoAppender);
                BasicConfigurator.Configure(repositoryEmail, adoAppenderEmail);
                BasicConfigurator.Configure(repositoryWechat, adoAppenderWechat);
            }
        }
    }
}
