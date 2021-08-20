export interface Recipe {
    reference: string;
    title: string;
    url: string;
    imageUrl: string;
    websiteName: string;
    description: string;
    addedAt: string;
    alias: string;
}

export interface GetRecipesResponse {
    recipes: Array<Recipe>;
}
