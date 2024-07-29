<template>
  <v-app>
    <NavBar />
    <NavDrawer />
    <v-main>
        <router-view class="pa-4" v-slot="{ Component }">
            <keep-alive>
              <component :is="Component" />
            </keep-alive>
        </router-view>
    </v-main>
    <ErrorOutput />
  </v-app>
</template>

<script>
import NavBar from '@/components/NavBar.vue'
import NavDrawer from '@/components/NavDrawer.vue'
import ErrorOutput from '@/components/ErrorOutput.vue'
import VueCookies from 'vue-cookies'

export default {
  name: "App",

  components: {
    NavBar,
    NavDrawer,
    ErrorOutput,
  },
  created() {
    this.$store.state.accessToken = VueCookies.get('accessToken');
    this.$store.state.refreshToken = VueCookies.get('refreshToken');
    this.$store.dispatch('getUserInfo')
  }
};
</script>
