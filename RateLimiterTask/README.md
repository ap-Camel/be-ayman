# RateLimitTask

- RateLimitTask is a C# class library for setting rate limit / throttling to an api
- All access limits are applied base on the ip address of the incoming request

# Setup and Configuration

- Download the project
- Add the project solution to your existing project
- Reference this project in the project that you want to use this library for
- you must also add the following code in the setup.cs file in your project 
```cs

 public void ConfigureServices(IServiceCollection services)
        {

            // add this ling of code in the ConfigureServices section
            services.Configure<Options>(Configuration.GetSection("RateLimiter"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiDemo02", Version = "v1" });
            });
        }

```

```cs

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDemo02 v1"));
            }


            //// add this line of code for using the ratelimiterMiddleware before any of the other middlewares **it must be the first
            app.UseRateLimiterMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

```

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

  
**Configuration example:**

```json
"RateLimiter": {
    "RequestLimiterEnabled": true,
    "DefaultRequestLimitMs": 1000,
    "DefaultRequestLimitCount": 10,
    "EndpointLimits": [
      {
        "Endpoint": "/WeatherForecast",
        "RequestLimitMs": 120000,
        "RequestLimitCount": 2
      }
    ]
  }
```




