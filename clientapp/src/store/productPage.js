import api from '@/api'


const productPageModule = {
    namespaced: true,
    state: {
        stocksByCompanyId: [{ name: 'Все склады', stockId: null }],
        fStocksByCompanyId: [{ name: 'Все склады', stockId: null }],

        productCategoriesByCompanyId: [{ name: 'Все категории', productCategoryId: null }],
        fProductCategoriesByCompanyId: [{ name: 'Все категории', productCategoryId: null }],

        billsByCompanyAndProviderId: [{ name: 'Все счета', billId: null }],
        fBillsByCompanyAndProviderId: [{ name: 'Все счета', billId: null }],

        storageLocationsByStockId: [{ name: 'Все стеллажи', storageLocationId: null }],
        fStorageLocationsByStockId: [{ name: 'Все стеллажи', storageLocationId: null }],

        employeesByStockId: [{ name: 'Все сотрудники', employeeId: null }],
        fEmployeesByStockId: [{ name: 'Все сотрудники', employeeId: null }],

        updsByBillId: [{ name: 'Все УПД', updId: null }],
        fUpdsByBillId: [{ name: 'Все УПД', updId: null }],

        updsByStockId: [{ name: 'Все УПД', updId: null }],
        fUpdsByStockId: [{ name: 'Все УПД', updId: null }],
    },
    getters: {
        stocksByCompanyId(state) {
            return state.stocksByCompanyId;
        },
        fStocksByCompanyId(state) {
            return state.fStocksByCompanyId;
        },
        productCategoriesByCompanyId(state) {
            return state.productCategoriesByCompanyId;
        },
        fProductCategoriesByCompanyId(state) {
            return state.fProductCategoriesByCompanyId;
        },
        billsByCompanyAndProviderId(state) {
            return state.billsByCompanyAndProviderId;
        },
        fBillsByCompanyAndProviderId(state) {
            return state.fBillsByCompanyAndProviderId;
        },
        storageLocationsByStockId(state) {
            return state.storageLocationsByStockId;
        },
        fStorageLocationsByStockId(state) {
            return state.fStorageLocationsByStockId;
        },
        employeesByStockId(state) {
            return state.employeesByStockId;
        },
        fEmployeesByStockId(state) {
            return state.fEmployeesByStockId;
        },
        updsByBillId(state) {
            return state.updsByBillId;
        },
        fUpdsByBillId(state) {
            return state.fUpdsByBillId;
        },
        updsByBillId(state) {
            return state.updsByStockId;
        },
        fUpdsByBillId(state) {
            return state.fUpdsByStockId;
        },
    },
    mutations: {
        setStocksByCompanyId(state, stocksByCompanyId) {
            state.stocksByCompanyId = stocksByCompanyId
        },
        setFStocksByCompanyId(state, stocksByCompanyId) {
            state.fStocksByCompanyId = stocksByCompanyId
        },
        setProductCategoriesByCompanyId(state, productCategoriesByCompanyId) {
            state.productCategoriesByCompanyId = productCategoriesByCompanyId
        },
        setFProductCategoriesByCompanyId(state, fProductCategoriesByCompanyId) {
            state.fProductCategoriesByCompanyId = fProductCategoriesByCompanyId
        },
        setBillsByCompanyAndProviderId(state, billsByCompanyAndProviderId) {
            state.billsByCompanyAndProviderId = billsByCompanyAndProviderId
        },
        setFBillsByCompanyAndProivderId(state, fBillsByCompanyAndProviderId) {
            state.fBillsByCompanyAndProviderId = fBillsByCompanyAndProviderId
        },
        setStorageLocationsByStockId(state, storageLocationsByStockId) {
            state.storageLocationsByStockId = storageLocationsByStockId
        },
        setFStorageLocationsByStockId(state, fStorageLocationsByStockId) {
            state.fStorageLocationsByStockId = fStorageLocationsByStockId
        },
        setEmployeesByStockId(state, employeesByStockId) {
            state.employeesByStockId = employeesByStockId
        },
        setFEmployeesByStockId(state, fEmployeesByStockId) {
            state.fEmployeesByStockId = fEmployeesByStockId
        },
        setUpdsByBillId(state, updsByBillId) {
            state.updsByBillId = updsByBillId
        },
        setFUpdsByBillId(state, fUpdsByBillId) {
            state.fUpdsByBillId = fUpdsByBillId
        },
        setUpdsByStockId(state, updsByStockId) {
            state.updsByStockId = updsByStockId
        },
        setFUpdsByStockId(state, fUpdsByStockId) {
            state.fUpdsByStockId = fUpdsByStockId
        },
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
        async getProductCategoriesByCompanyId({commit}, payload) {
            if (payload.selected) commit('setProductCategoriesByCompanyId', []);    
              else commit('setFProductCategoriesByCompanyId', []);

            let url = `/api/ProductCategory/getByCompany?companyId=${payload.companyId}`;

            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const productCategoriesByCompanyId = response.data.result.map(productCategory => ({
                name: productCategory.name,
                productCategoryId: productCategory.productCategoryId,
              }));
              productCategoriesByCompanyId.unshift({ name: 'Все категории', productCategoryId: null });

              if (payload.selected) commit('setProductCategoriesByCompanyId', productCategoriesByCompanyId);    
              else commit('setFProductCategoriesByCompanyId', productCategoriesByCompanyId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
        async getBillsByCompanyAndProviderId({commit}, payload) {
            if (payload.selected) commit('setBillsByCompanyAndProviderId', []);    
            else commit('setFBillsByCompanyAndProivderId', []);

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

              const billsByCompanyAndProivderId = response.data.result.map(bill => ({
                name: bill.billNumber,
                billId: bill.billId,
              }));
              billsByCompanyAndProivderId.unshift({ name: 'Все счета', billId: null });

              if (payload.selected) commit('setBillsByCompanyAndProviderId', billsByCompanyAndProivderId);    
              else commit('setFBillsByCompanyAndProivderId', billsByCompanyAndProivderId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
        async getStorageLocationsByStockId({commit}, payload) {
            if (payload.selected) commit('setStorageLocationsByStockId', []);    
            else commit('setFStorageLocationsByStockId', []);

            let url = `/api/StorageLocation/getByStockId?stockId=${payload.stockId}`;

            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const storageLocationsByStockId = response.data.result.map(sl => ({
                name: sl.rackCode,
                storageLocationId: sl.storageLocationId,
              }));
              storageLocationsByStockId.unshift({ name: 'Все стеллажи', storageLocationId: null });

              if (payload.selected) commit('setStorageLocationsByStockId', storageLocationsByStockId);    
              else commit('setFStorageLocationsByStockId', storageLocationsByStockId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
        async getEmployeesByStockId({commit}, payload) {
            if (payload.selected) commit('setEmployeesByStockId', []);    
            else commit('setFEmployeesByStockId', []);

            let url = `/api/Employee/getByStockId?stockId=${payload.stockId}`;

            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const employeesByStockId = response.data.result.map(emp => ({
                name: emp.fullName,
                employeeId: emp.employeeId,
              }));
              employeesByStockId.unshift({ name: 'Все сотрудники', employeeId: null });

              if (payload.selected) commit('setEmployeesByStockId', employeesByStockId);    
              else commit('setFEmployeesByStockId', employeesByStockId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
        async getUpdsByBillId({commit}, payload) {
            if (payload.selected) commit('setUpdsByBillId', []);    
            else commit('setFUpdsByBillId', []);

            let url = `/api/Upd/getByBillId?billId=${payload.billId}`;

            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const updsByBillId = response.data.result.map(upd => ({
                name: upd.documentNumber,
                updId: upd.updId,
              }));
              updsByBillId.unshift({ name: 'Все УПД', updId: null });

              if (payload.selected) commit('setUpdsByBillId', updsByBillId);    
              else commit('setFUpdsByBillId', updsByBillId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
        async getUpdsByStockId({commit}, payload) {
            if (payload.selected) commit('setUpdsByStockId', []);    
            else commit('setFUpdsByStockId', []);

            let url = `/api/Upd/getUpdsByStockId?StockId=${payload.stockId}`;

            try {
                const response = await api.get(url, {
                    headers: {
                        'accept': '*/*',
                    }
                });

              const updsByBillId = response.data.result.map(upd => ({
                name: upd.documentNumber,
                updId: upd.updId,
              }));
              updsByBillId.unshift({ name: 'Все УПД', updId: null });

              if (payload.selected) commit('setUpdsByStockId', updsByBillId);    
              else commit('setFUpdsByStockId', updsByBillId);

              
            } catch (error) {
              console.error('Error fetching stocks by company ID:', error);
            }
        },
        
    }
}

export default productPageModule;