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

    public override string ToString() {
      return string.Format("{0} => {1}", From, To);
    }
  }

  public static class GraphUtils {
    // Nodes are derived from the edges, so orphaned nodes will not be included
    public static List<T> SortDirectAcyclicGraph<T>(IEnumerable<Edge<T>> edges) {
      return SortDirectAcyclicGraph<T>(new T[0], edges);
    }

    // Assuming the passed-in graph is acyclic, sort the nodes from ones with no dependencies in the beginning
    // all the way to the child nodes on which everyone else depends
    public static List<T> SortDirectAcyclicGraph<T>(IEnumerable<T> nodes, IEnumerable<Edge<T>> edges) {
      Dictionary<T, int> incomingNodesCounts = edges
      .GroupBy(x => x.To)
      .ToDictionary(x => x.Key, x => x.Count());

      foreach (T from in edges.Select(x => x.From).Concat(nodes))
        if (!incomingNodesCounts.ContainsKey(from))
          incomingNodesCounts[from] = 0;

      Dictionary<T, List<Edge<T>>> outgoingEdgesByNode = edges
        .GroupBy(x => x.From)
        .ToDictionary(x => x.Key, x => x.ToList());

      List<T> orderedNodesResult = new List<T>();

      // While there are any nodes to be sorted left...
      while (incomingNodesCounts.Count > 0) {
        List<T> toRemove = new List<T>();

        // Remove any nodes with zero incoming edges and add to sorted list
        foreach (var item in incomingNodesCounts) {
          if (item.Value == 0) {
            T node = item.Key;
            toRemove.Add(node);
            orderedNodesResult.Add(node);
          }
        }

        // If we are stuck - no progress made (nothing to remove) and we are not done, throw an Exception
        if (toRemove.Count == 0 && incomingNodesCounts.Count > 0) {
          IEnumerable<T> circularDependencyNodes = incomingNodesCounts.Keys
            .Where(x => outgoingEdgesByNode.ContainsKey(x));
          throw new Exception("Circular reference(s) among: " + string.Join(", ", circularDependencyNodes));
        }

        // Remove all nodes previously marked for removal.
        foreach (T node in toRemove) {
          if (outgoingEdgesByNode.TryGetValue(node, out List<Edge<T>> outgoingEdges))
            foreach (Edge<T> edge in outgoingEdges)
              incomingNodesCounts[edge.To] -= 1;
          incomingNodesCounts.Remove(node);
        }
      }

      return orderedNodesResult;
    }
  }
}
