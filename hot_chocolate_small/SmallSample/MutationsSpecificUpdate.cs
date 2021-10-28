using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.SmallSample.Entities;
using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample {
  [ExtendObjectType(Name = "Mutation")]
  public partial class Mutations {

    #region MoveForm
    /// <summary>
    /// Input Data Type for MoveFormUpdateMove Mutation
    /// </summary>
    public class MoveFormMove : Base {
      public DateTime? Date { get; set; }
      [GraphQLNonNullType]
      public Building From { get; set; }
      [GraphQLNonNullType]
      public Building To { get; set; }
      [GraphQLNonNullType]
      public Tenant Tenant { get; set; }
    }

    /// <summary>
    /// Update mutation for the MoveForm component
    /// </summary>
    public virtual Move MoveFormUpdateMove(
      MoveFormMove data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
    #endregion

    #region BuildingForm
    /// <summary>
    /// Input Data Type for BuildingFormUpdateBuilding Mutation
    /// </summary>
    public class BuildingFormBuilding : Base {
      [GraphQLNonNullType]
      public Address PhysicalAddress { get; set; }
      public DateTime? DateOfOccupancy { get; set; }
      [GraphQLNonNullType]
      public string? Moniker { get; set; }
      [GraphQLNonNullType]
      public string? Name { get; set; }
      [GraphQLNonNullType]
      public string? Description { get; set; }
      [GraphQLNonNullType]
      public bool MailingAddressSameAsPhysical { get; set; }
      public Address MailingAddress { get; set; }
      public MailboxTypeEnum? MailboxType { get; set; }
      public PetPolicyEnum? PetPolicy { get; set; }
      [GraphQLNonNullType]
      public List<Unit>? Units { get; set; }
    }

    /// <summary>
    /// Update mutation for the BuildingForm component
    /// </summary>
    public virtual Building BuildingFormUpdateBuilding(
      BuildingFormBuilding data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
    #endregion

    #region TenantForm
    /// <summary>
    /// Input Data Type for TenantFormUpdateTenant Mutation
    /// </summary>
    public class TenantFormTenant : Base {
      [GraphQLNonNullType]
      public string? Id { get; set; }
      [GraphQLNonNullType]
      public string? Name { get; set; }
      [GraphQLNonNullType]
      public string? Phone { get; set; }
      [GraphQLNonNullType]
      public string? Email { get; set; }
      [GraphQLNonNullType]
      public Address PermanentMailingAddress { get; set; }
    }

    /// <summary>
    /// Update mutation for the TenantForm component
    /// </summary>
    public virtual Tenant TenantFormUpdateTenant(
      TenantFormTenant data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
    #endregion

  }
}
