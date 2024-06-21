<template>
  <div class="file_upload" @click="initUplada()">
    <input
      ref="uploader"
      class="hidden_upload"
      type="file"
      :accept="acceptedFormat"
      :multiple="multiple"
      :value="uploadValue"
    />
    <span v-show="uploadProcess" class="loader mt-2"></span>
    <span v-show="!uploadProcess">UPLAOD</span>
  </div>
</template>

<script>
export default {
  props: {
    acceptedFormat: {
      type: String,
      default: null,
      required: false,
    },
    multiple: {
      type: Boolean,
      default: false,
      required: false,
    },
  },
  data() {
    return {
      uploadProcess: false,
      uploadValue: null,
    };
  },
  methods: {
    async initUplada() {
      this.uploadProcess = true;
      this.$refs.uploader.click();
    },
    async readFileAsBase64(file) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result);
        reader.onerror = reject;
        reader.readAsDataURL(file);
      });
    },
    async processData(file) {
      const base64 = await this.readFileAsBase64(file);

      return {
        dataBase64: base64.substring(base64.indexOf(",") + 1),
        info: {
          active: true,
          name: file.name,
        },
      };
    },
    async upload() {
      if (this.$refs.uploader.files.length === 0) {
        this.uploadProcess = false;
        return;
      }

      const processData = [...this.$refs.uploader.files].map((file) => {
        return this.processData(file);
      });

      const base64Data = await Promise.all(processData);
      const sendList = base64Data.map((file) => {
        return this.sendFile(file);
      });

      const respons = await Promise.all(sendList);
      const responsIdList = respons.filter((x) => !!x);

      this.$emit("resId", responsIdList);
      this.uploadProcess = false;
      this.uploadValue = null;
      if (respons.length !== responsIdList.length) alert("Operation inwalid");
    },
    async sendFile(data) {
      try {
        const respons = await this.axios.post("File", data);
        if (respons.status === 200) return respons.data;
        return null;
      } catch (error) {
        return null;
      }
    },
  },
  mounted() {
    this.$refs.uploader.addEventListener("change", this.upload);
    this.$refs.uploader.addEventListener("cancel", () => {
      this.uploadValue = null;
      this.uploadProcess = false;
    });
  },
};
</script>

<style lang="scss">
.file_upload {
  cursor: pointer;
  width: 100%;

  .hidden_upload {
    display: none;
  }
}
</style>
