<template>
  <div class="department_modal">
    <modal ref="mainModal" @close="close">
      <template v-slot:header
        ><span
          ><b>{{ title }}</b></span
        ></template
      >
      <template v-slot:content>
        <div v-if="isReady" class="content">
          <div class="row mx-0 px-0">
            <div
              class="col-4 page_select"
              @click="setPage('general')"
              :class="{ page_select_active: selectedPage == 'general' }"
            >
              General
            </div>
            <div
              class="col-4 page_select"
              @click="setPage('users')"
              :class="{ page_select_active: selectedPage == 'users' }"
            >
              Users
            </div>
            <div
              class="col-4 page_select"
              @click="setPage('items')"
              :class="{ page_select_active: selectedPage == 'items' }"
            >
              Items
            </div>
          </div>
          <div v-if="selectedPage == 'general'" class="general_info">
            <div class="row mt-2">
              <div class="col-3 name_value"><span>Name</span></div>
              <div class="col-9 box_value">
                <input
                  type="text"
                  v-model="model.name"
                  :disabled="globalDisable"
                />
              </div>
            </div>

            <div class="row mt-2">
              <div class="col-3 name_value"><span>Description</span></div>
              <div class="col-9 box_value">
                <textarea
                  type="text"
                  v-model="model.description"
                  :disabled="globalDisable"
                ></textarea>
              </div>
            </div>

            <div class="row mt-2">
              <div class="col-3 name_value"><span>Active</span></div>
              <div class="col-9 box_value_chackbox">
                <input
                  type="checkbox"
                  v-model="model.active"
                  :disabled="globalDisable"
                />
              </div>
            </div>

            <div class="row mt-2" v-if="mode != 'add'">
              <div class="col-3 name_value"><span>Created</span></div>
              <div class="col-9 box_value">
                <input type="date" v-model="model.create" :disabled="true" />
              </div>
            </div>
          </div>
          <div v-if="selectedPage == 'users'" class="users_info">
            <div v-if="!globalDisable" class="pt-2 modal_tab_add">
              <button type="button" @click="showUserAddModal">Add</button>
            </div>
            <div class="tab">
              <div class="row header_tab">
                <div class="col-5"><b>Fullname</b></div>
                <div class="col-5"><b>Email</b></div>
                <div class="col-2"><b>Remove</b></div>
              </div>
              <div
                class="row row_tab"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(user, index) in model.users"
                :key="user.id"
              >
                <div class="col-5">{{ user.fullname }}</div>
                <div class="col-5">{{ user.email }}</div>
                <div v-if="!globalDisable" class="col-2">
                  <span class="tab_removeBtn" @click="removeUser(user.id)">
                    X
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div v-if="selectedPage == 'items'" class="items_info">
            <div class="tab">
              <div class="row header_tab">
                <div class="col-10"><b>Item name</b></div>
                <div class="col-2"><b>Remove</b></div>
              </div>
              <div
                class="row row_tab"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(item, index) in model.items"
                :key="item.id"
              >
                <div class="col-10">{{ item.name }}</div>
                <div class="col-2">{{ "X" }}</div>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="loader_modal_fix">
          <span class="loader"></span>
        </div>
      </template>
      <template v-slot:footer>
        <div v-if="mode != 'info'" class="save_box">
          <button class="save" type="button" @click="save">Save</button>
        </div>
      </template>
    </modal>
    <modal
      v-if="showAddUserModal"
      :z-index="addModalZIndex"
      :width="30"
      :height="76"
      @close="closeUserAddModal"
    >
      <template v-slot:header><b>Select user</b></template>
      <template v-slot:content>
        <div class="row px-0 mx-0">
          <span class="col-2">Search</span>
          <input class="col-10" type="text" v-model="searchUser" />
        </div>
        <div v-if="!addModalLoader">
          <div class="tab">
            <div class="row header_tab">
              <div class="col-6"><b>Username</b></div>
              <div class="col-6"><b>Email</b></div>
            </div>
            <div
              class="row row_tab"
              :class="[
                index % 2 === 0 ? 'row_color_1' : 'row_color_2',
                { active: selectUser === user.id },
              ]"
              v-for="(user, index) in allowedUsers"
              :key="user.id"
              @click="selectUser = user.id"
            >
              <div class="col-6">{{ user.username }}</div>
              <div class="col-6">{{ user.email }}</div>
            </div>
            <div>
              <span class="click_text" @click="loadUsers">Load more</span>
            </div>
          </div>
        </div>
        <div v-else class="loader_modal_fix">
          <span class="loader"></span>
        </div>
      </template>
      <template v-slot:footer>
        <div class="save_box">
          <button class="save" type="button" @click="addUser">Add</button>
        </div>
      </template>
    </modal>
  </div>
</template>

<script>
import modal from "@/components/ModalWindow.vue";

export default {
  components: {
    modal,
  },
  props: {
    mode: {
      type: String,
      required: true,
    },
    id: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      model: null,
      isReady: false,
      selectedPage: "general",
      globalDisable: false,
      showAddUserModal: false,
      selectUser: null,
      usersList: [],
      addModalLoader: false,
      searchUser: null,
      addUserPaging: {
        show: 30,
        skip: 0,
      },
    };
  },
  methods: {
    setPage(name) {
      this.selectedPage = name;
    },
    close() {
      this.$emit("close");
    },
    async save() {
      try {
        const respons =
          this.model.id == null
            ? await this.axios.put(`Department/Create`, this.model)
            : await this.axios.patch(`Department/Update`, this.model);
        if (respons.status !== 200) return;
        this.$emit("close");
      } catch (error) {
        console.log(error);
        alert("Error occured");
      }
    },
    async closeUserAddModal() {
      this.showAddUserModal = false;
      this.searchUser = null;
      this.selectUser = null;
      this.usersList = [];
      this.addUserPaging.skip = 0;
    },
    async showUserAddModal() {
      await this.loadUsers();
      this.showAddUserModal = true;
    },
    async addUser() {
      if (!this.selectUser) return;

      const user = this.usersList.find((x) => x.id === this.selectUser);

      this.model.users.push({
        id: user.id,
        fullname: user.username,
        email: user.email,
      });
      this.closeUserAddModal();
    },
    async removeUser(index) {
      if (!index) return;

      const response = confirm("Are you sure you want do that?");
      if (!response) return;

      this.model.users = this.model.users.filter((x) => x.id !== index);
    },
    async loadUsers() {
      this.addModalLoader = true;
      try {
        const respons = await this.axios.post(`User/GetAllCombo`, {
          take: this.addUserPaging.show,
          skip: this.addUserPaging.skip,
          search: this.searchUser,
        });
        if (respons.status !== 200) return;
        this.usersList = [...this.usersList, ...respons.data];
        this.addUserPaging.skip = this.usersList.length;
      } catch (error) {
        alert("Error occured");
      }
      this.addModalLoader = false;
    },
    async loadData() {
      if (!this.id) {
        this.$emit("close");
        return;
      }

      try {
        const respons = await this.axios.get(`Department/Get?id=${this.id}`);
        if (respons.status !== 200) return;
        this.model = respons.data;
        this.isReady = true;
      } catch (error) {
        alert("Error occured");
        this.$emit("close");
      }
    },
    getEmptyModel() {
      return {
        id: null,
        name: null,
        description: null,
        create: null,
        users: [],
        items: [],
      };
    },
  },
  watch: {
    async searchUser(oldValue, newValue) {
      if (oldValue === newValue) return;
      this.addUserPaging.skip = 0;
      this.usersList = [];
      await this.loadUsers();
    },
  },
  computed: {
    allowedUsers() {
      return this.usersList.filter(
        (x) => !this.model.users.some((y) => y.id === x.id)
      );
    },
    addModalZIndex() {
      return this.$refs.mainModal.zIndex + 10;
    },
    title() {
      if (!this.mode || !this.model) return "";
      switch (this.mode) {
        case "add":
          return "Add";
        case "edit":
          return `Edit: ${this.model.name}`;
        case "info":
          return `Info about: ${this.model.name}`;
        default:
          return "";
      }
    },
  },
  async mounted() {
    switch (this.mode) {
      case "add":
        this.model = this.getEmptyModel();
        this.isReady = true;
        break;
      case "edit":
        await this.loadData();
        break;
      case "info":
        this.globalDisable = true;
        await this.loadData();
        break;
      default:
        this.$emit("close");
        break;
    }
  },
};
</script>

<style lang="scss">
.department_modal {
  .click_text {
    cursor: pointer;
  }
}
</style>
