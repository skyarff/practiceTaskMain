<template>
    <div class="d-flex flex-column align-center justify-center" style="height: 100%; padding: 40px; margin-top: 1.7%;">
      <v-card class="mb-16" width="95%" max-width="1200">
        <v-card-title class="text-h4 font-weight-bold text-center py-6">
          Схема Access
        </v-card-title>
        <v-card-text class="pa-6">
          <v-img
            :src="`${apiBaseUrl}/Images/Common/schemaAccess.png`"
            contain
            height="600"
          />
        </v-card-text>
      </v-card>
  
      <v-card width="95%" max-width="1200">
        <v-card-title class="text-h4 font-weight-bold text-center py-6">
          Схема поведения при удалении
        </v-card-title>
        <v-card-text class="pa-6">
          <v-img
            :src="`${apiBaseUrl}/Images/Common/schemaGpt.png`"
            contain
            height="600"
          />
        </v-card-text>
      </v-card>
    </div>
  </template>

<script>
export default {
    data() {
        return {
            apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        }
    }
}
</script>

<style scoped>
.v-card {
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
  transition: all 0.3s;
}
.v-card:hover {
  box-shadow: 0 8px 16px rgba(0,0,0,0.2);
  transform: translateY(-5px);
}
</style>