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

public class ValueInPort<T> : IValueNodeInPort<T>
{
    public ValueInPort(IValueType type, PaGraphNode parent, uint index)
    {
        if (typeof(T) != type.NativeType) throw new CreatingValueDataPortTypeMismatchException(type, typeof(T));
        Type   = type;
        Parent = parent;
        Index  = index;
    }

    public IValueType Type { get; }
    public PaGraphNode Parent { get; }
    public uint Index { get; }


    [field: AllowNull, MaybeNull]
    IReadOnlyList<INodeConnection> INodeInPort.ConnectionsFrom => field ??= new Nullable2List<INodeConnection>(() => ConnectionFrom);

    public IValueConnectionTo<T>? ConnectionFrom { get; private set; }

    public bool TryCreateConnectionFrom(INodeOutPort from, [NotNullWhen(true)] out INodeConnection? connection)
    {
        connection = null;
        if (from is not IValueNodeOutPort valueOutPort) return false;

        if (!IValueType.TryGetConversion(valueOutPort.Type, Type, out var conversion)) return false;
        connection = conversion.CreateConnection(valueOutPort, this);

        return true;
    }

    public bool AddConnection(INodeConnection connection)
    {
        if (ConnectionFrom is not null) return false;
        if (connection is not IValueConnectionTo<T> valueConnection) return false;
        ConnectionFrom = valueConnection;

        valueConnection.UpdateNotified += value => UpdateNotified?.Invoke(value);
        return true;
    }

    public bool RemoveConnection(INodeConnection connection)
    {
        if (ConnectionFrom != connection) return false;
        ConnectionFrom = null;

        return true;
    }


    public Task<T>? RequestUpdate(Task startCommand)
    {
        return ConnectionFrom?.RequestUpdate(startCommand);
    }

    public event Action<T>? UpdateNotified;
}

public class ValueOutPort<T> : IValueNodeOutPort<T>
{
    public ValueOutPort(IValueType type, PaGraphNode parent, uint index)
    {
        if (typeof(T) != type.NativeType) throw new CreatingValueDataPortTypeMismatchException(type, typeof(T));
        Type   = type;
        Parent = parent;
        Index  = index;
    }


    public IValueType Type { get; }

    public PaGraphNode Parent { get; }

    public uint Index { get; }


    private readonly List<IValueConnectionFrom<T>> _connections = [];

    [field: AllowNull, MaybeNull]
    public IReadOnlyList<IValueConnectionFrom<T>> ConnectionsTo => field ??= _connections.AsReadOnly();

    public bool AddConnection(INodeConnection connection)
    {
        if (connection is not IValueConnectionFrom<T> valueConnection) return false;
        _connections.Add(valueConnection);

        valueConnection.UpdateRequested = s => UpdateRequested?.Invoke(s);
        return true;
    }

    public bool RemoveConnection(INodeConnection connection)
    {
        if (connection is not IValueConnectionFrom<T> valueConnection) return false;
        if (!_connections.Remove(valueConnection)) return false;

        return true;
    }


    public void NotifyUpdated(T newValue)
    {
        ConnectionsTo.ForEach(connection => connection.NotifyUpdated(newValue));
    }

    public Func<Task, Task<T>?>? UpdateRequested { private get; set; }
}

public class CreatingValueDataPortTypeMismatchException(IValueType valueType, Type nativeType) : Exception(
    $"Native type of creating ValuePort ({nativeType}) mismatched with ValueType {valueType}"
);