using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ShopThoiTrang.Common
{
    public static class Encryptor // mã hóa password
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            // tính toán băm từ các byte văn bản
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            // lấy kết quả băm sau khi tính toán
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                // thay đổi nó thành 2 chữ số thập lục phân
                // cho mỗi byte 
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }  
    }
}