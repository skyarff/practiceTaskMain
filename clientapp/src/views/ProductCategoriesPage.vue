<template>
  <v-container fluid>
    
    <!-- Секция таблицы -->
    <v-card v-if="!isLoading" class="mb-4">
      <v-data-table
        :headers="headers"
        :items="productCategories"
        :page.sync="page"
        :items-per-page="itemsPerPage"
        @update:page="updatePage"
        @update:items-per-page="updateItemsPerPage"
        class="elevation-1 bordered-table"
      >
        <template v-slot:item="{ item }">
          <tr 
          :class="{ 'selected-row': selectedProductCategory.productCategoryId === item.productCategoryId }" 
          @click="handleRowClick(item)">
            <td 
            @dblclick="navigateProductCategoryId(item)" 
            class="navigation-column ">
              {{ item.productCategoryId }}
            </td>
            <td>{{ item.name }}</td>
            <td>{{ getCompanyName(item.companyId) }}</td>
          </tr>
        </template>
      </v-data-table>
    </v-card>

    <Loader v-else />

    <!-- Секция фильтров -->
    <v-expansion-panels class="mb-4">
      <v-expansion-panel>
        <v-expansion-panel-title>
          <v-icon start icon="mdi-filter"></v-icon>
          Фильтры
        </v-expansion-panel-title>
        <v-expansion-panel-text class="pt-6">
          <v-row>
            <v-col cols="12" sm="6" md="4">
              <v-text-field
                label="ID категории продуктов"
                v-model="filters.productCategoryId"
                type="number"
                prepend-icon="mdi-identifier"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field
                label="Название категории продуктов"
                v-model="filters.name"
                prepend-icon="mdi-tag-multiple"
              ></v-text-field>
            </v-col>
                  <v-col cols="4">
                    <v-select
                      v-model="filtersCompanyId"
                      :items="companies"
                      item-title="name"
                      item-value="companyId"
                      label="Компания"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>
                  </v-col>
            <v-col cols="12">
              <v-btn class="mr-4" color="primary" @click="applyFilters" prepend-icon="mdi-magnify">
                Применить фильтры
              </v-btn>
              <v-btn color="secondary" @click="resetFilters" prepend-icon="mdi-eraser">
                Очистить фильтры
              </v-btn>
            </v-col>
          </v-row>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>

    <!-- Секция редактирования -->
    <v-card >
      <v-card-text>
        <v-row align="center" no-gutters>
            <v-col class="mr-4" cols="auto">
              <v-switch
                :model-value="isEditing"
                color="primary"
                label="Удаление"
                @click="switchEditingMode"
                hide-details
              ></v-switch>
            </v-col>
            <v-col cols="auto">
              <v-btn
                icon
                elevation="0"
                color="grey"
                variant="text"
                size="x-large"
                @click="selectedProductCategory = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
          </v-row>
      </v-card-text>
        <div v-if="selectedProductCategory.productCategoryId !== undefined || isEditing">
          <v-card-text>
            <v-form @submit.prevent="saveProductCategory">
                <v-card-title>
                    Удаление
                </v-card-title>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="selectedProductCategory.productCategoryId"
                    label="ID склада"
                    type="number"
                    prepend-icon="mdi-identifier"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-btn @click="deleteStock" color="secondary" prepend-icon="mdi-delete">
                    Удалить
                  </v-btn>
                </v-col>
              </v-row>
            </v-form>
          </v-card-text>
        </div>
        <div v-else>
          <v-card-title>Добавление</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="saveProductCategory">
                <v-row>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedProductCategory.name"
                      label="Название*"
                      prepend-icon="mdi-tag-multiple"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6">
                    <v-select
                      v-model="selectedCompanyId"
                      :items="companies"
                      item-title="name"
                      item-value="companyId"
                      label="Компания"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>
                </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-btn type="submit" color="primary" prepend-icon="mdi-plus-circle">
                      Добавить
                    </v-btn>
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
        </div>
    </v-card>

  </v-container>
</template>



<script>
import api from '@/api';
import Loader from '@/components/TableLoader.vue'
import { mapGetters } from 'vuex';

export default {
  components: {
    Loader
  },
  data() {
    return {
      apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
      filters: {},
      headers: [
        { title: 'ID категории продуктов*', key: 'productCategoryId', align: 'start', sortable: true },
        { title: 'Название', key: 'name', align: 'start', sortable: true },
        { title: 'Компания', key: 'companyName', align: 'start', sortable: true },
      ],
      productCategories: [],
      selectedProductCategory: {},
      isEditing: false,
      isLoading: true,
      page: 1,
      itemsPerPage: 10,
    }
  },
  activated() {
    this.applyFilters();
    this.$store.dispatch('getAllCompanies');
  },
  methods: {
    updatePage(newPage) {
      this.page = newPage;
    },
    updateItemsPerPage(itemsPerPage) {
      this.itemsPerPage = itemsPerPage;
    },
    navigateProductCategoryId(item) {
      this.filters = {productCategoryId: item.productCategoryId}
      this.isEditing = true
      this.selectedProductCategory = item
      this.applyFilters();
    },
    switchEditingMode() {
        this.isEditing = !this.isEditing
        if (!this.isEditing) {
          this.selectedProductCategory.productCategoryId = undefined
        }
    },
    async applyFilters() {
      this.isLoading = true;
      const url = '/api/ProductCategory/getProductCategoriesFiltered';

      try {
        const response = await api.post(url, this.filters, {
          headers: {
            'accept': '*/*',
            'Content-Type': 'application/json'
          }
        });

        this.productCategories = Array.from(response.data.result);
      } catch (error) {
        this.$store.commit('setErrorMessage', 'Записи, соответствующие заданным фильтрам, отсутствуют.')
      } finally {
        this.isLoading = false;
      }
    },
    resetFilters() {
      this.filters = {}
      this.applyFilters();
    },
    async saveProductCategory() {
      const data = {};

        if(this.selectedProductCategory.productCategoryId)
          data.ProductCategoryId = this.selectedProductCategory.productCategoryId
        if(this.selectedProductCategory.name)
          data.Name = this.selectedProductCategory.name
        if(this.selectedProductCategory.companyId)
          data.CompanyId = this.selectedProductCategory.companyId


      try {
         await api.post('/api/ProductCategory/create', data, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        
        this.applyFilters();
      } catch (error) {
        this.$store.commit('setErrorMessage', error)
      }
    },
    async deleteStock() {
      try {
        await api.delete(`api/ProductCategory/dellById?productCategoryId=${this.selectedProductCategory.productCategoryId}`);
        
        this.applyFilters();
      } catch (error) {
        this.$store.commit('setErrorMessage', error)
      }
    },
    async handleRowClick(item) {
      if (this.selectedProductCategory.productCategoryId === item.productCategoryId) {
        delete this.selectedProductCategory.productCategoryId
        this.isEditing = false
      } else {
        this.selectedProductCategory = {...item};
        this.isEditing = true
      }
    },
    getCompanyName(companyId) {
      const company = this.companies.find(c => c.companyId === companyId);
      return company ? company.name : 'Не указано';
    },
},
computed: {
  selectedCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.selectedProductCategory.companyId);
      return company ? company.companyId : null;
    },
    set(value) {
      this.selectedProductCategory.companyId = value;
    }
  },
  filtersCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.filters.companyId);
      return company ? company.companyId : null;
    },
    set(value) {
      this.filters.companyId = value;
    }
  },
  ...mapGetters(['companies'])
}
}
</script>


<style scoped>
.selected-row {
outline: 2px solid rgba(130, 184, 179, 0.81);
outline-offset: -2px;
border-radius: 0%;
}
.navigation-column {
background-color: rgba(234, 234, 234, 0.21);
}
.bordered-table :deep() td {
border-right: 1px solid rgba(222, 222, 222, 0.22);
}

</style>