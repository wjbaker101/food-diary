import dayjs from 'dayjs';

import { UploadCalendarPhotoRequest, UploadCalendarPhotoResponse } from '@/api/client/calendarPhoto/type/UploadCalendarPhoto.type';
import { CalendarPhoto } from '@/type/CalendarPhoto.type';

import { apiClient } from '@/api/Api.client';

class CalendarPhotoApiClient {

    public async uploadCalendarPhoto(request: UploadCalendarPhotoRequest): Promise<CalendarPhoto | Error> {
        const formData = new FormData();
        formData.set('description', request.description);
        formData.set('photo', request.photo);
        formData.set('date', request.date);

        const response = await apiClient.post<UploadCalendarPhotoResponse>('/calendar/photo', formData);

        if (response instanceof Error)
            return response;

        return {
            reference: response.reference,
            description: response.description,
            photoUrl: response.photoUrl,
            date: dayjs(response.date),
            uploadedAt: dayjs(response.date)
        }
    }
}

export const calendarPhotoApi = new CalendarPhotoApiClient();
