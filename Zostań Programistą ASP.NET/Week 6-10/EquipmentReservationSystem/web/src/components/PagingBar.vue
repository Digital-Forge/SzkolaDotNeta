<template><div></div></template>

<script>
export default {
  props: {
    page: {
      default: 1,
      required: false,
      type: Number,
    },
    total: {
      default: 1,
      required: true,
      type: Number,
    },
    show: {
      default: 30,
      required: false,
      type: Number,
    },
  },
  data() {
    return {
      start: [],
      middle: [],
      stop: [],
    };
  },
};
</script>
