namespace Framework.Core.Dal.BaseClass
{
    using System;
    using System.Data.Common;

    public class SqlCmd
    {
        private string cmdText;
        private DbParameter[] pars;

        public SqlCmd(string strCmdText, DbParameter[] parsInfo)
        {
            this.CmdText = strCmdText;
            this.Pars = parsInfo;
        }

        public string CmdText
        {
            get
            {
                return this.cmdText;
            }
            set
            {
                this.cmdText = value;
            }
        }

        public DbParameter[] Pars
        {
            get
            {
                return this.pars;
            }
            set
            {
                this.pars = value;
            }
        }
    }
}

