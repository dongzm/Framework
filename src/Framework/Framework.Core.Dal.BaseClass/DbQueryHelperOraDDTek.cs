using DDTek.Oracle;
using Framework.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Framework.Core.Dal.BaseClass
{
    public class DbQueryHelperOraDDTek
    {
        public static int GetCountByQuery(string sqlStr, OracleParameter[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT COUNT(*) FROM ( ");
            builder.Append(sqlStr);
            builder.Append(" )");
            object obj = DbHelperOraDDTek.GetSingle(builder.ToString(), parameters);
            return Convert.ToInt32(obj);
        }

        public static DataSet Query(string sqlStr)
        {
            return DbHelperOraDDTek.Query(sqlStr.ToString());
        }

        public static DataSet Query(string sqlStr, OracleParameter[] parameters)
        {
            return DbHelperOraDDTek.Query(sqlStr.ToString(), parameters);
        }

        public static DataSet Query(string sqlStr, OracleParameter[] parameters, string orderby)
        {
            if (string.IsNullOrEmpty(orderby))
            {
                return Query(sqlStr, parameters);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * ");
            builder.Append("FROM ");
            builder.Append("( ");
            builder.Append(sqlStr);
            builder.Append("\t)TMP_TABLE ");
            builder.AppendFormat(" ORDER BY {0} ", orderby);
            return DbHelperOraDDTek.Query(builder.ToString(), parameters);
        }

        public static DataSet Query(string sqlStr, OracleParameter[] parameters, PagingInfo paginginfo, string orderby)
        {
            StringBuilder builder = new StringBuilder();
            if (paginginfo != null)
            {
                builder.Append("SELECT * FROM (SELECT TT.*, ROWNUM AS ROWNO FROM ( ");
                builder.Append(sqlStr);
                if (!string.IsNullOrEmpty(orderby))
                {
                    builder.Append(" order by " + orderby);
                }
                builder.Append(") TT WHERE ROWNUM <= " + ((paginginfo.CurrentPage * paginginfo.PageSize)).ToString() + ") TABLE_ALIAS ");
                builder.Append("where TABLE_ALIAS.rowno > " + paginginfo.getCurrentRow().ToString());
                paginginfo.TotalRows = GetCountByQuery(sqlStr, parameters);
            }
            else
            {
                if (string.IsNullOrEmpty(orderby))
                {
                    return Query(sqlStr, parameters);
                }
                return Query(sqlStr, parameters, orderby);
            }
            return DbHelperOraDDTek.Query(builder.ToString(), parameters);
        }
    }
}
