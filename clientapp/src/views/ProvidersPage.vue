<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="providers"
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
              <td>{{ item.phone }}</td>
              <td>{{ item.inn }}</td>
              <td>{{ item.legalAdress }}</td>
              <td>{{ item.checkingAccount }}</td>
              <td>{{ item.bank }}</td>
              <td>{{ item.bik }}</td>
              <td>{{ item.correspondentAccount }}</td>
              <td>{{ item.managerFullname }}</td>
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
          </v-expansion-panel-title>
          <v-expansion-panel-text class="pt-6">
  
            <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID поставщика"
                  v-model="filters.providerId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Наименование"
                  v-model="filters.name"
                  prepend-icon="mdi-truck-delivery"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Телефон"
                  v-model="filters.phone"
                  prepend-icon="mdi-phone"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ИНН"
                  v-model="filters.inn"
                  prepend-icon="mdi-card-account-details"
                ></v-text-field>
              </v-col>


              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Юр. адресс"
                  v-model="filters.legalAdress"
                  prepend-icon="mdi-map-marker"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Расч. счет"
                  v-model="filters.checkingAccount"
                  prepend-icon="mdi-credit-card"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Банк"
                  v-model="filters.bank"
                  prepend-icon="mdi-bank"
                ></v-text-field>
              </v-col>
              
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="БИК"
                  v-model="filters.bik"
                  prepend-icon="mdi-numeric"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Корр. счет"
                  v-model="filters.correspondentAccount"
                  prepend-icon="mdi-bank-transfer"
                ></v-text-field>
              </v-col>

            </v-row>
            
            <v-row>
              <v-col cols="4" sm="4" md="4">
                <v-text-field
                  label="Почта"
                  v-model="filters.email"
                  prepend-icon="mdi-email"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ФИО менеджера"
                  v-model="filters.correspondentAccount"
                  prepend-icon="mdi-badge-account"
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
                @click="selectedProvider = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
          <div v-if="selectedProvider.providerId !== undefined || isEditing">
            <v-card-title>Редактирование/удаление</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="saveProvider">

                <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID поставщика"
                  v-model="selectedProvider.providerId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Наименование"
                  v-model="selectedProvider.name"
                  prepend-icon="mdi-truck-delivery"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Телефон"
                  v-model="selectedProvider.phone"
                  prepend-icon="mdi-phone"
                ></v-text-field>
              </v-col>
                </v-row>

                <v-row>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="ИНН"
                      v-model="selectedProvider.inn"
                      prepend-icon="mdi-card-account-details"
                    ></v-text-field>
                  </v-col>


                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Юр. адресс"
                      v-model="selectedProvider.legalAdress"
                      prepend-icon="mdi-map-marker"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Расч. счет"
                      v-model="selectedProvider.checkingAccount"
                      prepend-icon="mdi-credit-card"
                    ></v-text-field>
                  </v-col>
                </v-row>

                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Банк"
                      v-model="selectedProvider.bank"
                      prepend-icon="mdi-bank"
                    ></v-text-field>
                  </v-col>
                  
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="БИК"
                      v-model="selectedProvider.bik"
                      prepend-icon="mdi-numeric"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Корр. счет"
                      v-model="selectedProvider.correspondentAccount"
                      prepend-icon="mdi-bank-transfer"
                    ></v-text-field>
                  </v-col>

                </v-row>
            
                <v-row>
                  <v-col cols="4" sm="4" md="4">
                    <v-text-field
                      label="Почта"
                      v-model="selectedProvider.email"
                      prepend-icon="mdi-email"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="ФИО менеджера"
                      v-model="selectedProvider.managerFullname"
                      prepend-icon="mdi-badge-account"
                    ></v-text-field>
                  </v-col>
                </v-row>


                <v-row>
                  <v-col>
                    <v-btn class="mr-4" type="submit" color="primary" prepend-icon="mdi-plus-circle">
                      Редактировать
                    </v-btn>
                    <v-btn @click="deleteProvider" color="teal" prepend-icon="mdi-delete">
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
                <v-form @submit.prevent="saveProvider">

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Наименование*"
                        v-model="selectedProvider.name"
                        prepend-icon="mdi-truck-delivery"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Телефон*"
                        v-model="selectedProvider.phone"
                        prepend-icon="mdi-phone"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="ИНН"
                      v-model="selectedProvider.inn"
                      prepend-icon="mdi-card-account-details"
                    ></v-text-field>
                  </v-col>
                </v-row>

                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Юр. адресс"
                      v-model="selectedProvider.legalAdress"
                      prepend-icon="mdi-map-marker"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Расч. счет"
                      v-model="selectedProvider.checkingAccount"
                      prepend-icon="mdi-credit-card"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Банк"
                      v-model="selectedProvider.bank"
                      prepend-icon="mdi-bank"
                    ></v-text-field>
                  </v-col>
                </v-row>

                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="БИК"
                      v-model="selectedProvider.bik"
                      prepend-icon="mdi-numeric"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="Корр. счет"
                      v-model="selectedProvider.correspondentAccount"
                      prepend-icon="mdi-bank-transfer"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="4" sm="4" md="4">
                    <v-text-field
                      label="Почта"
                      v-model="selectedProvider.email"
                      prepend-icon="mdi-email"
                    ></v-text-field>
                  </v-col>
                </v-row>
            
                <v-row>
                  <v-col cols="12" sm="6" md="4">
                    <v-text-field
                      label="ФИО менеджера"
                      v-model="selectedProvider.managerFullname"
                      prepend-icon="mdi-badge-account"
                    ></v-text-field>
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
          { title: 'Телефон', key: 'phone', align: 'start', sortable: true },
          { title: 'ИНН', key: 'inn', align: 'start', sortable: true },
          { title: 'Юр. адресс', key: 'legalAdress', align: 'start', sortable: true },
          { title: 'Расч. счет', key: 'сheckingAccount', align: 'start', sortable: false },
          { title: 'Банк', key: 'bank', align: 'start', sortable: true },
          { title: 'БИК', key: 'email', bik: 'start', sortable: true },
          { title: 'Корр. счет', key: 'сorrespondentAccount', align: 'start', sortable: true },
          { title: 'ФИО менеджера', key: 'managerFullname', align: 'start', sortable: true },
          { title: 'Почта', key: 'email', align: 'start', sortable: true },
        ],
        providers: [],
        selectedProvider: {},
        isEditing: false,
        isLoading: true
      }
    },
    mounted() {
      this.applyFilters();
    },
    methods: {
      navigateProviderId(item) {
        this.filters = {providerId: item.providerId}
        this.isEditing = true
        this.selectedProvider = item
        this.applyFilters();
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedProvider.providerId = undefined
          }
      },
      async applyFilters() {
        this.isLoading = true;
        const url = '/api/Provider/getProvidersFiltered';

        try {
          const response = await api.post(url, this.filters, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });
          this.providers = Array.from(response.data.result);
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        } finally {
          this.isLoading = false;
        }
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
        try {
          await api.delete(`api/Provider/dellById?providerId=${this.selectedProvider.providerId}`);
          
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        }
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