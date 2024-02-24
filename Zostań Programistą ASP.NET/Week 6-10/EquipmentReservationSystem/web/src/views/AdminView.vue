<template>
  <div id="admin_container">
    <NavBar :view-type="'admin'"></NavBar>
    <div id="admin_box">
      <div class="menu">test</div>
      <div class="content_box">test</div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import NavBar from "@/components/NavBar.vue";

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
  },
  data() {
    return {};
  },
  methods: {},
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
    //display: inline-block;
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
    }
  }
}
</style>
