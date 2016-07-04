using OpenData.Data.Core;
namespace OpenData.Framework.Core.Entity
{
    public class  Workflow : BaseEntity 
    {
        public string EntityID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class  WorkflowState : BaseEntity
    {
        public string WorkflowID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
    public class Transition : BaseEntity
    {
        public string WorkflowID { get; set; }
        public string Name { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public string RoleList { get; set; }
    }
    
    public class Action : BaseEntity
    {
        public string TransitionID { get; set; }
        public string Name { get; set; }
        public ActionType Type { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
    }
    public class ActionDetail : BaseEntity
    {
        public string ActionID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

}