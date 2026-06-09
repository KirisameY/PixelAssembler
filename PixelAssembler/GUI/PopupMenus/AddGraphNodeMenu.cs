using Godot;

namespace PixelAssembler.GUI.PopupMenus;

public partial class AddGraphNodeMenu : PopupPanel
{
    #region Factory

    private AddGraphNodeMenu() { }

    private static PackedScene Scene => field ??= ResourceLoader.Load<PackedScene>("res://GUI/PopupMenus/AddGraphNodeMenu.tscn");

    public static AddGraphNodeMenu CreateInstance()
    {
        return Scene.Instantiate<AddGraphNodeMenu>();
    }

    #endregion

    #region SubNodes

    private LineEdit SearchBox => field ??= GetNode<LineEdit>("VBoxContainer/SearchBox");

    #endregion

    public override void _Ready()
    {
        base._Ready();

        // temp for testing
        CreateTween().TweenCallback(Callable.From(() =>
        {
            PopupCentered();
        }));
    }
}