using System;
using System.Collections.Generic;
using System.Data.Common;
namespace Framework.Core.Dal.BaseClass
{
    public abstract class DBParameterProvider<V, W> : List<V> where V: DbParameter
    {
        protected DBParameterProvider()
        {
        }

        public abstract V Create<T>(string parameterName, T paramterValue);
        public abstract V Create(string parameterName, object paramterValue);
        public abstract V Create<T>(string parameterName, W paramterType, T paramterValue);
        public abstract V Create(string parameterName, W paramterType, object paramterValue);
        public abstract V Create<T>(string parameterName, W paramterType, int paramterSize, T paramterValue);
    }
}

