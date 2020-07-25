using RicModel.Helpers;
using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent.Dtos.Audits
{
    public class AuditRenterDto : AuditRenter
    {
        public string RoomName => Room.Name;

        public string AdvancePaidDateString  => AdvancePaidDate.ToString("dd-MMM-yyyy");

        public string StartDateString => StartDate.ToString("dd-MMM-yyyy");

        public string AuditDateTimeString => AuditDateTime.ToString("dd-MMM-yyyy h:mm tt");

        public string DueDayString => $"{DueDay}{CommonFunctions.GetSuffix(DueDay.ToString())}";

        public string DateEndString => IsEndRent ? DateEndRent?.ToString("dd-MMM-yyyy") : "";

        public string BalancePaidDateString => BalancePaidDate?.ToString("dd-MMM-yyyy");
        
        public string NextDueDateString => NextDueDate.ToString("dd-MMM-yyyy");

        public string PreviousDueDateString => PreviousDueDate.ToString("dd-MMM-yyyy");

    }
}
