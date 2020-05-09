namespace Scraps.Common
{
    public static class Extensions
    {
        public static string Pluralize(this string singularForm, int howMany)
            => singularForm.Pluralize(howMany, singularForm + "s");

        public static string Pluralize(this string singularForm, int howMany, string pluralForm)
            => howMany == 1 ? singularForm : pluralForm;

        public static bool IsNullOrEmpty(this string input)
            => string.IsNullOrEmpty(input);
    }
}
