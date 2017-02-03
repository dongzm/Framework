using Framework.Core.Model;
using System;
using System.Collections.Generic;

namespace Framework.Core.Dal
{

    public interface IBaseDaoTrans
    {
        BaseModel Add(BaseModel model);
        bool CommitTran();
        void Delete<T>(List<string> listId) where T: BaseModel;
        void Delete<T>(string id) where T: BaseModel;
        void DeleteByExample(BaseModel model);
        void Rollback();
        BaseModel Save(BaseModel model);
        List<BaseModel> Save(List<BaseModel> modelCollection);
        BaseModel Update(BaseModel model);
    }
}

