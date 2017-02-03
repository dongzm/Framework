using Framework.Core.Attributes;
using Framework.Core.Dal.BaseClass;
using Framework.Core.Model;
using Framework.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Reflection;
using System.Text;

namespace Framework.Core.Dal
{
    public class ConditionOra<T, V> : BaseCondition<T, V, OracleParameter>
        where T : BaseModel
        where V : List<T>
    {
        private ModelAttributes _modelAttributes;

        public ConditionOra()
        {
            if (DataSchema.Instance.ModelReflectDictionary != null)
            {
                this._modelAttributes = DataSchema.Instance.ModelReflectDictionary[typeof(T)];
            }
        }

        protected override V ConvertToCollection(DataSet ds)
        {
            V local = Activator.CreateInstance<V>();
            if ((ds != null) && (ds.Tables[0].Rows.Count != 0))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    T local2 = Activator.CreateInstance<T>();
                    Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
                    foreach (PropertyInfo info in propertyCloumnReflectAttr.Keys)
                    {
                        CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                        string cloumnName = attribute.CloumnName;
                        if (row[cloumnName].ToString() != "")
                        {
                            info.SetValue(local2, row[cloumnName], null);
                        }
                    }
                    MethodInfo method = local.GetType().GetMethod("Add");
                    object[] parameters = new object[] { local2 };
                    method.Invoke(local, parameters);
                }
            }
            return local;
        }

        public override V GetByCondition(Condition<T> condition)
        {
            return this.GetByCondition(condition, null, null);
        }

        public override V GetByCondition(List<Condition<T>> conditionList)
        {
            return this.GetByCondition(conditionList, null, null);
        }

        public override V GetByCondition(Condition<T> condition, string orderby)
        {
            return this.GetByCondition(condition, null, orderby);
        }

        public override V GetByCondition(List<Condition<T>> conditionList, string orderby)
        {
            return this.GetByCondition(conditionList, null, orderby);
        }

        public override V GetByCondition(Condition<T> condition, PagingInfo pagingInfo, string orderby)
        {
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            StringBuilder builder = new StringBuilder();
            if (condition != null)
            {
                string sqlAndParmsByCondition = this.GetSqlAndParmsByCondition(condition, ref parameters);
                builder.Append(sqlAndParmsByCondition);
            }
            return this.GetModelCollection(builder.ToString(), parameters.ToArray(), pagingInfo, orderby);
        }

        public override V GetByCondition(List<Condition<T>> conditionList, PagingInfo pagingInfo, string orderby)
        {
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            StringBuilder builder = new StringBuilder();
            builder.Append("1=2");
            if ((conditionList != null) && (conditionList.Count > 0))
            {
                foreach (Condition<T> condition in conditionList)
                {
                    string sqlAndParmsByCondition = this.GetSqlAndParmsByCondition(condition, ref parameters);
                    builder.AppendFormat(" OR ({0})", sqlAndParmsByCondition);
                }
            }
            return this.GetModelCollection(builder.ToString(), parameters.ToArray(), pagingInfo, orderby);
        }

        public override int GetCountByCondition(Condition<T> condition)
        {
            StringBuilder builder = new StringBuilder();
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            string tableName = this._modelAttributes.TableReflect.TableName;
            builder.AppendFormat("select count(*) from {0} where ", tableName);
            if (condition != null)
            {
                string sqlAndParmsByCondition = this.GetSqlAndParmsByCondition(condition, ref parameters);
                builder.Append(sqlAndParmsByCondition);
            }
            return Convert.ToInt32(DbHelperOra.GetSingle(builder.ToString(), parameters.ToArray()));
        }

        public override int GetCountByCondition(List<Condition<T>> conditionList)
        {
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            StringBuilder builder = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            builder.AppendFormat("select count(*) from {0} where ", tableName);
            builder.Append("1=2");
            if ((conditionList != null) && (conditionList.Count > 0))
            {
                foreach (Condition<T> condition in conditionList)
                {
                    string sqlAndParmsByCondition = this.GetSqlAndParmsByCondition(condition, ref parameters);
                    builder.AppendFormat(" OR ({0})", sqlAndParmsByCondition);
                }
            }
            return Convert.ToInt32(DbHelperOra.GetSingle(builder.ToString(), parameters.ToArray()));
        }

        protected override DataSet GetList(string strWhere, OracleParameter[] parameters, PagingInfo pagingInfo, string orderby)
        {
            StringBuilder builder = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            builder.AppendFormat("select * from {0} ", tableName);
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DbQueryHelperOra.Query(builder.ToString(), parameters, pagingInfo, orderby);
        }

        protected override V GetModelCollection(string strWhere, OracleParameter[] parameters)
        {
            return this.GetModelCollection(strWhere, parameters, null, null);
        }

        protected override V GetModelCollection(string strWhere, OracleParameter[] parameters, string orderby)
        {
            return this.GetModelCollection(strWhere, parameters, null, orderby);
        }

        protected override V GetModelCollection(string strWhere, OracleParameter[] parameters, PagingInfo pagingInfo, string orderby)
        {
            DataSet ds = this.GetList(strWhere, parameters, pagingInfo, orderby);
            return this.ConvertToCollection(ds);
        }

        private string GetSqlAndParmsByCondition(Condition<T> condition, ref DBParameterProviderOra parameters)
        {
            object obj2;
            string cloumnName;
            ArrayList list;
            int num;
            object obj3;
            StringBuilder builder = new StringBuilder("1=1");
            T model = condition.Model;
            if (model != null)
            {
                Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
                PropertyInfo[] properties = model.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    obj2 = info.GetValue(model, null);
                    CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                    cloumnName = attribute.CloumnName;
                    if ((obj2 != null) && (obj2.ToString() != ""))
                    {
                        builder.AppendFormat(" AND {0}=:param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                        parameters.Create(":param" + parameters.Count.ToString(), obj2);
                    }
                }
            }
            Dictionary<string, object> greaterthan = condition.Greaterthan;
            if (greaterthan.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in greaterthan)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} > :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> lessthan = condition.Lessthan;
            if (lessthan.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in lessthan)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} < :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> equals = condition.Equals;
            if (equals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in equals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} = :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> notEquals = condition.NotEquals;
            if (notEquals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in notEquals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} <> :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> greaterthanAndEquals = condition.GreaterthanAndEquals;
            if (greaterthanAndEquals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in greaterthanAndEquals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} >= :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> lessthanAndEquals = condition.LessthanAndEquals;
            if (lessthanAndEquals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in lessthanAndEquals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} <= :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> like = condition.Like;
            if (like.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in like)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} like :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> notLike = condition.NotLike;
            if (notLike.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in notLike)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} not like :param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create(":param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, ArrayList> @in = condition.In;
            if (@in.Count > 0)
            {
                foreach (KeyValuePair<string, ArrayList> pair2 in @in)
                {
                    cloumnName = pair2.Key;
                    list = pair2.Value;
                    if ((list != null) && (list.Count > 0))
                    {
                        builder.AppendFormat(" AND {0} in (", cloumnName);
                        num = 0;
                        while (num < list.Count)
                        {
                            obj3 = list[num];
                            builder.AppendFormat(":param{0},", parameters.Count.ToString());
                            parameters.Create(":param" + parameters.Count.ToString(), obj3);
                            num++;
                        }
                        if (builder.ToString().EndsWith(","))
                        {
                            builder.Remove(builder.Length - 1, 1);
                        }
                        builder.Append(")");
                    }
                }
            }
            Dictionary<string, ArrayList> notIn = condition.NotIn;
            if (notIn.Count > 0)
            {
                foreach (KeyValuePair<string, ArrayList> pair2 in notIn)
                {
                    cloumnName = pair2.Key;
                    list = pair2.Value;
                    if ((list != null) && (list.Count > 0))
                    {
                        builder.AppendFormat(" AND {0} not in (", cloumnName);
                        for (num = 0; num < list.Count; num++)
                        {
                            obj3 = list[num];
                            builder.AppendFormat(":param{0},", parameters.Count.ToString());
                            parameters.Create(":param" + parameters.Count.ToString(), obj3);
                        }
                        if (builder.ToString().EndsWith(","))
                        {
                            builder.Remove(builder.Length - 1, 1);
                        }
                        builder.Append(")");
                    }
                }
            }
            return builder.ToString();
        }
    }
}

