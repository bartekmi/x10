namespace Small.Entities {
  public class Address : EntityBase {
    // Regular Attributes
    public string? UnitNumber { get; set; }
    public string? TheAddress { get; set; }
    public string? City { get; set; }
    public string? StateOrProvince { get; set; }
    public string? Zip { get; set; }

    // Associations
  }
}