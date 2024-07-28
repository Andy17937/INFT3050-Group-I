using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gadgets.Models
{
    /// <summary>
    /// 通用返回Message类
    /// </summary>
    public class MessageModel<T>
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public ResponseCodeEnum Code { get; set; }

        /// <summary>
        /// 返回Message
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Data { get; set; }
    }
}