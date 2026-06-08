using System.Collections.Generic;
using System.Linq;

using KirisameY.Relinq.Extensions;

namespace PixelAssembler.GraphElements.GraphNodes;

public static class GraphNodeExtensions
{
    extension(IPaGraphNode node)
    {
        public IEnumerable<IPaGraphNode> EnumerateAfterNodes()
        {
            return node.OutPorts
                       .SelectExist(p => p?.Parent)
                       .Concat(node.OutPorts
                                   .SelectMany(p => p?.ConnectionsTo ?? [])
                                   .SelectMany(p => p.To.Parent.EnumerateAfterNodes())
                        );
        }

        public IEnumerable<IPaGraphNode> EnumerateBeforeNodes()
        {
            return node.InPorts
                       .SelectExist(p => p?.Parent)
                       .Concat(node.InPorts
                                   .SelectMany(p => p?.ConnectionsFrom ?? [])
                                   .SelectMany(p => p.From.Parent.EnumerateBeforeNodes())
                        );
        }

        public bool IsAfter(IPaGraphNode other) => other.EnumerateAfterNodes().Contains(node);
        public bool IsBefore(IPaGraphNode other) => other.EnumerateBeforeNodes().Contains(node);
    }
}