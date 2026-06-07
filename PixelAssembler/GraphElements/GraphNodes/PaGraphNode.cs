using System.Collections.Generic;

using Godot;

using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.GUI.GraphMaps;

namespace PixelAssembler.GraphElements.GraphNodes;

public abstract partial class PaGraphNode : GraphNode, IPaGraphNode
{
    public PaGraphMap ParentMap => GetParent<PaGraphMap>();
    public abstract IReadOnlyList<INodeInPort?> InPorts { get; }
    public abstract IReadOnlyList<INodeOutPort?> OutPorts { get; }
}