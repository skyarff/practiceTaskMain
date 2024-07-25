import { createStore } from "vuex";
import api from '@/api'
import updPageModule from '@/store/updPage'
import employeePageModule from '@/store/employeePage';
import storageLocationPageModule from '@/store/storageLocationPage'
import productPageModule from '@/store/productPage'
import VueCookies from 'vue-cookies'

export default createStore({
  state: {
    errorMessage: '',
    employeeInfo: null,
    accessToken: '',
    companies: [],
    providers: [],
  },
  getters: {
    companies(state) {
      return state.companies;
    },
    providers(state) {
      return state.providers;
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
    setUserInfo(state) {
      state.employeeInfo = VueCookies.get('employee');
      state.accessToken = VueCookies.get('token');
    }
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
  },
  modules: {
    updPage: updPageModule,
    employeePage: employeePageModule,
    storageLocationPage: storageLocationPageModule,
    productPage: productPageModule
  },
});
