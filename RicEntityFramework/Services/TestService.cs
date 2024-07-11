using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace RicEntityFramework.Services
{
    public class TestService
    {
        public int Compute(int a, int b)
        {
            return a + b;
        }
        public DateTime LastPingDate()
        {
            return DateTime.Now;
        }

        public PingOptions GetPingOptions()
        {
            return new PingOptions
            {
                DontFragment = true,
                Ttl = 1
            }; 
        }

        public IEnumerable<PingOptions> MostRecentPings()
        {
            IEnumerable<PingOptions> pingOptions = new[]
            {
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 1
                },
            };

            return pingOptions;
        }
    }
}
