<template>
  <div id="home_area">
    <NavBar :view-type="'home'"></NavBar>
    <div class="home_container">
      <div class="home_content_box">
        <div class="home_nav">
          <div
            class="home_nav_element"
            :class="[page == 'my' ? 'home_nav_element_active' : '']"
            @click="page = 'my'"
          >
            My item
          </div>
          <div
            class="home_nav_element"
            :class="[page == 'history' ? 'home_nav_element_active' : '']"
            @click="page = 'history'"
          >
            History
          </div>
          <div
            class="home_nav_element"
            :class="[page == 'reservation' ? 'home_nav_element_active' : '']"
            @click="page = 'reservation'"
          >
            Reservation
          </div>
        </div>
        <div class="home_content">
          <MyItems v-if="page == 'my'" :searchData="searchOptions"></MyItems>
          <HistoryItems
            v-if="page == 'history'"
            :searchData="searchOptions"
          ></HistoryItems>
          <ItemToReservation
            v-if="page == 'reservation'"
            :searchData="searchOptions"
          ></ItemToReservation>
        </div>
      </div>
      <div class="search_box">
        <div class="search_placeholder">
          <span>Search</span>
        </div>
        <div class="search_text_input">
          <input type="text" v-model="searchOptions.searchName" />
        </div>
        <div class="search_placeholder">
          <span>Departments</span>
        </div>
        <div class="search_department_input">
          <ComboBoxApi
            multiple
            :api="'Department/GetUserDepartments'"
            :placeholder="'Select departments'"
            v-model:value="searchOptions.selectedDepartments"
          ></ComboBoxApi>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import NavBar from "@/components/NavBar.vue";
import MyItems from "@/components/home/MyItems.vue";
import HistoryItems from "@/components/home/HistoryItems.vue";
import ItemToReservation from "@/components/home/ItemToReservation.vue";
import ComboBoxApi from "@/components/ComboBoxApi.vue";

export default {
  name: "HomeView",
  components: {
    NavBar,
    MyItems,
    ComboBoxApi,
    HistoryItems,
    ItemToReservation,
  },
  data() {
    return {
      page: "my",
      searchOptions: {
        selectedDepartments: null,
        searchName: null,
      },
    };
  },
  methods: {},
};
</script>

<style lang="scss">
#home_area {
  height: 100vh;
  display: flex;
  flex-direction: column;

  .home_container {
    display: flex;
    width: 100%;
    height: max-content;
    flex-grow: 1;
  }

  .home_nav {
    width: 100%;
    display: flex;
    padding: 0;
    margin: 0;
  }

  .home_nav_element {
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
    }

    .search_department_input {
      margin-left: 0.5rem;
      margin-right: 0.5rem;
    }
  }

  .home_content_box {
    width: 75%;

    display: flex;
    flex-direction: column;
  }

  .home_content {
    width: 100%;
    height: 100%;
    background-color: antiquewhite;
    flex-grow: 1;
  }
}
</style>
