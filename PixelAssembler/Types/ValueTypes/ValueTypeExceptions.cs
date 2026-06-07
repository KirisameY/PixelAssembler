using System;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes;

public class ValuePortNativeTypeMismatchedException(IValueNodePort port) :
    Exception($"Value port {port} has mismatched native value type with its type {port.Type}");

public class ValueConversionNativeTypeMismatchedException(IValueTypeConversion first, IValueTypeConversion second) :
    Exception($"Value conversion {first} and {second} has mismatched intermediate native value type {first.To} and {second.From}");