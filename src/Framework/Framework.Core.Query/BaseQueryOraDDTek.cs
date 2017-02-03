using DDTek.Oracle;
using Framework.Core.Attributes;
using Framework.Core.Attributes.Support;
using Framework.Core.Dal.BaseClass;
using Framework.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Core.Query
{
    public abstract class BaseQueryOraDDTek<T, V> where T : BaseQueryCondition where V : BaseQueryResult
    {
        private ModelAttributes _modelAttributes;

        public BaseQueryOraDDTek()
        {
            if (DataSchema.Instance.ModelReflectDictionary != null)
            {
                this._modelAttributes = DataSchema.Instance.ModelReflectDictionary[typeof(V)];
            }
        }

        protected virtual List<V> ConvertToCollection(DataSet ds)
        {
            List<V> list = new List<V>();
            if ((ds != null) && (ds.Tables[0].Rows.Count != 0))
            {
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

        private IList<string> GetArgs(string cmd)
        {
            IList<string> list = new List<string>();
            MatchCollection matchs = Regex.Matches(cmd.Trim(), @":(\w+)\b");
            foreach (Match match in matchs)
            {
                list.Add(match.Value.TrimStart(new char[] { ':' }).ToLower());
            }
            return list;
        }

        protected virtual OracleParameter[] GetCondition(string strCmd, T condition)
        {
            DBParameterProviderDDTek ora = new DBParameterProviderDDTek();
            IList<string> args = this.GetArgs(strCmd);
            PropertyInfo[] properties = condition.GetType().GetProperties();
            string item = string.Empty;
            if (args.Count > 0)
            {
                foreach (PropertyInfo info in properties)
                {
                    if (!GetCustomAttributes.IsUnParamedConditionAttribute(info))
                    {
                        item = info.Name.ToLower();
                        if (args.Contains(item))
                        {
                            object paramterValue = info.GetValue(condition, null);
                            if (paramterValue != null)
                            {
                                ora.Create(":" + item, paramterValue);
                            }
                        }
                    }
                }
            }
            return ora.ToArray();
        }

        protected abstract string GetSql(T condition);
        protected bool isParamValid(object param)
        {
            return ((param != null) && ("" != param.ToString()));
        }

        public List<V> Query(T condition)
        {
            string sql = this.GetSql(condition);
            DataSet ds = DbQueryHelperOraDDTek.Query(sql, this.GetCondition(sql, condition));
            return this.ConvertToCollection(ds);
        }

        public List<V> Query(T condition, string orderby)
        {
            string sql = this.GetSql(condition);
            DataSet ds = DbQueryHelperOraDDTek.Query(sql, this.GetCondition(sql, condition), orderby);
            return this.ConvertToCollection(ds);
        }

        public List<V> Query(T condition, PagingInfo pagingInfo, string orderby)
        {
            string sql = this.GetSql(condition);
            DataSet ds = DbQueryHelperOraDDTek.Query(sql, this.GetCondition(sql, condition), pagingInfo, orderby);
            return this.ConvertToCollection(ds);
        }
    }
}
