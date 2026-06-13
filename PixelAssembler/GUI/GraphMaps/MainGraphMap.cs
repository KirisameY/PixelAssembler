using Godot;

using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.GUI.PopupMenus;

namespace PixelAssembler.GUI.GraphMaps;

public partial class MainGraphMap : PaGraphMap
{
    protected override AddGraphNodeMenu AddNodeMenu => field ??= GetNode<AddGraphNodeMenu>("AddGraphNodeMenu");

    protected override bool OnConnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }

    protected override bool OnDisconnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }
}