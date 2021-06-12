import { CalendarEntry } from '@/type/CalendarEntry.type';
import { CookbookRecipe } from '@/type/CookbookRecipe.type';

export interface CalendarState {
    entries: Map<string, CalendarEntry>;
}

export interface CookbookState {
    recipes: Array<CookbookRecipe>;
}

export interface AppState {
    calendar: CalendarState;
    cookbook: CookbookState;
}
