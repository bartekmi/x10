using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  // Enums
  public enum CtpatReviewStatusEnum {
    Compliant,
    GracePeriod,
    NonCompliant,
    Provisional,
  }


  /// <summary>
  /// Status of the current CTPAT review
  /// </summary>
  public class CtpatReview : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public bool ComplianceScreenRequired { get; set; }
    public CtpatReviewStatusEnum? Status { get; set; }
    public DateTime? ExpiresAt { get; set; }
    [GraphQLNonNullType]
    public string? ComplianceContactEmail { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "CtpatReview: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);
    }
  }
}

