export interface ApiResultResponse<T> {
    result: T;
    responseAt: string;
}

export interface ApiErrorResponse {
    error: string;
    responseAt: string;
}
