module.exports = {
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    "plugin:vue/essential",
    "airbnb-base",
    "plugin:prettier/recommended",
    "eslint:recommended",
  ],
  parserOptions: {
    ecmaVersion: 12,
    sourceType: "module",
  },
  plugins: ["vue", "prettier"],
  rules: {
    "no-unused-vars": "warn",
    "prettier/prettier": "error",
    "import/no-unresolved": "off",
    "import/no-extraneous-dependencies": "off",
    "no-console": "off",
    "no-empty-function": "warn",
    "no-plusplus": "off",
    "no-restricted-globals": "off",
    "no-alert": "off",
    "no-continue": "off",
    "vue/no-v-model-argument": "off",
  },
};
