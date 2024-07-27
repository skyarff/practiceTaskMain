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
            <td>{{ item.companyName }}</td>
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
            <v-row @click.stop justify="end" align="center" class="ml-auto mr-5">
              <v-col cols="auto">
                <v-btn
                  class="mr-3"
                  color="primary"
                  @click="applyFilters"
                  icon="mdi-magnify"
                  size="small"
                  rounded="circle"
                ></v-btn>
                <v-btn
                  color="secondary"
                  @click="resetFilters"
                  icon="mdi-eraser"
                  size="small"
                  rounded="circle"
                ></v-btn>
              </v-col>
            </v-row>
          </v-expansion-panel-title>
        <v-expansion-panel-text class="pt-2">

          <v-row>
              <v-col 
              :cols="`${policyA.includes(getEmployeeInfo.Role) ? 7 : 12}`"
              >
                <v-card color="grey-lighten-4">
                  <v-card-title>
                    Поля
                  </v-card-title>
                  <v-card-subtitle>
                    Заполните поля данными
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
                      <v-col cols="6">
                        <v-text-field
                          label="ID категории продуктов"
                          v-model="filters.productCategoryId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="6">
                        <v-text-field
                          label="Название категории продуктов"
                          v-model="filters.name"
                          prepend-icon="mdi-tag-multiple"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                  </v-card-text>
                </v-card>
              </v-col>

              <v-col v-if="policyA.includes(getEmployeeInfo.Role)" cols="5">
                <v-card color="teal-lighten-5">
                  <v-card-title>
                    Иерархические сущности
                  </v-card-title>
                  <v-card-subtitle>
                    Выберите один из предложенных вариантов
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
                      <v-col cols="12">
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
                    </v-row>
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>

    <!-- Секция редактирования -->
    <v-card v-if="policyCA.includes(getEmployeeInfo.Role)">
      <v-card-title>
          <v-row align="center">
            <v-col cols="auto">
              <v-switch
                :model-value="isEditing"
                color="primary"
                label="Редактирование"
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
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveProductCategory"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteProductCategory"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
      </v-card-title>
      <v-card-text>
        <div v-if="selectedProductCategory.productCategoryId !== undefined || isEditing">

          <v-row>
              <v-col cols="12">
                <v-card color="grey-lighten-4">
                  <v-card-title>
                    Удаление
                  </v-card-title>
                  <v-card-subtitle>
                    Укажите Id удаляемой категории продуктов
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
                      <v-col cols="12">
                        <v-text-field
                          v-model="selectedProductCategory.productCategoryId"
                          label="ID склада"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>
                    </v-row>
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>
    </div>
    <div v-else>

    <v-row>
        <v-col 
        :cols="`${policyA.includes(getEmployeeInfo.Role) ? 7 : 12}`"
        >
          <v-card color="grey-lighten-4">
            <v-card-title>
              Поля
            </v-card-title>
            <v-card-subtitle>
              Заполните поля данными
            </v-card-subtitle>
            <v-card-text>
              <v-row>
                <v-col cols="12">
              <v-text-field
                v-model="selectedProductCategory.name"
                label="Название*"
                prepend-icon="mdi-tag-multiple"
              ></v-text-field>
            </v-col>
              </v-row>
            </v-card-text>
          </v-card>
        </v-col>

        <v-col v-if="policyA.includes(getEmployeeInfo.Role)" cols="5">
          <v-card color="teal-lighten-5">
            <v-card-title>
              Иерархические сущности
            </v-card-title>
            <v-card-subtitle>
              Выберите один из предложенных вариантов
            </v-card-subtitle>
            <v-card-text>
              <v-row>
                <v-col cols="12">
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
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </div>
      </v-card-text>
    </v-card>

  </v-container>
</template>



<script>
import api from '@/api';
import Loader from '@/components/TableLoader.vue'
import { mapGetters } from 'vuex';
import '@/assets/main.css';

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
      abortFlag: false,
      timeoutId: null
    }
  },
  activated() {
    this.abortFlag = false
    this.checkConnection();

    if (this.policyA.includes(this.getEmployeeInfo.Role))
      this.$store.dispatch('getAllCompanies');
  },
  deactivated() {
      this.abortFlag = true
      clearTimeout(this.timeoutId)
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
      this.filters = {}
      window.scrollTo(0, document.body.scrollHeight);
    },
    switchEditingMode() {
        this.isEditing = !this.isEditing
        if (!this.isEditing) {
          this.selectedProductCategory.productCategoryId = undefined
        }
    },
    async checkConnection() {
        let flag = true;
        while (flag && !this.abortFlag) {
          try {
            await this.applyFilters();
            flag = false;
          } catch {
            await new Promise(resolve => {
              this.timeoutId = setTimeout(resolve, 30000)
            });
          }
        }
      },
    async applyFilters() {
      this.isLoading = true;
      const url = '/api/ProductCategory/getProductCategoriesFiltered';

      const data = {}

      if (this.filters.productCategoryId)
        data.productCategoryId = this.filters.productCategoryId
      if (this.filters.name)
        data.name = this.filters.name
      if (this.filters.name)
        data.name = this.filters.name
      if (this.filters.companyId)
        data.companyId = this.filters.companyId

      return new Promise((resolve, reject) => {
          api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          })
          .then(response => {
            this.productCategories = Array.from(response.data.result);
            resolve();
          })
          .catch(error => {
            this.$store.commit('setErrorMessage', error);
            reject();
          })
          .finally(() => {
            this.isLoading = false;
          });
        });
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
    async deleteProductCategory() {

      api.delete(`api/ProductCategory/dellById?productCategoryId=${this.selectedProductCategory.productCategoryId}`)
      .then(() => this.applyFilters())
      .catch(() => this.$store.commit('setErrorMessage', error))

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
  ...mapGetters(['companies']),
  getEmployeeInfo() {
        return this.$store.state.employeeInfo
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

