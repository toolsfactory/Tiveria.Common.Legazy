using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureNullableValueTypeExtensions
    {
        [DebuggerStepThrough]
        public static Param<T?> IsNotNull<T>(this Param<T?> param) where T : struct
        {
            if (param.Value == null || !param.Value.HasValue)
                throw ExceptionFactory.CreateForParamNullValidation(param, Properties.Ensure.Ensure_IsNotNull);

            return param;
        }
    }
}