using Inventario.Domain.Repositories;
using Inventario.Infraestructure.Models;
using Inventario.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();  
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddDbContext<InventarioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<Jwt>();
builder.Services.AddScoped<ILote,LoteService>();
builder.Services.AddScoped<IProducto,ProductosServices>();
builder.Services.AddScoped<IUsuario,UsuarioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



// builder.Host.UseSerilog(SeriLogger.Configure);

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//
// }).AddIdentityServerAuthentication("Bearer", options =>
// {
//     options.Authority = builder.Configuration.GetConnectionString("IdentityServerAuthentication");
//     options.JwtValidationClockSkew = TimeSpan.Zero;
// });


app.UseHttpsRedirection();

// app.UseSerilogRequestLogging();
app.UseAuthorization();
app.MapControllers();


app.UseCors();

app.Run();
