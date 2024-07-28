import { createRouter, createWebHistory } from "vue-router";
import store from '@/store/index'

const routes = [
  {
    path: "/CompaniesPage",
    name: "CompaniesPage",
    component: () => import('@/views/CompaniesPage.vue'),
    meta: { hasRole: ['Admin']}
  },
  {
    path: "/StocksPage",
    name: "StocksPage",
    component: () =>
      import("@/views/StocksPage.vue"),
    meta: { hasRole: ['CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/ProductCategoriesPage",
    name: "ProductCategoriesPage",
    component: () =>
      import("@/views/ProductCategoriesPage.vue"),
    meta: { hasRole: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/StorageLocationsPage",
    name: "StorageLocationsPage",
    component: () =>
      import("@/views/StorageLocationsPage.vue"),
    meta: { hasRole: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/EmployeesPage",
    name: "EmployeesPage",
    component: () =>
      import("@/views/EmployeesPage.vue"),
    meta: { hasRole: ['CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/ProvidersPage",
    name: "ProvidersPage",
    component: () =>
      import("@/views/ProvidersPage.vue"),
    meta: { hasRole: ['CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/BillsPage",
    name: "BillsPage",
    component: () =>
      import("@/views/BillsPage.vue"),
    meta: { hasRole: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/UpdsPage",
    name: "UpdsPage",
    component: () =>
      import("@/views/UpdsPage.vue"),
    meta: { hasRole: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/ProductsPage",
    name: "ProductsPage",
    component: () =>
      import("@/views/ProductsPage.vue"),
    meta: { hasRole: ['StockLevelWorker', 'CompanyLevelWorker', 'Admin']}
  },
  {
    path: "/AuthPage",
    name: "AuthPage",
    component: () =>
      import("@/views/LogIn.vue"),
  },
  {
    path: "/SchemaPage",
    name: "SchemaPage",
    component: () =>
      import("@/views/SchemaPage.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

function getCurrentUserRole() {
  if (store.state.employeeInfo !== null)
    return store.state.employeeInfo.Role ? store.state.employeeInfo.Role : ''
}

router.beforeResolve((to, from, next) => {
  const userRole = getCurrentUserRole();

  if (to.meta.hasRole) {
    if (to.meta.hasRole.includes(userRole)) {
      next();
    } else {
      next('/SchemaPage');
    }
  } else {
    if (to.path === '/AuthPage' && userRole) {
      next('/SchemaPage');
    } else next();
  }
});


export default router;
