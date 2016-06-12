using System;
using System.Reflection;

namespace OpenData.Utility
{
    public class WebServiceHelper2
    {
        [AttributeUsage(AttributeTargets.Class)]
        class WebServiceDataAttribute : Attribute
        {

            public static bool IsWebServiceData(System.Type type)
            {
                if (type.GetCustomAttributes(typeof(WebServiceDataAttribute), false).Length == 0)
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Created by Weijie
        /// </summary>
        /// <param name="webServiceType"></param>
        /// <param name="webMethodName"></param>
        /// <param name="returnObjectType"></param>
        /// <param name="webMethodParameters"></param>
        /// <returns></returns>
        public static object CallWebMethod(Type webServiceType, string webMethodName, Type returnObjectType, params object[] webMethodParameters)
        {
            //1. prepare input parameters for web method calling.

            object[] parameters = new object[webMethodParameters.Length];
            for (int i = 0; i < webMethodParameters.Length; i++)
            {

                //step over all input parameters
                if (WebServiceDataAttribute.IsWebServiceData(webMethodParameters[i].GetType()) == false)
                {
                    //the parameter is not a web service data.
                    parameters[i] = webMethodParameters[i];
                }
                else
                {
                    //parameter is tagged by WebServiceDataAttribute.
                    //create a instance in Web Service name space.
                    string wsFullName = webServiceType.FullName;
                    string wsNamespace = wsFullName.Substring(0, wsFullName.Length - wsFullName.LastIndexOf("."));
                    string fullName = webMethodParameters[i].GetType().FullName;
                    string name = fullName.Substring(fullName.LastIndexOf(".") + 1);

                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(webServiceType);
                    object parameter = assembly.CreateInstance(wsNamespace + "." + name);

                    //copy type
                    CopyFieldValue(parameter, webMethodParameters[i]);
                    parameters[i] = parameter;
                }
            }

            //2. web method calling
            object webServiceObject = System.Activator.CreateInstance(webServiceType);
            MethodInfo mi = webServiceType.GetMethod(webMethodName);

            object wsResult = mi.Invoke(webServiceObject, parameters);

            //3. prepare return value;
            if (WebServiceDataAttribute.IsWebServiceData(returnObjectType) == true)
            {
                object result = null;

                if (returnObjectType != null)
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(returnObjectType);

                    result = assembly.CreateInstance(returnObjectType.FullName);

                    CopyFieldValue(result, wsResult);
                }

                return result;
            }
            else
            {
                return wsResult;
            }
        }



        /// <summary>
        /// Copy field value in B to fields in A
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Source"></param>
        public static void CopyFieldValue(object target, object source)
        {
            Type sourceType = source.GetType();

            if (sourceType.IsValueType == true)
            {
                //for value type, set directly.
                target = source;
                return;
            }

            //for refrence type, use reflector.
            Type targetType = target.GetType();

            FieldInfo targetField = null;

            //step over source fields
            FieldInfo[] sourceFieldArray = sourceType.GetFields();
            for (int i = 0; i < sourceFieldArray.Length; i++)
            {
                targetField = targetType.GetField(sourceFieldArray[i].Name);

                //Target don't have this field 
                if (targetField == null)
                    continue;
                else
                {
                    if (targetField.FieldType.IsArray == true)
                    {
                        //for array, must prepare space manually and step over it to fill data
                        //get source and create target array.
                        System.Array sourceArray = sourceFieldArray[i].GetValue(source) as System.Array;

                        if (sourceArray != null)
                        {
                            //if source is not null, create target array
                            System.Array targetArray = System.Array.CreateInstance(targetField.FieldType.GetElementType(), sourceArray.Length);

                            for (int j = 0; j < sourceArray.Length; j++)
                            {
                                //create property instance as set value
                                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(targetField.FieldType.GetElementType());
                                object targetElement = assembly.CreateInstance(targetField.FieldType.GetElementType().FullName);
                                CopyFieldValue(targetElement, sourceArray.GetValue(j));
                                targetArray.SetValue(targetElement, j);
                            }
                            targetField.SetValue(target, targetArray);
                        }
                        else
                        {
                            targetField.SetValue(target, null);
                        }
                    }
                    else
                    {
                        //other member
                        if (targetField.FieldType.IsValueType == true)
                        {
                            //for value type data, set value directly.
                            targetField.SetValue(target, sourceFieldArray[i].GetValue(source));
                        }
                        else
                        {
                            //for reference type data, set value by function recursive.
                            object memberOfTarget, memberOfSource;

                            memberOfTarget = targetField.GetValue(target);
                            if (memberOfTarget == null)
                            {
                                //the member is not created yet.
                                //ignore the sub entity whether in same *.dll/or assembly
                                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(targetField.FieldType);
                                memberOfTarget = assembly.CreateInstance(targetField.FieldType.FullName);
                            }

                            memberOfSource = sourceFieldArray[i].GetValue(source);

                            CopyFieldValue(memberOfTarget, memberOfSource);

                            //set value of new member of target.
                            targetField.SetValue(target, memberOfTarget);
                        }
                    }
                }
            }
        }

    }
}
