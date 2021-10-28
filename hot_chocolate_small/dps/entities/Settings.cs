using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// Settings for the DPS application. This is a singleton.
  /// </summary>
  public class Settings : Base {
    // Regular Attributes
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
    public string? MessageHitDetected { get; set; }
    [GraphQLNonNullType]
    public string? MessageHitCleared { get; set; }
    public int? DefaultWhitelistDurationDays { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Settings: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public List<WhitelistDuration>? WhitelistDurations { get; set; }
    [GraphQLNonNullType]
    public List<SettingsAutoAssignment>? AutoAssignments { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      WhitelistDurations?.ForEach(x => x.EnsureUniqueDbid());
      AutoAssignments?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      if (WhitelistDurations != null)
        foreach (WhitelistDuration whitelistDurations in WhitelistDurations)
          whitelistDurations.SetNonOwnedAssociations(repository);

      if (AutoAssignments != null)
        foreach (SettingsAutoAssignment autoAssignments in AutoAssignments)
          autoAssignments.SetNonOwnedAssociations(repository);
    }
  }
}

