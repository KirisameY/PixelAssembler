using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.Connections;

public interface IValueConnectionFrom<T> : INodeConnectionFrom, IValueUpdateNotifier<T>
{
    public new IValueOutPort<T> From { get; }
    INodeOutPort INodeConnectionFrom.From => From;
}

public interface IValueConnectionTo<T> : INodeConnectionTo, IValueUpdateRequester<T>
{
    public new IValueInPort<T> To { get; }
    INodeInPort INodeConnectionTo.To => To;
}

public interface IValueConnection<TFrom, TTo> : INodeConnection, IValueConnectionFrom<TFrom>, IValueConnectionTo<TTo>;