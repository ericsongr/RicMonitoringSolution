using System;

namespace RicMonitoringAPI.Infrastructure.Helpers
{
    public static class Calculate
    {
        public static int Age(DateTime dob)
        {
            var currentDate = DateTime.Now;
            if (currentDate.Year.Equals(dob.Year))
                return 0;
            
            int age = 0;
            age = currentDate.AddYears(-dob.Year).Year;

            if (currentDate.Month < dob.Month || (currentDate.Month == dob.Month && currentDate.Day < dob.Day))
            {
                age--;
            }

            return age;
        }
    }
}
