/** @type {import("snowpack").SnowpackUserConfig } */
module.exports = {
  mount: {
    public: "/",
    fableOut: "/dist",

  },
  plugins: [
    "snowpack-plugin-hash",
    "@snowpack/plugin-postcss",
    "@jadex/snowpack-plugin-tailwindcss-jit",
  ],
  optimize: {
    bundle: true,
    minify: true,
    target: "es2020",
  },
};
