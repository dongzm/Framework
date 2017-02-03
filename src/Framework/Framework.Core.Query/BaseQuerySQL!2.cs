using Framework.Core.Attributes.Support;
using Framework.Core.Dal.BaseClass;
using Framework.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace Framework.Core.Query
{

    public abstract class BaseQuerySQL<T, V> where T: BaseQueryCondition where V: BaseQueryResult
    {
        protected BaseQuerySQL()
        {
        }

        protected virtual List<V> ConvertToCollection(DataSet ds)
        {
            List<V> list = new List<V>();
            if ((ds == null) || (ds.Tables[0].Rows.Count == 0))
            {
                return null;
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                V local = Activator.CreateInstance<V>();
                PropertyInfo[] properties = local.GetType().GetProperties();
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    foreach (PropertyInfo info in properties)
                    {
                        if (column.ColumnName.ToLower() == info.Name.ToLower())
                        {
                            if (!(row[column] is DBNull))
                            {
                                info.SetValue(local, row[column], null);
                            }
                            break;
                        }
                    }
                }
                list.Add(local);
            }
            return list;
        }

        protected virtual SqlParameter[] GetCondition(T condition)
        {
            DBParameterProviderSQL rsql = new DBParameterProviderSQL();
            PropertyInfo[] properties = condition.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                if (!GetCustomAttributes.IsUnParamedConditionAttribute(info))
                {
                    object paramterValue = info.GetValue(condition, null);
                    if (paramterValue != null)
                    {
                        rsql.Create("@" + info.Name.ToLower(), paramterValue);
                    }
                }
            }
            return rsql.ToArray();
        }

        protected abstract string GetSql(T condition);
        protected bool isParamValid(object param)
        {
            return ((param != null) && ("" != param.ToString()));
        }

        public List<V> Query(T condition)
        {
            DataSet ds = DbQueryHelperSQL.Query(this.GetSql(condition), this.GetCondition(condition));
            return this.ConvertToCollection(ds);
        }

        public List<V> Query(T condition, string orderby)
        {
            DataSet ds = DbQueryHelperSQL.Query(this.GetSql(condition), this.GetCondition(condition), orderby);
            return this.ConvertToCollection(ds);
        }

        public List<V> Query(T condition, PagingInfo pagingInfo, string orderby)
        {
            DataSet ds = DbQueryHelperSQL.Query(this.GetSql(condition), this.GetCondition(condition), pagingInfo, orderby);
            return this.ConvertToCollection(ds);
        }
    }
}

