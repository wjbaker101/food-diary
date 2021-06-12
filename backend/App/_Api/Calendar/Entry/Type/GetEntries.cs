using System;
using System.Collections.Generic;

namespace FoodDiaryApi.App._Api.Calendar.Entry.Type
{
    public sealed class GetEntriesResponse
    {
        public List<Entry> Entries { get; init; }

        public sealed class Entry
        {
            public Guid Reference { get; init; }
            public DateTime At { get; init; }
            public string Title { get; init; }
            public string Description { get; init; }
            public DateTime CreatedAt { get; init; }
            public int DaySpan { get; init; }
            public List<Exclusion> Exclusions { get; init; }
        }

        public sealed class Exclusion
        {
            public DateTime Date { get; init; }
        }
    }
}