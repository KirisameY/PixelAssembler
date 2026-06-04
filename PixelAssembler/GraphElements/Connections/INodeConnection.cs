using System;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.Connections;

public interface INodeConnectionFrom
{
    public INodeOutPort From { get; }
}

public interface INodeConnectionFrom<in T> : INodeConnectionFrom, IUpdateNotifier<T>
{
    public new INodeOutPort<T> From { get; }
    INodeOutPort INodeConnectionFrom.From => From;
}

public interface INodeConnectionTo
{
    public INodeInPort To { get; }
}

public interface INodeConnectionTo<out T> : INodeConnectionTo, IUpdateRequester<T>
{
    public new INodeInPort<T> To { get; }
    INodeInPort INodeConnectionTo.To => To;
}

public interface INodeConnection : INodeConnectionFrom, INodeConnectionTo;

public interface INodeConnection<in TFrom, out TTo> : INodeConnection, INodeConnectionFrom<TFrom>, INodeConnectionTo<TTo>;