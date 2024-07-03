<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card class="mb-4">
        <v-data-table
          :headers="headers"
          :items="companies"
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
                <v-img
                    :src="`${apiBaseUrl}//${item.logoPath}`"
                    class="full-size-image"
                    style="max-width: 40px; max-height: 40px"
                  ></v-img>
                <v-tooltip 
                  v-if="item.logoPath"
                  activator="parent" 
                  location="start"
                  content-class="image-tooltip"
                >
                  <v-img
                    :src="`${apiBaseUrl}//${item.logoPath}`"
                    class="full-size-image"
                  ></v-img>
                </v-tooltip>
              </td>
            </tr>
          </template>
        </v-data-table>
      </v-card>
  

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
                  label="ID компании"
                  v-model="filters.companyId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Название компании"
                  v-model="filters.name"
                  prepend-icon="mdi-domain"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ИНН"
                  v-model="filters.inn"
                  prepend-icon="mdi-card-account-details"
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
            @click="editModeSwitch"
          ></v-switch>
        </v-card-text>
          <div v-if="selectedCompany.companyId !== undefined || isEditing">
            <v-card-title>Редактирование/удаление</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="saveCompany">
                <v-row>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedCompany.companyId"
                      label="ID компании"
                      type="number"
                      prepend-icon="mdi-identifier"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedCompany.name"
                      label="Название"
                      prepend-icon="mdi-domain"
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedCompany.inn"
                      label="ИНН"
                      prepend-icon="mdi-card-account-details"
                    ></v-text-field>
                  </v-col>
                  <v-col cols="12" sm="6">
                    <v-file-input
                      v-model="selectedCompany.image"
                      label="Логотип"
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
                    <v-btn @click="deleteCompany" color="secondary" prepend-icon="mdi-delete">
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
                <v-form @submit.prevent="saveCompany">
                  <v-row>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="selectedCompany.name"
                        label="Название"
                        prepend-icon="mdi-domain"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6">
                      <v-text-field
                        v-model="selectedCompany.inn"
                        label="ИНН"
                        prepend-icon="mdi-card-account-details"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="12">
                      <v-file-input
                        v-model="selectedCompany.image"
                        label="Логотип"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
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

  export default {
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
      }
    },
    mounted() {
      this.applyFilters();
    },
    methods: {
      navigateCompanyId(item) {
        this.filters = {companyId: item.companyId}
        this.isEditing = true
        this.selectedCompany = item
        this.applyFilters();
        window.scrollTo(0, document.body.scrollHeight);
      },
      editModeSwitch() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedCompany.companyId = undefined
          }
      },
      async applyFilters() {
        const url = '/api/Company/getCompaniesFiltered';
        const data = {}

        if(this.filters.companyId)
          data.companyId = this.filters.companyId
        if(this.filters.name)
          data.name = this.filters.name
        if(this.filters.inn)
          data.inn = this.filters.inn
        
        try {
          const response = await api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });
          this.companies = Array.from(response.data.result);
        } catch (error) {
          console.error('Ошибка при выполнении запроса:', error);
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
        if (this.selectedCompany.image) {
          formData.append('Image', this.selectedCompany.image);
        }

        try {
          let response;
          if (this.selectedCompany.companyId !== undefined || this.isEditing) {
            response = await api.put('api/Company/update', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } else {
            response = await api.post('api/Company/Create', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } 
          
          this.applyFilters();
        } catch (error) {
          console.error('Ошибка при сохранении компании:', error);
        }
      },
      async deleteCompany() {
        try {
          await api.delete(`api/Company/dellById?companyId=${this.selectedCompany.companyId}`);
          
          this.applyFilters();
        } catch (error) {
          console.error('Ошибка при удалении компании:', error);
        }
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