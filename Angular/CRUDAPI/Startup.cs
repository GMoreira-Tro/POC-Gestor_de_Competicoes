using CRUDAPI.Models;
using CRUDAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CRUDAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string? connectionString = Configuration.GetConnectionString("ConexaoBD");
            services.AddDbContext<Contexto>(opcoes => opcoes.UseSqlServer(connectionString));

            services.AddHttpClient();
            services.AddScoped<CategoriaService>();
            services.AddScoped<CompeticaoService>();
            services.AddScoped<CompetidorService>();
            services.AddScoped<ConfrontoInscricaoService>();
            services.AddScoped<ConfrontoService>();
            services.AddScoped<GeoNamesService>();
            services.AddScoped<GerencianetService>();
            services.AddScoped<InscricaoService>();
            services.AddScoped<NotificacaoService>();
            services.AddScoped<PremioService>();
            services.AddScoped<UsuarioNotificacaoService>();
            services.AddScoped<UsuarioService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(opcoes => opcoes.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });
        }
    }
}