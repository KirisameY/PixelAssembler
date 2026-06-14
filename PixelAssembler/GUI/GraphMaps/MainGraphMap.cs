using Godot;

using KirisameY.GenericUtils;
using KirisameY.NotifiableCollections.Collections;

using PixelAssembler.Data;
using PixelAssembler.GraphElements.GraphNodes.String;
using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.GUI.PopupMenus;

namespace PixelAssembler.GUI.GraphMaps;

public partial class MainGraphMap : PaGraphMap
{
    protected override IReadOnlyNotifiableDictionary<string, IReadOnlyNotifiableList<NodeFactory>> NodeFactories => field ??=
        Registries.NodeFactories.Main.AsType(TypeA.Of<IReadOnlyNotifiableList<NodeFactory>>());

    protected override bool OnConnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }

    protected override bool OnDisconnectionRequest(INodeOutPort from, INodeInPort to)
    {
        return true;
    }
}