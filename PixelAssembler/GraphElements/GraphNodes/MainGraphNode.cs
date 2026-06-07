using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using KirisameY.Relinq.Extensions;

using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.Types.ValueTypes;

namespace PixelAssembler.GraphElements.GraphNodes;

public abstract partial class MainGraphNode : PaGraphNode
{
    private readonly Lock _lock = new(); //todo: 锁

    public abstract void InPortDisconnected();

    public void OutPortConnected(IValueNodeOutPort port)
    {
        if (_outPortSenders.TryGetValue(port, out var action)) action.Invoke();
    }


    protected abstract bool Update();


    private readonly ConditionalWeakTable<IValueNodeInPort, Func<Task, Task<bool>?>> _inPortUpdaters = [];
    private readonly ConditionalWeakTable<IValueNodeOutPort, object> _outPortValueGetters = [];
    private readonly ConditionalWeakTable<IValueNodeOutPort, Action> _outPortSenders = [];

    protected IValueNodeInPort<T> CreateInPort<T>(uint index, IValueType type, Func<T, bool> updateValue) where T : notnull
    {
        var result = new ValueInPort<T>(type, this, index)
           .WithUpdateNotified(v =>
            {
                if (updateValue(v)) Update();
            });
        _inPortUpdaters.Add(
            result, s => result.ConnectionFrom?.RequestUpdate(s)?.ContinueWith(t => updateValue.Invoke(t.Result))
        );
        return result;
    }

    protected IValueNodeOutPort<T> CreateOutPort<T>(uint index, IValueType type, Func<T> getValue)
    {
        var result = new ValueOutPort<T>(type, this, index);
        result.WithUpdateRequested(s => RequestUpdateFrom(s, result));
        _outPortValueGetters.Add(result, getValue);
        _outPortSenders.Add(result, () => result.NotifyUpdated(getValue.Invoke()));
        return result;
    }

    private readonly ConditionalWeakTable<Task, Task> _requestTasks = [];

    public async Task<T> RequestUpdateFrom<T>(Task startCommand, ValueOutPort<T> port)
    {
        await _requestTasks.GetOrAdd(startCommand, static (start, node) =>
        {
            var requestAll = node.InPorts.OfType<IValueNodeInPort>()
                                 .SelectExist(p => node._inPortUpdaters.TryGetValue(p, out var u) ? u : null)
                                 .SelectExist(u => u.Invoke(start))
                                 .WhenAll();
            return requestAll.ContinueWith(result =>
            {
                if (result.Result.Any(b => b)) node.Update();
            });
        }, this);

        _outPortValueGetters.TryGetValue(port, out var value);
        if (value is not Func<T> valueGetter) throw new Exception($"ValueGetter of port {port} was not saved or has mismatched type");
        return valueGetter.Invoke();
    }
}