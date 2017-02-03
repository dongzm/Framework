using Framework.Support;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace Framework.Core.Dal.BaseClass
{

    public class DbQueryHelperOra
    {
        public static int GetCountByQuery(string sqlStr, OracleParameter[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT COUNT(*) FROM ( ");
            builder.Append(sqlStr);
            builder.Append(" )");
            return Convert.ToInt32(DbHelperOra.GetSingle(builder.ToString(), parameters));
        }

        public static DataSet Query(string sqlStr)
        {
            return DbHelperOra.Query(sqlStr.ToString());
        }

        public static DataSet Query(string sqlStr, OracleParameter[] parameters)
        {
            return DbHelperOra.Query(sqlStr.ToString(), parameters);
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
            return DbHelperOra.Query(builder.ToString(), parameters);
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
            return DbHelperOra.Query(builder.ToString(), parameters);
        }
    }
}

