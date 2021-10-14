using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  // Enums
  public enum DocumentTypeEnum {
    BusinessLicense,
    BusinessRegistration,
    PowerOfAttorney,
    Etc,
  }


  /// <summary>
  /// Metadata about a document
  /// </summary>
  public class Document : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? FileName { get; set; }
    [GraphQLNonNullType]
    public DocumentTypeEnum? DocumentType { get; set; }
    public DateTime? UploadedTimestamp { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Document: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public User? UploadedBy { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? uploadedBy = IdUtils.FromRelayId(UploadedBy?.Id);
      UploadedBy = uploadedBy == null ? null : repository.GetUser(uploadedBy.Value);
    }
  }
}

