﻿using OpenData.Data.Core;
using System;

namespace OpenData.Framework.Core.Entity
{

    public class WechatArticleRead : BaseEntity
    {

        public string OfficialAccount { get; set; }
        public DateTime RefDateTime { get; set; }
        public int PageReadUser { get; set; }
        public int PageReadCount { get; set; }
        public int OriginalPageReadUser { get; set; }
        public int OriginalPageReadCount { get; set; }
        public int PageShareUser { get; set; }
        public int PageShareCount { get; set; }
        public int PageFavriateUser { get; set; }
        public int PageFavriateCount { get; set; }


    }
}