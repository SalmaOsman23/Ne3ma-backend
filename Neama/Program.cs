using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ne3ma.Helper;
using Ne3ma.Services;
using Neama;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register dependencies first
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite() 
    )
);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});

builder.Services.AddAuthorization();

// ✅ Register other dependencies
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// ✅ Ensure proper middleware order
app.UseHttpsRedirection();
app.UseAuthentication();  // ✅ Ensure it's only called once
app.UseAuthorization();

// ✅ Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Database Seeding (AFTER `builder.Build()`)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.SeedSuperAdmin(context);
}

app.MapControllers();
app.Run();
