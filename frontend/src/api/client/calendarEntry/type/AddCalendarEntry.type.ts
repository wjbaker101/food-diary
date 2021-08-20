export interface AddCalendarEntryRequest {
    title: string;
    description: string;
    at: string;
    daySpan: number;
}

export interface AddCalendarEntryResponse {
    reference: string;
    title: string;
    description: string;
    at: string;
    createdAt: string;
    daySpan: number;
}
