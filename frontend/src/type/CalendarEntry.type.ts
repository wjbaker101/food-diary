import { Dayjs } from 'dayjs';

export interface CalendarEntryExclusion {
    date: Dayjs;
}

export interface CalendarEntry {
    reference: string;
    title: string;
    description: string;
    at: Dayjs;
    createdAt: Dayjs;
    daySpan: number;
    exclusions: Array<CalendarEntryExclusion>;
}
