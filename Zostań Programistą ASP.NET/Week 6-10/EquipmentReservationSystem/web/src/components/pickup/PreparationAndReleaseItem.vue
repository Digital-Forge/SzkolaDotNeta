<template>
  <div class="item_history">
    <div class="tab">
      <div class="row header_tab">
        <div class="col-2"><b>Image</b></div>
        <div class="col-4"><b>Name</b></div>
        <div class="col-2"><b>Period</b></div>
        <div class="col-2"><b>User</b></div>
        <div class="col-2"><b>Status</b></div>
      </div>
      <div v-if="items">
        <div
          class="row row_tab row_tab_click"
          :class="[
            index % 2 === 0 ? 'row_color_1' : 'row_color_2',
            { active: selectedRow?.id === item.id },
            { item_near_expired: itemNearExpired(item.from) },
            { item_expired: itemExpired(item.from) },
          ]"
          v-for="(item, index) in items"
          :key="item.id"
          @click="selectedRow = item"
          @dblclick="showModal = true"
        >
          <div class="col-2 row_tab_center">
            <image-box class="item_image_table" :id="item.image"></image-box>
          </div>
          <div class="col-4 row_tab_center">{{ item.itemName }}</div>
          <div class="col-2 row_tab_center">
            <div>
              <div class="period_tab_show">{{ item.from }}</div>
              <div class="period_tab_show" v-if="item.to">{{ item.to }}</div>
            </div>
          </div>
          <div class="col-2 row_tab_center">{{ item.username }}</div>
          <div class="col-2 row_tab_center">
            {{ camelCaseToNormal(item.status) }}
          </div>
        </div>
      </div>
      <span v-else class="loader"></span>
      <pagging-bar
        :api-path="'PickupPoint/Reservation/GetPreparationAndReleaseReservations'"
        :show="20"
        :search="searchData.searchName"
        :searchExtraData="searchExtras"
        :key="refresh"
        @changePage="updateTable"
      ></pagging-bar>
    </div>
    <ReservationModal
      v-if="showModal"
      :id="selectedRow.id"
      :mode="selectedRow.status === 'InPreparation' ? 'preparation' : 'release'"
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
    itemExpired(date) {
      if (!date) return false;
      return new Date(date) < new Date(Date.now()).setHours(0, 0, 0, 0);
    },
    itemNearExpired(date) {
      if (!date) return false;
      return (
        new Date(date).setHours(0, 0, 0, 0) ===
        new Date(Date.now()).setHours(0, 0, 0, 0)
      );
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
        {
          name: "showType",
          value: this.searchData.showReservationType,
        },
      ];
    },
  },
  // async mounted() {},
};
</script>

<style lang="scss">
.item_history {
  .period_tab_show {
    display: block;
  }

  .item_expired {
    background-color: red;
  }

  .item_near_expired {
    background-color: orangered;
  }
}
</style>
