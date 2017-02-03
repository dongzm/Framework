using Framework.Core.Attributes;
using Framework.Core.Dal.BaseClass;
using Framework.Core.Model;
using Framework.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Framework.Core.Dal
{
    public class BaseDaoSQL<T, V> : BaseDao<T, V, SqlParameter>
        where T : BaseModel
        where V : List<T>
    {
        private ModelAttributes _modelAttributes;

        public BaseDaoSQL()
        {
            if (DataSchema.Instance.ModelReflectDictionary != null)
            {
                this._modelAttributes = DataSchema.Instance.ModelReflectDictionary[typeof(T)];
            }
        }

        public override T Add(T model)
        {
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            StringBuilder strSql = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("insert into {0}(", tableName);
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                strSql.Append(attribute.CloumnName + ",");
            }
            strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(") values (");
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                strSql.AppendFormat("@" + attribute.CloumnName + ",", new object[0]);
            }
            strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(")");
            this.FillSaveSqlAndParms(model, ref strSql, ref parameters);
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
            return model;
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

        public V ConvertToCollection(DataTable dt)
        {
            V local = Activator.CreateInstance<V>();
            if ((dt != null) && (dt.Rows.Count != 0))
            {
                foreach (DataRow row in dt.Rows)
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

        public override int DelByCondition(Condition<T> condition)
        {
            throw new NotImplementedException();
        }

        public override int DelByCondition(List<Condition<T>> conditionList)
        {
            throw new NotImplementedException();
        }

        public override void Delete(List<string> listId)
        {
            foreach (string str in listId)
            {
                this.Delete(str);
            }
        }

        public override void Delete(string id)
        {
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            StringBuilder strSql = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("delete from {0} ", tableName);
            strSql.Append(" where ");
            this.FillGetModelSqlAndParms(id, ref strSql, ref parameters);
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
        }

        public override void DeleteByExample(T model)
        {
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            StringBuilder strSql = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("delete from {0} ", tableName);
            strSql.Append(" where 1=1 ");
            if (model != null)
            {
                this.FillGetListSqlAndParms(model, ref strSql, ref parameters);
            }
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
        }

        protected override void DeleteBySql(string strWhere, SqlParameter[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            builder.AppendFormat("delete from {0} ", tableName);
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            DbHelperSQL.ExecuteSql(builder.ToString(), parameters);
        }

        private void FillGetListSqlAndParms(T model, ref StringBuilder strSql, ref DBParameterProviderSQL parameters)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                object paramterValue = info.GetValue(model, null);
                CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                string cloumnName = attribute.CloumnName;
                if ((paramterValue != null) && (paramterValue.ToString() != ""))
                {
                    strSql.AppendFormat(" AND {0}=@{0}", cloumnName);
                    parameters.Create("@" + cloumnName, paramterValue);
                }
            }
        }

        private void FillGetModelSqlAndParms(string id, ref StringBuilder strSql, ref DBParameterProviderSQL parameters)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (attribute.IsPrimaryKey)
                {
                    string cloumnName = attribute.CloumnName;
                    strSql.AppendFormat("{0}=@{0}", cloumnName);
                    parameters.Create<string>("@" + cloumnName, id);
                    break;
                }
            }
        }

        private void FillSaveSqlAndParms(T model, ref StringBuilder strSql, ref DBParameterProviderSQL parameters)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                object paramterValue = info.GetValue(model, null);
                if (propertyCloumnReflectAttr.ContainsKey(info))
                {
                    CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                    string cloumnName = attribute.CloumnName;
                    if ((paramterValue != null) && (paramterValue.ToString() != ""))
                    {
                        parameters.Create("@" + cloumnName, paramterValue);
                    }
                    else
                    {
                        strSql = RegularExpressionsHelper.Replace(strSql, "@" + cloumnName, "null");
                    }
                }
            }
        }

        public override V GetAll()
        {
            return this.GetByExample(default(T));
        }

        public override V GetAll(string orderby)
        {
            return this.GetByExample(default(T), orderby);
        }

        public override V GetAll(PagingInfo pagingInfo, string orderby)
        {
            return this.GetByExample(default(T), pagingInfo, orderby);
        }

        public override int GetAllCount()
        {
            return this.GetCountByExample(default(T));
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
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
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
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
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

        public override V GetByExample(T model)
        {
            return this.GetByExample(model, null, null);
        }

        public override V GetByExample(T model, string orderby)
        {
            return this.GetByExample(model, null, orderby);
        }

        public override V GetByExample(T model, PagingInfo pagingInfo, string orderby)
        {
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" 1=1 ");
            if (model != null)
            {
                this.FillGetListSqlAndParms(model, ref strSql, ref parameters);
            }
            return this.GetModelCollection(strSql.ToString(), parameters.ToArray(), pagingInfo, orderby);
        }

        public override int GetCountByCondition(Condition<T> condition)
        {
            StringBuilder builder = new StringBuilder();
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            string tableName = this._modelAttributes.TableReflect.TableName;
            builder.AppendFormat("select count(*) from {0} where ", tableName);
            if (condition != null)
            {
                string sqlAndParmsByCondition = this.GetSqlAndParmsByCondition(condition, ref parameters);
                builder.Append(sqlAndParmsByCondition);
            }
            return Convert.ToInt32(DbHelperSQL.GetSingle(builder.ToString(), parameters.ToArray()));
        }

        public override int GetCountByCondition(List<Condition<T>> conditionList)
        {
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
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
            return Convert.ToInt32(DbHelperSQL.GetSingle(builder.ToString(), parameters.ToArray()));
        }

        public override int GetCountByExample(T model)
        {
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("select count(*) from {0} where 1=1 ", tableName);
            if (model != null)
            {
                this.FillGetListSqlAndParms(model, ref strSql, ref parameters);
            }
            return Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString(), parameters.ToArray()));
        }

        public DataSet GetDataSet(string strSql)
        {
            return DbQueryHelperSQL.Query(strSql);
        }

        protected override DataSet GetList(string strWhere, SqlParameter[] parameters, PagingInfo pagingInfo, string orderby)
        {
            StringBuilder builder = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            builder.AppendFormat("select * from {0} ", tableName);
            if (strWhere.Trim() != "")
            {
                builder.Append(" where " + strWhere);
            }
            return DbQueryHelperSQL.Query(builder.ToString(), parameters, pagingInfo, orderby);
        }

        public override T GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            this.FillGetModelSqlAndParms(id, ref strSql, ref parameters);
            V modelCollection = this.GetModelCollection(strSql.ToString(), parameters.ToArray());
            if ((modelCollection != null) && (modelCollection.Count > 0))
            {
                return modelCollection[0];
            }
            return default(T);
        }

        protected override V GetModelCollection(string strWhere, SqlParameter[] parameters)
        {
            return this.GetModelCollection(strWhere, parameters, null, null);
        }

        protected override V GetModelCollection(string strWhere, SqlParameter[] parameters, string orderby)
        {
            return this.GetModelCollection(strWhere, parameters, null, orderby);
        }

        protected override V GetModelCollection(string strWhere, SqlParameter[] parameters, PagingInfo pagingInfo, string orderby)
        {
            DataSet ds = this.GetList(strWhere, parameters, pagingInfo, orderby);
            return this.ConvertToCollection(ds);
        }

        private string GetSqlAndParmsByCondition(Condition<T> condition, ref DBParameterProviderSQL parameters)
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
                        builder.AppendFormat(" AND {0}=@param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                        parameters.Create("@param" + parameters.Count.ToString(), obj2);
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
                    builder.AppendFormat(" AND {0} > @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> lessthan = condition.Lessthan;
            if (lessthan.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in lessthan)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} < @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> equals = condition.Equals;
            if (equals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in equals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} = @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> notEquals = condition.NotEquals;
            if (notEquals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in notEquals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} <> @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> greaterthanAndEquals = condition.GreaterthanAndEquals;
            if (greaterthanAndEquals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in greaterthanAndEquals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} >= @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> lessthanAndEquals = condition.LessthanAndEquals;
            if (lessthanAndEquals.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in lessthanAndEquals)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} <= @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> like = condition.Like;
            if (like.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in like)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} like @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
                }
            }
            Dictionary<string, object> notLike = condition.NotLike;
            if (notLike.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in notLike)
                {
                    cloumnName = pair.Key;
                    obj2 = pair.Value;
                    builder.AppendFormat(" AND {0} not like @param{1}", (object[])new string[] { cloumnName, parameters.Count.ToString() });
                    parameters.Create("@param" + parameters.Count.ToString(), obj2);
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
                            builder.AppendFormat("@param{0},", parameters.Count.ToString());
                            parameters.Create("@param" + parameters.Count.ToString(), obj3);
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
                            builder.AppendFormat("@param{0},", parameters.Count.ToString());
                            parameters.Create("@param" + parameters.Count.ToString(), obj3);
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

        public override bool IsExists(string id)
        {
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("select count(*) from {0} ", tableName);
            strSql.Append(" where ");
            this.FillGetModelSqlAndParms(id, ref strSql, ref parameters);
            object single = DbHelperSQL.GetSingle(strSql.ToString(), parameters.ToArray());
            if (object.Equals(single, null) || object.Equals(single, DBNull.Value))
            {
                return false;
            }
            return (Convert.ToInt32(single) > 0);
        }

        public override T Save(T model)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                if (attribute.IsPrimaryKey)
                {
                    object obj2 = info.GetValue(model, null);
                    if (!(((obj2 != null) && !string.IsNullOrEmpty(obj2.ToString())) && this.IsExists(obj2.ToString())))
                    {
                        info.SetValue(model, Guid.NewGuid().ToString(), null);
                        return this.Add(model);
                    }
                    return this.Update(model);
                }
            }
            return model;
        }

        public override List<T> Save(List<T> modelCollection)
        {
            List<T> list = new List<T>();
            foreach (T local in modelCollection)
            {
                list.Add(this.Save(local));
            }
            return list;
        }

        public override T Update(T model)
        {
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderSQL parameters = new DBParameterProviderSQL();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("update {0} set ", tableName);
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (!attribute.IsPrimaryKey)
                {
                    strSql.AppendFormat(" {0}=@{0},", attribute.CloumnName);
                }
            }
            strSql.Remove(strSql.Length - 1, 1);
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (attribute.IsPrimaryKey)
                {
                    strSql.AppendFormat(" where {0}=@{0}", attribute.CloumnName);
                }
            }
            this.FillSaveSqlAndParms(model, ref strSql, ref parameters);
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
            return model;
        }
    }
}

