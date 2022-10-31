# Task requirement

It is necessary to create **.NET Core** library `RateLimiter` that implements the basic functionality of filtering access to service endpoints based on configurable limits.
The library should be structured in a manner that can be placed on random Nuget repository, thus it is necessary to be **self contained.**

# Functionality

In functional pattern `Ratelimitter` library need to represent **middleware**, which exist in middleware pipeline before *request-specific* middleware library that require business processing of the request header (Auth, CORS, etc.). 
The basic criteria how requests will be restricted is the **incoming IP address**, in another words all access limits will be applied based on IP adress from which request comes.

Library should enable:

**Default limit for all endpoints**

It is necessary to implement a default limit access with all endpoints services:

- Limit on the consecutive number of requests from the same IP address to a specific endpoint in the appropriate time frame - `DefaultRequestLimitCount`,
- Time frame for the number of requests from the same IP address to a specific endpoint (in milliseconds) - `DefaultRequestLimitMs`,
- In case of exceeding the limit, `the error 429 - Too Many Requests` should be returned to the user,
- **It is not necessary** to implement standardization for rate-limit header fields (`Retry-After, X-Limit`- ect.)

**Example:**

For the  `DefaultRequestLimitCount = 5 `and `DefaultRequestLimitMs = 1000` values, a user from one IP adress is allowed to send **5 requests** to endpoint services **within 1 second.**

**Extra credits – not necessary for library review – Limit for one specific endpiint**

It is necessary to implement a limit for access to the **specific endpoint** of the service:

- Limit on the consecutive number of requests from the same IP address to a specific endpoint in the appropriate time frame - `RequestLimitCount`,
- Time frame for the number of requests from the same IP address to a specific endpoint (in milliseconds) - `RequestLimitMs`,
- In case of exceeding the limit, `the error 429 - Too Many Requests` should be returned to the user,
- It is not necessary to implement standardization for rate-limit header fields (`Retry-After, X-Limit`- etc.)

# Service configuration

|Parameter|	Description|	Value|
|---------| -----------|----------|
|RequestLimiterEnabled|	Includes rate limiter functionality|	boolean|
|DefaultRequestLimitMs| Default time frame on the number of requests for all endpoints|integer|
|DefaultRequestLimitCount|Limit on the consecutive number of requests for all endpoints|integer|	
| EndpointLimits*|	Limit list for specific endpoint |	
EndpointLimits/Endpoint*|	Specific endpoint line trace|	string
EndpointLimits/RequestLimitMs*|	Default timeframe to the number of endpoint requests| integer|
EndpointLimits/RequestLimitCount*|Limit on the number of consecutive requests in a time frame for an endpoint | integer|

*If implemented extra credits for the task
  
**Configuration example:**

```
"RateLimiter": {
  "RequestLimiterEnabled": true,
  "DefaultRequestLimitMs": 1000,
  "DefaultRequestLimitCount": 10,
  "EndpointLimits": [```
    {
      "Endpoint": "/api/products/books",
      "RequestLimitMs": 1000,
      "RequestLimitCount": 1
    },
    {
      "Endpoint": "/api/products/pencils",
      "RequestLimitMs": 500,
      "RequestLimitCount": 2
    }
  ]
} 
```

# Prerequisites for the project
- [.NET Core 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- Adequate [README.md](https://github.com/Ripley07/BE-Recruiting-Rate-limiter-English-version/blob/main/Readme.md) with configuration description and method how to initialize library into the project. 

# Remarks
- Ignore production grade optimization, project will be used for candidates evaluation only
- Expected time for finishing the task – 4 working hours

