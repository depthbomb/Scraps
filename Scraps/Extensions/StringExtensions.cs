#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2021  Caprine Logic

/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.

/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
/// GNU General Public License for more details.

/// You should have received a copy of the GNU General Public License
/// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion License

using System.Text;

namespace Scraps.Extensions
{
    public static class StringExtensions
    {
        public static string Repeat(this string str, int count) => new StringBuilder(str.Length * count).Insert(0, str, count).ToString();

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static string Pluralize(this string singularForm, int howMany) => singularForm.Pluralize(howMany, singularForm + "s");

        public static string Pluralize(this string singularForm, int howMany, string pluralForm) => howMany == 1 ? singularForm : pluralForm;
    }
}
