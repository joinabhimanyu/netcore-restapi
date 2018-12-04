using System;
using System.Collections.Generic;
using System.Text;

namespace DRS.Model.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RegisterRepositoryAttribute : Attribute
    {
        public Type TargetType { get; set; }
    }
    public sealed class AttributeParams
    {
        public string PropName { get; set; }
        public object PropValue { get; set; }
    }
}
