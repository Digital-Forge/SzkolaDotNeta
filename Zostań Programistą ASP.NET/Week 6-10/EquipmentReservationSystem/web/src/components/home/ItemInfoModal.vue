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
              class="col-4 page_select"
              @click="setPage('general')"
              :class="[
                { page_select_active: selectedPage == 'general' },
                model.files && model.files.length > 0 ? 'col-4' : 'col-6',
              ]"
            >
              General info
            </div>
            <div
              class="col-4 page_select"
              @click="setPage('foto')"
              :class="[
                { page_select_active: selectedPage == 'foto' },
                model.files && model.files.length > 0 ? 'col-4' : 'col-6',
              ]"
            >
              Picture
            </div>
            <div
              v-if="model.files && model.files.length > 0"
              class="col-4 page_select"
              @click="setPage('files')"
              :class="{ page_select_active: selectedPage == 'files' }"
            >
              Files
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
          </div>

          <!----------------------------------------------------------------------------------------------------- Foto -------------->
          <div v-show="selectedPage == 'foto'" class="foto_info">
            <div class="photo_show">
              <div class="photo_carousel">
                <div
                  class="photo_carousel_img"
                  v-for="image in model.images"
                  :key="image"
                  @click="selectImage = image"
                >
                  <ImageBox
                    class="photo_carousel_img2"
                    :id="image"
                    verticalMode
                  ></ImageBox>
                </div>
              </div>
              <div class="photo_screen_box">
                <div class="photo_screen">
                  <ImageBox :id="selectImage" :key="selectImage"></ImageBox>
                </div>
              </div>
            </div>
          </div>

          <!----------------------------------------------------------------------------------------------------- File -------------->
          <div v-show="selectedPage == 'files'" class="file_info">
            <div class="tab">
              <div class="row header_tab">
                <div :class="[isAvailableFile ? 'col-10' : 'col-12']">
                  <b>Files</b>
                </div>
                <div class="col-2" v-if="isAvailableFile"><b>Download</b></div>
              </div>
              <div>
                <div
                  class="row row_tab"
                  :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                  v-for="(file, index) in model.files"
                  :key="file.id"
                >
                  <div :class="[isAvailableFile ? 'col-10' : 'col-12']">
                    {{ file.name }}
                  </div>
                  <div class="col-2" v-if="isAvailableFile">
                    <FileDownload :id="file.id" class="download_btn"
                      >Download</FileDownload
                    >
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="loader_modal_fix">
          <span class="loader"></span>
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
      required: false,
    },
  },
  data() {
    return {
      model: null,
      isReady: false,
      selectedPage: "general",
      selectImage: null,
    };
  },
  computed: {
    isAvailableFile() {
      return ["InPreparation", "ReadyToPickedUp", "Issued"].includes(
        this.model.status
      );
    },
  },
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
    async loadData() {
      if (!this.id) {
        this.$emit("close");
        return;
      }

      try {
        const respons = await this.axios.get(
          `Reservation/GetReservation?id=${this.id}`
        );
        if (respons.status !== 200) return;
        this.model = respons.data;
        this.selectImage =
          this.model.images && this.model.images.length > 0
            ? this.model.images[0]
            : null;
        this.isReady = true;
      } catch (error) {
        alert("Error occured");
        this.$emit("close");
      }
    },
  },
  async mounted() {
    await this.loadData();
  },
};
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
  }

  .foto_info {
    .photo_show {
      margin: 0.7rem;
      border-radius: 0.8rem;
      border: 3px solid brown;

      .photo_carousel {
        display: flex;
        overflow-x: auto;
        white-space: nowrap;
        height: 8rem;
        border-bottom: 3px solid brown;
        padding: 0;
        margin: 0;
      }

      .photo_carousel_img {
        flex: 0 0 auto;
        height: 85%;

        border-radius: 0.5rem;
        overflow: hidden;
        border: 2px dashed darkolivegreen;
        margin-top: 0.5rem;
        margin-bottom: 0.5rem;
        margin-left: 0.5rem;
        padding: 0;
        cursor: pointer;
      }

      .photo_carousel_img2 {
        flex: 0 0 auto;
        height: 100%;
        width: 100%;
      }

      .photo_screen_box {
        width: 100%;
      }

      .photo_screen {
        padding: 0.5rem;
      }
    }
  }

  .file_info {
    .download_btn {
      &:hover {
        font-weight: bold;
      }
    }
  }
}
</style>
