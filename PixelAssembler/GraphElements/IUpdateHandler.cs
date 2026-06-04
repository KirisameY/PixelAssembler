using System;
using System.Threading.Tasks;

namespace PixelAssembler.GraphElements;

public interface IValueUpdateRequester<T>
{
    public Task<T>? RequestUpdate();
    public event Action<T>? UpdateNotified;
}

public interface IValueUpdateNotifier<T>
{
    public void NotifyUpdated(T newValue);
    public Func<Task<T>?>? UpdateRequested { set; }
}