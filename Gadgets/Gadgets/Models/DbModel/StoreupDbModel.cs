using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: 收藏表
    /// </summary>
    [SugarTable("storeup")]
	public class StoreupDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: 用户id
		/// </summary>
        [SugarColumn(ColumnName = "userid")]
		public long? Userid { get; set; } = 0;

		/// <summary>
		/// Desc: 收藏id
		/// </summary>
        [SugarColumn(ColumnName = "refid")]
		public long? Refid { get; set; } = 0;

		/// <summary>
		/// Desc: 表名
		/// </summary>
		[SugarColumn(ColumnName = "tablename")]
		public string Tablename { get; set; } = "";

		/// <summary>
		/// Desc: Favorite Item Name
		/// </summary>
		[SugarColumn(ColumnName = "name")]
		public string Name { get; set; } = "";

		/// <summary>
		/// Desc: Item Picture
		/// </summary>
		[SugarColumn(ColumnName = "picture")]
		public string Picture { get; set; } = "";

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
