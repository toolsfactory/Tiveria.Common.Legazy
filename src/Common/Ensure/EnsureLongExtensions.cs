using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureLongExtensions
    {
        [DebuggerStepThrough]
        public static Param<long> IsLowerThan(this Param<long> param, long limit)
        {
            if (param.Value >= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<long> IsLowerOrEqual(this Param<long> param, long limit)
        {
            if (!(param.Value <= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<long> IsGreaterThan(this Param<long> param, long limit)
        {
            if (param.Value <= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<long> IsGreaterOrEqual(this Param<long> param, long limit)
        {
            if (!(param.Value >= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<long> IsInRange(this Param<long> param, long min, long max)
        {
            if (param.Value < min)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToLow.Inject(param.Value, min));

            if (param.Value > max)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToHigh.Inject(param.Value, max));

            return param;
        }
    }
}