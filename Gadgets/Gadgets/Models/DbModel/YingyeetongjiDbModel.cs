using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: 营业额统计
    /// </summary>
    [SugarTable("yingyeetongji")]
	public class YingyeetongjiDbModel
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
		/// Desc: 收入
		/// </summary>
		[SugarColumn(ColumnName = "shouru")]
		public string Shouru { get; set; } = "";

		/// <summary>
		/// Desc: 支出
		/// </summary>
		[SugarColumn(ColumnName = "zhichu")]
		public string Zhichu { get; set; } = "";

		/// <summary>
		/// Desc: 营业额
		/// </summary>
		[SugarColumn(ColumnName = "yingyee")]
		public string Yingyee { get; set; } = "";

		/// <summary>
		/// Desc: 添加时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
