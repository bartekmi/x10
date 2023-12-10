using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// A quote for a shipment - it might become one
  /// </summary>
  public class Quote : Base {
    // Regular Attributes
    public int? Dbid { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    public TransportationModeEnum? TransportationMode { get; set; }
    [GraphQLNonNullType]
    public string? Status { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Quote: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    public Client? Client { get; set; }
    public Port? DeparturePort { get; set; }
    public Port? ArrivalPort { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? client = IdUtils.FromFrontEndId(Client?.Id);
      Client = client == null ? null : repository.GetClient(client.Value);

      int? departurePort = IdUtils.FromFrontEndId(DeparturePort?.Id);
      DeparturePort = departurePort == null ? null : repository.GetPort(departurePort.Value);

      int? arrivalPort = IdUtils.FromFrontEndId(ArrivalPort?.Id);
      ArrivalPort = arrivalPort == null ? null : repository.GetPort(arrivalPort.Value);
    }
  }
}

