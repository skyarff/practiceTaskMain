<template>
    <v-container fluid class="d-flex justify-center" style="height: 100vh;">

   <div style="display: flex; flex-direction: column; margin-top: 6%;">
    <v-card 
        class="pa-8 ma-0"
        elevation="16"
        width="70vw"
    >
        <v-card-title>
            Sign In
        </v-card-title>

        <v-form 
        ref="signForm"
        @submit.prevent="signIn"
        >
            <v-text-field
                v-model="login"
                label="Login"
                :rules="loginRules"
            >
                <template v-slot:prepend>
                    <v-icon>mdi-email</v-icon>
                </template>
            </v-text-field>

            <v-text-field
                v-model="password"
                label="Password"
                :type="showPassword ? 'text' : 'password'"
                :rules="passwordRules"
            >
                <template v-slot:prepend>
                    <v-icon>mdi-lock</v-icon>
                </template>
            </v-text-field>

            <div class="d-flex justify-start align-center mb-6">
                <v-btn
                    icon
                    @click="showPassword = !showPassword"
                    class="mr-1"
                    elevation="0"
                >
                    <v-icon>{{ showPassword ? 'mdi-eye' : 'mdi-eye-off' }}</v-icon>
                </v-btn>
                <span class="caption">Show password</span>
            </div>

            <v-btn
                class="me-4"
                type="submit"
                :loading="loading"
            >
                Войти
            </v-btn>
        </v-form>
    </v-card>

</div>
</v-container>
</template>

<script>
import api from '@/api'


export default {
    data: () => ({
        showPassword: false,
        login: '',
        password: '',
        loginRules: [
        v => !!v || 'E-mail is required',
        v => (v && v.length >= 3) || 'Minimum length is 3 characters',
        ],
        passwordRules: [
        v => !!v || 'Password is required',
        v => (v && v.length >= 3) || 'Minimum length is 3 characters',
        ],
        loading: false
    }),
    methods: {
        async signIn() {
            this.loading = true;
            const url = `/api/Employee/signIn`;
            
            try {
                const response = await api.post(url, {
                    login: this.login,
                    password: this.password
                }, 
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'accept': '*/*'
                    }
                });


                this.$store.commit('setUserInfo', response.data.result);
                this.$router.push('/ProductsPage');
            } catch (error) {
                this.$store.commit('setErrorMessage', error.response.data.message);
            } finally {
                this.loading = false;
            }
        },
    },

}

</script>

<style scoped>

.error-card {
    background: #ffebcd;
    color: #a3540f
    ;
}

</style>