﻿namespace Hotboil.Mailer.TemplateEngines;

public interface ITemplateEngine
{
    Task<string> ParseAsync<T>(string template, T model, bool isHtml = true);
    string Parse<T>(string template, T model, bool isHtml = true);
}