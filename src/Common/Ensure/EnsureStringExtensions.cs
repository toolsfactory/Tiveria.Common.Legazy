using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Tiveria.Common.Ensure
{
    public static class EnsureStringExtensions
    {
        [DebuggerStepThrough]
        public static Param<string> IsNotNullOrWhiteSpace(this Param<string> param)
        {
            if (string.IsNullOrWhiteSpace(param.Value))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotNullOrWhiteSpace);

            return param;
        }

        [DebuggerStepThrough]
        public static Param<string> IsNotNullOrEmpty(this Param<string> param)
        {
            if (string.IsNullOrEmpty(param.Value))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotNullOrEmpty);

            return param;
        }

        [DebuggerStepThrough]
        public static Param<string> HasLengthBetween(this Param<string> param, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(param.Value))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotNullOrEmpty);

            var length = param.Value.Length;

            if (length < minLength)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToShort.Inject(minLength, maxLength, length));

            if (length > maxLength)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToLong.Inject(minLength, maxLength, length));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<string> Matches(this Param<string> param, string match)
        {
            return Matches(param, new Regex(match));
        }

        [DebuggerStepThrough]
        public static Param<string> Matches(this Param<string> param, Regex match)
        {
            if (!match.IsMatch(param.Value))
            {
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_NoMatch.Inject(param.Value, match));
            }
            return param;
        }
    }
}
