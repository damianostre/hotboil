// https://github.com/lukencode/FluentEmail/blob/master/src/FluentEmail.Core/EmbeddedResourceHelper.cs

using System.Reflection;

namespace Hotboil.Mailer;

internal static class EmbeddedResourceHelper
{
    internal static string GetResourceAsString(Assembly assembly, string path)
    {
        using var stream = assembly.GetManifestResourceStream(path);
        using var reader = new StreamReader(stream);
        var result = reader.ReadToEnd();

        return result;
    }
}