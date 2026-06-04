using System.Collections.Generic;

using Godot;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.GraphNodes;

public abstract partial class PaGraphNode : GraphNode, IPaGraphNode
{
    public abstract IReadOnlyList<INodeInPort?> InPorts { get; }
    public abstract IReadOnlyList<INodeOutPort?> OutPorts { get; }
}