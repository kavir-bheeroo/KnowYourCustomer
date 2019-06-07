using AutoMapper;
using KnowYourCustomer.Common.Messaging.Kafka.Extensions;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Data.EfCore;
using KnowYourCustomer.Kyc.Data.EfCore.Repositories;
using KnowYourCustomer.Kyc.Mappers;
using KnowYourCustomer.Kyc.MrzProcessor.Abbyy.Processors;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Services;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Verifier.Trulioo.Verifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KnowYourCustomer.Kyc.Host
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
            var connectionString = Configuration.GetConnectionString("KycDb");

            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<KycDbContext>(options =>
                {
                    options.UseSqlServer(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                });

            CreateDatabase(services.BuildServiceProvider());

            services.AddScoped<IKycRepository, KycRepository>();
            services.AddScoped<IKycDocumentRepository, KycDocumentRepository>();
            services.AddScoped<IKycOperationRepository, KycOperationRepository>();

            services.AddScoped<IMrzProcessor, AbbyyMrzProcessor>();
            services.AddScoped<IKycService, KycService>();
            services.AddScoped<IVerifier, TruliooApiVerifier>();

            services.AddKafkaProducer<string, InitiateKycResponseModel>(Configuration);

            services.AddAutoMapper(typeof(MappingProfile), typeof(Mappers.MappingProfile));

            services.AddHttpClient("trulioo", c =>
            {
                c.BaseAddress = new Uri("https://gateway.trulioo.com/");
                c.DefaultRequestHeaders.Add("x-trulioo-api-key", "58dacf1c48b6f2828dd6f46e7054415b");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetValue<string>("IdentityServerUrl");
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "kyc";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }

        public void CreateDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var db = serviceProvider.GetService<KycDbContext>())
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}