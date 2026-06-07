using System;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes;

public static class ValueTypeConversion
{
    public static IValueTypeConversion<T, T> SelfConversion<T>() => new ValueTypeConversion<T, T>(x => x);
    public static IValueTypeConversion<TFrom, TTo> Create<TFrom, TTo>(Func<TFrom, TTo> convert) => new ValueTypeConversion<TFrom, TTo>(convert);
}

public class ValueTypeConversion<TFrom, TTo>(Func<TFrom, TTo> convert) : IValueTypeConversion<TFrom, TTo>
{
    public Func<TFrom, TTo> Convert => convert;

    public IValueConnection<TFrom, TTo> CreateConnection(IValueNodeOutPort from, IValueNodeInPort to)
    {
        if (from is not IValueNodeOutPort<TFrom> fromPort) throw new ValuePortNativeTypeMismatchedException(from);
        if (to is not IValueNodeInPort<TTo> toPort) throw new ValuePortNativeTypeMismatchedException(to);

        return CreateConnection(fromPort, toPort);
    }

    public IValueConnection<TFrom, TTo> CreateConnection(IValueNodeOutPort<TFrom> from, IValueNodeInPort<TTo> to)
    {
        return new ValueDataConnection<TFrom, TTo>(from, to, convert);
    }


    public IValueTypeConversion Concat(IValueTypeConversion then)
    {
        if (then is not IValueTypeConversionFrom<TTo> toConcat) throw new ValueConversionNativeTypeMismatchedException(this, then);

        return toConcat.AcceptConcat(this);
    }

    public IValueTypeConversion<TSource, TTo> AcceptConcat<TSource>(IValueTypeConversion<TSource, TFrom> before) =>
        new ValueTypeConversion<TSource, TTo>(s => convert.Invoke(before.Convert.Invoke(s)));
}