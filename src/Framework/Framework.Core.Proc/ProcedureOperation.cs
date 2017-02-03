using System;
using System.Collections.Generic;


namespace Framework.Core.Proc
{

    public class ProcedureOperation
    {
        private static BaseProcOra procOperation = new BaseProcOra();

        public static List<V> RunProcedure<T, V>(T condition) where T: BaseProcedureCondition where V: BaseProcedureResult
        {
            return procOperation.RunProcedure<T, V>(condition);
        }

        public static void RunProcedure<T>(T condition) where T: BaseProcedureCondition
        {
            procOperation.RunProcedure<T>(condition);
        }

        public static void RunProcedure(string procName)
        {
            procOperation.RunProcedure(procName);
        }
    }
}

