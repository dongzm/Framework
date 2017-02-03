using Framework.Core.Attributes;
using Framework.Core.Attributes.Support;
using Framework.Core.Dal.BaseClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;


namespace Framework.Core.Proc
{


    public class BaseProcOra
    {
        private ProcModelAttributes _modelAttributes;

        protected virtual List<V> ConvertToCollection<V>(DataSet ds) where V: BaseProcedureResult
        {
            List<V> list = new List<V>();
            if ((ds != null) && (ds.Tables[0].Rows.Count != 0))
            {
                this.LoadModelAttributes<V>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    V local = Activator.CreateInstance<V>();
                    Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
                    foreach (PropertyInfo info in propertyCloumnReflectAttr.Keys)
                    {
                        CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                        string cloumnName = attribute.CloumnName;
                        if (row[cloumnName].ToString() != "")
                        {
                            info.SetValue(local, row[cloumnName], null);
                        }
                    }
                    list.Add(local);
                }
            }
            return list;
        }

        protected virtual OracleParameter[] GetCondition<T>(T condition) where T: BaseProcedureCondition
        {
            DBParameterProviderOra ora = new DBParameterProviderOra();
            PropertyInfo[] properties = condition.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                object param = info.GetValue(condition, null);
                if (!(!this.isParamValid(param) || GetCustomAttributes.IsUnParamedConditionAttribute(info)))
                {
                    ora.Create(info.Name.ToLower(), param);
                }
            }
            return ora.ToArray();
        }

        protected virtual OracleParameter[] GetCondition<T, V>(T condition) where T: BaseProcedureCondition where V: BaseProcedureResult
        {
            DBParameterProviderOra ora = new DBParameterProviderOra();
            ora.AddRange(this.GetCondition<T>(condition));
            this.LoadModelAttributes<V>();
            if (this._modelAttributes.OutCursor != null)
            {
                ora.Create(this._modelAttributes.OutCursor.CursorName, OracleType.Cursor, null).Direction = ParameterDirection.Output;
            }
            return ora.ToArray();
        }

        protected bool isParamValid(object param)
        {
            return (null != param);
        }

        private void LoadModelAttributes<V>() where V: BaseProcedureResult
        {
            if (DataSchema.Instance.ModelReflectDictionary != null)
            {
                this._modelAttributes = DataSchema.Instance.ProcModelDictionary[typeof(V)];
            }
        }

        public List<V> RunProcedure<T, V>(T condition) where T: BaseProcedureCondition where V: BaseProcedureResult
        {
            DataSet ds = DbHelperOra.RunProcedure(condition.ProcName, this.GetCondition<T, V>(condition), "TableName");
            return this.ConvertToCollection<V>(ds);
        }

        public void RunProcedure<T>(T condition) where T: BaseProcedureCondition
        {
            DataSet set = DbHelperOra.RunProcedure(condition.ProcName, this.GetCondition<T>(condition), "TableName");
        }

        public void RunProcedure(string name)
        {
            DataSet set = DbHelperOra.RunProcedure(name, null, "TableName");
        }
    }
}

