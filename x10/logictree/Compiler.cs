using System;
using System.Collections.Generic;
using System.IO;

using x10.error;
using x10.complib;

namespace x10.logictree {
    public class Compiler {
        public List<ElementDef> Elements = new List<ElementDef>();
        // Warning! Set only once. No way to clear.        
        public ErrorBucket ErrorBucket = new ErrorBucket();

        private HashSet<ElementDef> _passTwoInProgress = new HashSet<ElementDef>();

        public void CompilePass1(IEnumerable<string> relativePaths) {
            foreach (string relativePath in relativePaths) {
                string absPath = Path.Combine(Env.APP_DIR, relativePath);
                ElementDef element = TreeBuilder.BuildPass1(absPath, out ErrorBucket errors);
                if (element != null) {
                    // Register this new component                
                    ComponentDefComposite componentDef = new ComponentDefComposite(element) {
                        ImportFile = Path.Combine(Env.OUTPUT_DIR_WEBPACK, element.Name),
                        // In future, if we have a method to define params, they would
                        // be added here, too alongside the defaults 
                        Params = new ParamDef[] {
                            new ParamDef() { Name = "showErrors", Type = ParamType.VarShowErrors },
                            new ParamDef() { Name = "data", Type = ParamType.VarInputData },
                            new ParamDef() { Name = "onDataChanged", Type = ParamType.VarCallback },
                        }
                    };
                    element.Component = componentDef;
                    ComponentLibrary.Singleton.Register(componentDef);
                }

                ErrorBucket.Add(errors);

                if (element != null)
                    Elements.Add(element);
            }
        }

        public void CompilePass2() {
            foreach (ElementDef element in Elements) {
                _passTwoInProgress.Add(element);
                TreeBuilder.BuildPass2(element, out ErrorBucket errors);
                ErrorBucket.Add(errors);
            }
        }
    }
}