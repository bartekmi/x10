using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.utils {
  public class Edge<T> {
    public T From;
    public T To;

    public Edge(T from, T to) {
      From = from;
      To = to;
    }
  }

  public static class GraphUtils {
    // Nodes are derived from the edges, so orphaned nodes will not be included
    public static List<T> SortDirectAcyclicGraph<T>(IEnumerable<Edge<T>> edges) {
      Dictionary<T, int> _incomingNodesCount = edges
        .GroupBy(x => x.To)
        .ToDictionary(x => x.Key, x => x.Count());

      foreach (T from in edges.Select(x => x.From))
        if (!_incomingNodesCount.ContainsKey(from))
          _incomingNodesCount[from] = 0;

      Dictionary<T, List<Edge<T>>> _outgoingEdges = edges
        .GroupBy(x => x.From)
        .ToDictionary(x => x.Key, x => x.ToList());

      List<T> orderedNodes = new List<T>();

      while (_incomingNodesCount.Count > 0) {
        List<T> toRemove = new List<T>();
        foreach (var item in _incomingNodesCount) {
          if (item.Value == 0) {
            T node = item.Key;
            toRemove.Add(node);
            orderedNodes.Add(node);
          }
        }

        if (toRemove.Count == 0 && _incomingNodesCount.Count > 0)
          throw new Exception("Circular reference(s) among: " + string.Join(", ", _incomingNodesCount.Keys));

        foreach (T node in toRemove) {
          if (_outgoingEdges.TryGetValue(node, out List<Edge<T>> outgoingEdges))
            foreach (Edge<T> edge in outgoingEdges)
              _incomingNodesCount[edge.To] -= 1;
          _incomingNodesCount.Remove(node);
        }
      }

      return orderedNodes;
    }
  }
}
