using System;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes;

public interface IValueTypeConversion
{
    public Type From { get; }
    public Type To { get; }
    public INodeConnection CreateConnection(IValueOutPort from, IValueInPort to);
    public IValueTypeConversion Concat(IValueTypeConversion then);
}

public interface IValueTypeConversionFrom<TFrom> : IValueTypeConversion
{
    Type IValueTypeConversion.From => typeof(TFrom);

    public IValueTypeConversion AcceptConcat<TSource>(IValueTypeConversion<TSource, TFrom> before);
}

public interface IValueTypeConversionTo<TTo> : IValueTypeConversion
{
    Type IValueTypeConversion.To => typeof(TTo);
}

public interface IValueTypeConversion<TFrom, TTo> : IValueTypeConversionFrom<TFrom>, IValueTypeConversionTo<TTo>
{
    public Func<TFrom, TTo> Convert { get; }

    public new IValueConnection<TFrom, TTo> CreateConnection(IValueOutPort from, IValueInPort to);
    public IValueConnection<TFrom, TTo> CreateConnection(IValueOutPort<TFrom> from, IValueInPort<TTo> to);
    public new IValueTypeConversion<TSource, TTo> AcceptConcat<TSource>(IValueTypeConversion<TSource, TFrom> before);

    INodeConnection IValueTypeConversion.CreateConnection(IValueOutPort from, IValueInPort to) => CreateConnection(from, to);
    IValueTypeConversion IValueTypeConversionFrom<TFrom>.AcceptConcat<TSource>(IValueTypeConversion<TSource, TFrom> before) => AcceptConcat(before);
}