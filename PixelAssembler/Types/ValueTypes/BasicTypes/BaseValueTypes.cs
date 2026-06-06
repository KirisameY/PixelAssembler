using System;
using System.Collections.Generic;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public partial class BaseValueType
{
    public static BaseValueType String { get; } = Create<string>();
    public static BaseValueType Bool { get; } = Create<bool>(convertTo:
    [
        SelfConversion<bool>(),
        CreateConnectionFactoryTo<bool, string>(b => b.ToString()),
    ]);


    public static KeyValuePair<Type, IValueTypeConversion> CreateConnectionFactoryFrom<TFrom, TTo>(Func<TFrom, TTo> converter) =>
        new(typeof(TFrom), ValueTypeConversion.Create(converter));

    public static KeyValuePair<Type, IValueTypeConversion> CreateConnectionFactoryTo<TFrom, TTo>(Func<TFrom, TTo> converter) =>
        new(typeof(TTo), ValueTypeConversion.Create(converter));

    public static BaseValueType Create<T>(
        IEnumerable<KeyValuePair<Type, IValueTypeConversion>>? convertFrom = null,
        IEnumerable<KeyValuePair<Type, IValueTypeConversion>>? convertTo = null
    ) => new(typeof(T), convertFrom ?? [SelfConversion<T>()], convertTo ?? [SelfConversion<T>()]);

    public static KeyValuePair<Type, IValueTypeConversion> SelfConversion<T>() => new(typeof(T), ValueTypeConversion.SelfConversion<T>());
}