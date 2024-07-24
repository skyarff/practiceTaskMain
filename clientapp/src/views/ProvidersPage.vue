<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="providers"
          :page.sync="page"
          :items-per-page="itemsPerPage"
          @update:page="updatePage"
          @update:items-per-page="updateItemsPerPage"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedProvider.providerId === item.providerId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateProviderId(item)" 
              class="navigation-column">
                {{ item.providerId}}
              </td>
              <td>{{ item.name }}</td>
              <td>{{ item.managerFullname }}</td>
              <td>{{ item.phone }}</td>
              <td>{{ item.email }}</td>
              <td>{{ item.bank }}</td>
              <td>{{ item.checkingAccount }}</td>
              <td>{{ item.correspondentAccount }}</td>
              <td>{{ item.bik }}</td>
              <td>{{ item.inn }}</td>
              <td>{{ item.legalAdress }}</td>
              
              
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
                          label="ID поставщика"
                          v-model="filters.providerId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="Наименование"
                          v-model="filters.name"
                          prepend-icon="mdi-truck-delivery"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="Телефон"
                          v-model="filters.phone"
                          prepend-icon="mdi-phone"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3" >
                        <v-text-field
                          label="Почта"
                          v-model="filters.email"
                          prepend-icon="mdi-email"
                        ></v-text-field>
                      </v-col>
                    </v-row>

                    <v-row>
                      <v-col cols="3">
                        <v-text-field
                          label="Банк"
                          v-model="filters.bank"
                          prepend-icon="mdi-bank"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="Расч. счет"
                          v-model="filters.checkingAccount"
                          prepend-icon="mdi-credit-card"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="Корр. счет"
                          v-model="filters.correspondentAccount"
                          prepend-icon="mdi-bank-transfer"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="БИК"
                          v-model="filters.bik"
                          prepend-icon="mdi-numeric"
                        ></v-text-field>
                      </v-col>

                    </v-row>

                    <v-row>
                      
                      <v-col cols="4">
                        <v-text-field
                          label="ИНН"
                          v-model="filters.inn"
                          prepend-icon="mdi-card-account-details"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="Юр. адресс"
                          v-model="filters.legalAdress"
                          prepend-icon="mdi-map-marker"
                        ></v-text-field>
                      </v-col>
                     
                      <v-col cols="4">
                        <v-text-field
                          label="ФИО менеджера"
                          v-model="filters.managerFullname"
                          prepend-icon="mdi-badge-account"
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
                @click="selectedProvider = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveProvider"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteProvider"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
        <v-card-text>
          <div v-if="selectedProvider.providerId !== undefined || isEditing">

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
                          label="ID поставщика"
                          v-model="selectedProvider.providerId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="Наименование"
                          v-model="selectedProvider.name"
                          prepend-icon="mdi-truck-delivery"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="Телефон"
                          v-model="selectedProvider.phone"
                          prepend-icon="mdi-phone"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="Почта"
                          v-model="selectedProvider.email"
                          prepend-icon="mdi-email"
                        ></v-text-field>
                      </v-col>
                    </v-row>

                    <v-row>
                      <v-col cols="3" >
                        <v-text-field
                          label="Банк"
                          v-model="selectedProvider.bank"
                          prepend-icon="mdi-bank"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3" >
                        <v-text-field
                          label="Расч. счет"
                          v-model="selectedProvider.checkingAccount"
                          prepend-icon="mdi-credit-card"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="3">
                        <v-text-field
                          label="Корр. счет"
                          v-model="selectedProvider.correspondentAccount"
                          prepend-icon="mdi-bank-transfer"
                        ></v-text-field>
                      </v-col>
                      
                      <v-col cols="3">
                        <v-text-field
                          label="БИК"
                          v-model="selectedProvider.bik"
                          prepend-icon="mdi-numeric"
                        ></v-text-field>
                      </v-col>

                    </v-row>

                    <v-row>
                      <v-col cols="4">
                        <v-text-field
                          label="ИНН"
                          v-model="selectedProvider.inn"
                          prepend-icon="mdi-card-account-details"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="Юр. адресс"
                          v-model="selectedProvider.legalAdress"
                          prepend-icon="mdi-map-marker"
                        ></v-text-field>
                      </v-col>

                      <v-col cols="4">
                        <v-text-field
                          label="ФИО менеджера"
                          v-model="selectedProvider.managerFullname"
                          prepend-icon="mdi-badge-account"
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
                        label="Наименование*"
                        v-model="selectedProvider.name"
                        prepend-icon="mdi-truck-delivery"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4">
                      <v-text-field
                        label="Телефон*"
                        v-model="selectedProvider.phone"
                        prepend-icon="mdi-phone"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4">
                    <v-text-field
                      label="Почта"
                      v-model="selectedProvider.email"
                      prepend-icon="mdi-email"
                    ></v-text-field>
                  </v-col>

                    
                </v-row>

                <v-row>
                  <v-col cols="3">
                    <v-text-field
                      label="Банк"
                      v-model="selectedProvider.bank"
                      prepend-icon="mdi-bank"
                    ></v-text-field>
                  </v-col>
                
                  <v-col cols="3">
                    <v-text-field
                      label="Расч. счет"
                      v-model="selectedProvider.checkingAccount"
                      prepend-icon="mdi-credit-card"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="3">
                    <v-text-field
                      label="Корр. счет"
                      v-model="selectedProvider.correspondentAccount"
                      prepend-icon="mdi-bank-transfer"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="3">
                    <v-text-field
                      label="БИК"
                      v-model="selectedProvider.bik"
                      prepend-icon="mdi-numeric"
                    ></v-text-field>
                  </v-col>

                  
                </v-row>

                <v-row>
                  <v-col cols="4">
                    <v-text-field
                      label="ИНН"
                      v-model="selectedProvider.inn"
                      prepend-icon="mdi-card-account-details"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="4">
                    <v-text-field
                      label="Юр. адресс"
                      v-model="selectedProvider.legalAdress"
                      prepend-icon="mdi-map-marker"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="4">
                    <v-text-field
                      label="ФИО менеджера"
                      v-model="selectedProvider.managerFullname"
                      prepend-icon="mdi-badge-account"
                    ></v-text-field>
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
import Loader from '@/components/TableLoader.vue';
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
          { title: 'ID поставщика*', key: 'providerId', align: 'start', sortable: true },
          { title: 'Наименование', key: 'name', align: 'start', sortable: true },
          { title: 'ФИО менеджера', key: 'managerFullname', align: 'start', sortable: true },
          { title: 'Телефон', key: 'phone', align: 'start', sortable: true },
          { title: 'Почта', key: 'email', align: 'start', sortable: true },
          { title: 'Банк', key: 'bank', align: 'start', sortable: true },
          { title: 'Расч. счет', key: 'сheckingAccount', align: 'start', sortable: false },
          { title: 'Корр. счет', key: 'сorrespondentAccount', align: 'start', sortable: true },
          { title: 'БИК', key: 'email', bik: 'start', sortable: true },
          { title: 'ИНН', key: 'inn', align: 'start', sortable: true },
          { title: 'Юр. адресс', key: 'legalAdress', align: 'start', sortable: true },
        ],
        providers: [],
        selectedProvider: {},
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
      navigateProviderId(item) {
        this.filters = {providerId: item.providerId}
        this.isEditing = true
        this.selectedProvider = item
        this.applyFilters();
        this.filters = {}
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedProvider.providerId = undefined
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
        const url = '/api/Provider/getProvidersFiltered';

        const data = {}

        if (this.filters.providerId)
          data.providerId = this.filters.providerId
        if (this.filters.name)
          data.name = this.filters.name
        if (this.filters.phone)
          data.phone = this.filters.phone
        if (this.filters.email)
          data.email = this.filters.email
        if (this.filters.bank)
          data.bank = this.filters.bank
        if (this.filters.checkingAccount)
          data.checkingAccount = this.filters.checkingAccount
        if (this.filters.correspondentAccount)
          data.correspondentAccount = this.filters.correspondentAccount
        if (this.filters.bik)
          data.bik = this.filters.bik
        if (this.filters.inn)
          data.inn = this.filters.inn
        if (this.filters.legalAdress)
          data.legalAdress = this.filters.legalAdress
        if (this.filters.managerFullname)
          data.managerFullname = this.filters.managerFullname


        return new Promise((resolve, reject) => {
          api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          })
          .then(response => {
            this.providers = Array.from(response.data.result);
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
      async saveProvider() {
        const data = {};

        if(this.selectedProvider.providerId)
          data.providerId = this.selectedProvider.providerId
        if(this.selectedProvider.name)
          data.name = this.selectedProvider.name
        if(this.selectedProvider.inn)
          data.inn = this.selectedProvider.inn
        if(this.selectedProvider.legalAdress)
          data.legalAdress = this.selectedProvider.legalAdress
        if(this.selectedProvider.checkingAccount)
          data.checkingAccount = this.selectedProvider.checkingAccount
        if(this.selectedProvider.bank)
          data.bank = this.selectedProvider.bank
        if(this.selectedProvider.bik)
          data.bik = this.selectedProvider.bik
        if(this.selectedProvider.correspondentAccount)
          data.correspondentAccount = this.selectedProvider.correspondentAccount
        if(this.selectedProvider.managerFullname)
          data.managerFullname = this.selectedProvider.managerFullname
        if(this.selectedProvider.email)
          data.email = this.selectedProvider.email
        if(this.selectedProvider.phone)
          data.phone = this.selectedProvider.phone

        try {
          if (this.selectedProvider.providerId !== undefined || this.isEditing) {
            await api.put('api/Provider/update', data, {
              headers: {
                'Content-Type': 'application/json'
              }
            });
          } else {
            await api.post('api/Provider/Create', data, {
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
      async deleteProvider() {

        api.delete(`api/Provider/dellById?providerId=${this.selectedProvider.providerId}`)
        .then(() => this.applyFilters())
        .catch(error => this.$store.commit('setErrorMessage', error))
      },
      async handleRowClick(item) {
        
        if (this.selectedProvider.providerId === item.providerId) {
          delete this.selectedProvider.providerId
          this.isEditing = false
        } else {
          this.selectedProvider = {...item};
          this.isEditing = true
        }
      }
  },
}
</script>