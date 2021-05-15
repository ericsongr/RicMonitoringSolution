using RicEntityFramework.Helpers;

namespace RicEntityFramework.Parameters
{
    public class RoomResourceParameters : BaseResourceParameters
    {
        public int AccountId { get; set; }
        public int RenterId { get; set; }
    }
}
