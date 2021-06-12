<template>
    <div class="add-recipe-modal-component">
        <h2>Add Recipe</h2>
        <label>
            <span>Url</span>
            <input type="text" placeholder="example.com/recipe.html" v-model="newRecipeUrl">
        </label>
        <label>
            <span>Recipe Title Alias</span>
            <small>(Leave blank to not give an alias)</small>
            <input type="text" placeholder="Apple Strudel" v-model="newRecipeAlias">
        </label>
        <button @click="onAddRecipe">Add Recipe</button>
        <UserMessageComponent :userMessage="userMessage" />
    </div>
</template>

<script lang="ts">
import { ref } from 'vue';
import { useStore } from 'vuex';

import UserMessageComponent from '@/component/UserMessage.component.vue';

import { AppState } from '@/store/type/AppState.type';
import { AppStoreKey } from '@/store/AppStoreKey';

import { api } from '@/api/API';
import { eventService } from '@/service/Event.service';

export default {
    name: 'AddRecipeModalComponent',

    components: {
        UserMessageComponent,
    },

    setup() {
        const store = useStore<AppState>();

        const userMessage = ref<string | null>(null);

        const newRecipeUrl = ref<string>('');
        const newRecipeAlias = ref<string>('');

        const showMessage = function (message: string) {
            userMessage.value = message;

            setTimeout(() => {
                userMessage.value = null;
            }, 6000);
        };

        return {
            userMessage,
            newRecipeUrl,
            newRecipeAlias,

            async onAddRecipe() {
                if (newRecipeUrl.value.length < 3)
                    return showMessage('The requested URL is invalid. Please enter a new one and try again.');

                const recipe = await api.addRecipe({
                    url: newRecipeUrl.value,
                    alias: newRecipeAlias.value,
                });

                if (recipe instanceof Error)
                    return showMessage(recipe.message);

                store.dispatch(AppStoreKey.ADD_COOKBOOK_RECIPE, recipe);

                eventService.pub('close-modal');
            },
        }
    },
}
</script>

<style lang="scss">
.add-recipe-modal-component {
}
</style>