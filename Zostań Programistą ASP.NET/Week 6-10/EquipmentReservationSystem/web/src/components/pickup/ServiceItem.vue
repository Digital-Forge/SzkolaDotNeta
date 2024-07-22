<template>
  <div class="item_history">
    <div class="tab">
      <div class="row header_tab">
        <div class="col-4"><b>Image</b></div>
        <div class="col-4"><b>Name</b></div>
        <div class="col-4"><b>Serial number</b></div>
      </div>
      <div v-if="items">
        <div
          class="row row_tab row_tab_click"
          :class="[
            index % 2 === 0 ? 'row_color_1' : 'row_color_2',
            { active: selectedRow === item.id },
          ]"
          v-for="(item, index) in items"
          :key="item.id"
          @click="selectedRow = item.id"
          @dblclick="showModal = true"
        >
          <div class="col-4 row_tab_center">
            <image-box class="item_image_table" :id="item.imageId"></image-box>
          </div>
          <div class="col-4 row_tab_center">{{ item.name }}</div>
          <div class="col-4 row_tab_center">
            {{ item.serialNumber }}
          </div>
        </div>
      </div>
      <span v-else class="loader"></span>
      <pagging-bar
        :api-path="'PickupPoint/Service/GetServicesItems'"
        :show="20"
        :search="searchData.searchSerialNumber"
        :searchExtraData="searchExtras"
        :key="refresh"
        @changePage="updateTable"
      ></pagging-bar>
    </div>
    <service-modal
      v-if="showModal"
      :id="selectedRow"
      @close="closeModal()"
    ></service-modal>
  </div>
</template>

<script>
import serviceModal from "@/components/pickup/ServiceModal.vue";
import paggingBar from "@/components/PagingBar.vue";
import imageBox from "@/components/ImageBox.vue";

export default {
  components: {
    serviceModal,
    paggingBar,
    imageBox,
  },
  props: {
    searchData: {
      type: Object,
      default: null,
    },
  },
  data() {
    return {
      items: null,
      selectedRow: null,
      showModal: false,
      refresh: 0,
    };
  },
  methods: {
    updateTable(data) {
      this.items = data;
    },
    closeModal() {
      this.showModal = false;
      this.refresh++;
    },
  },
  computed: {
    searchExtras() {
      return [
        {
          name: "searchItems",
          value: this.searchData.searchItem,
        },
      ];
    },
  },
  // async mounted() {},
};
</script>

<style lang="scss"></style>
