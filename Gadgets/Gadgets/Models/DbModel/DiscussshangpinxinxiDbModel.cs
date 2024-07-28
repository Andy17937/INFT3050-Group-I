using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Item Comment表
    /// </summary>
    [SugarTable("discussshangpinxinxi")]
	public class DiscussshangpinxinxiDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: 关联表id
		/// </summary>
        [SugarColumn(ColumnName = "refid")]
		public long? Refid { get; set; } = 0;

		/// <summary>
		/// Desc: 用户id
		/// </summary>
        [SugarColumn(ColumnName = "userid")]
		public long? Userid { get; set; } = 0;

		/// <summary>
		/// Desc: Username
		/// </summary>
		[SugarColumn(ColumnName = "nickname")]
		public string Nickname { get; set; } = "";

		/// <summary>
		/// Desc: Comments
		/// </summary>
		[SugarColumn(ColumnName = "content")]
		public string Content { get; set; } = "";

		/// <summary>
		/// Desc: Reply content
		/// </summary>
		[SugarColumn(ColumnName = "reply")]
		public string Reply { get; set; } = "";

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
