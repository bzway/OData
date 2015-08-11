using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OpenData.Framework.Script
{
    public interface IJsHost
    {
        dynamic parameter { get; set; }
        object Execute();
        Action BeforeAction { get; set; }
        Action EndAction { get; set; }
    }
    public class JavascriptExecuter
    {

        static Dictionary<int, IJsHost> dict = new Dictionary<int, IJsHost>();
        static public string Exec(string jsCode, dynamic parameter)
        {
            var instance = GetJsHost(jsCode);
            if (instance == null)
            {
                return "动态编译出错!";
            }
            //调用方法
            instance.parameter = parameter;
            var a = instance.Execute();
            return a.ToString();
        }
        private static IJsHost GetJsHost(string jsCode)
        {

            var hash = jsCode.GetHashCode();
            if (dict.ContainsKey(hash))
            {

                return dict[hash];
            }

            CSharpCodeProvider provider = new CSharpCodeProvider();
            //表示用于调用编译器的参数
            CompilerParameters parameter = new CompilerParameters();
            //获取当前项目所引用的程序集。
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.Core.dll");
            parameter.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameter.ReferencedAssemblies.Add(@"D:\Work\OpenData\OpenData.Framework\bin\Debug\OpenData.Framework.dll");

            //获取或设置一个值，该值指示是否生成可执行文件。
            parameter.GenerateExecutable = false;
            //获取或设置一个值，该值指示是否在内存中生成输出。
            parameter.GenerateInMemory = true;

            var sourceCode = File.ReadAllText(@"D:\Work\OpenData\OpenData.Framework\bin\Debug\Script\Template.cs");
            sourceCode = sourceCode.Replace("//todo", jsCode);
            //使用指定的编译器设置，从包含源代码的字符串的指定数组，编译程序集。返回编译结果。
            CompilerResults result = provider.CompileAssemblyFromSource(parameter, sourceCode);//将你的式子放在这里
            //获取编译器错误和警告的集合
            if (result.Errors.Count > 0)
            {
                return null;
            }

            //获取或设置已编译的程序集。
            Assembly assembly = result.CompiledAssembly;
            //获取当前实例的 Type
            var instance = (IJsHost)assembly.CreateInstance("OpenData.Framework.Script.JsHost");
            if (instance != null)
            {
                dict.Add(hash, instance);
            }
            return instance;
        }
    }
}