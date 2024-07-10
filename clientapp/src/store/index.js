import { createStore } from "vuex";
import api from '@/api'
import updPageModule from '@/store/updPage'

export default createStore({
  state: {
    errorMessage: '',
    companyId: 3,
    employeeId: 5,
    stockId: 4,
    companies: [],
    providers: [],
    bills: [],
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
  },
  modules: {
    updPage: updPageModule
  },
});
