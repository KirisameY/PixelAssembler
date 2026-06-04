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
    public IReadOnlyList<INodeConnectionTo> Connections { get; }

    internal bool AddConnection(INodeConnectionTo connection);
    internal bool RemoveConnection(INodeConnectionTo connection);
}

public interface INodeOutPort : INodePort
{
    public IReadOnlyList<INodeConnectionFrom> Connections { get; }

    internal bool AddConnection(INodeConnectionFrom connection);
    internal bool RemoveConnection(INodeConnectionFrom connection);
}