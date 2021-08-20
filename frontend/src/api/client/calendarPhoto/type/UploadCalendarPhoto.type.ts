export interface UploadCalendarPhotoRequest {
    description: string;
    photo: Blob;
    date: string;
}

export interface UploadCalendarPhotoResponse {
    reference: string;
    description: string;
    photoUrl: string;
    date: string;
    uploadedAt: string;
}
