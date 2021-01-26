using System;

namespace x10.hotchoc.Entities {
  public class Move : EntityBase {

    // Regular Attributes
    public DateTime? Date { get; set; }

    // Associations
    public virtual Building? From { get; set; }   // Should be unit, but more complex
    public virtual Building? To { get; set; }     // Should be unit, but more complex
    public virtual Tenant? Tenant { get; set; }
  }
}
