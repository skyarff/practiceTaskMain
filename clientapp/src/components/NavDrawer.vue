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
          :title="`${getEmployeeInfo.CompanyName ? 'Компания ' + getEmployeeInfo.CompanyName : ''}`"
          :subtitle="`${getEmployeeInfo.StockName ? 'Склад ' + getEmployeeInfo.StockName : ''}`"
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
          <v-list-item v-if="policyA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-domain" title="Компании" value="companies" to="/CompaniesPage"></v-list-item>
          <v-list-item v-if="policyCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-package-variant-closed" title="Склады" value="stocks" to="/StocksPage"></v-list-item>
          <v-list-item v-if="policySCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-tag-multiple" title="Категории продуктов" value="product categories" to="/ProductCategoriesPage"></v-list-item>
          <v-list-item v-if="policyCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-account-tie" title="Сотрудники" value="employees" to="/EmployeesPage"></v-list-item>
          <v-list-item v-if="policySCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-locker-multiple" title="Стеллажи" value="storage locations" to="/StorageLocationsPage"></v-list-item>
          <v-list-item v-if="policySCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-shape-outline" title="Продукты" value="products" to="/ProductsPage"></v-list-item>
          <v-list-item v-if="policySCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-file-document-outline" title="УПД" value="upds" to="/UpdsPage"></v-list-item>
          <v-list-item v-if="policyCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-receipt" title="Счета" value="bills" to="/BillsPage"></v-list-item>
          <v-list-item v-if="policyCA.includes(getEmployeeInfo.Role)" prepend-icon="mdi-truck-delivery" title="Поставщики" value="providers" to="/ProvidersPage"></v-list-item>
          <v-list-item prepend-icon="mdi-vector-polygon" title="Схема" value="Schema" to="/SchemaPage"></v-list-item>
        </v-list>

        <v-card v-if="!rail && getEmployeeInfo.Role" class="mx-3">
          <v-card-title>
            <div>
              <div v-if="getEmployeeInfo.ImagePath">
                    <v-img 
                    :src="`${apiBaseUrl}//${getEmployeeInfo.ImagePath}`"
                    class="full-size-image"
                    style="max-width: 85px; max-height: 85px"
                  ></v-img>
                  </div>
                  <div v-else>
                    <v-img 
                      :src="`${apiBaseUrl}//Images//Common//NoAvatar.jpg`"
                      class="full-size-image"
                      style="max-width: 85px; max-height: 85px"
                  ></v-img>
                  </div>
            </div>
            
            <div>Логин: {{ getEmployeeInfo.Login }}</div>
            <div>Роль: {{ getEmployeeInfo.Role }}</div>
          </v-card-title>
          <v-card-subtitle>
            <div>ФИО: {{ getEmployeeInfo.FullName }}</div>
            <div>Должность: {{ getEmployeeInfo.JobTitle }}</div>
            <div>Почта: {{ getEmployeeInfo.Email }}</div>
            <div>Телефон: {{ getEmployeeInfo.Phone }}</div>
          </v-card-subtitle>
          <v-card-text>    
            

          </v-card-text>
        </v-card>

      </v-navigation-drawer>
</template>

<script>
  export default {
    data () {
      return {
        drawer: true,
        rail: true,
        apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        policySCA: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin'],
        policyCA: ['CompanyLevelWorker', 'Admin'],
        policyA: ['Admin']
      }
    },
    computed: {
      getEmployeeInfo() {
        if (this.$store.state.employeeInfo) 
          return this.$store.state.employeeInfo
        else return {}
      },
      policySCA() {
        return this.$store.state.policySCA
      },
      policyCA() {
        return this.$store.state.policyCA
      },
      policyA() {
        return this.$store.state.policyA
      }
    }
  }
</script>

<style scoped>
.fixed-list-item {
  height: 64px;
}


</style>