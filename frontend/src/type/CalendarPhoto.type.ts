import { Dayjs } from 'dayjs';

export interface CalendarPhoto {
    reference: string;
    description: string;
    photoUrl: string;
    date: Dayjs;
    uploadedAt: Dayjs;
}
