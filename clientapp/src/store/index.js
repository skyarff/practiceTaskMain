import { createStore } from "vuex";

export default createStore({
  state: {
    errorMessage: '',
    companyId: 3,
    employeeId: 5,
    stockId: 4,
  },
  getters: {},
  mutations: {
    setErrorMessage(state, message) {
      state.errorMessage = message
    }
  },
  actions: {},
  modules: {},
});
