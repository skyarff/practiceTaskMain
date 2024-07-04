<template>
  <v-container fluid>
    
    <!-- Секция таблицы -->
    <v-card v-if="!isLoading" class="mb-4">
      <v-data-table
        :headers="headers"
        :items="stocks"
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
            <td>{{ item.companyId }}</td>
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
                label="ID склада"
                v-model="filters.stockId"
                type="number"
                prepend-icon="mdi-identifier"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field
                label="Название склада"
                v-model="filters.name"
                prepend-icon="mdi-package-variant-closed"
              ></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field
                label="ID компании"
                v-model="filters.companyId"
                type="number"
                prepend-icon="mdi-identifier"
              ></v-text-field>
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
        <v-switch
          :model-value="isEditing"
          color="primary"
          label="Редактирование"
          @click="switchEditingMode"
        ></v-switch>
      </v-card-text>
        <div v-if="selectedStock.stockId !== undefined || isEditing">
          <v-card-title>Редактирование/удаление</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="saveStock">
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="selectedStock.stockId"
                    label="ID склада"
                    type="number"
                    prepend-icon="mdi-identifier"
                  ></v-text-field>
                </v-col>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="selectedStock.name"
                    label="Название"
                    prepend-icon="mdi-package-variant-closed"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12" sm="6">
                  <v-text-field
                    v-model="selectedStock.companyId"
                    label="ID компании"
                    type="number"
                    prepend-icon="mdi-domain"
                  ></v-text-field>
                </v-col>
              </v-row>
              <v-row>
                <v-col>
                  <v-btn class="mr-4" type="submit" color="primary" prepend-icon="mdi-content-save">
                    Редактировать
                  </v-btn>
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
              <v-form @submit.prevent="saveStock">
                <v-row>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedStock.name"
                      label="Название"
                      prepend-icon="mdi-package-variant-closed"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedStock.companyId"
                      label="ID компании"
                      type="number"
                      prepend-icon="mdi-identifier"
                    ></v-text-field>
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

export default {
  components: {
    Loader
  },
  data() {
    return {
      apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
      filters: {},
      headers: [
        { title: 'ID склада*', key: 'stockId', align: 'start', sortable: true },
        { title: 'Название', key: 'name', align: 'start', sortable: true },
        { title: 'ID компании*', key: 'companyId', align: 'start', sortable: true },
      ],
      stocks: [],
      selectedStock: {},
      isEditing: false,
      isLoading: true
    }
  },
  mounted() {
    this.applyFilters();
  },
  methods: {
    navigateStockId(item) {
      this.filters = {stockId: item.stockId}
      this.isEditing = true
      this.selectedStock = item
      this.applyFilters();
    },
    navigateCompanyId(item) {
      // this.filters = {companyId: item.companyId}
      // this.isEditing = true
      // this.selectedCompany = item
      // this.applyFilters();
    },
    switchEditingMode() {
        this.isEditing = !this.isEditing
        if (!this.isEditing) {
          this.selectedStock.stockId = undefined
        }
    },
    async applyFilters() {
      this.isLoading = true;
      const url = '/api/Stock/getStocksFiltered';
      const data = {}
      
        if(this.filters.stockId)
          data.stockId = this.filters.stockId
        if(this.filters.name)
          data.name = this.filters.name
        if(this.filters.companyId)
          data.companyId = this.filters.companyId

      try {
        const response = await api.post(url, data, {
          headers: {
            'accept': '*/*',
            'Content-Type': 'application/json'
          }
        });
        this.stocks = Array.from(response.data.result);
      } catch (error) {
        console.error('Ошибка при выполнении запроса:', error);
      } finally {
        this.isLoading = false;
      }
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
        let response;
        if (this.selectedStock.stockId !== undefined || this.isEditing) {
          response = await api.put('api/Stock/update', data, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        } else {
          response = await api.post('api/Stock/Create', data, {
            headers: {
              'Content-Type': 'application/json'
            }
          });
        } 
        
        this.applyFilters();
      } catch (error) {
        console.error('Ошибка при сохранении компании:', error);
      }
    },
    async deleteStock() {
      try {
        await api.delete(`api/Stock/dellById?stockId=${this.selectedStock.stockId}`);
        
        this.applyFilters();
      } catch (error) {
        console.error('Ошибка при удалении компании:', error);
      }
    },
    async handleRowClick(item) {
      
      if (this.selectedStock.stockId === item.stockId) {
        delete this.selectedStock.stockId
        this.isEditing = false
      } else {
        this.selectedStock = {...item};
        this.isEditing = true
      }
    }
},
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