// Paths will export some path variables that we'll
// use in other Webpack config and server files

const path = require("path");
const fs = require("fs");

const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

module.exports = {
  appAssets: resolveApp("src/assets"), // For images and other assets
  appConfig: resolveApp("config"), // App config files
  appHtml: resolveApp("src/index.html"),
  appIndexJs: resolveApp("src/index.jsx"), // Main entry point

  appSrc: resolveApp("src"), 
  appGenerated: resolveApp("x10_generated"), 

  appBuild: resolveApp("build"), // Prod built files end up here
  appBuildDev: resolveApp("dist"), // Dev built files end up here
};