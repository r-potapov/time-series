using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TimeSeries.Domain.Entities
{
    public class ValidVectorDataAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (!(value is string))
            {
                return true;
            }
            var source = value as string;
            if (string.IsNullOrWhiteSpace(source))
            {
                return true;
            }

            var regex = new Regex(@"(-?\d+([.,]\d)*)+([\s|\r\n|\n]*-?\d+[.,]?\d*)*", RegexOptions.Compiled);
            var match = regex.Match(source);
            return (match.Success && match.Length == source.Length);
        }
    }
}
