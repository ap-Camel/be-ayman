using System;
using System.Collections.Generic;
using System.Text;

namespace RateLimiterTask
{

    // class for mapping configuration options from appsettings.json
    public class Options
    {


        public class EndpointLimit
        {
            public string Endpoint { get; set; }
            public int RequestLimitMs { get; set; }
            public int RequestLimitCount { get; set; }
        }


        public bool RequestLimiterEnabled { get; set; } = true;
        public int DefaultRequestLimitMs { get; set; } = 1000;
        public int DefaultRequestLimitCount { get; set; } = 5;
        public List<EndpointLimit> EndpointLimits { get; set; }

    }
}
