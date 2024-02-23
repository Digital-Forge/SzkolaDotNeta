/* eslint no-param-reassign: "error" */

import { createStore } from "vuex";

export default createStore({
  state: {
    authInfo: {
      token: null,
      refresh: null,
      expires: null,
    },
  },
  getters: {
    isAuth: (state) => {
      return state.authInfo.token !== null;
    },
    getAccessToken: (state) => {
      return state.authInfo.token;
    },
    getRefreshToken: (state) => {
      return state.authInfo.refresh;
    },
  },
  mutations: {
    setAccessToken(state, newToken) {
      state.authInfo.token = newToken.token;
      state.authInfo.refresh = newToken.refreshToken;
      state.authInfo.expires = new Date(newToken.expiry);
      localStorage.setItem("token", state.authInfo.token);
      localStorage.setItem("refreshtoken", state.authInfo.refresh);
      localStorage.setItem("expires", state.authInfo.expires);
    },
    recoveryAccessToken(state) {
      if (localStorage.getItem("token") === undefined) return;
      if (localStorage.getItem("expires") <= new Date()) {
        this.commit("removeAccerssToken");
        return;
      }
      state.authInfo.token = localStorage.getItem("token");
      state.authInfo.refresh = localStorage.getItem("refreshtoken");
      state.authInfo.expires = localStorage.getItem("expires");
    },
    removeAccerssToken(state) {
      localStorage.removeItem("token");
      localStorage.removeItem("refreshtoken");
      localStorage.removeItem("expires");
      state.authInfo.token = null;
      state.authInfo.refresh = null;
      state.authInfo.expires = null;
    },
  },
  actions: {},
  modules: {},
});
