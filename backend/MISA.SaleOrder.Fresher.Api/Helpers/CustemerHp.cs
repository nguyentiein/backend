using SalesManagement.BusinessLogic.Interfaces.Repository;
using SalesManagement.BusinessLogic.Result;

namespace SalesManagement.Api.Helpers
{
    public class CustemerHp
    {
        public ICustomerRepo customerRepo;
        public CustemerHp(ICustomerRepo _customerRepo)
        {
            this.customerRepo = _customerRepo;
        }
        public  string GenerateCustomerCode()
        {
            string prefix = "KH" + DateTime.Now.ToString("yyyyMM");

            string latestCode = customerRepo.GetLatestCustomerCode(prefix);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(latestCode) && latestCode.Length >= 14)
            {
                string lastNumberStr = latestCode.Substring(8);
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            string newCode = prefix + nextNumber.ToString("D6");
            return newCode;
        }
    }
}
