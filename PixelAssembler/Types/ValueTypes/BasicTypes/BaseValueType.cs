using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public partial class BaseValueType(
    Type nativeType,
    IEnumerable<KeyValuePair<Type, IValueTypeConversion>> convertFrom,
    IEnumerable<KeyValuePair<Type, IValueTypeConversion>> convertTo
) : IValueType
{
    public Type NativeType => nativeType;

    private FrozenDictionary<Type, IValueTypeConversion> ConvertFrom { get; } = convertFrom.ToFrozenDictionary();
    private FrozenDictionary<Type, IValueTypeConversion> ConvertTo { get; } = convertTo.ToFrozenDictionary();

    public IValueTypeConversion? GetConversionFrom(IValueType from) => ConvertFrom.GetValueOrDefault(from.NativeType);
    public IValueTypeConversion? GetConversionTo(IValueType to)=> ConvertTo.GetValueOrDefault(to.NativeType);
}