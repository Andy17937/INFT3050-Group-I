using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Staff
    /// </summary>
    [SugarTable("yuangong")]
	public class YuangongDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: Staff号
		/// </summary>
		[SugarColumn(ColumnName = "yuangonghao")]
		public string Yuangonghao { get; set; } = "";

		/// <summary>
		/// Desc: Password
		/// </summary>
		[SugarColumn(ColumnName = "mima")]
		public string Mima { get; set; } = "";

		/// <summary>
		/// Desc: Phone
		/// </summary>
		[SugarColumn(ColumnName = "shouji")]
		public string Shouji { get; set; } = "";

		/// <summary>
		/// Desc: Email
		/// </summary>
		[SugarColumn(ColumnName = "youxiang")]
		public string Youxiang { get; set; } = "";

		/// <summary>
		/// Desc: Name
		/// </summary>
		[SugarColumn(ColumnName = "xingming")]
		public string Xingming { get; set; } = "";

		/// <summary>
		/// Desc: Name
		/// </summary>
		[SugarColumn(ColumnName = "shenfenzheng")]
		public string Shenfenzheng { get; set; } = "";

		/// <summary>
		/// Desc: Balance
		/// </summary>
        [SugarColumn(ColumnName = "money")]
		public float? Money { get; set; } = 0;

		/// <summary>
		/// Desc: 添加时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
