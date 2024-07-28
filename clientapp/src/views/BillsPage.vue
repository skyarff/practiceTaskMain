<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="bills"
          :page.sync="page"
          :items-per-page="itemsPerPage"
          @update:page="updatePage"
          @update:items-per-page="updateItemsPerPage"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedBill.billId === item.billId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateBillId(item)" 
              class="navigation-column">
                {{ item.billId}}
              </td>
              <td>{{ item.billNumber }}</td>
              <td>{{ item.billTotal }}</td>
              <td>{{ formatDate(item.createDate) }}</td>
              <td>
                  <div v-if="item.billPdfPath">
                    <v-img 
                    :src="`${apiBaseUrl}//${item.billPdfPath}`"
                    class="full-size-image"
                    style="max-width: 40px; max-height: 40px"
                  ></v-img>
                  <v-tooltip 
                    activator="parent" 
                    location="start"
                    content-class="image-tooltip"
                  >
                    <v-img
                      :src="`${apiBaseUrl}//${item.billPdfPath}`"
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
              v-if="policyA.includes(getEmployeeInfo.Role)"
              >{{ item.companyName }}</td>
              <td>{{ item.providerName }}</td>
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
                          label="ID счета"
                          v-model="filters.billId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="8">
                        <v-text-field
                          label="Номер счета"
                          v-model="filters.billNumber"
                          prepend-icon="mdi-receipt"
                        ></v-text-field>
                      </v-col>
                    </v-row>

                    <v-row>
                      <v-col cols="6">
                        <v-text-field
                          label="От даты и времени"
                          v-model="filters.startDate"
                          type="datetime-local"
                          prepend-icon="mdi-calendar-clock"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="6">
                        <v-text-field
                          label="До даты и времени"
                          v-model="filters.endDate"
                          type="datetime-local"
                          prepend-icon="mdi-calendar-clock"
                        ></v-text-field>
                      </v-col>
                    </v-row>

                    <v-row>
                      <v-col cols="6">
                        <v-text-field
                          label="Минимальная сумма"
                          v-model="filters.lowerBillTotalLimit"
                          type="number"
                          prepend-icon="mdi-currency-usd"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="6">
                        <v-text-field
                          label="Максимальная сумма"
                          v-model="filters.upperBillTotalLimit"
                          type="number"
                          prepend-icon="mdi-currency-usd"
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
                      <v-col 
                      v-if="policyA.includes(getEmployeeInfo.Role)"
                      cols="6">
                            <v-select
                              v-model="filtersCompanyId"                    
                              :items="companies"
                              item-title="name"
                              item-value="companyId"
                              label="Компания"
                              prepend-icon="mdi-domain"
                              dense
                            ></v-select>
                          </v-col>
                      <v-col 
                      :cols="`${policyA.includes(getEmployeeInfo.Role) ? 6 : 12}`"
                      >
                        <v-select
                          v-model="filtersProviderId"                    
                          :items="providers"
                          item-title="name"
                          item-value="providerId"
                          label="Поставщик"
                          prepend-icon="mdi-domain"
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
                @click="selectedBill = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveBill"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteBill"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
        <v-card-text>
          <div v-if="selectedBill.billId !== undefined || isEditing">

            <v-row>
              <v-col cols="12">
                <v-card color="grey-lighten-4">
                  <v-card-title>
                    Удаление
                  </v-card-title>
                  <v-card-subtitle>
                    Укажите Id удаляемого счета
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
                      <v-col cols="12">
                        <v-text-field
                          v-model="selectedBill.billId"
                          label="ID счета"
                          type="number"
                          prepend-icon="mdi-identifier"
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
                      <v-col cols="12">
                      <v-text-field
                        v-model="selectedBill.billNumber"
                        label="Номер счета*"
                        prepend-icon="mdi-receipt"
                      ></v-text-field>
                    </v-col>
                    </v-row>
                    <v-row>
                      <v-col cols="4">
                      <v-file-input
                        v-model="selectedBill.billPdf"
                        label="Скан PDF счета*"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>
                    <v-col cols="8">
                      <v-text-field
                        v-model="selectedBill.billTotal"
                        label="Сумма счета"
                        type="number"
                        prepend-icon="mdi-currency-usd"
                      ></v-text-field>
                    </v-col>
                    </v-row>
                  </v-card-text>
                </v-card>
              </v-col>

              <v-col 
              cols="6"
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
                  v-if="policyA.includes(getEmployeeInfo.Role)" cols="6"
                  >
                    <v-select
                      v-model="selectedCompanyId"                    
                      :items="companies"
                      item-title="name"
                      item-value="companyId"
                      label="Компания"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>
                  </v-col>

                  <v-col 
                  :cols="`${policyA.includes(getEmployeeInfo.Role) ? 6 : 12}`"
                  >
                    <v-select
                      v-model="selectedProviderId"                    
                      :items="providers"
                      item-title="name"
                      item-value="providerId"
                      label="Поставщик"
                      prepend-icon="mdi-domain"
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
          { title: 'ID счета*', key: 'billId', align: 'start', sortable: true },
          { title: 'Номер счёта', key: 'billNumber', align: 'start', sortable: true },
          { title: 'Сумма счета', key: 'billTotal', align: 'start', sortable: false },
          { title: 'Дата добавления', key: 'createDate', align: 'start', sortable: false },
          { title: 'Скан. PDF счета', key: 'billPdfPath', align: 'start', sortable: true },
          { title: 'Провайдер', key: 'providerName', align: 'start', sortable: false },
        ],
        bills: [],
        selectedBill: {},
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

      this.$store.dispatch('getAllProviders');

      if (this.policyA.includes(this.getEmployeeInfo.Role))
        this.$store.dispatch('getAllCompanies');

    },
    deactivated() {
      this.abortFlag = true
      clearTimeout(this.timeoutId);
    },
    methods: {
      updatePage(newPage) {
        this.page = newPage;
      },
      updateItemsPerPage(itemsPerPage) {
        this.itemsPerPage = itemsPerPage;
      },
      navigateBillId(item) {
        this.filters = {billId: item.billId}
        this.isEditing = true
        this.selectedBill = item
        this.applyFilters();
        this.filters = {}
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedBill.billId = undefined
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
        const url = '/api/Bill/getBillsFiltered';

        const data = {}

        if (this.filters.billId)
          data.billId = this.filters.billId
        if (this.filters.billNumber)
          data.billNumber = this.filters.billNumber
        if (this.filters.billNumber)
          data.billNumber = this.filters.billNumber
        if (this.filters.lowerBillTotalLimit)
          data.lowerBillTotalLimit = this.filters.lowerBillTotalLimit
        if (this.filters.upperBillTotalLimit)
          data.upperBillTotalLimit = this.filters.upperBillTotalLimit
        if (this.filters.companyId)
          data.companyId = this.filters.companyId
        if (this.filters.providerId)
          data.providerId = this.filters.providerId
        if (this.filters.startDate)
          data.startDate = new Date(this.filters.startDate).toISOString();
        if (this.filters.endDate)
          data.endDate = new Date(this.filters.endDate).toISOString();


        return new Promise((resolve, reject) => {
          api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          })
          .then(response => {
            this.bills = Array.from(response.data.result);
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
      async saveBill() {
        const formData = new FormData();
        if(this.selectedBill.billId)
          formData.append('BillId', this.selectedBill.billId);
        if(this.selectedBill.billNumber)
          formData.append('BillNumber', this.selectedBill.billNumber);
        if(this.selectedBill.providerId)
          formData.append('ProviderId', this.selectedBill.providerId);
        if(this.selectedBill.companyId)
          formData.append('CompanyId', this.selectedBill.companyId);
        if (this.selectedBill.billPdf)
          formData.append('BillPdf', this.selectedBill.billPdf);
        if (this.selectedBill.billTotal)
          formData.append('BillTotal', this.selectedBill.billTotal);

        try {
          await api.post('api/Bill/Create', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        }
      },
      async deleteBill() {
        api.delete(`/api/Bill/delById?billId=${this.selectedBill.billId}`)
        .then(() => this.applyFilters())
        .catch(error => this.$store.commit('setErrorMessage', error))
      },
      async handleRowClick(item) {
        
        if (this.selectedBill.billId === item.billId) {
          delete this.selectedBill.billId
          this.isEditing = false
        } else {
          this.selectedBill = {...item};
          this.isEditing = true
        }
      },
      formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleString();
      },
  },
  computed: {
    selectedProviderId: {
    get() {
      const provider = this.providers.find(p => p.providerId === this.selectedBill.providerId);
      return provider ? provider.providerId : null;
    },
    set(value) {
      this.selectedBill.providerId = value;
    }
    },
    selectedCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.selectedBill.companyId);
      return company ? company.name : null;
    },
    set(value) {
      this.selectedBill.companyId = value;
    }
    },
    filtersProviderId: {
    get() {
      const provider = this.providers.find(p => p.providerId === this.filters.providerId);
      return provider ? provider.providerId : null;
    },
    set(value) {
      this.filters.providerId = value;
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
    ...mapGetters([
      'companies',
      'providers'
    ]),
    getEmployeeInfo() {
        return this.$store.state.employeeInfo
      },
      policyA() {
        return this.$store.state.policyA
      },
      headers() {
      if (this.policyA.includes(this.getEmployeeInfo.Role)) {
        this.baseHeaders.splice(5, 0, { 
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


