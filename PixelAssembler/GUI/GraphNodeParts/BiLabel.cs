using System;
using System.Diagnostics.CodeAnalysis;

using Godot;

namespace PixelAssembler.GUI.GraphNodeParts;

[Tool]
public partial class BiLabel : Control
{
    #region Factory

    [field: AllowNull, MaybeNull]
    private static PackedScene Scene => field ??= ResourceLoader.Load<PackedScene>("res://GUI/GraphNodeParts/BiLabel.tscn");

    private BiLabel() { }

    public static BiLabel NewInstance() => Scene.Instantiate<BiLabel>();

    #endregion


    #region Properties

    [field: AllowNull, MaybeNull]
    private Label LabelL => field ??= GetNode<Label>("L");

    [field: AllowNull, MaybeNull]
    private Label LabelR => field ??= GetNode<Label>("R");

    // ReSharper disable ConvertNullableToShortForm
    [NotNull] private Nullable<Callable> SetTextL => field ??= Callable.From<string>(value => LabelL.Text = value);
    [NotNull] private Nullable<Callable> SetTextR => field ??= Callable.From<string>(value => LabelR.Text = value);
    [NotNull] private Nullable<Callable> SetStyleL => field ??= Callable.From<LabelSettings?>(value => LabelL.LabelSettings = value);
    [NotNull] private Nullable<Callable> SetStyleR => field ??= Callable.From<LabelSettings?>(value => LabelR.LabelSettings = value);
    // ReSharper restore ConvertNullableToShortForm

    [Export]
    public string TextL
    {
        get;
        set
        {
            field = value;
            SetTextL.Value.CallDeferred(value);
        }
    } = "";

    [Export]
    public string TextR
    {
        get;
        set
        {
            field = value;
            SetTextR.Value.CallDeferred(value);
        }
    } = "";

    [Export]
    public LabelSettings? StyleL
    {
        get;
        set
        {
            field = value;
            SetStyleL.Value.CallDeferred(Variant.From(value));
        }
    }

    [Export]
    public LabelSettings? StyleR
    {
        get;
        set
        {
            field = value;
            SetStyleR.Value.CallDeferred(Variant.From(value));
        }
    }

    [Export]
    public float SplitWidth
    {
        get;
        set
        {
            field = value;
            SubLabelResized();
        }
    } = 12f;

    #endregion


    private void SubLabelResized()
    {
        CustomMinimumSize = new Vector2(
            LabelL.Size.X + LabelR.Size.X + SplitWidth,
            Math.Max(LabelL.Size.Y, LabelR.Size.Y)
        );
    }
}