using System;
using System.Linq;
using System.Text;

using x10.utils;
using x10.error;

namespace x10.schema {

    public enum DataTypeEnum {
        Boolean,
        Int,
        Float,
        String,
        Text,
        Enum,
        DateTime,
    }

    public class DataType {
        public DataTypeEnum Id { get; private set; }
        public string FlowType { get; private set; }

        // A JavaScript representation of the most reasonable value to use 
        // to represent a null value of the type when null values are not allowed
        public string NullValueAlternative { get; private set; }

        internal Func<Property, object> ConvertDefaultValue { get; private set; }

        // Derived
        public string Name { get { return Id.ToString().ToLower(); } }


        private DataType(
                DataTypeEnum id,
                string flowType,
                string nullValueAlternative,
                Func<Property, object> convertDefaultValue
        ) {
            Id = id;
            FlowType = flowType;
            NullValueAlternative = nullValueAlternative;
            ConvertDefaultValue = convertDefaultValue;
        }

        private static DataType[] DataTypes = new DataType[] {
            new DataType(DataTypeEnum.Boolean, "boolean", null, p => bool.Parse(p.DefaultValueAsString)),
            new DataType(DataTypeEnum.Int, "number", "0", p => int.Parse(p.DefaultValueAsString)),
            new DataType(DataTypeEnum.Float, "number", "0.0", p => double.Parse(p.DefaultValueAsString)),
            new DataType(DataTypeEnum.String, "string", "\"\"", p => p.DefualtValue),
            new DataType(DataTypeEnum.Text, "string", "\"\"", p => p.DefualtValue),
            new DataType(DataTypeEnum.Enum, "string", null, GetDefaultEnumValue),
            new DataType(DataTypeEnum.DateTime, "string", null, p => p.DefaultValueAsString),
        };

        private static object GetDefaultEnumValue(Property property) {
            EnumValue value = property.Enum.Parse(property.DefaultValueAsString, out string error);
            if (error != null)
                throw new Exception(error);
            return value.Name;
        }

        internal static void ParseAndSetDataType(ErrorBucket errors, string dataTypeString, Property property) {
            DataTypeEnum? typeId = null;

            // Enum types must have the form: <enumName>Enum
            if (dataTypeString.ToLower().EndsWith("enum")) {
                typeId = DataTypeEnum.Enum;
                property.EnumAsString = dataTypeString;
            }

            // Float is specified as float.n, where n is the decimal digits
            if (dataTypeString.ToLower().StartsWith("float")) {
                typeId = DataTypeEnum.Float;

                // TODO: validate
                string[] pieces = dataTypeString.Split(".");
                property.DecimalPlaces = int.Parse(pieces[1]);
            }

            if (typeId == null) {
                typeId = EnumUtils.Parse<DataTypeEnum>(dataTypeString);
                if (typeId == null) {
                    errors.Add(new Error() {
                        Message = "Invalid type value: " + dataTypeString,
                    });
                    return;
                }
            }

            // Future: validation
            DataType dataType = DataTypes.Single(x => x.Id == typeId);
            property.Type = dataType;
        }

        public static DataType FindDataType(string name) {
            return DataTypes.SingleOrDefault(x => x.Name == name);
        }

        public override string ToString() {
            return Name;
        }
    }
}