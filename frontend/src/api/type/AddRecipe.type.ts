export interface AddRecipeRequest {
    url: string;
    alias: string;
}

export interface AddRecipeResponse {
    reference: string;
    title: string;
    url: string;
    imageUrl: string;
    websiteName: string;
    description: string;
    addedAt: string;
    alias: string;
}
