import axios from 'axios';
import store from '@/store/index'


const api = axios.create({
  // baseURL: 'https://jwt-learn-c0174-default-rtdb.europe-west1.firebasedatabase.app', 
});

api.interceptors.request.use((config) => {
//   const token = store.state.auth.userInfo.token;
//   if (!config.url.includes('signUp') && !config.url.includes('signInWithPassword') && token) {
//     config.params = {
//       ...config.params,
//       auth: token,
//     }
//   }
  return config;
});


api.interceptors.response.use((response) => {
  return response
}, async function (error) {

//   const originalRequest = error.config
//   if (error.response.status === 401 & !originalRequest._retry) {
//     originalRequest._retry = true
//     try {
//       const newTokens = await axios.post(
//         `https://securetoken.googleapis.com/v1/token?key=${apiKey}`, {
//           grant_type: 'refresh_token',
//           refresh_token: JSON.parse(localStorage.getItem('userTokens')).refreshToken
//         }
//       )

//       store.state.auth.userInfo.token = newTokens.data.access_token
//       store.state.auth.userInfo.refreshToken = newTokens.data.refresh_token

//       localStorage.setItem('userTokens', JSON.stringify({
//         token: newTokens.data.access_token,
//         refreshToken: newTokens.data.refresh_token,
//       }))
//     } catch (error) {
//       console.log(error)
//       localStorage.removeItem('userTokens');
//       store.state.auth.userInfo.token = '';
//       store.state.auth.userInfo.refreshToken = '';
//     }
//   }

  store.commit('setErrorMessage', error.response.data.message || error.message)
})

export default api;