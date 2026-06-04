using System;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.Connections;

public interface INodeConnectionFrom
{
    public INodeOutPort From { get; }
}

public interface INodeConnectionTo
{
    public INodeInPort To { get; }
}

public interface INodeConnection : INodeConnectionFrom, INodeConnectionTo;
