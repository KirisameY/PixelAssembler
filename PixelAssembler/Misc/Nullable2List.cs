using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelAssembler.Misc;

public class Nullable2List<T>(Func<T?> getter) : IReadOnlyList<T>
{
    public IEnumerator<T> GetEnumerator()
    {
        var value = getter.Invoke();
        if (value is not null) yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => getter.Invoke() is null ? 0 : 1;
    public T this[int index]
    {
        get
        {
            if (index < 0) throw new IndexOutOfRangeException($"index {index} is less than 0");
            if (index != 0 || getter.Invoke() is not { } result) throw new IndexOutOfRangeException($"index {index} is out of length");
            return result;
        }
    }
}