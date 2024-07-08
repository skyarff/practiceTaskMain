<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="filteredHeaders"
          :items="products"
          class="elevation-1 bordered-table"
        >
          <template v-slot:item="{ item }">
            <tr 
            :class="{ 'selected-row': selectedProduct.productId === item.productId }" 
            @click="handleRowClick(item)">
              <td 
              @dblclick="navigateProductId(item)" 
              class="navigation-column">
                {{ item.productId}}
              </td>
              <td>{{ item.name }}</td>
              <td>{{ item.manufacturer }}</td>
              <td>{{ item.productionArticle }}</td>
              <td>{{ item.innerArticle }}</td>
              <td>{{ item.factoryNumber }}</td>
              <td>{{ item.price }}</td>
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
              <td>{{ formatDate(item.createDate) }}</td>
              <td>{{ item.billId }}</td>
              <td>{{ item.updId }}</td>
              <td>{{ item.productCategoryId }}</td>
              <td>{{ item.storageLocationId }}</td>

              <td 
              v-if="!stockId"
              >
                {{ item.employeeId }}</td>
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
                  label="ID продукта"
                  v-model="filters.productId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Наименование продукта"
                  v-model="filters.name"
                  prepend-icon="mdi-shape-outline"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Производитель"
                  v-model="filters.manufacturer"
                  prepend-icon="mdi-factory"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Произв. артикул"
                  v-model="filters.productionArticle"
                  prepend-icon="mdi-tag"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Внутр. артикул"
                  v-model="filters.innerArticle"
                  prepend-icon="mdi-tag-outline"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Заводской номер"
                  v-model="filters.factoryNumber"
                  prepend-icon="mdi-pound"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Минимальная цена"
                  v-model="filters.lowerPriceLimit"
                  type="number"
                  prepend-icon="mdi-currency-usd"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="Максимальная цена"
                  type="number"
                  v-model="filters.upperPriceLimit"
                  prepend-icon="mdi-currency-usd"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID счета"
                  type="number"
                  v-model="filters.billId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID УПД"
                  type="number"
                  v-model="filters.updId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID категории продукта"
                  type="number"
                  v-model="filters.productCategoryId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID места хранения"
                  type="number"
                  v-model="filters.storageLocationId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="12" sm="6" md="4">
                <v-text-field
                  label="ID сотрудника"
                  type="number"
                  v-model="filters.employeeId"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
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
                @click="selectedProduct = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
          <div v-if="selectedProduct.productId !== undefined || isEditing">
            <v-card-title>Редактирование/удаление</v-card-title>
            <v-card-text>
              <v-form @submit.prevent="saveProduct">

                <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID продукта"
                        v-model="selectedProduct.productId"
                        type="number"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Наименование продукта"
                        v-model="selectedProduct.name"
                        prepend-icon="mdi-shape-outline"
                      ></v-text-field>
                    </v-col>


                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Производитель"
                        v-model="selectedProduct.manufacturer"
                        prepend-icon="mdi-factory"
                      ></v-text-field>
                    </v-col>

                  </v-row>
                  
                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Произв. артикул"
                        v-model="selectedProduct.productionArticle"
                        prepend-icon="mdi-tag"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Внутр. артикул"
                        v-model="selectedProduct.innerArticle"
                        prepend-icon="mdi-tag-outline"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Заводской номер"
                        v-model="selectedProduct.factoryNumber"
                        prepend-icon="mdi-pound"
                      ></v-text-field>
                    </v-col>

                  </v-row>

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Цена продукта"
                        v-model="selectedProduct.price"
                        prepend-icon="mdi-currency-usd"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID счета"
                        v-model="selectedProduct.billId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID УПД"
                        v-model="selectedProduct.updId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>             
                  </v-row>

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID категории продуктов"
                        type="number"
                        v-model="selectedProduct.productCategoryId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>   
                     
                    <v-col cols="4" sm="4">
                      <v-file-input
                        v-model="selectedProduct.image"
                        label="Фото продукта"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>
                  </v-row>


                  <v-row>
                  <v-col>
                    <v-btn class="mr-4" type="submit" color="primary" prepend-icon="mdi-plus-circle">
                      Редактировать
                    </v-btn>
                    <v-btn @click="deleteProduct" color="teal" prepend-icon="mdi-delete">
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
                <v-form @submit.prevent="saveProduct">

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Наименование продукта*"
                        v-model="selectedProduct.name"
                        prepend-icon="mdi-shape-outline"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID места хранения*"
                        type="number"
                        v-model="selectedProduct.storageLocationId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Производитель"
                        v-model="selectedProduct.manufacturer"
                        prepend-icon="mdi-factory"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Произв. артикул"
                        v-model="selectedProduct.productionArticle"
                        prepend-icon="mdi-tag"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Внутр. артикул"
                        v-model="selectedProduct.innerArticle"
                        prepend-icon="mdi-tag-outline"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Заводской номер"
                        v-model="selectedProduct.factoryNumber"
                        prepend-icon="mdi-pound"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Цена продукта"
                        v-model="selectedProduct.price"
                        type="number"
                        prepend-icon="mdi-currency-usd"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID счета"
                        type="number"
                        v-model="selectedProduct.billId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID УПД"
                        type="number"
                        v-model="selectedProduct.updId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID категории продукта"
                        type="number"
                        v-model="selectedProduct.productCategoryId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>
                    

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="ID сотрудника"
                        type="number"
                        v-model="selectedProduct.employeeId"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4">
                      <v-file-input
                        v-model="selectedProduct.image"
                        label="Фото продукта"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
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
import store from '@/store/index'

  export default {
    components: {
      Loader
    },
    data() {
      return {
        apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        filters: {},
        defaultFilters: {},
        headers: [
          { title: 'ID продукта*', key: 'productId', align: 'start', sortable: true },
          { title: 'Наименование продутка', key: 'name', align: 'start', sortable: true },
          { title: 'Производитель', key: 'manufacturer', align: 'start', sortable: true },
          { title: 'Произв. артикул', key: 'productionArticle', align: 'start', sortable: true },
          { title: 'Внутр. артикул', key: 'innerArticle', align: 'start', sortable: false },
          { title: 'Заводской номер', key: 'factoryNumber', align: 'start', sortable: true },
          { title: 'Цена', key: 'price', align: 'start', sortable: true },
          { title: 'Фото продукта', key: 'imagePath', align: 'start', sortable: true },
          { title: 'Дата добавления', key: 'createDate', align: 'start', sortable: true },
          { title: 'ID счета*', key: 'billId', align: 'start', sortable: true },
          { title: 'ID УПД*', key: 'updId', align: 'start', sortable: true },
          { title: 'ID категории продукта*', key: 'productCategoryId', align: 'start', sortable: true },
          { title: 'ID места хранения*', key: 'storageLocationId', align: 'start', sortable: true },
          { title: 'ID работника*', key: 'employeeId', align: 'start', sortable: true },
        ],
        products: [],
        selectedProduct: {},
        isEditing: false,
        isLoading: true
      }
    },
    mounted() {
      this.applyFilters();
    },
    created() {
      this.filters.stockId = this.stockId;
      this.defaultFilters.stockId = this.stockId;
    },
    methods: {
      navigateProductId(item) {
        this.filters = {productId: item.productId}
        this.isEditing = true
        this.selectedProduct = item
        this.applyFilters();
        window.scrollTo(0, document.body.scrollHeight);
      },
      switchEditingMode() {
          this.isEditing = !this.isEditing
          if (!this.isEditing) {
            this.selectedProduct.productId = undefined
          }
      },
      async applyFilters() {
        this.isLoading = true;
        const url = '/api/Product/getProductsFiltered';
        const data = {}

        if(this.filters.productId)
          data.productId = this.filters.productId
        if(this.filters.billId)
          data.billId = this.filters.billId
        if(this.filters.updId)
          data.updId = this.filters.updId
        if(this.filters.productCategoryId)
          data.productCategoryId = this.filters.productCategoryId
        if(this.filters.storageLocationId)
          data.storageLocationId = this.filters.storageLocationId
        if(this.filters.employeeId)
          data.employeeId = this.filters.employeeId

        if(this.filters.stockId)
          data.stockId = this.filters.stockId
        if(this.filters.companyId)
          data.companyId = this.filters.companyId
        if(this.filters.providerId)
          data.providerId = this.filters.providerId


        if(this.filters.name)
          data.name = this.filters.name
        if(this.filters.manufacturer)
          data.manufacturer = this.filters.manufacturer
        if(this.filters.productionArticle)
          data.productionArticle = this.filters.productionArticle
        if(this.filters.innerArticle)
          data.innerArticle = this.filters.innerArticle
        if(this.filters.factoryNumber)
          data.factoryNumber = this.filters.factoryNumber


        if(this.filters.lowerPriceLimit)
          data.lowerPriceLimit = this.filters.lowerPriceLimit
        if(this.filters.upperPriceLimit)
          data.upperPriceLimit = this.filters.upperPriceLimit

        if (this.filters.startDate)
            data.startDate = new Date(this.filters.startDate).toISOString();
        if(this.filters.endDate)
          data.endDate = new Date(this.filters.endDate).toISOString();
        
        
        try {
          const response = await api.post(url, data, {
            headers: {
              'accept': '*/*',
              'Content-Type': 'application/json'
            }
          });
          this.products = Array.from(response.data.result);
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
      async saveProduct() {
        const formData = new FormData();


        if(this.selectedProduct.productId)
          formData.append('ProductId', this.selectedProduct.productId);
        if(this.selectedProduct.name)
          formData.append('Name', this.selectedProduct.name);
        if(this.selectedProduct.manufacturer)
          formData.append('Manufacturer', this.selectedProduct.manufacturer);
        if(this.selectedProduct.productionArticle)
          formData.append('ProductionArticle', this.selectedProduct.productionArticle);
        if(this.selectedProduct.innerArticle)
          formData.append('InnerArticle', this.selectedProduct.innerArticle);
        if(this.selectedProduct.factoryNumber)
          formData.append('FactoryNumber', this.selectedProduct.factoryNumber);
        if(this.selectedProduct.price)
          formData.append('Price', this.selectedProduct.price);
        if(this.selectedProduct.image)
          formData.append('Image', this.selectedProduct.image);
        if (this.selectedProduct.billId)
          formData.append('BillId', this.selectedProduct.billId);
        if (this.selectedProduct.updId)
          formData.append('UpdId', this.selectedProduct.updId);
        if (this.selectedProduct.productCategoryId)
          formData.append('ProductCategoryId', this.selectedProduct.productCategoryId);
        if (this.selectedProduct.storageLocationId)
          formData.append('StorageLocationId', this.selectedProduct.storageLocationId);
        if (this.selectedProduct.employeeId)
          formData.append('EmployeeId', this.selectedProduct.employeeId);

        try {
          if (this.selectedProduct.productId !== undefined || this.isEditing) {
            await api.put('api/Product/update', formData, {
              headers: {
                'Content-Type': 'multipart/form-data'
              }
            });
          } else {
            await api.post('api/Product/Create', formData, {
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
      async deleteProduct() {
        try {
          await api.delete(`api/Product/dellById?productId=${this.selectedProduct.productId}`);
          
          this.applyFilters();
        } catch (error) {
          this.$store.commit('setErrorMessage', error)
        }
      },
      async handleRowClick(item) {
        
        if (this.selectedProduct.productId === item.productId) {
          delete this.selectedProduct.productId
          this.isEditing = false
        } else {
          this.selectedProduct = {...item};
          this.isEditing = true
        }
      },
      formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleString();
      }
  },
  computed: {
    stockId() {
    return store.state.stockId;
    },
    filteredHeaders() {
      return this.headers.filter(header => {
        if (this.stockId && header.key === 'employeeId') 
          return false;

        return true;
      });
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