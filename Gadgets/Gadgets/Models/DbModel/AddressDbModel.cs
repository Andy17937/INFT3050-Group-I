using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Gadgets.Models.DbModel
{
    /// <summary>
    ///	Desc: Address
    /// </summary>
    [SugarTable("address")]
	public class AddressDbModel
	{           
		/// <summary>
		/// Desc: Primary key id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// Desc: 用户id
		/// </summary>
        [SugarColumn(ColumnName = "userid")]
		public long? Userid { get; set; } = 0;

		/// <summary>
		/// Desc: Address
		/// </summary>
		[SugarColumn(ColumnName = "address")]
		public string Address { get; set; } = "";

		/// <summary>
		/// Desc: 收货人
		/// </summary>
		[SugarColumn(ColumnName = "name")]
		public string Name { get; set; } = "";

		/// <summary>
		/// Desc: 电话
		/// </summary>
		[SugarColumn(ColumnName = "phone")]
		public string Phone { get; set; } = "";

		/// <summary>
		/// Desc: YesNoDefaultAddress[Yes/No]
		/// </summary>
		[SugarColumn(ColumnName = "isdefault")]
		public string Isdefault { get; set; } = "";

		/// <summary>
		/// Desc: Add时间
		/// </summary>
		[SugarColumn(ColumnName = "addtime")]
		public DateTime? Addtime { get; set; }

	}
}
