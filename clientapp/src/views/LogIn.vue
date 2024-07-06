<template>
    <v-container fluid class="d-flex justify-center" style="height: 100vh;">
        <!-- <span>{{ $store.state.auth.isLoggedIn }}</span> -->

   <div style="display: flex; flex-direction: column; margin-top: 6%;">
    <v-card 
        class="pa-8 ma-0"
        elevation="16"
        width="70vw"
    >
        <v-card-title>
            Sign In
        </v-card-title>

        <!-- <v-card-subtitle>
            Sign
        </v-card-subtitle> -->

        <v-form 
        ref="signForm"
        @submit.prevent="signIn"
        >
            <v-text-field
                v-model="email"
                label="E-mail"
                :rules="emailRules"
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
                submit
            </v-btn>

            <v-btn @click="clear">
                clear
            </v-btn>

            <router-link 
            to="/signUp"
            class="ml-5"
            style="text-decoration: none;"
            >
                <span class="caption">Don't have an account? Register to sign in.</span>
            </router-link>

        </v-form>
    </v-card>

    

    <v-card
    v-if="errorMessage"
    class="mx-auto mt-3 error-card"
    elevation="16"
    width="70vw"
    style="border-left: 4px solid #a3540f; border-radius: 0;"
  >
        <v-layout class="ma-1 d-flex align-center">
            <v-flex class="xs11 sm11 md11 lg11 xl11 xxl11 d-flex justify-start ml-5">
                <v-icon default class="error-card mr-1">
                    mdi-alert-octagon-outline
                </v-icon>
            <strong style="font-size: 1.4vw;">{{ errorMessage }}</strong>
        </v-flex>
        <v-flex class="xs1 sm1 md1 lg1 xl1 xxl1 d-flex justify-center">
            <v-btn class="error-card" fab text @click="closeErrorMessage">
                <v-icon>
                    mdi-close
                </v-icon>
            </v-btn>
        </v-flex>
        </v-layout>

  </v-card>

  
</div>
</v-container>
</template>

<script>

export default {
    data: () => ({
        showPassword: false,
        email: '',
        password: '',
        emailRules: [
        v => !!v || 'E-mail is required',
        v => /.+@.+\..+/.test(v) || 'E-mail must be valid',
        v => (v && v.length >= 3) || 'Minimum length is 3 characters',
        ],
        passwordRules: [
        v => !!v || 'Password is required',
        v => (v && v.length >= 3) || 'Minimum length is 3 characters',
        ],
    }),
    methods: {
        async signIn() {
            const t = true

            // if (t || this.$refs.signForm.validate()) {

            //     const payLoad = {
            //         email: this.email,
            //         password: this.password,
            //         type: 'signInWithPassword'
            //     }


            //     await this.$store.dispatch('auth/auth', payLoad);

            //     this.$router.push('/items')
            // }
            
        },
        clear() {
            this.showPassword = false
            this.email = ''
            this.password = ''
        },
        closeErrorMessage() {
            // this.$store.state.auth.error = ''
        }
    },
    computed: {
        // loading() {
        //     return this.$store.state.auth.loading
        // },
        // errorMessage() {
        //     return this.$store.state.auth.error
        // },
    }
    
}

</script>

<style scoped>

.error-card {
    background: #ffebcd;
    color: #a3540f
    ;
}

</style>