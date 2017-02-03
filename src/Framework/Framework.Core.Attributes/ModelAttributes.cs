using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Core.Attributes
{
    public class ModelAttributes
    {
        private Dictionary<PropertyInfo, CloumnReflectAttribute> _propertyCloumnReflectAttr;
        private TableReflectAttribute _tableReflect;

        public Dictionary<PropertyInfo, CloumnReflectAttribute> PropertyCloumnReflectAttr
        {
            get
            {
                return this._propertyCloumnReflectAttr;
            }
            set
            {
                this._propertyCloumnReflectAttr = value;
            }
        }

        public TableReflectAttribute TableReflect
        {
            get
            {
                return this._tableReflect;
            }
            set
            {
                this._tableReflect = value;
            }
        }
    }
}

