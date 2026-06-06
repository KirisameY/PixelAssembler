using System;
using System.Diagnostics.CodeAnalysis;

namespace PixelAssembler.Types.ValueTypes;

public interface IValueType
{
    public Type NativeType { get; }

    public bool CanSkipCheckOtherToSelf => false;

    public IValueTypeConversion? GetConversionFrom(IValueType from);
    public IValueTypeConversion? GetConversionTo(IValueType to);

    public static bool TryGetConversion(IValueType from, IValueType to, [NotNullWhen(true)] out IValueTypeConversion? conversion)
    {
        conversion = to.GetConversionFrom(from);
        if (!to.CanSkipCheckOtherToSelf) conversion ??= from.GetConversionTo(to);
        return conversion is not null;
    }
}