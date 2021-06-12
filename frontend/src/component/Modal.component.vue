<template>
    <div class="modal-component" :class="{ 'is-visible': modalComponent !== null, [`is-${side}`]: true }" @click.self="onClose">
        <div class="outer-content flex flex-vertical">
            <div class="flex-auto">
                <button class="close-button" @click="onClose">
                    <CloseIcon />
                </button>
            </div>
            <div class="inner-content">
                <component :is="modalComponent" v-bind="props" />
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, onMounted, ref, shallowRef } from 'vue';

import CloseIcon from '@/component/icon/CrossIcon.vue';

import { eventService, OpenModalDetails } from '@/service/Event.service';

export default {
    name: 'ModalComponent',

    components: {
        CloseIcon,
    },

    setup() {
        const modalComponent = shallowRef<Component | null>(null);
        const side = ref<'left' | 'right'>('left');
        const props = ref<any>();

        onMounted(() => {
            eventService.sub('open-modal', (details: OpenModalDetails) => {
                modalComponent.value = details.component;
                side.value = details.side;
                props.value = details.props;
            });

            eventService.sub('close-modal', () => {
                modalComponent.value = null;
            });
        });

        return {
            modalComponent,
            side,
            props,

            onClose() {
                eventService.pub('close-modal');
            },
        }
    },
}
</script>

<style lang="scss">
.modal-component {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    background-color: rgba(0, 0, 0, 0.2);
    opacity: 0;
    pointer-events: none;
    transition: opacity DURATION('short');
    z-index: 20;

    &.is-visible {
        opacity: 1;
        pointer-events: all;

        .outer-content {
            transform: translateX(0);
        }
    }

    .outer-content {
        width: 33.33%;
        max-width: 500px;
        margin-right: auto;
        height: 100%;
        padding: 1rem;
        background-color: COLOUR('grey-light');
        box-shadow: 1px 2px 5px rgba(0, 0, 0, 0.3);
        border-top-right-radius: BORDER_RADIUS();
        border-bottom-right-radius: BORDER_RADIUS();
        transform: translateX(-100%);
        transition: transform DURATION('mid');
    }

    .close-button {
        width: auto;
        display: table;
        margin-left: auto;
        padding: 0.5rem;
        line-height: 0;
        background-color: transparent;
        border: 0;
        border-radius: BORDER_RADIUS();
        cursor: pointer;
        color: COLOUR('grey');

        &:hover {
            background-color: COLOUR('grey');
            color: COLOUR('black');
        }
    }
}
</style>