//https://github.com/lukencode/FluentEmail/blob/master/src/FluentEmail.Core/Defaults/ReplaceRenderer.cs

using System.Reflection;

namespace Hotboil.Mailer.TemplateEngines.SimpleTemplateEngine;

public class SimpleTemplateEngine : ITemplateEngine
{
    public string Parse<T>(string template, T model, bool isHtml = true)
    {
        if (model == null)
        {
            return template;
        }
        
        foreach (PropertyInfo pi in model.GetType().GetRuntimeProperties())
        {
            if (pi.GetValue(model, null) == null)
            {
                continue;
            }
            
            template = template.Replace($"##{pi.Name}##", pi.GetValue(model, null)?.ToString());
        }

        return template;            
    }

    public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
    {
        return Task.FromResult(Parse(template, model, isHtml));
    }
}