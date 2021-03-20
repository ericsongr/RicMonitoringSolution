using Mustache;

namespace RicCommon
{
    public static class Templater
    {
        public static string ReplaceHtml(string template, object source)
        {
            var generator = new HtmlFormatCompiler().Compile(template);
            return generator.Render(source);
        }

        public static string ReplaceText(string template, object source)
        {
            var generator = new FormatCompiler().Compile(template);
            return generator.Render(source);
        }
    }
}
