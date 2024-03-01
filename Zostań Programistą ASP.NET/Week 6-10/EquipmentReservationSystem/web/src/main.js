import "bootstrap-vue/dist/bootstrap-vue.css";
import "bootstrap/dist/css/bootstrap.css";

import { createApp } from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
// import { BootstrapVue, IconsPlugin } from "bootstrap-vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

// axios instance

axios.defaults.baseURL = "https://localhost:7166/api/";
axios.defaults.headers.common["content-type"] = "application/json";

const refreshAxios = axios.create({
  baseURL: `${axios.defaults.baseURL}Auth`,
});

// axios interceptors

const setAuthHeders = (config) => {
  const accessToken = store.state.authInfo.token;
  if (accessToken == null) return config;
  // eslint-disable-next-line
  config.headers.authorization = `Bearer ${accessToken}`;
  return config;
};

axios.interceptors.request.use((config) => {
  setAuthHeders(config);
  return config;
});

axios.interceptors.response.use(
  (res) => {
    return res;
  },
  async (error) => {
    if (
      axios.isAxiosError(error) &&
      error.response &&
      error.response.status === 401 &&
      store.state.authInfo.refresh
    ) {
      try {
        const newToken = await refreshAxios.post(
          `RefreshToken?token=${store.state.authInfo.refresh}`
        );
        if (newToken.status !== 200) {
          store.commit("removeAccerssToken");
          router.push({ name: "login" });
          return Promise.reject(error);
        }
        store.commit("setAccessToken", newToken.data);
        return axios(setAuthHeders(error.config));
      } catch (innsererror) {
        store.commit("removeAccerssToken");
        router.push({ name: "login" });
        return Promise.reject(error);
      }
    }
    return Promise.reject(error);
  }
);

// general

store.commit("recoveryAccessToken");

createApp(App)
  .use(store)
  .use(router)
  .use(VueAxios, axios)
  // .use(BootstrapVue)
  // .use(IconsPlugin)
  .mount("#app");
