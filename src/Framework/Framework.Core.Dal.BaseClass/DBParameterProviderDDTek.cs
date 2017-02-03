using DDTek.Oracle;
using Framework.Core.Dal.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Dal.BaseClass
{
    public class DBParameterProviderDDTek : DBParameterProvider<OracleParameter, OracleDbType>
    {

        public override OracleParameter Create(string parameterName, object paramterValue)
        {
            OracleParameter item = new OracleParameter
            {
                ParameterName = parameterName,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create<T>(string parameterName, T paramterValue)
        {
            OracleParameter item = new OracleParameter
            {
                ParameterName = parameterName,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create(string parameterName, OracleDbType paramterType, object paramterValue)
        {
            OracleParameter item = new OracleParameter
            {
                ParameterName = parameterName,
                OracleDbType = paramterType,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create<T>(string parameterName, OracleDbType paramterType, T paramterValue)
        {
            OracleParameter item = new OracleParameter
            {
                ParameterName = parameterName,
                OracleDbType = paramterType,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }

        public override OracleParameter Create<T>(string parameterName, OracleDbType paramterType, int paramterSize, T paramterValue)
        {
            OracleParameter item = new OracleParameter
            {
                ParameterName = parameterName,
                OracleDbType = paramterType,
                Size = paramterSize,
                Value = paramterValue
            };
            base.Add(item);
            return item;
        }       
    }
}
