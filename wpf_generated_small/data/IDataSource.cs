using System.Collections.Generic;
using wpf_lib.lib;

using wpf_generated.entities;

namespace wpf_generated.data {
  public interface IDataSource {
    IEnumerable<Building> Buildings { get; }
    void CreateOrUpdate(Building model);

    T GetById<T>(int id) where T : EntityBase;
  }
}
