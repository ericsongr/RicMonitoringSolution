using System;
using System.Collections.Generic;

namespace RicEntityFramework.Interfaces
{
    public interface IAccountService
    {
        IList<TimeZoneInfo> SetupTimeZones();
    }
}
