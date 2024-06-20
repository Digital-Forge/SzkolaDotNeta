<template>
  <div class="combobox_api">
    <multiselect
      v-model="selectedOption"
      :options="options"
      :multiple="multiple"
      :disabled="disabled"
      :loading="isLoading"
      :placeholder="placeholder"
      label="name"
      track-by="code"
      :hideSelected="true"
      @search-change="fetchOptions"
    />
  </div>
</template>

<script>
import Multiselect from "vue-multiselect";
import "vue-multiselect/dist/vue-multiselect.css";

export default {
  name: "ComboBoxApi",
  components: { Multiselect },
  props: {
    value: {
      required: true,
    },
    api: {
      type: String,
      required: true,
    },
    take: {
      type: Number,
      default: 20,
    },
    placeholder: {
      type: String,
      default: "",
    },
    multiple: {
      type: Boolean,
      default: false,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["update:value"],
  data() {
    return {
      selectedOption: null,
      options: [],
      isLoading: false,
      outsideChange: false,
      insideChange: false,
    };
  },
  watch: {
    value(newValue, oldValue) {
      if (newValue === oldValue || this.insideChange) {
        this.insideChange = false;
        return;
      }
      this.outsideChange = true;

      if (newValue == null || newValue.length === 0) {
        this.selectedOption = null;
        return;
      }
      this.fetchOptions("", Array.isArray(newValue) ? newValue : [newValue]);
    },
    selectedOption(newValue, oldValue) {
      if (newValue === oldValue || this.outsideChange) {
        this.outsideChange = false;
        return;
      }
      this.insideChange = true;
      this.$emit(
        "update:value",
        newValue.map((x) => x.code)
      );
    },
  },
  methods: {
    async fetchOptions(searchText = "", serchingPosition = null) {
      this.isLoading = true;

      const data = !serchingPosition
        ? {
            search: searchText,
            take: this.take,
          }
        : {
            search: searchText,
            take: this.take,
            serchingPosition,
          };

      try {
        const respons = await this.axios.post(this.api, data);
        this.options = respons.data.data;
        if (respons.data.serchingPosition.length !== 0)
          this.selectedOption = respons.data.serchingPosition;
      } catch (error) {
        console.error("api not respons on");
      } finally {
        this.isLoading = false;
      }
    },
  },
  async mounted() {
    await this.fetchOptions();
  },
};
</script>

<style lang="scss"></style>
