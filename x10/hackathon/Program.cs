using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using x10.schema;
using x10.logictree;
using x10.error;
using x10.generate.react;

namespace x10 {
    class Program {
        static void Main(string[] args) {
            Env.Configure();

            string[] relativePaths = new string[] {
                // Loyalty
                // "components/CreatePoolTemplate.xml",
                
                // Cargo Editor
                "components/CargoEditor.xml",
                "components/CargoTotals.xml",
                "components/PackageDetails.xml",
            };

            Compiler compiler = new Compiler();
            compiler.CompilePass1(relativePaths);
            compiler.CompilePass2();

            if (compiler.ErrorBucket.HasErrors) {
                Console.WriteLine("**************** ERRORS during compilation:");
                foreach (Error error in compiler.ErrorBucket.Errors)
                    Console.WriteLine(error);
            } else {
                foreach (ElementDef element in compiler.Elements)
                    GenerateCode(element);

                GenerateTypes.Generate();
                GenerateInitialData.Generate();
                Console.WriteLine("SUCCESS!!!");
            }
        }

        private static void GenerateCode(ElementDef element) {
            GenerateFunctionalComponent.Generate(element, false);
            GenerateStorybook.Generate(element);
        }

        private static void PrintElement(ElementDef element) {
            StringBuilder builder = new StringBuilder();
            element.PrettyPrint(builder, 0);
            Console.WriteLine(builder.ToString());
        }

        private static void PrintSchema() {
            Schema schema = Schema.Singleton;
            string output = JsonConvert.SerializeObject(schema, Formatting.Indented);
            Console.WriteLine(output);
        }
    }
}
