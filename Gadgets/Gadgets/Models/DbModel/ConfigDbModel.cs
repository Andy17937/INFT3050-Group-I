using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    [SugarTable("config")]
    public class ConfigDbModel
    {
        /// <summary>
        /// Primary key id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// 配置名
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 配置Value
        /// </summary>
        [SugarColumn(ColumnName = "value")]
        public string Value { get; set; }
    }
}
