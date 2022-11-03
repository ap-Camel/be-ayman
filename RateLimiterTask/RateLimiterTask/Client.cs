using System;
using System.Collections.Generic;
using System.Text;

namespace RateLimiterTask
{

    // class for holding information in the latest request from client
    class Client
    {
        public DateTime lastResponseTime { get; set; }
        public int requestCount { get; set; }
    }
}
