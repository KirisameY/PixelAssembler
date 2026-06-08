using Godot;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GUI.GraphMaps;

public partial class MainGraphMap : PaGraphMap
{
    protected override bool OnConnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }

    protected override bool OnDisconnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }
}