using System;
using System.Collections.Generic;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

using ConvertDelegate = Func<IValueOutPort, IValueInPort, INodeConnection>;

public partial class BaseValueType
{
    public static BaseValueType String { get; } = Create<string>(convertFrom: [SelfConversion<string>()]);
    public static BaseValueType StringPath { get; } = Create<string>(convertFrom: [SelfConversion<string>()]);


    private static ConvertDelegate CreateConnectionFactory<TFrom, TTo>(Func<TFrom, TTo> converter) => (outPort, inPort) =>
    {
        if (outPort is not IValueOutPort<TFrom> fromPort) throw new NativeTypeMismatchedException(outPort);
        if (inPort is not IValueInPort<TTo> toPort) throw new NativeTypeMismatchedException(inPort);

        return new ValueDataConnection<TFrom, TTo>(fromPort, toPort, converter);
    };

    public static KeyValuePair<Type, ConvertDelegate> CreateConnectionFactoryFrom<TFrom, TTo>(Func<TFrom, TTo> converter) =>
        new(typeof(TFrom), CreateConnectionFactory(converter));

    public static KeyValuePair<Type, ConvertDelegate> CreateConnectionFactoryTo<TFrom, TTo>(Func<TFrom, TTo> converter) =>
        new(typeof(TTo), CreateConnectionFactory(converter));

    public static BaseValueType Create<T>(
        IEnumerable<KeyValuePair<Type, ConvertDelegate>>? convertFrom = null,
        IEnumerable<KeyValuePair<Type, ConvertDelegate>>? convertTo = null
    ) => new(typeof(T), convertFrom ?? [SelfConversion<T>()], convertTo ?? [SelfConversion<T>()]);

    public static KeyValuePair<Type, ConvertDelegate> SelfConversion<T>() =>
        new(typeof(T), CreateConnectionFactory<T, T>(static a => a));

    public class NativeTypeMismatchedException(IValuePort port) : Exception($"Value port {port} has mismatched native value type with its type {port.Type}");
}