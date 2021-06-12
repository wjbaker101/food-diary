<template>
    <div class="user-message-component" v-if="userMessage !== null">
        {{ userMessage }}
    </div>
</template>

<script lang="ts">
export default {
    name: 'UserMessageComponent',

    props: {
        userMessage: String,
    },
}
</script>

<style lang="scss">
.user-message-component {
    margin-top: 1rem;
    padding: 1rem;
    background-color: transparentize(COLOUR('error'), 0.7);
    border: 1px solid transparentize(COLOUR('error-dark'), 0.7);
    border-left: 5px solid COLOUR('error-dark');
    border-radius: BORDER_RADIUS();
}
</style>