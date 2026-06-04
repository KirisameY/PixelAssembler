using System;
using System.Threading.Tasks;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.GraphElements.Connections;

public record ValueDataConnection<TFrom, TTo>(IValueOutPort<TFrom> From, IValueInPort<TTo> To, Func<TFrom, TTo> Converter) : IValueConnection<TFrom, TTo>
{
    public Task<TTo>? RequestUpdate()
    {
        return UpdateRequested?.Invoke()?.ContinueWith(task => Converter.Invoke(task.Result));
    }

    public void NotifyUpdated(TFrom newValue)
    {
        var converted = Converter.Invoke(newValue);
        UpdateNotified?.Invoke(converted);
    }

    public Func<Task<TFrom>?>? UpdateRequested { private get; set; }

    public event Action<TTo>? UpdateNotified;
}