using System;

namespace RicMonitoringAPI.Infrastructure.Helpers
{
    public static class Calculate
    {
        public static int Age(DateTime dob)
        {
            int age = 0;
            age = DateTime.Now.AddYears(-dob.Year).Year;
            return age;
        }
    }
}
