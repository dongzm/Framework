using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Proc
{
    public class ProcedureOperationDDTek
    {
        private static BaseProcOraDDTek procOperation = new BaseProcOraDDTek();

        public static List<V> RunProcedure<T, V>(T condition) where T : BaseProcedureCondition where V : BaseProcedureResult
        {
            return procOperation.RunProcedure<T, V>(condition);
        }

        public static void RunProcedure<T>(T condition) where T : BaseProcedureCondition
        {
            procOperation.RunProcedure<T>(condition);
        }

        public static void RunProcedure(string procName)
        {
            procOperation.RunProcedure(procName);
        }
    }
}
