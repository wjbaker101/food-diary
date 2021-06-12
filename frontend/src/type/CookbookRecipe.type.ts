import { Dayjs } from 'dayjs';

export interface CookbookRecipe {
    reference: string;
    title: string;
    url: string;
    imageUrl: string;
    websiteName: string;
    description: string;
    addedAt: Dayjs;
    alias: string;
}
