<template>
  <div class="item_to_reservation_modal">
    <modal ref="mainModal" @close="close">
      <template v-slot:header
        ><span v-if="isReady"
          ><b>{{ model.name }}</b></span
        ></template
      >
      <template v-slot:content>
        <div v-if="isReady" class="content">
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
          <div class="info_data">
            <div
              class="text_description"
              :style="{ width: (model.files.length > 0 ? 75 : 100) + '%' }"
            >
              <div class="header_info">Description</div>
              <div>{{ model.description }}</div>
            </div>
            <div v-if="model.files && model.files.length > 0" class="tab files">
              <div class="row header_tab">
                <div class="col-12"><b>Files</b></div>
              </div>
              <div>
                <div
                  class="row row_tab"
                  :class="[index % 2 === 0 ? 'row_color_1' : 'row_color_2']"
                  v-for="(file, index) in model.files"
                  :key="file.id"
                >
                  <div class="file_name">{{ file.name }}</div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="loader_modal_fix">
          <span class="loader"></span>
        </div>
      </template>
      <template v-slot:footer>
        <div class="reserve_btn_box">
          <button class="reserve_btn" type="button" @click="reserve">
            Reserve
          </button>
        </div>
      </template>
    </modal>
    <modal
      ref="reservationModal"
      v-if="reserveProcess"
      :height="22"
      :width="25"
      :z-index="fixZIndex"
      @close="close"
    >
      <template v-slot:header
        ><span
          ><b>Reserve: {{ model.name }}</b></span
        ></template
      >
      <template v-slot:content>
        <div v-if="isReserveReady" class="content vertical_center_box">
          <div class="vertical_center">
            <div class="row">
              <span class="col-4">From</span>
              <input
                class="col-7"
                type="date"
                v-model="modelResevation.from"
                :max="modelResevation.to"
                :min="minDate"
                @blur="v$.modelResevation.from.$touch()"
              />
              <span
                class="invalid_info"
                v-for="error of v$.modelResevation.from.$errors"
                :key="error.$uid"
              >
                {{ error.$message }}
              </span>
            </div>
            <div class="row mt-2">
              <span class="col-4">To</span>
              <input
                class="col-7"
                type="date"
                v-model="modelResevation.to"
                :min="modelResevation.from"
                @blur="v$.modelResevation.to.$touch()"
              />
              <span
                class="invalid_info"
                v-for="error of v$.modelResevation.to.$errors"
                :key="error.$uid"
              >
                {{ error.$message }}
              </span>
            </div>
          </div>
        </div>
        <div v-else class="loader_modal_fix">
          <span class="loader"></span>
        </div>
      </template>
      <template v-slot:footer>
        <div class="reserve_btn_box">
          <button class="reserve_btn" type="button" @click="reserveExecution">
            Reserve
          </button>
        </div>
      </template>
    </modal>
  </div>
</template>

<script>
import { useVuelidate } from "@vuelidate/core";
import { required } from "@vuelidate/validators";
import modal from "@/components/ModalWindow.vue";
import ImageBox from "@/components/ImageBox.vue";

export default {
  setup() {
    return {
      v$: useVuelidate(),
    };
  },
  components: {
    modal,
    ImageBox,
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
      selectImage: null,
      reserveProcess: false,
      modelResevation: null,
      isReserveReady: false,
      minDate: null,
    };
  },
  validations() {
    return {
      modelResevation: {
        from: { required },
        to: { required },
      },
    };
  },
  computed: {
    fixZIndex() {
      return this.$refs.mainModal.zIndex + 10;
    },
  },
  methods: {
    async reserve() {
      await this.getModelResevation();
      this.reserveProcess = true;
      await this.checkAvailableItem();
    },
    close() {
      this.$emit("close");
    },
    async getModelResevation() {
      const currentDate = new Date();
      currentDate.setDate(currentDate.getDate() + 1);
      const year = currentDate.getFullYear();
      const month = String(currentDate.getMonth() + 1).padStart(2, "0");
      const day = String(currentDate.getDate()).padStart(2, "0");
      const tomorrowDate = `${year}-${month}-${day}`;

      this.minDate = tomorrowDate;
      this.modelResevation = {
        itemId: this.id,
        from: tomorrowDate,
        to: null,
      };
    },
    async loadData() {
      if (!this.id) {
        this.$emit("close");
        return;
      }

      try {
        const respons = await this.axios.get(`Item/GetItem?id=${this.id}`);
        if (respons.status === 404) {
          alert("The item is no longer available");
          this.$emit("close");
          return;
        }
        if (respons.status !== 200) {
          alert("Error occured");
          this.$emit("close");
          return;
        }
        this.model = respons.data;
        this.selectImage =
          this.model.images.length > 0 ? this.model.images[0] : null;
        this.isReady = true;
      } catch (error) {
        alert("Error occured");
        this.$emit("close");
      }
    },
    async checkAvailableItem() {
      try {
        const respons = await this.axios.get(
          `Item/CheckAvailableItem?id=${this.id}`
        );
        if (respons.status === 404) {
          alert("The item is no longer available");
          this.$emit("close");
          return;
        }
        if (respons.status !== 200) {
          alert("Error occured");
          this.$emit("close");
          return;
        }
        this.isReserveReady = true;
      } catch (error) {
        alert("Error occured");
        this.$emit("close");
      }
    },
    async reserveExecution() {
      this.isReserveReady = false;
      this.v$.$touch();
      if (this.v$.modelResevation.$invalid) {
        this.isReserveReady = true;
        return;
      }

      try {
        const respons = await this.axios.post(
          "Reservation/CreateReservation",
          this.modelResevation
        );
        if (respons.status === 404) alert("The item is no longer available");
        if (respons.status !== 200) alert("Error occured");
        this.$emit("close");
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
.item_to_reservation_modal {
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

  .info_data {
    display: flex;
    margin: 0.7rem;
    padding: 0;

    .text_description {
      border: 3px dashed brown;
      border-radius: 0.8rem;
      margin: 0;
      padding: 0.5rem;
      padding-top: 0;
      text-align: justify;
      margin-bottom: auto;
    }

    .header_info {
      font-weight: bold;
    }

    .files {
      width: 25%;
      border: 3px dashed black;
      border-radius: 0.8rem;
      margin: 0;
      margin-left: 0.75rem;
      overflow: hidden;
      margin-bottom: auto;

      .file_name {
        text-align: left;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
    }
  }

  .reserve_btn_box {
    padding-right: 1.4rem;
    margin-left: auto;
    margin-right: auto;
    width: 90%;

    .reserve_btn {
      width: 100%;
      margin-top: 0.2rem;
      margin-bottom: 0.2rem;
      padding-top: 0.2rem;
      padding-bottom: 0.2rem;
      border: 2px dashed #333;
      border-radius: 25px;
      background-color: #f0f0f0;
      color: #333;
      font-size: 16px;
      font-weight: bold;
      cursor: pointer;
      transition: background-color 0.3s, color 0.3s;
      transition: border 0.3s, color 0.3s;

      &:hover {
        background-color: #333;
        color: #fff;
        border: 2px dashed #fff;
      }
    }
  }

  .vertical_center {
    display: flex;
    flex-grow: 1;
    flex-direction: column;
    justify-content: center;
    height: 100%;
    width: 100%;
  }

  .vertical_center_box {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100%;
  }
}
</style>
