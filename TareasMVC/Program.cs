using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TareasMVC;
using Microsoft.AspNetCore.Mvc.Razor;
using TareasMVC.Servicios;

var builder = WebApplication.CreateBuilder(args);




//AUTORIZAR SOLO USUARIOS REGISTRADOS
var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

// Add services to the container. & AGREGAMOS LA AUTORIZACION 
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
}).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(opciones =>
    {   //TRADUCIR LAS ANOTACIONES DE DATOS
        opciones.DataAnnotationLocalizerProvider = (_, factoria) =>
            factoria.Create(typeof(RecursoCompartido));
    });

// CONFIGURAR EL DbContext COMO UN SERVICIO 
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));




//ACTIVAR AUTENTICACION 
//AUTENTICACION CON CUENTA DE MICROSOFT
builder.Services.AddAuthentication().AddMicrosoftAccount(opciones =>
{
    opciones.ClientId = builder.Configuration["MicrosoftClientId"];
    opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
});

//ACTIVAR SERVICIOS DE Identity 
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.SignIn.RequireConfirmedAccount = false;

}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//AUTENTICACION BASADA EN COOKIES
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/usuarios/login";
    opciones.AccessDeniedPath= "/usuarios/login";
});


//LOCALIZACIÓN EN EL PROYECTO (para utilizar varios idiomas)
builder.Services.AddLocalization(opciones =>
{
    //Agregacion de los recursos (en este caso idioma diferente)
    opciones.ResourcesPath = "Recursos";
});


//CONFIGURAR EL SERVICIO DE USUARIOS
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();

//CONFIGURAR EL SERVICIO DE AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

//AGREGAR DISTINTOS IDIOMAS AL PROYECTO
//LOCALIZAR LAS PETICIONES DEL USUARIO
app.UseRequestLocalization(opciones =>
{
    // PONER IDIOMA POR DEFAULT (indicar)
    opciones.DefaultRequestCulture = new RequestCulture("es");

    opciones.SupportedUICultures = Constantes.CulturasUISoportadas
        .Select(cultura => new CultureInfo(cultura.Value)).ToList();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//COLOCACION DE MIDDLEWARE 
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
