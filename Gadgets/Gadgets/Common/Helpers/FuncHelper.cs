using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gadgets.Common.Helpers
{
    public static class FuncHelper
    {
        /// <summary>
        /// 文件目录如果不存在，就创建一个新的目录
        /// </summary>
        /// <param name="path"></param>
        public static void DicCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 图片转Base64编码
        /// </summary>
        /// <param name="imageFullName">图片完整路径</param>
        /// <returns></returns>
        public static string ImageToBase64(string imageFullName)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(imageFullName);
            return Convert.ToBase64String(imageArray);
        }
    }
}
