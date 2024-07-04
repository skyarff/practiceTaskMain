<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="employees"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedEmployee.employeeId === item.employeeId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateCompanyId(item)" 
              class="navigation-column">
                {{ item.employeeId}}
              </td>
              <td>{{ item.fullName }}</td>
              <td>{{ item.jobTitle }}</td>

              <td>{{ item.login }}</td>
              <td>{{ item.password }}</td>
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
              <td>{{ item.stockId }}</td>
              <td>{{ item.email }}</td>
              <td>{{ item.phone }}</td>
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
                  label="ID сотрудника"
                  v-model="filters.employeeId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ФИО"
                  v-model="filters.name"
                  prepend-icon="mdi-account"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Должность"
                  v-model="filters.jobTitle"
                  prepend-icon="mdi-briefcase"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Логин"
                  v-model="filters.login"
                  prepend-icon="mdi-account-circle"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Пароль"
                  v-model="filters.password"
                  prepend-icon="mdi-account-key"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Почта"
                  v-model="filters.email"
                  prepend-icon="mdi-email"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Телефон"
                  v-model="filters.phone"
                  prepend-icon="mdi-phone"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID склада"
                  v-model="filters.stockId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID компании"
                  v-model="filters.companyId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
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
          <v-switch
            :model-value="isEditing"
            color="primary"
            label="Редактирование"
            @click="switchEditingMode"
          ></v-switch>
        </v-card-text>
          <div v-if="selectedEmployee.employeeId !== undefined || isEditing">
            <v-card-title>Редактирование/удаление</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="saveCompany">

                <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID сотрудника"
                        v-model="selectedEmployee.employeeId"
                        type="number"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ФИО"
                        v-model="selectedEmployee.name"
                        prepend-icon="mdi-account"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Должность"
                        v-model="selectedEmployee.jobTitle"
                        prepend-icon="mdi-briefcase"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  
                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Логин"
                        v-model="selectedEmployee.login"
                        prepend-icon="mdi-account-circle"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Пароль"
                        v-model="selectedEmployee.password"
                        prepend-icon="mdi-account-key"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Почта"
                        v-model="selectedEmployee.email"
                        prepend-icon="mdi-email"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Телефон"
                        v-model="selectedEmployee.phone"
                        prepend-icon="mdi-phone"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID склада"
                        v-model="selectedEmployee.stockId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID компании"
                        v-model="selectedEmployee.companyId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
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
                <v-form @submit.prevent="saveEmployee">

                  <v-row>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ФИО"
                        v-model="selectedEmployee.fullName"
                        prepend-icon="mdi-account"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Должность"
                        v-model="selectedEmployee.jobTitle"
                        prepend-icon="mdi-briefcase"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Логин"
                        v-model="selectedEmployee.login"
                        prepend-icon="mdi-account-circle"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  
                  <v-row>
                    
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Пароль"
                        v-model="selectedEmployee.password"
                        prepend-icon="mdi-account-key"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Почта"
                        v-model="selectedEmployee.email"
                        prepend-icon="mdi-email"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Телефон"
                        v-model="selectedEmployee.phone"
                        prepend-icon="mdi-phone"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID склада"
                        v-model="selectedEmployee.stockId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4" sm="4">
                      <v-file-input
                        v-model="selectedEmployee.image"
                        label="Фото сотрудника"
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
          { title: 'ID сотрудника*', key: 'employeeId', align: 'start', sortable: true },
          { title: 'ФИО', key: 'fullName', align: 'start', sortable: true },
          { title: 'Должность', key: 'jobTitile', align: 'start', sortable: true },
          { title: 'Логин', key: 'login', align: 'start', sortable: true },
          { title: 'Пароль', key: 'password', align: 'start', sortable: true },
          { title: 'Фото сотудника', key: 'imagePath', align: 'start', sortable: false },
          { title: 'ID склада*', key: 'stockId', align: 'start', sortable: true },
          { title: 'Почта', key: 'email', align: 'start', sortable: true },
          { title: 'Телефон', key: 'phone', align: 'start', sortable: true },
        ],
        employees: [],
        selectedEmployee: {},
        isEditing: false,
        isLoading: true
      }
    },
    mounted() {
      this.applyFilters();
    },
    methods: {
      navigateEmployeeId(item) {
        this.filters = {employeeId: item.employeeId}
        this.isEditing = true
        this.selectedEmployee = item
        this.applyFilters();
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedEmployee.employeeId = undefined
          }
      },
      async applyFilters() {
        this.isLoading = true;
        const url = '/api/Employee/getEmployeesFiltered';
        const data = {}

        if(this.filters.employeeId)
          data.employeeId = this.filters.employeeId
        if(this.filters.login)
          data.login = this.filters.login
        if(this.filters.fullname)
          data.fullname = this.filters.fullname
        if(this.filters.jobtitle)
          data.jobtitle = this.filters.jobtitle
        if(this.filters.email)
          data.email = this.filters.email
        if(this.filters.phone)
          data.phone = this.filters.phone

        if(this.filters.stockId)
          data.companyId = this.filters.companyId
        if(this.filters.companyId)
          data.companyId = this.filters.companyId
        
        try {
          const response = await api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });
          this.employees = Array.from(response.data.result);
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
      async saveEmployee() {
        const formData = new FormData();
        if(this.selectedEmployee.employeeId)
          formData.append('EmployeeId', this.selectedEmployee.employeeId);
        if(this.selectedEmployee.fullName)
          formData.append('FullName', this.selectedEmployee.fullName);
        if(this.selectedEmployee.jobTitle)
          formData.append('JobTitle', this.selectedEmployee.jobTitle);
        if(this.selectedEmployee.login)
          formData.append('Login', this.selectedEmployee.login);
        if(this.selectedEmployee.password)
          formData.append('Password', this.selectedEmployee.password);
        if(this.selectedEmployee.stockId)
          formData.append('StockId', this.selectedEmployee.stockId);
        if(this.selectedEmployee.email)
          formData.append('Email', this.selectedEmployee.email);
        if(this.selectedEmployee.phone)
          formData.append('Phone', this.selectedEmployee.phone);
        if (this.selectedEmployee.image)
          formData.append('Image', this.selectedEmployee.image);

        try {
          let response;
          if (this.selectedEmployee.employeeId !== undefined || this.isEditing) {
            response = await api.put('api/Employee/update', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } else {
            response = await api.post('api/Employee/Create', formData, {
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
          await api.delete(`api/Employee/dellById?employeeId=${this.selectedEmployee.employeeId}`);
          
          this.applyFilters();
        } catch (error) {
          console.error('Ошибка при удалении компании:', error);
        }
      },
      async handleRowClick(item) {
        
        if (this.selectedEmployee.employeeId === item.employeeId) {
          delete this.selectedEmployee.employeeId
          this.isEditing = false
        } else {
          this.selectedEmployee = {...item};
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