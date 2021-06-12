import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'

import CalendarView from '@/view/Calendar.view.vue';
import CookbookView from '@/view/Cookbook.view.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        component: CalendarView,
    },
    {
        path: '/cookbook',
        component: CookbookView,
    },
]

const appRouter = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
})

export { appRouter }
