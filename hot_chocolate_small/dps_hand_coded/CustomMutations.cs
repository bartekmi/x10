using System;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.dps.Entities;
using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps {
  [ExtendObjectType(Name = "Mutation")]
  public class CustomMutations : Mutations {
    /// <summary>
    /// Update a hit from the Clearance Form
    /// </summary>
    public override Hit ClearanceFormUpdateHit(
      ClearanceFormHit data,
      [Service] IRepository repository) {
        int? id = IdUtils.FromRelayId(data.Id);
        if (id == null)
          throw new Exception("Could not extract dbid from " + data.Id);

        Hit hit = repository.GetHit(id.Value);

        hit.Notes = data.Notes;
        hit.ReasonForClearance = data.ReasonForClearance;
        hit.Status = data.Status;
        hit.WhitelistDays = data.WhitelistDays;

        hit.SetNonOwnedAssociations(repository);
        return hit;
    }

    /// <summary>
    /// Update mutation for the SettingsEditor component
    /// </summary>
    public override Settings SettingsEditorUpdateSettings(
      SettingsEditorSettings data,
      [Service] IRepository repository) {
        int? id = IdUtils.FromRelayId(data.Id);
        if (id == null)
          throw new Exception("Could not extract dbid from " + data.Id);

        Settings settings = repository.GetSettings(id.Value);

        settings.HighUrgencyShipments = data.HighUrgencyShipments;
        settings.HighUrgencyQuotes = data.HighUrgencyQuotes;
        settings.HighUrgencyBookings = data.HighUrgencyBookings;
        settings.HighUrgencyDaysBeforeShipment = data.HighUrgencyDaysBeforeShipment;
        settings.HighUrgencyEscalated = data.HighUrgencyEscalated;

        settings.MediumUrgencyShipments = data.MediumUrgencyShipments;
        settings.MediumUrgencyQuotes = data.MediumUrgencyQuotes;
        settings.MediumUrgencyBookings = data.MediumUrgencyBookings;
        settings.MediumUrgencyDaysBeforeShipment = data.MediumUrgencyDaysBeforeShipment;

        settings.WhitelistDurations = data.WhitelistDurations;
        settings.DefaultWhitelistDuration = data.DefaultWhitelistDuration;

        settings.MessageHitDetected = data.MessageHitDetected;
        settings.MessageHitCleared = data.MessageHitCleared;

        settings.AutoAssignments = data.AutoAssignments;

        settings.SetNonOwnedAssociations(repository);
        return settings;
    }
  }
}
