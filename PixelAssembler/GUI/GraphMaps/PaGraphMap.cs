using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Godot;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.GraphNodes;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GUI.GraphMaps;

public abstract partial class PaGraphMap : GraphEdit
{
    public sealed override void _Ready()
    {
        ConnectionRequest += (from, fromPortI, to, toPortI) =>
        {
            var fromNode = GetNode<IPaGraphNode>(from.ToString());
            var toNode = GetNode<IPaGraphNode>(to.ToString());
            var fromPort = fromNode.OutPorts[(int)fromPortI];
            var toPort = toNode.InPorts[(int)toPortI];
            if ((fromPort, toPort) is (not null, not null) && OnConnectionRequest(fromPort, toPort) && !fromNode.IsAfter(toNode))
                ConnectPort(fromPort, toPort);
        };
        DisconnectionRequest += (from, fromPortI, to, toPortI) =>
        {
            var fromNode = GetNode<IPaGraphNode>(from.ToString());
            var toNode = GetNode<IPaGraphNode>(to.ToString());
            var fromPort = fromNode.OutPorts[(int)fromPortI];
            var toPort = toNode.InPorts[(int)toPortI];
            if ((fromPort, toPort) is (not null, not null) && OnDisconnectionRequest(fromPort, toPort))
                DisconnectPort(fromPort, toPort);
        };

        AfterReady();
    }

    protected virtual void AfterReady() { }

    protected abstract bool OnConnectionRequest(INodeOutPort from, INodeInPort to);
    protected abstract bool OnDisconnectionRequest(INodeOutPort from, INodeInPort to);


    private readonly Dictionary<(INodeOutPort from, INodeInPort to), INodeConnection> _connections = [];
    [field: AllowNull, MaybeNull]
    public new IReadOnlyCollection<INodeConnection> Connections => field ??= _connections.Values;

    public bool ConnectPort(INodeOutPort from, INodeInPort to)
    {
        if (!to.TryCreateConnectionFrom(from, out var connection)) return false;

        // release single port
        Action? reconnectOld = null;
        if (from is INodeSingleOutPort singleOutPort)
        {
            var cn = singleOutPort.ConnectionTo;
            if (cn is not null)
            {
                Disconnect(cn);
                reconnectOld += () => ConnectPort(cn.From, cn.To);
            }
        }
        if (to is INodeSingleInPort singleInPort)
        {
            var cn = singleInPort.ConnectionFrom;
            if (cn is not null)
            {
                Disconnect(cn);
                reconnectOld += () => ConnectPort(cn.From, cn.To);
            }
        }

        // rollback on fall
        if (!from.AddConnection(connection) || !to.AddConnection(connection) ||
            !_connections.TryAdd((connection.From, connection.To), connection))
        {
            from.RemoveConnection(connection);
            to.RemoveConnection(connection);
            reconnectOld?.Invoke();
            return false;
        }

        ConnectNode(from.Parent.Name, (int)from.Index, to.Parent.Name, (int)to.Index);

        connection.OnConnected();

        return true;
    }

    public bool DisconnectPort(INodeOutPort from, INodeInPort to)
    {
        if (!_connections.Remove((from, to), out var connection)) return false;

        DisconnectNode(from.Parent.Name, (int)from.Index, to.Parent.Name, (int)to.Index);

        connection.From.RemoveConnection(connection);
        connection.To.RemoveConnection(connection);

        connection.OnDisConnected();

        return true;
    }

    public bool Disconnect(INodeConnection connection) => DisconnectPort(connection.From, connection.To);
}