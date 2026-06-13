using System;

using CaseConverter;

using Godot;

using KirisameY.GodotExtra.Extensions;

using PixelAssembler.GraphElements.GraphNodes;

namespace PixelAssembler.Data;

public record NodeFactory(string Name, Func<IPaGraphNode> Creator)
{
    public string DisplayName => field ??= Name.ToTitleCase();
    public string Description => field ??= $"${Converters.ToSnakeCase(Name).ToUpper()}_DESC$";
    public Texture2D Icon => field ??= ResourceLoader.LoadOrDefault<Texture2D>(
        $"res://@res/node_icons/{Converters.ToSnakeCase(Name)}.svg",
        defaultValue: new PlaceholderTexture2D { Size = new Vector2(80, 80) }
    );
}