using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public abstract partial class ScalarValueType
{
    protected static FrozenDictionary<Type, IValueTypeConversionTo<IConvertible>> GeneralConversionsFrom { get; } = new List<
        KeyValuePair<Type, IValueTypeConversionTo<IConvertible>>>
    {
        CreateGeneralConversionFrom<bool>(b => b ? 1 : 0),
    }.ToFrozenDictionary();
    protected static FrozenDictionary<Type, IValueTypeConversionFrom<IConvertible>> GeneralConversionsTo { get; } = new List<
        KeyValuePair<Type, IValueTypeConversionFrom<IConvertible>>>
    {
        CreateGeneralConversionTo(c => c.ToString(CultureInfo.InvariantCulture)),
        CreateGeneralConversionTo(c => c.ToBoolean(CultureInfo.InvariantCulture)),
    }.ToFrozenDictionary();


    public static KeyValuePair<Type, IValueTypeConversionTo<IConvertible>> CreateGeneralConversionFrom<TFrom>(Func<TFrom, IConvertible> converter) =>
        new(typeof(TFrom), ValueTypeConversion.Create(converter));

    public static KeyValuePair<Type, IValueTypeConversionFrom<IConvertible>> CreateGeneralConversionTo<TTo>(Func<IConvertible, TTo> converter) =>
        new(typeof(TTo), ValueTypeConversion.Create(converter));
}