using Autofac;
using OpenData.Common.AppEngine;
using System.Data.SqlClient;

namespace OpenData.Log
{

    public class LoggerStartupTask : IStartupTask
    {
        public void Execute()
        {
            try
            {
                var commandText = @"
            IF NOT EXISTS (SELECT
	            *
            FROM sys.objects o
            WHERE o.NAME = 'Log4Net' AND o.TYPE = 'U')
            BEGIN

	            CREATE TABLE [dbo].[Log4Net](
		            [ID] [int] IDENTITY(1,1) NOT NULL,
		            [Thread] [nvarchar](1024) NULL,
		            [Level] [nvarchar](1024) NULL,
		            [Logger] [nvarchar](1024) NULL,
		            [Source] [nvarchar](1024) NULL,
		            [Date] [datetime] NOT NULL,
		            [Message] [nvarchar](max) NULL,
		            [Exception] [nvarchar](max) NULL,
		            [CreatedOn] [datetime] NOT NULL,
	             CONSTRAINT [PK_Log4Net] PRIMARY KEY CLUSTERED 
	            (
		            [ID] ASC
	            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	            ALTER TABLE [dbo].[Log4Net] ADD  CONSTRAINT [DF_Log4Net_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
            END
            IF NOT EXISTS (SELECT
	            *
            FROM sys.objects o
            WHERE o.NAME = 'Log4Email' AND o.TYPE = 'U')
            BEGIN

	            CREATE TABLE [dbo].[Log4Email](
		            [ID] [int] IDENTITY(1,1) NOT NULL,
		            [Thread] [nvarchar](1024) NOT NULL,
		            [Level] [nvarchar](1024) NOT NULL,
		            [Logger] [nvarchar](1024) NOT NULL,
		            [Source] [nvarchar](1024) NOT NULL,
		            [Date] [datetime] NOT NULL,
		            [From] [nvarchar](1024) NOT NULL,
		            [To] [nvarchar](1024) NOT NULL,
		            [Subject] [nvarchar](1024) NOT NULL,
		            [Body] [nvarchar](max) NOT NULL,
		            [CreatedOn] [datetime] NOT NULL,
	             CONSTRAINT [PK_Log4Email] PRIMARY KEY CLUSTERED 
	            (
		            [ID] ASC
	            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	            ALTER TABLE [dbo].[Log4Email] ADD  CONSTRAINT [DF_Log4Email_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
            END

            IF NOT EXISTS (SELECT
	            *
            FROM sys.objects o
            WHERE o.NAME = 'Log4Wechat' AND o.TYPE = 'U')
            BEGIN

	            CREATE TABLE [dbo].[Log4Wechat](
		            [ID] [int] IDENTITY(1,1) NOT NULL,
		            [Thread] [nvarchar](1024) NOT NULL,
		            [Level] [nvarchar](1024) NOT NULL,
		            [Logger] [nvarchar](1024) NOT NULL,
		            [Source] [nvarchar](1024) NOT NULL,
		            [Date] [datetime] NOT NULL,
		            [MsgId] [bigint] NOT NULL,
		            [ToUserName] [nvarchar](max) NOT NULL,
		            [FromUserName] [nvarchar](max) NOT NULL,
		            [MsgType] [nvarchar](max) NOT NULL,
		            [Content] [nvarchar](max) NOT NULL,
		            [CreateTime] [datetime] NOT NULL,
		            [ContentXML] [nvarchar](max) NOT NULL,
		            [CreatedOn] [datetime] NOT NULL,
		            CONSTRAINT [PK_Log4Wechat] PRIMARY KEY CLUSTERED 
	            (
		            [ID] ASC
	            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	            ALTER TABLE [dbo].[Log4Wechat] ADD  CONSTRAINT [DF_Log4Wechat_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
            END


            IF NOT EXISTS (SELECT
	            *
            FROM sys.objects o
            WHERE o.NAME = 'Log4WebService' AND o.TYPE = 'U')
            BEGIN
                CREATE TABLE [dbo].[Log4WebService](
	                [ID] [int] IDENTITY(1,1) NOT NULL,
	                [Thread] [nvarchar](1024) NOT NULL,
	                [Level] [nvarchar](1024) NOT NULL,
	                [Logger] [nvarchar](1024) NOT NULL,
	                [Source] [nvarchar](1024) NOT NULL,
	                [Date] [datetime] NOT NULL,
	                [MsgId] [bigint] NOT NULL,
	                [URL] [nvarchar](max) NOT NULL,
	                [Name] [nvarchar](max) NOT NULL,
	                [Content] [nvarchar](max) NOT NULL,
	                [Result] [nvarchar](max) NOT NULL,
	                [CreatedOn] [datetime] NOT NULL,
                 CONSTRAINT [PK_Log4WebService] PRIMARY KEY CLUSTERED 
                (
	                [ID] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                ALTER TABLE [dbo].[Log4WebService] ADD  CONSTRAINT [DF_Log4WebService_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
            END

            ";
                var ConnectionString = ApplicationEngine.Current.Default.ResolveNamed<string>("ConnectionString");
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = commandText;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
