using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;

namespace x10.gen.sql.primitives {
  public class Row {
    public int Id { get; internal set; }
    public Entity Entity { get; internal set; }
    public List<MemberAndValue> Values = new List<MemberAndValue>();
    public Dictionary<Association, List<Row>> ChildAssociations { get; private set; }

    public object ValueFor(Member member) {
      MemberAndValue memberAndValue = Values.Single(x => x.Member == member);
      return memberAndValue.Value;
    }

    internal void AddChildAssociation(Association association, Row row) {
      if (ChildAssociations == null)
        ChildAssociations = new Dictionary<Association, List<Row>>();

      if (!ChildAssociations.TryGetValue(association, out List<Row> rows)) {
        rows = new List<Row>();
        ChildAssociations[association] = rows;
      }
      rows.Add(row);
    }

    public object[] ValueForSql { get; internal set; }
  }
}