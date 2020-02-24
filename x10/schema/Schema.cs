using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using x10.error;

namespace x10.schema {
    public class Schema {
        // At present, Schema is a singleton. In the future, there could be a Schema
        // for each API, for example
        private static Schema _schema;
        public static Schema Singleton {
            get {
                if (_schema == null) {
                    SchemaParser parser = new SchemaParser();
                    IEnumerable<Entity> entities = parser.Parse(out IEnumerable<X10Enum> sharedEnums);
                    _schema = new Schema(entities, sharedEnums);
                    _schema.PostProcess(parser.ErrorBucket);
                }
                return _schema;
            }
        }

        public IEnumerable<Entity> Entities { get; set; }
        public IEnumerable<X10Enum> Enums { get; set; }

        private Dictionary<string, Entity> _entitiesByName = new Dictionary<string, Entity>();
        private Dictionary<string, X10Enum> _enumsByName = new Dictionary<string, X10Enum>();

        internal Schema(IEnumerable<Entity> entities, IEnumerable<X10Enum> sharedEnums) {
            Entities = entities;
            Enums = sharedEnums;

            _entitiesByName = entities.ToDictionary(x => x.Name);
            _enumsByName = sharedEnums.ToDictionary(x => x.Name);
        }

        public Entity FindEntityByName(string entityName) {
            _entitiesByName.TryGetValue(entityName, out Entity entity);
            return entity;
        }

        public X10Enum FindGlobalEnumByName(string enumName) {
            _enumsByName.TryGetValue(enumName, out X10Enum anEnum);
            return anEnum;
        }

        private void PostProcess(ErrorBucket errors) {
            PostProcessEnums(errors);
            PostProcessDefaultValues(errors);
            PostProcessAssociations(errors);
        }

        private void PostProcessEnums(ErrorBucket errors) {
            foreach (Property property in Entities.SelectMany(x => x.Properties))
                if (property.Type.Id == DataTypeEnum.Enum)
                    property.Enum = FindEnum(property);
        }

        private void PostProcessDefaultValues(ErrorBucket errors) {
            foreach (Property property in Entities.SelectMany(x => x.Properties))
                if (property.HasDefault)
                    // TODO: Validation
                    property.DefualtValue = property.Type.ConvertDefaultValue(property);
        }

        private X10Enum FindEnum(Property property) {
            string enumName = property.EnumAsString;

            X10Enum theEnum = property.Owner.Enums.SingleOrDefault(x => x.Name == enumName);
            if (theEnum == null)
                theEnum = FindGlobalEnumByName(property.EnumAsString);

            if (theEnum == null)
                // TODO: Validation
                throw new Exception(string.Format("Unknown enum {0} in {1}.{2}",
                    enumName, property.Owner.Name, property.Name));

            return theEnum;
        }

        private void PostProcessAssociations(ErrorBucket errors) {
            foreach (Association association in Entities.SelectMany(x => x.Associations))
                // TODO: Validation
                association.ChildEntity = _entitiesByName[association.ChildEntityName];
        }
    }
}