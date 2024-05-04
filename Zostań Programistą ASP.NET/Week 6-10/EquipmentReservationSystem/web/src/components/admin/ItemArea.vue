<template>
  <div class="item_area">
    <h2 class="pt-2">Items</h2>
    <div>
      <div class="action_bar">
        <span class="action">Search</span>
        <input class="action" type="text" v-model="search" />
        <button
          class="action"
          type="button"
          :disabled="!selectedRow"
          @click="info"
        >
          Info
        </button>
        <button class="action" type="button" @click="add">Add</button>
        <button
          class="action"
          type="button"
          :disabled="!selectedRow"
          @click="edit"
        >
          Edit
        </button>
        <button
          class="action"
          type="button"
          :disabled="!selectedRow"
          @click="remove"
        >
          Remove
        </button>
      </div>
      <div class="tab">
        <div class="row header_tab">
          <div class="col-1"><b>Active</b></div>
          <div class="col-3"><b>Image</b></div>
          <div class="col-5"><b>Name</b></div>
          <div class="col-3"><b>Count</b></div>
        </div>
        <div v-if="items">
          <div
            class="row row_tab"
            :class="[
              index % 2 === 0 ? 'row_color_1' : 'row_color_2',
              { active: selectedRow === item.id },
            ]"
            v-for="(item, index) in items"
            :key="item.id"
            @click="selectedRow = item.id"
          >
            <div class="p-0 col-1 row_tab_center">
              <input type="checkbox" v-model="item.active" :disabled="true" />
            </div>
            <div class="col-3 row_tab_center">
              <image-box
                class="item_image_table"
                :id="item.imageId"
              ></image-box>
            </div>
            <div class="col-5 row_tab_center">{{ item.name }}</div>
            <div class="col-3 row_tab_center">{{ item.count }}</div>
          </div>
        </div>
        <span v-else class="loader"></span>
        <pagging-bar
          :api-path="'Admin/Item/GetAll'"
          :show="20"
          :search="search"
          :extra-data="searchExtras"
          @changePage="updateTable"
          :key="refresh"
        ></pagging-bar>
      </div>
    </div>
    <item-modal
      v-if="showModal"
      :id="selectedRow"
      :mode="modalMode"
      @close="close"
    ></item-modal>
  </div>
</template>

<script>
import itemModal from "@/components/admin/ItemModal.vue";
import paggingBar from "@/components/PagingBar.vue";
import imageBox from "@/components/ImageBox.vue";

export default {
  components: {
    itemModal,
    paggingBar,
    imageBox,
  },
  data() {
    return {
      items: null,
      selectedRow: null,
      modalMode: null,
      showModal: false,
      search: null,
      refresh: 0,
      searchDepartments: [],
    };
  },
  watch: {
    async search() {
      this.selectedRow = null;
    },
  },
  computed: {
    searchExtras() {
      return [
        {
          name: "availableInDepartments",
          value: this.searchDepartments,
        },
      ];
    },
  },
  methods: {
    async add() {
      this.modalMode = "add";
      this.showModal = true;
    },
    async edit() {
      if (!this.selectedRow) return;
      this.modalMode = "edit";
      this.showModal = true;
    },
    async info() {
      if (!this.selectedRow) return;
      this.modalMode = "info";
      this.showModal = true;
    },
    async close() {
      this.showModal = false;
      this.modalMode = null;
      this.refresh++;
    },
    async remove() {
      if (!this.selectedRow) return;

      const response = confirm("Are you sure you want do that?");
      if (!response) return;

      try {
        const respons = await this.axios.delete(
          `Admin/Item/Delete?id=${this.selectedRow}`
        );
        if (respons.status !== 200) return;
        this.selectedRow = null;
        this.items = null;
      } catch (error) {
        alert("Error occured");
      }
      this.refresh++;
    },
    updateTable(data) {
      this.items = data;
    },
  },
};
</script>

<style lang="scss">
.item_area {
  .action_bar {
    display: flex;
    margin-right: 1rem;
    margin-left: 1rem;

    .action {
      flex: 1;
      margin-left: 0.25rem;
      margin-right: 0.25rem;
    }
  }

  .item_image_table {
    border-radius: 1rem;
    overflow: hidden;
    padding: 0;
    margin: 0;
  }
}
</style>
