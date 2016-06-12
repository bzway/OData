using System.Reflection;
using log4net.Layout;
using log4net.Layout.Pattern;

namespace OpenData.Log
{
    public class PropertyLayout : RawPropertyLayout
    {
        string property = string.Empty;
        public PropertyLayout(string p)
        {
            this.property = p;
        }
        public override object Format(log4net.Core.LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty;

            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            }
            return propertyValue;
        }

    }
    //public class PropertyLayout : PatternLayout
    //{
    //    public PropertyLayout()
    //    {
    //        this.AddConverter("property", typeof(PropertyPatternConverter));

    //    }
    //    public override void Format(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    //    {

    //        base.Format(writer, loggingEvent);
    //    }
    //}

    //public class PropertyPatternConverter : PatternLayoutConverter
    //{
    //    protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
    //    {
    //        if (Option != null)
    //        {
    //            // Write the value for the specified key
    //            WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
    //        }
    //        else
    //        {
    //            // Write all the key value pairs
    //            WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
    //        }
    //    }
    //    public override void Format(System.IO.TextWriter writer, object state)
    //    {
    //        base.Format(writer, state);
    //    }
    //    protected override void Convert(System.IO.TextWriter writer, object state)
    //    {
    //        base.Convert(writer, state);
    //    }
    //    public override log4net.Util.FormattingInfo FormattingInfo
    //    {
    //        get
    //        {
    //            return base.FormattingInfo;
    //        }
    //        set
    //        {
    //            base.FormattingInfo = value;
    //        }
    //    }
    //    public override string Option
    //    {
    //        get
    //        {
    //            return base.Option;
    //        }
    //        set
    //        {
    //            base.Option = value;
    //        }
    //    }
    //    public override bool IgnoresException
    //    {
    //        get
    //        {
    //                return base.IgnoresException;
    //        }
    //        set
    //        {
    //            base.IgnoresException = value;
    //        }
    //    }
    //    public override log4net.Util.PatternConverter SetNext(log4net.Util.PatternConverter patternConverter)
    //    {
    //        return base.SetNext(patternConverter);
    //    }
    //    public override log4net.Util.PatternConverter Next
    //    {
    //        get
    //        {
    //            return base.Next;
    //        }
    //    }
    //    /// <summary>
    //    /// 通过反射获取传入的日志对象的某个属性的值
    //    /// </summary>
    //    /// <param name="property"></param>
    //    /// <returns></returns>
    //    private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
    //    {
    //        object propertyValue = string.Empty;

    //        PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
    //        if (propertyInfo != null)
    //        {
    //            propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
    //        }
    //        return propertyValue;
    //    }
    //}
}