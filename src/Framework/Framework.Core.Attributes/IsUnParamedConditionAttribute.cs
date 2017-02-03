using System;


namespace Framework.Core.Attributes
{   

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class IsUnParamedConditionAttribute : Attribute
    {
    }
}

