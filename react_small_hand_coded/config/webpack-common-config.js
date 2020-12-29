// This file will contain configuration data that
// is shared between development and production builds.

const HtmlWebpackPlugin = require("html-webpack-plugin");
const path = require("path");

const paths = require("./paths");

module.exports = {
  plugins: [
    new HtmlWebpackPlugin({
      inject: true,
      template: paths.appHtml,
    })
  ],
  resolve: {
    extensions: [".js", ".jsx"],
    // This is where "import" statements are resolved. 
    // This must be kept in sync with module.name_mapper rules in .flowconfig
    modules: ["node_modules", "src"],
  },
  module: {
    rules: [
      {
        test: /\.(png|svg|jpg|ico)$/,
        use: ["file-loader"]
      }
    ]
  }
};