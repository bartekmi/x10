// configuration data related to development only

const path = require("path");
const webpack = require("webpack");
const CopyPlugin = require("copy-webpack-plugin");
const {merge} = require("webpack-merge");

const paths = require("./paths");
const common = require("./webpack-common-config.js");

module.exports = merge(common, {
  entry: [paths.appIndexJs],
  mode: "development",
  output: {
    path: paths.appBuildDev,
    publicPath: "/" // e.g. vendors~DatePickerOverride.js
  },
  // devtool option controls if and how source maps are generated.
  // see https://webpack.js.org/configuration/devtool/
  // If you find that you need more control of source map generation,
  // see https://webpack.js.org/plugins/source-map-dev-tool-plugin/
  devtool: "#inline-source-map",
  plugins: [
    new webpack.HotModuleReplacementPlugin(),
    new webpack.NamedModulesPlugin(),
    new webpack.DefinePlugin({
      "process.env": {
        NODE_ENV: JSON.stringify("development")
      }
    }),
    new CopyPlugin({
      patterns: [
        {from: "public", to: "public" }
      ]
    }),
  ],
  module: {
    rules: [
      {
        test: /\.jsx?$/,
        include: path.resolve(paths.appSrc),
        loader: "babel-loader", // Options in .babelrc
      },
      {
        test: /\.jsx?$/,
        include: path.resolve(paths.appGenerated),
        loader: "babel-loader", // Options in .babelrc
      },
      {
        test: /\.jsx?$/,
        include: /node_modules\/latitude\/.*/,
        exclude: /node_modules\/latitude\/node_modules\/.*/,
        use: {
          loader: "babel-loader",
          options: {
            presets: [
              "@babel/preset-env",
              "@babel/preset-react",
              "@babel/preset-flow"
            ],
            "plugins": [
              "@babel/plugin-proposal-class-properties",
            ]
          }
        }
      },      
      {
        test: /\.(css|scss)$/,
        use: ["style-loader", "css-loader"]
      }
    ]
  }
});