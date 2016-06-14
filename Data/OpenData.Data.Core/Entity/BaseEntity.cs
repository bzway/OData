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
    /// ����һ������ʽ�����ܺ�ǿ���Ժ�ᵥ��ƪ�½���formula�÷�
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
    /// ����һ��Date���ͣ��û���ǰ̨�󶨺����ֱ��ʹ��Date������Ӧ�Ŀؼ�
    /// </summary>
    public class Date { }

    /// <summary>
    /// ����һ��Date��Time���ͣ��û�ѡ�����ں����ں͵�ǰʱ��㸳ֵ��������
    /// </summary>
    public class Time { }
    /// <summary>
    /// ����һ��Email����
    /// </summary>
    public class Email { }
    public class Url { }
    /// <summary>
    /// ����һ��λ�õ����ͣ������Ͱ�����γ����Ϣ
    /// </summary>
    public class Geolocation
    { }

    /// <summary>
    /// ϵͳ���ɵ����кţ�ͨ�����������ʽ��ʾ��Ϊÿ���¼�¼�Զ�������
    /// </summary>
    public class AutoNumber { }

    //3.Lookup Relationship:��������һ���������һ������Ĺ�ϵ��������ϵ��ͨ��һ��������Է�����һ�������������Ϣ��

    //4.Master-Detail Relationship:����һ������ĸ��ӹ�ϵ�����ӹ�ϵ������lookup Relationship ����ͬ�������������ܣ�

    //5.External Lookup Relationship:����һ���������һ���������Ĺ�ϵ�����������������ݴ洢�ڶ�����������Դ�У�
    /// <summary>
    /// ����һ����������
    /// </summary>
    public class Checkbox { }
    /// <summary>
    /// ����һ����������
    /// </summary>
    public class Currency { }
    /// <summary>
    /// ����һ���ٷֱ�����
    /// </summary>
    public class Percent { }

    /// <summary>
    /// ����һ���ֻ��������ͣ�����������Զ�ת���ɴ�����
    /// </summary>
    public class Phone { }
    /// <summary>
    /// ����һ���б����ͣ�������HTML�е�<select><option></option></select>��ϵ������������ӽ���
    /// </summary>
    public class Picklist { }
    /// <summary>
    /// ����һ���ַ������ͣ���󳤶�Ϊ255
    /// </summary>
    public class Text { }
    /// <summary>
    /// ��Text�������ƣ�����Ϊ���ݿ��Ի���
    /// </summary>
    public class TextArea
    { }
}