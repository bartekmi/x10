using System;
using System.Collections.Generic;
using System.Text;

namespace x10.formula {
  public interface IVisitor {
    public void VisitBinary(ExpBinary exp);
    public void VisitIdentifier(ExpIdentifier exp);
    public void VisitInvocation(ExpInvocation exp);
    public void VisitLiteral(ExpLiteral exp);
    public void VisitMemberAccess(ExpMemberAccess exp);
    public void VisitParenthesized(ExpParenthesized exp);
    public void VisitUnknown(ExpUnknown exp);
  }
}
