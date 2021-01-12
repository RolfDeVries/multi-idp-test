# multi-idp-test
Purpose: Create a single backend for multiple idps.

## Getting started
Open solution in VS2019 and hit F5.

What is does is:
- Nuget will restore all packages
- Build and launch all projects using .net 5.0
- Opens 5 browsers: 
	- Backend= https://localhost:5666
	- Client1= https://localhost:5001
	- Idsrv1 = https://localhost:5051
	- Client2= https://localhost:5002
	- Idsrv2 = https://localhost:5052

Testing:	
- Login with username alice and password alice
- Go to the privacy tab and check if you got a reply from the backend.

## How is the solution setup
The solution is setup by following the guides below

https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html
https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html
https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-5.0

**IMPORTANT: The solution is not setup using best practices**

The trick for multiple idps is found in de `api4both\startup.cs`
- Remove default scheme
- Override the default policy and add all authentication schemas

``` csharp
            services.AddAuthentication() //remove default scheme here
               .AddJwtBearer("Bearer1", options =>
               {
                   options.Authority = "https://localhost:5051";

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = true,
                       ValidAudience = "https://localhost:5051/resources"
                   };
               })
               .AddJwtBearer("Bearer2", options =>
               {
                   options.Authority = "https://localhost:5052";

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = true,
                       ValidAudience = "https://localhost:5052/resources"
                   };
               });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes("Bearer1", "Bearer2") //add all schemas here
                            .RequireAuthenticatedUser()
                            .Build();
            });
        }
```