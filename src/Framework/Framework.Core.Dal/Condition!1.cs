using Framework.Core.Model;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Framework.Core.Dal
{

    public class Condition<V> where V: BaseModel
    {
        //等于
        private Dictionary<string, object> _equals;
        //大于
        private Dictionary<string, object> _greaterthan;
        //大于等于
        private Dictionary<string, object> _greaterthanAndEquals;
        //in
        private Dictionary<string, ArrayList> _in;
        private Dictionary<string, object> _lessthan;
        private Dictionary<string, object> _lessthanAndEquals;
        private Dictionary<string, object> _like;        
        private Dictionary<string, object> _notEquals;
        private Dictionary<string, ArrayList> _notIn;
        private Dictionary<string, object> _notLike;
        private V _model;

        public Condition()
        {
            this._greaterthan = new Dictionary<string, object>();
            this._lessthan = new Dictionary<string, object>();
            this._equals = new Dictionary<string, object>();
            this._notEquals = new Dictionary<string, object>();
            this._greaterthanAndEquals = new Dictionary<string, object>();
            this._lessthanAndEquals = new Dictionary<string, object>();
            this._like = new Dictionary<string, object>();
            this._notLike = new Dictionary<string, object>();
            this._in = new Dictionary<string, ArrayList>();
            this._notIn = new Dictionary<string, ArrayList>();
        }

        public Condition(V model)
        {
            this._greaterthan = new Dictionary<string, object>();
            this._lessthan = new Dictionary<string, object>();
            this._equals = new Dictionary<string, object>();
            this._notEquals = new Dictionary<string, object>();
            this._greaterthanAndEquals = new Dictionary<string, object>();
            this._lessthanAndEquals = new Dictionary<string, object>();
            this._like = new Dictionary<string, object>();
            this._notLike = new Dictionary<string, object>();
            this._in = new Dictionary<string, ArrayList>();
            this._notIn = new Dictionary<string, ArrayList>();
            this._model = model;
        }

        public void SetEquals<T>(string columnName, T value)
        {
            this._equals.Add(columnName, value);
        }

        public void SetGreaterThan<T>(string columnName, T value)
        {
            this._greaterthan.Add(columnName, value);
        }

        public void SetGreaterThanAndEquals<T>(string columnName, T value)
        {
            this._greaterthanAndEquals.Add(columnName, value);
        }

        public void SetIn(string columnName, ArrayList value)
        {
            this._in.Add(columnName, value);
        }

        public void SetLessThan<T>(string columnName, T value)
        {
            this._lessthan.Add(columnName, value);
        }

        public void SetLessThanAndEquals<T>(string columnName, T value)
        {
            this._lessthanAndEquals.Add(columnName, value);
        }

        public void SetLike(string columnName, string value)
        {
            this._like.Add(columnName, value);
        }

        public void SetNotEquals<T>(string columnName, T value)
        {
            this._notEquals.Add(columnName, value);
        }

        public void SetNotIn(string columnName, ArrayList value)
        {
            this._notIn.Add(columnName, value);
        }

        public void SetNotLike(string columnName, string value)
        {
            this._notLike.Add(columnName, value);
        }

        public Dictionary<string, object> Equals
        {
            get
            {
                return this._equals;
            }
        }

        public Dictionary<string, object> Greaterthan
        {
            get
            {
                return this._greaterthan;
            }
        }

        public Dictionary<string, object> GreaterthanAndEquals
        {
            get
            {
                return this._greaterthanAndEquals;
            }
        }

        public Dictionary<string, ArrayList> In
        {
            get
            {
                return this._in;
            }
        }

        public Dictionary<string, object> Lessthan
        {
            get
            {
                return this._lessthan;
            }
        }

        public Dictionary<string, object> LessthanAndEquals
        {
            get
            {
                return this._lessthanAndEquals;
            }
        }

        public Dictionary<string, object> Like
        {
            get
            {
                return this._like;
            }
        }

        public V Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._model = value;
            }
        }

        public Dictionary<string, object> NotEquals
        {
            get
            {
                return this._notEquals;
            }
        }

        public Dictionary<string, ArrayList> NotIn
        {
            get
            {
                return this._notIn;
            }
        }

        public Dictionary<string, object> NotLike
        {
            get
            {
                return this._notLike;
            }
        }
    }
}

