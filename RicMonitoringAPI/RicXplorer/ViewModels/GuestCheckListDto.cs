using System.Collections.Generic;

namespace RicMonitoringAPI.RicXplorer.ViewModels
{
    public class GuestCheckListDto
    {
        public int GuestBookingDetailId { get; set; }
        public List<GuestCheckListDetailDto> GuestCheckListDetails { get; set; }
    }
}
