using System;
using System.Collections.Generic;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public partial class BaseValueType
{
    public static BaseValueType String { get; } = Create<string>(convertFrom: [SelfConversion<string>()]);
    public static BaseValueType StringPath { get; } = Create<string>(convertFrom: [SelfConversion<string>()]);


    public static KeyValuePair<Type, ConvertConnectFunc> CreateConnectionFactoryFrom<TFrom, TTo>(Func<TFrom, TTo> converter) =>
        new(typeof(TFrom), ValueTypeUtils.CreateConnectionFactory(converter));

    public static KeyValuePair<Type, ConvertConnectFunc> CreateConnectionFactoryTo<TFrom, TTo>(Func<TFrom, TTo> converter) =>
        new(typeof(TTo), ValueTypeUtils.CreateConnectionFactory(converter));

    public static BaseValueType Create<T>(
        IEnumerable<KeyValuePair<Type, ConvertConnectFunc>>? convertFrom = null,
        IEnumerable<KeyValuePair<Type, ConvertConnectFunc>>? convertTo = null
    ) => new(typeof(T), convertFrom ?? [SelfConversion<T>()], convertTo ?? [SelfConversion<T>()]);

    public static KeyValuePair<Type, ConvertConnectFunc> SelfConversion<T>() =>
        new(typeof(T), ValueTypeUtils.CreateConnectionFactory<T, T>(static a => a));
}