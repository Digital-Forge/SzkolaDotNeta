<template>
  <div class="department_modal">
    <modal @close="close">
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

            <div class="row mt-2" v-if="mode != 'add'">
              <div class="col-3 name_value"><span>Created</span></div>
              <div class="col-9 box_value">
                <input type="date" v-model="model.create" :disabled="true" />
              </div>
            </div>
          </div>
          <div v-if="selectedPage == 'users'" class="users_info">
            <div class="tab">
              <div class="row header_table">
                <div class="col-10"><b>Fullname</b></div>
                <div class="col-2"><b>Remove</b></div>
              </div>
              <div
                class="row row_table"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(user, index) in model.users"
                :key="user.id"
              >
                <div class="col-10">{{ user.fullname }}</div>
                <div class="col-2">{{ "X" }}</div>
              </div>
            </div>
          </div>
          <div v-if="selectedPage == 'items'" class="items_info">
            <div class="tab">
              <div class="row header_table">
                <div class="col-10"><b>Item name</b></div>
                <div class="col-2"><b>Remove</b></div>
              </div>
              <div
                class="row row_table"
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
        <div v-else class="loader_fix">
          <span class="loader"></span>
        </div>
      </template>
      <template v-slot:footer>
        <div class="save_box">
          <button class="save" type="button" @click="save">save</button>
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
    async loadData() {
      if (!this.id) {
        this.$emit("close");
        return;
      }

      try {
        const respons = await this.axios.get(
          `Department/GetFull?id=${this.id}`
        );
        if (respons.status !== 200) return;
        this.model = respons.data;
        this.isReady = true;
      } catch (error) {
        console.log(error);
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
  computed: {
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
  .content {
    .page_select {
      border: 1px black solid;
      background-color: gray;
      padding: 0;
      margin: 0;
    }

    .page_select:hover {
      background-color: magenta;
      font-weight: bold;
    }

    .page_select_active {
      background-color: greenyellow;
    }

    .name_value {
      background-color: transparent;
      text-align: right;
      justify-content: center;
      justify-items: center;
      margin: auto;
      padding: 0;
      color: black;
    }

    .box_value {
      padding-right: 2rem;
      padding-left: 0.5rem;
      margin: 0;

      input {
        width: 100%;
      }

      textarea {
        width: 100%;
        height: 20rem;
      }
    }
  }

  .save {
    border-radius: 1rem;
    padding-left: 0.5rem;
    padding-right: 0.5rem;
    margin-right: 1rem;

    border: 1px black dashed;
    background-color: rgb(201, 112, 40);
  }

  .save:hover {
    background-color: rgb(153, 255, 0);
    border: 1px black solid;
  }

  .save_box {
    float: left;
    display: flex;
    align-items: right;
    justify-content: right;
    width: 100%;
  }

  .loader_fix {
    align-content: center;
    align-items: center;
    justify-content: center;
    margin: 0;
    display: flex;
    height: 100%;
  }
}
</style>
