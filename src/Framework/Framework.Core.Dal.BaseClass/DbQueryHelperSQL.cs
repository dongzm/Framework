namespace Framework.Core.Dal.BaseClass
{
    using global::Framework.Support;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class DbQueryHelperSQL
    {
        public static int GetCountByQuery(string sqlStr, SqlParameter[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT COUNT(*) FROM ( ");
            builder.Append(sqlStr);
            builder.Append(" ) AS TMP_TABLE");
            return Convert.ToInt32(DbHelperSQL.GetSingle(builder.ToString(), parameters));
        }

        public static DataSet Query(string sqlStr)
        {
            return DbHelperSQL.Query(sqlStr.ToString());
        }

        public static DataSet Query(string sqlStr, SqlParameter[] parameters)
        {
            return DbHelperSQL.Query(sqlStr.ToString(), parameters);
        }

        public static DataSet Query(string sqlStr, SqlParameter[] parameters, string orderby)
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
            return DbHelperSQL.Query(builder.ToString(), parameters);
        }

        public static DataSet Query(string sqlStr, SqlParameter[] parameters, PagingInfo paginginfo, string orderby)
        {
            if (string.IsNullOrEmpty(orderby))
            {
                return Query(sqlStr, parameters);
            }
            if (null == paginginfo)
            {
                return Query(sqlStr, parameters, orderby);
            }
            StringBuilder builder = new StringBuilder();
            int num = paginginfo.PageSize * paginginfo.CurrentPage;
            int pageSize = paginginfo.PageSize;
            paginginfo.TotalRows = GetCountByQuery(sqlStr, parameters);
            if ((paginginfo.TotalRows > 0) && (num > paginginfo.TotalRows))
            {
                pageSize = paginginfo.PageSize - (num - paginginfo.TotalRows);
            }
            if (pageSize <= 0)
            {
                paginginfo.CurrentPage = paginginfo.getTotalPages();
                return Query(sqlStr, parameters, paginginfo, orderby);
            }
            builder.Append("SELECT * FROM ( ");
            builder.AppendFormat("SELECT top {0} * ", pageSize.ToString());
            builder.Append("From ( ");
            builder.AppendFormat("SELECT top {0} * From ({1}) as t3 Order by {2}) as t2 ", num.ToString(), sqlStr, orderby);
            StringBuilder builder2 = new StringBuilder();
            string[] strArray = orderby.ToLower().Split(new char[] { ',' });
            if ((strArray != null) && (strArray.Length > 0))
            {
                foreach (string str in strArray)
                {
                    if (str.IndexOf(" asc") > 0)
                    {
                        builder2.AppendFormat("{0},", str.Replace(" asc", " desc"));
                    }
                    else if (str.IndexOf(" desc") > 0)
                    {
                        builder2.AppendFormat("{0},", str.Replace(" desc", " asc"));
                    }
                    else
                    {
                        builder2.AppendFormat("{0} desc,", str);
                    }
                }
            }
            builder.AppendFormat("ORDER BY {0} ) as t3 ", builder2.ToString().TrimEnd(new char[] { ',' }));
            builder.AppendFormat("ORDER BY {0} ", orderby);
            return DbHelperSQL.Query(builder.ToString(), parameters);
        }
    }
}

