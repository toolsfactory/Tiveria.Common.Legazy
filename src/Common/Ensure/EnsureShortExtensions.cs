using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureShortExtensions
    {
        [DebuggerStepThrough]
        public static Param<short> IsLowerThan(this Param<short> param, short limit)
        {
            if (param.Value >= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<short> IsLowerOrEqual(this Param<short> param, short limit)
        {
            if (!(param.Value <= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<short> IsGreaterThan(this Param<short> param, short limit)
        {
            if (param.Value <= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<short> IsGreaterOrEqual(this Param<short> param, short limit)
        {
            if (!(param.Value >= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<short> IsInRange(this Param<short> param, short min, short max)
        {
            if (param.Value < min)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToLow.Inject(param.Value, min));

            if (param.Value > max)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToHigh.Inject(param.Value, max));

            return param;
        }
    }
}