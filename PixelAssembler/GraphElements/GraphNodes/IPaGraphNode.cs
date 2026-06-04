using System.Collections.Generic;

using Godot;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.GraphNodes;

public interface IPaGraphNode
{
    public StringName Name { get; }

    public IReadOnlyList<INodeInPort?> InPorts { get; }
    public IReadOnlyList<INodeOutPort?> OutPorts { get; }
}