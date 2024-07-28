<template>
  <v-container fluid>
    
    <!-- Секция таблицы -->
    <v-card v-if="!isLoading" class="mb-4">
      <v-data-table
        :headers="headers"
        :items="stocks"
        :page.sync="page"
        :items-per-page="itemsPerPage"
        @update:page="updatePage"
        @update:items-per-page="updateItemsPerPage"
        class="elevation-1 bordered-table"
      >
        <template v-slot:item="{ item }">
          <tr 
          :class="{ 'selected-row': selectedStock.stockId === item.stockId }" 
          @click="handleRowClick(item)">
            <td 
            @dblclick="navigateStockId(item)" 
            class="navigation-column ">
              {{ item.stockId}}
            </td>
            <td>{{ item.name }}</td>
            <td
            v-if="policyA.includes(getEmployeeInfo.Role)"
            >{{ item.companyName }}</td>
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
                      <v-col cols="4">
                        <v-text-field
                          label="ID склада"
                          v-model="filters.stockId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="8">
                        <v-text-field
                          label="Название склада"
                          v-model="filters.name"
                          prepend-icon="mdi-package-variant-closed"
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
    <v-card>
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
                @click="selectedStock = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveStock"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteStock"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
      <v-card-text>
          <div v-if="selectedStock.stockId !== undefined || isEditing">
            
            <v-row>
                <v-col v-if="policyA.includes(getEmployeeInfo.Role)" cols="7">
                  <v-card color="grey-lighten-4">
                    <v-card-title>
                      Поля
                    </v-card-title>
                    <v-card-subtitle>
                      Заполните поля данными
                    </v-card-subtitle>
                    <v-card-text>
                      <v-row>
                        <v-col cols="4">
                          <v-text-field
                            v-model="selectedStock.stockId"
                            label="ID склада"
                            type="number"
                            prepend-icon="mdi-identifier"
                          ></v-text-field>
                        </v-col>
                        <v-col cols="8">
                          <v-text-field
                            v-model="selectedStock.name"
                            label="Название"
                            prepend-icon="mdi-package-variant-closed"
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
                          v-model="selectedStock.name"
                          label="Название*"
                          prepend-icon="mdi-package-variant-closed"
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
      baseHeaders: [
        { title: 'ID склада*', key: 'stockId', align: 'start', sortable: true },
        { title: 'Название', key: 'name', align: 'start', sortable: true },
      ],
      stocks: [],
      selectedStock: {},
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
    navigateStockId(item) {
      this.filters = {stockId: item.stockId}
      this.isEditing = true
      this.selectedStock = item
      this.applyFilters();
      this.filters = {}
    },
    switchEditingMode() {
        this.isEditing = !this.isEditing
        if (!this.isEditing) {
          this.selectedStock.stockId = undefined
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
      const url = '/api/Stock/getStocksFiltered';

      const data = {}

      if (this.filters.stockId)
        data.stockId = this.filters.stockId
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
            this.stocks = Array.from(response.data.result);
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
    async saveStock() {
      const data = {};

        if(this.selectedStock.stockId)
          data.StockId = this.selectedStock.stockId
        if(this.selectedStock.name)
          data.Name = this.selectedStock.name
        if(this.selectedStock.companyId)
          data.CompanyId = this.selectedStock.companyId


      try {
        if (this.selectedStock.stockId !== undefined || this.isEditing) {
          await api.put('api/Stock/update', data, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        } else {
          await api.post('api/Stock/Create', data, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        } 
        
        this.applyFilters();
      } catch (error) {
        this.$store.commit('setErrorMessage', error)
      }
    },
    async deleteStock() {
      api.delete(`api/Stock/dellById?stockId=${this.selectedStock.stockId}`)
      .then(() => this.applyFilters())
      .catch(error => this.$store.commit('setErrorMessage', error))
    },
    async handleRowClick(item) {
      if (this.selectedStock.stockId === item.stockId) {
        delete this.selectedStock.stockId
        this.isEditing = false
      } else {
        this.selectedStock = {...item};
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
      const company = this.companies.find(c => c?.companyId === this.selectedStock.companyId);
      return company ? company.companyId : null;
    },
    set(value) {
      this.selectedStock.companyId = value;
    }
  },
  filtersCompanyId: {
    get() {
      const company = this.companies.find(c => c?.companyId === this.filters.companyId);
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
      policyA() {
        return this.$store.state.policyA
      },
      headers() {
      if (this.policyA.includes(this.getEmployeeInfo.Role)) {
        this.baseHeaders.splice(2, 0, { 
            title: 'Компания', 
            key: 'companyName', 
            align: 'start', 
            sortable: true 
          });
      }
      return this.baseHeaders;
    }
}
}
</script>