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

            // Configuração do CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // Se precisar enviar cookies ou autenticação
                });
            });

            services.AddControllers();

            // Configuração do Swagger com mais detalhes
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CRUD API",
                    Version = "v1",
                    Description = "API para gerenciar competições",
                    Contact = new OpenApiContact
                    {
                        Name = "Seu Nome",
                        Email = "seuemail@email.com",
                        Url = new Uri("https://seusite.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Aplicando a política de CORS corretamente
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configuração do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API V1");
                c.RoutePrefix = string.Empty; // Faz com que o Swagger seja carregado na raiz (http://localhost:5000/)
            });
        }
    }
}