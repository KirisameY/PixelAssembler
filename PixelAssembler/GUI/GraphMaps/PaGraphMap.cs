using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Godot;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GUI.GraphMaps;

public abstract partial class PaGraphMap : GraphEdit
{
    public sealed override void _Ready()
    {
        ConnectionRequest += (from, fromPort, to, toPort) =>
        {
            var fromNode = GetNode<GraphNode>(from.ToString());
            var toNode = GetNode<GraphNode>(to.ToString());
            if (OnConnectionRequest(fromNode, fromPort, toNode, toPort)) ConnectNode(from, (int)fromPort, to, (int)toPort);
        };
        DisconnectionRequest += (from, fromPort, to, toPort) =>
        {
            var fromNode = GetNode<GraphNode>(from.ToString());
            var toNode = GetNode<GraphNode>(to.ToString());
            if (OnDisconnectionRequest(fromNode, fromPort, toNode, toPort)) DisconnectNode(from, (int)fromPort, to, (int)toPort);
        };
        AfterReady();
    }

    protected virtual void AfterReady() { }

    protected abstract bool OnConnectionRequest(GraphNode from, long fromPort, GraphNode to, long toPort);
    protected abstract bool OnDisconnectionRequest(GraphNode from, long fromPort, GraphNode to, long toPort);


    private readonly Dictionary<(INodeOutPort from, INodeInPort to), INodeConnection> _connections = [];
    [field: AllowNull, MaybeNull]
    public new IReadOnlyCollection<INodeConnection> Connections => field ??= _connections.Values;

    public bool ConnectPort(INodeOutPort from, INodeInPort to)
    {
        if (!to.TryCreateConnectionFrom(from, out var connection)) return false;
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

        if (!from.AddConnection(connection) || !to.AddConnection(connection) ||
            !_connections.TryAdd((connection.From, connection.To), connection))
        {
            from.RemoveConnection(connection);
            to.RemoveConnection(connection);
            reconnectOld?.Invoke();
            return false;
        }

        ConnectNode(from.Parent.Name, (int)from.Index, to.Parent.Name, (int)to.Index);

        // todo: after-connection update here

        return true;
    }

    public bool DisconnectPort(INodeOutPort from, INodeInPort to)
    {
        if (!_connections.Remove((from, to), out var connection)) return false;

        DisconnectNode(from.Parent.Name, (int)from.Index, to.Parent.Name, (int)to.Index);

        connection.From.RemoveConnection(connection);
        connection.To.RemoveConnection(connection);

        // todo: after-disconnection update here

        return true;
    }

    public bool Disconnect(INodeConnection connection) => DisconnectPort(connection.From, connection.To);
}