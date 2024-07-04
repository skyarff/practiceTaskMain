import { createStore } from "vuex";

export default createStore({
  state: {
    errorMessage: '',
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
