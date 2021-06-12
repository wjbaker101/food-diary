<template>
    <header role="banner" class="flex flex-auto">
        <div class="flex-auto">
            <router-link to="/"><strong>Food Diary</strong></router-link>
        </div>
        <div></div>
        <div class="flex-auto">
            <router-link to="/cookbook">Cookbook</router-link>
        </div>
    </header>
</template>

<script lang="ts">
export default {
    name: 'HeaderComponent',
}
</script>

<style lang="scss">
header[role=banner] {
    padding: 0.25rem 0.5rem;
    background-color: COLOUR('primary');
    color: COLOUR('white');

    a {
        color: inherit;
        text-decoration: none;
    }
}
</style>