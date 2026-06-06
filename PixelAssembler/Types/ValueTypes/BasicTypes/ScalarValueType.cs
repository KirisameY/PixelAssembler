using System;
using System.Collections.Generic;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public abstract partial class ScalarValueType : IValueType
{
    public abstract Type NativeType { get; }

    public abstract IValueTypeConversion? GetConversionFrom(IValueType from);

    public abstract IValueTypeConversion? GetConversionTo(IValueType to);

    public abstract IValueTypeConversionFrom<IConvertible> ConversionFromGeneral { get; }
    public abstract IValueTypeConversionTo<IConvertible> ConversionToGeneral { get; }
}

public sealed class ScalarValueType<T>(IValueTypeConversion<IConvertible, T> fromGeneral, IValueTypeConversion<T, IConvertible> toGeneral) : ScalarValueType, IValueType
{
    public override Type NativeType => typeof(T);

    public bool CanSkipCheckOtherToSelf => true;


    public override IValueTypeConversion<IConvertible, T> ConversionFromGeneral => fromGeneral;
    public override IValueTypeConversion<T, IConvertible> ConversionToGeneral => toGeneral;


    public override IValueTypeConversion? GetConversionFrom(IValueType from)
    {
        if (from.NativeType == NativeType) return SelfConversion;
        if (from is ScalarValueType fromScalar) return fromScalar.ConversionToGeneral.Concat(fromGeneral);

        var conv = GeneralConversionsFrom.GetValueOrDefault(from.NativeType);
        if (conv is not null) return conv.Concat(fromGeneral);

        var result = from.GetConversionTo(this);
        result ??= from.GetConversionTo(General)?.Concat(fromGeneral);

        return result;
    }

    public override IValueTypeConversion? GetConversionTo(IValueType to)
    {
        if (to.NativeType == NativeType) return SelfConversion;
        if (to is ScalarValueType toScalar) return toGeneral.Concat(toScalar.ConversionFromGeneral);

        var conv = GeneralConversionsTo.GetValueOrDefault(to.NativeType);
        if (conv is not null) return toGeneral.Concat(conv);

        var result = to.GetConversionFrom(this);
        if (result is null)
        {
            var then = to.GetConversionFrom(General);
            if (then is not null) result = toGeneral.Concat(then);
        }
        return result;
    }

    public static IValueTypeConversion SelfConversion { get; } = ValueTypeConversion.SelfConversion<T>();
}