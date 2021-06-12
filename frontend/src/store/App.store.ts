import { createStore } from 'vuex';

import { AppState } from '@/store/type/AppState.type';
import { CalendarEntry } from '@/type/CalendarEntry.type';

import { AppStoreKey as Key } from '@/store/AppStoreKey';
import { api } from '@/api/API';
import { CookbookRecipe } from '@/type/CookbookRecipe.type';

const appStore = createStore<AppState>({

    state: {
        calendar: {
            entries: new Map(),
        },
        cookbook: {
            recipes: [],
        },
    },

    getters: {
        calendar: x => x.calendar,

        cookbook: x => x.cookbook,
    },

    mutations: {

        [Key.SET_CALENDAR_ENTRIES](state: AppState, entries: Map<string, CalendarEntry>) {
            state.calendar.entries = entries;
        },

        [Key.ADD_CALENDAR_ENTRY](state: AppState, entry: CalendarEntry) {
            state.calendar.entries.set(entry.reference, entry);
        },

        [Key.REMOVE_CALENDAR_ENTRY](state: AppState, reference: string) {
            state.calendar.entries.delete(reference);
        },

        [Key.SET_COOKBOOK_RECIPES](state: AppState, recipes: Array<CookbookRecipe>) {
            state.cookbook.recipes = recipes;
        },

        [Key.ADD_COOKBOOK_RECIPE](state: AppState, recipe: CookbookRecipe) {
            state.cookbook.recipes.push(recipe);
        },

        [Key.REMOVE_COOKBOOK_RECIPE](state: AppState, reference: string) {
            state.cookbook.recipes = state.cookbook.recipes
                .filter(x => x.reference !== reference);
        },
    },

    actions: {

        [Key.SET_CALENDAR_ENTRIES]({ commit }, entries: Map<string, CalendarEntry>) {
            commit(Key.SET_CALENDAR_ENTRIES, entries);
        },

        [Key.ADD_CALENDAR_ENTRY]({ commit }, entry: CalendarEntry) {
            commit(Key.ADD_CALENDAR_ENTRY, entry);
        },

        async [Key.REMOVE_CALENDAR_ENTRY]({ commit }, reference: string) {
            await api.removeCalendarEntry(reference);

            commit(Key.REMOVE_CALENDAR_ENTRY, reference);
        },

        [Key.SET_COOKBOOK_RECIPES]({ commit }, recipes: Array<CookbookRecipe>) {
            commit(Key.SET_COOKBOOK_RECIPES, recipes);
        },

        [Key.ADD_COOKBOOK_RECIPE]({ commit }, recipe: CookbookRecipe) {
            commit(Key.ADD_COOKBOOK_RECIPE, recipe);
        },

        async [Key.REMOVE_COOKBOOK_RECIPE]({ commit }, reference: string) {
            await api.removeRecipe(reference);

            commit(Key.REMOVE_COOKBOOK_RECIPE, reference);
        },
    }
});

export { appStore }
