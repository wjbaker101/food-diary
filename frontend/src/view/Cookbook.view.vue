<template>
    <main class="cookbook-view">
        <div class="content-width">
            <div class="flex flex-middle">
                <h2>Cookbook</h2>
                <div class="flex-auto">
                    <button class="vertical-middle" @click="onAddRecipe">
                        <PlusIcon class="gap-right" />
                        <span>Add Recipe</span>
                    </button>
                </div>
            </div>
            <section class="grid three-columns gap-small">
                <RecipeComponent
                    :key="`recipe-${recipe.reference}`"
                    v-for="recipe in recipes"
                    :recipe="recipe"
                />
            </section>
        </div>
    </main>
</template>

<script lang="ts">
import { computed, onMounted } from 'vue';
import { useStore } from 'vuex';

import PlusIcon from '@/component/icon/PlusIcon.vue';
import RecipeComponent from '@/component/cookbook/Recipe.component.vue';
import AddRecipeModalComponent from '@/component/modal/AddRecipeModal.component.vue';

import { AppState } from '@/store/type/AppState.type';
import { CookbookRecipe } from '@/type/CookbookRecipe.type';
import { api } from '@/api/API';
import { AppStoreKey } from '@/store/AppStoreKey';
import { eventService, OpenModalDetails } from '@/service/Event.service';

export default {
    name: 'CookbookView',

    components: {
        PlusIcon,
        RecipeComponent,
    },

    setup() {
        const store = useStore<AppState>();

        const recipes = computed<Array<CookbookRecipe>>(() => store.getters.cookbook.recipes);

        onMounted(async () => {
            const response = await api.getRecipes();

            store.dispatch(AppStoreKey.SET_COOKBOOK_RECIPES, response);
        });

        return {
            recipes,

            onAddRecipe() {
                eventService.pub('open-modal', <OpenModalDetails>{
                    side: 'left',
                    component: AddRecipeModalComponent,
                    props: {},
                });
            },
        }
    },
}
</script>

<style lang="scss">
</style>