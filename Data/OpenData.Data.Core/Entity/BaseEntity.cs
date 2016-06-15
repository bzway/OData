using OpenData.Data.Core;
using System;
using System.Reflection;
using System.Text;
namespace OpenData.Data.Core
{
    public class BaseEntity : IEntity
    {

        public virtual string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public int Status { get; set; }
        //public virtual string EntityName { get; set; }
        //public virtual bool EnableVersion { get; set; }
        //public virtual bool HasWorkflow { get; set; }
    }

    public interface IEntity : IEntity<string>
    { }
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime UpdatedOn { get; set; }

        TKey CreatedBy { get; set; }

        TKey UpdatedBy { get; set; }
        int Status { get; set; }
        //public virtual string EntityName { get; set; }
        //public virtual bool EnableVersion { get; set; }
        //public virtual bool HasWorkflow { get; set; }
    }




    public class Number
    {


    }
    /// <summary>
    /// 声明一个计算式，功能很强大，以后会单独篇章讲解formula用法
    /// </summary>
    public class Formula
    {

    }
    public class ChildRelationship
    { }
    public class MasterRelationship
    {
    }
    /// <summary>
    /// 声明一个Date类型，用户在前台绑定后可以直接使用Date类型相应的控件
    /// </summary>
    public class Date { }

    /// <summary>
    /// 声明一个Date和Time类型，用户选择日期后，日期和当前时间便赋值到输入域
    /// </summary>
    public class Time { }
    /// <summary>
    /// 声明一个Email类型
    /// </summary>
    public class Email { }
    public class Url { }
    /// <summary>
    /// 声明一个位置的类型，此类型包含经纬度信息
    /// </summary>
    public class Geolocation
    { }

    /// <summary>
    /// 系统生成的序列号，通过自身定义的形式显示，为每条新纪录自动递增数
    /// </summary>
    public class AutoNumber { }

    //3.Lookup Relationship:创建链接一个对象和另一个对象的关系，创建关系后，通过一个对象可以访问另一个对象的内容信息；

    //4.Master-Detail Relationship:创建一个特殊的父子关系（主从关系），和lookup Relationship 的相同与差异在下面介绍；

    //5.External Lookup Relationship:创建一个对象和另一个额外对象的关系。其中这个对象的数据存储在额外对象的数据源中；
    /// <summary>
    /// 声明一个布尔类型
    /// </summary>
    public class Checkbox { }
    /// <summary>
    /// 声明一个货币类型
    /// </summary>
    public class Currency { }
    /// <summary>
    /// 声明一个百分比类型
    /// </summary>
    public class Percent { }

    /// <summary>
    /// 声明一个手机号码类型，输入的内容自动转换成此类型
    /// </summary>
    public class Phone { }
    /// <summary>
    /// 声明一个列表类型，类似于HTML中的<select><option></option></select>关系，下面会有例子讲解
    /// </summary>
    public class Picklist { }
    /// <summary>
    /// 声明一个字符串类型，最大长度为255
    /// </summary>
    public class Text { }
    /// <summary>
    /// 和Text类型相似，区别为内容可以换行
    /// </summary>
    public class TextArea
    { }
}