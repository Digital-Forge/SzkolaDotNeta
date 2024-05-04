<template>
  <div class="user_modal">
    <modal ref="mainModal" :z-index="zIndexFix" @close="close">
      <template v-slot:header
        ><span
          ><b>{{ title }}</b></span
        ></template
      >
      <template v-slot:content>
        <div v-if="isReady" class="content">
          <div class="row mx-0 px-0">
            <div
              class="page_select"
              :class="[
                mode == 'add' ? 'col-6' : 'col-4',
                { page_select_active: selectedPage == 'general' },
              ]"
              @click="setPage('general')"
            >
              General
            </div>
            <div
              class="col-4 page_select"
              :class="[
                mode == 'add' ? 'col-6' : 'col-4',
                { page_select_active: selectedPage == 'departments' },
              ]"
              @click="setPage('departments')"
            >
              Departments
            </div>
            <div
              v-if="mode != 'add'"
              class="col-4 page_select"
              :class="{ page_select_active: selectedPage == 'items' }"
              @click="setPage('items')"
            >
              Items history
            </div>
          </div>
          <div v-if="selectedPage == 'general'" class="general_info">
            <div class="row mt-2">
              <div class="col-3 name_value"><span>Username</span></div>
              <div class="col-9 box_value">
                <input
                  type="text"
                  v-model="model.username"
                  :disabled="globalDisable"
                />
              </div>
            </div>

            <div class="row mt-2">
              <div class="col-3 name_value"><span>Email</span></div>
              <div class="col-9 box_value">
                <input
                  type="text"
                  v-model="model.email"
                  :disabled="globalDisable"
                />
              </div>
            </div>

            <div class="row mt-2">
              <div class="col-3 name_value"><span>Password</span></div>
              <div class="col-9 box_value">
                <input
                  type="password"
                  v-model="model.password"
                  :disabled="globalDisable"
                />
              </div>
            </div>

            <div class="row mt-2">
              <div class="col-3 name_value"><span>Phone number</span></div>
              <div class="col-9 box_value">
                <input
                  type="text"
                  v-model="model.phone"
                  :disabled="globalDisable"
                />
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

            <div class="row mt-2">
              <div class="col-3 name_value_list"><span>Roles</span></div>
              <div class="col-9 box_value_chackbox">
                <div>
                  <input
                    type="checkbox"
                    v-model="model.isAdmin"
                    :disabled="globalDisable"
                  /><span>Admin</span>
                </div>
                <div>
                  <input
                    type="checkbox"
                    v-model="model.isPickupPoint"
                    :disabled="globalDisable"
                  /><span>Pickup Point</span>
                </div>
              </div>
            </div>

            <div class="row mt-2" v-if="mode != 'add'">
              <div class="col-3 name_value"><span>Created</span></div>
              <div class="col-9 box_value">
                <input type="date" v-model="model.created" :disabled="true" />
              </div>
            </div>
          </div>
          <div v-if="selectedPage == 'departments'" class="departments_info">
            <div v-if="!globalDisable" class="pt-2 modal_tab_add">
              <button type="button" @click="showDepartmentAddModal">Add</button>
            </div>
            <div class="tab">
              <div class="row header_tab">
                <div :class="[globalDisable ? 'col-12' : 'col-10']">
                  <b>Name</b>
                </div>
                <div v-if="!globalDisable" class="col-2"><b>Remove</b></div>
              </div>
              <div
                class="row row_tab"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(department, index) in model.departments"
                :key="department.id"
              >
                <div :class="[globalDisable ? 'col-12' : 'col-10']">
                  {{ department.name }}
                </div>
                <div v-if="!globalDisable" class="col-2">
                  <span
                    class="tab_removeBtn"
                    @click="removeDepartment(department.id)"
                  >
                    X
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div v-if="selectedPage == 'items'" class="items_info">
            <div class="tab">
              <div class="row header_tab">
                <div class="col-5"><b>Name</b></div>
                <div class="col-2"><b>From</b></div>
                <div class="col-2"><b>To</b></div>
                <div class="col-3"><b>Status</b></div>
              </div>
              <div
                class="row row_tab"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(item, index) in model.itemHistory"
                :key="item.id"
              >
                <div class="col-5">{{ item.name }}</div>
                <div class="col-2">{{ item.from }}</div>
                <div class="col-2">{{ item.to }}</div>
                <div class="col-3">{{ item.status }}</div>
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
      v-if="showAddDepartmentModal"
      :z-index="addModalZIndex"
      :width="30"
      :height="76"
      @close="closeDepartmentAddModal"
    >
      <template v-slot:header><b>Select department</b></template>
      <template v-slot:content>
        <div class="tab">
          <div class="row header_tab">
            <div class="col-12"><b>Name</b></div>
          </div>
          <div
            class="row row_tab"
            :class="[
              index % 2 === 0 ? 'row_color_1' : 'row_color_2',
              { active: selectDepartment === department.id },
            ]"
            v-for="(department, index) in allowedDepartment"
            :key="department.id"
            @click="selectDepartment = department.id"
          >
            <div class="col-12">{{ department.name }}</div>
          </div>
        </div>
      </template>
      <template v-slot:footer>
        <div class="save_box">
          <button class="save" type="button" @click="addDepartment">Add</button>
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
    zIndexFix: {
      type: Number,
      required: false,
    },
  },
  data() {
    return {
      model: null,
      departmentsList: null,
      isReady: false,
      selectedPage: "general",
      globalDisable: false,
      showAddDepartmentModal: false,
      selectDepartment: null,
    };
  },
  methods: {
    async setPage(name) {
      this.selectedPage = name;
    },
    async close() {
      this.$emit("close");
    },
    async closeDepartmentAddModal() {
      this.selectDepartment = null;
      this.showAddDepartmentModal = false;
    },
    async showDepartmentAddModal() {
      this.showAddDepartmentModal = true;
    },
    async addDepartment() {
      if (!this.selectDepartment) return;

      const department = this.departmentsList.find(
        (x) => x.id === this.selectDepartment
      );

      this.model.departments.push({
        id: department.id,
        name: department.name,
      });
      this.closeDepartmentAddModal();
    },
    async removeDepartment(index) {
      if (!index) return;

      const response = confirm("Are you sure you want do that?");
      if (!response) return;

      this.model.departments = this.model.departments.filter(
        (x) => x.id !== index
      );
    },
    async save() {
      try {
        const respons =
          this.model.id == null
            ? await this.axios.put(`Admin/User/Create`, this.model)
            : await this.axios.patch(`Admin/User/Update`, this.model);
        if (respons.status !== 200) return;
        this.$emit("close");
      } catch (error) {
        console.log(error);
        alert("Error occured");
      }
    },
    async loadData() {
      if (!this.id) {
        this.$emit("close");
        return;
      }

      try {
        const respons = await this.axios.get(
          `Admin/User/GetFull?id=${this.id}`
        );
        if (respons.status !== 200) return;
        this.model = respons.data;
        this.isReady = true;
      } catch (error) {
        console.log(error);
        alert("Error occured - load data");
        this.$emit("close");
      }
    },
    async loadDepartments() {
      try {
        const respons = await this.axios.get(`Admin/Department/GetAllCombo`);
        if (respons.status !== 200) return;
        this.departmentsList = respons.data;
      } catch (error) {
        alert("Error occured - load departments");
      }
    },
    getEmptyModel() {
      return {
        id: null,
        active: false,
        email: null,
        password: null,
        phone: null,
        username: null,
        isAdmin: false,
        isPickupPoint: false,
        itemHistory: [],
        departments: [],
      };
    },
  },
  computed: {
    allowedDepartment() {
      return this.departmentsList.filter(
        (x) => !this.model.departments.some((y) => y.id === x.id)
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
          return `Edit: ${this.model.username}`;
        case "info":
          return `Info about: ${this.model.username}`;
        default:
          return "";
      }
    },
  },
  async mounted() {
    switch (this.mode) {
      case "add":
        this.model = this.getEmptyModel();
        await this.loadDepartments();
        this.isReady = true;
        break;
      case "edit":
        await this.loadData();
        await this.loadDepartments();
        break;
      case "info":
        this.globalDisable = true;
        await this.loadData();
        await this.loadDepartments();
        break;
      default:
        this.$emit("close");
        break;
    }
  },
};
</script>

<style scoped lang="scss">
.user_modal {
}
</style>
