using Framework.Core.Model;
using Framework.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Framework.Core.Dal
{

    public abstract class BaseDao<T, V, W>
        where T : BaseModel
        where V : List<T>
        where W : DbParameter
    {
        protected BaseDao()
        {
        }

        public abstract T Add(T model);
        protected abstract V ConvertToCollection(DataSet ds);
        public abstract int DelByCondition(Condition<T> condition);
        public abstract int DelByCondition(List<Condition<T>> conditionList);
        public abstract void Delete(List<string> listId);
        public abstract void Delete(string id);
        public abstract void DeleteByExample(T model);
        protected abstract void DeleteBySql(string strWhere, W[] parameters);
        public abstract V GetAll();
        public abstract V GetAll(string orderby);
        public abstract V GetAll(PagingInfo pagingInfo, string orderby);
        public abstract int GetAllCount();
        public abstract V GetByCondition(Condition<T> condition);
        public abstract V GetByCondition(List<Condition<T>> conditionList);
        public abstract V GetByCondition(Condition<T> condition, string orderby);
        public abstract V GetByCondition(List<Condition<T>> conditionList, string orderby);
        public abstract V GetByCondition(Condition<T> condition, PagingInfo pagingInfo, string orderby);
        public abstract V GetByCondition(List<Condition<T>> conditionList, PagingInfo pagingInfo, string orderby);
        public abstract V GetByExample(T model);
        public abstract V GetByExample(T model, string orderby);
        public abstract V GetByExample(T model, PagingInfo pagingInfo, string orderby);
        public abstract int GetCountByCondition(Condition<T> condition);
        public abstract int GetCountByCondition(List<Condition<T>> conditionList);
        public abstract int GetCountByExample(T model);
        protected abstract DataSet GetList(string strWhere, W[] parameters, PagingInfo pagingInfo, string orderby);
        public abstract T GetModel(string id);
        protected abstract V GetModelCollection(string strWhere, W[] parameters);
        protected abstract V GetModelCollection(string strWhere, W[] parameters, string orderby);
        protected abstract V GetModelCollection(string strWhere, W[] parameters, PagingInfo pagingInfo, string orderby);
        public abstract bool IsExists(string id);
        public abstract T Save(T model);
        public abstract List<T> Save(List<T> modelCollection);
        public abstract T Update(T model);
    }
}

