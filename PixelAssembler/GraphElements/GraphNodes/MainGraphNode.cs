using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Godot;

using KirisameY.Relinq.Extensions;

using PixelAssembler.GraphElements.NodePorts;
using PixelAssembler.Types.ValueTypes;

namespace PixelAssembler.GraphElements.GraphNodes;

public abstract partial class MainGraphNode : PaGraphNode
{
    // 注：涉及状态的更改、读、写的部分需要上锁
    private readonly Lock _lock = new();


    /// <returns><c>true</c> if update is needed, otherwise <c>false</c></returns>
    protected abstract bool WhenInPortDisconnected(IValueNodeInPort port);

    public void InPortDisconnected(IValueNodeInPort port)
    {
        Task.Run(() =>
        {
            lock (_lock)
            {
                if (WhenInPortDisconnected(port) && Update())
                    NotifyAllOutputs();
            }
        }).ContinueWith(
            task => GD.PrintErr("Error on updating node values", task.Exception), // todo: 日后我们需要更好的异常回报机制
            TaskContinuationOptions.NotOnRanToCompletion
        );
    }

    public void OutPortConnected(IValueNodeOutPort port)
    {
        if (_outPortSenders.TryGetValue(port, out var action))
        {
            Task.Run(() =>
            {
                lock (_lock)
                {
                    action.Invoke();
                }
            }).ContinueWith(
                task => GD.PrintErr("Error on updating node values", task.Exception),
                TaskContinuationOptions.NotOnRanToCompletion
            );
        }
    }

    /// <summary>
    /// 不自带上锁但是读了状态，调用前手动锁一下
    /// </summary>
    private void NotifyAllOutputs(IReadOnlyCollection<IValueNodeOutPort>? exceptions = null)
    {
        OutPorts.OfType<IValueNodeOutPort>()
                .Where(port => exceptions?.Contains(port) is not true)
                .SelectExist(p => _outPortSenders.TryGetValue(p, out var sender) ? sender : null)
                .ForEach(sender => sender.Invoke());
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
                lock (_lock)
                {
                    if (updateValue(v) && Update())
                        NotifyAllOutputs();
                }
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

    private readonly ConditionalWeakTable<Task, Tuple<Task, List<IValueNodeOutPort>>> _requestTasks = [];

    public async Task<T> RequestUpdateFrom<T>(Task startCommand, ValueOutPort<T> port)
    {
        var t = _requestTasks.GetOrAdd(startCommand, static (start, node) =>
        {
            var requestAll = node.InPorts.OfType<IValueNodeInPort>()
                                 .SelectExist(p => node._inPortUpdaters.TryGetValue(p, out var u) ? u : null)
                                 .SelectExist(u => u.Invoke(start))
                                 .WhenAll();
            var list = new List<IValueNodeOutPort>();

            return Tuple.Create(requestAll.ContinueWith(result =>
            {
                if (!result.Result.Any(b => b)) return;
                lock (node._lock)
                {
                    if (node.Update()) node.NotifyAllOutputs(list);
                }
            }), list);
        }, this);

        t.Item2.Add(port);
        await t.Item1;

        lock (_lock)
        {
            _outPortValueGetters.TryGetValue(port, out var value);
            if (value is not Func<T> valueGetter) throw new Exception($"ValueGetter of port {port} was not saved or has mismatched type");
            return valueGetter.Invoke();
        }
    }
}