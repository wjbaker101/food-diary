import { createApp } from 'vue';

import '@/SetupDayJS';

import App from '@/App.vue';
import { appStore } from '@/store/App.store';
import { appRouter } from '@/router/App.router';

createApp(App)
    .use(appRouter)
    .use(appStore)
    .mount('#app');
