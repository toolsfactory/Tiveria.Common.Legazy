using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureDoubleExtensions
    {
        [DebuggerStepThrough]
        public static Param<double> IsLowerThan(this Param<double> param, double limit)
        {
            if (param.Value >= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<double> IsLowerOrEqual(this Param<double> param, double limit)
        {
            if (!(param.Value <= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotLte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<double> IsGreaterThan(this Param<double> param, double limit)
        {
            if (param.Value <= limit)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGt.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<double> IsGreaterOrEqual(this Param<double> param, double limit)
        {
            if (!(param.Value >= limit))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotGte.Inject(param.Value, limit));

            return param;
        }

        [DebuggerStepThrough]
        public static Param<double> IsInRange(this Param<double> param, double min, double max)
        {
            if (param.Value < min)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToLow.Inject(param.Value, min));

            if (param.Value > max)
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsNotInRange_ToHigh.Inject(param.Value, max));

            return param;
        }
    }
}