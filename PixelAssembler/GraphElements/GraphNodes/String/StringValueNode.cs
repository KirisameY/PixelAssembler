using System;
using System.Collections.Generic;
using System.Threading;

using Godot;

using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.Types.ValueTypes.BasicTypes;

namespace PixelAssembler.GraphElements.GraphNodes.String;

public partial class StringValueNode : MainGraphNode
{
    #region Ports

    private IValueNodeInPort<string> InPort => field ??= CreateInPort<string>(
        0, BaseValueType.String,
        s => Interlocked.Exchange(ref _inputValue, s) != s);

    private IValueNodeOutPort<string> OutPort => field ??= CreateOutPort<string>(
        0, BaseValueType.String, () => Value
    );

    private string? _inputValue;
    private string _editValue = "";

    private string Value => InPort.Connected ? _inputValue ?? "" : _editValue;


    public override IReadOnlyList<INodeInPort?> InPorts => field ??= [InPort];

    public override IReadOnlyList<INodeOutPort?> OutPorts => field ??= [OutPort];

    #endregion


    #region Godot

    // [Export]
    private TextEdit TextEdit => field ??= GetNode<TextEdit>("TextEdit");
    // {
    //     get => field ?? throw new NullReferenceException(nameof(TextEdit));
    //     set;
    // }

    private void OnTextEditOnTextChanged()
    {
        _editValue = TextEdit.Text;
        StartUpdate();
    }

    #endregion


    protected override bool WhenInPortDisconnected(IValueNodeInPort port)
    {
        if (port != InPort) return false;
        _inputValue = null;
        return true;
    }

    protected override bool Update()
    {
        return true;
    }

    protected override void GuiUpdate()
    {
        if (InPort.Connected)
        {
            TextEdit.Editable = false;
            TextEdit.Text     = _inputValue;
        }
        else if (!TextEdit.Editable)
        {
            TextEdit.Editable = true;
            TextEdit.Text     = _editValue;
        }
    }
}