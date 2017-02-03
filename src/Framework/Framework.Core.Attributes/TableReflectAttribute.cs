using System;

namespace Framework.Core.Attributes
{   

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class TableReflectAttribute : Attribute
    {
        private string _tableName;

        public string TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }
    }   
}

