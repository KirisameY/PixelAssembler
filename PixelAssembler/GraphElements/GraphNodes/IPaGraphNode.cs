using System.Collections.Generic;

using Godot;

using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.GUI.GraphMaps;

namespace PixelAssembler.GraphElements.GraphNodes;

public interface IPaGraphNode
{
    public StringName Name { get; }
    public PaGraphMap ParentMap { get; }

    public IReadOnlyList<INodeInPort?> InPorts { get; }
    public IReadOnlyList<INodeOutPort?> OutPorts { get; }
}