using System;
using System.Globalization;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public abstract partial class ScalarValueType
{
    public static ScalarValueType<T> Create<T>(Func<IConvertible, T> fromGeneral, Func<T, IConvertible> toGeneral) => new(fromGeneral, toGeneral);

    public static ScalarValueType Int { get; } = Create(c => c.ToInt32(NumberFormatInfo.InvariantInfo), i => i);
    public static ScalarValueType Float { get; } = Create(c => c.ToSingle(NumberFormatInfo.InvariantInfo), f => f);
    public static ScalarValueType Decimal { get; } = Create(c => c.ToDecimal(NumberFormatInfo.InvariantInfo), d => d);
}