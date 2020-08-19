using System;

namespace RicModel.RoomRent.Dtos
{
    public class MonthlyRentBatchDto : MonthlyRentBatch
    {
        public string ProcessStartDateTimeString =>
            ProcessStartDateTime.ToString("MM/dd/yyyy h:mm tt");

        public string ProcesssEndDateTimeString =>
            ProcesssEndDateTime.HasValue
                ? ProcesssEndDateTime.Value.ToString("MM/dd/yyyy h:mm tt")
                : "";

        public TimeSpan Duration => ProcesssEndDateTime.HasValue
            ? (ProcessStartDateTime - ProcesssEndDateTime.Value).Duration()
            : TimeSpan.Zero;

    }
}
