using MvcWithApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DbHelper>(items =>
    new DbHelper(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PartyRepository>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();   // ✅ enable serving CSS/JS from wwwroot
app.UseRouting();
app.UseAuthorization();

// ✅ Swagger available at /swagger, not default page
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Party API V1");
    options.RoutePrefix = "swagger"; // Swagger UI only at /swagger
});

// ✅ MVC routing takes precedence for /
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
