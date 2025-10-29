using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Result.Enums
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Chức năng: lấy ra tên của phần tử CodeMessage enum
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static string GetElementNameCodeMessage(this CodeMessage statusCode) =>
            Enum.GetName(typeof(CodeMessage), statusCode).TrimStart('_');

        /// <summary>
        /// Chức năng: cast string to CodeMessage
        /// </summary>
        /// <param name="codeMessage"></param>
        /// <returns></returns>
        public static CodeMessage? GetCodeMessage(this string codeMessage) =>
            Enum.TryParse($"_{codeMessage.RemoveAllSpaceChar()}", out CodeMessage @enum) ? @enum : null;
    }
}
