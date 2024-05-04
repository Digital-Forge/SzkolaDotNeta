<template>
  <div class="user_area">
    <h2 class="pt-2">Users</h2>
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
          <div style="width: 8%" class="simple_header p-0"><b>Active</b></div>
          <div style="width: 18%" class="simple_header p-0">
            <b>Username</b>
          </div>
          <div style="width: 25%" class="simple_header p-0"><b>Email</b></div>
          <div style="width: 12%" class="simple_header p-0"><b>Phone</b></div>
          <div style="width: 12%" class="simple_header p-0">
            <b>Departments</b>
          </div>
          <div style="width: 9%" class="simple_header p-0">
            <b>Rented items</b>
          </div>
          <div style="width: 8%" class="simple_header p-0"><b>Admin</b></div>
          <div style="width: 8%" class="simple_header p-0">
            <b>Pickup Point</b>
          </div>
        </div>
        <div v-if="users">
          <div
            class="row row_tab"
            :class="[
              index % 2 === 0 ? 'row_color_1' : 'row_color_2',
              { active: selectedRow === user.id },
            ]"
            v-for="(user, index) in users"
            :key="user.id"
            @click="selectedRow = user.id"
          >
            <div style="width: 8%" class="p-0">
              <input type="checkbox" v-model="user.active" :disabled="true" />
            </div>
            <div style="width: 18%" class="p-0">{{ user.username }}</div>
            <div style="width: 25%" class="p-0">{{ user.email }}</div>
            <div style="width: 12%" class="p-0">{{ user.phone }}</div>
            <div style="width: 12%" class="p-0">
              {{ user.departmentsCount }}
            </div>
            <div style="width: 9%" class="p-0">{{ user.rentedItemsCount }}</div>
            <div style="width: 8%" class="p-0">
              <input type="checkbox" v-model="user.isAdmin" :disabled="true" />
            </div>
            <div style="width: 8%" class="p-0">
              <input
                type="checkbox"
                v-model="user.isPickupPoint"
                :disabled="true"
              />
            </div>
          </div>
        </div>
        <span v-else class="loader"></span>
        <pagging-bar
          :api-path="'Admin/User/GetAll'"
          :show="20"
          :search="search"
          @changePage="updateTable"
          :key="refresh"
        ></pagging-bar>
      </div>
    </div>
    <user-modal
      v-if="showModal"
      :id="selectedRow"
      :mode="modalMode"
      @close="close"
    ></user-modal>
  </div>
</template>

<script>
import userModal from "@/components/admin/UserModal.vue";
import paggingBar from "@/components/PagingBar.vue";

export default {
  components: {
    userModal,
    paggingBar,
  },
  data() {
    return {
      users: null,
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
          `Admin/User/Delete?id=${this.selectedRow}`
        );
        if (respons.status !== 200) return;
        this.selectedRow = null;
        this.users = null;
      } catch (error) {
        alert("Error occured");
      }
      this.refresh++;
    },
    updateTable(data) {
      this.users = data;
    },
  },
};
</script>

<style lang="scss">
.user_area {
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
