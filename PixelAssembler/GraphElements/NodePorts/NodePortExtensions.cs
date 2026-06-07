namespace PixelAssembler.GraphElements.NodePorts;

public static class NodePortExtensions
{
    extension(INodeInPort port)
    {
        public bool Connected => port is INodeSingleInPort s ? s.ConnectionFrom is not null : port.ConnectionsFrom is not [];
    }

    extension(INodeOutPort port)
    {
        public bool Connected => port is INodeSingleOutPort s ? s.ConnectionTo is not null : port.ConnectionsTo is not [];
    }
}