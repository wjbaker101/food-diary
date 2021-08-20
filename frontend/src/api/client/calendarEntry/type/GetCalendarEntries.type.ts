export interface CalendarEntryExclusion {
    date: string;
}

export interface CalendarEntry {
    reference: string;
    title: string;
    description: string;
    at: string;
    createdAt: string;
    daySpan: number;
    exclusions: Array<CalendarEntryExclusion>;
}

export interface GetCalendarEntriesResponse {
    entries: Array<CalendarEntry>;
}
