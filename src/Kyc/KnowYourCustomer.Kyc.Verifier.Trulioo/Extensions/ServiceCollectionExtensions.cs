using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Verifier.Contracts.Options;
using KnowYourCustomer.Kyc.Verifier.Trulioo.Verifiers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KnowYourCustomer.Kyc.Verifier.Trulioo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVerifierServices(this IServiceCollection services, IConfiguration configuration)
        {
            Guard.IsNotNull(services, nameof(services));

            services.Configure<VerificationProviderOptions>(configuration.GetSection(VerificationProviderOptions.DefaultSectionName));

            var verificationProviderOptions = configuration.GetSection(VerificationProviderOptions.DefaultSectionName).Get<VerificationProviderOptions>();

            if (verificationProviderOptions.ProviderType.Equals("api"))
            {
                services.AddScoped<IVerifier, TruliooApiVerifier>();
            }
            else
            {
                services.AddScoped<IVerifier, TruliooSdkVerifier>();
            }

            services.AddHttpClient("verifier", c =>
            {
                c.BaseAddress = new Uri(verificationProviderOptions.Url);
                c.DefaultRequestHeaders.Add(verificationProviderOptions.HeaderKey, verificationProviderOptions.Token);
            });

            return services;
        }
    }
}