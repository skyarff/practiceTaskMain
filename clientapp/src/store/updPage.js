import api from '@/api'

const updPageModule = {
    namespaced: true,
    state: {
        billsByProviderAndCompanyId: [{ name: 'Все счета', billId: null }]

    },
    getters: {
        billsByProviderAndCompanyId(state) {
            return state.billsByProviderAndCompanyId;
        },
    },
    mutations: {
        setBillsByProviderAndCompanyId(state, billsByProviderAndCompanyId) {
            state.billsByProviderAndCompanyId = billsByProviderAndCompanyId;
        }   
    },
    actions: {
        async getBillsByProviderAndCompanyId({commit}, payload) {
            commit('setBillsByProviderAndCompanyId', []);

            let url = '/api/Bill/getByProviderAndCompanyId';
            const params = new URLSearchParams();

            if (payload.providerId) 
                params.append('ProviderId', payload.providerId);
            if (payload.companyId) 
                params.append('CompanyId', payload.companyId);
            if (params.toString()) 
                url += `?${params.toString()}`;

         
            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const billsByProviderAndCompanyId = response.data.result.map(bill => ({
                name: bill.billNumber,
                billId: bill.billId,
              }));
              billsByProviderAndCompanyId.unshift({ name: 'Все счета', billId: null });

              commit('setBillsByProviderAndCompanyId', billsByProviderAndCompanyId);
              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
    }
}

export default updPageModule



