using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Register用户
    /// </summary>
    [SugarTable("defaultuser")]
	public class DefaultuserDbModel
	{           
		/// <summary>
		/// Desc: 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: Username
		/// </summary>
		[SugarColumn(ColumnName = "username")]
		public string Username { get; set; } = "";

		/// <summary>
		/// Desc: Password
		/// </summary>
		[SugarColumn(ColumnName = "mima")]
		public string Mima { get; set; } = "";

		/// <summary>
		/// Desc: Name
		/// </summary>
		[SugarColumn(ColumnName = "name")]
		public string Name { get; set; } = "";

		/// <summary>
		/// Desc: 性别
		/// </summary>
		[SugarColumn(ColumnName = "sex")]
		public string Sex { get; set; } = "";

		/// <summary>
		/// Desc: 年龄
		/// </summary>
        [SugarColumn(ColumnName = "age")]
		public int? Age { get; set; } = 0;

		/// <summary>
		/// Desc: 电话
		/// </summary>
		[SugarColumn(ColumnName = "phone")]
		public string Phone { get; set; } = "";

		/// <summary>
		/// Desc: 照片
		/// </summary>
		[SugarColumn(ColumnName = "picture")]
		public string Picture { get; set; } = "";

		/// <summary>
		/// Desc: Email
		/// </summary>
		[SugarColumn(ColumnName = "email")]
		public string Email { get; set; } = "";

		/// <summary>
		/// Desc: Balance
		/// </summary>
        [SugarColumn(ColumnName = "money")]
		public float? Money { get; set; } = 0;

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
