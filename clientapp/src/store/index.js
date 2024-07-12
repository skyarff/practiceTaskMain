import { createStore } from "vuex";
import api from '@/api'
import updPageModule from '@/store/updPage'
import employeePageModule from '@/store/employeePage';
import storageLocationPageModule from '@/store/storageLocationPage'
import productPageModule from '@/store/productPage'

export default createStore({
  state: {
    errorMessage: '',
    companyId: 3,
    employeeId: 5,
    stockId: 4,
    companies: [],
    providers: [],
    bills: [],
    upds: [],
    stocks: [],
    employees: [],
    productCategories: []
  },
  getters: {
    companies(state) {
      return state.companies;
    },
    providers(state) {
      return state.providers;
    },
    bills(state) {
      return state.bills;
    },
    upds(state) {
      return state.upds;
    },
    stocks(state) {
      return state.stocks;
    },
    employees(state) {
      return state.employees;
    },
    productCategories(state) {
      return state.productCategories;
    },
  },
  mutations: {
    setErrorMessage(state, message) {
      state.errorMessage = message
    },
    setCompanies(state, companies) {
      state.companies = companies
    },
    setProviders(state, providers) {
      state.providers = providers
    },
    setBills(state, bills) {
      state.bills = bills
    },
    setUpds(state, upds) {
      state.upds = upds
    },
    setProductCategories(state, productCategories) {
      state.productCategories = productCategories
    },
    setStocks(state, stocks) {
      state.stocks = stocks
    },
    setEmployees(state, employees) {
      state.employees = employees
    },
  },
  actions: {
      async getAllCompanies({commit}) {
      const url = '/api/Company/getAll';
        try {

          const response = await api.get(url, {
            headers: {
              'accept': '*/*'
            }
          });
  
          const companies = response.data.result.map(company => ({
            name: company.name,
            companyId: company.companyId,
          }));

          companies.unshift({ companyId: null, name: 'Все компании' });
          commit('setCompanies', companies)
          
        } catch (error) {
          // this.$store.commit('setErrorMessage', error);
        }
      },
      async getAllProviders({commit}) {
        const url = '/api/Provider/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            const providers = response.data.result.map(provider => ({
              name: provider.name,
              providerId: provider.providerId,
            }));

            providers.unshift({ name: 'Все поставщики', providerId: null });

            commit('setProviders', providers)

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      async getAllBills({commit}) {

        const url = '/api/Bill/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            const bills = response.data.result.map(bill => ({
              name: bill.billNumber,
              billId: bill.billId,
            }));

            bills.unshift({ name: 'Все счета', billId: null });

            commit('setBills', bills)

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      async getAllStocks({commit}) {

        const url = '/api/Stock/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            const stocks = response.data.result.map(stock => ({
              name: stock.name,
              stockId: stock.stockId,
            }));

            stocks.unshift({ name: 'Все склады', stockId: null });

            commit('setStocks', stocks)

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      async getAllEmployees({commit}) {

        const url = '/api/Employee/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            const employees = response.data.result.map(emp => ({
              name: emp.login,
              employeeId: emp.employeeId,
            }));

            commit('setEmployees', employees)

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      async getAllProductCategories({commit}) {

        const url = '/api/ProductCategory/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            const productCategories = response.data.result.map(pc => ({
              name: pc.name,
              productCategoryId: pc.productCategoryId,
            }));

            productCategories.unshift({ name: 'Все категории', productCategoryId: null });

            commit('setProductCategories', productCategories)

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
      async getAllUpds({commit}) {

        const url = '/api/Upd/getAll';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            const upds = response.data.result.map(upd => ({
              name: upd.documentNumber,
              updId: upd.updId,
            }));

            upds.unshift({ name: 'Все УПД', updId: null });

            commit('setUpds', upds)

          } catch (error) {
            // this.$store.commit('setErrorMessage', error);
          } 
      },
  },
  modules: {
    updPage: updPageModule,
    employeePage: employeePageModule,
    storageLocationPage: storageLocationPageModule,
    productPage: productPageModule
  },
});
