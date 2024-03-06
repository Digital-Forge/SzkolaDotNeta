<template>
  <div>
    <h2 class="pt-2">Users</h2>
    <div v-if="users">
      <div>
        <button type="button" @click="add">Add</button>
        <button type="button" @click="edit">Edit</button>
        <button type="button" @click="remove">Remove</button>
        <button type="button" @click="info">Info</button>
      </div>
      <div class="tab">
        <div class="row header_tab">
          <div style="width: 7%" class="p-0"><b>Active</b></div>
          <div style="width: 20%" class="p-0"><b>Username</b></div>
          <div style="width: 25%" class="p-0"><b>Email</b></div>
          <div style="width: 15%" class="p-0"><b>Phone</b></div>
          <div style="width: 12%" class="p-0"><b>Departments</b></div>
          <div style="width: 7%" class="p-0"><b>Rented items</b></div>
          <div style="width: 7%" class="p-0"><b>Admin</b></div>
          <div style="width: 7%" class="p-0"><b>Pickup Point</b></div>
        </div>
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
          <div style="width: 7%" class="p-0">
            <input type="checkbox" v-model="user.active" :disabled="true" />
          </div>
          <div style="width: 20%" class="p-0">{{ user.username }}</div>
          <div style="width: 25%" class="p-0">{{ user.email }}</div>
          <div style="width: 15%" class="p-0">{{ user.phone }}</div>
          <div style="width: 12%" class="p-0">{{ user.departmentsCount }}</div>
          <div style="width: 7%" class="p-0">{{ user.rentedItemsCount }}</div>
          <div style="width: 7%" class="p-0">
            <input type="checkbox" v-model="user.isAdmin" :disabled="true" />
          </div>
          <div style="width: 7%" class="p-0">
            <input
              type="checkbox"
              v-model="user.isPickupPoint"
              :disabled="true"
            />
          </div>
        </div>
      </div>
    </div>
    <span v-else class="loader"></span>
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

export default {
  components: {
    userModal,
  },
  data() {
    return {
      users: null,
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
          `User/Delete?id=${this.selectedRow}`
        );
        if (respons.status !== 200) return;
        this.selectedRow = null;
        this.users = null;
        await this.loadData();
      } catch (error) {
        console.log(error);
        alert("Error occured");
      }
    },
    async loadData() {
      try {
        const respons = await this.axios.get("User/GetAll");
        if (respons.status !== 200) return;
        this.users = respons.data;
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

<style lang="scss"></style>
