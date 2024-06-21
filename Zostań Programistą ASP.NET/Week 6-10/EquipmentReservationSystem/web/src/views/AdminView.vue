<template>
  <div id="admin_container">
    <NavBar :view-type="'admin'"></NavBar>
    <div id="admin_box" :key="view.refreskKey">
      <div class="menu">
        <h2>Menu</h2>
        <div class="position" :class="{ active: view.isUsers }">
          <div @click="showUsers">Users</div>
        </div>
        <div class="position" :class="{ active: view.isDepartments }">
          <div @click="showDepartments">Departments</div>
        </div>
        <div class="position" :class="{ active: view.isItems }">
          <div @click="showItems">Items</div>
        </div>
      </div>
      <div class="content_box">
        <UserArea v-if="view.isUsers"></UserArea>
        <ItemArea v-if="view.isItems"></ItemArea>
        <DepartmentArea v-if="view.isDepartments"></DepartmentArea>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import NavBar from "@/components/NavBar.vue";
import UserArea from "@/components/admin/UserArea.vue";
import ItemArea from "@/components/admin/ItemArea.vue";
import DepartmentArea from "@/components/admin/DepartmentArea.vue";

const authAdministrationArea = async () => {
  try {
    const respons = await axios.get("Auth/CheckAdminAuth");
    return respons.status === 200;
  } catch (error) {
    return false;
  }
};

export default {
  name: "AdminView",
  components: {
    NavBar,
    UserArea,
    ItemArea,
    DepartmentArea,
  },
  data() {
    return {
      view: {
        isUsers: false,
        isDepartments: false,
        isItems: false,
        isReservations: false,
        refreskKey: 0,
      },
    };
  },
  methods: {
    async showUsers() {
      this.view.isUsers = true;
      this.view.isDepartments = false;
      this.view.isItems = false;
      this.view.isReservations = false;
      this.view.refreskKey++;
    },
    async showDepartments() {
      this.view.isUsers = false;
      this.view.isDepartments = true;
      this.view.isItems = false;
      this.view.isReservations = false;
      this.view.refreskKey++;
    },
    async showItems() {
      this.view.isUsers = false;
      this.view.isDepartments = false;
      this.view.isItems = true;
      this.view.isReservations = false;
      this.view.refreskKey++;
    },
    async showReservations() {
      this.view.isUsers = false;
      this.view.isDepartments = false;
      this.view.isItems = false;
      this.view.isReservations = true;
      this.view.refreskKey++;
    },
  },
  async beforeRouteEnter(to, from, next) {
    const result = await authAdministrationArea();
    if (result) next();
    else next({ name: "home" });
  },
};
</script>

<style lang="scss">
#admin_container {
  display: block;
  height: 100vh;
  width: 100%;
  background-color: white;
  border-left: 2px lightgrey solid;
  border-right: 2px lightgrey solid;
  border-top-left-radius: 0.5rem;
  border-top-right-radius: 0.5rem;
  padding: 0;
  margin: 0;

  #admin_box {
    height: 95.5%;
    width: 100%;

    .content_box {
      display: inline-block;
      width: 75%;
      height: 100%;
      overflow: auto;
      vertical-align: top;

      background-color: white;
    }

    .menu {
      display: inline-block;
      width: 25%;
      height: 100%;
      overflow: auto;
      vertical-align: top;

      background-color: lightgray;
      border: 4px magenta groove;

      .position {
        margin-top: 0.4rem;
      }

      .active {
        box-shadow: 0 0 10px rgba(0, 0, 0, 1.5);
      }

      .position:hover {
        cursor: pointer;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
      }
    }
  }
}
</style>
