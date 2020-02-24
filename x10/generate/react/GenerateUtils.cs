using System;
using System.IO;

using x10.logictree;
using x10.schema;

namespace x10.generate.react {

    internal enum Flow {
        SrictLocal,
        JustFlow,
    }

    internal static class GenerateUtils {
        internal static void GenerateHeader(TextWriter writer, ElementDef elementDef, Flow flow) {
            writer.WriteLine(
@"/**
 * AUTO-GENERATED at {0} from {1}
 * TEAM: {2}
 * @flow {3}
 */", DateTime.Now, elementDef.Path, elementDef.Team, GetFlowTag(flow));
            writer.WriteLine();
        }

        internal static void GenerateHeader(TextWriter writer, Flow flow) {
            writer.WriteLine(
@"/**
 * AUTO-GENERATED at {0}
 * TEAM: bookings
 * @flow {1}
 */", DateTime.Now, GetFlowTag(flow));
            writer.WriteLine();
        }

        private static string GetFlowTag(Flow flow) {
            switch (flow) {
                case Flow.SrictLocal: return "strict-local";
                case Flow.JustFlow: return "";
                default:
                    throw new Exception("Fix you code!");
            }
        }

        internal static TextWriter CreateWriter(string filename, string outDir = null) {
            if (outDir == null)
                outDir = Env.OUTPUT_DIR_JSX;

            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            return new StreamWriter(GetOutPath(filename, outDir));
        }

        internal static string GetOutPath(string filename, string outDir) {
            return Path.Combine(outDir, filename);
        }

        internal static string GetLocalImportPath(string filename) {
            string noExtension = Path.GetFileNameWithoutExtension(filename);
            return Path.Combine(Env.OUTPUT_DIR_WEBPACK, noExtension);
        }

        internal static string GetFlowTypeName(Entity entity) {
            return entity.Name + "Type";
        }

        // Return properly formatted default value JavaScript, or 'null'
        // if no default value exists
        internal static string DefaultValueJavaScript(Property property) {
            object _default = property.DefualtValue;
            if (_default == null)
                return "null";

            return _default is string ?
                "\"" + _default + "\"" :       // Strings must be in quotes
                _default.ToString().ToLower();
        }
    }
}