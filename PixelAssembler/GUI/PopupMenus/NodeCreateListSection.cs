using System;

using Godot;

using PixelAssembler.Data;
using PixelAssembler.Misc;

namespace PixelAssembler.GUI.PopupMenus;

public partial class NodeCreateListSection : FoldableContainer
{
    #region Factory

    private NodeCreateListSection() { }
    private static PackedScene Scene => field ??= ResourceLoader.Load<PackedScene>("res://GUI/PopupMenus/NodeCreateListSection.tscn");

    public static NodeCreateListSection Create(string title = "", Action<NodeFactory>? selected = null)
    {
        var instance = Scene.Instantiate<NodeCreateListSection>();
        instance.Title     =  title;
        instance.Activated += selected;
        return instance;
    }

    #endregion


    #region SubNodes

    private ItemList ItemList => field ??= GetNode<ItemList>("ItemList");

    public override void _Ready()
    {
        ItemList.ItemActivated += Send;
        ItemList.ItemSelected  += Send;
        return;

        void Send(long index)
        {
            var nf = (GdWrapper<NodeFactory>)ItemList.GetItemMetadata((int)index);
            Activated?.Invoke(nf.Value);
        }
    }

    #endregion

    public event Action<NodeFactory>? Activated;


    public void Add(NodeFactory item)
    {
        var i = ItemList.AddItem(item.DisplayName, item.Icon);
        ItemList.SetItemTooltip(i, item.Description);
        ItemList.SetItemMetadata(i, GdWrapper.New(item));
    }

    public void Insert(NodeFactory item, int index)
    {
        Add(item);
        ItemList.MoveItem(ItemList.ItemCount - 1, index);
    }

    public void Remove(int index)
    {
        ItemList.RemoveItem(index);
    }

    public void Clear()
    {
        ItemList.Clear();
    }

    public void ReplaceItem(int index, NodeFactory item)
    {
        ItemList.SetItemIcon(index, item.Icon);
        ItemList.SetItemText(index, item.DisplayName);
        ItemList.SetItemTooltip(index, item.Description);
        ItemList.SetItemMetadata(index, GdWrapper.New(item));
    }
}