using Godot;

namespace PixelAssembler.GUI.GraphMaps;

public abstract partial class BaseGraphMap : GraphEdit
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
}