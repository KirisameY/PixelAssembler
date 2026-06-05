using System;
using System.Collections.Generic;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public abstract partial class ScalarValueType : IValueType
{
    public abstract Type NativeType { get; }

    public abstract ConvertConnectFunc? GetConversionFrom(IValueType from);

    public abstract ConvertConnectFunc? GetConversionTo(IValueType to);

    public abstract ConvertConnectFunc FillConnectionFactoryFrom<T>(Func<T, IConvertible> convertPrefix);
    public abstract ConvertConnectFunc FillConnectionFactoryTo<T>(Func<IConvertible, T> convertSuffix);
}

public sealed class ScalarValueType<T>(Func<IConvertible, T> fromGeneral, Func<T, IConvertible> toGeneral) : ScalarValueType
{
    public override Type NativeType => typeof(T);

    public override ConvertConnectFunc? GetConversionFrom(IValueType from)
    {
        if (from.NativeType == NativeType) return SelfConversion;
        if (from is ScalarValueType fromScalar) return fromScalar.FillConnectionFactoryTo(fromGeneral);

        var func = ConvertFrom.GetValueOrDefault(from.NativeType);
        return func?.Invoke(this);
    }

    public override ConvertConnectFunc? GetConversionTo(IValueType to)
    {
        if (to.NativeType == NativeType) return SelfConversion;
        if (to is ScalarValueType toScalar) return toScalar.FillConnectionFactoryFrom(toGeneral);

        var func = ConvertTo.GetValueOrDefault(to.NativeType);
        return func?.Invoke(this);
    }


    public static ConvertConnectFunc SelfConversion { get; } = ValueTypeUtils.CreateConnectionFactory<T, T>(v => v);

    public override ConvertConnectFunc FillConnectionFactoryFrom<TFrom>(Func<TFrom, IConvertible> convertPrefix) =>
        ValueTypeUtils.CreateConnectionFactory<TFrom, T>(from => fromGeneral.Invoke(convertPrefix.Invoke(from)));

    public override ConvertConnectFunc FillConnectionFactoryTo<TTo>(Func<IConvertible, TTo> convertSuffix) =>
        ValueTypeUtils.CreateConnectionFactory<T, TTo>(from => convertSuffix.Invoke(toGeneral.Invoke(from)));
}