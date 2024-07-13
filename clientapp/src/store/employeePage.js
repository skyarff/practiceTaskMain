import api from '@/api'


const employeePageModule = {
    namespaced: true,
    state: {
        stocksByCompanyId: [{ name: 'Все склады', stockId: null }],
        fStocksByCompanyId: [{ name: 'Все склады', stockId: null }]
    },
    getters: {
        stocksByCompanyId(state) {
            return state.stocksByCompanyId;
        },
        fStocksByCompanyId(state) {
            return state.fStocksByCompanyId;
        },
    },
    mutations: {
        setStocksByCompanyId(state, stocksByCompanyId) {
            state.stocksByCompanyId = stocksByCompanyId
        },
        setFStocksByCompanyId(state, stocksByCompanyId) {
            state.fStocksByCompanyId = stocksByCompanyId
        }
    },
    actions: {
        async getStocksByCompanyId({commit}, payload) {
            
            if (payload.selected) commit('setStocksByCompanyId', []);    
              else commit('setFStocksByCompanyId', []);

            let url = `/api/Stock/getByCompanyId?companyId=${payload.companyId}`;

            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const stocksByCompanyId = response.data.result.map(stock => ({
                name: stock.name,
                stockId: stock.stockId,
              }));
              stocksByCompanyId.unshift({ name: 'Все склады', stockId: null });

              if (payload.selected) commit('setStocksByCompanyId', stocksByCompanyId);    
              else commit('setFStocksByCompanyId', stocksByCompanyId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
    }
}

export default employeePageModule;