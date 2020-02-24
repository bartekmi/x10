using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using x10.logictree;
using x10.complib;
using x10.schema;
using x10.schema.validation;
using x10.utils;

namespace x10.generate.react {

    public static class GenerateInitialData {

        internal static string INITIAL_DATA_FILE_NAME = "initialData.js";

        #region Top Level
        public static void Generate() {
            using (TextWriter writer = GenerateUtils.CreateWriter(INITIAL_DATA_FILE_NAME)) {
                GenerateUtils.GenerateHeader(writer, Flow.SrictLocal);

                writer.WriteLine("import uuid from \"uuid/v4\";");
                writer.WriteLine();

                foreach (Entity entity in Schema.Singleton.Entities)
                    GenerateFunction(writer, entity);
            }
        }
        #endregion

        #region Initial Data
        private static void GenerateFunction(TextWriter writer, Entity entity) {
            writer.WriteLine(
@"export function {0}() {{
  return Object.freeze({{", GetFunctionName(entity));

            // Properties
            if (entity.Properties.Any()) {
                writer.WriteLine("    // Properties");
                foreach (Property property in entity.Properties)
                    writer.WriteLine("    {0}: {1},",
                        property.Name,
                        GenerateUtils.DefaultValueJavaScript(property));
                writer.WriteLine();
            }

            // Associations
            if (entity.Associations.Any()) {
                writer.WriteLine("    // Associations");
                foreach (Association association in entity.Associations) {
                    string template = null;
                    switch (association.Type) {
                        case Association.TypeEnum.HasMany:
                            template = "    {0}: [{1}()],";
                            break;
                        case Association.TypeEnum.HasOne:
                            template = "    {0}: {1}(),";
                            break;
                    }
                    writer.WriteLine(template,
                        association.Name,
                        GetFunctionName(association.ChildEntity));
                }
                writer.WriteLine();
            }

            // TODO: Consider if we should us DB id for entities fetched from db
            writer.WriteLine("    reactKey: uuid(),");

            writer.WriteLine("  });");
            writer.WriteLine("}");
            writer.WriteLine();
        }
        #endregion

        public static string GetFunctionName(Entity entity) {
            return string.Format("{0}InitialData",
                NameUtils.Uncapitalize(entity.Name));
        }
    }
}
