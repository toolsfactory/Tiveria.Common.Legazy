using System;
using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureDateTimeExtensions
    {
        [DebuggerStepThrough]
        public static Param<DateTime> IsLowerThan(this Param<DateTime> param, DateTime limit)
        {
            if (param.Value >= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<DateTime> IsLowerOrEqual(this Param<DateTime> param, DateTime limit)
        {
            if (!(param.Value <= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<DateTime> IsGreaterThan(this Param<DateTime> param, DateTime limit)
        {
            if (param.Value <= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<DateTime> IsGreaterOrEqual(this Param<DateTime> param, DateTime limit)
        {
            if (!(param.Value >= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<DateTime> IsInRange(this Param<DateTime> param, DateTime min, DateTime max)
        {
            if (param.Value < min)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToLow.Inject(param.Value, min));

            if (param.Value > max)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToHigh.Inject(param.Value, max));

            return param;
        }
    }
}