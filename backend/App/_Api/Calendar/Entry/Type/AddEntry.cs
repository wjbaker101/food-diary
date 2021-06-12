using System;

namespace FoodDiaryApi.App._Api.Calendar.Entry.Type
{
    public sealed class AddEntryRequest
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime At { get; init; }
        public int DaySpan { get; init; }
    }

    public sealed class AddEntryResponse
    {
        public Guid Reference { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime At { get; init; }
        public DateTime CreatedAt { get; init; }
        public int DaySpan { get; init; }
    }
}