<template>
  <div id="app">
    <div id="app_contener"><router-view /></div>
  </div>
</template>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  align-content: center;

  background-color: whitesmoke;
  width: 100vw;
  height: 100vh;
  margin: 0;
  padding: 0;
}

#app_contener {
  align-self: center;
  width: 70%;
  height: 100%;
  margin-left: auto;
  margin-right: auto;
  margin-top: 0;
}

//------------------------------------------------------------- loader

.loader {
  width: 48px;
  height: 48px;
  border: 3px dotted #2a98b9;
  border-style: solid solid dotted dotted;
  border-radius: 50%;
  display: inline-block;
  position: relative;
  box-sizing: border-box;
  animation: rotation 2s linear infinite;
}

.loader::after {
  content: "";
  box-sizing: border-box;
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  margin: auto;
  border: 3px dotted #ff3d00;
  border-style: solid solid dotted;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  animation: rotationBack 1s linear infinite;
  transform-origin: center center;
}

@keyframes rotation {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
@keyframes rotationBack {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(-360deg);
  }
}

.loader_modal_fix {
  align-content: center;
  align-items: center;
  justify-content: center;
  margin: 0;
  display: flex;
  height: 100%;
}

//------------------------------------------------------------- table
.tab {
  border: 2px black solid;
  margin-top: 0.5rem;
  margin-left: 1rem;
  margin-right: 1rem;

  .header_tab {
    background-color: pink;
    margin: 0;
    padding: 0;

    .simple_header {
      overflow: hidden;
      text-overflow: ellipsis;
    }
  }

  .row_tab {
    border-top: 1px black solid;
    margin: 0;
    padding-right: 0;
    padding-left: 0;
  }

  .row_tab_click {
    cursor: pointer;
  }

  .row_tab_center {
    display: flex;
    align-items: center;
    justify-content: center;
  }

  .row_color_1 {
    background-color: rgb(226, 221, 141);
  }

  .row_color_2 {
    background-color: rgb(171, 221, 151);
  }

  .active {
    border: 2px rgba(255, 0, 0, 0.404) dashed;
  }

  .tab_infoBtn {
    border-radius: 1rem;
    background-color: rgb(49, 187, 233);
    border: 1px solid black;
    cursor: pointer;
    padding: 0.1rem;

    &:hover {
      background-color: lightgreen;
      border: 1px solid yellowgreen;
    }
  }

  .tab_removeBtn {
    border-radius: 1rem;
    background-color: grey;
    border: 1px solid black;
    cursor: pointer;
    padding: 0.1rem;

    &:hover {
      background-color: yellow;
      border: 1px solid red;
    }
  }
}

//------------------------------------------------------------- modal

.modalplate {
  .save {
    border-radius: 1rem;
    padding-left: 0.5rem;
    padding-right: 0.5rem;
    margin: 0.2rem;
    margin-right: 1rem;

    background-color: silver;

    border: 4px #525252 dotted;
    box-shadow: 0 0 0 1px rgb(150, 150, 150);
  }

  .save:hover {
    border: 4px #525252 dashed;
    box-shadow: 0 0 0 2px rgb(150, 150, 150);
  }

  .save_box {
    float: left;
    display: flex;
    align-items: right;
    justify-content: right;
    width: 100%;
  }

  .modal_tab_add {
    width: 100%;
    padding-left: 1rem;
    padding-right: 1rem;

    button {
      width: 100%;
      border-radius: 1rem;
      border: 2px black dashed;
      background-color: lightgray;

      &:hover {
        border: 2px black solid;
      }
    }
  }

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
      cursor: pointer;
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

    .name_value_list {
      background-color: transparent;
      text-align: right;
      justify-content: center;
      justify-items: center;
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

    .box_value_chackbox {
      margin-left: 0;
      display: flex;
      flex-direction: column;
      align-items: baseline;

      input {
        margin-right: 0.25rem;
      }
    }
  }
}
</style>
