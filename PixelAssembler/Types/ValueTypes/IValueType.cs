using System;
using System.Diagnostics.CodeAnalysis;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes;

public interface IValueType
{
    public Type NativeType { get; }

    public Func<IValueOutPort, IValueInPort, INodeConnection>? GetConversionFrom(IValueType from);
    public Func<IValueOutPort, IValueInPort, INodeConnection>? GetConversionTo(IValueType to);

    public static bool TryGetConversion(IValueType from, IValueType to, [NotNullWhen(true)] out Func<IValueOutPort, IValueInPort, INodeConnection>? connectionCreate)
    {
        connectionCreate = to.GetConversionFrom(from) ?? from.GetConversionTo(to);
        return connectionCreate is not null;
    }
}