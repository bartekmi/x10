using System.IO;
using System.Linq;
using System.Collections.Generic;

using x10.schema;
using x10.logictree;

namespace x10.generate.react {
    public static class GenerateTypes {

        internal static string TYPES_FILE_NAME = "types.js";

        #region Top Level
        public static void Generate() {
            using (TextWriter writer = GenerateUtils.CreateWriter(TYPES_FILE_NAME)) {
                GenerateUtils.GenerateHeader(writer, Flow.SrictLocal);

                foreach (Entity entity in Schema.Singleton.Entities)
                    GenerateFlowType(writer, entity);
            }
        }
        #endregion

        #region Flow Type
        private static void GenerateFlowType(TextWriter writer, Entity entity) {
            writer.WriteLine(string.Format("export type {0} = {{", GenerateUtils.GetFlowTypeName(entity)));

            // Properties
            //
            // NOTE: At present, we make all fields nullable. In order for a field to NEVER be nullable,
            // two conditions would have to be fulfilled:
            // 1) UI can't sent it's current value to null;
            // 2) Property must have a default
            // I considered a third condition: Property must be mandatory, but the first two
            // are sufficient.
            if (entity.Properties.Any()) {
                writer.WriteLine("  // Properties");
                foreach (Property property in entity.Properties) {
                    writer.WriteLine("  +{0}: {1} | null,",
                        property.Name,
                        property.Type.FlowType);
                }
                writer.WriteLine();
            }

            // Associations
            if (entity.Associations.Any()) {
                writer.WriteLine("  // Associations");
                foreach (Association association in entity.Associations) {
                    string template = null;
                    switch (association.Type) {
                        case Association.TypeEnum.HasMany:
                            template = "  +{0}: $ReadOnlyArray<{1}>,";
                            break;
                        case Association.TypeEnum.HasOne:
                            template = "  +{0}: {1},";
                            break;
                    }
                    writer.WriteLine(template,
                        association.Name,
                        GenerateUtils.GetFlowTypeName(association.ChildEntity));
                }
                writer.WriteLine();
            }

            // Used by React in lists. We add it to all entities - no harm done if unused
            writer.WriteLine("  +reactKey: string,");

            writer.WriteLine("};");
            writer.WriteLine();
        }

        #endregion
    }
}