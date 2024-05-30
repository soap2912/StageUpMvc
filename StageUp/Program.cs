using StageUp.DAL;
using StageUp.Models.Interfaces;
using StageUp.Services;

namespace StageUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adiciona servi�os ao cont�iner.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(); // Adiciona suporte para sess�o
            builder.Services.AddHttpContextAccessor(); // Adiciona o HttpContextAccessor

            // Registra implementa��es

            builder.Services.AddScoped<IEnderecoDAL, EnderecoDAL>(); // Substitua EnderecoDAL pela implementa��o real que voc� tem
            builder.Services.AddScoped<IUsuarioDAL, UsuarioDAL>();
            builder.Services.AddScoped<IEmpresaDAL, EmpresaDAL>();
            builder.Services.AddScoped<IApis, Apis>();
            builder.Services.AddScoped<ISetorDAl, SetorDAL>();
            builder.Services.AddScoped<IRamoDAL, RamoDAL>();

            var app = builder.Build();
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

            app.UseSession(); // Adiciona o uso da sess�o ao pipeline


            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=CadastroEmpresa}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
