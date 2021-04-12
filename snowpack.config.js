/** @type {import("snowpack").SnowpackUserConfig } */
module.exports = {
  mount: {
    public: "/",
    fableOut: "/dist",
  },
  plugins: [
    [
      "@snowpack/plugin-run-script",
      {
        cmd: "dotnet fable ./src/App/App.fsproj --outDir ./fableOut",
        watch: "dotnet fable watch ./src/App/App.fsproj --outDir ./fableOut",
      },
    ],
  ],
  optimize: {
    bundle: true,
    minify: true,
    target: "es2020",
  },
};
