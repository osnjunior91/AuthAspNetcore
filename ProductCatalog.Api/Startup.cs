using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Data.Repository;
using ProductCatalog.Api.Data.Repository.Implementattions;
using ProductCatalog.Api.Security;
using ProductCatalog.Api.Security.Jwt;
using ProductCatalog.Api.Security.Permission;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;


namespace ProductCatalog.Api
{
    public class Startup
    {
        private IConfigurationRoot _configuration;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment { get; }

        public Startup(IHostingEnvironment environment, ILogger<Startup> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();

            var signingConfigurations = new JwtSigningConfiguration();
            services.AddSingleton(signingConfigurations);
            //Adicionado contexto de banco ao startup.cs
            services.AddDbContext<StoreDataContext>(options => options.UseSqlServer(_configuration.GetValue<string>("ConnectionString")));

            var tokenConfigurations = new JwtConfiguration();
            new ConfigureFromConfigurationOptions<JwtConfiguration>(
                    _configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = tokenConfigurations.Issuer,
                                    ValidAudience = tokenConfigurations.Audience,
                                    ClockSkew = TimeSpan.Zero,
                                    IssuerSigningKey = JwtSecurityKey.Create(tokenConfigurations.JwtKey)
                                };

                                options.Events = new JwtBearerEvents
                                {
                                    OnAuthenticationFailed = context =>
                                    {
                                        Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                                        return Task.CompletedTask;
                                    },
                                    OnTokenValidated = context =>
                                    {
                                        Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                                        return Task.CompletedTask;
                                    }
                                };
                            });
            //Adicionado a politica de segurança onde o
            services.AddAuthorization(auth =>
            {
                //Politica para adicionar editar ou remover registro
                auth.AddPolicy("Add", policy=> policy.Requirements.Add(new Method(new List<string>() { "Post", "Put", "Delete" })));
                //politica para busca de dergistros 
                auth.AddPolicy("Find", policy => policy.Requirements.Add(new Method(new List<string>() { "Get" })));
                /*Pelo fato de nao ter uma politica separada para todos os verbos criei usando o IAuthorizationRequirement 
                 * Pois se nao fosse esse o caso poderia usar policy.RequireAssertion e criar o metodo aqui mesmo.
                 */
            });
            services.AddSingleton<IAuthorizationHandler, PermissionByClaim>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            

            
            
            //Chamada do metodo migrate.
            MigrateDatabase();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>());


        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
        private void MigrateDatabase()
        {
            //Verificando se o ambiente e de desenvolvimento
            if (_environment.IsDevelopment())
            {
                try
                {
                    //Criação do objeto de conexão
                    var evolveConnections = new SqlConnection(_configuration.GetValue<string>("ConnectionString"));
                    //Cria um objeto do tipo evolve baseado no objeto de conexão
                    var evolve = new Evolve.Evolve(evolveConnections, msg => Trace.TraceInformation(msg))
                    {
                        //Local onde estão os arquivos do migration
                        Locations = new List<string> { "Data/Migrations" },
                        //Quando hablitada essa opção os dados sao apagados 
                        IsEraseDisabled = false,
                    };
                    //Rodando o Migrate
                    evolve.Migrate();

                }
                catch (Exception ex)
                {
                    _logger.LogCritical("Database migration failed." + ex.Message);
                    throw;
                }
            }

        }
    }
}
