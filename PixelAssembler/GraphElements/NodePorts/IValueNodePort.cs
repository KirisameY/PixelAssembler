using System.Collections.Generic;

using PixelAssembler.GraphElements.Connections;

namespace PixelAssembler.GraphElements.NodePorts;

public interface IValueInPort<T> : INodeInPort, IValueUpdateRequester<T>
{
    public IValueConnectionTo<T>? Connection { get; }
}

public interface IValueOutPort<T> : INodeOutPort, IValueUpdateNotifier<T>
{
    public new IReadOnlyList<IValueConnectionFrom<T>> Connections { get; }
    IReadOnlyList<INodeConnectionFrom> INodeOutPort.Connections => Connections;
}