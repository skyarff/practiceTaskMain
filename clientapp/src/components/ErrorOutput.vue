<template>
  <v-snackbar
    v-model="localShow"
    :timeout="timeout"
    color="teal-lighten-4"
    elevation="4"
    location="top"
  >
    <div class="d-flex align-center">
      <v-icon
        left
        color="white"
        class="mr-3"
      >
        mdi-alert-circle-outline
      </v-icon>
      <span>{{ errorMessage }}</span>
    </div>
    <template v-slot:actions>
      <v-btn
        color="white"
        text
        @click="clearError"
        class="text-capitalize font-weight-bold"
      >
        Закрыть
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script>
import { mapState, mapMutations } from 'vuex'

export default {
  name: 'ErrorSnackbar',
  props: {
    timeout: {
      type: Number,
      default: 5000
    }
  },
  computed: {
    ...mapState(['errorMessage']),
    localShow: {
      get() {
        return !!this.errorMessage;
      },
      set() {
        this.clearError();
      }
    }
  },
  methods: {
    ...mapMutations({
      clearError: 'setErrorMessage'
    }),
    clearError() {
      this.$store.commit('setErrorMessage', '');
    }
  }
}
</script>
