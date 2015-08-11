namespace OpenData.Framework.Script
{
    using System;
    using OpenData.Framework.Script;
    public class JsHost : IJsHost
    {
        public object Execute()
        {
            try
            {
                //todo
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Action BeforeAction { get; set; }
        public Action EndAction { get; set; }
        public dynamic parameter { get; set; }


    }
}