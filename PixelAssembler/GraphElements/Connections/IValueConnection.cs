using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.Connections;

public interface IValueConnectionFrom<T> : INodeConnection, IValueUpdateNotifier<T>
{
    public new IValueOutPort<T> From { get; }
    INodeOutPort INodeConnection.From => From;
}

public interface IValueConnectionTo<T> : INodeConnection, IValueUpdateRequester<T>
{
    public new IValueInPort<T> To { get; }
    INodeInPort INodeConnection.To => To;
}

public interface IValueConnection<TFrom, TTo> : IValueConnectionFrom<TFrom>, IValueConnectionTo<TTo>;