using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.dps.Entities;
using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps {
  [ExtendObjectType(Name = "Mutation")]
  public partial class Mutations {

    #region ClearanceForm
    /// <summary>
    /// Input Data Type for ClearanceFormUpdateHit Mutation
    /// </summary>
    public class ClearanceFormHit : Base {
      public HitStatusEnum? Status { get; set; }
      public ReasonForCleranceEnum? ReasonForClearance { get; set; }
      [GraphQLNonNullType]
      public string WhitelistDaysId { get; set; }
      [GraphQLNonNullType]
      public string? Notes { get; set; }
    }

    /// <summary>
    /// Update mutation for the ClearanceForm component
    /// </summary>
    public virtual Hit ClearanceFormUpdateHit(
      ClearanceFormHit data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
    #endregion

  }
}
