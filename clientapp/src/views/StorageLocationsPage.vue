<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="storageLocations"
          :page.sync="page"
          :items-per-page="itemsPerPage"
          @update:page="updatePage"
          @update:items-per-page="updateItemsPerPage"
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
              <td
              v-if="policyA.includes(getEmployeeInfo.role)"
              >{{ item.companyName }}</td>
              <td
              v-if="policyCA.includes(getEmployeeInfo.role)"
              >{{ item.stockName }}</td>
              <td>{{ item.description }}</td>
              
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
            :cols="`${policyCA.includes(getEmployeeInfo.role) ? 6 : 12}`"
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
                  label="ID места хранения"
                  v-model="filters.storageLocationId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="8">
                <v-text-field
                  label="Код стеллажа"
                  v-model="filters.rackCode"
                  prepend-icon="mdi-file-cabinet"
                ></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-text-field
                  label="Описание"
                  v-model="filters.description"
                  prepend-icon="mdi-note-text"
                ></v-text-field>
              </v-col>
              </v-row>

             
                </v-card-text>
              </v-card>
            </v-col>

              
            <v-col
            v-if="policyCA.includes(getEmployeeInfo.role)"
             cols="6">
              <v-card color="teal-lighten-5">
                <v-card-title>
                  Иерархические сущности
                </v-card-title>
                <v-card-subtitle>
                  Выберите один из предложенных вариантов
                </v-card-subtitle>
                <v-card-text>
                  <v-row>
                <v-col
                v-if="policyA.includes(getEmployeeInfo.role)"
                cols="12">
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
              </v-row>
              <v-row>
                <v-col
                cols="12">
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
              </v-row>
                </v-card-text>
              </v-card>
            </v-col>
            </v-row>

  
          </v-expansion-panel-text>
        </v-expansion-panel>
      </v-expansion-panels>
  
      <!-- Секция редактирования -->
      <v-card >
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
                @click="selectedStorageLocation = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveStorageLocation"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteStorageLocation"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
        <v-card-text>
          <div v-if="selectedStorageLocation.storageLocationId !== undefined || isEditing">
            <v-row>
              <v-col cols="12">
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
                        v-model="selectedStorageLocation.storageLocationId"
                        label="ID места хранения"
                        type="number"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="8">
                      <v-file-input
                        v-model="selectedStorageLocation.image"
                        label="Фото места хранения"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
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

                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>

          </div>

          <div v-else>
            <v-row>
              <v-col
              :cols="`${policyCA.includes(getEmployeeInfo.role) ? 6 : 12}`"
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
                        v-model="selectedStorageLocation.rackCode"
                        label="Код стеллажа*"
                        prepend-icon="mdi-file-cabinet"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="8">
                      <v-file-input
                        v-model="selectedStorageLocation.image"
                        label="Фото места хранения"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
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
                  </v-card-text>
                </v-card>
              </v-col>

              <v-col cols="6"
              v-if="policyCA.includes(getEmployeeInfo.role)"
              >
                <v-card color="teal-lighten-5">
                  <v-card-title>
                    Иерархические сущности
                  </v-card-title>
                  <v-card-subtitle>
                    Выберите один из предложенных вариантов
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
                      <v-col
                      v-if="policyA.includes(getEmployeeInfo.role)"
                      cols="12">
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
                    </v-row>
                    <v-row>
                      <v-col
                      cols="12">
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
          { title: 'ID стеллажа*', key: 'storageLocationId', align: 'start', sortable: true },
          { title: 'Код стеллажа', key: 'rackCode', align: 'start', sortable: true },
          { title: 'Фото места хранения', key: 'imagePath', align: 'start', sortable: false },
          { title: 'Описание', key: 'description', align: 'start', sortable: true },
          
        ],
        storageLocations: [],
        selectedStorageLocation: {},
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

      if (this.policyA.includes(this.getEmployeeInfo.role))
        this.$store.dispatch('getAllCompanies');
      else if (this.policyCA.includes(this.getEmployeeInfo.role)) {
        this.$store.dispatch( 'storageLocationPage/getStocksByCompanyId', {companyId: this.getEmployeeInfo.companyId, selected: true})
        this.$store.dispatch( 'storageLocationPage/getStocksByCompanyId', {companyId: this.getEmployeeInfo.companyId, selected: false})
      }
    },
    deactivated() {
      this.abortFlag = true
      clearTimeout(this.timeoutId)
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
      updatePage(newPage) {
        this.page = newPage;
      },
      updateItemsPerPage(itemsPerPage) {
        this.itemsPerPage = itemsPerPage;
      },
      navigateStorageLocationId(item) {
        this.filters = {storageLocationId: item.storageLocationId}
        this.isEditing = true
        this.selectedStorageLocation = {...item};
        this.applyFilters();
        this.filters = {}
        window.scrollTo(0, document.body.scrollHeight);

        if (this.policyCA.includes(this.getEmployeeInfo.role))
          this.$store.dispatch('storageLocationPage/getStocksByCompanyId', 
            {companyId: this.selectedStorageLocation.companyId, selected: true})
   
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedStorageLocation.storageLocationId = undefined
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
        const url = '/api/StorageLocation/getStorageLocationsFiltered';

        const data = {}

      if (this.filters.storageLocationId)
        data.storageLocationId = this.filters.storageLocationId
      if (this.filters.rackCode)
        data.rackCode = this.filters.rackCode
      if (this.filters.description)
        data.description = this.filters.description
      if (this.filters.companyId)
        data.companyId = this.filters.companyId
      if (this.filters.stockId)
        data.stockId = this.filters.stockId

        return new Promise((resolve, reject) => {
          api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          })
          .then(response => {
            this.storageLocations = Array.from(response.data.result);
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
      async saveStorageLocation() {
        const formData = new FormData();
        if(this.selectedStorageLocation.storageLocationId)
          formData.append('StorageLocationId', this.selectedStorageLocation.storageLocationId);
        if(this.selectedStorageLocation.rackCode)
          formData.append('RackCode', this.selectedStorageLocation.rackCode);
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
        api.delete(`/api/StorageLocation/dellById?storageLocationId=${this.selectedStorageLocation.storageLocationId}`)
        .then(() => this.applyFilters())
        .catch(error => this.$store.commit('setErrorMessage', error))
      },
      async handleRowClick(item) {
        
        if (this.selectedStorageLocation.storageLocationId === item.storageLocationId) {
          delete this.selectedStorageLocation.storageLocationId
          this.isEditing = false
        } else {
          this.selectedStorageLocation = {...item};
          this.isEditing = true


          if (this.policyCA.includes(this.getEmployeeInfo.role))
            this.$store.dispatch('storageLocationPage/getStocksByCompanyId', 
              {companyId: this.selectedStorageLocation.companyId, selected: true})
        }
        
      },
  },
  computed: {
    selectedStockId: {
      get() {
        const stock = this.stocksByCompanyId.find(s => s.stockId === this.selectedStorageLocation.stockId);
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
        const stock = this.fStocksByCompanyId.find(s => s.stockId === this.filters.stockId);
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
    ]),
    getEmployeeInfo() {
        if (this.$store.state.employeeInfo) 
          return this.$store.state.employeeInfo
        else return {}
      },
      policyCA() {
        return this.$store.state.policyCA
      },
      policyA() {
        return this.$store.state.policyA
      },
      headers() {
        if (this.policyCA.includes(this.getEmployeeInfo.role)) {
        this.baseHeaders.splice(3, 0, { 
          title: 'Склад', 
          key: 'stockName', 
          align: 'start', 
          sortable: true 
        });
      }

      if (this.policyA.includes(this.getEmployeeInfo.role)) {
        this.baseHeaders.splice(3, 0, { 
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