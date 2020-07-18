using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wpf_lib.lib.attributes;

namespace wpf_lib.lib.utils {

  public class EnumValueRepresentation {
    public string Label { get; }
    public object Value { get; }

    public EnumValueRepresentation(object value, string label) {
      Value = value;
      Label = label;
    }

    public override string ToString() {
      return Label;
    }

    public static IEnumerable<EnumValueRepresentation> GetForEnumType(Type type) {
      List<EnumValueRepresentation> values = new List<EnumValueRepresentation>();
      foreach (object value in Enum.GetValues(type)) {
        MemberInfo memberInfo = type.GetMember(value.ToString()).Single();
        LabelAttribute labelAttr = (LabelAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(LabelAttribute));
        
        string label = labelAttr == null ?
          NameUtils.CamelCaseToHumanReadable(value.ToString()) :
          labelAttr.Label;

        values.Add(new EnumValueRepresentation(value, label));
      }
      return values;
    }
  }
}
