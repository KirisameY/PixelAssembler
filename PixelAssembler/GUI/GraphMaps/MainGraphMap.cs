using Godot;

using KirisameY.NotifiableCollections.Collections;

using PixelAssembler.Data;
using PixelAssembler.GraphElements.GraphNodes.String;
using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.GUI.PopupMenus;

namespace PixelAssembler.GUI.GraphMaps;

public partial class MainGraphMap : PaGraphMap
{
    protected override IReadOnlyNotifiableDictionary<string, IReadOnlyNotifiableList<NodeFactory>> NodeFactories => field ??= new NotifiableDictionary<string, IReadOnlyNotifiableList<NodeFactory>>
    {
        ["Asd"] = new NotifiableList<NodeFactory>
        {
            new("AsdFdsE", StringValueNode.Create)
        }
    };

    protected override bool OnConnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }

    protected override bool OnDisconnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }
}