using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Services.Description;
using System.Windows.Forms;
using System.Xml.Serialization;
using Binding = System.Web.Services.Description.Binding;
using ServicesMessage = System.Web.Services.Description.Message;
using System.Xml;
using System.Xml.Schema;

namespace OpenData.Utility
{
    public class OperationInputOutputParams
    {
        public string ClassName { set; get; }
        public string MethodName { set; get; }
        public string ReturnType { set; get; }
        public Dictionary<string, string> Parameters { set; get; }
    }
    public class WSDLParser
    {
        BindingCollection bindColl;
        PortTypeCollection portTypColl;
        MessageCollection msgColl;
        Types typs; 
        private XmlSchemas schemas;
        public List<OperationInputOutputParams> returnOperationsParameters(ServiceDescription serviceDescription)
        {
            List<OperationInputOutputParams> lst = new List<OperationInputOutputParams>();

            foreach (Service ser in serviceDescription.Services)
            {
                string webServiceNmae = ser.Name.ToString();
                foreach (Port port in ser.Ports)
                {
                    string portName = port.Name;
                    string binding = port.Binding.Name;
                    Binding bind = bindColl[binding];

                    PortType portTyp = portTypColl[bind.Type.Name];

                    foreach (Operation op in portTyp.Operations)
                    {
                        OperationMessageCollection opMsgColl = op.Messages;
                        OperationInput opInput = opMsgColl.Input;
                        OperationOutput opOutput = opMsgColl.Output;
                        string inputMsg = opInput.Message.Name;
                        string outputMsg = opOutput.Message.Name;

                        ServicesMessage msgInput = msgColl[inputMsg];
                        Dictionary<string, string> InputParam = parseMessageAndReturnParameters(msgInput);

                        ServicesMessage msgOutput = msgColl[outputMsg];
                        Dictionary<string, string> OutputParams = parseMessageAndReturnParameters(msgOutput);

                        string operationName = op.Name;
                        OperationInputOutputParams operObj = new OperationInputOutputParams();
                        operObj.ClassName = webServiceNmae;
                        operObj.MethodName = operationName;
                        operObj.Parameters = InputParam;
                        operObj.ReturnType = OutputParams.Values.ToArray<string>()[0];
                        lst.Add(operObj);
                    }
                }
            }
            return lst;
        }
        public Dictionary<string, string> parseMessageAndReturnParameters(ServicesMessage msg)
        {
            Dictionary<string, string> ParameterAndType = new Dictionary<string, string>();
            foreach (MessagePart msgpart in msg.Parts)
            {

                XmlQualifiedName typName = msgpart.Element;

                XmlSchemaElement lookup = (XmlSchemaElement)schemas.Find(typName, typeof(XmlSchemaElement));

                XmlSchemaComplexType tt = (XmlSchemaComplexType)lookup.SchemaType;

                XmlSchemaSequence sequence = (XmlSchemaSequence)tt.Particle;
                if (sequence != null)
                {
                    foreach (XmlSchemaElement childElement in sequence.Items)
                    {
                        ParameterAndType.Add(childElement.Name, childElement.SchemaTypeName.Name);
                        //Console.WriteLine("Element: {0} ,{1}", childElement.Name,childElement.SchemaTypeName.Name);
                    }
                }
            }

            return ParameterAndType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service">包括服务的所有终结点及其各自地址、绑定、协定和行为的规范</param>
        public WSDLParser(ServiceDescription service)
        {
            if (service.Name == string.Empty)
            {
                service.Name = service.RetrievalUrl;
            }
            if (service.Name == string.Empty)
            {
                service.Name = service.TargetNamespace;
            }
            bindColl = service.Bindings;
            portTypColl = service.PortTypes;
            msgColl = service.Messages;
            typs = service.Types;
            schemas = typs.Schemas;
            var list = this.returnOperationsParameters(service);

            //ServiceNode = new TreeNode(service.Name);
            //foreach (var s in list.Select(m => m.ClassName).Distinct().ToList())
            //{
            //    var serviceNode = this.ServiceNode.Nodes.Add(s);
            //    foreach (var item in list.Where(m => m.ClassName == s).ToList())
            //    {
            //        var node = serviceNode.Nodes.Add(item.MethodName);
            //        foreach (var p in item.Parameters)
            //        {
            //            node.Nodes.Add(p.Key + ":" + p.Value);
            //        }
            //    }

            //}
            //ServiceNode.Expand();
        }

        private string GetProtocol(Binding binding)
        {
            if (binding.Extensions.Find(typeof(SoapBinding)) != null) return "Soap";
            HttpBinding hb = (HttpBinding)binding.Extensions.Find(typeof(HttpBinding));
            if (hb == null) return "";
            if (hb.Verb == "POST") return "HttpPost";
            if (hb.Verb == "GET") return "HttpGet";
            return "";
        }
        void GetOperationFormat(OperationBinding obin, out SoapBindingStyle style, out SoapBindingUse inputUse, out SoapBindingUse outputUse)
        {
            style = SoapBindingStyle.Document;
            inputUse = SoapBindingUse.Literal;
            outputUse = SoapBindingUse.Literal;
            if (obin.Extensions != null)
            {
                SoapOperationBinding sob = obin.Extensions.Find(typeof(SoapOperationBinding)) as SoapOperationBinding;
                if (sob != null)
                {
                    style = sob.Style;
                    if (obin.Input != null)
                    {
                        SoapBodyBinding sbb0 = obin.Input.Extensions.Find(typeof(SoapBodyBinding)) as SoapBodyBinding;
                        if (sbb0 != null)
                            inputUse = sbb0.Use;
                    }

                    if (obin.Output != null)
                    {
                        SoapBodyBinding sbb1 = obin.Output.Extensions.Find(typeof(SoapBodyBinding)) as SoapBodyBinding;
                        if (sbb1 != null)
                            outputUse = sbb1.Use;
                    }
                }
            }
        }
    }
}