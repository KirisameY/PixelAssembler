using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.GraphNodes;

namespace PixelAssembler.GraphElements.NodePorts;

public interface INodePort
{
    public PaGraphNode Parent { get; }
    public uint Index { get; }
}

public interface INodeInPort : INodePort
{
    public IReadOnlyList<INodeConnection> ConnectionsFrom { get; }

    public bool TryCreateConnectionFrom(INodeOutPort from, [NotNullWhen(true)]out INodeConnection? connection);

    public bool AddConnection(INodeConnection connection);
    public bool RemoveConnection(INodeConnection connection);
}

public interface INodeSingleInPort : INodeInPort
{
    public INodeConnection? ConnectionFrom { get; }
}

public interface INodeOutPort : INodePort
{
    public IReadOnlyList<INodeConnection> ConnectionsTo { get; }

    internal bool AddConnection(INodeConnection connection);
    internal bool RemoveConnection(INodeConnection connection);
}

public interface INodeSingleOutPort : INodeOutPort
{
    public INodeConnection? ConnectionTo { get; }
}