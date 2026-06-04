using System;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.Connections;

public interface INodeConnection
{
    public INodeOutPort From { get; }
    public INodeInPort To { get; }
}
