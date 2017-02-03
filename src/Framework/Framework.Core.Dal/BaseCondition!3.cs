using Framework.Core.Model;
using Framework.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Framework.Core.Dal
{
    

    public abstract class BaseCondition<T, V, W>
        where T : BaseModel
        where V : List<T>
        where W : DbParameter
    {
        protected BaseCondition()
        {
        }

        protected abstract V ConvertToCollection(DataSet ds);
        public abstract V GetByCondition(Condition<T> condition);
        public abstract V GetByCondition(List<Condition<T>> conditionList);
        public abstract V GetByCondition(Condition<T> condition, string orderby);
        public abstract V GetByCondition(List<Condition<T>> conditionList, string orderby);
        public abstract V GetByCondition(Condition<T> condition, PagingInfo pagingInfo, string orderby);
        public abstract V GetByCondition(List<Condition<T>> conditionList, PagingInfo pagingInfo, string orderby);
        public abstract int GetCountByCondition(Condition<T> condition);
        public abstract int GetCountByCondition(List<Condition<T>> conditionList);
        protected abstract DataSet GetList(string strWhere, W[] parameters, PagingInfo pagingInfo, string orderby);
        protected abstract V GetModelCollection(string strWhere, W[] parameters);
        protected abstract V GetModelCollection(string strWhere, W[] parameters, string orderby);
        protected abstract V GetModelCollection(string strWhere, W[] parameters, PagingInfo pagingInfo, string orderby);
    }
}

