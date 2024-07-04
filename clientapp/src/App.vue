<template>
  <v-app>

    <NavBar />

    <NavDrawer />

    <v-main>
      <router-view class="pa-4" />
    </v-main>


    <ErrorOutput />
    
  </v-app>


</template>

<script>
import NavBar from '@/components/NavBar.vue'
import NavDrawer from '@/components/NavDrawer.vue'
import ErrorOutput from '@/components/ErrorOutput.vue'

export default {
  name: "App",

  components: {
    NavBar,
    NavDrawer,
    ErrorOutput
  },
};
</script>
