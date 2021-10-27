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

    #region SettingsEditor
    /// <summary>
    /// Input Data Type for SettingsEditorUpdateSettings Mutation
    /// </summary>
    public class SettingsEditorSettings : Base {
      public int? HighUrgencyShipments { get; set; }
      public int? HighUrgencyQuotes { get; set; }
      public int? HighUrgencyBookings { get; set; }
      public int? HighUrgencyDaysBeforeShipment { get; set; }
      [GraphQLNonNullType]
      public bool HighUrgencyEscalated { get; set; }
      public int? MediumUrgencyShipments { get; set; }
      public int? MediumUrgencyQuotes { get; set; }
      public int? MediumUrgencyBookings { get; set; }
      public int? MediumUrgencyDaysBeforeShipment { get; set; }
      [GraphQLNonNullType]
      public List<WhitelistDuration>? WhitelistDurations { get; set; }
      [GraphQLNonNullType]
      public WhitelistDuration DefaultWhitelistDuration { get; set; }
      [GraphQLNonNullType]
      public string? MessageHitDetected { get; set; }
      [GraphQLNonNullType]
      public string? MessageHitCleared { get; set; }
      [GraphQLNonNullType]
      public List<SettingsAutoAssignment>? AutoAssignments { get; set; }
    }

    /// <summary>
    /// Update mutation for the SettingsEditor component
    /// </summary>
    public virtual Settings SettingsEditorUpdateSettings(
      SettingsEditorSettings data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
    #endregion

    #region ClearanceForm
    /// <summary>
    /// Input Data Type for ClearanceFormUpdateHit Mutation
    /// </summary>
    public class ClearanceFormHit : Base {
      public HitStatusEnum? Status { get; set; }
      public ReasonForCleranceEnum? ReasonForClearance { get; set; }
      [GraphQLNonNullType]
      public WhitelistDuration WhitelistDays { get; set; }
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
