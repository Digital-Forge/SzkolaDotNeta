<template>
  <div>
    <h2 class="pt-2">Departments</h2>
    <div v-if="departments.length > 0">
      <div>
        <button type="button" @click="add">Add</button>
        <button type="button" @click="edit">Edit</button>
        <button type="button" @click="remove">Remove</button>
        <button type="button" @click="info">Info</button>
      </div>
      <div class="tab">
        <div class="row header_table">
          <div class="col-6"><b>Name</b></div>
          <div class="col-3"><b>User count</b></div>
          <div class="col-3"><b>Item type count</b></div>
        </div>
        <div
          class="row row_table"
          :class="[
            index % 2 === 0 ? 'row_color_1' : 'row_color_2',
            { active: selectedRow === department.id },
          ]"
          v-for="(department, index) in departments"
          :key="department.id"
          @click="selectedRow = department.id"
        >
          <div class="col-6">{{ department.name }}</div>
          <div class="col-3">{{ department.userCount }}</div>
          <div class="col-3">{{ department.itemTypeCount }}</div>
        </div>
      </div>
    </div>
    <span v-else class="loader"></span>
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

export default {
  components: {
    departmentModal,
  },
  data() {
    return {
      departments: [],
      selectedRow: null,
      modalMode: null,
      showModal: false,
    };
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
      await this.loadData();
    },
    async remove() {
      if (!this.selectedRow) return;

      const response = confirm("Are you sure you want do that?");
      if (!response) return;

      try {
        const respons = await this.axios.delete(
          `Department/Delete?id=${this.selectedRow}`
        );
        if (respons.status !== 200) return;
        this.selectedRow = null;
        this.departments = [];
        await this.loadData();
      } catch (error) {
        console.log(error);
        alert("Error occured");
      }
    },
    async loadData() {
      try {
        const respons = await this.axios.get("Department/GetAll");
        if (respons.status !== 200) return;
        this.departments = respons.data;
      } catch (error) {
        console.log(error);
        alert("Error occured");
      }
    },
  },
  async mounted() {
    await this.loadData();
  },
};
</script>

<style lang="scss">
.tab {
  border: 2px black solid;
  margin-top: 0.5rem;
  margin-left: 1rem;
  margin-right: 1rem;

  .header_table {
    background-color: pink;
    margin: 0;
    padding: 0;
  }

  .row_table {
    border-top: 1px black solid;
    margin: 0;
    padding-right: 0;
    padding-left: 0;
  }

  .row_color_1 {
    background-color: rgb(226, 221, 141);
  }

  .row_color_2 {
    background-color: rgb(171, 221, 151);
  }

  .active {
    border: 2px rgba(255, 0, 0, 0.404) dashed;
  }
}
</style>
