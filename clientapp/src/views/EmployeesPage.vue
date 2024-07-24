<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="employees"
          :page.sync="page"
          :items-per-page="itemsPerPage"
          @update:page="updatePage"
          @update:items-per-page="updateItemsPerPage"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedEmployee.employeeId === item.employeeId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateEmployeeId(item)" 
              class="navigation-column">
                {{ item.employeeId}}
              </td>
              <td>{{ item.login }}</td>
              <td>{{ item.fullName }}</td>
              <td>{{ item.jobTitle }}</td>
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
              <td>{{ item.companyName }}</td>
              <td>{{ item.stockName }}</td>
              <td>{{ item.phone }}</td>
              <td>{{ item.email }}</td> 
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
              <v-col cols="6">
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
                  label="ID сотрудника"
                  v-model="filters.employeeId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="4">
                <v-text-field
                  label="ФИО"
                  v-model="filters.fullName"
                  prepend-icon="mdi-account"
                ></v-text-field>
              </v-col>

              <v-col cols="4">
                <v-text-field
                  label="Должность"
                  v-model="filters.jobTitle"
                  prepend-icon="mdi-briefcase"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="4">
                <v-text-field
                  label="Логин"
                  v-model="filters.login"
                  prepend-icon="mdi-account-circle"
                ></v-text-field>
              </v-col>

              <v-col cols="4">
                <v-text-field
                  label="Почта"
                  v-model="filters.email"
                  prepend-icon="mdi-email"
                ></v-text-field>
              </v-col>

              <v-col cols="4">
                <v-text-field
                  label="Телефон"
                  v-model="filters.phone"
                  prepend-icon="mdi-phone"
                ></v-text-field>
              </v-col>

            </v-row>
                  </v-card-text>
                </v-card>
              </v-col>

              <v-col cols="6">
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
                      <v-col cols="12">
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
                @click="selectedEmployee = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveEmployee"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteEmployee"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
        <v-card-text>
          <div v-if="selectedEmployee.employeeId !== undefined || isEditing">

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
                        label="ID сотрудника"
                        v-model="selectedEmployee.employeeId"
                        type="number"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="3">
                      <v-text-field
                        label="ФИО"
                        v-model="selectedEmployee.fullName"
                        prepend-icon="mdi-account"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="3">
                      <v-file-input
                        v-model="selectedEmployee.image"
                        label="Фото сотрудника"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>

                    
                    <v-col cols="4">
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

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Пароль"
                        v-model="selectedEmployee.password"
                        prepend-icon="mdi-account-key"
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
              <v-col cols="8">
                <v-card color="grey-lighten-4">
                  <v-card-title>
                    Поля
                  </v-card-title>
                  <v-card-subtitle>
                    Заполните поля данными
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>

                    <v-col cols="3">
                      <v-text-field
                        label="ФИО*"
                        v-model="selectedEmployee.fullName"
                        prepend-icon="mdi-account"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="2">
                      <v-file-input
                        v-model="selectedEmployee.image"
                        label="Фото сотрудника"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>

                    <v-col cols="4">
                      <v-text-field
                        label="Должность*"
                        v-model="selectedEmployee.jobTitle"
                        prepend-icon="mdi-briefcase"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="3">
                      <v-text-field
                        label="Логин*"
                        v-model="selectedEmployee.login"
                        prepend-icon="mdi-account-circle"
                      ></v-text-field>
                    </v-col>
                    </v-row>

                    <v-row>
                      <v-col cols="4">
                        <v-text-field
                          label="Пароль"
                          v-model="selectedEmployee.password"
                          prepend-icon="mdi-account-key"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="Почта"
                          v-model="selectedEmployee.email"
                          prepend-icon="mdi-email"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="Телефон"
                          v-model="selectedEmployee.phone"
                          prepend-icon="mdi-phone"
                        ></v-text-field>
                      </v-col>
                    </v-row>

                  </v-card-text>
                </v-card>
              </v-col>

              <v-col cols="4">
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
                        @update:modelValue="selectionOfStocks(true)"
                      ></v-select>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="12">
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
        headers: [
          { title: 'ID сотрудника*', key: 'employeeId', align: 'start', sortable: true },
          { title: 'Логин', key: 'login', align: 'start', sortable: true },
          { title: 'ФИО', key: 'fullName', align: 'start', sortable: true },
          { title: 'Должность', key: 'jobTitile', align: 'start', sortable: true },
          { title: 'Фото сотудника', key: 'imagePath', align: 'start', sortable: false },
          { title: 'Компания', key: 'companyName', align: 'start', sortable: true },
          { title: 'Склад', key: 'stockName', align: 'start', sortable: true },
          { title: 'Телефон', key: 'phone', align: 'start', sortable: true },
          { title: 'Почта', key: 'email', align: 'start', sortable: true },
        ],
        employees: [],
        selectedEmployee: {},
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
      this.$store.dispatch('getAllCompanies');
    },
    deactivated() {
      this.abortFlag = true
      clearTimeout(this.timeoutId)
    },
    methods: {
      selectionOfStocks(selected) {
        let companyId;
        if (selected) {
          companyId = this.selectedEmployee.companyId
          this.selectedStockId = null
        } else {
          companyId = this.filters.companyId
          this.filtersStockId = null
        }
        this.$store.dispatch( 'employeePage/getStocksByCompanyId', {companyId: companyId, selected: selected})
      },
      updatePage(newPage) {
        this.page = newPage;
      },
      updateItemsPerPage(itemsPerPage) {
        this.itemsPerPage = itemsPerPage;
      },
      navigateEmployeeId(item) {
        this.filters = {employeeId: item.employeeId}
        this.isEditing = true
        this.selectedEmployee = {...item};
        delete this.selectedEmployee.password
        this.applyFilters();
        this.filters = {}
        window.scrollTo(0, document.body.scrollHeight);
        this.$store.dispatch( 'employeePage/getStocksByCompanyId', {companyId: this.selectedEmployee.companyId, selected: true})
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedEmployee.employeeId = undefined
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
        const url = '/api/Employee/getEmployeesFiltered';

        const data = {}

        if (this.filters.employeeId)
          data.employeeId = this.filters.employeeId
        if (this.filters.fullName)
          data.fullName = this.filters.fullName
        if (this.filters.jobTitle)
          data.jobTitle = this.filters.jobTitle
        if (this.filters.login)
          data.login = this.filters.login
        if (this.filters.email)
          data.email = this.filters.email
        if (this.filters.phone)
          data.phone = this.filters.phone
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
            this.employees = Array.from(response.data.result);
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
          if (this.selectedEmployee.employeeId !== undefined || this.isEditing) {
            await api.put('api/Employee/update', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } else {
            await api.post('api/Employee/Create', formData, {
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
      async deleteEmployee() {

        api.delete(`api/Employee/dellById?employeeId=${this.selectedEmployee.employeeId}`)
        .then(() => this.applyFilters())
        .catch(error => this.$store.commit('setErrorMessage', error))
      },
      handleRowClick(item) {
        if (this.selectedEmployee.employeeId === item.employeeId) {
          delete this.selectedEmployee.employeeId
          this.isEditing = false
        } else {
          this.selectedEmployee = {...item};
          this.isEditing = true
          delete this.selectedEmployee.password
          this.$store.dispatch( 'employeePage/getStocksByCompanyId', {companyId: this.selectedEmployee.companyId, selected: true})
        }
      },
  },
    computed: {
    
    selectedCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.selectedEmployee.companyId);
      return company ? company.name : null;
    },
    set(value) {
      this.selectedEmployee.companyId = value;
    }
    },
    selectedStockId: {
      get() {
        const stock = this.stocksByCompanyId.find(s => s.stockId === this.selectedEmployee.stockId);
        return stock ? stock.name : null;
      },
      set(value) {
        this.selectedEmployee.stockId = value;
      }
    },
    filtersCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.filters.companyId);
      return company ? company.name : null;
    },
    set(value) {
      this.filters.companyId = value;
    }
    },
    filtersStockId: {
      get() {
        const stock = this.fStocksByCompanyId.find(s => s.stockId === this.filters.stockId);
        return stock ? stock.name : null;
      },
      set(value) {
        this.filters.stockId = value;
      }
    },
    ...mapGetters([
      'companies',
      'stocks'
    ]),
    ...mapGetters('employeePage', 
    [
      'stocksByCompanyId',
      'fStocksByCompanyId'
    ]) 
  
  }
}
</script>