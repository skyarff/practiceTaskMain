<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="storageLocations"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedStorageLocation.storageLocationId === item.storageLocationId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateStorageLocationId(item)" 
              class="navigation-column">
                {{ item.storageLocationId}}
              </td>
              <td>{{ item.rackCode }}</td>
              <td>{{ item.shelfCode }}</td>
              <td>{{ item.description }}</td>
              <td>
                  <div v-if="item.imagePath">
                    <v-img 
                    :src="`${apiBaseUrl}//${item.imagePath}`"
                    class="full-size-image"
                    style="max-width: 40px; max-height: 40px"
                  ></v-img>
                  <v-tooltip 
                    activator="parent" 
                    location="start"
                    content-class="image-tooltip"
                  >
                    <v-img
                      :src="`${apiBaseUrl}//${item.imagePath}`"
                      class="full-size-image"
                    ></v-img>
                  </v-tooltip>
                  </div>
                  <div v-else>
                    <v-icon>
                      mdi-image
                    </v-icon>
                  </div>
              </td>
              <td>{{ getStockName(item.stockId) }}</td>
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
                  label="ID места хранения"
                  v-model="filters.storageLocationId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Код стеллажа"
                  v-model="filters.rackCode"
                  prepend-icon="mdi-file-cabinet"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Код полки"
                  v-model="filters.shelfCode"
                  prepend-icon="mdi-bookshelf"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Описание"
                  v-model="filters.description"
                  prepend-icon="mdi-note-text"
                ></v-text-field>
              </v-col>

              <v-col cols="4">
                    <v-select
                      v-model="filtersCompanyId"
                      @update:modelValue="selectionOfStocks(false)"
                      :items="companies"
                      item-title="name"
                      item-value="companyId"
                      label="Компания"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>
                  </v-col>
                

                  <v-col cols="4">
                      <v-select
                        v-model="filtersStockId"
                        :items="fStocksByCompanyId"
                        item-title="name"
                        item-value="stockId"
                        label="Склад"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                  <v-col cols="4">
                    <v-select
                      v-model="filters.isBusy"
                      :items="filterOptions"
                      item-title="title"
                      item-value="value"
                      label="Места хранения"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>
                  </v-col>

            </v-row>
            <v-row>
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
                @click="selectedStorageLocation = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
          <div v-if="selectedStorageLocation.storageLocationId !== undefined || isEditing">
            <v-card-title>Редактирование/удаление</v-card-title>
              <v-card-text>
                <v-form @submit.prevent="saveStorageLocation">
                  <v-row>

                    <v-col cols="12">
                      <v-text-field
                        v-model="selectedStorageLocation.storageLocationId"
                        label="ID места хранения"
                        type="number"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="12">
                      <v-text-field
                        v-model="selectedStorageLocation.description"
                        label="Описание"
                        prepend-icon="mdi-note-text"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="12">
                      <v-file-input
                        v-model="selectedStorageLocation.image"
                        label="Фото места хранения"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col>
                      <v-btn class="mr-4" type="submit" color="primary" prepend-icon="mdi-content-save">
                        Редактировать
                      </v-btn>
                      <v-btn @click="deleteStorageLocation" color="secondary" prepend-icon="mdi-delete">
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
                <v-form @submit.prevent="saveStorageLocation">
                  <v-row>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="selectedStorageLocation.rackCode"
                        label="Код стеллажа*"
                        prepend-icon="mdi-file-cabinet"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="selectedStorageLocation.shelfCode"
                        label="Код полки*"
                        prepend-icon="mdi-bookshelf"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="6">
                      <v-text-field
                        v-model="selectedStorageLocation.description"
                        label="Описание"
                        prepend-icon="mdi-note-text"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="6" sm="6">
                      <v-file-input
                        v-model="selectedStorageLocation.image"
                        label="Фото места хранения"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>
                  </v-row>

                  <v-row>      
                    <v-col cols="4">
                      <v-select
                        v-model="selectedCompanyId"
                        :items="companies"
                        item-title="name"
                        item-value="companyId"
                        label="Компания"
                        prepend-icon="mdi-domain"
                        dense
                        @update:modelValue="selectionOfStocks(true)"
                      ></v-select>
                    </v-col>

   

                    <v-col cols="4">
                      <v-select
                        v-model="selectedStockId"
                        :items="stocksByCompanyId"
                        item-title="name"
                        item-value="stockId"
                        label="Склад"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                  </v-row>

                  <v-row>
                    <v-col>
                      <v-btn class="mr-4" type="submit" color="primary" prepend-icon="mdi-plus-circle">
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
        filters: {
        },
        filterOptions: [
          { title: 'Все места хранения', value: null },
          { title: 'Свободные', value: false },
          { title: 'Занятые', value: true }
        ],
        headers: [
          { title: 'ID места хранения*', key: 'storageLocationId', align: 'start', sortable: true },
          { title: 'Код стеллажа', key: 'rackCode', align: 'start', sortable: true },
          { title: 'Код полки', key: 'shelfCode', align: 'start', sortable: true },
          { title: 'Описание', key: 'description', align: 'start', sortable: true },
          { title: 'Фото места хранения', key: 'imagePath', align: 'start', sortable: false },
          { title: 'Склад', key: 'stockName', align: 'start', sortable: false },
          { title: 'Компания', key: 'companyName', align: 'start', sortable: false },
        ],
        storageLocations: [],
        selectedStorageLocation: {},
        isEditing: false,
        isLoading: true
      }
    },
    mounted() {
      this.applyFilters();
      this.$store.dispatch('getAllCompanies');
      this.$store.dispatch('getAllStocks');
    },
    methods: {
      selectionOfStocks(selected) {
        let companyId;
        if (selected) {
          companyId = this.selectedStorageLocation.companyId
          this.selectedStockId = null
        } else {
          companyId = this.filters.companyId
          this.filtersStockId = null
        }

        this.$store.dispatch('storageLocationPage/getStocksByCompanyId', {companyId: companyId, selected: selected})
      },
      async getAllCompanies() {
        const url = '/api/Company/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            this.companies = response.data.result.map(company => ({
              name: company.name,
              companyId: company.companyId.toString(),
            }));

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      async getAllStocks() {
        const url = '/api/Stock/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            this.stocks = response.data.result.map(stock => ({
              name: stock.name,
              stockId: stock.stockId.toString(),
              companyId: stock.companyId
            }));

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          }
      },
      async getStocksByCompanyId(companyId) {
        this.stocksByCompanyId = [];
        delete this.filters.stockId
        const url = `/api/Stock/getByCompanyId?companyId=${companyId}`;

        try {
          const response = await api.get(url, {
            headers: {
              'accept': '*/*'
            }
          });

          this.stocksByCompanyId = response.data.result.map(stock => ({
            name: stock.name,
            stockId: stock.stockId.toString(),
          }));
          
        } catch (error) {
          console.error('Error fetching stocks by company ID:', error);
          // this.$store.commit('setErrorMessage', error);
        }
      },
      navigateStorageLocationId(item) {
        this.filters = {
          storageLocationId: item.storageLocationId,
          isBusy: ''
        }

        this.filters = {storageLocationId: item.storageLocationId}
        this.isEditing = true
        const stock = this.stocks.find(s => s.stockId === item.stockId);
        this.selectedStorageLocation = {companyId: stock.companyId, ...item};
        this.applyFilters();

        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedStorageLocation.storageLocationId = undefined
          }
      },
      async applyFilters() {
        this.isLoading = true;
        const url = '/api/StorageLocation/getStorageLocationsFiltered';

        try {
          const response = await api.post(url, this.filters, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });
          this.storageLocations = Array.from(response.data.result);
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        } finally {
          this.isLoading = false;
        }
      },
      resetFilters() {
        this.filters = {isBusy: null}
        this.applyFilters();
      },
      async saveStorageLocation() {
        const formData = new FormData();
        if(this.selectedStorageLocation.storageLocationId)
          formData.append('StorageLocationId', this.selectedStorageLocation.storageLocationId);
        if(this.selectedStorageLocation.rackCode)
          formData.append('RackCode', this.selectedStorageLocation.rackCode);
        if(this.selectedStorageLocation.shelfCode)
          formData.append('ShelfCode', this.selectedStorageLocation.shelfCode);
        if(this.selectedStorageLocation.description)
          formData.append('Description', this.selectedStorageLocation.description);
        if (this.selectedStorageLocation.image) 
          formData.append('Image', this.selectedStorageLocation.image);
        if(this.selectedStorageLocation.stockId)
          formData.append('StockId', this.selectedStorageLocation.stockId);

        try {
          if (this.selectedStorageLocation.storageLocationId !== undefined || this.isEditing) {
            await api.put('/api/StorageLocation/update', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } else {
            await api.post('/api/StorageLocation/create', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } 
          
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        }
      },
      async deleteStorageLocation() {
        try {
          await api.delete(`/api/StorageLocation/dellById?storageLocationId=${this.selectedStorageLocation.storageLocationId}`);
          
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        }
      },
      async handleRowClick(item) {
        
        if (this.selectedStorageLocation.storageLocationId === item.storageLocationId) {
          delete this.selectedStorageLocation.storageLocationId
          this.isEditing = false
        } else {
          this.selectedStorageLocation = {...item};
          this.isEditing = true
        }
      },
      getStockName(stockId) {
        const stock = this.stocks.find(s => s.stockId === stockId);
        return stock ? stock.name : 'Не указано';
      },
      getCompanyName(companyId) {
        const company = this.companies.find(c => c.companyId === companyId);
        return company ? company.name : 'Не указано';
      },
  },
  computed: {
    selectedStockId: {
      get() {
        const stock = this.stocks.find(s => s.stockId === this.selectedStorageLocation.stockId);
        return stock ? stock : null;
      },
      set(value) {
        this.selectedStorageLocation.stockId = value;
      }
    },
    selectedCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.selectedStorageLocation.companyId);
      return company ? company.companyId : null;
    },
    set(value) {
      this.selectedStorageLocation.companyId = value;
    }
    },
    filtersStockId: {
      get() {
        const stock = this.stocks.find(s => s.stockId === this.filters.stockId);
        return stock ? stock : null;
      },
      set(value) {
        this.filters.stockId = value;
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
    ...mapGetters([
      'companies',
      'stocks'
    ]),
    ...mapGetters('storageLocationPage', [
      'stocksByCompanyId',
      'fStocksByCompanyId'
    ])

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

:deep(.image-tooltip) {
  padding: 0 !important;
  background-color: transparent !important;
  opacity: 1 !important;
}
.full-size-image {
  width: 200px;
  height: 200px; 
  object-fit: cover;
}
</style>