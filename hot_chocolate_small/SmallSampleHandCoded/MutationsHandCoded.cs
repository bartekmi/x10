// Auto-generated by x10 - do not edit
using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.SmallSample.Entities;
using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample {
  [ExtendObjectType(Name = "Mutation")]
  public class MutationsHandCoded : Mutations {

    /// <summary>
    /// Update mutation for the MoveForm component
    /// </summary>
    public override Move MoveFormUpdateMove(
      MoveFormMove data,
      [Service] IRepository repository) {

      int? id = IdUtils.FromFrontEndId(data.Id);
      Move entity = id == null ? new Move() : repository.GetMove(id.Value);

      entity.Tenant = new Tenant() { Id = data.Tenant.Id };
      entity.From = new Building() { Id = data.From.Id };
      entity.To = new Building() { Id = data.To.Id };
      entity.Date = data.Date;

      entity.SetNonOwnedAssociations(repository);
      repository.AddOrUpdateMove(id, entity);

      return entity;      
    }

    /// <summary>
    /// Update mutation for the BuildingForm component
    /// </summary>
    public override Building BuildingFormUpdateBuilding(
      BuildingFormBuilding data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }

    /// <summary>
    /// Update mutation for the TenantForm component
    /// </summary>
    public override Tenant TenantFormUpdateTenant(
      TenantFormTenant data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
  }
}
