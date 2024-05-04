<template>
  <div class="pagging_bar">
    <div
      class="pabe_btn"
      :class="{ pabe_btn_active: option.page === 1 }"
      @click="clickPage(1)"
    >
      1
    </div>
    <div v-if="option.page > 3 && pages.end > 5">-</div>
    <div v-for="page in pages.middle" :key="page">
      <div
        class="pabe_btn"
        :class="{ pabe_btn_active: option.page === page }"
        @click="clickPage(page)"
      >
        {{ page }}
      </div>
    </div>
    <div v-if="pages.end && pages.end - option.page >= 3 && pages.end > 5">
      -
    </div>
    <div v-if="pages.end">
      <div
        class="pabe_btn"
        :class="{ pabe_btn_active: option.page === pages.end }"
        @click="clickPage(pages.end)"
      >
        {{ pages.end }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    apiPath: {
      required: true,
      type: String,
    },
    currentPage: {
      default: 1,
      required: false,
      type: Number,
    },
    show: {
      default: 30,
      required: false,
      type: Number,
    },
    search: {
      default: null,
      required: false,
      type: String,
    },
    searchExtraData: {
      default: null,
      required: false,
      type: Array,
    },
  },
  data() {
    return {
      pages: {
        middle: [],
        end: null,
      },
      option: {
        page: null,
        total: null,
      },
    };
  },
  watch: {
    async search(oldValue, newValue) {
      if (oldValue === newValue) return;
      await this.clickPage(1);
    },
    async searchExtraData(oldValue, newValue) {
      if (oldValue === newValue) return;
      await this.clickPage(1);
    },
  },
  methods: {
    async clickPage(pageNumber) {
      const tableOptions = {
        take: this.show,
        skip: (pageNumber - 1) * this.show,
        search: this.search,
      };

      if (this.searchExtraData) {
        this.searchExtraData.forEach((element) => {
          tableOptions[element.name] = element.value;
        });
      }

      const respons = await this.axios.post(this.apiPath, tableOptions);
      if (!respons || !respons.data || respons.status !== 200) {
        alert("Error - Unable to download data");
        return;
      }

      this.option.page = pageNumber;
      this.option.total = respons.data.totalCount;
      await this.calculatePages();
      this.$emit("changePage", respons.data.data);
    },
    async calculatePages() {
      this.pages.middle = [];
      const totalPage = Math.ceil(this.option.total / this.show);
      this.pages.end = totalPage === 1 ? null : totalPage;
      let currentStart = this.option.page + 1;

      if (this.pages.end - this.option.page < 2)
        currentStart -= 2 - (this.pages.end - this.option.page);

      for (
        let i = currentStart - 2;
        i < this.pages.end && i <= currentStart;
        i++
      ) {
        if (i < 2) {
          i = 1;
          currentStart = 4;
          continue;
        }
        this.pages.middle.push(i);
      }
    },
  },
  mounted() {
    this.option.page = this.currentPage;
    this.clickPage(1);
  },
};
</script>
<style lang="scss">
.pagging_bar {
  width: min-content;
  display: flex;

  .pabe_btn {
    background-color: deepskyblue;
    border: 1px solid dodgerblue;
    padding-left: 0.3rem;
    padding-right: 0.3rem;
    margin: 0.2rem;
    flex: 1;
    border-radius: 0.5rem;

    &:hover {
      background-color: dodgerblue;
      border: 2px solid greenyellow;
    }

    &_active {
      background-color: gold !important;
      border: 1px solid black !important;
      font-weight: bold;
    }
  }
}
</style>
