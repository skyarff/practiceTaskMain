<template>
    <v-navigation-drawer
        v-model="drawer"
        :rail="rail"
        permanent
        app
        @click="rail = false"
      >
        <v-list-item
          :prepend-avatar="`${apiBaseUrl}//Images//Common//2.png`"
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
          <v-list-item prepend-icon="mdi-shape-outline" title="Продукты" value="products" to="/ProductsPage"></v-list-item>
          <v-list-item prepend-icon="mdi-file-document-outline" title="УПД" value="upds" to="/UpdsPage"></v-list-item>
          <v-list-item prepend-icon="mdi-receipt" title="Счета" value="bills" to="/BillsPage"></v-list-item>
          <v-list-item prepend-icon="mdi-truck-delivery" title="Поставщики" value="providers" to="/ProvidersPage"></v-list-item>
          <v-list-item prepend-icon="mdi-vector-polygon" title="Схема" value="Schrma" to="/SchemaPage"></v-list-item>


        </v-list>

      </v-navigation-drawer>
</template>

<script>
  export default {
    data () {
      return {
        drawer: true,
        rail: true,
        apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
      }
    },
  }
</script>

<style scoped>
.fixed-list-item {
  height: 64px;
}


</style>