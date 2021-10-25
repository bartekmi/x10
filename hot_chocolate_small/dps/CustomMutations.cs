using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.dps.Entities;
using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps {
  /// <summary>
  /// Input Data Type for ClearanceFormHit 
  /// </summary>
  public class ClearanceFormHit : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string Notes { get; set; }
    [GraphQLNonNullType]
    public ReasonForCleranceEnum ReasonForClearance { get; set; }
    [GraphQLNonNullType]
    public HitStatusEnum Status { get; set; }
    [GraphQLNonNullType]
    public string WhitelistDaysId { get; set; }
  }

  [ExtendObjectType(Name = "Mutation")]
  public class CustomMutations : Mutations {
    /// <summary>
    /// Update a hit from the Clearance Form
    /// </summary>
    public Hit ClearanceFormUpdateHit(
      ClearanceFormHit data,
      [Service] IRepository repository) {
        int? id = IdUtils.FromRelayId(data.Id);
        if (id == null)
          throw new Exception("Could not extract dbid from " + data.Id);

        Hit hit = repository.GetHit(id.Value);

        hit.Notes = data.Notes;
        hit.ReasonForClearance = data.ReasonForClearance;
        hit.Status = data.Status;

        int? whitelistDurationId = IdUtils.FromRelayId(data.WhitelistDaysId);
        if (whitelistDurationId != null)
          hit.WhitelistDays = repository.GetWhitelistDuration( whitelistDurationId.Value);

        return hit;
    }
  }
}
