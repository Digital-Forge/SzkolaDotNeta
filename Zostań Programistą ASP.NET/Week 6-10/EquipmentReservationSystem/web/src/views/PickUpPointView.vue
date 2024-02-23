<template>
  <div class="about">
    <h1>Pick up point</h1>
  </div>
</template>

<script>
import axios from "axios";

const authAdministrationArea = async () => {
  try {
    const respons = await axios.get("Auth/CheckPickUpPointAuth");
    return respons.status === 200;
  } catch (error) {
    return false;
  }
};

export default {
  name: "PickUpPointView",
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
