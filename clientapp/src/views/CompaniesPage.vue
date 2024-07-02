<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card class="mb-4">
        <v-data-table
          :headers="headers"
          :items="companies"
          class="elevation-1"
          :search="search"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedCompany.companyId === item.companyId }" 
            @click="handleRowClick(item)">
              <td>{{ item.companyId }}</td>
              <td>{{ item.name }}</td>
              <td>{{ item.inn }}</td>
              <td>
                <v-icon>
                  mdi-image
                </v-icon>
                <v-tooltip v-if="item.logoPath" 
                activator="parent" location="top">
                  <v-img
                    :width="100"
                    cover
                    :src="'http://localhost:5180//' + item.logoPath"
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
          <v-btn color="primary" @click="applyFilters" prepend-icon="mdi-magnify">
            Применить фильтры
          </v-btn>
        </v-col>
      </v-row>
    </v-expansion-panel-text>
  </v-expansion-panel>
      </v-expansion-panels>
  
      <v-card >
        <v-card-text>
          <v-switch
            :model-value="isEditing"
            color="primary"
            label="Редактирование"
            @click="modeSwitch"
          ></v-switch>
        </v-card-text>
          <div v-if="selectedCompany.companyId !== 0 || isEditing">
      <v-card-title>Редактирование/удаление</v-card-title>
      <v-card-text>
        <v-form @submit.prevent="saveCompany">
          <v-row>
            <v-col cols="12" sm="6">
              <v-text-field
                v-model="selectedCompany.companyId"
                label="ID"
                readonly
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
import axios from 'axios';

  export default {
    data() {
      return {
        filters: {
          name: '',
          status: null,
        },
        headers: [
          { title: 'ID', key: 'companyId', align: 'start', sortable: true },
          { title: 'Название', key: 'name', align: 'start', sortable: true },
          { title: 'ИНН', key: 'inn', align: 'start', sortable: true },
          { title: 'Логотип', key: 'logoPath', align: 'start', sortable: false },
        ],
        companies: [],
        selectedCompany: {
          companyId: 0,
          name: '',
          inn: '',
          image: null,
        },
        isEditing: false,
      }
    },
    mounted() {
        this.applyFilters();
    },
    methods: {
      modeSwitch() {
        this.isEditing = !this.isEditing
        if (!this.isEditing) {
          this.selectedCompany.companyId = 0
        }
      },
      async fetchCompanies() {
        const url = '/api/Company/getCompaniesFiltered';
        const data = {};

        axios.post(url, data, {
          headers: {
            'accept': '*/*',
            'Content-Type': 'application/json'
          }
        })
        .then(response => {
          // console.log(response.data)
          this.companies = Array.from(response.data.result);
        })
        .catch(error => {
          console.error('Ошибка при выполнении запроса:', error);
        });
    },
    async applyFilters() {
      const url = '/api/Company/getCompaniesFiltered';
      const data = {
        companyId: this.filters.companyId || null,
        name: this.filters.name || null,
        inn: this.filters.inn || null
      };
      try {
        const response = await axios.post(url, data, {
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
      getImageUrl(path) {
      if (!path) return ''; // Проверка на пустой путь
      return path.startsWith('/api') ? path : `/api${path}`;
    },
    async saveCompany() {

      const formData = new FormData();
      formData.append('CompanyId', this.selectedCompany.companyId || '');
      formData.append('Name', this.selectedCompany.name);
      formData.append('Inn', this.selectedCompany.inn);
      if (this.selectedCompany.image) {
        formData.append('Image', this.selectedCompany.image);
      }

      try {
        let response;
        if (this.selectedCompany.companyId !== 0 || this.isEditing) {
          response = await axios.put('api/Company/update', formData, {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          });
        } else {
          response = await axios.post('api/Company/Create', formData, {
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
      if (!this.selectedCompany.companyId) return;

      try {
        await axios.delete(`api/Company/dellById?companyId=${this.selectedCompany.companyId}`);
        
        this.applyFilters();
      } catch (error) {
        console.error('Ошибка при удалении компании:', error);
      }
    },
    closeDialog() {
      this.selectedCompany = {
        companyId: null,
        name: '',
        inn: '',
        image: null
      };
      this.selectedCompany.companyId = 0;
    },
    async handleRowClick(item) {
      
      if (this.selectedCompany.companyId === item.companyId) {
        this.selectedCompany.companyId = 0
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
  outline: 2px solid #7dc0a4;
  outline-offset: -2px;
  border-radius: 0%;
}
</style>