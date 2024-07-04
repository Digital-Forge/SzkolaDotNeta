<template>
  <div class="item_info_modal">
    <modal ref="mainModal" :height="90" :width="60" @close="close">
      <template v-slot:header
        ><span v-if="isReady"
          ><b>{{ model.name }}</b></span
        ></template
      >
      <template v-slot:content>
        <div v-if="isReady" class="content">
          <div class="row mx-0 px-0">
            <div
              class="col-6 page_select"
              @click="setPage('general')"
              :class="[{ page_select_active: selectedPage == 'general' }]"
            >
              General info
            </div>
            <div
              class="col-6 page_select"
              @click="setPage('actions')"
              :class="[{ page_select_active: selectedPage == 'actions' }]"
            >
              Actions
            </div>
          </div>

          <!----------------------------------------------------------------------------------------------------- General -------------->
          <div v-if="selectedPage == 'general'" class="general_info">
            <div class="reserve_info">
              <div class="reserve_info_data reserve_info_status">
                <span class="header_info">Status :</span>
                <span>{{ camelCaseToNormal(model.status) }}</span>
              </div>

              <div class="reserve_info_data">
                <span class="header_info">From :</span>
                <span>{{ model.from }}</span>
              </div>

              <div class="reserve_info_data">
                <span class="header_info">To :</span>
                <span>{{ model.to }}</span>
              </div>
            </div>

            <div class="serial_info">
              <span class="header_info">Serial number :</span>
              <span>{{ model.serialNumber }}</span>
            </div>

            <div class="text_description">
              <div class="header_info">Description</div>
              <div>{{ model.description }}</div>
            </div>

            <div class="tab" v-if="model.files && model.files.length > 0">
              <div class="row header_tab">
                <div class="col-8">
                  <b>Files</b>
                </div>
                <div class="col-2"><b>Status</b></div>
                <div class="col-2"><b>Download</b></div>
              </div>
              <div>
                <div
                  class="row row_tab"
                  :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                  v-for="(file, index) in model.files"
                  :key="file.id"
                >
                  <div class="col-8">
                    {{ file.name }}
                  </div>
                  <div class="col-2">
                    {{ file.isMissing ? "Error" : "OK" }}
                  </div>
                  <div class="col-2" v-if="!file.isMissing">
                    <FileDownload :id="file.id" class="download_btn"
                      >Download</FileDownload
                    >
                  </div>
                </div>
              </div>
            </div>

            <div class="photo_show">
              <div class="photo_carousel">
                <div
                  class="photo_carousel_img"
                  v-for="image in model.images"
                  :key="image"
                  @click="selectImage = image"
                >
                  <ImageBox :id="image"></ImageBox>
                </div>
              </div>
              <div class="photo_screen_box">
                <div class="photo_screen">
                  <ImageBox
                    class="photo_screen_obj"
                    :id="selectImage"
                    :key="selectImage"
                    verticalMode
                  ></ImageBox>
                </div>
              </div>
            </div>
          </div>

          <!----------------------------------------------------------------------------------------------------- Actions -------------->
          <div v-show="selectedPage == 'actions'" class="actions_page">
            <div class="mt-3 prop_box row">
              <span class="header_prop col-6">Change status on : </span>
              <select
                class="value_prop col-3"
                v-model="changeModel.status"
                :key="changeModel.adminMode"
              >
                <option v-if="changeModel.adminMode" value="InPreparation">
                  In preparation
                </option>
                <option
                  v-if="mode == 'preparation' || changeModel.adminMode"
                  value="ReadyToPickedUp"
                >
                  Ready to picked up
                </option>
                <option
                  v-if="mode == 'release' || changeModel.adminMode"
                  value="Issued"
                >
                  Issued
                </option>
                <option
                  v-if="
                    mode == 'preparation' ||
                    mode == 'release' ||
                    changeModel.adminMode
                  "
                  value="Canceled"
                >
                  Canceled
                </option>
                <option
                  v-if="mode == 'return' || changeModel.adminMode"
                  value="Returned"
                >
                  Returned
                </option>
                <option
                  v-if="mode == 'return' || changeModel.adminMode"
                  value="Lost"
                >
                  Lost
                </option>
                <option
                  v-if="mode == 'return' || changeModel.adminMode"
                  value="Destroyed"
                >
                  Destroyed
                </option>
              </select>
            </div>
            <div v-if="changeModel.adminMode">
              <div class="prop_box row">
                <span class="header_prop col-6">Change data : </span>
                <div class="value_prop_checkbox col-3">
                  <input
                    class="value_prop"
                    type="checkbox"
                    v-model="changeDate"
                  />
                </div>
              </div>
              <div>
                <div class="prop_box row">
                  <span class="header_prop col-6">New from : </span>
                  <input
                    class="value_prop col-3"
                    type="date"
                    v-model="changeModel.from"
                    :max="changeModel.to"
                    :disabled="!changeDate"
                  />
                </div>
                <div class="prop_box row">
                  <span class="header_prop col-6">New to : </span>
                  <input
                    class="value_prop col-3"
                    type="date"
                    v-model="changeModel.to"
                    :min="changeModel.from"
                    :disabled="!changeDate"
                  />
                  <input
                    class="value_prop col-2"
                    type="button"
                    value="To unlimited"
                    :disabled="!changeDate"
                    @click="changeModel.to = null"
                  />
                </div>
              </div>
            </div>
            <div>
              <div class="prop_box">
                <span class="header_prop">Inner note</span>
              </div>
              <div class="textarea_box">
                <textarea
                  v-model="changeModel.innerNote"
                  :maxlength="3000 - (model.innerNote?.length ?? 0)"
                ></textarea>
              </div>
            </div>
            <div v-if="model.innerNote?.length ?? 0 > 0">
              <div><span> Previous note</span></div>
              <div class="textarea_box">
                <textarea
                  v-model="model.innerNote"
                  :maxlength="3000"
                  :disabled="true"
                ></textarea>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="loader_modal_fix">
          <span class="loader"></span>
        </div>
      </template>
      <template v-slot:footer>
        <div v-if="isReady" class="footer_style">
          <div class="admin_mode_btn">
            <input
              v-if="availableAdminMode"
              type="checkbox"
              v-model="changeModel.adminMode"
            />
            <span class="admin_mode_btn_text"> Admin mode</span>
          </div>
          <div class="change_btn save_box">
            <button
              v-if="selectedPage == 'actions'"
              class="save"
              type="button"
              @click="sendChanges"
            >
              Change status
            </button>
          </div>
        </div>
      </template>
    </modal>
  </div>
</template>

<script>
import modal from "@/components/ModalWindow.vue";
import ImageBox from "@/components/ImageBox.vue";
import FileDownload from "@/components/FileDownload.vue";

export default {
  components: {
    modal,
    ImageBox,
    FileDownload,
  },
  props: {
    id: {
      type: String,
      required: true,
    },
    mode: {
      type: String,
      required: true,
      validator(value) {
        return ["preparation", "release", "return"].includes(value);
      },
    },
  },
  data() {
    return {
      model: null,
      changeModel: null,
      isReady: false,
      selectedPage: "general",
      selectImage: null,
      availableAdminMode: false,
      changeDate: false,
    };
  },
  watch: {
    changeDate(newValue, oldValue) {
      if (newValue === oldValue) return;
      if (newValue) {
        this.changeModel.from = this.model.from;
        this.changeModel.to = this.model.to;
      } else {
        this.changeModel.from = null;
        this.changeModel.to = null;
      }
    },
    // eslint-disable-next-line
    "changeModel.adminMode": function (newValue, oldValue) {
      if (newValue === oldValue) return;
      if (newValue) return;

      this.changeModel.from = null;
      this.changeModel.to = null;
      this.changeDate = false;

      switch (this.model.status) {
        case "InPreparation":
          this.changeModel.status = "ReadyToPickedUp";
          break;
        case "ReadyToPickedUp":
          this.changeModel.status = "Issued";
          break;
        case "Issued":
          this.changeModel.status = "Returned";
          break;

        default:
          break;
      }
    },
  },
  computed: {},
  methods: {
    async setPage(name) {
      this.selectedPage = name;
    },
    camelCaseToNormal(text) {
      const normalText = text
        .replace(/([a-z])([A-Z])/g, "$1 $2")
        .replace(/([A-Z])([A-Z][a-z])/g, "$1 $2")
        .toLowerCase();

      return normalText.charAt(0).toUpperCase() + normalText.slice(1);
    },
    close() {
      this.$emit("close");
    },
    async sendChanges() {
      this.isReady = false;

      try {
        const respons = await this.axios.post(
          `PickupPoint/Reservation/ChangeReservationStatus`,
          this.changeModel
        );
        if (respons.status !== 200) return;
        this.close();
      } catch (error) {
        alert("Error occured");
      }

      this.isReady = true;
    },
    async loadData() {
      if (!this.id) {
        this.$emit("close");
        return;
      }

      try {
        const respons = await this.axios.get(
          `PickupPoint/Reservation/GetReservationInfo?id=${this.id}`
        );
        if (respons.status !== 200) return;
        this.model = respons.data;
        this.selectImage =
          this.model.images && this.model.images.length > 0
            ? this.model.images[0]
            : null;
      } catch (error) {
        alert("Error occured");
        this.$emit("close");
      }
    },
    async chackAdminPermission() {
      try {
        const respons = await this.axios.get(`Auth/CheckAdminAuth`);
        if (respons.status !== 200) return;
        this.availableAdminMode = true;
      } catch (error) {
        this.availableAdminMode = false;
      }
    },
    async prepareChangeModel() {
      this.changeModel = {
        reservationId: this.model.id,
        adminMode: false,
        innerNote: "",
        status: null,
        from: null,
        to: null,
      };

      switch (this.model.status) {
        case "InPreparation":
          this.changeModel.status = "ReadyToPickedUp";
          break;
        case "ReadyToPickedUp":
          this.changeModel.status = "Issued";
          break;
        case "Issued":
          this.changeModel.status = "Returned";
          break;

        default:
          break;
      }
    },
  },
  async mounted() {
    await this.loadData();
    await this.chackAdminPermission();
    await this.prepareChangeModel();
    this.isReady = true;
  },
};

/*
InPreparation,
ReadyToPickedUp,
Issued,
Returned,
Lost,
Destroyed
      */
</script>

<style lang="scss" scoped>
.item_info_modal {
  .general_info {
    .reserve_info {
      display: flex;
      margin: 0.7rem;
      margin-bottom: 0;

      &_data {
        width: 100%;
      }

      &_status {
        display: flex;
      }
    }

    .serial_info {
      display: flex;
      justify-content: left;
      margin: 0.7rem;
      margin-top: 0;
      margin-bottom: 0;
    }

    .text_description {
      border: 3px dashed brown;
      border-radius: 0.8rem;
      margin: 0.7rem;
      margin-top: 0;
      padding: 0.5rem;
      padding-top: 0;
      text-align: justify;
      margin-bottom: auto;
    }

    .header_info {
      font-weight: bold;
      margin-right: 0.3rem;
    }

    .photo_show {
      display: flex;
      margin: 0.7rem;
      border-radius: 0.8rem;
      border: 3px solid brown;
      height: 20rem;

      .photo_carousel {
        overflow-y: auto;
        width: 12%;
        border-right: 3px solid brown;
        padding: 0;
        margin: 0;
      }

      .photo_carousel_img {
        border-radius: 0.5rem;
        overflow: hidden;
        border: 2px dashed darkolivegreen;
        margin-top: 0.5rem;
        margin-right: 0.5rem;
        margin-left: 0.5rem;
        padding: 0;
        cursor: pointer;
      }

      .photo_screen_box {
        width: 88%;
        justify-content: center;
        align-items: center;
        overflow: hidden;
      }

      .photo_screen {
        width: auto;
        height: 100%;
        display: flex;
        padding: 0.5rem;
        overflow-x: auto;
      }

      .photo_screen_obj {
        margin-left: auto;
        margin-right: auto;
        width: max-content;
      }
    }

    .download_btn {
      &:hover {
        font-weight: bold;
      }
    }
  }

  .actions_page {
    width: 100%;
    padding: 0;
    margin: 0;

    .prop_box {
      margin: 0;
      padding: 0;
      margin-top: 0.3rem;

      .header_prop {
        text-align: right;
        padding-left: 0.25rem;
        padding-right: 0.25rem;
        font-weight: bold;
      }

      .value_prop {
        padding-left: 0;
        padding-right: 0;
      }

      .value_prop_checkbox {
        display: flex;
        justify-content: left;
        align-items: center;
        padding-left: 0;
        padding-right: 0;
        margin-left: 0.15rem;
      }
    }

    .textarea_box {
      width: 100%;
      padding-left: 1rem;
      padding-right: 1rem;
    }

    textarea {
      width: 100%;
      min-height: 10rem;
    }
  }

  .footer_style {
    display: flex;
    width: 100%;

    .admin_mode_btn {
      display: flex;
    }

    .admin_mode_btn_text {
      justify-content: center;
      align-content: center;
      white-space: nowrap;
      margin-left: 0.5rem;
    }
  }
}
</style>
