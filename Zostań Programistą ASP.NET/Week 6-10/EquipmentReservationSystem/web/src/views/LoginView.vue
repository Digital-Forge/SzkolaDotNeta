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
}

.login_box {
  margin: auto;
  padding-left: 2rem;
  padding-right: 2rem;
  padding-top: 1.2rem;
  padding-bottom: 1.2rem;
  background-color: gray;
  border-radius: 1rem;
  border: black 2px solid;
}

.action_area {
  display: flex;
  align-items: center;
  justify-content: center;
}

.loader {
  width: 48px;
  height: 48px;
  border: 3px dotted #fff;
  border-style: solid solid dotted dotted;
  border-radius: 50%;
  display: inline-block;
  position: relative;
  box-sizing: border-box;
  animation: rotation 2s linear infinite;
}

.loader::after {
  content: "";
  box-sizing: border-box;
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  margin: auto;
  border: 3px dotted #ff3d00;
  border-style: solid solid dotted;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  animation: rotationBack 1s linear infinite;
  transform-origin: center center;
}

@keyframes rotation {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
@keyframes rotationBack {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(-360deg);
  }
}
</style>
