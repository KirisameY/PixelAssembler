using System.Collections.Generic;

using Godot;

namespace PixelAssembler.Misc;

public static class GdWrapper
{
    public static GdWrapper<T> New<T>(T value) => new(value);
}

public partial class GdWrapper<T>(T value) : GodotObject
{
    public T Value => value;

    protected bool Equals(GdWrapper<T> other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is GdWrapper<T> w) return Equals(w);
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(GdWrapper<T> x, GdWrapper<T> y) => x.Equals(y);

    public static bool operator !=(GdWrapper<T> x, GdWrapper<T> y) => !(x == y);
}