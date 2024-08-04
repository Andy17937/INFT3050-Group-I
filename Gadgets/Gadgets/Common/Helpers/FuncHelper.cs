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
        /// Create a new directory if the file directory does not exist
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
        /// Image to Base64 encoding
        /// </summary>
        /// <param name="imageFullName">Image Full Path</param>
        /// <returns></returns>
        public static string ImageToBase64(string imageFullName)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(imageFullName);
            return Convert.ToBase64String(imageArray);
        }
    }
}
