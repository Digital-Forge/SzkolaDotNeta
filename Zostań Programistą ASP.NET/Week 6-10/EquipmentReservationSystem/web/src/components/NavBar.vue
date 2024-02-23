<template>
  <div id="nav-bar">
    <button v-if="viewType != 'home'" type="button" @click="goHome">
      Home
    </button>
    <button
      v-if="viewType != 'pickup' && pickupAccess"
      type="button"
      @click="goPickUpPoint"
    >
      Pick up point
    </button>
    <button
      v-if="viewType != 'admin' && adminAccess"
      type="button"
      @click="goAdmin"
    >
      Administration
    </button>
    <button type="button" @click="logout">Logout</button>
  </div>
</template>

<script>
export default {
  props: {
    viewType: {
      required: true,
      type: String,
      // eslint-disable-next-line
      validator(value, props) {
        return ["home", "admin", "pickup"].includes(value);
      },
    },
  },
  data() {
    return {
      adminAccess: false,
      pickupAccess: false,
    };
  },
  methods: {
    async goHome() {
      this.$router.push({ name: "home" });
    },
    async goAdmin() {
      if (this.adminAccess) this.$router.push({ name: "admin" });
    },
    async goPickUpPoint() {
      if (this.pickupAccess) this.$router.push({ name: "pickuppoint" });
    },
    async logout() {
      this.$store.commit("removeAccerssToken");
      this.$router.push({ name: "login" });
    },
  },
  async mounted() {
    const respons = await this.axios.get("User/PanelAccess");
    if (respons.status !== 200) return;
    this.adminAccess = respons.data.admin;
    this.pickupAccess = respons.data.pickUpPoint;
  },
};
</script>

<style lang="scss">
#nav-bar {
  width: 100%;
  background-color: lightslategrey;
  border-radius: 0.55rem;
  display: flex;
  justify-content: right;

  Button {
    margin-right: 0.3rem;
    border-radius: 0.5rem;
    margin-top: 0.2rem;
    margin-bottom: 0.25rem;
  }
}
</style>
