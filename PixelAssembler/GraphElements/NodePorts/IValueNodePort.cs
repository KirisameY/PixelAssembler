using System.Collections.Generic;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.Types.ValueTypes;

namespace PixelAssembler.GraphElements.NodePorts;

public interface IValuePort : INodePort
{
    public IValueType Type { get; }
}

public interface IValueInPort : IValuePort, INodeSingleInPort;

public interface IValueInPort<T> : IValueInPort, IValueUpdateRequester<T>
{
    public new IValueConnectionTo<T>? ConnectionFrom { get; }
    INodeConnection? INodeSingleInPort.ConnectionFrom => ConnectionFrom;
}

public interface IValueOutPort : IValuePort, INodeOutPort;

public interface IValueOutPort<T> : IValueOutPort, IValueUpdateNotifier<T>
{
    public new IReadOnlyList<IValueConnectionFrom<T>> ConnectionsTo { get; }
    IReadOnlyList<INodeConnection> INodeOutPort.ConnectionsTo => ConnectionsTo;
}