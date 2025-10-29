using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.BusinessLogic.Exceptions
{
    public class ValidateException: Exception
    {
        private string _errorMessage;
        public ValidateException(string msg)
        {
            _errorMessage = msg;
        }

        public override string Message => _errorMessage;
    }
}
