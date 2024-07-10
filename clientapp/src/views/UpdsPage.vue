<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
          :items="upds"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedUpd.updId === item.updId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateUpdId(item)" 
              class="navigation-column">
                {{ item.updId}}
              </td>
              <td>{{ item.documentNumber }}</td>
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
              <td>{{ getProviderName(item.providerId) }}</td>
              <td>{{ getCompanyName(item.companyId) }}</td>
              <td>{{ getBillName(item.billId) }}</td>
              <td>{{ formatDate(item.createDate) }}</td>
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
                  label="ID УПД"
                  v-model="filters.updId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Номер документа"
                  v-model="filters.documentNumber"
                  prepend-icon="mdi-file-document-outline"
                ></v-text-field>
              </v-col>
                  

              <v-col cols="4">
                      <v-select
                      v-model="filtersCompanyId"
                      @update:modelValue="this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', 
                      {providerId: filters.providerId, companyId: filters.companyId})"
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

              <v-col cols="4">
                      <v-select
                      v-model="filtersProviderId"
                      @update:modelValue="this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', 
                      {providerId: filters.providerId, companyId: filters.companyId})"
                      :items="providers"
                      item-title="name"
                      item-value="providerId"
                      label="Поставщик"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>   
                  </v-col>

              <v-col cols="4">
                    <v-select
                      v-model="filtersBillId"                    
                      :items="billsByProviderAndCompanyId"
                      item-title="name"
                      item-value="billId"
                      label="Счет"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>
                  </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="От даты и времени"
                  v-model="filters.startDate"
                  type="datetime-local"
                  prepend-icon="mdi-calendar-clock"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="До даты и времени"
                  v-model="filters.endDate"
                  type="datetime-local"
                  prepend-icon="mdi-calendar-clock"
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
                label="Удаление"
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
          </v-row>
          
        </v-card-text>
          <div v-if="selectedUpd.updId !== undefined || isEditing">
            <v-card-title>Удаление</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="saveUpd">
                <v-row>
                  <v-col cols="12" sm="6">
                    <v-text-field
                      v-model="selectedUpd.updId"
                      label="ID УПД"
                      type="number"
                      prepend-icon="mdi-identifier"
                    ></v-text-field>
                  </v-col>
                </v-row>

                <v-row>
                  <v-col>
                    <v-btn @click="deleteUpd" color="secondary" prepend-icon="mdi-delete">
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
                <v-form @submit.prevent="saveUpd">
                  <v-row>
                    <v-col cols="4">
                      <v-text-field
                        v-model="selectedUpd.documentNumber"
                        label="Номер документа*"
                        prepend-icon="mdi-file-document-outline"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4">
                      <v-file-input
                        v-model="selectedUpd.updPdf"
                        label="Скан PDF УПД*"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>
  
                  </v-row>
                  <v-row>

                    <v-col cols="4">
                      <v-select
                      v-model="selectedCompanyId"
                      @update:modelValue="this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', 
                      {providerId: selectedUpd.providerId, companyId: selectedUpd.companyId})"
                      :items="companies"
                      item-title="name"
                      item-value="companyId"
                      label="Компания"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>   
                  </v-col>


                    <v-col cols="4">
                      <v-select
                      v-model="selectedProviderId"
                      @update:modelValue="this.$store.dispatch('updPage/getBillsByProviderAndCompanyId', 
                      {providerId: selectedUpd.providerId, companyId: selectedUpd.companyId})"
                      :items="providers"
                      item-title="name"
                      item-value="providerId"
                      label="Поставщик"
                      prepend-icon="mdi-domain"
                      dense
                    ></v-select>   
                  </v-col>

                  <v-col cols="4">
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
import { mapGetters } from 'vuex';

  export default {
    components: {
      Loader
    },
    data() {
      return {
        apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        filters: {},
        headers: [
          { title: 'ID УПД*', key: 'updId', align: 'start', sortable: true },
          { title: 'Номер документа', key: 'documentNumber', align: 'start', sortable: true },
          { title: 'Скан. УПД', key: 'updPdfPath', align: 'start', sortable: true },
          { title: 'Поставщик', key: 'providerName', align: 'start', sortable: false },
          { title: 'Компания', key: 'companyName', align: 'start', sortable: false },
          { title: 'Счет', key: 'billName', align: 'start', sortable: false },
          { title: 'Дата добавления', key: 'createDate', align: 'start', sortable: false },
        ],
        upds: [],
        selectedUpd: {},
        isEditing: false,
        isLoading: true
      }
    },
    mounted() {
      this.applyFilters();
      this.$store.dispatch('getAllCompanies');
      this.$store.dispatch('getAllProviders');
      this.$store.dispatch('getAllBills');
    },
    methods: {
      async getAllProviders() {
        const url = '/api/Provider/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            this.providers = response.data.result.map(provider => ({
              name: provider.name,
              providerId: provider.providerId.toString(),
            }));

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      navigateUpdId(item) {
        this.filters = {updId: item.updId}
        this.isEditing = true
        this.selectedUpd = item
        this.applyFilters();
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {

          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedUpd.updId = undefined
          }
      },
      async applyFilters() {
        this.isLoading = true;
        const url = '/api/Upd/getUpdsFiltered';
    
        try {
          const response = await api.post(url, this.filters, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });
          this.upds = Array.from(response.data.result);
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
          this.$store.commit('setErrorMessage', error)
        }
      },
      async deleteUpd() {
        try {
          await api.delete(`/api/Upd/delById?updId=${this.selectedUpd.updId}`);
          
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        }
      },
      async handleRowClick(item) {
        
        if (this.selectedUpd.updId === item.updId) {
          delete this.selectedUpd.updId
          this.isEditing = false
        } else {
          this.selectedUpd = {...item};
          this.isEditing = true
        }
      },
      formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleString();
      },
      getProviderName(providerId) {
          const provider = this.providers.find(p => p.providerId === providerId);
          return provider ? provider.name : 'Не указано';
      },
      getCompanyName(companyId) {
        const company = this.companies.find(c => c.companyId === companyId);
        return company ? company.name : 'Не указано';
      },
      getBillName(billId) {
        const bill = this.bills.find(b => b.billId === billId);
        return bill ? bill.name : 'Не указано';
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
      const bill = this.bills.find(b => b.billId === this.selectedUpd.billId);
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
      const bill = this.bills.find(b => b.billId === this.filters.billId);
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
    ...mapGetters('updPage', ['billsByProviderAndCompanyId']) 
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