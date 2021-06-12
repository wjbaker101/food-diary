<template>
    <div
        ref="component"
        class="day-component flex flex-vertical"
        :class="{
            'is-today': isToday,
            'is-not-current-month': !isCurrentMonth,
        }"
    >
        <div class="flex-auto">
            {{ date }}
        </div>
        <div class="food-container">
            <div v-if="entriesForDay.length > 0">
                <div
                    class="day-item flex"
                    :class="{ 'is-past': isPast(entry) }"
                    :key="`day-entry-${entry.reference}`"
                    v-for="entry in entriesForDay"
                >
                    <div class="day-content" :title="`${entry.title}: ${entry.description}`">
                        <strong>{{ entry.title }}: </strong>
                        <span>{{ entry.description }}</span>
                    </div>
                    <div class="flex-auto">
                        <small>@ {{ formatEntryTime(entry.at) }}</small>
                    </div>
                </div>
            </div>
        </div>
        <div class="controls flex flex-auto">
            <div></div>
            <div class="flex-auto">
                <SettingsIcon @click="onOpenSettings" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { computed, PropType, ref } from 'vue';
import { useStore } from 'vuex';
import dayjs from 'dayjs';

import SettingsIcon from '@/component/icon/CogIcon.vue';
import EditDayModalComponent from '@/component/modal/EditDayModal.component.vue';

import { AppState } from '@/store/type/AppState.type';
import { CalendarEntry } from '@/type/CalendarEntry.type';
import { EditDayModalProps } from '@/component/modal/EditDayModal.props';

import { eventService, OpenModalDetails } from '@/service/Event.service';
import { calendarEntryService } from '@/service/CalendarEntry.service';

interface Props {
    day: dayjs.Dayjs;
}

export default {
    name: 'DayComponent',

    props: {
        day: {
            type: Object as PropType<dayjs.Dayjs>,
            required: true,
        },
    },

    components: {
        SettingsIcon,
    },

    setup(props: Props) {
        const store = useStore<AppState>();

        const component = ref<HTMLDivElement | null>(null);

        const calendarEntries = computed<Map<string, CalendarEntry>>(() => store.getters.calendar.entries);
        const entriesForDay = computed<Array<CalendarEntry>>(() => {
            return Array.from(calendarEntries.value.values())
                .filter(x => calendarEntryService.isEntryWithinDay(props.day, x))
                .sort(calendarEntryService.sortByAtAscending);
        });

        const date = computed<number>(() => props.day.date());

        const isToday = computed<boolean>(() => props.day.isToday());
        const isCurrentMonth = computed<boolean>(() => props.day.startOf('month').isSame(dayjs().startOf('month')));

        return {
            component,
            entriesForDay,
            date,
            isToday,
            isCurrentMonth,

            onOpenSettings() {
                if (component.value === null)
                    return;

                eventService.pub('open-modal', <OpenModalDetails>{
                    side: 'left',
                    component: EditDayModalComponent,
                    props: <EditDayModalProps>{
                        day: props.day,
                    },
                });
            },

            isPast(entry: CalendarEntry): boolean {
                return calendarEntryService.isBeforeDay(entry, props.day);
            },

            formatEntryTime(entryAt: dayjs.Dayjs): string {
                return entryAt.format('HH:mm');
            },
        }
    },
}
</script>

<style lang="scss">
.day-component {
    font-size: 0.75rem;
    border-right: 1px solid COLOUR('grey');
    border-top: 1px solid COLOUR('grey');

    & > * {
        padding: 0.25rem;
    }

    &.is-not-current-month {
        background-color: COLOUR('grey-light');
    }

    &.is-today {
        background-color: COLOUR('primary');
        color: COLOUR('white');

        .controls {
            .icon {
                &:hover {
                    color: COLOUR('white');
                }
            }
        }
    }

    .day-content {
        white-space: nowrap;
        overflow-x: hidden;
        text-overflow: ellipsis;
    }

    .day-item {
        &.is-past {
            text-decoration: line-through;
        }
    }

    .food-container {
        overflow-y: auto;
    }

    .controls {
        line-height: 1em;
        opacity: 0;
        pointer-events: none;
        transition: opacity DURATION('short');
        color: COLOUR('grey');

        .icon {
            transition: color DURATION('short');

            &:hover {
                color: COLOUR('black');
                cursor: pointer;
            }
        }
    }

    &:hover {
        .controls {
            opacity: 1;
            pointer-events: all;
        }
    }

    &:nth-child(-n + 7) {
        border-top: 0;
    }

    &:nth-child(7n) {
        border-right: 0;
    }
}
</style>