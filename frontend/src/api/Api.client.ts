import axios, { AxiosInstance } from 'axios';

import { ApiErrorResponse, ApiResultResponse } from '@/api/type/ApiResponse.type';

class ApiClient {

    private api: AxiosInstance;

    constructor() {
        this.api = axios.create({
            baseURL: '/api',
        });
    }

    private handleError(error: any): Error {
        if (axios.isAxiosError(error)) {
            const errorResult = error.response?.data as ApiErrorResponse;
            return new Error(errorResult.error);
        }

        return new Error('An error occured, please try again later or contact an administrator.');
    }

    public async get<T>(url: string): Promise<T | Error> {
        try {
            const response = await this.api.get<ApiResultResponse<T>>(url);

            return response.data.result;
        }
        catch (error) {
            return this.handleError(error);
        }
    }

    public async post<T>(url: string, data: Object | FormData): Promise<T | Error> {
        try {
            const response = await this.api.post<ApiResultResponse<T>>(url, data);

            return response.data.result;
        }
        catch (error) {
            return this.handleError(error);
        }
    }

    public async put<T>(url: string, data: Object | FormData): Promise<T | Error> {
        try {
            const response = await this.api.put<ApiResultResponse<T>>(url, data);

            return response.data.result;
        }
        catch (error) {
            return this.handleError(error);
        }
    }

    public async patch<T>(url: string, data: Object | FormData): Promise<T | Error> {
        try {
            const response = await this.api.patch<ApiResultResponse<T>>(url, data);

            return response.data.result;
        }
        catch (error) {
            return this.handleError(error);
        }
    }

    public async delete<T>(url: string): Promise<T | Error> {
        try {
            const response = await this.api.delete<ApiResultResponse<T>>(url);

            return response.data.result;
        }
        catch (error) {
            return this.handleError(error);
        }
    }
}

export const apiClient = new ApiClient();
