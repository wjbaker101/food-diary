<template>
    <main class="calendar-component flex flex-vertical">
        <nav class="flex flex-auto">
            <div class="nav-left flex" @click="onIncrementMonth(-1)">
                <ChevronLeftIcon class="flex-self-middle" />
            </div>
            <div class="nav-centre flex-auto" :class="{ 'is-not-current-month': !isCurrentMonth }" @click="onReturnToNow">
                <div class="month-message">
                    <strong>{{ displayCurrentMonth }}</strong>
                </div>
                <div class="return-message">Return to Now</div>
            </div>
            <div class="nav-right flex" @click="onIncrementMonth(1)">
                <ChevronRightIcon class="flex-self-middle" />
            </div>
        </nav>
        <header class="flex flex-auto">
            <div>Monday</div>
            <div>Tuesday</div>
            <div>Wednesday</div>
            <div>Thursday</div>
            <div>Friday</div>
            <div>Saturday</div>
            <div>Sunday</div>
        </header>
        <div class="days grid" :class="{ [`rows-${numberOfWeeks}`]: true }">
            <DayComponent
                :key="`day-${day.toISOString()}`"
                v-for="day in calendarDays"
                :day="day"
            />
            <div class="loading-container" v-if="isLoadingEntries">Loading...</div>
        </div>
    </main>
</template>

<script lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useStore } from 'vuex';
import dayjs from 'dayjs';

import DayComponent from '@/component/calendar/Day.component.vue';
import ChevronLeftIcon from '@/component/icon/ChevronLeftIcon.vue';
import ChevronRightIcon from '@/component/icon/ChevronRightIcon.vue';

import { CalendarEntry } from '@/type/CalendarEntry.type';
import { AppState } from '@/store/type/AppState.type';
import { AppStoreKey } from '@/store/AppStoreKey';

import { api } from '@/api/API';

export default {
    name: 'CalendarView',

    components: {
        DayComponent,
        ChevronLeftIcon,
        ChevronRightIcon,
    },

    setup() {
        const store = useStore<AppState>();

        const startOfMonth = ref<dayjs.Dayjs>(dayjs().startOf('month'));
        const endOfMonth = computed<dayjs.Dayjs>(() => startOfMonth.value.endOf('month'));
        const startDay = computed<dayjs.Dayjs>(() => startOfMonth.value.subtract(startOfMonth.value.weekday(), 'days'));
        const numberOfWeeks = computed<number>(() => endOfMonth.value.isBefore(startDay.value.add(7 * 5, 'days')) ? 5 : 6);

        const calendarDays = computed<Array<dayjs.Dayjs>>(() => Array(7 * numberOfWeeks.value).fill(null).map((_, index) => startDay.value.add(index, 'days')));

        const isCurrentMonth = computed<boolean>(() => startOfMonth.value.isSame(dayjs(), 'month'));

        const displayCurrentMonth = computed<string>(() => {
            if (startOfMonth.value.year() !== dayjs().year())
                return `${startOfMonth.value.format('MMMM YYYY')}`;

            return `${startOfMonth.value.format('MMMM')}`;
        });

        const isLoadingEntries = ref<boolean>(false);

        const getCalendarEntries = async function () {
            isLoadingEntries.value = true;

            const calendarEntries = await api.getCalendarEntries(
                calendarDays.value[0],
                calendarDays.value[calendarDays.value.length - 1]);

            isLoadingEntries.value = false;

            const entries = new Map<string, CalendarEntry>();
            calendarEntries.forEach(x => entries.set(x.reference, x));

            store.dispatch(AppStoreKey.SET_CALENDAR_ENTRIES, entries);
        };

        onMounted(async () => {
            await getCalendarEntries();
        });

        return {
            calendarDays,
            displayCurrentMonth,
            numberOfWeeks,
            isCurrentMonth,
            isLoadingEntries,

            async onReturnToNow() {
                startOfMonth.value = dayjs().startOf('month');

                await getCalendarEntries();
            },

            async onIncrementMonth(offset: number) {
                startOfMonth.value = startOfMonth.value.add(offset, 'month');

                await getCalendarEntries();
            },
        }
    },
}
</script>

<style lang="scss">
.calendar-component {
    margin: 1rem;
    border: 1px solid COLOUR('grey');
    border-radius: BORDER_RADIUS();
    background-color: COLOUR('white');
    overflow: hidden;

    header {
        background-color: COLOUR('primary');
        color: COLOUR('white');
        text-align: center;

        div {
            padding: 0 0.5rem;
        }
    }

    .days {
        position: relative;
        grid-template-columns: repeat(7, (100% / 7));

        &.rows-5 {
            grid-template-rows: repeat(5, (100% / 5));
        }

        &.rows-6 {
            grid-template-rows: repeat(6, (100% / 6));
        }

        .loading-container {
            padding: 1rem;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: COLOUR('white');
            box-shadow: 1px 1px 3px rgba(0, 0, 0, 0.3);
        }
    }

    .nav-left,
    .nav-right,
    .nav-centre {
        padding: 0 1rem;
        cursor: pointer;
        text-align: center;
        transition: background-color DURATION('short'), color DURATION('short');
        user-select: none;

        &:hover {
            background-color: COLOUR('grey');
        }
    }

    .nav-left,
    .nav-right {
        color: COLOUR('grey');

        &:hover {
            color: COLOUR('black');
        }
    }

    .nav-centre {
        .return-message {
            opacity: 0;
        }

        .month-message {
            line-height: 1em;
            transform: translateY(0.8rem);
            transition: transform DURATION('short');
        }

        &.is-not-current-month {
            .return-message {
                opacity: 1;
            }

            .month-message {
                transform: translateY(0);
            }
        }
    }
}
</style>