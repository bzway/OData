using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace OpenData.RuleEngine
{
    public class Rule
    {
        public Rule()
        {
            Inputs = new List<object>();
        }
        public string MemberName { get; set; }
        public string Operator { get; set; }
        public string TargetValue { get; set; }
        public List<Rule> Rules { get; set; }
        public List<object> Inputs { get; set; }
    }
}