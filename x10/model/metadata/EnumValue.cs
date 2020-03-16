using System;
using System.Collections.Generic;

using x10.model.definition;

namespace x10.model.metadata
{
    public class EnumValue
    {
        public object Value {get;set;}
        public string Label {get;set;}
        public List<ModelAttributeValue> Attributes {get;set;}
    }
}