using Hotboil.Mailer.Transports;
using Hotboil.Mailer.Transports.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hotboil.Mailer;

public static class ServicesExtensions
{
    public static MailerBuilder AddMailer(this IServiceCollection services, Action<MailerOptions>? configure = null)
    {
        var mailerBuilder = new MailerBuilder(services);
        
        services.AddOptions<MailerOptions>()
            .BindConfiguration(MailerOptions.SectionName)
            .Configure(configure ?? (_ => { }));
        services.AddTransient<IMailerService, MailerService>();
        
        return mailerBuilder;
    }
}

public class MailerBuilder
{
    public IServiceCollection Services { get; set; }

    public MailerBuilder(IServiceCollection services)
    {
        Services = services;
    }
}

public static class MailerBuilderExtensions
{
    public static MailerBuilder AddSmtp(this MailerBuilder builder, Action<SmtpClientOptions>? configure = null)
    {
        builder.Services.AddOptions<SmtpClientOptions>()
            .BindConfiguration(SmtpClientOptions.SectionName)
            .Configure(configure ?? (_ => { }));
        builder.Services.TryAddScoped<IMailTransport, SmtpMailTransport>();
        return builder;
    }
}