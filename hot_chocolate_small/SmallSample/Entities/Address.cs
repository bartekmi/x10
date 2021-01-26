using HotChocolate;

namespace x10.hotchoc.Entities {
  public class Address : EntityBase {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? UnitNumber { get; set; }
    [GraphQLNonNullType]
    public string? TheAddress { get; set; }
    [GraphQLNonNullType]
    public string? City { get; set; }
    [GraphQLNonNullType]
    public string? StateOrProvince { get; set; }
    [GraphQLNonNullType]
    public string? Zip { get; set; }

    // Associations
  }
}