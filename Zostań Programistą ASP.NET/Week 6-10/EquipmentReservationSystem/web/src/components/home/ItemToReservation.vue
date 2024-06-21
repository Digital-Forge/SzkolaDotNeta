<template>
  <div>
    <div class="tab">
      <div class="row header_tab">
        <div class="col-3"><b>Image</b></div>
        <div class="col-6"><b>Name</b></div>
        <div class="col-3"><b>Available quantity</b></div>
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
          <div class="col-3 row_tab_center">
            <image-box class="item_image_table" :id="item.imageId"></image-box>
          </div>
          <div class="col-6 row_tab_center">{{ item.name }}</div>
          <div class="col-3 row_tab_center">{{ item.availableQuantity }}</div>
        </div>
      </div>
      <span v-else class="loader"></span>
      <pagging-bar
        :api-path="'Item/GetAvailableItems'"
        :show="20"
        :search="searchData.searchName"
        :searchExtraData="searchExtras"
        @changePage="updateTable"
        :key="refresh"
      ></pagging-bar>
    </div>
    <reservation-info-modal
      v-if="showModal"
      :id="selectedRow"
      @close="closeModal()"
    ></reservation-info-modal>
  </div>
</template>

<script>
import ReservationInfoModal from "@/components/home/ReservationInfoModal.vue";
import paggingBar from "@/components/PagingBar.vue";
import imageBox from "@/components/ImageBox.vue";

export default {
  components: {
    ReservationInfoModal,
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
      refresh: 0,
      showModal: false,
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
          name: "availableInDepartments",
          value: this.searchData.selectedDepartments,
        },
      ];
    },
  },
};
</script>

<style lang="scss"></style>
