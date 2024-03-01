<template>
  <div class="backplate" :style="backplateConfig">
    <div class="modalplate" :style="modalplateConfig">
      <div class="modal_header">
        <slot name="header"></slot>
        <div class="modal_header_close" @click="close"><b>X</b></div>
      </div>
      <div class="modal_content">
        <slot name="content"></slot>
      </div>
      <div class="modal_footer">
        <slot name="footer"></slot>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    zIndex: {
      type: Number,
      default: 10,
    },
    width: {
      type: Number,
      default: 50,
    },
    height: {
      type: Number,
      default: 80,
    },
  },
  data() {
    return {};
  },
  computed: {
    backplateConfig() {
      return {
        "z-index": this.zIndex,
      };
    },
    modalplateConfig() {
      return {
        "z-index": this.zIndex + 1,
        width: `${this.width}vw`,
        height: `${this.height}vh`,
      };
    },
  },
  methods: {
    close() {
      this.$emit("close");
    },
  },
};
</script>

<style lang="scss">
.backplate {
  width: 100vw;
  height: 100vh;
  top: 0;
  left: 0;
  position: absolute;
  background-color: rgba(0, 0, 0, 0.356);
}

.modalplate {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
  flex-flow: column;
  border: 0.25rem brown solid;
  background-color: whitesmoke;
  border-radius: 2rem;
  padding: 0;
  margin: 0;
  overflow: hidden;
  box-shadow: 1rem 1rem 2rem rgba(0, 0, 0, 0.5);

  .modal_header {
    flex: 0 1 auto;
    height: 2.4rem;
    padding: 0;
    margin: 0;
    padding-left: 1.4rem;
    padding-top: 0.3rem;
    display: flex;
    align-items: center;
    text-align: left;
    background-color: rgb(189, 81, 62);
    border-bottom: 0.2rem rgba(255, 255, 255, 0.493) dashed;

    .modal_header_close {
      float: right;
      margin-top: 0.25rem;
      margin-right: 1rem;
      margin-left: auto;
      margin-bottom: auto;
      padding-bottom: 1.25rem;
      font-size: 1rem;
      width: 1.1rem;
      height: 1.1rem;
      border-radius: 0.3rem;
      text-align: center;
      justify-content: center;
      background-color: gray;
      cursor: pointer;
    }
  }

  .modal_content {
    flex: 1 1 auto;
    width: 100%;
    height: 100%;
    max-height: 100%;
    overflow-y: auto;
    overflow-x: hidden;
    padding: 0;
    margin: 0;
    border-top: 1px black solid;
    border-bottom: 1px black solid;
  }

  .modal_footer {
    flex: 0 1 2.4rem;
    width: 100%;
    margin: 0;
    padding: 0;
    padding-left: 1.4rem;
    display: flex;
    text-align: left;
    align-items: center;
    background-color: rgb(189, 81, 62);
    border-top: 0.2rem rgba(255, 255, 255, 0.493) dashed;
  }
}
</style>
