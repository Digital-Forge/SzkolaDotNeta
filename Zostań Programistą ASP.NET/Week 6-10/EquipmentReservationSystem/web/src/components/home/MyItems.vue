<template>
  <div class="my_item">
    <div class="tab">
      <div class="row header_tab">
        <div class="col-2"><b>Image</b></div>
        <div class="col-4"><b>Name</b></div>
        <div class="col-2"><b>From</b></div>
        <div class="col-2"><b>To</b></div>
        <div class="col-2"><b>Status</b></div>
      </div>
      <div v-if="items">
        <div
          class="row row_tab row_tab_click"
          :class="[
            index % 2 === 0 ? 'row_color_1' : 'row_color_2',
            { active: selectedRow === item.id },
            { item_expired: checkItemExpired(item.to) },
          ]"
          v-for="(item, index) in items"
          :key="item.id"
          @click="selectedRow = item.id"
          @dblclick="showModal = true"
        >
          <div class="col-2 row_tab_center">
            <image-box class="item_image_table" :id="item.imageId"></image-box>
          </div>
          <div class="col-4 row_tab_center">{{ item.name }}</div>
          <div class="col-2 row_tab_center">{{ item.from }}</div>
          <div class="col-2 row_tab_center">{{ item.to }}</div>
          <div class="col-2 row_tab_center">
            {{ camelCaseToNormal(item.status) }}
          </div>
        </div>
      </div>
      <span v-else class="loader"></span>
      <pagging-bar
        :api-path="'Reservation/MyReservation'"
        :show="20"
        :search="searchData.searchName"
        :searchExtraData="searchExtras"
        @changePage="updateTable"
      ></pagging-bar>
    </div>
    <item-info-modal
      v-if="showModal"
      :id="selectedRow"
      @close="showModal = false"
    ></item-info-modal>
  </div>
</template>

<script>
import itemInfoModal from "@/components/home/ItemInfoModal.vue";
import paggingBar from "@/components/PagingBar.vue";
import imageBox from "@/components/ImageBox.vue";

export default {
  components: {
    paggingBar,
    imageBox,
    itemInfoModal,
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
    };
  },
  methods: {
    updateTable(data) {
      this.items = data;
    },
    camelCaseToNormal(text) {
      const normalText = text
        .replace(/([a-z])([A-Z])/g, "$1 $2")
        .replace(/([A-Z])([A-Z][a-z])/g, "$1 $2")
        .toLowerCase();

      return normalText.charAt(0).toUpperCase() + normalText.slice(1);
    },
    checkItemExpired(to) {
      if (!to) return false;
      return (
        new Date(to).setHours(0, 0, 0, 0) <=
        new Date(Date.now()).setHours(0, 0, 0, 0)
      );
    },
  },
  computed: {
    searchExtras() {
      return [
        {
          name: "availableInDepartments",
          value: this.searchData.selectedDepartments,
        },
      ];
    },
  },
};
</script>

<style lang="scss">
.my_item {
  .item_expired {
    background-color: red;
  }
}
</style>
