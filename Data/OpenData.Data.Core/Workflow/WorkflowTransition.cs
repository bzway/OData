using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Data
{
    public partial class WorkflowTransition : DynamicEntity
    {
        public Workflow Workflow
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public override bool EnableVersion
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public override bool HasWorkflow
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        public override string Id
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
    }

    public partial class WorkflowTransition : DynamicEntity
    {
        /*
        System.String WorkflowID, 
        System.String Name, 
        System.String FromState, 
        System.String ToState, 
        System.String RoleList, 
        */
        public System.String WorkflowID
        {
            get
            {
                if (this.ContainsKey("WorkflowID") && this["WorkflowID"] != null)
                {
                    return this["WorkflowID"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["WorkflowID"] = value;
            }
        }
        public System.String Name
        {
            get
            {
                if (this.ContainsKey("Name") && this["Name"] != null)
                {
                    return this["Name"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Name"] = value;
            }
        }
        public System.String FromState
        {
            get
            {
                if (this.ContainsKey("FromState") && this["FromState"] != null)
                {
                    return this["FromState"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["FromState"] = value;
            }
        }
        public System.String ToState
        {
            get
            {
                if (this.ContainsKey("ToState") && this["ToState"] != null)
                {
                    return this["ToState"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["ToState"] = value;
            }
        }
        public System.String RoleList
        {
            get
            {
                if (this.ContainsKey("RoleList") && this["RoleList"] != null)
                {
                    return this["RoleList"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["RoleList"] = value;
            }
        }

       
    }

 }