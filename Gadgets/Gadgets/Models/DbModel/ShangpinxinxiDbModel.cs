using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Item Information
    /// </summary>
    [SugarTable("shangpinxinxi")]
	public class ShangpinxinxiDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: Item图片
		/// </summary>
		[SugarColumn(ColumnName = "shangpintupian")]
		public string Shangpintupian { get; set; } = "";

		/// <summary>
		/// Desc: ItemName
		/// </summary>
		[SugarColumn(ColumnName = "shangpinmingcheng")]
		public string Shangpinmingcheng { get; set; } = "";

		/// <summary>
		/// Desc: Category
		/// </summary>
		[SugarColumn(ColumnName = "shangpinfenlei")]
		public string Shangpinfenlei { get; set; } = "";

		/// <summary>
		/// Desc: Item Details
		/// </summary>
		[SugarColumn(ColumnName = "shangpinxiangqing")]
		public string Shangpinxiangqing { get; set; } = "";

		/// <summary>
		/// Desc: Spot Amount
		/// </summary>
		[SugarColumn(ColumnName = "xianhuoshuliang")]
		public string Xianhuoshuliang { get; set; } = "";

		/// <summary>
		/// Desc: Vendor inventory
		/// </summary>
		[SugarColumn(ColumnName = "gongyingshangkucun")]
		public string Gongyingshangkucun { get; set; } = "";

		/// <summary>
		/// Desc: 赞
		/// </summary>
        [SugarColumn(ColumnName = "thumbsupnum")]
		public int? Thumbsupnum { get; set; } = 0;

		/// <summary>
		/// Desc: 踩
		/// </summary>
        [SugarColumn(ColumnName = "crazilynum")]
		public int? Crazilynum { get; set; } = 0;

		/// <summary>
		/// Desc: 最近点击时间
		/// </summary>
        [SugarColumn(ColumnName = "clicktime")]
		public DateTime? Clicktime { get; set; }

		/// <summary>
		/// Desc: Number of Clicks
		/// </summary>
        [SugarColumn(ColumnName = "clicknum")]
		public int? Clicknum { get; set; } = 0;

		/// <summary>
		/// Desc: Price
		/// </summary>
        [SugarColumn(ColumnName = "price")]
		public float? Price { get; set; } = 0;

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
