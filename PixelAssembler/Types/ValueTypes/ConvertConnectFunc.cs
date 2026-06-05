using PixelAssembler.GraphElements.Connections;
using PixelAssembler.GraphElements.NodePorts;

namespace PixelAssembler.Types.ValueTypes;

public delegate INodeConnection ConvertConnectFunc(IValueOutPort from, IValueInPort to);