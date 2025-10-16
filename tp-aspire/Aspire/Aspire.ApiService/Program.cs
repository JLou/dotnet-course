using Aspire.ApiService;
using Aspire.Persistence;
using Aspire.ServiceDefaults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddDbContext<MyAppContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyAppDb"));
    options.UseSeeding(
        (context, _) =>
        {
            if (!context.Set<Item>().Any())
            {
                context
                    .Set<Item>()
                    .AddRange(
                        new Item { Value = "Value1" },
                        new Item { Value = "Value2" },
                        new Item { Value = "Value3" }
                    );
                context.SaveChanges();
            }
        }
    );

    options.UseAsyncSeeding(
        async (context, _, cancellationToken) =>
        {
            if (!context.Set<Item>().Any())
            {
                context
                    .Set<Item>()
                    .AddRange(
                        new Item { Value = "Value1" },
                        new Item { Value = "Value2" },
                        new Item { Value = "Value3" }
                    );
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    );
});

// Add services to the container.
builder.Services.AddProblemDetails();
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            options.Authority = builder.Configuration["Authentication:OIDC:Authority"];
            options.Audience = builder.Configuration["Authentication:OIDC:Audience"];
            options.RequireHttpsMetadata = false; // in local, we work in http
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                NameClaimType = "name", // map "name" claim to ClaimsPrincipal.Name
                RoleClaimType = "role", // map "role" claim to ClaimsPrincipal.Role
            };
            options.MapInboundClaims = false; // to keep original claim types
        }
    );
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    // Ensure database is created and seeded
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MyAppContext>();
        await context.Database.EnsureCreatedAsync();
    }
}

app.Run();
