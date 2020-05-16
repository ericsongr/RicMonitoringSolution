import { FuseUtils } from "@fuse/utils";

export class Room {
    id          : number;
    name        : string;
    frequency   : string;
    price       : number;
    handle      : string;

    constructor(room?) {
        room = room || {};
        this.id = room.id || 0;
        this.name = room.name || '';
        this.frequency = room.frequency;
        this.price = room.price;
        this.handle = room.handle || FuseUtils.handleize(this.name);
    }

}
