using KirisameY.NotifiableCollections.Collections;

using PixelAssembler.Data;
using PixelAssembler.GraphElements.GraphNodes.String;

namespace PixelAssembler.Registries;

public static class NodeFactories
{
    public static NotifiableDictionary<string, NotifiableList<NodeFactory>> Main { get; } = new()
    {
        ["String"] =
        [
            new("StringValue", StringValueNode.Create)
        ],
    };
}