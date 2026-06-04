using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using KirisameY.Relinq.Extensions;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.GraphNodes;
using PixelAssembler.Misc;
using PixelAssembler.Types.ValueTypes;

namespace PixelAssembler.GraphElements.NodePorts;

public class ValueInPort<T>(IValueType type, PaGraphNode parent, uint index) : IValueInPort<T>
{
    public IValueType Type => type;
    public PaGraphNode Parent => parent;
    public uint Index => index;

    [field: AllowNull, MaybeNull]
    IReadOnlyList<INodeConnectionTo> INodeInPort.Connections => field ??= new Nullable2List<INodeConnectionTo>(() => Connection);

    public IValueConnectionTo<T>? Connection { get; private set; }

    public bool AddConnection(INodeConnectionTo connection)
    {
        if (Connection is not null) return false;
        if (connection is not IValueConnectionTo<T> valueConnection) return false;
        Connection = valueConnection;

        valueConnection.UpdateNotified += value => UpdateNotified?.Invoke(value);
        return true;
    }

    public bool RemoveConnection(INodeConnectionTo connection)
    {
        if (Connection != connection) return false;
        Connection = null;

        return true;
    }


    public Task<T>? RequestUpdate()
    {
        return Connection?.RequestUpdate();
    }

    public event Action<T>? UpdateNotified;
}

public class ValueOutPort<T>(IValueType type, PaGraphNode parent, uint index, IReadOnlyList<IValueConnectionFrom<T>> connections) : IValueOutPort<T>
{
    public IValueType Type { get; } = type;
    public PaGraphNode Parent { get; } = parent;
    public uint Index { get; } = index;


    private readonly List<IValueConnectionFrom<T>> _connections = [];

    public IReadOnlyList<IValueConnectionFrom<T>> Connections
    {
        get => field ??= _connections.AsReadOnly();
    } = connections;

    public bool AddConnection(INodeConnectionFrom connection)
    {
        if (connection is not IValueConnectionFrom<T> valueConnection) return false;
        _connections.Add(valueConnection);

        valueConnection.UpdateRequested = () => UpdateRequested?.Invoke();
        return true;
    }

    public bool RemoveConnection(INodeConnectionFrom connection)
    {
        if (connection is not IValueConnectionFrom<T> valueConnection) return false;
        if (!_connections.Remove(valueConnection)) return false;

        return true;
    }


    public void NotifyUpdated(T newValue)
    {
        Connections.ForEach(connection => connection.NotifyUpdated(newValue));
    }

    public Func<Task<T>?>? UpdateRequested { private get; set; }
}