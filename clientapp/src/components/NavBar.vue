<template>


    <v-app-bar
          color="lime-darken-3"
          :image="`${apiBaseUrl}//Images//Common//1.jpg`"
          height="45"
        >
          <template v-slot:image>
            <v-img
              gradient="to top right, rgba(19,84,122,.8), rgba(128,208,199,.8)"
            ></v-img>
          </template>
  
          <template v-slot:prepend>
            <v-app-bar-nav-icon></v-app-bar-nav-icon>
          </template>
  
          <v-app-bar-title>INDRIVE</v-app-bar-title>
  
          <v-spacer></v-spacer>
  
          

          <v-btn class="mr-5" icon to="/AuthPage">
            <v-icon>mdi-login</v-icon>
          </v-btn>
  
          <!-- <v-btn icon>
            <v-icon>mdi-heart</v-icon>
          </v-btn>
  
          <v-btn icon>
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn> -->


    </v-app-bar>
</template>

<script>
export default {
  data() {
    return {
      apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
    }
  }
}
</script>