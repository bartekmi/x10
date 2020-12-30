using System;
using x10.parsing;
using x10.ui.libraries;
using x10.ui.platform;

using Xunit;
using Xunit.Abstractions;

namespace x10.gen.wpf {
  // The purpose of this test file is to test nitty-gritty behavior of C#
  // for the purpose of knowing what to generate.
  public class CSharpBehavior {
    [Fact]
    public void AccessYearOfNullableDate() {
      Assert.Equal(2020, GetYear(new DateTime(2020, 1, 2)));
      Assert.Null(GetYear(null));
    }

    private int? GetYear(DateTime? date) {
      return date?.Year;
    }
  }
}