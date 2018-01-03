using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureObjectExtensions
    {
        [DebuggerStepThrough]
        public static Param<T> IsNotNull<T>(this Param<T> param) where T : class
        {
            if (param.Value == null)
                throw ExceptionFactory.CreateForParamNullValidation(param, Properties.Ensure.Ensure_IsNotNull);

            return param;
        }
    }
}