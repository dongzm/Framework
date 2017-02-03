using Framework.Core.Attributes;
using System;

namespace Framework.Core.Proc
{
    public class BaseProcedureCondition
    {
        private string procName;

        public BaseProcedureCondition(string strProcName)
        {
            this.ProcName = strProcName;
        }

        [IsUnParamedCondition]
        public string ProcName
        {
            get
            {
                return this.procName;
            }
            set
            {
                this.procName = value;
            }
        }
    }
}

