using System;
using System.IO;
using System.Collections.Generic;

namespace x10 {
    public static class Env {
        // Root directory for schema and components
        public static string APP_DIR;

        // Output directory for generated code (relative to 'webpack/assets/javascripts')
        // Hence, the import root.
        public static string OUTPUT_DIR_WEBPACK;

        // Output directory for generated .jsx code (absolute)
        public static string OUTPUT_DIR_JSX;

        // Output directory for generated Storybook code (absolute)
        public static string OUTPUT_DIR_STORYBOOK;

        private static void ConfigureCargoEditor() {
            OUTPUT_DIR_WEBPACK = "hackathon";
            APP_DIR = UserPath("flexport/script/x10/x10/examples/cargoeditor");
            OUTPUT_DIR_JSX = UserPath(Path.Combine("flexport/webpack/assets/javascripts", OUTPUT_DIR_WEBPACK));
            OUTPUT_DIR_STORYBOOK = UserPath("flexport/webpack/assets/javascripts/components/stories/client/quote_request");
        }

        private static void ConfigureLoyalty() {
            OUTPUT_DIR_WEBPACK = "hackathon/loyalty";
            APP_DIR = UserPath("flexport/script/x10/x10/examples/loyalty");
            OUTPUT_DIR_JSX = UserPath(Path.Combine("flexport/webpack/assets/javascripts", OUTPUT_DIR_WEBPACK));
            OUTPUT_DIR_STORYBOOK = UserPath("flexport/webpack/assets/javascripts/components/stories/client/loyalty");
        }

        private static string UserPath(string path) {
            string home = Environment.GetEnvironmentVariable("HOME");
            return Path.Combine(home, path);
        }

        internal static void Configure() {
            ConfigureCargoEditor();
            // ConfigureLoyalty();
        }
    }
}