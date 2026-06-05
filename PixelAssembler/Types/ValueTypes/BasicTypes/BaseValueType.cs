using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

using ConvertDelegate = Func<IValueOutPort, IValueInPort, INodeConnection>;

public partial class BaseValueType(
    Type nativeType,
    IEnumerable<KeyValuePair<Type, ConvertDelegate>> convertFrom,
    IEnumerable<KeyValuePair<Type, ConvertDelegate>> convertTo
) : IValueType
{
    public Type NativeType => nativeType;

    private FrozenDictionary<Type, ConvertDelegate> ConvertFrom { get; } = convertFrom.ToFrozenDictionary();
    private FrozenDictionary<Type, ConvertDelegate> ConvertTo { get; } = convertTo.ToFrozenDictionary();

    public ConvertDelegate? GetConversionFrom(IValueType from) => ConvertFrom?.GetValueOrDefault(from.NativeType);
    public ConvertDelegate? GetConversionTo(IValueType to)=> ConvertTo?.GetValueOrDefault(to.NativeType);
}