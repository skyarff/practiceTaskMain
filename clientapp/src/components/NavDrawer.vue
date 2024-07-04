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
          title="Компания №1"
          subtitle="Склад №1"
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

          

          <v-list-item prepend-icon="mdi-domain" title="Компании" value="companies" to="/CompaniesPage"></v-list-item>
          <v-list-item prepend-icon="mdi-package-variant-closed" title="Склады" value="stocks" to="/StocksPage"></v-list-item>
          <v-list-item prepend-icon="mdi-tag-multiple" title="Категории продукты" value="product categories" to="/ProductCategoriesPage"></v-list-item>
          <v-list-item prepend-icon="mdi-locker-multiple" title="Места хранения" value="storage locations" to="/StorageLocationsPage"></v-list-item>
          <v-list-item prepend-icon="mdi-account-tie" title="Сотрудники" value="employees" to="/EmployeesPage"></v-list-item>

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