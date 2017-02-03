using System;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CloumnReflectAttribute : Attribute
    {
        private string _cloumnName;
        private object _dbType;
        private bool _isNullable = true;
        private bool _isPrimaryKey = false;
        private int _maxLength;
        private bool _isIgnoreInTable;
        private bool _isSeq;
        private string _seqName;

        public string CloumnName
        {
            get
            {
                return this._cloumnName;
            }
            set
            {
                this._cloumnName = value;
            }
        }

        public object DbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                this._dbType = value;
            }
        }

        public bool IsNullable
        {
            get
            {
                return this._isNullable;
            }
            set
            {
                this._isNullable = value;
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return this._isPrimaryKey;
            }
            set
            {
                this._isPrimaryKey = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }

        public bool IsIgnoreInTable
        {
            get
            {
                return this._isIgnoreInTable;
            }
            set
            {
                this._isIgnoreInTable = value;
            }
        }

        public bool IsSeq
        {
            get
            {
                return this._isSeq;
            }
            set
            {
                this._isSeq = value;
            }
        }

        public string SeqName
        {
            get
            {
                return this._seqName;
            }
            set
            {
                this._seqName = value;
            }
        }
    }
}

