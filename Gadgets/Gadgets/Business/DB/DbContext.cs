using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gadgets.Common.Helpers;
using Gadgets.Models.ConfModel;

namespace Gadgets.Business.DB
{
    public class DbContext<T> where T : class, new()
    {
        public DbContext()
        {
            DbConfig dbConfig = ConfigHelper.GetConfig<DbConfig>("DbConfig");

            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbConfig.ConnectionString,
                DbType = dbConfig.DbType.ToLower() == "mysql" ? DbType.MySql : DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,

            });

            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                // Console.WriteLine(sql + "\r\n" +
                    // Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                // Console.WriteLine();
            };
        }

        public SqlSugarClient Db;
        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }

        public dynamic Common(string tableName, string columnName, string columnValue = "", int type = 0, string sign = "option", int remindStart = 0, int remindEnd = 0)
        {
            sign = sign.ToLower();
            string sql = "";
            dynamic res = null;
            switch (sign)
            {
                case "option":
                    string where = "";
                    if (type == 1)
                    {
                        where = " WHERE level = " + type;
                    }

                    if (type > 1)
                    {
                        where = " WHERE level = " + type + " AND parent = " + columnValue;
                    }
                    sql = "SELECT " + columnName + " FROM " + tableName + where;
                    res = Db.Ado.SqlQuery<string>(sql).ToList();
                    break;

                case "follow":
                    sql = "SELECT * FROM " + tableName +" WHERE " + columnName + " = '" + columnValue + "'";
                    res = Db.Ado.SqlQuerySingle<dynamic>(sql);
                    break;

                case "sh":
                    columnValue = columnValue == "Yes" ? "No" : "Yes";
                    sql = "UPDATE " + tableName + " SET Sfsh = '" + columnValue + "' WHERE Id = " + Convert.ToInt32(columnName);
                    res = Db.Ado.ExecuteCommand(sql);
                    break;

                case "remind":
                    res = 0;
                    if (remindStart > 0 || remindEnd > 0)
                    {
                        if (type == 1)
                        {
                            if (remindStart > 0)
                            {
                                sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " >= " + remindStart;
                            }

                            if (remindEnd > 0)
                            {
                                sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " <= " + remindEnd;
                            }

                            if (remindStart > 0 && remindEnd > 0)
                            {
                                sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " >= " + remindStart + " AND " + columnName + " <= " + remindEnd;
                            }

                            res = Db.Ado.SqlQuerySingle<int>(sql);
                        }

                        if (type == 2)
                        {
                            DateTime dt = DateTime.Now;
                            DateTime dtStart = dt.AddDays(-remindStart);
                            DateTime dtEnd = dt.AddDays(remindEnd);

                            if (remindStart > 0 && remindEnd > 0)
                            {
                                sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " BETWEEN '" + dtStart + "' AND '" + dtEnd + "'";
                            }
                            else if (remindStart > 0)
                            {
                                sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " >= '" + dtStart + "'";
                            }
                            else if (remindEnd > 0)
                            {
                                sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " <= '" + dtEnd + "'";
                            }

                            res = Db.Ado.SqlQuerySingle<int>(sql);
                        }
                    }
                    break;

                case "cal":
                    sql = "SELECT SUM(" + columnName + ") AS sum, MAX(" + columnName + ") AS max, MIN(" + columnName + ") AS min, AVG(" + columnName + ") AS avg FROM " + tableName;
                    res = Db.Ado.SqlQuerySingle<dynamic>(sql);
                    break;

                case "group":
                    sql = "SELECT COUNT(*) AS total, " + columnName + " FROM " + tableName + " GROUP BY " + columnName;
                    res = Db.Ado.SqlQuery<dynamic>(sql);
                    break;

                case "value":
                    // sql = "SELECT " + columnName + ", " + columnValue + " AS total FROM " + tableName + " GROUP BY " + columnName + ", " + columnValue;
                    sql = "SELECT " + columnName + ", SUM(" + columnValue + ") AS total FROM " + tableName + " GROUP BY " + columnName;
                    res = Db.Ado.SqlQuery<dynamic>(sql);
                    break;
            }

            return res;
        }
    }
}
