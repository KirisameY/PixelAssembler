using System.Collections.Generic;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.Types.ValueTypes;

namespace PixelAssembler.GraphElements.NodePorts;

public interface IValueNodePort : INodePort
{
    public IValueType Type { get; }
}

public interface IValueNodeInPort : IValueNodePort, INodeSingleInPort;

public interface IValueNodeInPort<T> : IValueNodeInPort, IValueUpdateRequester<T>
{
    public new IValueConnectionTo<T>? ConnectionFrom { get; }
    INodeConnection? INodeSingleInPort.ConnectionFrom => ConnectionFrom;
}

public interface IValueNodeOutPort : IValueNodePort, INodeOutPort;

public interface IValueNodeOutPort<T> : IValueNodeOutPort, IValueUpdateNotifier<T>
{
    public new IReadOnlyList<IValueConnectionFrom<T>> ConnectionsTo { get; }
    IReadOnlyList<INodeConnection> INodeOutPort.ConnectionsTo => ConnectionsTo;
}