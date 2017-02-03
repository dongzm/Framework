namespace Framework.Core.Dal.BaseClass
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class DBParameterProviderSQL : DBParameterProvider<SqlParameter, SqlDbType>
    {
        public override SqlParameter Create(string parameterName, object paramterValue)
        {
            SqlParameter item = new SqlParameter {
                ParameterName = parameterName,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override SqlParameter Create<T>(string parameterName, T paramterValue)
        {
            SqlParameter item = new SqlParameter {
                ParameterName = parameterName,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override SqlParameter Create(string parameterName, SqlDbType paramterType, object paramterValue)
        {
            SqlParameter item = new SqlParameter {
                ParameterName = parameterName,
                SqlDbType = paramterType,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override SqlParameter Create<T>(string parameterName, SqlDbType paramterType, T paramterValue)
        {
            SqlParameter item = new SqlParameter {
                ParameterName = parameterName,
                SqlDbType = paramterType,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override SqlParameter Create<T>(string parameterName, SqlDbType paramterType, int paramterSize, T paramterValue)
        {
            SqlParameter item = new SqlParameter {
                ParameterName = parameterName,
                SqlDbType = paramterType,
                Size = paramterSize,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }
    }
}

