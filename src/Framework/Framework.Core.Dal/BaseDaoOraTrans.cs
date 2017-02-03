namespace Framework.Core.Dal
{
    using BaseClass;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Attributes;
    using Model;
    using global::Framework.Support;

    public class BaseDaoOraTrans : IBaseDaoTrans
    {
        private ModelAttributes _modelAttributes;
        private List<SqlCmd> lisCmds = null;

        public BaseDaoOraTrans()
        {
            this.lisCmds = new List<SqlCmd>();
        }

        public BaseModel Add(BaseModel model)
        {
            this.SetCurrModelAttributes(model);
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("insert into {0}(", tableName);
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (!attribute.IsIgnoreInTable)
                {
                    strSql.Append(attribute.CloumnName + ",");
                }
            }
            strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(") values (");
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (!attribute.IsIgnoreInTable)
                {
                    strSql.AppendFormat(":" + attribute.CloumnName + ",", new object[0]);
                }
            }
            strSql.Remove(strSql.Length - 1, 1);
            strSql.Append(")");
            model = this.FillModelInfoAspect<BaseModel>(model);
            this.FillSaveSqlAndParms(model, ref strSql, ref parameters);
            this.lisCmds.Add(new SqlCmd(strSql.ToString(), parameters.ToArray()));
            return model;
        }

        public bool CommitTran()
        {
            bool flag = DbHelperOra.ExecuteSqlTran(this.lisCmds);
            this.lisCmds.Clear();
            return flag;
        }

        public void Delete<T>(List<string> listId) where T : BaseModel
        {
            this.SetCurrModelAttributes(typeof(T));
            foreach (string str in listId)
            {
                this.Delete(str);
            }
        }

        protected void Delete(string id)
        {
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("delete from {0} ", tableName);
            strSql.Append(" where ");
            this.FillGetModelSqlAndParms(id, ref strSql, ref parameters);
            this.lisCmds.Add(new SqlCmd(strSql.ToString(), parameters.ToArray()));
        }

        public void Delete<T>(string id) where T : BaseModel
        {
            this.SetCurrModelAttributes(typeof(T));
            this.Delete(id);
        }

        public void DeleteByExample(BaseModel model)
        {
            this.SetCurrModelAttributes(model);
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            StringBuilder strSql = new StringBuilder();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("delete from {0} ", tableName);
            strSql.Append(" where 1=1 ");
            if (model != null)
            {
                this.FillGetListSqlAndParms(model, ref strSql, ref parameters);
            }
            this.lisCmds.Add(new SqlCmd(strSql.ToString(), parameters.ToArray()));
        }

        private void FillGetListSqlAndParms(BaseModel model, ref StringBuilder strSql, ref DBParameterProviderOra parameters)
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
                    strSql.AppendFormat(" AND {0}=:{0}", cloumnName);
                    parameters.Create(":" + cloumnName, paramterValue);
                }
            }
        }

        private void FillGetModelSqlAndParms(string id, ref StringBuilder strSql, ref DBParameterProviderOra parameters)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (attribute.IsPrimaryKey)
                {
                    string cloumnName = attribute.CloumnName;
                    strSql.AppendFormat("{0}=:{0}", cloumnName);
                    parameters.Create<string>(":" + cloumnName, id);
                    break;
                }
            }
        }

        private OracleParameter FillModelInfoAspect<OracleParameter>(OracleParameter model)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (attribute.CloumnName.ToLower() == "createtime")
                {
                    PropertyInfo property = model.GetType().GetProperty("Createtime");
                    if (null != property)
                    {
                        property.SetValue(model, this.GetCurrentDate(), null);
                    }
                    return model;
                }
            }
            return model;
        }

        private void FillSaveSqlAndParms(BaseModel model, ref StringBuilder strSql, ref DBParameterProviderOra parameters)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                object paramterValue = info.GetValue(model, null);
                CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                string cloumnName = attribute.CloumnName;
                if (!attribute.IsIgnoreInTable && (paramterValue != null) && (paramterValue.ToString() != ""))
                {
                    parameters.Create(":" + cloumnName, paramterValue);
                }
                else if (!attribute.IsIgnoreInTable)
                {
                    strSql = RegularExpressionsHelper.Replace(strSql, ":" + cloumnName, "null");
                }
            }
        }

        private DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public void Rollback()
        {
            this.lisCmds.Clear();
        }

        public List<BaseModel> Save(List<BaseModel> modelCollection)
        {
            List<BaseModel> list = new List<BaseModel>();
            foreach (BaseModel model in modelCollection)
            {
                list.Add(this.Save(model));
            }
            return list;
        }

        public BaseModel Save(BaseModel model)
        {
            this.SetCurrModelAttributes(model);
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            PropertyInfo[] properties = model.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                CloumnReflectAttribute attribute = propertyCloumnReflectAttr[info];
                if (attribute.IsPrimaryKey)
                {
                    if (info.GetValue(model, null) == null)
                    {
                        info.SetValue(model, Guid.NewGuid().ToString("N"), null);
                        return this.Add(model);
                    }
                    return this.Update(model);
                }
            }
            return model;
        }

        private void SetCurrModelAttributes(BaseModel model)
        {
            if (model == null)
            {
                throw new Exception("对象为NULL");
            }
            this.SetCurrModelAttributes(model.GetType());
        }

        private void SetCurrModelAttributes(Type type)
        {
            if (DataSchema.Instance.ModelReflectDictionary != null)
            {
                this._modelAttributes = DataSchema.Instance.ModelReflectDictionary[type];
            }
        }

        public BaseModel Update(BaseModel model)
        {
            this.SetCurrModelAttributes(model);
            StringBuilder strSql = new StringBuilder();
            DBParameterProviderOra parameters = new DBParameterProviderOra();
            string tableName = this._modelAttributes.TableReflect.TableName;
            strSql.AppendFormat("update {0} set ", tableName);
            Dictionary<PropertyInfo, CloumnReflectAttribute> propertyCloumnReflectAttr = this._modelAttributes.PropertyCloumnReflectAttr;
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (!attribute.IsPrimaryKey)
                {
                    strSql.AppendFormat(" {0}=:{0},", attribute.CloumnName);
                }
            }
            strSql.Remove(strSql.Length - 1, 1);
            foreach (CloumnReflectAttribute attribute in propertyCloumnReflectAttr.Values)
            {
                if (attribute.IsPrimaryKey)
                {
                    strSql.AppendFormat(" where {0}=:{0}", attribute.CloumnName);
                }
            }
            this.FillSaveSqlAndParms(model, ref strSql, ref parameters);
            this.lisCmds.Add(new SqlCmd(strSql.ToString(), parameters.ToArray()));
            return model;
        }
    }
}

