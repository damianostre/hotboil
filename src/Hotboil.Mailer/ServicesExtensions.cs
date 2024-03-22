using Hotboil.Mailer.Transports.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hotboil.Mailer;

public static class ServicesExtensions
{
    public static MailerBuilder AddMailer(this IServiceCollection services)
    {
        var mailerBuilder = new MailerBuilder(services);
        
        services.AddOptions<MailerOptions>().BindConfiguration(MailerOptions.SectionName);
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

public class MailerBuilderExtensions
{
    public static MailerBuilder AddSmtp(this MailerBuilder builder, Action<SmtpClientOptions> configure)
    {
        builder.Services.AddOptions<SmtpClientOptions>().BindConfiguration(SmtpClientOptions.SectionName);   
        builder.Services.TryAdd(ServiceDescriptor.Scoped<IMailTransport>(
            _ => new SmtpMailTransport()));
        return builder;
    }
}