import { createRouter, createWebHistory } from "vue-router";
import axios from "axios";
import LoginView from "../views/LoginView.vue";
import store from "../store";

const authGuard = async (to, from, next) => {
  if (store.getters.isAuth) {
    const respons = await axios.get("Auth/CheckAuth");
    if (respons.status === 204) next();
    else next({ name: "login" });
  } else next({ name: "login" });
};

const notAuthGuard = async (to, from, next) => {
  if (!store.getters.isAuth) next();
  else next({ name: "home" });
};

const routes = [
  {
    path: "/",
    name: "login",
    component: LoginView,
    beforeEnter: notAuthGuard,
  },
  {
    path: "/home",
    name: "home",
    component: () => import("@/views/HomeView.vue"),
    beforeEnter: authGuard,
  },
  {
    path: "/administration",
    name: "admin",
    component: () => import("@/views/AdminView.vue"),
    beforeEnter: authGuard,
  },
  {
    path: "/pickup-point",
    name: "pickuppoint",
    component: () => import("@/views/PickUpPointView.vue"),
    beforeEnter: authGuard,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
