using RicEntityFramework.Helpers;

namespace RicEntityFramework.Parameters
{
    public class RentTransactionResourceParameters : BaseResourceParameters
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}