using System;

namespace Small.Entities {
  public class Move : EntityBase {

    // Regular Attributes
    public DateTime? Date { get; set; }

    // Associations
    public virtual Unit? From { get; set; }
    public virtual Unit? To { get; set; }
    public virtual Tenant? Tenant { get; set; }
  }
}
