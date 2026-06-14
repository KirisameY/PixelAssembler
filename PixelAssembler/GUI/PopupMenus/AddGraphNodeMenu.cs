using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using KirisameY.NotifiableCollections.Collections;
using KirisameY.NotifiableCollections.EventArgs;
using KirisameY.Relinq.Extensions;

using PixelAssembler.Data;

namespace PixelAssembler.GUI.PopupMenus;

public partial class AddGraphNodeMenu : PopupPanel
{
    #region Factory

    private AddGraphNodeMenu() { }

    private static PackedScene Scene => field ??= ResourceLoader.Load<PackedScene>("res://GUI/PopupMenus/AddGraphNodeMenu.tscn");

    public static AddGraphNodeMenu CreateInstance(IReadOnlyNotifiableDictionary<string, IReadOnlyNotifiableList<NodeFactory>> nodes)
    {
        var instance = Scene.Instantiate<AddGraphNodeMenu>();
        instance.Nodes = nodes;
        return instance;
    }

    #endregion


    #region SubNodes

    private LineEdit SearchBox => field ??= GetNode<LineEdit>("VBoxContainer/SearchBox");
    private VBoxContainer ItemsContainer => field ??= GetNode<VBoxContainer>("VBoxContainer/ScrollContainer/ItemsContainer");

    #endregion


    #region List & Update

    private readonly Dictionary<object, NodeCreateListSection> _sections = [];

    private IReadOnlyNotifiableDictionary<string, IReadOnlyNotifiableList<NodeFactory>>? Nodes
    {
        get;
        set
        {
            field?.DictionaryUpdated -= DictionaryUpdated;
            field                    =  value;
            value?.DictionaryUpdated += DictionaryUpdated;
            if (value is not null) ResetItems(value);
        }
    }

    private void ResetItems(IReadOnlyDictionary<string, IReadOnlyNotifiableList<NodeFactory>> dict)
    {
        _sections.Values.ForEach(s => s.QueueFree());
        _sections.Clear();
        foreach (var pair in dict)
        {
            var section = NewSection(pair.Key);
            _sections.Add(pair.Key, section);
            foreach (var node in pair.Value)
            {
                section.Add(node);
            }
        }
    }

    private void DictionaryUpdated(object? source, DictionaryUpdateEventArgs<string, IReadOnlyNotifiableList<NodeFactory>> e)
    {
        switch (e)
        {
            case IDictionaryItemAddedEventArgs<string, IReadOnlyNotifiableList<NodeFactory>> add:
            {
                foreach (var (name, list) in add.AddedItems)
                {
                    var section = NewSection(name);
                    _sections.Add(list, section);
                    list.ForEach(section.Add);

                    list.ListUpdated += ListUpdated;
                }
                break;
            }
            case IDictionaryItemRemovedEventArgs<string, IReadOnlyNotifiableList<NodeFactory>> remove:
            {
                foreach (var (_, list) in remove.RemovedItems)
                {
                    if (!_sections.Remove(list, out var section)) continue;
                    section.QueueFree();

                    list.ListUpdated -= ListUpdated;
                }
                break;
            }
            case IDictionaryItemReplacedEventArgs<string, IReadOnlyNotifiableList<NodeFactory>> replace:
            {
                foreach (var info in replace.ItemChanges)
                {
                    if (!_sections.Remove(info.OldValue, out var section))
                    {
                        section = NewSection(info.Key);
                    }
                    _sections[info.NewValue] = section;

                    section.Clear();
                    info.NewValue.ForEach(section.Add);

                    info.OldValue.ListUpdated -= ListUpdated;
                    info.NewValue.ListUpdated += ListUpdated;
                }
                break;
            }
        }
    }

    private void ListUpdated(object? source, ListUpdateEventArgs<NodeFactory> e)
    {
        if (source is null) return;
        if (!_sections.TryGetValue(source, out var section)) return;
        switch (e)
        {
            case IListItemAddedEventArgs<NodeFactory> add:
            {
                var from = add.StartIndex;
                add.AddedItems.ForEach((i, node) => section.Insert(node, from + i));
                break;
            }
            case IListItemClearedEventArgs<NodeFactory>:
            {
                section.Clear();
                break;
            }
            case IListItemRemovedEventArgs<NodeFactory> remove:
            {
                remove.Indexes.OrderDescending().ForEach(i => section.Remove(i));
                break;
            }
            case IListItemReplacedEventArgs<NodeFactory> replace:
            {
                replace.ItemChanges.ForEach(rp => section.ReplaceItem(rp.Index, rp.New));
                break;
            }
            case IListSortedEventArgs<NodeFactory> sort:
            {
                section.Clear();
                sort.ListView.ForEach(node => section.Add(node));
                break;
            }
        }
    }

    private NodeCreateListSection NewSection(string name)
    {
        var result = NodeCreateListSection.Create(name, nodeFac =>
        {
            NodeSelected?.Invoke(this, nodeFac);
            Hide();
        });
        ItemsContainer.AddChild(result);
        return result;
    }

    private NodeCreateListSection GetOrNewSection(string name)
    {
        if (!_sections.TryGetValue(name, out var section))
        {
            section = NewSection(name);
            _sections.Add(name, section);
        }
        return section;
    }

    #endregion


    public event EventHandler<NodeFactory>? NodeSelected;


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