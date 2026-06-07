using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KirisameY.Relinq.Extensions;

namespace PixelAssembler.GraphElements.NodePorts;

public static class ValueNodePortExtensions
{
    extension<T>(IValueNodeInPort<T> port)
    {
        public IValueNodeInPort<T> WithUpdateNotified(params IEnumerable<Action<T>> handlers)
        {
            handlers.ForEach(h => port.UpdateNotified += h);
            return port;
        }
    }

    extension<T>(IValueNodeOutPort<T> port)
    {
        public IValueNodeOutPort<T> WithUpdateRequested(Func<Task, Task<T>?>? handler = null)
        {
            port.UpdateRequested = handler;
            return port;
        }
    }
}