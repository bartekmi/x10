using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Small.Entities {
  public class Tenant : EntityBase {

    // Regular Attributes
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    // Associations
    public Address? PermanentMailingAddress { get; set; }
  }
}
