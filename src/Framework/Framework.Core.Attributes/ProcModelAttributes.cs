using System;
using System.Collections.Generic;
using System.Reflection;


namespace Framework.Core.Attributes
{
    public class ProcModelAttributes
    {
        private OutCursorAttribute _outCursor;
        private Dictionary<PropertyInfo, CloumnReflectAttribute> _propertyCloumnReflectAttr;

        public OutCursorAttribute OutCursor
        {
            get
            {
                return this._outCursor;
            }
            set
            {
                this._outCursor = value;
            }
        }

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
    }
}

