using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hotboil.Mailer;

public static class ServicesExtensions
{
    public static IServiceCollection AddMailer(
        this IServiceCollection services, IConfiguration configuration, Action<MailerOptions> options)
    {
        services.Configure<MailerOptions>(configuration.GetSection(MailerOptions.SectionName));
        services.AddTransient<IMailerService, MailerService>();
        services.AddFluentEmail().AddMailKitSender()
        return services;
    }
}