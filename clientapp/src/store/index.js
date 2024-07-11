import { createStore } from "vuex";
import api from '@/api'
import updPageModule from '@/store/updPage'
import employeePageModule from '@/store/employeePage';
import storageLocationPageModule from '@/store/storageLocationPage'

export default createStore({
  state: {
    errorMessage: '',
    companyId: 3,
    employeeId: 5,
    stockId: 4,
    companies: [],
    providers: [],
    bills: [],
    stocks: []
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
    stocks(state) {
      return state.stocks;
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
    setStocks(state, stocks) {
      state.stocks = stocks
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
  },
  modules: {
    updPage: updPageModule,
    employeePage: employeePageModule,
    storageLocationPage: storageLocationPageModule
  },
});
