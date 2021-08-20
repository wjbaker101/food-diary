import dayjs from 'dayjs';

import { CookbookRecipe } from '@/type/CookbookRecipe.type';
import { GetRecipesResponse } from '@/api/client/cookbookRecipe/type/GetRecipes.type';
import { AddRecipeRequest, AddRecipeResponse } from '@/api/client/cookbookRecipe/type/AddRecipe.type';

import { apiClient } from '@/api/Api.client';

class CookbookRecipeApiClient {

    public async getRecipes(): Promise<Array<CookbookRecipe> | Error> {
        const response = await apiClient.get<GetRecipesResponse>('/cookbook/recipes');

        if (response instanceof Error)
            return response;

        return response.recipes.map(x => ({
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
        const response = await apiClient.post<AddRecipeResponse>('/cookbook/recipe', request);

        if (response instanceof Error)
            return response;

        return {
            reference: response.reference,
            title: response.title,
            url: response.url,
            imageUrl: response.imageUrl,
            websiteName: response.websiteName,
            description: response.description,
            addedAt: dayjs(response.addedAt),
            alias: response.alias,
        }
    }

    public async removeRecipe(reference: string): Promise<void | Error> {
        const response = await apiClient.delete<any>(`/cookbook/recipe/${reference}`);

        if (response instanceof Error)
            return response;
    }
}

export const cookbookrecipeApi = new CookbookRecipeApiClient();
