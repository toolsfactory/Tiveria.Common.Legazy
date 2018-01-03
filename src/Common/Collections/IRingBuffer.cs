using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace Tiveria.Common.Collections
{
    public interface IRingBuffer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the maximal count of items within the ring buffer.
        /// </summary>
        int Capacity { get; }
    }
}
