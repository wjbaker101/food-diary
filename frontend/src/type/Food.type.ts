export interface FoodTimeOfDay {
    hours: number;
    minutes: number;
}

export interface Food {
    title: string;
    description: string;
    timeOfDay: FoodTimeOfDay,
}
