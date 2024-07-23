<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="companies"
          :page.sync="page"
          :items-per-page="itemsPerPage"
          @update:page="updatePage"
          @update:items-per-page="updateItemsPerPage"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedCompany.companyId === item.companyId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateCompanyId(item)" 
              class="navigation-column">
                {{ item.companyId}}
              </td>
              <td>{{ item.name }}</td>
              <td>{{ item.inn }}</td>
              <td>
                  <div v-if="item.logoPath">
                    <v-img 
                    :src="`${apiBaseUrl}//${item.logoPath}`"
                    class="full-size-image"
                    style="max-width: 40px; max-height: 40px"
                  ></v-img>

                  <v-tooltip 
                    activator="parent" 
                    location="start"
                    content-class="image-tooltip"
                  >
                    <v-img
                      :src="`${apiBaseUrl}//${item.logoPath}`"
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
                      <v-col cols="2">
                        <v-text-field
                          label="ID компании"
                          v-model="filters.companyId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="5">
                        <v-text-field
                          label="Название компании"
                          v-model="filters.name"
                          prepend-icon="mdi-domain"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="5">
                        <v-text-field
                          label="ИНН"
                          v-model="filters.inn"
                          prepend-icon="mdi-card-account-details"
                        ></v-text-field>
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
          <div v-if="selectedCompany.companyId !== undefined || isEditing">

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
                  <v-col cols="2">
                    <v-text-field
                      v-model="selectedCompany.companyId"
                      label="ID компании"
                      type="number"
                      prepend-icon="mdi-identifier"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="10">
                    <v-text-field
                      v-model="selectedCompany.name"
                      label="Название"
                      prepend-icon="mdi-domain"
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="10">
                    <v-text-field
                      v-model="selectedCompany.inn"
                      label="ИНН"
                      prepend-icon="mdi-card-account-details"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="2">
                    <v-file-input
                      v-model="selectedCompany.image"
                      label="Логотип"
                      accept="image/*"
                      prepend-icon="mdi-image"
                    ></v-file-input>
                  </v-col>
                </v-row>
                  </v-card-text>
                </v-card>
              </v-col>
            </v-row>        
          </div>
          <div v-else>

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
                    <v-col cols="5">
                      <v-text-field
                        v-model="selectedCompany.name"
                        label="Название*"
                        prepend-icon="mdi-domain"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="5">
                      <v-text-field
                        v-model="selectedCompany.inn"
                        label="ИНН"
                        prepend-icon="mdi-card-account-details"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="2">
                      <v-file-input
                        v-model="selectedCompany.image"
                        label="Логотип"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
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

  export default {
    components: {
      Loader
    },
    data() {
      return {
        apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        filters: {},
        headers: [
          { title: 'ID компании*', key: 'companyId', align: 'start', sortable: true },
          { title: 'Название', key: 'name', align: 'start', sortable: true },
          { title: 'ИНН', key: 'inn', align: 'start', sortable: true },
          { title: 'Логотип', key: 'logoPath', align: 'start', sortable: false },
        ],
        companies: [],
        selectedCompany: {},
        isEditing: false,
        isLoading: true,
        page: 1,
        itemsPerPage: 10,
        controller: null,
        flag: false
      }
    },
    activated() {
      // this.controller = new AbortController();
      this.flag = false
      this.checkConnection();
    },
    deactivated() {
      // this.controller.abort();
      this.flag = true
    },
    methods: {
      updatePage(newPage) {
        this.page = newPage;
      },
      updateItemsPerPage(itemsPerPage) {
        this.itemsPerPage = itemsPerPage;
      },
      navigateCompanyId(item) {
        this.filters = {companyId: item.companyId}
        this.isEditing = true
        this.selectedCompany = item
        this.applyFilters();
        this.filters = {}
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedCompany.companyId = undefined
          }
      },
      async applyFilters() {
        this.isLoading = true;
        const url = '/api/Company/getCompaniesFiltered';
        
        return new Promise((resolve, reject) => {
          api.post(url, this.filters, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          })
          .then(response => {
            this.companies = Array.from(response.data.result);
            resolve();
          })
          .catch(error => {
            this.$store.commit('setErrorMessage', error);
            reject(error);
          })
          .finally(() => {
            this.isLoading = false;
          });
        });
      },
      async checkConnection() {
        // signal.addEventListener('abort', () => console.log('Aborted!'));

        let flag = true;
        while (flag && !this.flag) {
          try {
            await this.applyFilters();
            flag = false;
          } catch {
            await new Promise(resolve => setTimeout(resolve, 7000));
          }
        }
      },
      resetFilters() {
        this.filters = {}
        this.applyFilters();
      },
      async saveCompany() {
        const formData = new FormData();
        if(this.selectedCompany.companyId)
          formData.append('CompanyId', this.selectedCompany.companyId);
        if(this.selectedCompany.name)
          formData.append('Name', this.selectedCompany.name);
        if(this.selectedCompany.inn)
          formData.append('Inn', this.selectedCompany.inn);
        if (this.selectedCompany.image)
          formData.append('Image', this.selectedCompany.image);

        try {
          if (this.selectedCompany.companyId !== undefined || this.isEditing) {
            await api.put('api/Company/update', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } else {
            await api.post('api/Company/Create', formData, {
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
      async deleteCompany() {

        api.delete(`api/Company/dellById?companyId=${this.selectedCompany.companyId}`)
        .then(() => this.applyFilters())
        .catch(error => this.$store.commit('setErrorMessage', error)) 
      },
      async handleRowClick(item) {
        
        if (this.selectedCompany.companyId === item.companyId) {
          delete this.selectedCompany.companyId
          this.isEditing = false
        } else {
          this.selectedCompany = {...item};
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