using Microsoft.EntityFrameworkCore;
using MvcNetCoreSessionEmpleados.Data;
using MvcNetCoreSessionEmpleados.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//agregar session
builder.Services.AddDistributedMemoryCache();//va con decoraciones
builder.Services.AddSession();

//habilitamos la memoria cache personalizada
builder.Services.AddMemoryCache();//IMemoryCache va con ese objeto y yo le digo que guardar de forma explicita


string connectionString = builder.Configuration.GetConnectionString("SqlHospital");
builder.Services.AddTransient<RepositoryEmpleados>();
builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
