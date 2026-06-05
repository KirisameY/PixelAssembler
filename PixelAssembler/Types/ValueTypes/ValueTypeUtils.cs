using System;

using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes.BasicTypes;

public static class ValueTypeUtils
{
    public static ConvertConnectFunc CreateConnectionFactory<TFrom, TTo>(Func<TFrom, TTo> converter) => (outPort, inPort) =>
    {
        if (outPort is not IValueOutPort<TFrom> fromPort) throw new ValuePortNativeTypeMismatchedException(outPort);
        if (inPort is not IValueInPort<TTo> toPort) throw new ValuePortNativeTypeMismatchedException(inPort);

        return new ValueDataConnection<TFrom, TTo>(fromPort, toPort, converter);
    };
}