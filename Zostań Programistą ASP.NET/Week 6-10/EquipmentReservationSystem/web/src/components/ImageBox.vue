<template>
  <div
    v-if="load"
    :class="[verticalMode ? 'imagebox_vertical' : 'imagebox_horizontal']"
  >
    <img :src="src" alt="" />
  </div>
  <div v-else class="imagebox_loader">
    <span class="loader"></span>
  </div>
</template>

<script>
export default {
  props: {
    id: {
      default: "",
      required: true,
      type: String,
    },
    verticalMode: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      base64: null,
      format: null,
      load: false,
    };
  },
  computed: {
    src() {
      return `data:image/${this.format};base64,${this.base64}`;
    },
  },
  methods: {},
  async mounted() {
    if (this.id === "") return;
    try {
      const respons = await this.axios.get(`File?id=${this.id}`);
      if (respons.data.dataBase64) this.base64 = respons.data.dataBase64;
      if (respons.data.info.name) {
        const name = respons.data.info.name.split(".");
        this.format = name[name.length - 1];
      }
    } catch (error) {
      this.base64 = null;
    }
    this.load = true;
  },
};
</script>

<style lang="scss">
.imagebox_horizontal {
  max-width: 100%;

  img {
    max-width: 100%;
  }
}

.imagebox_vertical {
  max-height: 100%;
  width: auto;

  img {
    max-height: 100%;
    width: auto;
  }
}

.imagebox_loader {
  width: 100%;
  margin-top: 0.4rem;
}
</style>
