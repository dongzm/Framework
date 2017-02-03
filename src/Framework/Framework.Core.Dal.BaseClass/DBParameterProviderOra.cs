namespace Framework.Core.Dal.BaseClass
{
    using System;
    using System.Data.OracleClient;

    public class DBParameterProviderOra : DBParameterProvider<OracleParameter, OracleType>
    {
        public override OracleParameter Create(string parameterName, object paramterValue)
        {
            OracleParameter item = new OracleParameter {
                ParameterName = parameterName,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create<T>(string parameterName, T paramterValue)
        {
            OracleParameter item = new OracleParameter {
                ParameterName = parameterName,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create(string parameterName, OracleType paramterType, object paramterValue)
        {
            OracleParameter item = new OracleParameter {
                ParameterName = parameterName,
                OracleType = paramterType,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create<T>(string parameterName, OracleType paramterType, T paramterValue)
        {
            OracleParameter item = new OracleParameter {
                ParameterName = parameterName,
                OracleType = paramterType,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create<T>(string parameterName, OracleType paramterType, int paramterSize, T paramterValue)
        {
            OracleParameter item = new OracleParameter {
                ParameterName = parameterName,
                OracleType = paramterType,
                Size = paramterSize,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }
    }
}

