using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Service.Wechat.Models
{
    /// <summary>
    /// 上传媒体文件返回结果
    /// </summary>
    public class UploadMediaFileResult
    { 
        public string media_id { get; set; }
        public long created_at { get; set; }
    }
}
