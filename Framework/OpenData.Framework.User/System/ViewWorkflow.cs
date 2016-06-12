using Bzway.Data;
using System;

namespace Bzway.Business.Model
{
    public class ViewWorkflow : IViewBase
    {
        public string EntityID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class ViewWorkflowState : IViewBase
    {
        public string WorkflowID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
    public class Transition : IViewBase
    {
        public string WorkflowID { get; set; }
        public string Name { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public string RoleList { get; set; }
    }
    
    public class Action : IViewBase
    {
        public string TransitionID { get; set; }
        public string Name { get; set; }
        public ActionType Type { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
    }
    public class ActionDetail : IViewBase
    {
        public string ActionID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

}