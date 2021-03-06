﻿#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OpenData.Framework.Common
{
    public class JsonTextResult : ContentResult
    {
        public JsonTextResult(object data)
        {
            ContentType = "text/plain";
            Content = new JavaScriptSerializer().Serialize(data);
        }
    }


    //extension methods for the controller to allow jsonp.
    public static class JsonTextResultContollerExtensions
    {
        public static JsonTextResult JsonText(this Controller controller, object data)
        {
            JsonTextResult result = new JsonTextResult(data);
            result.ExecuteResult(controller.ControllerContext);
            return result;
        }
    }
}
