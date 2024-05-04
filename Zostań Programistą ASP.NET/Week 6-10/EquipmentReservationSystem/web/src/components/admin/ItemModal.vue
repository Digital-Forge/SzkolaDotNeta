<template>
  <div class="user_modal">
    <modal
      ref="mainModal"
      :z-index="zIndexFix"
      :width="80"
      :height="90"
      @close="close"
    >
      <template v-slot:header
        ><span
          ><b>{{ title }}</b></span
        ></template
      >
      <template v-slot:content>
        <div v-if="isReady" class="content">
          <!-----------------------------------------------------------------------------------------------------Nav---------------------->
          <div class="row mx-0 px-0">
            <div
              class="page_select col-6"
              :class="[{ page_select_active: selectedPage == 'general' }]"
              @click="setPage('general')"
            >
              General
            </div>
            <div
              class="col-6 page_select"
              :class="[{ page_select_active: selectedPage == 'images' }]"
              @click="setPage('images')"
            >
              Images
            </div>
            <div
              class="col-4 page_select"
              :class="[{ page_select_active: selectedPage == 'files' }]"
              @click="setPage('files')"
            >
              Files
            </div>
            <div
              class="col-4 page_select"
              :class="[{ page_select_active: selectedPage == 'departments' }]"
              @click="setPage('departments')"
            >
              Departments
            </div>
            <div
              class="col-4 page_select"
              :class="[{ page_select_active: selectedPage == 'history' }]"
              @click="setPage('history')"
            >
              History
            </div>
          </div>

          <!-----------------------------------------------------------------------------------------------------General------------------>
          <div v-if="selectedPage == 'general'" class="general_info">
            <div class="left_box">
              <div class="general_data">
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
                      v-model="model.description"
                      :disabled="globalDisable"
                      style="height: 25vh"
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
                    <input
                      type="date"
                      v-model="model.create"
                      :disabled="true"
                    />
                  </div>
                </div>
              </div>
              <div class="serial_data">
                <span><b>Serial object</b></span>
                <div
                  v-if="selectedSerialItem"
                  class="remove_serial_item"
                  @click="removeSerialItem()"
                >
                  Remove
                </div>
                <div v-if="selectedSerialItem">
                  <div class="row mt-2">
                    <div class="col-3 name_value">
                      <span>Serial number</span>
                    </div>
                    <div class="col-9 box_value">
                      <input
                        type="text"
                        v-model="selectedSerialItem.serialNumber"
                        :disabled="globalDisable"
                      />
                    </div>
                  </div>

                  <div class="row mt-2">
                    <div class="col-3 name_value"><span>Active</span></div>
                    <div class="col-9 box_value_chackbox">
                      <input
                        type="checkbox"
                        v-model="selectedSerialItem.active"
                        :disabled="globalDisable"
                      />
                    </div>
                  </div>

                  <div class="row mt-2">
                    <div class="col-3 name_value"><span>Status</span></div>
                    <div class="col-9 box_value">
                      <select
                        v-model="selectedSerialItem.status"
                        :disabled="globalDisable"
                        class="box_value_chackbox"
                      >
                        <option value="Available">Available</option>
                        <option value="Unavailable">Unavailable</option>
                        <option value="Reservations">Reservations</option>
                        <option value="Service" selected>Service</option>
                        <option value="Withdrawn" selected>Withdrawn</option>
                      </select>
                    </div>
                  </div>

                  <div class="row mt-2">
                    <div class="col-3 name_value"><span>Added date</span></div>
                    <div class="col-9 box_value">
                      <input
                        type="date"
                        :max="selectedSerialItem.withdrawalDate"
                        v-model="selectedSerialItem.addedDate"
                        :disabled="globalDisable"
                      />
                    </div>
                  </div>

                  <div class="row mt-2">
                    <div class="col-3 name_value">
                      <span>Withdrawal date</span>
                    </div>
                    <div class="col-9 box_value">
                      <input
                        type="date"
                        :min="selectedSerialItem.addedDate"
                        v-model="selectedSerialItem.withdrawalDate"
                        :disabled="globalDisable"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="right_box">
              <div v-if="!globalDisable" class="pt-2 modal_tab_add">
                <button type="button" @click="addSerialItem()">Add</button>
              </div>
              <div class="tab">
                <div class="row header_tab">
                  <div class="col-2">
                    <b>Active</b>
                  </div>
                  <div class="col-6">
                    <b>Serial number</b>
                  </div>
                  <div class="col-4">
                    <b>Status</b>
                  </div>
                </div>
                <div
                  class="row row_tab"
                  :class="[
                    index % 2 === 0 ? 'row_color_1' : 'row_color_2',
                    { active: selectedSerialItem === item },
                  ]"
                  v-for="(item, index) in model.instances"
                  :key="index"
                  @click="selectedSerialItem = item"
                >
                  <div class="p-0 col-2">
                    <input
                      type="checkbox"
                      v-model="item.active"
                      :disabled="true"
                    />
                  </div>
                  <div class="col-6">
                    {{ item.serialNumber }}
                  </div>
                  <div class="col-4">
                    {{ item.status }}
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-----------------------------------------------------------------------------------------------------Images=------------------>
          <div v-if="selectedPage == 'images'" class="images_info">
            <div
              class="image_content"
              v-for="image in model.imagesId"
              :key="image"
            >
              <div class="image_content_box">
                <div class="image_content_box_img">
                  <image-box :id="image"></image-box>
                </div>
                <div v-if="!globalDisable" class="image_content_box_delete">
                  <button
                    class="image_content_box_delete_btn"
                    type="button"
                    @click="removeImage(image)"
                  >
                    X
                  </button>
                </div>
              </div>
            </div>
            <div
              class="image_content image_content_box image_content_box_upload"
            >
              <fileUpload
                :accepted-format="'image/*'"
                :multiple="true"
                @resId="addImage"
              ></fileUpload>
            </div>
          </div>

          <!-----------------------------------------------------------------------------------------------------Files-------------------->
          <div v-if="selectedPage == 'files'" class="files_info">
            <div v-if="!globalDisable" class="files_upload">
              <fileUpload :multiple="true" @resId="addFile"></fileUpload>
            </div>
            <div class="tab">
              <div class="row header_tab">
                <div class="col-1">
                  <b>Active</b>
                </div>
                <div :class="globalDisable ? 'col-7' : 'col-6'">
                  <b>File name</b>
                </div>
                <div class="col-2">
                  <b>Date</b>
                </div>
                <div class="col-2">
                  <b>Download</b>
                </div>
                <div v-if="!globalDisable" class="col-1">
                  <b>Remove</b>
                </div>
              </div>
              <div
                class="row row_tab"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(file, index) in model.addictionFiles"
                :key="file.id"
              >
                <div class="p-0 col-1 row_tab_center">
                  <input type="checkbox" v-model="file.active" />
                </div>
                <div
                  class="row_tab_center"
                  :class="globalDisable ? 'col-7' : 'col-6'"
                >
                  {{ file.name }}
                </div>
                <div class="col-2 row_tab_center">
                  {{ dateformatter(file.date) }}
                </div>
                <div class="col-2 row_tab_center">
                  <file-download :id="file.id"
                    ><span class="download_btn">Download</span></file-download
                  >
                </div>
                <div v-if="!globalDisable" class="col-1 row_tab_center">
                  <span class="tab_removeBtn" @click="removeFile(file.id)">
                    X
                  </span>
                </div>
              </div>
            </div>
          </div>

          <!-----------------------------------------------------------------------------------------------------Departments-------------->
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

          <!-----------------------------------------------------------------------------------------------------History------------------>
          <div v-if="selectedPage == 'history'" class="history_info">
            <div class="tab">
              <div class="row header_tab">
                <div class="col-5"><b>Serial number</b></div>
                <div class="col-2"><b>From</b></div>
                <div class="col-2"><b>To</b></div>
                <div class="col-3"><b>Status</b></div>
              </div>
              <div
                class="row row_tab"
                :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                v-for="(item, index) in model.reservationsHistory"
                :key="item.id"
              >
                <div class="col-5">{{ item.serialNumber }}</div>
                <div class="col-2">{{ item.From }}</div>
                <div class="col-2">{{ item.To }}</div>
                <div class="col-3">{{ item.Status }}</div>
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
import { format } from "date-fns";
import modal from "@/components/ModalWindow.vue";
import imageBox from "@/components/ImageBox.vue";
import fileUpload from "@/components/FileUpload.vue";
import fileDownload from "@/components/FileDownload.vue";

export default {
  components: {
    modal,
    imageBox,
    fileUpload,
    fileDownload,
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
      isReady: false,
      selectedPage: "general",
      globalDisable: false,
      showAddDepartmentModal: false,
      selectDepartment: null,
      model: null,
      departmentsList: null,
      selectedSerialItem: null,
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
    async addImage(idList) {
      this.model.imagesId.push(...idList);
    },
    async removeImage(id) {
      this.model.imagesId = this.model.imagesId.filter((x) => x !== id);
    },
    async addFile(idList) {
      [...idList].forEach(async (id) => {
        try {
          const respons = await this.axios.get(`File/Info?id=${id}`);
          if (!respons.data) return;

          this.model.addictionFiles.push({
            id,
            active: true,
            name: respons.data.name,
            date: respons.data.date,
          });
        } catch (error) {
          console.log(error);
        }
      });
    },
    async removeFile(id) {
      this.model.addictionFiles = this.model.addictionFiles.filter(
        (x) => x.id !== id
      );
    },
    dateformatter(date) {
      return format(new Date(date), "dd-MM-yyyy HH:mm");
    },
    async save() {
      try {
        const respons =
          this.model.id == null
            ? await this.axios.put(`Admin/Item/Create`, this.model)
            : await this.axios.patch(`Admin/Item/Update`, this.model);
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
        const respons = await this.axios.get(`Admin/Item/Get?id=${this.id}`);
        if (respons.status !== 200) return;
        this.model = respons.data;
        this.selectedSerialItem = this.model.instances[0] ?? null;
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
    async addSerialItem() {
      this.selectedSerialItem = {
        id: null,
        active: false,
        serialNumber: null,
        status: null,
        addedDate: null,
        withdrawalDate: null,
      };
      this.model.instances.push(this.selectedSerialItem);
    },
    async selectSerialItem(item) {
      this.selectedSerialItem = item;
    },
    async removeSerialItem() {
      const response = confirm("Are you sure you want do that?");
      if (!response) return;

      this.model.instances = this.model.instances.filter(
        (i) => i !== this.selectedSerialItem
      );
      this.selectedSerialItem = null;
    },
    getEmptyModel() {
      return {
        id: null,
        active: false,
        name: null,
        description: null,
        departments: [],
        instances: [],
        imagesId: [],
        addictionFiles: [],
        reservationsHistory: [],
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
  .general_info {
    display: flex;
    .left_box {
      flex: 1;

      .general_data {
        border: 2px solid black;
        margin: 1rem;
        padding-bottom: 1rem;
      }

      .serial_data {
        border: 2px dashed black;
        margin: 1rem;
        padding-bottom: 1rem;
        margin-bottom: 0;
        position: relative;

        .remove_serial_item {
          display: inline-flex;
          position: absolute;
          background-color: red;
          color: wheat;
          border: 1px solid darkred;
          border-radius: 0.5rem;
          padding-left: 0.5rem;
          padding-right: 0.5rem;
          right: 0;

          &:hover {
            cursor: pointer;
            background-color: coral;
          }
        }
      }
    }

    .right_box {
      flex: 1;
    }
  }

  .images_info {
    display: ruby-text;

    .image_content {
      width: 25%;
    }

    .image_content_box {
      border: 2px gray dashed;
      border-radius: 1rem;
      margin: 0.5rem;
      overflow: hidden;
      display: flex;
      align-items: center;
      position: relative;
    }

    .image_content_box_upload {
      text-align: center;
      &:hover {
        background-color: lightgray;
        border: 2px gray solid;
      }
    }

    .image_content_box_img {
      width: 100%;
      height: 100%;
      transition: filter 0.3s ease;
    }

    .image_content_box_delete {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      display: flex;
      justify-content: center;
      align-items: center;
      opacity: 0;
      transition: opacity 0.8s ease;
    }

    .image_content_box_delete_btn {
      padding: 1rem 2rem;
      font-size: 1.3rem;
      color: white;
      background-color: rgba(226, 1, 1, 0.7);
      border: none;
      border-radius: 0.7rem;
      cursor: pointer;
    }

    .image_content_box:hover .image_content_box_img {
      filter: blur(5px);
    }

    .image_content_box:hover .image_content_box_delete {
      opacity: 1;
    }
  }

  .files_info {
    width: 100%;

    .files_upload {
      text-align: center;
      border: 2px gray dashed;
      border-radius: 1rem;
      margin: 0.5rem;
      overflow: hidden;
      display: flex;
      align-items: center;
      position: relative;

      &:hover {
        background-color: lightgray;
        border: 2px gray solid;
      }
    }

    .download_btn {
      &:hover {
        font-weight: bold;
      }
    }
  }
}
</style>
