using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public partial class BaseValueType(
    Type nativeType,
    IEnumerable<KeyValuePair<Type, ConvertConnectFunc>> convertFrom,
    IEnumerable<KeyValuePair<Type, ConvertConnectFunc>> convertTo
) : IValueType
{
    public Type NativeType => nativeType;

    private FrozenDictionary<Type, ConvertConnectFunc> ConvertFrom { get; } = convertFrom.ToFrozenDictionary();
    private FrozenDictionary<Type, ConvertConnectFunc> ConvertTo { get; } = convertTo.ToFrozenDictionary();

    public ConvertConnectFunc? GetConversionFrom(IValueType from) => ConvertFrom.GetValueOrDefault(from.NativeType);
    public ConvertConnectFunc? GetConversionTo(IValueType to)=> ConvertTo.GetValueOrDefault(to.NativeType);
}