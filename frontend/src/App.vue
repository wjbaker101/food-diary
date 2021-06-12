<template>
    <HeaderComponent />
    <router-view></router-view>
    <ModalComponent />
</template>

<script lang="ts">
import HeaderComponent from '@/component/Header.component.vue';
import ModalComponent from '@/component/Modal.component.vue';

export default {
    name: 'App',

    components: {
        HeaderComponent,
        ModalComponent,
    },
}
</script>

<style lang="scss">
@import './style/main.scss';
</style>