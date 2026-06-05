using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public abstract partial class ScalarValueType
{
    protected static FrozenDictionary<Type, Func<ScalarValueType, ConvertConnectFunc>> ConvertFrom { get; } = new List<
        KeyValuePair<Type, Func<ScalarValueType, ConvertConnectFunc>>>
    {
        //
    }.ToFrozenDictionary();
    protected static FrozenDictionary<Type, Func<ScalarValueType, ConvertConnectFunc>> ConvertTo { get; } = new List<
        KeyValuePair<Type, Func<ScalarValueType, ConvertConnectFunc>>>
    {
        CreateConnectionFactoryTo(c=>c.ToString(CultureInfo.InvariantCulture))
    }.ToFrozenDictionary();


    public static KeyValuePair<Type, Func<ScalarValueType, ConvertConnectFunc>> CreateConnectionFactoryFrom<TFrom>(Func<TFrom, IConvertible> converter) =>
        new(typeof(TFrom), type => type.FillConnectionFactoryFrom(converter));

    public static KeyValuePair<Type, Func<ScalarValueType, ConvertConnectFunc>> CreateConnectionFactoryTo<TTo>(Func<IConvertible, TTo> converter) =>
        new(typeof(TTo), type => type.FillConnectionFactoryTo(converter));
}