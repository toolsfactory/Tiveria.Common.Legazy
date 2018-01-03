using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tiveria.Common.MVP
{
    public interface IViewModelValidation : IDataErrorInfo
    {
        /// <summary>
        /// Gets the messages.
        /// </summary>
        Dictionary<string, string> ValidationMessages { get; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is validating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is validating; otherwise, <c>false</c>.
        /// </value>
        bool IsValidating { get; }
        /// <summary>
        /// provides the result of the last validation eg. no ValidationMessages = true
        /// </summary>
        bool ValidationOk { get; }
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        void Validate();
        /// <summary>
        /// Occurs when [validated].
        /// </summary>
        event EventHandler Validated;
        /// <summary>
        /// Occurs when [validating].
        /// </summary>
        event EventHandler<CancelEventArgs> Validating;
    }
}
