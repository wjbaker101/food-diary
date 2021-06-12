import dayjs, { Dayjs } from 'dayjs';

import { CalendarEntry } from '@/type/CalendarEntry.type';

class CalendarEntryService {

    public sortByAtAscending(a: CalendarEntry, b: CalendarEntry): -1 | 0 | 1 {
        const aAt = a.at.year(2000).month(1).date(1);
        const bAt = b.at.year(2000).month(1).date(1);

        if (aAt.isSame(bAt))
            return 0;

        if (aAt.unix() > bAt.unix())
            return 1;

        return -1;
    }

    public isEntryWithinDay(day: Dayjs, entry: CalendarEntry): boolean {
        if (entry.exclusions.some(x => x.date.isSame(day)))
            return false;

        const entryStart = entry.at.startOf('day').unix();

        const entryEnd = entry.at
            .add(entry.daySpan - 1, 'days')
            .endOf('day')
            .unix();

        return day.unix() >= entryStart && day.unix() <= entryEnd;
    }

    public isBeforeDay(entry: CalendarEntry, day: Dayjs): boolean {
        const entryAt = entry.at
            .year(day.year())
            .month(day.month())
            .date(day.date());

        return entryAt.isBefore(dayjs());
    }
}

export const calendarEntryService = new CalendarEntryService();
