using System.Collections.Generic;

using wpf_generated.entities;

namespace wpf_generated.data {
  public interface IDataSource {
    IEnumerable<Building> Buildings { get; }
    void Create(Building model);
  }
}
