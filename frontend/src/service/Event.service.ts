import { Component } from 'vue';

type Event = 'open-modal' | 'close-modal';

class EventService {

    private events: Record<string, Array<Function>>;

    constructor() {
        this.events = {};
    }

    sub(key: Event, callback: Function) {
        if (key in this.events)
            this.events[key].push(callback);

        this.events[key] = [ callback ];
    }

    unsub(key: Event, callback: Function) {
        if (!(key in this.events))
            return;

        this.events[key] = this.events[key].filter(x => x !== callback);
    }

    pub(key: Event, details: Record<string, any> = {}) {
        this.events[key].forEach(x => x(details));
    }
}

export const eventService = new EventService();

export interface OpenModalDetails {
    side: 'left' | 'right';
    component: Component;
    props: any,
}
