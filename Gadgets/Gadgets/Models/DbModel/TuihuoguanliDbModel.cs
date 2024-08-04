using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Returns Management
    /// </summary>
    [SugarTable("tuihuoguanli")]
	public class TuihuoguanliDbModel
	{           
		/// <summary>
		/// Desc: Primary key id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: 时间
		/// </summary>
        [SugarColumn(ColumnName = "shijian")]
		public DateTime Shijian { get; set; }

		/// <summary>
		/// Desc: Member Name
		/// </summary>
		[SugarColumn(ColumnName = "huiyuanming")]
		public string Huiyuanming { get; set; } = "";

		/// <summary>
		/// Desc: Name
		/// </summary>
		[SugarColumn(ColumnName = "xingming")]
		public string Xingming { get; set; } = "";

		/// <summary>
		/// Desc: 退货时间
		/// </summary>
        [SugarColumn(ColumnName = "tuihuoshijian")]
		public DateTime? Tuihuoshijian { get; set; }

		/// <summary>
		/// Desc: Return Reason
		/// </summary>
		[SugarColumn(ColumnName = "tuihuoliyou")]
		public string Tuihuoliyou { get; set; } = "";

		/// <summary>
		/// Desc: Return Status
		/// </summary>
		[SugarColumn(ColumnName = "tuihuozhuangtai")]
		public string Tuihuozhuangtai { get; set; } = "";

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
