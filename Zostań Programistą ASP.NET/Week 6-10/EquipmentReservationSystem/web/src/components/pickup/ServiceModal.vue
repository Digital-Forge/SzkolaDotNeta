<template>
  <div class="service_modal">
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
              @click="setPage('service')"
              :class="[{ page_select_active: selectedPage == 'service' }]"
            >
              Service
            </div>
          </div>

          <!----------------------------------------------------------------------------------------------------- General -------------->
          <div v-if="selectedPage == 'general'" class="general_info">
            <div class="reserve_info">
              <div class="header_info">Last reservation info</div>
              <div class="reserve_info_data">
                <div class="reserve_info_date">
                  <span class="header_info">By :</span>
                  <span>{{ model.lastReservationInfo.username }}</span>
                </div>

                <div class="reserve_info_date">
                  <span class="header_info">From :</span>
                  <span>{{ model.lastReservationInfo.from }}</span>
                </div>

                <div class="reserve_info_date">
                  <span class="header_info">To :</span>
                  <span>{{ model.lastReservationInfo.to }}</span>
                </div>
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

          <!----------------------------------------------------------------------------------------------------- Service -------------->
          <div v-show="selectedPage == 'service'" class="services_page">
            <div class="mt-3 prop_box row">
              <span class="header_prop col-6">Service status : </span>
              <select class="value_prop col-3" v-model="changeModel.status">
                <option value="Serviced">In serviced</option>
                <option value="Repaired">Repaired</option>
                <option value="Destroyed">Destroyed</option>
              </select>
            </div>
            <div>
              <div class="prop_box">
                <span class="header_prop">Service note</span>
              </div>
              <div class="textarea_box">
                <textarea
                  v-model="changeModel.serviceNote"
                  :maxlength="3000 - 35"
                ></textarea>
              </div>
            </div>
            <div v-if="model.serviceNoteList.length > 0">
              <div><span> Previous note</span></div>
              <div
                v-for="(note, index) in model.serviceNoteList"
                :key="index"
                class="textarea_box"
              >
                <textarea :value="note" :disabled="true"></textarea>
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
          <div class="change_btn save_box">
            <button
              v-if="selectedPage == 'service'"
              class="save"
              type="button"
              @click="sendChanges"
            >
              Save
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
  },
  data() {
    return {
      model: null,
      changeModel: null,
      isReady: false,
      selectedPage: "general",
      selectImage: null,
    };
  },
  methods: {
    async setPage(name) {
      this.selectedPage = name;
    },
    close() {
      this.$emit("close");
    },
    async sendChanges() {
      this.isReady = false;

      try {
        const respons = await this.axios.post(
          `PickupPoint/Service/UpdateServieceItem`,
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
          `PickupPoint/Service/GetServiceItemInfo?id=${this.id}`
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
    async prepareChangeModel() {
      this.changeModel = {
        id: this.model.id,
        status: "Serviced",
        serviceNote: null,
      };
    },
  },
  async mounted() {
    await this.loadData();
    await this.prepareChangeModel();
    this.isReady = true;
  },
};

/*
Serviced
Repaired
Destroyed
*/
</script>

<style lang="scss" scoped>
.service_modal {
  .general_info {
    .reserve_info {
      margin: 0.7rem;
      margin-bottom: 0;
      border: 3px dashed brown;
      border-radius: 0.8rem;

      &_data {
        display: flex;
      }

      &_date {
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
      margin-top: 1rem;
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

  .services_page {
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
  }
}
</style>
