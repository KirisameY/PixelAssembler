using System;
using System.Threading.Tasks;

using PixelAssembler.GraphElements.GraphNodes;
using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.GUI.GraphMaps;

namespace PixelAssembler.GraphElements.Connections;

public record ValueDataConnection<TFrom, TTo>(IValueNodeOutPort<TFrom> From, IValueNodeInPort<TTo> To, Func<TFrom, TTo> Converter) : IValueConnection<TFrom, TTo>
{
    public Task<TTo>? RequestUpdate(Task startCommand)
    {
        return UpdateRequested?.Invoke(startCommand)?.ContinueWith(task => Converter.Invoke(task.Result));
    }

    public void NotifyUpdated(TFrom newValue)
    {
        var converted = Converter.Invoke(newValue);
        UpdateNotified?.Invoke(converted);
    }

    public Func<Task, Task<TFrom>?>? UpdateRequested { private get; set; }

    public event Action<TTo>? UpdateNotified;

    public void OnConnected()
    {
        if (From.Parent is MainGraphNode node) node.OutPortConnected(From);
    }

    public void OnDisConnected()
    {
        if (To.Parent is MainGraphNode node) node.InPortDisconnected(To);
    }
}