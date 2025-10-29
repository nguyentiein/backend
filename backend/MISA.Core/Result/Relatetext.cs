using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Result
{
    public static class Relatetext
    {
        /// <summary>
        /// Chức năng: xoá các kí tự khoảng trắng bị lặp lại (2 kí tự space -> 1 kí tự space)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveSpaceCharacter(this string source) =>
            string.IsNullOrEmpty(source) ? string.Empty : Regex.Replace(source.Trim(), @"\s{2,}", " ");

        /// <summary>
        /// Chức năng: xoá kí tự khoảng trắng bị lặp và viết hoa tất cả
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToUpperAndRemoveSpace(this string source) =>
            RemoveSpaceCharacter(source).ToUpper();

        /// <summary>
        /// Chức năng: thay thế kí tự trong một chuỗi cho trước
        /// </summary>
        /// <param name="source"></param>
        /// <param name="old"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static string ReplaceChar(this string source, char old = ':', char @new = '_') =>
            string.IsNullOrEmpty(source) ? string.Empty : source.Replace(old, @new);

        /// <summary>
        /// Chức năng: làm sạch dữ liệu base64 trước khi sử dụng
        /// </summary>
        /// <param name="base64Data"></param>
        /// <returns></returns>
        public static string GetBase64Data(this string base64Data)
        {
            int base64Index = base64Data.IndexOf("base64,");
            string trimData = base64Data.RemoveSpaceCharacter();

            if (base64Index != -1)
                return trimData.Substring(base64Index + 7);
            else
                return trimData;
        }

        /// <summary>
        /// Chức năng: loại bỏ toàn bộ kí tự khoảng trắng khỏi chuỗi
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveAllSpaceChar(this string text) =>
           string.IsNullOrEmpty(text) ? string.Empty : Regex.Replace(text.Trim(), @"\s+", "");


        /// <summary>
        /// Chức năng: format tiếng việt về chuẩn
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RenewAccentVietnamese(this string source)
        {
            source = !string.IsNullOrEmpty(source) ? source.ToLower() : string.Empty;

            source = Regex.Replace(source, @"(?:òa)", "oà");
            source = Regex.Replace(source, @"(?:óa)", "oá");
            source = Regex.Replace(source, @"(?:ỏa)", "oả");
            source = Regex.Replace(source, @"(?:õa)", "oã");
            source = Regex.Replace(source, @"(?:ọa)", "oạ");

            source = Regex.Replace(source, @"(?:òe)", "oè");
            source = Regex.Replace(source, @"(?:óe)", "oé");
            source = Regex.Replace(source, @"(?:ỏe)", "oẻ");
            source = Regex.Replace(source, @"(?:õe)", "oẽ");
            source = Regex.Replace(source, @"(?:ọe)", "oẹ");

            source = Regex.Replace(source, @"(?:ùy)", "uỳ");
            source = Regex.Replace(source, @"(?:úy)", "uý");
            source = Regex.Replace(source, @"(?:ủy)", "uỷ");
            source = Regex.Replace(source, @"(?:ũy)", "uỹ");
            source = Regex.Replace(source, @"(?:ụy)", "uỵ");

            return source;
        }
    }
    }
