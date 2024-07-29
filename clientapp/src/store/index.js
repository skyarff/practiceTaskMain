import { createStore } from "vuex";
import api from '@/api'
import updPageModule from '@/store/updPage'
import employeePageModule from '@/store/employeePage';
import storageLocationPageModule from '@/store/storageLocationPage'
import productPageModule from '@/store/productPage'
import VueCookies from 'vue-cookies'



export default createStore({
  state: getInitialState,
  getters: {
    companies(state) {
      return state.companies;
    },
    providers(state) {
      return state.providers;
    },
  },
  mutations: {
    setInitialState(state) {
      state = getInitialState()
    },
    setErrorMessage(state, message) {
      state.errorMessage = message
    },
    setCompanies(state, companies) {
      state.companies = companies
    },
    setProviders(state, providers) {
      state.providers = providers
    },
    setUserInfo(state, employeeInfo) {
      state.employeeInfo = employeeInfo
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
            commit('setErrorMessage', error);
          }
      },
      logout({commit}) {
        commit('setInitialState');
        // VueCookies.remove('employee');
        VueCookies.remove('accessToken');
        VueCookies.remove('refreshToken');
      },
      async setUserInfo({commit, state}) {

        state.accessToken = VueCookies.get('accessToken');
        state.refreshToken = VueCookies.get('refreshToken');

        if (state.accessToken) {
          const url = '/api/Employee/getEmployeeInfo';

          try {
            const response = await api.get(url, {
              headers: {
                'accept': '*/*'
              }
            });

            commit('setUserInfo', response.data.result);

          } catch (error) {
            commit('setErrorMessage', error);
          }
        }
      }

  },
  modules: {
    updPage: updPageModule,
    employeePage: employeePageModule,
    storageLocationPage: storageLocationPageModule,
    productPage: productPageModule
  },
});


function getInitialState() {
  return {
    errorMessage: '',
    employeeInfo: null,
    accessToken: '',
    refreshToken: '',
    companies: [],
    providers: [],
    policyS: ['StockLevelWorker'],
    policySCA: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin'],
    policyCA: ['CompanyLevelWorker', 'Admin'],
    policyA: ['Admin']
  };
}
