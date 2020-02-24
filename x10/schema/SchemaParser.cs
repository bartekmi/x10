using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using YamlDotNet.RepresentationModel;

using x10.utils;
using x10.schema.validation;
using x10.error;

namespace x10.schema {
    public class SchemaParser {

        public ErrorBucket ErrorBucket;

        public IEnumerable<Entity> Parse(out IEnumerable<X10Enum> enums) {
            ErrorBucket = new ErrorBucket();

            // Shared Enums
            string sharedEnumsPath = Path.Combine(Env.APP_DIR, "sharedEnums.yaml");
            YamlSequenceNode yamlEnums = (YamlSequenceNode)YamlUtils.ReadYaml(sharedEnumsPath).RootNode;
            enums = ParseEnums(yamlEnums);

            // Entities
            List<Entity> entities = new List<Entity>();

            string dirPath = Path.Combine(Env.APP_DIR, "entities");
            foreach (string path in Directory.EnumerateFiles(dirPath)) {
                // ErrorBucket.SetFile = path;
                if (!path.EndsWith(".yaml"))
                    continue;
                entities.Add(ParseEntityFile(path));
            }

            return entities;
        }

        #region Shared Enums
        private List<X10Enum> ParseEnums(YamlSequenceNode yamlEnums) {
            List<X10Enum> enums = new List<X10Enum>();

            foreach (YamlMappingNode yamlEnum in yamlEnums)
                enums.Add(ParseEnum(yamlEnum));

            return enums;
        }
        #endregion

        #region Entities
        private Entity ParseEntityFile(string path) {
            YamlMappingNode yamlEntity = (YamlMappingNode)YamlUtils.ReadYaml(path).RootNode;
            Entity entity = new Entity() {
                Name = YamlUtils.GetString(yamlEntity, "name"),
                Description = YamlUtils.GetString(yamlEntity, "description"),
                Properties = YamlUtils.GetSequence(yamlEntity, "properties")
                    .Select(x => ParseProperty((YamlMappingNode)x))
                    .ToArray(),
                Associations = YamlUtils.GetSequence(yamlEntity, "associations")
                    .Select(x => ParseAssociation((YamlMappingNode)x))
                    .ToArray(),
                Enums = YamlUtils.GetSequence(yamlEntity, "enums")
                    .Select(x => ParseEnum((YamlMappingNode)x))
                    .ToArray(),
            };

            foreach (Property property in entity.Properties)
                property.Owner = entity;

            return entity;
        }

        private Property ParseProperty(YamlMappingNode yamlProperty) {
            Property property = new Property() {
                Name = YamlUtils.GetString(yamlProperty, "name"),
                Description = YamlUtils.GetString(yamlProperty, "description"),
                LabelOverride = YamlUtils.GetString(yamlProperty, "label"),
                Units = YamlUtils.GetEnum<Units>(yamlProperty, "units"),
                DefaultValueAsString = YamlUtils.GetString(yamlProperty, "default"),
                UiOverrideAsString = YamlUtils.GetString(yamlProperty, "ui"),
                IsRelevant = YamlUtils.GetString(yamlProperty, "isRelevant"),
            };

            if (YamlUtils.GetBoolean(yamlProperty, "mandatory")) {
                property.Validations.Add(new ValidationMandatory());
            }

            string dataTypeString = YamlUtils.GetString(yamlProperty, "dataType");
            DataType.ParseAndSetDataType(ErrorBucket, dataTypeString, property);

            return property;
        }

        private Association ParseAssociation(YamlMappingNode yamlAssociation) {
            Association association = new Association() {
                Name = YamlUtils.GetString(yamlAssociation, "name"),
            };

            string hasMany = YamlUtils.GetString(yamlAssociation, "hasMany");
            string hasOne = YamlUtils.GetString(yamlAssociation, "hasOne");


            if (hasMany != null) {
                association.ChildEntityName = hasMany;
                association.Type = Association.TypeEnum.HasMany;
            } else if (hasOne != null) {
                association.ChildEntityName = hasOne;
                association.Type = Association.TypeEnum.HasOne;
            } else
                // Future: validation
                throw new Exception("Nither hasMany nor hasOne");

            return association;
        }

        private X10Enum ParseEnum(YamlMappingNode yamlEnum) {
            X10Enum anEnum = new X10Enum() {
                Name = YamlUtils.GetString(yamlEnum, "name"),
                Values = YamlUtils.GetCommaSeparatedString(yamlEnum, "values")
                    .Select(x => new EnumValue() {
                        Name = x,
                    })
                    .ToArray(),
            };

            // TODO: This only allows for simple comma-separated lists
            // Also allow for the full format where you can specify icon, decription, etc
            string[] values = YamlUtils.GetCommaSeparatedString(yamlEnum, "values");
            int index = 0;
            anEnum.Values = values
                .Select(x => new EnumValue() {
                    IntValue = index++,
                    Name = x,
                    Label = NameUtils.Capitalize(x),
                })
                .ToArray();

            return anEnum;
        }

        #endregion
    }
}