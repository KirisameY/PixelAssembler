using System;
using System.Diagnostics.CodeAnalysis;

namespace PixelAssembler.Types.ValueTypes;

public interface IValueType
{
    public Type NativeType { get; }

    public ConvertConnectFunc? GetConversionFrom(IValueType from);
    public ConvertConnectFunc? GetConversionTo(IValueType to);

    public static bool TryGetConversion(IValueType from, IValueType to, [NotNullWhen(true)] out ConvertConnectFunc? connectionCreate)
    {
        connectionCreate = to.GetConversionFrom(from) ?? from.GetConversionTo(to);
        return connectionCreate is not null;
    }
}