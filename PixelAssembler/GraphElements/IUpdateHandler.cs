using System;

namespace PixelAssembler.GraphElements;

public interface IUpdateRequester<out T>
{
    public void RequestUpdate();
    public event EventHandler<T> UpdateNotified;
}

public interface IUpdateNotifier<in T>
{
    public void NotifyUpdated(T newValue);
    public event EventHandler UpdateRequested;
}