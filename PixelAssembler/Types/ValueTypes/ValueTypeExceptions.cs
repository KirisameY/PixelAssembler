using System;

using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes;

public class ValuePortNativeTypeMismatchedException(IValuePort port) : Exception($"Value port {port} has mismatched native value type with its type {port.Type}");