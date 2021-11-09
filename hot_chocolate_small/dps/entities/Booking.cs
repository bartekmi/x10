using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  // Enums
  public enum BookingStgeEnum {
    Archived,
    Booked,
    Draft,
    Shipment,
    Submitted,
  }


  /// <summary>
  /// Will eventually become a full-fledged Shipment
  /// </summary>
  public class Booking : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public bool Ocean_fcl { get; set; }
    [GraphQLNonNullType]
    public bool Ocean_lcl { get; set; }
    [GraphQLNonNullType]
    public bool Truck_ftl { get; set; }
    [GraphQLNonNullType]
    public bool Truck_ltl { get; set; }
    [GraphQLNonNullType]
    public bool Air { get; set; }
    public BookingStgeEnum? Booking_stage { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? Cargo_ready_date { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Booking: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public CompanyEntity? Shipper_entity { get; set; }
    [GraphQLNonNullType]
    public CompanyEntity? Consignee_entity { get; set; }
    [GraphQLNonNullType]
    public Cargo? Cargo { get; set; }
    [GraphQLNonNullType]
    public Shipment? Shipment { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Cargo?.EnsureUniqueDbid();
      Shipment?.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? shipper_entity = IdUtils.FromRelayId(Shipper_entity?.Id);
      Shipper_entity = shipper_entity == null ? null : repository.GetCompanyEntity(shipper_entity.Value);

      int? consignee_entity = IdUtils.FromRelayId(Consignee_entity?.Id);
      Consignee_entity = consignee_entity == null ? null : repository.GetCompanyEntity(consignee_entity.Value);

      Cargo?.SetNonOwnedAssociations(repository);

      Shipment?.SetNonOwnedAssociations(repository);
    }
  }
}

