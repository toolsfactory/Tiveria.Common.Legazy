using System;
using System.Diagnostics;

namespace Tiveria.Common.Ensure
{
    public static class EnsureGuidExtensions
    {
        [DebuggerStepThrough]
        public static Param<Guid> IsNotEmpty(this Param<Guid> param)
        {
            if (Guid.Empty.Equals(param.Value))
                throw ExceptionFactory.CreateForParamValidation(param, Properties.Ensure.Ensure_IsEmptyGuid);

            return param;
        }
    }
}