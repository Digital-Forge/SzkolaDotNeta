<template>
  <div class="about">
    <NavBar :view-type="'home'"></NavBar>
    <h1>Home</h1>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";

export default {
  name: "HomeView",
  components: {
    NavBar,
  },
  data() {
    return {};
  },
  methods: {},
};
</script>
