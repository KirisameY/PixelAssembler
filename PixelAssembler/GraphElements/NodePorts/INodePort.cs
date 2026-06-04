using System.Collections.Generic;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.GraphNodes;
using PixelAssembler.Types.ValueTypes;

namespace PixelAssembler.GraphElements.NodePorts;

public interface INodePort
{
    public IValueType Type { get; }
    public PaGraphNode Parent { get; }
    public uint Index { get; }
}

public interface INodeInPort : INodePort
{
    public INodeConnectionTo? Connection { get; }
}

public interface INodeInPort<out T> : INodeInPort, IUpdateRequester<T>
{
    public new INodeConnectionTo<T>? Connection { get; }
    INodeConnectionTo? INodeInPort.Connection => Connection;
}

public interface INodeOutPort : INodePort
{
    public IReadOnlyList<INodeConnectionFrom> Connections { get; }
}

public interface INodeOutPort<in T> : INodeOutPort, IUpdateNotifier<T>
{
    public new IReadOnlyList<INodeConnectionFrom<T>> Connections { get; }
    IReadOnlyList<INodeConnectionFrom> INodeOutPort.Connections => Connections;
}