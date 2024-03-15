using Microsoft.Extensions.DependencyInjection;

namespace Hotboil.Mailer;

public static class ServicesExtensions
{
    public static IServiceCollection AddMailer(this IServiceCollection services)
    {
        services.AddOptions<MailerOptions>().BindConfiguration(MailerOptions.SectionName);
        services.AddTransient<IMailerService, MailerService>();
        services
            .AddFluentEmail("")
            .AddMailKitSender(MailerOptions.SmtpSectionName);
        return services;
    }
}