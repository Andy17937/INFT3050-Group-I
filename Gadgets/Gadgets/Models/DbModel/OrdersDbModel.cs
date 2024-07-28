using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: 订单
    /// </summary>
    [SugarTable("orders")]
	public class OrdersDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: Order #
		/// </summary>
        [SugarColumn(ColumnName = "orderid", IsOnlyIgnoreUpdate = true)]
		public string Orderid { get; set; } = "";

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
		/// Desc: Item图片
		/// </summary>
		[SugarColumn(ColumnName = "picture")]
		public string Picture { get; set; } = "";

		/// <summary>
		/// Desc: Item Amount
		/// </summary>
        [SugarColumn(ColumnName = "buynumber")]
		public int? Buynumber { get; set; } = 0;

		/// <summary>
		/// Desc: Price/Credit
		/// </summary>
        [SugarColumn(ColumnName = "price")]
		public float? Price { get; set; } = 0;

		/// <summary>
		/// Desc: Discount Price
		/// </summary>
        [SugarColumn(ColumnName = "discountprice")]
		public float? Discountprice { get; set; } = 0;

		/// <summary>
		/// Desc: Total Price/Credit
		/// </summary>
        [SugarColumn(ColumnName = "total")]
		public float? Total { get; set; } = 0;

		/// <summary>
		/// Desc: Total Discount Price
		/// </summary>
        [SugarColumn(ColumnName = "discounttotal")]
		public float? Discounttotal { get; set; } = 0;

		/// <summary>
		/// Desc: Payment Method
		/// </summary>
        [SugarColumn(ColumnName = "type")]
		public int? Type { get; set; } = 0;

		/// <summary>
		/// Desc: Status
		/// </summary>
		[SugarColumn(ColumnName = "status")]
		public string Status { get; set; } = "";

		/// <summary>
		/// Desc: Address
		/// </summary>
		[SugarColumn(ColumnName = "address")]
		public string Address { get; set; } = "";

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
