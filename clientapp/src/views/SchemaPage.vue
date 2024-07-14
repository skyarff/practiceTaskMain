<template>
    <div class="d-flex flex-column align-center justify-center" style="height: 100%; padding: 40px; margin-top: 1.7%;">
      <v-card class="mb-16" width="60%">
        <v-card-title class="text-h4 font-weight-bold text-center">
          Схема Access
        </v-card-title>
        <v-card-text >
          <v-img
            :src="`${apiBaseUrl}/Images/Common/SchemaAccess.png`"
            contain
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