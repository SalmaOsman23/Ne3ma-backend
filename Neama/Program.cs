using Neama;

var builder = WebApplication.CreateBuilder(args);

//// Add cookie policy
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.Strict;
//    options.Secure = CookieSecurePolicy.Always; // Ensure cookies are only sent over HTTPS
//});

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//// Use cookie policy
//app.UseCookiePolicy();

app.UseHttpsRedirection();

app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
