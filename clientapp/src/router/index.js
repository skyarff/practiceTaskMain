import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    path: "/CompaniesPage",
    name: "CompaniesPage",
    component: () => import('@/views/CompaniesPage.vue'),
  },
  {
    path: "/StocksPage",
    name: "StocksPage",
    component: () =>
      import("@/views/StocksPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
