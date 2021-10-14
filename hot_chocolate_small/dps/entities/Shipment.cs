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

  public enum ShipmentPriorityEnum {
    Standard,
    High,
  }


  /// <summary>
  /// A portion of Core Shipment entity
  /// </summary>
  public class Shipment : Base {
    // Regular Attributes
    public int? Dbid { get; set; }
    [GraphQLNonNullType]
    public string? FlexId { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    public ShipmentPriorityEnum? Priority { get; set; }
    public TransportationModeEnum? TransportationMode { get; set; }
    [GraphQLNonNullType]
    public string? Status { get; set; }
    public DateTime? CargoReadyDate { get; set; }
    public DateTime? ActualDepartureDate { get; set; }
    public DateTime? ArrivalDate { get; set; }
    [GraphQLNonNullType]
    public bool IsLcl { get; set; }
    [GraphQLNonNullType]
    public bool IsLtl { get; set; }
    [GraphQLNonNullType]
    public string? Customs { get; set; }
    public DateTime? DueDate { get; set; }
    [GraphQLNonNullType]
    public string? DueDateTask { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Shipment: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    public CompanyEntity? Consignee { get; set; }
    public CompanyEntity? Shipper { get; set; }
    public Port? DeparturePort { get; set; }
    public Port? ArrivalPort { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? consignee = IdUtils.FromRelayId(Consignee?.Id);
      Consignee = consignee == null ? null : repository.GetCompanyEntity(consignee.Value);

      int? shipper = IdUtils.FromRelayId(Shipper?.Id);
      Shipper = shipper == null ? null : repository.GetCompanyEntity(shipper.Value);

      int? departurePort = IdUtils.FromRelayId(DeparturePort?.Id);
      DeparturePort = departurePort == null ? null : repository.GetPort(departurePort.Value);

      int? arrivalPort = IdUtils.FromRelayId(ArrivalPort?.Id);
      ArrivalPort = arrivalPort == null ? null : repository.GetPort(arrivalPort.Value);
    }
  }
}

