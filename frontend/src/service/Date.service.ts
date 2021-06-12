const daysOfWeek = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wednesday',
    'Thursday',
    'Friday',
    'Saturday',
];

const months = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
];

const postfixes = [ 'th', 'st', 'nd', 'rd' ];

class DateService {

    pad(n: number): string {
        return String(n).padStart(2, '0');
    }

    areEqualByDay(date1: Date, date2: Date): boolean {
        return date1.getFullYear() === date2.getFullYear()
            && date1.getMonth() === date2.getMonth()
            && date1.getDate() === date2.getDate();
    }

    areEqualByMonth(date1: Date, date2: Date): boolean {
        return date1.getFullYear() === date2.getFullYear()
            && date1.getMonth() === date2.getMonth();
    }

    justDay(date: Date): Date {
        return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    }

    addDays(date: Date, days: number): Date {
        const newDate = new Date(date);
        newDate.setDate(newDate.getDate() + days);

        return newDate;
    }

    addMonths(date: Date, months: number): Date {
        const newDate = new Date(date);
        newDate.setMonth(newDate.getMonth() + months);

        return newDate;
    }

    ord(n: number): string {
        return n + (n > 0 ? postfixes[(n > 3 && n < 21) || n % 10 > 3 ? 0 : n % 10] : '');
    }

    formatLong(date: Date): string {
        const dayOfWeek = daysOfWeek[date.getDay()];
        const month = months[date.getMonth()];

        return `${dayOfWeek} ${this.ord(date.getDate())} ${month} ${date.getFullYear()}`;
    }

    isToday(date: Date): boolean {
        const today = new Date();

        return date.getFullYear() == today.getFullYear()
            && date.getMonth() == today.getMonth()
            && date.getDate() == today.getDate();
    }

    isTodayWithOffset(date: Date, offsetInDays: number): boolean {
        const today = new Date();
        today.setDate(today.getDate() + offsetInDays);

        return date.getFullYear() == today.getFullYear()
            && date.getMonth() == today.getMonth()
            && date.getDate() == today.getDate();
    }

    getDateDifferenceFromNow(date: Date): string {
        if (this.isToday(date))
            return 'Today';

        if (this.isTodayWithOffset(date, 1))
            return 'Tomorrow';

        if (this.isTodayWithOffset(date, -1))
            return 'Yesterday';

        const differenceInDays = (date.getTime() - Date.now()) / (1000 * 60 * 60 * 24);

        if (differenceInDays < 0)
            return `${Math.abs(differenceInDays).toFixed(0)} days ago`;

        return `in ${differenceInDays.toFixed(0)} days`;
    }

    getMonth(date: Date): string {
        return months[date.getMonth()];
    }
}

export const dateService = new DateService();
