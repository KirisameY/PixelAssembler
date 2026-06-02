using System;

using Godot;

namespace PixelAssembler.GUI.GraphMaps;

public partial class MainGraphMap : BaseGraphMap
{
    protected override bool OnConnectionRequest(GraphNode from, long fromPort, GraphNode to, long toPort)
    {
        return true;
    }
    protected override bool OnDisconnectionRequest(GraphNode from, long fromPort, GraphNode to, long toPort)
    {
        return true;
    }
}