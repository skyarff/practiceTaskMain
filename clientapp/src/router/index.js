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
    path: "/ProductCategoriesPage",
    name: "ProductCategoriesPage",
    component: () =>
      import("@/views/ProductCategoriesPage.vue"),
  },
  {
    path: "/StorageLocationsPage",
    name: "StorageLocationsPage",
    component: () =>
      import("@/views/StorageLocationsPage.vue"),
  },
  {
    path: "/EmployeesPage",
    name: "EmployeesPage",
    component: () =>
      import("@/views/EmployeesPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
