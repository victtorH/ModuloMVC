using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ModuloMVC.Context;
using ModuloMVC.Services;
using ModuloMVC.Interfaces;
using ModuloMVC.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TEnancyDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConectandoDb")));

builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<ITarefasRepository, TarefaRepository>();
builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
        options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}
).AddEntityFrameworkStores<TEnancyDB>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
     app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
