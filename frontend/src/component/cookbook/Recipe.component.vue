<template>
    <article class="recipe-component">
        <a :href="recipe.url" target="_blank" rel="noopener noreferrer">
            <div class="image-container flex">
                <div class="remove" @click.stop="onRemove">
                    <RemoveIcon />
                </div>
                <div class="blurred-image" :style="{ 'background-image': `url(${recipe.imageUrl})` }"></div>
                <img class="flex-self-middle flex-auto" :src="recipe.imageUrl">
            </div>
            <div class="title-container">
                <h3 v-html="displayTitle"></h3>
            </div>
        </a>
    </article>
</template>

<script lang="ts">
import { computed, PropType } from 'vue';
import { useStore } from 'vuex';

import RemoveIcon from '@/component/icon/CrossIcon.vue';

import { AppStoreKey } from '@/store/AppStoreKey';
import { CookbookRecipe } from '@/type/CookbookRecipe.type';
import { AppState } from '@/store/type/AppState.type';

interface Props {
    recipe: CookbookRecipe;
}

export default {
    name: 'RecipeComponent',

    props: {
        recipe: {
            type: Object as PropType<CookbookRecipe>,
            required: true,
        },
    },

    components: {
        RemoveIcon,
    },

    setup(props: Props) {
        const store = useStore<AppState>();

        const displayTitle = computed<string>(() => props.recipe.alias ?? props.recipe.title);

        return {
            displayTitle,

            onClick() {
                window.open(props.recipe.url, '_blank', 'noopener,noreferrer');
            },

            onRemove() {
                store.dispatch(AppStoreKey.REMOVE_COOKBOOK_RECIPE, props.recipe.reference);
            },
        }
    },
}
</script>

<style lang="scss">
.recipe-component {
    border-radius: BORDER_RADIUS();
    overflow: hidden;
    cursor: pointer;

    &:hover {
        .image-container .remove {
            opacity: 1;

            &:hover {
                color: COLOUR('white');
            }
        }
    }

    a {
        color: inherit;
        text-decoration: none;
    }

    .image-container {
        position: relative;
        height: 270px;
        padding: 1rem;
        overflow: hidden;
        border: 1px solid COLOUR('grey-dark');
        border-bottom: 0;
        border-top-right-radius: BORDER_RADIUS();
        border-top-left-radius: BORDER_RADIUS();

        .remove {
            position: absolute;
            top: 0;
            right: 0;
            padding: 1rem;
            line-height: 0;
            opacity: 0;
            border-bottom-left-radius: BORDER_RADIUS();
            color: COLOUR('error');
            z-index: 2;
            transition: all DURATION('short');

            &:hover {
                background-color: COLOUR('error');
            }
        }

        .blurred-image {
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background-size: cover;
            filter: blur(5px);
        }

        img {
            max-width: 180px;
            max-height: 180px;
            margin: auto;
            position: relative;
            border-radius: BORDER_RADIUS();
            box-shadow: 1px 2px 12px rgba(0, 0, 0, 0.3);
            z-index: 1;
        }
    }

    .title-container {
        width: 100%;
        padding: 0.5rem;
        background-color: COLOUR('primary');
        border: 1px solid COLOUR('primary-dark');
        color: COLOUR('white');
        border-bottom-right-radius: BORDER_RADIUS();
        border-bottom-left-radius: BORDER_RADIUS();
    }

    h3 {
        margin: 0;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
}
</style>
