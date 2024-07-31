<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="upds"
          :page.sync="page"
          :items-per-page="itemsPerPage"
          @update:page="updatePage"
          @update:items-per-page="updateItemsPerPage"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedUpd.updId === item.updId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateUpdId(item)" 
              class="navigation-column">
                {{ item.updId }}
              </td>
              <td>{{ item.documentNumber }}</td>
              <td>{{ formatDate(item.createDate) }}</td>
              <td>
                  <div v-if="item.updPdfPath">
                    <v-img 
                    :src="`${apiBaseUrl}//${item.updPdfPath}`"
                    class="full-size-image"
                    style="max-width: 40px; max-height: 40px"
                  ></v-img>
                  <v-tooltip 
                    activator="parent" 
                    location="start"
                    content-class="image-tooltip"
                  >
                    <v-img
                      :src="`${apiBaseUrl}//${item.updPdfPath}`"
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
              <td>{{ item.providerName }}</td> 
              <td>{{ item.billNumber }}</td>
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
                          label="ID УПД"
                          v-model="filters.updId"
                          type="number"
                          prepend-icon="mdi-identifier"
                        ></v-text-field>
                      </v-col>
                      <v-col cols="8">
                        <v-text-field
                          label="Номер документа"
                          v-model="filters.documentNumber"
                          prepend-icon="mdi-file-document-outline"
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
                      cols="6">
                          <v-select
                              v-model="filtersCompanyId"
                              @update:modelValue="selectionOfBills(false)"
                              :items="companies"
                              item-title="name"
                              item-value="companyId"
                              label="Компания"
                              prepend-icon="mdi-domain"
                              dense
                            ></v-select>   
                          </v-col>

                          <v-col 
                          :cols="`${policyA.includes(getEmployeeInfo.role) ? 6 : 12}`"
                          >
                          <v-select
                          v-model="filtersProviderId"
                          @update:modelValue="selectionOfBills(false)"
                          :items="providers"
                          item-title="name"
                          item-value="providerId"
                          label="Поставщик"
                          prepend-icon="mdi-domain"
                          dense
                        ></v-select>   
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col cols="12">
                        <v-select
                          v-model="filtersBillId"                    
                          :items="fBillsByProviderAndCompanyId"
                          item-title="name"
                          item-value="billId"
                          label="Счет"
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
      <v-card v-if="policyCA.includes(getEmployeeInfo.role)">
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
                @click="selectedUpd = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveUpd"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-3"
              color="secondary"
              @click="deleteUpd"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
        <v-card-text>
          <div v-if="selectedUpd.updId !== undefined || isEditing">

            <v-row>
              <v-col cols="12">
                <v-card color="grey-lighten-4">
                  <v-card-title>
                    Удаление
                  </v-card-title>
                  <v-card-subtitle>
                    Укажите Id удаляемого УПД
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
                      <v-col cols="12" >
                        <v-text-field
                          v-model="selectedUpd.updId"
                          label="ID УПД"
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
                    <v-col cols="12">
                      <v-text-field
                        v-model="selectedUpd.documentNumber"
                        label="Номер документа*"
                        prepend-icon="mdi-file-document-outline"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="12">
                      <v-file-input
                        v-model="selectedUpd.updPdf"
                        label="Скан PDF УПД*"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
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
                     cols="6">
                      <v-select
                      v-model="selectedCompanyId"
                      @update:modelValue="selectionOfBills(true)"
                      :items="companies"
                      item-title="name"
                      item-value="companyId"
                      label="Компания"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>   
                  </v-col>

                    <v-col 
                    :cols="`${policyA.includes(getEmployeeInfo.role) ? 6 : 12}`"
                    >
                      <v-select
                      v-model="selectedProviderId"
                      @update:modelValue="selectionOfBills(true)"
                      :items="providers"
                      item-title="name"
                      item-value="providerId"
                      label="Поставщик"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>   
                  </v-col>
                  </v-row>
                  <v-row>
                    <v-col cols="12">
                      <v-select
                      v-model="selectedBillId"
                      :items="billsByProviderAndCompanyId"
                      item-title="name"
                      item-value="billId"
                      label="Счет"
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
          { title: 'ID УПД*', key: 'updId', align: 'start', sortable: true },
          { title: 'Номер документа', key: 'documentNumber', align: 'start', sortable: true },
          { title: 'Дата добавления', key: 'createDate', align: 'start', sortable: false },
          { title: 'Скан. УПД', key: 'updPdfPath', align: 'start', sortable: true },
          { title: 'Поставщик', key: 'providerName', align: 'start', sortable: false },
          { title: 'Счет', key: 'billName', align: 'start', sortable: false },        
        ],
        upds: [],
        selectedUpd: {},
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

      if (this.policyA.includes(this.getEmployeeInfo.role)) {
        this.$store.dispatch('getAllCompanies');
        this.$store.dispatch('getAllProviders');
      }
      else if (this.policyCA.includes(this.getEmployeeInfo.role)) {
        this.$store.dispatch('getAllProviders');
        this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', {providerId: this.selectedUpd.providerId, companyId: this.getEmployeeInfo.companyId, selected: true})
        this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', {providerId: this.filters.providerId, companyId: this.getEmployeeInfo.companyId, selected: false})
      }
    },
    deactivated() {
      this.abortFlag = true
      clearTimeout(this.timeoutId)
    },
    methods: {
      selectionOfBills(selected) {

        let companyId;
        let providerId;
        if (selected) {
          companyId = this.selectedUpd.companyId
          providerId = this.selectedUpd.providerId
          this.selectedBillId = null
        } else {
          companyId = this.filters.CompanyId
          providerId = this.filters.providerId
          this.filtersBillId = null
        }

        this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', {providerId: providerId, companyId: companyId, selected: selected})
      },
      updatePage(newPage) {
      this.page = newPage;
      },
      updateItemsPerPage(itemsPerPage) {
        this.itemsPerPage = itemsPerPage;
      },
      navigateUpdId(item) {
        this.filters = {updId: item.updId}
        this.isEditing = true
        this.selectedUpd = item
        this.applyFilters();
        this.filters = {}
        window.scrollTo(0, document.body.scrollHeight);

        if (this.policyCA.includes(this.getEmployeeInfo.role))
          this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', 
            {providerId: this.selectedUpd.providerId, companyId: this.selectedUpd.companyId, selected: true})
      },
      switchEditingMode() {

          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedUpd.updId = undefined
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
        const url = '/api/Upd/getUpdsFiltered';

        const data = {}

        if (this.filters.updId)
          data.updId = this.filters.updId
        if (this.filters.documentNumber)
          data.documentNumber = this.filters.documentNumber
        if (this.filters.companyId)
          data.companyId = this.filters.companyId
        if (this.filters.providerId)
          data.providerId = this.filters.providerId
        if (this.filters.billId)
          data.billId = this.filters.billId
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
            this.upds = Array.from(response.data.result);
            resolve();
          })
          .catch(error => {
            this.$store.commit('setErrorMessage', error.response.data.message);
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
      async saveUpd() {
        const formData = new FormData();
        if(this.selectedUpd.updId)
          formData.append('updId', this.selectedUpd.updId);
        if(this.selectedUpd.documentNumber)
          formData.append('DocumentNumber', this.selectedUpd.documentNumber);
        if (this.selectedUpd.updPdf)
          formData.append('UpdPdf', this.selectedUpd.updPdf);
        if (this.selectedUpd.billId)
          formData.append('BillId', this.selectedUpd.billId);

        try {
          await api.post('api/Upd/Create', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error.response.data.message);
        }
      },
      async deleteUpd() {
        api.delete(`/api/Upd/delById?updId=${this.selectedUpd.updId}`)
        .then(() => this.applyFilters())
        .catch(error => this.$store.commit('setErrorMessage', error.response.data.message))
      },
      async handleRowClick(item) {
        
        if (this.selectedUpd.updId === item.updId) {
          delete this.selectedUpd.updId
          this.isEditing = false
        } else {
          this.selectedUpd = {...item};
          this.isEditing = true

          if (this.policyCA.includes(this.getEmployeeInfo.role))
            this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', 
              {providerId: this.selectedUpd.providerId, companyId: this.selectedUpd.companyId, selected: true})
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
      const provider = this.providers.find(p => p.providerId === this.selectedUpd.providerId);
      return provider ? provider.name : null;
    },
    set(value) {
      this.selectedUpd.providerId = value;
    }
    },
    selectedCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.selectedUpd.companyId);
      return company ? company.name : null;
    },
    set(value) {
      this.selectedUpd.companyId = value;
    }
    },
    selectedBillId: {
    get() {
      const bill = this.billsByProviderAndCompanyId.find(b => b.billId === this.selectedUpd.billId);
      return bill ? bill.name : null;
    },
    set(value) {
      this.selectedUpd.billId = value;
    }
    },
    filtersProviderId: {
    get() {
      const provider = this.providers.find(p => p.providerId === this.filters.providerId);
      return provider ? provider.name : null;
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
    filtersBillId: {
    get() {
      const bill = this.fBillsByProviderAndCompanyId.find(b => b.billId === this.filters.billId);
      return bill ? bill.name : null;
    },
    set(value) {
      this.filters.billId = value;
    }
    },
    ...mapGetters([
      'companies',
      'providers',
      'bills'
    ]),
    ...mapGetters('updPage', 
    [
      'billsByProviderAndCompanyId',
      'fBillsByProviderAndCompanyId'
    ]),
    getEmployeeInfo() {
        if (this.$store.state.employeeInfo) 
          return this.$store.state.employeeInfo
        else return {}
      },
      policySCA() {
        return this.$store.state.policySCA
      },
      policyCA() {
        return this.$store.state.policyCA
      },
      policyA() {
        return this.$store.state.policyA
      },
      headers() {
      if (this.policyA.includes(this.getEmployeeInfo.role)) {
          this.baseHeaders.splice(4, 0, { 
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