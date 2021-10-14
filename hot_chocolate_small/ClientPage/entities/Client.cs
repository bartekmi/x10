using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  // Enums
  public enum ClientStatusEnum {
    Lead,
    Test,
    Demo,
    Active,
    Churned,
    Deactivated,
  }

  public enum ClientSegmentEnum {
    Emerging,
    Smb,
    MidMarket,
    NotQualified,
    Unknown,
    Enterprise,
  }

  public enum ClientPurchasingBehaviorEnum {
    Price,
    Value,
  }

  public enum ClientPrimaryShipmentRoleEnum {
    Unknown,
    Shipper,
    Consignee,
  }


  /// <summary>
  /// More information about a company that we do business with as a client (we sell them stuff)
  /// </summary>
  public class Client : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? SalesforceAccountRef { get; set; }
    [GraphQLNonNullType]
    public string? ReferredBy { get; set; }
    [GraphQLNonNullType]
    public ClientStatusEnum? Status { get; set; }
    [GraphQLNonNullType]
    public ClientSegmentEnum? Segment { get; set; }
    public ClientPurchasingBehaviorEnum? PurchasingBehavior { get; set; }
    [GraphQLNonNullType]
    public ClientPrimaryShipmentRoleEnum? PrimaryShipmentRole { get; set; }
    [GraphQLNonNullType]
    public int? ShipmentsAsClient { get; set; }
    [GraphQLNonNullType]
    public int? ShipmentsAsShipper { get; set; }
    [GraphQLNonNullType]
    public int? ShipmentsAsConsignee { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Client: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Company? Company { get; set; }
    [GraphQLNonNullType]
    public Contact? PrimaryContact { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Company?.EnsureUniqueDbid();
      PrimaryContact?.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      Company?.SetNonOwnedAssociations(repository);

      PrimaryContact?.SetNonOwnedAssociations(repository);
    }
  }
}

