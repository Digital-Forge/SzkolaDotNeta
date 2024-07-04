<template>
  <div id="pickup_area">
    <NavBar :view-type="'pickup'"></NavBar>
    <div class="pickup_container">
      <div class="pickup_content_box">
        <div class="pickup_nav">
          <div
            class="pickup_nav_element"
            :class="[page == 'release' ? 'pickup_nav_element_active' : '']"
            @click="setPage('release')"
          >
            Preparation & release
          </div>
          <div
            class="pickup_nav_element"
            :class="[page == 'returns' ? 'pickup_nav_element_active' : '']"
            @click="setPage('returns')"
          >
            Returns
          </div>
          <div
            class="pickup_nav_element"
            :class="[page == 'service' ? 'pickup_nav_element_active' : '']"
            @click="setPage('service')"
          >
            Service
          </div>
        </div>
        <div class="pickup_content">
          <PreparationAndReleaseItem
            v-if="page == 'release'"
            :searchData="searchOptions"
          ></PreparationAndReleaseItem>
          <ReturnItem
            v-if="page == 'returns'"
            :searchData="searchOptions"
          ></ReturnItem>
          <ServiceItem
            v-if="page == 'service'"
            :searchData="searchOptions"
          ></ServiceItem>
        </div>
      </div>
      <div class="search_box" :key="refreshPage">
        <div class="search_placeholder">
          <span>Search item</span>
        </div>
        <div class="search_combo_input">
          <ComboBoxApi
            v-model:value="searchOptions.searchItem"
            :api="searchItemApiUrl"
            multiple
          ></ComboBoxApi>
        </div>
        <div v-if="page != 'service'" class="search_placeholder">
          <span>Search user</span>
        </div>
        <div v-if="page != 'service'" class="search_combo_input">
          <ComboBoxApi
            v-model:value="searchOptions.searchUser"
            :api="searchUserApiUrl"
            multiple
          ></ComboBoxApi>
        </div>
        <div v-if="page == 'release'" class="search_placeholder">
          <span>Show reservation type</span>
        </div>
        <div v-if="page == 'release'" class="search_text_input">
          <select v-model="searchOptions.showReservationType">
            <option value="All">All</option>
            <option value="InPreparation">In preparation</option>
            <option value="ReadyToPickedUp">Ready to release</option>
          </select>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import NavBar from "@/components/NavBar.vue";
import ComboBoxApi from "@/components/ComboBoxApi.vue";
import PreparationAndReleaseItem from "@/components/pickup/PreparationAndReleaseItem.vue";
import ReturnItem from "@/components/pickup/ReturnItem.vue";
import ServiceItem from "@/components/pickup/ServiceItem.vue";

const authAdministrationArea = async () => {
  try {
    const respons = await axios.get("Auth/CheckPickUpPointAuth");
    return respons.status === 200;
  } catch (error) {
    return false;
  }
};

export default {
  name: "PickUpPointView",
  components: {
    NavBar,
    ComboBoxApi,
    PreparationAndReleaseItem,
    ReturnItem,
    ServiceItem,
  },
  data() {
    return {
      page: "release",
      refreshPage: 0,
      searchOptions: {
        searchItem: null,
        searchUser: null,
        showReservationType: "All",
      },
    };
  },
  computed: {
    searchItemApiUrl() {
      switch (this.page) {
        case "release":
          return "PickupPoint/Reservation/GetAvailableReservedItemToPreparationAndRelease";
        case "returns":
          return "PickupPoint/Reservation/GetAvailableReservedItemToReturns";
        case "service":
          return "PickupPoint/Item/GetAvailableItemInService";
        default:
          return null;
      }
    },
    searchUserApiUrl() {
      switch (this.page) {
        case "release":
          return "PickupPoint/Reservation/GetAvailableReservedUserToPreparationAndRelease";
        case "returns":
          return "PickupPoint/Reservation/GetAvailableReservedUserToReturns";
        case "service":
          return null;
        default:
          return null;
      }
    },
  },
  methods: {
    setPage(selectedPage) {
      this.page = selectedPage;
      this.searchOptions.searchItem = null;
      this.searchOptions.searchUser = null;
      this.searchOptions.showReservationType = "All";
      this.refreshPage++;
    },
  },
  async beforeRouteEnter(to, from, next) {
    const result = await authAdministrationArea();
    if (result) next();
    else next({ name: "home" });
  },
};
</script>

<style lang="scss">
#pickup_area {
  height: 100vh;
  display: flex;
  flex-direction: column;

  .pickup_container {
    display: flex;
    width: 100%;
    height: max-content;
    flex-grow: 1;
  }

  .pickup_nav {
    width: 100%;
    display: flex;
    padding: 0;
    margin: 0;
  }

  .pickup_nav_element {
    width: 33.3%;
    background-color: red;
    border: 1px solid violet;
    cursor: pointer;

    &_active {
      background-color: aqua;
      border: 2px solid violet;
    }

    &:hover {
      background-color: green;
      border: 3px dashed darkmagenta;
    }
  }

  .search_box {
    width: 25%;
    background-color: lightgray;

    .search_placeholder {
      margin-top: 1rem;
    }

    .search_text_input {
      margin-left: 0.5rem;
      margin-right: 0.5rem;

      input {
        width: 100%;
      }

      select {
        width: 100%;
        text-align: center;
      }
    }

    .search_combo_input {
      margin-left: 0.5rem;
      margin-right: 0.5rem;
    }
  }

  .pickup_content_box {
    width: 75%;

    display: flex;
    flex-direction: column;
  }

  .pickup_content {
    width: 100%;
    height: 100%;
    background-color: antiquewhite;
    flex-grow: 1;
  }
}
</style>
