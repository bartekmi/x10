using System;
using System.Collections.Generic;
using System.Text;
using x10.formula;
using x10.model.libraries;

namespace x10.model.definition {

  public class Validation : ModelComponent {
    public string Message { get; set; }
    public string Trigger { get; set; }
    public Entity Owner { get; set; }

    private ExpBase _triggerExpression;
    public ExpBase TriggerExpression {
      get {
        if (_triggerExpression == null) {
          ModelAttributeValue triggerAttrValue = this.FindAttribute(BaseLibrary.TRIGGER);
          _triggerExpression = triggerAttrValue.Expression;
        }
        return _triggerExpression;
      }
    }


    public Validation() {
      // Do nothing
    }
  }
}
