<template>
  <div id="login_container">
    <div class="login_box">
      <div class="row">
        <span class="col-4">Email</span
        ><input
          class="col-8"
          type="text"
          v-model="login"
          :disabled="showLoader"
        />
      </div>
      <div class="row mt-1">
        <span class="col-4">Password</span
        ><input
          class="col-8"
          type="password"
          v-model="password"
          :disabled="showLoader"
          @keyup.enter="loginSubmit"
        />
      </div>
      <div class="row mt-2 action_area">
        <button v-if="!showLoader" type="button" @click="loginSubmit">
          Login
        </button>
        <span v-else class="loader"></span>
      </div>
      <div class="mt-2" v-if="isErrorAuth">
        <span style="color: red"><b>Login failed</b></span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "LoginView",
  data() {
    return {
      login: null,
      password: null,
      isErrorAuth: false,
      showLoader: false,
    };
  },
  methods: {
    async loginSubmit() {
      try {
        this.showLoader = true;
        this.isErrorAuth = false;

        const respons = await this.axios.post("Auth/Login", {
          email: this.login,
          password: this.password,
        });

        if (respons.status !== 200) {
          this.isErrorAuth = true;
          return;
        }

        this.$store.commit("setAccessToken", respons.data);
        this.showLoader = false;
        this.isErrorAuth = false;
        this.$router.push({ name: "home" });
      } catch (error) {
        console.log(error);
        this.isErrorAuth = true;
        this.showLoader = false;
      }
    },
  },
};
</script>

<style lang="scss">
#login_container {
  width: 38%;
  margin: auto;
  padding-top: 30vh;

  .login_box {
    margin: auto;
    padding-left: 2rem;
    padding-right: 2rem;
    padding-top: 1.2rem;
    padding-bottom: 1.2rem;
    background-color: gray;
    border-radius: 1rem;
    border: black 2px solid;

    .action_area {
      display: flex;
      align-items: center;
      justify-content: center;
    }
  }
}
</style>
