using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

namespace x10.utils {
  public class GraphUtilsTest {

    [Fact]
    public void Sort() {
      int[] sorted = GraphUtils.SortDirectAcyclicGraph(new Edge<int>[] {
        new Edge<int>(1, 3),
        new Edge<int>(2, 4),
        new Edge<int>(5, 1),
        new Edge<int>(2, 5),
      }).ToArray();

      Assert.Equal(new int[] { 2, 4, 5, 1, 3 }, sorted);
    }

    [Fact]
    public void SortWithCycle() {
      try {
        GraphUtils.SortDirectAcyclicGraph(new Edge<int>[] {
          new Edge<int>(4, 1),
          new Edge<int>(1, 2),
          new Edge<int>(2, 3),
          new Edge<int>(3, 1),
        });
        Assert.True(false);
      } catch (Exception e) {
        Assert.Equal("Circular reference(s) among: 1, 2, 3", e.Message);
      }
    }

    [Fact]
    public void SortWithSelfCycle() {
      try {
        GraphUtils.SortDirectAcyclicGraph(new Edge<int>[] {
          new Edge<int>(1, 1),
        });
        Assert.True(false);
      } catch (Exception e) {
        Assert.Equal("Circular reference(s) among: 1", e.Message);
      }
    }
  }
}