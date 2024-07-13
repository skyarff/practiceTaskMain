<template>
    <v-container fluid>
      
      <!-- Секция таблицы -->
      <v-card v-if="!isLoading" class="mb-4">
        <v-data-table
          :headers="headers"
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
              <td>{{ getBillName(item.billId) }}</td>
              <td>{{ getUpdName(item.updId) }}</td>
              <td>{{ getProductCategoryName(item.productCategoryId) }}</td>
              <td>{{ getCompanyName(item.companyId) }}</td>
              <td>{{ getStockName(item.stockId) }}</td>
              <td>{{ item.rackCode }}</td>
              <td>{{ item.shelfCode }}</td>
              <td>{{ getEmployeeName(item.employeeId) }}</td>
              <td>{{ getProviderName(item.providerId) }}</td>
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
                  class="mr-2"
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

                <v-card  color="grey-lighten-4">
                  <v-card-title>
                    Поля
                  </v-card-title>
                  <v-card-subtitle>
                    Выберите вхождение или диапазон
                  </v-card-subtitle>
                  <v-card-text>
                    <v-row>
              <v-col cols="3">
                <v-text-field
                  label="ID продукта"
                  v-model="filters.productId"
                  type="number"
                  prepend-icon="mdi-identifier"
                ></v-text-field>
              </v-col>

              <v-col cols="3">
                      <v-text-field
                        label="Полка"
                        v-model="filters.shelfCode"
                        prepend-icon="mdi-factory"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="6">
                          <v-text-field
                            label="Наименование продукта*"
                            v-model="filters.name"
                            prepend-icon="mdi-shape-outline"
                          ></v-text-field>
                        </v-col>
                </v-row>

                <v-row>
                  <v-col cols="6">
                    <v-text-field
                      label="Минимальная цена"
                      v-model="filters.lowerPriceLimit"
                      type="number"
                      prepend-icon="mdi-currency-usd"
                    ></v-text-field>
                  </v-col>

                  <v-col cols="6">
                    <v-text-field
                      label="Максимальная цена"
                      type="number"
                      v-model="filters.upperPriceLimit"
                      prepend-icon="mdi-currency-usd"
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

              <v-col cols="6">
              <v-card  color="teal-lighten-5"> 
              <v-card-title>
                Иерархические сущности
              </v-card-title>
              <v-card-subtitle>
                Выберите один из предложенных вариантов
              </v-card-subtitle>

                  <v-card-text>
                    <v-row>
                        <v-col cols="6">
                          <v-select
                            v-model="filtersCompanyId"
                            :items="companies"
                            item-title="name"
                            item-value="companyId"
                            label="Компания"
                            prepend-icon="mdi-domain"
                            dense
                            @update:modelValue="selectionOfStocksAndProductCategoriesAndBills(false)"
                          ></v-select>
                        </v-col>

                        <v-col cols="6">
                          <v-select
                            v-model="filtersProviderId"
                            :items="providers"
                            item-title="name"
                            item-value="providerId"
                            label="Поставщик"
                            prepend-icon="mdi-domain"
                            dense
                            @update:modelValue="selectionOfBills(false)"
                          ></v-select>
                        </v-col>
                  </v-row>

                  <v-row>
                    <v-col cols="4">
                      <v-select
                        v-model="filtersStockId"
                        :items="fStocksByCompanyId"
                        item-title="name"
                        item-value="stockId"
                        label="Склад"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                        @update:modelValue="selectionOfStorageLocationsAndEmployees(false)"
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="filtersProductCategoryId"
                        :items="fProductCategoriesByCompanyId"
                        item-title="name"
                        item-value="productCategoryId"
                        label="Категория"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="filtersBillId"
                        :items="fBillsByCompanyAndProviderId"
                        item-title="name"
                        item-value="billId"
                        label="Счет"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                        @update:modelValue="selectionOfUpds(false)"
                      ></v-select>
                    </v-col>

                  </v-row>

                  <v-row>
                    <v-col cols="4">
                      <v-select
                        v-model="filtersStorageLocationId"
                        :items="fStorageLocationsByStockId"
                        item-title="name"
                        item-value="storageLocationId"
                        label="Стеллаж"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="filtersEmployeeId"
                        :items="fEmployeesByStockId"
                        item-title="name"
                        item-value="employeeId"
                        label="Сотрудник"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="filtersUpdId"
                        :items="fUpdsByBillId"
                        item-title="name"
                        item-value="updId"
                        label="УПД"
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
                @click="selectedProduct = {}"
              >
                <v-icon>mdi-broom</v-icon>
              </v-btn>
            </v-col>
            <v-spacer />
          <v-col class="mr-15" cols="auto">
            <v-btn
              color="primary"
              @click="saveProduct"
              icon="mdi-plus-circle"
              size="small"
              rounded="circle"
            ></v-btn>
            <v-btn 
              v-if="isEditing"
              class="ml-2"
              color="secondary"
              @click="deleteProduct"
              icon="mdi-delete"
              size="small"
              rounded="circle"
            ></v-btn>
          </v-col>
        </v-row>
        </v-card-title>
        <v-card-text>
          <div v-if="selectedProduct.productId !== undefined || isEditing">
              <v-card color="grey-lighten-4">
                <v-card-text class=pa-5>
                  <v-row>
                    <v-col cols="2">
                      <v-text-field
                        label="ID продукта"
                        v-model="selectedProduct.productId"
                        type="number"
                        prepend-icon="mdi-identifier"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="3">
                      <v-text-field
                        label="Наименование продукта"
                        v-model="selectedProduct.name"
                        prepend-icon="mdi-shape-outline"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="3">
                      <v-text-field
                        label="Производитель"
                        v-model="selectedProduct.manufacturer"
                        prepend-icon="mdi-factory"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="2">
                      <v-text-field
                        label="Цена продукта"
                        v-model="selectedProduct.price"
                        prepend-icon="mdi-currency-usd"
                      ></v-text-field>
                    </v-col>
                  
                    <v-col cols="2" >
                      <v-file-input
                        v-model="selectedProduct.image"
                        label="Фото продукта"
                        accept="image/*"
                        prepend-icon="mdi-image"
                      ></v-file-input>
                    </v-col>

              </v-row>
                  
                  <v-row>
                    <v-col cols="4">
                      <v-text-field
                        label="Заводской номер"
                        v-model="selectedProduct.factoryNumber"
                        prepend-icon="mdi-pound"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="4">
                      <v-text-field
                        label="Произв. артикул"
                        v-model="selectedProduct.productionArticle"
                        prepend-icon="mdi-tag"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4">
                      <v-text-field
                        label="Внутр. артикул"
                        v-model="selectedProduct.innerArticle"
                        prepend-icon="mdi-tag-outline"
                      ></v-text-field>
                    </v-col>
                  </v-row>
                </v-card-text>
              </v-card>
          </div>
          <div v-else>
                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Наименование продукта*"
                        v-model="selectedProduct.name"
                        prepend-icon="mdi-shape-outline"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedProviderId"
                        :items="providers"
                        item-title="name"
                        item-value="providerId"
                        label="Поставщик"
                        prepend-icon="mdi-domain"
                        dense
                        @update:modelValue="selectionOfBills(true)"
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedCompanyId"
                        :items="companies"
                        item-title="name"
                        item-value="companyId"
                        label="Компания"
                        prepend-icon="mdi-domain"
                        dense
                        @update:modelValue="selectionOfStocksAndProductCategoriesAndBills(true)"
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedStockId"
                        :items="stocksByCompanyId"
                        item-title="name"
                        item-value="stockId"
                        label="Склад"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                        @update:modelValue="selectionOfStorageLocationsAndEmployees(true)"
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedProductCategoryId"
                        :items="productCategoriesByCompanyId"
                        item-title="name"
                        item-value="productCategoryId"
                        label="Категория"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedBillId"
                        :items="billsByCompanyAndProviderId"
                        item-title="name"
                        item-value="billId"
                        label="Счет"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                        @update:modelValue="selectionOfUpds(true)"
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedStorageLocationId"
                        :items="storageLocationsByStockId"
                        item-title="name"
                        item-value="storageLocationId"
                        label="Стеллаж"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedEmployeeId"
                        :items="employeesByStockId"
                        item-title="name"
                        item-value="employeeId"
                        label="Сотрудник"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="4">
                      <v-select
                        v-model="selectedUpdId"
                        :items="updsByBillId"
                        item-title="name"
                        item-value="updId"
                        label="УПД"
                        prepend-icon="mdi-package-variant-closed"
                        dense
                      ></v-select>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Полка"
                        v-model="selectedProduct.shelfCode"
                        prepend-icon="mdi-factory"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Производитель"
                        v-model="selectedProduct.manufacturer"
                        prepend-icon="mdi-factory"
                      ></v-text-field>
                    </v-col>

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Произв. артикул"
                        v-model="selectedProduct.productionArticle"
                        prepend-icon="mdi-tag"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row> 
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

                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        label="Цена продукта"
                        v-model="selectedProduct.price"
                        type="number"
                        prepend-icon="mdi-currency-usd"
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <v-row>
                    
                  </v-row>

                  <v-row>
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
                      <v-btn class="mr-4" @click="saveProduct" color="primary" prepend-icon="mdi-plus-circle">
                        Добавить
                      </v-btn>
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

  export default {
    components: {
      Loader
    },
    data() {
      return {
        apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        filters: {},
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
          { title: 'Счет', key: 'billName', align: 'start', sortable: true },
          { title: 'УПД', key: 'updName', align: 'start', sortable: true },
          { title: 'Категория', key: 'productCategoryName', align: 'start', sortable: true },
          { title: 'Компания', key: 'companyName', align: 'start', sortable: true },
          { title: 'Склад', key: 'stockName', align: 'start', sortable: true },
          { title: 'Стеллаж', key: 'rackCode', align: 'start', sortable: true },
          { title: 'Полка', key: 'shelfCode', align: 'start', sortable: true },
          { title: 'Работник', key: 'employeeName', align: 'start', sortable: true },
          { title: 'Поставщик', key: 'providerName', align: 'start', sortable: true },
        ],
        products: [],
        selectedProduct: {},
        isEditing: false,
        isLoading: true
      }
    },
    activated() {
      this.applyFilters();
      this.$store.dispatch('getAllProviders');
      this.$store.dispatch('getAllCompanies');
      this.$store.dispatch('getAllStocks');
      this.$store.dispatch('getAllEmployees');
      this.$store.dispatch('getAllProductCategories');
      this.$store.dispatch('getAllBills');
      this.$store.dispatch('getAllUpds');
    },
    methods: {
      selectionOfStocksAndProductCategoriesAndBills(selected) {
        const companyId = this.selectionArgsLvl2CalcCSP(selected);
        const providerId = this.selectionArgsLvl2CalcPB(selected);
        
        this.$store.dispatch( 'productPage/getStocksByCompanyId', {companyId: companyId, selected: selected})
        this.$store.dispatch( 'productPage/getProductCategoriesByCompanyId', {companyId: companyId, selected: selected})
        this.$store.dispatch( 'productPage/getBillsByCompanyAndProviderId', {companyId: companyId, selected: selected, providerId: providerId})
      },
      selectionOfBills(selected) {
        const providerId = this.selectionArgsLvl2CalcPB(selected);
        let companyId;
        if (selected) 
          companyId = this.selectedProduct.companyId  
        else 
          companyId = this.filters.companyId
        
        this.$store.dispatch( 'productPage/getBillsByCompanyAndProviderId', {companyId: companyId, selected: selected, providerId: providerId})
      },
      selectionOfStorageLocationsAndEmployees(selected) {
        const stockId = this.selectionArgsLvl3CalcCSSL(selected);
        
        this.$store.dispatch( 'productPage/getEmployeesByStockId', {stockId: stockId, selected: selected})
        this.$store.dispatch( 'productPage/getStorageLocationsByStockId', {stockId: stockId, selected: selected})
      },
      selectionOfUpds(selected) {
        const billId = this.selectionArgsLvl3CalcPBU(selected);

        this.$store.dispatch( 'productPage/getUpdsByBillId', {billId: billId, selected: selected})
      },
      selectionArgsLvl2CalcCSP(selected) {
        this.selectionArgsLvl3CalcCSSL(selected);
        this.selectionArgsLvl3CalcPBU(selected);
        let companyId;
        if (selected) {
          companyId = this.selectedProduct.companyId
          this.selectedStockId = null
          this.selectedProductCategoryId = null  
        } else {
          companyId = this.filters.companyId
          this.filtersStockId = null
          this.filtersProductCategoryId = null
        }
        return companyId
      },
      selectionArgsLvl2CalcPB(selected) {
        this.selectionArgsLvl3CalcPBU(selected);
        let providerId;
        if (selected) {
          providerId = this.selectedProduct.providerId
          this.selectedBillId = null

        } else {
          providerId = this.filters.providerId
          this.filtersBillId = null
        }
        return providerId
      },
      selectionArgsLvl3CalcCSSL(selected) {
        let stockId;
        if (selected) {
          stockId = this.selectedProduct.stockId
          this.selectedEmployeeId = null
          this.selectedStorageLocationId = null
        } else {
          stockId = this.filters.stockId
          this.filtersEmployeeId = null
          this.filtersStorageLocationId = null
        }
        return stockId
      },
      selectionArgsLvl3CalcPBU(selected) {
        let billId;
        if (selected) {
          billId = this.selectedProduct.billId
          this.selectedUpdId = null
        } else {
          billId = this.filters.billId
          this.filtersUpdId = null
        }
        return billId
      },
      navigateProductId(item) {
        this.filters = {productId: item.productId}
        this.isEditing = true
        this.selectedProduct = item
        this.applyFilters();
        window.scrollTo(0, document.body.scrollHeight);
        this.$store.dispatch( 'productPage/getStocksByCompanyId', {companyId: this.selectedProduct.companyId, selected: true})
        this.$store.dispatch( 'productPage/getProductCategoriesByCompanyId', {companyId: this.selectedProduct.productCategoryId, selected: true})
        this.$store.dispatch( 'productPage/getBillsByCompanyAndProviderId', 
        {companyId: this.selectedProduct.companyId, providerId: this.selectedProduct.providerId, selected: true, })
        this.$store.dispatch( 'productPage/getStorageLocationsByStockId', {stockId: this.selectedProduct.stockId, selected: true})
        this.$store.dispatch( 'productPage/getEmployeesByStockId', {stockId: this.selectedProduct.stockId, selected: true})
        this.$store.dispatch( 'productPage/getUpdsByBillId', {billId: this.selectedProduct.billId, selected: true})
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

        if (this.filters.productId)
          data.productId = this.filters.productId
        if (this.filters.name)
          data.name = this.filters.name
        if (this.filters.providerId)
          data.providerId = this.filters.providerId
        if (this.filters.companyId)
          data.companyId = this.filters.companyId
        if (this.filters.stockId)
          data.stockId = this.filters.stockId
        if (this.filters.productCategoryId)
          data.productCategoryId = this.filters.productCategoryId
        if (this.filters.employeeId)
          data.employeeId = this.filters.employeeId
        if (this.filters.updId)
          data.updId = this.filters.updId
        if (this.filters.shelfCode)
          data.shelfCode = this.filters.shelfCode
        if (this.filters.lowerPriceLimit)
          data.lowerPriceLimit = this.filters.lowerPriceLimit
        if (this.filters.upperPriceLimit)
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
        if(this.selectedProduct.shelfCode)
          formData.append('ShelfCode', this.selectedProduct.shelfCode);
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
        
        this.$store.dispatch( 'productPage/getStocksByCompanyId', {companyId: this.selectedProduct.companyId, selected: true})
        this.$store.dispatch( 'productPage/getProductCategoriesByCompanyId', {companyId: this.selectedProduct.productCategoryId, selected: true})
        this.$store.dispatch( 'productPage/getBillsByCompanyAndProviderId', 
        {companyId: this.selectedProduct.companyId, providerId: this.selectedProduct.providerId, selected: true, })
        this.$store.dispatch( 'productPage/getStorageLocationsByStockId', {stockId: this.selectedProduct.stockId, selected: true})
        this.$store.dispatch( 'productPage/getEmployeesByStockId', {stockId: this.selectedProduct.stockId, selected: true})
        this.$store.dispatch( 'productPage/getUpdsByBillId', {billId: this.selectedProduct.billId, selected: true})
      },
      formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleString();
      },
      getStockName(stockId) {
        const stock = this.stocks.find(s => s.stockId === stockId);
        return stock ? stock.name : 'Не указано';
      },
      getCompanyName(companyId) {
        const company = this.companies.find(c => c.companyId === companyId);
        return company ? company.name : 'Не указано';
      },
      getProviderName(providerId) {
        const provider = this.providers.find(p => p.providerId === providerId);
        return provider ? provider.name : 'Не указано';
      },
      getEmployeeName(employeeId) {
        const employee = this.employees.find(e => e.employeeId === employeeId);
        return employee ? employee.name : 'Не указано';
      },
      getProductCategoryName(productCategoryId) {
        const productCategory = this.productCategories.find(pc => pc.productCategoryId === productCategoryId);
        return productCategory ? productCategory.name : 'Не указано';
      },
      getBillName(billId) {
        const bill = this.bills.find(b => b.billId === billId);
        return bill ? bill.name : 'Не указано';
      },
      getUpdName(updId) {
        const upd = this.upds.find(u => u.updId === updId);
        return upd ? upd.name : 'Не указано';
      },
  },
  computed: {
    selectedCompanyId: {
    get() {
      const company = this.companies.find(c => c.companyId === this.selectedProduct.companyId);
      return company ? company.name : null;
    },
    set(value) {
      this.selectedProduct.companyId = value;
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
    selectedStockId: {
      get() {
        const stock = this.stocks.find(s => s.stockId === this.selectedProduct.stockId);
        return stock ? stock.name : null;
      },
      set(value) {
        this.selectedProduct.stockId = value;
      }
    },
    filtersStockId: {
      get() {
        const stock = this.stocks.find(s => s.stockId === this.filters.stockId);
        return stock ? stock.name : null;
      },
      set(value) {
        this.filters.stockId = value;
      }
    },
    selectedProductCategoryId: {
      get() {
        const productCategory = this.productCategories.find(pc => pc.productCategoryId === this.selectedProduct.productCategoryId);
        return productCategory ? productCategory.name : null;
      },
      set(value) {
        this.selectedProduct.productCategoryId = value;
      }
    },
    filtersProductCategoryId: {
      get() {
        const productCategory = this.productCategories.find(pc => pc.productCategoryId === this.filters.productCategoryId);
        return productCategory ? productCategory.name : null;
      },
      set(value) {
        this.filters.productCategoryId = value;
      }
    },
    selectedBillId: {
      get() {
        const bill = this.bills.find(b => b.billId === this.selectedProduct.billId);
        return bill ? bill.name : null;
      },
      set(value) {
        this.selectedProduct.billId = value;
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
    selectedProviderId: {
      get() {
        const provider = this.providers.find(p => p.providerId === this.selectedProduct.providerId);
        return provider ? provider.name : null;
      },
      set(value) {
        this.selectedProduct.providerId = value;
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
    selectedStorageLocationId: {
      get() {
        const storageLocation = this.storageLocationsByStockId.find(sl => sl.storageLocationId === this.selectedProduct.storageLocationId);
        return storageLocation ? storageLocation.name : null;
      },
      set(value) {
        this.selectedProduct.storageLocationId = value;
      }
    },
    filtersStorageLocationId: {
      get() {
        const storageLocation = this.storageLocationsByStockId.find(sl => sl.storageLocationId === this.filters.storageLocationId);
        return storageLocation ? storageLocation.name : null;
      },
      set(value) {
        this.filters.storageLocationId = value;
      }
    },
    selectedEmployeeId: {
      get() {
        const employee = this.employeesByStockId.find(e => e.employeeId === this.selectedProduct.employeeId);
        return employee ? employee.name : null;
      },
      set(value) {
        this.selectedProduct.employeeId = value;
      }
    },
    filtersEmployeeId: {
      get() {
        const employee = this.employeesByStockId.find(e => e.employeeId === this.filters.employeeId);
        return employee ? employee.name : null;
      },
      set(value) {
        this.filters.employeeId = value;
      }
    },
    selectedUpdId: {
      get() {
        const upd = this.updsByBillId.find(u => u.updId === this.selectedProduct.updId);
        return upd ? upd.name : null;
      },
      set(value) {
        this.selectedProduct.updId = value;
      }
    },
    filtersUpdId: {
      get() {
        const upd = this.updsByBillId.find(u => u.updId === this.filters.updId);
        return upd ? upd.name : null;
      },
      set(value) {
        this.filters.updId = value;
      }
    },
    ...mapGetters([
      'companies',
      'stocks',
      'bills',
      'upds',
      'providers',
      'employees',
      'productCategories'
    ]),
    ...mapGetters('productPage', 
    [
      'stocksByCompanyId',
      'fStocksByCompanyId',
      'productCategoriesByCompanyId',
      'fProductCategoriesByCompanyId',
      'billsByCompanyAndProviderId',
      'fBillsByCompanyAndProviderId',
      'storageLocationsByStockId',
      'fStorageLocationsByStockId',
      'employeesByStockId',
      'fEmployeesByStockId',
      'updsByBillId',
      'fUpdsByBillId'
    ]) 
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