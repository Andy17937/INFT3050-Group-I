using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    /// Dashboard员用户表
    /// </summary>
    [SugarTable("users")]
    public class UsersDbModel
    {
        /// <summary>
        /// Primary key id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [SugarColumn(ColumnName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// 用户Password
        /// </summary>
        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }

        /// <summary>
        /// 用户Role
        /// </summary>
        [SugarColumn(ColumnName = "role")]
        public string Role { get; set; }

        /// <summary>
        /// Add时间
        /// </summary>
        [SugarColumn(ColumnName = "addtime")]
        public DateTime? Addtime { get; set; }
    }
}
