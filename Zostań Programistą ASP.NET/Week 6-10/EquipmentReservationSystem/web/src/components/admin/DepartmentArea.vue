<template>
  <div class="department_area">
    <h2 class="pt-2">Departments</h2>
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
          <div class="col-5"><b>Name</b></div>
          <div class="col-3"><b>User count</b></div>
          <div class="col-3"><b>Item type count</b></div>
        </div>
        <div v-if="departments">
          <div
            class="row row_tab row_tab_click"
            :class="[
              index % 2 === 0 ? 'row_color_1' : 'row_color_2',
              { active: selectedRow === department.id },
            ]"
            v-for="(department, index) in departments"
            :key="department.id"
            @click="selectedRow = department.id"
          >
            <div class="p-0 col-1">
              <input
                type="checkbox"
                v-model="department.active"
                :disabled="true"
              />
            </div>
            <div class="col-5">{{ department.name }}</div>
            <div class="col-3">{{ department.userCount }}</div>
            <div class="col-3">{{ department.itemTypeCount }}</div>
          </div>
        </div>
        <span v-else class="loader"></span>
        <pagging-bar
          :api-path="'Admin/Department/GetAll'"
          :show="20"
          :search="search"
          @changePage="updateTable"
          :key="refresh"
        ></pagging-bar>
      </div>
    </div>
    <department-modal
      v-if="showModal"
      :id="selectedRow"
      :mode="modalMode"
      @close="close"
    ></department-modal>
  </div>
</template>

<script>
import departmentModal from "@/components/admin/DepartmentModal.vue";
import paggingBar from "@/components/PagingBar.vue";

export default {
  components: {
    departmentModal,
    paggingBar,
  },
  data() {
    return {
      departments: null,
      selectedRow: null,
      modalMode: null,
      showModal: false,
      search: null,
      refresh: 0,
    };
  },
  watch: {
    async search() {
      this.selectedRow = null;
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
          `Admin/Department/Delete?id=${this.selectedRow}`
        );
        if (respons.status !== 200) return;
        this.selectedRow = null;
        this.departments = null;
      } catch (error) {
        alert("Error occured");
      }
      this.refresh++;
    },
    updateTable(data) {
      this.departments = data;
    },
  },
};
</script>

<style lang="scss">
.department_area {
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
}
</style>
