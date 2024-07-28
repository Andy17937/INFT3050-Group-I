using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Cart表
    /// </summary>
    [SugarTable("cart")]
	public class CartDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: Item表名
		/// </summary>
		[SugarColumn(ColumnName = "tablename")]
		public string Tablename { get; set; } = "";

		/// <summary>
		/// Desc: 用户id
		/// </summary>
        [SugarColumn(ColumnName = "userid")]
		public long? Userid { get; set; } = 0;

		/// <summary>
		/// Desc: Itemid
		/// </summary>
        [SugarColumn(ColumnName = "goodid")]
		public long? Goodid { get; set; } = 0;

		/// <summary>
		/// Desc: ItemName
		/// </summary>
		[SugarColumn(ColumnName = "goodname")]
		public string Goodname { get; set; } = "";

		/// <summary>
		/// Desc: 图片
		/// </summary>
		[SugarColumn(ColumnName = "picture")]
		public string Picture { get; set; } = "";

		/// <summary>
		/// Desc: 购买Amount
		/// </summary>
        [SugarColumn(ColumnName = "buynumber")]
		public int? Buynumber { get; set; } = 0;

		/// <summary>
		/// Desc: 单价
		/// </summary>
        [SugarColumn(ColumnName = "price")]
		public float? Price { get; set; } = 0;

		/// <summary>
		/// Desc: Member价
		/// </summary>
        [SugarColumn(ColumnName = "discountprice")]
		public float? Discountprice { get; set; } = 0;

		/// <summary>
		/// Desc: 添加时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
