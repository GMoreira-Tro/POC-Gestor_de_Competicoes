using CRUDAPI.Models;
using CRUDAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
            services.AddScoped<EmailService>();
            services.AddScoped<JwtService>();

            // Configuração do CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            // Configuração de Autenticação JWT
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            services.AddControllers();

            // Configuração do Swagger com suporte a autenticação JWT
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CRUD API",
                    Version = "v1",
                    Description = "API para gerenciar competições",
                    Contact = new OpenApiContact
                    {
                        Name = "Guilherme dos Santos Moreira",
                        Email = "guilherme.dsmoreira@gmail.com",
                        Url = new Uri("https://seusite.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Adiciona suporte ao JWT no Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu_token}"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
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

            app.UseStaticFiles();
            // Configura a pasta "uploads" para servir arquivos
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.WebRootPath, "uploads")),
                RequestPath = "/uploads"
            });
            app.UseHttpsRedirection();
            app.UseRouting();

            // Aplicando a política de CORS corretamente
            app.UseCors("AllowSpecificOrigin");

            // Configuração da autenticação e autorização
            app.UseAuthentication();  // ⬅️ Agora autenticamos os usuários
            app.UseAuthorization();   // ⬅️ Agora aplicamos as permissões

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configuração do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
