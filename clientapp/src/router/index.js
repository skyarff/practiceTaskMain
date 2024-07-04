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
  {
    path: "/ProductCategoryPage",
    name: "ProductCategoryPage",
    component: () =>
      import("@/views/ProductCategoryPage.vue"),
  },
  {
    path: "/StorageLocationPage",
    name: "StorageLocationPage",
    component: () =>
      import("@/views/StorageLocationPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
