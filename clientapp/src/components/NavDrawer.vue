<template>
    <v-navigation-drawer
        v-model="drawer"
        :rail="rail"
        permanent
        app
        @click="rail = false"
      >
        <v-list-item
          prepend-avatar="https://picsum.photos/1920/1080?random"
          title="Company №1"
          subtitle="Stock №1"
          nav
          class="fixed-list-item"
        >
          <template v-slot:append>
            <v-btn
              icon="mdi-chevron-left"
              variant="text"
              @click.stop="rail = !rail"
            ></v-btn>
          </template>
        </v-list-item>



        <v-divider></v-divider>

        <v-list density="compact" nav>

          

          <v-list-item prepend-icon="mdi-domain" title="Companies" value="Сompanies" to="/CompaniesPage"></v-list-item>
          <v-list-item prepend-icon="mdi-package-variant-closed" title="Stocks" value="stocks" to="/StocksPage"></v-list-item>
          <v-list-item prepend-icon="mdi-tag-multiple" title="Product categories" value="stocks" to="/ProductCategoryPage"></v-list-item>
          <v-list-item prepend-icon="mdi-locker-multiple" title="Storage locations" value="stocks" to="/StorageLocationPage"></v-list-item>

        </v-list>

      </v-navigation-drawer>
</template>

<script>
  export default {
    data () {
      return {
        drawer: true,
        rail: true,
      }
    },
  }
</script>

<style scoped>
.fixed-list-item {
  height: 64px;
}


</style>