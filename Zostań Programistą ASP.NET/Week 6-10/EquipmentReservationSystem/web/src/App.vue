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
</style>
