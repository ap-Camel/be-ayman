using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiterTask
{
    public class RateLimiterMiddleware
    {
        
        private int RequestLimitCount;
        private int RequestLimitMs;

        // hashtable for holding key/value pair key ==> concatination of endpoint and client ip, value ==> client request time and count
        public static Hashtable clientTable;

        private readonly RequestDelegate _next;

        private readonly IOptions<Options> _config;

        public RateLimiterMiddleware(RequestDelegate next, IOptions<Options> config)
        {
            _next = next;
            clientTable = new Hashtable();
            _config = config;

            // setting the default values even if there is no configurations set
            RequestLimitCount = _config.Value.DefaultRequestLimitCount;
            RequestLimitMs = _config.Value.DefaultRequestLimitMs;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // checking if rateLimitting is enabled
            if (!_config.Value.RequestLimiterEnabled)
            {
                await _next(context);
            }

            // creating key ==> concatination of request path and ip address
            string key = $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";

            // checking if there are configurations for specific route
            foreach (Options.EndpointLimit item in _config.Value.EndpointLimits)
            {
                if (item.Endpoint == context.Request.Path)
                {
                    RequestLimitCount = item.RequestLimitCount;
                    RequestLimitMs = item.RequestLimitMs;
                    break;
                }
            }

            // checking if first time making this request
            if (clientTable.ContainsKey(key))
            {
                Client clientStatistics = (Client)clientTable[key];

                // checking if last request time is within the requestLimitMs
                if (DateTime.Now < clientStatistics.lastResponseTime.AddMilliseconds(RequestLimitMs))
                {

                    // updating client request values
                    clientStatistics.requestCount += 1;
                    clientStatistics.lastResponseTime = DateTime.Now;
                    clientTable[key] = clientStatistics;

                    // checking if requestLimitCount has been reached
                    if (clientStatistics.requestCount >= RequestLimitCount)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        return;
                    }
                }

                // if the time limit expiered, reset the client request values
                else
                {

                    // updating client request values
                    clientStatistics.requestCount = 1;
                    clientStatistics.lastResponseTime = DateTime.Now;
                    clientTable[key] = clientStatistics;
                }
            }
            else
            {
                clientTable.Add(key, new Client() { lastResponseTime = DateTime.Now, requestCount = 1 });
                await _next(context);
            }
        }

    }

    public static class RateLimiterMiddlewareExtentions
    {
        public static IApplicationBuilder UseRateLimiterMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimiterMiddleware>();
        }
    }
}
