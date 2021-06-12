<template>
    <div class="edit-day-modal-component flex flex-vertical">
        <div class="flex-auto">
            <h2>Edit Day</h2>
            <p class="day-being-edited">{{ displayDate }} <small>({{ dateDifference }})</small></p>
        </div>
        <div class="edit-day-content">
            <div class="notes">
                <h3>Food of the Day</h3>
                <section v-if="entriesForDay.length > 0">
                    <div
                        class="calendar-entry-item flex"
                        :class="{ 'is-past': isPast(entry) }"
                        :key="`food-${entry.reference}`"
                        v-for="entry in entriesForDay"
                    >
                        <div>
                            <strong>{{ entry.title }}: </strong>
                            <span> {{ entry.description }}</span>
                        </div>
                        <div class="flex-auto">
                            <small>{{ entry.at.format('@ HH:mm') }}</small>
                        </div>
                        <div class="flex-auto">
                            <div class="delete-calendar-entry-item" @click="onDeleteEntry(entry.reference)">
                                <DeleteIcon small />
                            </div>
                        </div>
                    </div>
                </section>
                <section v-if="entriesForDay.length === 0">
                    No foods have been added for this day, consider adding one below!
                </section>
                <section>
                    <h3>Uploaded Photos</h3>
                </section>
                <section v-if="photosForDay.length === 0">
                    <p>Have a photo of your food? Consider adding one below!</p>
                </section>
            </div>
        </div>
        <div class="edit-day-container flex-auto" v-if="currentEditingType !== 'none'">
            <div v-if="currentEditingType === 'entry'">
                <h3>New Entry</h3>
                <label>
                    <span>Title</span>
                    <input ref="newEntryTitleInput" type="text" placeholder="Dessert" v-model="newEntryTitle">
                </label>
                <label>
                    <span>Description</span>
                    <input type="text" placeholder="Apple Strudel" v-model="newEntryDescription">
                </label>
                <label>
                    <span>Time of Day</span>
                    <div class="flex flex-middle">
                        <div>
                            <input type="number" min="0" max="23" step="1" v-model="newEntryAtHour">
                        </div>
                        <div class="new-entry-date-separator flex-auto">:</div>
                        <div>
                            <input type="number" min="0" max="45" step="15" v-model="newEntryAtMinute">
                        </div>
                    </div>
                </label>
                <label>
                    <span>Timespan (Days)</span>
                    <div class="flex flex-middle">
                        <div class="flex-4">
                            <input type="range" min="1" max="14" v-model="newEntryDaySpan">
                        </div>
                        <div class="flex-1 text-centered">
                            {{ newEntryDaySpan }}
                        </div>
                    </div>
                </label>
                <button @click="onAddEntry">Add Entry</button>
            </div>
            <div v-else-if="currentEditingType === 'photo'">
                <h3>New Photo</h3>
                <label>
                    <input ref="newPhotoInput" type="file" accept="image/png,image/jpeg">
                </label>
                <label>
                    <span>Description</span>
                    <input type="text" placeholder="My Apple Strudel" v-model="newPhotoDescription">
                </label>
                <button @click="onAddPhoto">Submit Photo</button>
            </div>
        </div>
        <div class="edit-day-actions flex flex-auto gap-small">
            <button @click="setCurrentEditingType('entry')">Add Entry</button>
            <button @click="setCurrentEditingType('photo')">Upload Photo</button>
        </div>
    </div>
</template>

<script lang="ts">
import { computed, PropType, ref } from 'vue';
import { useStore } from 'vuex';
import dayjs from 'dayjs';

import DeleteIcon from '@/component/icon/CrossIcon.vue';

import { CalendarEntry } from '@/type/CalendarEntry.type';
import { AppStoreKey } from '@/store/AppStoreKey';
import { AppState } from '@/store/type/AppState.type';
import { EditDayModalProps } from '@/component/modal/EditDayModal.props';

import { api } from '@/api/API';
import { dateService } from '@/service/Date.service';
import { calendarEntryService } from '@/service/CalendarEntry.service';
import { CalendarPhoto } from '@/type/CalendarPhoto.type';

type EditingType = 'none' | 'entry' | 'photo';

export default {
    name: 'EditDayModalComponent',

    props: {
        day: {
            type: Object as PropType<dayjs.Dayjs>,
            required: true,
        },
    },

    components: {
        DeleteIcon,
    },

    setup(props: EditDayModalProps) {
        const store = useStore<AppState>();

        const calendarEntries = computed<Map<string, CalendarEntry>>(() => store.getters.calendar.entries);
        const entriesForDay = computed<Array<CalendarEntry>>(() => {
            return Array.from(calendarEntries.value.values())
                .filter(x => calendarEntryService.isEntryWithinDay(props.day, x))
                .sort(calendarEntryService.sortByAtAscending);
        });

        const photosForDay = ref<Array<CalendarPhoto>>([]);

        const displayDate = computed<string>(() => props.day.format('dddd Do MMMM YYYY'));

        const dateDifference = computed<string>(() => {
            if (props.day.isToday())
                return 'Today';

            if (props.day.isTomorrow())
                return 'Tomorrow';

            if (props.day.isYesterday())
                return 'Yesterday';

            return props.day.fromNow();
        });

        const newEntryTitleInput = ref<HTMLInputElement | null>(null);
        const newPhotoInput = ref<HTMLInputElement | null>(null);

        const currentEditingType = ref<EditingType>('none');

        const newEntryTitle = ref<string>('');
        const newEntryDescription = ref<string>('');
        const newEntryAtHour = ref<number>(12);
        const newEntryAtMinute = ref<number>(0);
        const newEntryDaySpan = ref<number>(1);

        const newPhotoDescription = ref<string>('');

        const getToggledEditingType = function (editingType: EditingType): EditingType {
            if (editingType !== currentEditingType.value)
                return editingType;

            return 'none';
        }

        return {
            entriesForDay,
            photosForDay,
            displayDate,
            dateDifference,
            newEntryTitleInput,
            newPhotoInput,
            currentEditingType,
            newEntryTitle,
            newEntryDescription,
            newEntryAtHour,
            newEntryAtMinute,
            newEntryDaySpan,
            newPhotoDescription,

            pad: dateService.pad,

            isPast(entry: CalendarEntry): boolean {
                return calendarEntryService.isBeforeDay(entry, props.day);
            },

            async onAddEntry() {
                if (newEntryTitle.value.length === 0)
                    return;

                if (newEntryDescription.value.length === 0)
                    return;

                const newEntry = await api.addCalendarEntry({
                    title: newEntryTitle.value,
                    description: newEntryDescription.value,
                    at: props.day.hour(newEntryAtHour.value).minute(newEntryAtMinute.value).toISOString(),
                    daySpan: newEntryDaySpan.value,
                });

                store.dispatch(AppStoreKey.ADD_CALENDAR_ENTRY, newEntry);

                newEntryTitle.value = '';
                newEntryDescription.value = '';
                newEntryAtHour.value = 12;
                newEntryAtMinute.value = 0;
                newEntryDaySpan.value = 1;

                currentEditingType.value = 'none';
            },

            onDeleteEntry(reference: string) {
                store.dispatch(AppStoreKey.REMOVE_CALENDAR_ENTRY, reference);
            },

            async onAddPhoto() {
                if (newPhotoInput.value === null)
                    return;

                if (newPhotoDescription.value.length === 0)
                    return;

                const files = newPhotoInput.value.files;
                if (files === null || files.length === 0)
                    return;

                const file = files[0];

                const newPhoto = await api.uploadCalendarPhoto({
                    description: newPhotoDescription.value,
                    photo: file,
                    date: props.day.toISOString()
                });

                newPhotoDescription.value = '';

                currentEditingType.value = 'none';
            },

            setCurrentEditingType(editingType: EditingType) {
                currentEditingType.value = getToggledEditingType(editingType);
            },
        }
    },
}
</script>

<style lang="scss">
.edit-day-modal-component {
    height: 100%;

    .edit-day-content {
        margin: -1rem -1rem 0 -1rem;
        padding: 0 1rem;
        flex: 1 1 auto;
        overflow-y: auto;
        height: 0px;
    }

    h2 {
        margin-bottom: 0.5rem;
    }

    .day-being-edited {
        margin-top: 0;
    }

    .new-entry-button {
        padding: 0 0.5rem;
        border: 1px solid COLOUR('grey');
        border-radius: BORDER_RADIUS();
        text-align: center;
        cursor: pointer;
        transition: background-color DURATION('short');

        &:hover {
            background-color: COLOUR('grey');
        }
    }

    .calendar-entry-item {
        &.is-past {
            text-decoration: line-through;
        }

        &:hover {
            .delete-calendar-entry-item {
                color: COLOUR('grey');

                &:hover {
                    background-color: COLOUR('grey');
                    color: COLOUR('black');
                }
            }
        }
    }

    .delete-calendar-entry-item {
        padding: 0.5rem;
        line-height: 0;
        color: transparent;
        border-radius: BORDER_RADIUS();
        cursor: pointer;
    }

    .edit-day-container {
        margin: 0 -1rem;
        padding: 1rem;
        border-top: 1px solid COLOUR('grey');
        background-color: COLOUR('white');

        h3 {
            margin-top: 0;
        }
    }

    .edit-day-actions {
        margin: -1rem;
        margin-top: 0;
        padding: 1rem;
        background-color: COLOUR('white');
        border-top: 1px solid COLOUR('grey');
        border-bottom-right-radius: BORDER_RADIUS();
    }

    .new-entry-date-separator {
        padding: 0 0.5rem;
    }
}
</style>