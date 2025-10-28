using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Result
{
    public sealed class ResponseMessage
    {
        #region Property
        public Dictionary<string, string> Values { get; set; }
        #endregion
    }
    public enum CodeMessage
    {
        _98,
        _99,
        _101,
        _102,
        _103,
        _104,
        _105,
        _106,
        _107,
        _108,
        _200,
        _209,
        _210,
        _211,
        _212,
        _213,
        _214,
        _215,
        _216,
        _218,
        _219,
        _220,
        _221,
        _222,
        _223,
        _224,
        _225,
        _226,
        _227,
        _228,
        _229,
        _230,
        _231,
        _232,
        _233,
        _234,
        _235,
        _236,
        _237,
        _238,
        _239,
        _240,
        _241,
        _242,
        _243,
        _244,
        _245,
        _246,
        _300,
        _401,
    }
}
