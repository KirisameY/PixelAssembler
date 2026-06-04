using System.Collections.Generic;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.GraphNodes;

public interface IPaGraphNode
{
    public IReadOnlyList<INodeInPort?> InPorts { get; }
    public IReadOnlyList<INodeOutPort?> OutPorts { get; }
}