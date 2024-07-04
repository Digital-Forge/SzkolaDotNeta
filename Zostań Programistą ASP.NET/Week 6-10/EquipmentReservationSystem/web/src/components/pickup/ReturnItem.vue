<template>
  <div class="item_history">
    <div class="tab">
      <div class="row header_tab">
        <div class="col-2"><b>Image</b></div>
        <div class="col-4"><b>Name</b></div>
        <div class="col-3"><b>Period</b></div>
        <div class="col-3"><b>User</b></div>
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
          <div class="col-2 row_tab_center">
            <image-box class="item_image_table" :id="item.image"></image-box>
          </div>
          <div class="col-4 row_tab_center">{{ item.itemName }}</div>
          <div class="col-3 row_tab_center">
            <div>
              <div class="period_tab_show">{{ item.from }}</div>
              <div class="period_tab_show" v-if="item.to">{{ item.to }}</div>
            </div>
          </div>
          <div class="col-3 row_tab_center">{{ item.username }}</div>
        </div>
      </div>
      <span v-else class="loader"></span>
      <pagging-bar
        :api-path="'PickupPoint/Reservation/GetItemsToReturn'"
        :show="20"
        :search="searchData.searchName"
        :searchExtraData="searchExtras"
        :key="refresh"
        @changePage="updateTable"
      ></pagging-bar>
    </div>
    <ReservationModal
      v-if="showModal"
      :id="selectedRow"
      :mode="'return'"
      @close="closeModal"
    ></ReservationModal>
  </div>
</template>

<script>
import ReservationModal from "@/components/pickup/ReservationModal.vue";
import paggingBar from "@/components/PagingBar.vue";
import imageBox from "@/components/ImageBox.vue";

export default {
  components: {
    ReservationModal,
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
    camelCaseToNormal(text) {
      const normalText = text
        .replace(/([a-z])([A-Z])/g, "$1 $2")
        .replace(/([A-Z])([A-Z][a-z])/g, "$1 $2")
        .toLowerCase();

      return normalText.charAt(0).toUpperCase() + normalText.slice(1);
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
        {
          name: "SearchUsers",
          value: this.searchData.searchUser,
        },
      ];
    },
  },
  // async mounted() {},
};
</script>

<style lang="scss"></style>
