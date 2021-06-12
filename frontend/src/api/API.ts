import axios from 'axios';
import dayjs, { Dayjs } from 'dayjs';

import { ApiResultResponse } from '@/api/type/ApiResponse.type';
import { AddCalendarEntryRequest, AddCalendarEntryResponse } from '@/api/type/AddCalendarEntry.type';
import { GetCalendarEntriesResponse } from '@/api/type/GetCalendarEntries.type';
import { UploadCalendarPhotoRequest, UploadCalendarPhotoResponse } from '@/api/type/UploadCalendarPhoto.type';
import { CalendarEntry } from '@/type/CalendarEntry.type';
import { CalendarPhoto } from '@/type/CalendarPhoto.type';
import { CookbookRecipe } from '@/type/CookbookRecipe.type';
import { GetRecipesResponse } from './type/GetRecipes.type';
import { AddRecipeRequest, AddRecipeResponse } from './type/AddRecipe.type';

import { apiClient } from '@/api/client/Api.client';

const client = axios.create({
    baseURL: '/api',
});

class API {

    public async getCalendarEntries(startAt: Dayjs, endAt: Dayjs): Promise<Array<CalendarEntry>> {
        const start = encodeURIComponent(startAt.toISOString());
        const end = encodeURIComponent(endAt.toISOString());

        const response = await client.get<ApiResultResponse<GetCalendarEntriesResponse>>(`/calendar/entries/${start}/${end}`);
        const entries = response.data.result.entries;

        return entries.map(x => ({
            reference: x.reference,
            title: x.title,
            description: x.description,
            at: dayjs(x.at),
            createdAt: dayjs(x.createdAt),
            daySpan: x.daySpan,
            exclusions: x.exclusions.map(x => ({
                date: dayjs(x.date),
            })),
        }));
    }

    public async addCalendarEntry(request: AddCalendarEntryRequest): Promise<CalendarEntry> {
        const response = await client.post<ApiResultResponse<AddCalendarEntryResponse>>('/calendar/entry', request);
        const entry = response.data.result;

        return {
            reference: entry.reference,
            title: entry.title,
            description: entry.description,
            at: dayjs(entry.at),
            createdAt: dayjs(entry.createdAt),
            daySpan: entry.daySpan,
            exclusions: [],
        };
    }

    public async removeCalendarEntry(reference: string): Promise<void> {
        await client.delete<ApiResultResponse<any>>(`/calendar/entry/${reference}`);
    }

    public async uploadCalendarPhoto(request: UploadCalendarPhotoRequest): Promise<CalendarPhoto> {
        const formData = new FormData();
        formData.set('description', request.description);
        formData.set('photo', request.photo);
        formData.set('date', request.date);

        const response = await client.post<ApiResultResponse<UploadCalendarPhotoResponse>>('/calendar/photo', formData);
        const photo = response.data.result;

        return {
            reference: photo.reference,
            description: photo.description,
            photoUrl: photo.photoUrl,
            date: dayjs(photo.date),
            uploadedAt: dayjs(photo.date)
        }
    }

    public async getRecipes(): Promise<Array<CookbookRecipe>> {
        const response = await client.get<ApiResultResponse<GetRecipesResponse>>('/cookbook/recipes');
        const recipes = response.data.result;

        return recipes.recipes.map(x => ({
            reference: x.reference,
            title: x.title,
            url: x.url,
            imageUrl: x.imageUrl,
            websiteName: x.websiteName,
            description: x.description,
            addedAt: dayjs(x.addedAt),
            alias: x.alias,
        }));
    }

    public async addRecipe(request: AddRecipeRequest): Promise<CookbookRecipe | Error> {
        const recipe = await apiClient.post<AddRecipeResponse>('/cookbook/recipe', request);

        if (recipe instanceof Error)
            return recipe;

        return {
            reference: recipe.reference,
            title: recipe.title,
            url: recipe.url,
            imageUrl: recipe.imageUrl,
            websiteName: recipe.websiteName,
            description: recipe.description,
            addedAt: dayjs(recipe.addedAt),
            alias: recipe.alias,
        }
    }

    public async removeRecipe(reference: string): Promise<void> {
        await client.delete<ApiResultResponse<any>>(`/cookbook/recipe/${reference}`);
    }
}

export const api = new API();
