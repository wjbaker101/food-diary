import dayjs, { Dayjs } from 'dayjs';

import { CalendarEntry } from '@/type/CalendarEntry.type';
import { AddCalendarEntryRequest, AddCalendarEntryResponse } from './type/AddCalendarEntry.type';
import { GetCalendarEntriesResponse } from '@/api/client/calendarEntry/type/GetCalendarEntries.type';

import { apiClient } from '@/api/Api.client';

class CalendarEntryApiClient {

    public async getCalendarEntries(startAt: Dayjs, endAt: Dayjs): Promise<Array<CalendarEntry> | Error> {
        const start = encodeURIComponent(startAt.toISOString());
        const end = encodeURIComponent(endAt.toISOString());

        const response = await apiClient.get<GetCalendarEntriesResponse>(`/calendar/entries/${start}/${end}`);

        if (response instanceof Error)
            return response;

        return response.entries.map(x => ({
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

    public async addCalendarEntry(request: AddCalendarEntryRequest): Promise<CalendarEntry | Error> {
        const response = await apiClient.post<AddCalendarEntryResponse>('/calendar/entry', request);

        if (response instanceof Error)
            return response;

        return {
            reference: response.reference,
            title: response.title,
            description: response.description,
            at: dayjs(response.at),
            createdAt: dayjs(response.createdAt),
            daySpan: response.daySpan,
            exclusions: [],
        };
    }

    public async removeCalendarEntry(reference: string): Promise<void | Error> {
        const response = await apiClient.delete<any>(`/calendar/entry/${reference}`);

        if (response instanceof Error)
            return response;
    }
}

export const calendarEntryApi = new CalendarEntryApiClient();

