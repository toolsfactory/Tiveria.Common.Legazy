using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureDecimalExtensions
    {
        [DebuggerStepThrough]
        public static Param<decimal> IsLowerThan(this Param<decimal> param, decimal limit)
        {
            if (param.Value >= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<decimal> IsLowerOrEqual(this Param<decimal> param, decimal limit)
        {
            if (!(param.Value <= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<decimal> IsGreaterThan(this Param<decimal> param, decimal limit)
        {
            if (param.Value <= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<decimal> IsGreaterOrEqual(this Param<decimal> param, decimal limit)
        {
            if (!(param.Value >= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<decimal> IsInRange(this Param<decimal> param, decimal min, decimal max)
        {
            if (param.Value < min)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToLow.Inject(param.Value, min));

            if (param.Value > max)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToHigh.Inject(param.Value, max));

            return param;
        }
    }
}