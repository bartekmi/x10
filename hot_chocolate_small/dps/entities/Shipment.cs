using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  // Enums
  public enum TransportationModeEnum {
    Air,
    Ocean,
    Truck,
    Rail,
    UnknownTransportation,
    TruckIntl,
    WarehouseStorage,
  }


  /// <summary>
  /// A portion of Core Shipment entity
  /// </summary>
  public class Shipment : Base {
    // Regular Attributes
    public int? CoreId { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    public TransportationModeEnum? TransportationMode { get; set; }
    [GraphQLNonNullType]
    public string? Status { get; set; }
    [GraphQLNonNullType]
    public string? Customs { get; set; }
    public DateTime? CargoReady { get; set; }
    public DateTime? DepartsDate { get; set; }
    [GraphQLNonNullType]
    public string? DepartsLocation { get; set; }
    public DateTime? ArrivesDate { get; set; }
    [GraphQLNonNullType]
    public string? ArrivesLocation { get; set; }
    public DateTime? DueDate { get; set; }
    [GraphQLNonNullType]
    public string? DueDateTask { get; set; }
    [GraphQLNonNullType]
    public bool IsLcl { get; set; }
    [GraphQLNonNullType]
    public bool IsLtl { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Shipment: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    public CompanyEntity? Consignee { get; set; }
    public CompanyEntity? Shipper { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? consignee = IdUtils.FromRelayId(Consignee?.Id);
      Consignee = consignee == null ? null : repository.GetCompanyEntity(consignee.Value);

      int? shipper = IdUtils.FromRelayId(Shipper?.Id);
      Shipper = shipper == null ? null : repository.GetCompanyEntity(shipper.Value);
    }
  }
}

