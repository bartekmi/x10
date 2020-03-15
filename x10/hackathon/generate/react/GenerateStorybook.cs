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

    public static class GenerateStorybook {

        private const string STORYBOOK_SECTION = "sections.client.quickQuote";

        #region Top Level
        public static void Generate(ElementDef elementDef) {
            string outFile = Path.GetFileNameWithoutExtension(elementDef.Path) + ".stories.jsx";

            using (TextWriter writer = GenerateUtils.CreateWriter(outFile, Env.OUTPUT_DIR_STORYBOOK)) {
                GenerateUtils.GenerateHeader(writer, elementDef, Flow.JustFlow);
                GenerateImports(writer, elementDef);
                GenerateMisc(writer, elementDef);
                GenerateWrapperFunctionalComponent(writer, elementDef);
            }
        }
        #endregion

        #region Imports
        private static void GenerateImports(TextWriter writer, ElementDef elementDef) {
            writer.WriteLine(
@"import React, {useState} from ""react"";

import {storiesOf} from ""@storybook/react"";
import {withKnobs, boolean} from ""@storybook/addon-knobs"";
import sections from ""components/stories/sections"";
import {action} from ""@storybook/addon-actions"";");
            writer.WriteLine();

            // Import Initial Data
            string initialData = GenerateInitialData.GetFunctionName(elementDef.DataModel);
            writer.WriteLine(@"import {{ {0} }} from ""{1}"";",
                 initialData,
                 GenerateUtils.GetLocalImportPath(GenerateInitialData.INITIAL_DATA_FILE_NAME));

            // Import the actual component
            string componentName = elementDef.Name;
            writer.WriteLine(@"import {0} from ""{1}"";",
                componentName,
                GenerateUtils.GetLocalImportPath(componentName));

            writer.WriteLine();
        }
        #endregion

        #region Misc
        private static void GenerateMisc(TextWriter writer, ElementDef elementDef) {
            writer.WriteLine(
@"const stories = storiesOf({0}, module);
stories.addDecorator(withKnobs);

stories.add(""X10 {1}"", () => <{2}Wrapper />);",
            STORYBOOK_SECTION,
            NameUtils.ToHuman(elementDef.Name),
            elementDef.Name);

            writer.WriteLine();
        }
        #endregion

        #region Wrapper Functional Component
        private static void GenerateWrapperFunctionalComponent(TextWriter writer, ElementDef elementDef) {
            string componentName = elementDef.Name;
            string entityName = elementDef.DataModel.Name;
            string initialDataFunc = GenerateInitialData.GetFunctionName(elementDef.DataModel);
            string initialValue = elementDef.IsDataModelMultiple ?
              string.Format("[{0}()]", initialDataFunc) :
              string.Format("{0}()", initialDataFunc);


            writer.WriteLine(
@"function {0}Wrapper() {{
  const [{1}, set{2}] = useState({3});

  return (
    <{0}
      data={{ {1} }}
      onDataChanged={{new{2} => {{
        set{2}(new{2});
        action(""on{2}Changed"")(new{2});
      }} }}
      showErrors={{boolean(""Show Errors"", false)}}
    />
  );
}}",
                        componentName,
                        NameUtils.Uncapitalize(entityName),
                        entityName,
                        initialValue);
        }
        #endregion

    }
}
