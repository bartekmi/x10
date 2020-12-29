using HotChocolate;
namespace Small.Entities {
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