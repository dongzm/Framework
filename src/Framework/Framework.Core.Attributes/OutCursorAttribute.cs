using System;

namespace Framework.Core.Attributes
{


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class OutCursorAttribute : Attribute
    {
        private string _cursorName;

        public OutCursorAttribute(string cursorName)
        {
            this.CursorName = cursorName;
        }

        public string CursorName
        {
            get
            {
                return this._cursorName;
            }
            set
            {
                this._cursorName = value;
            }
        }
    }
}

