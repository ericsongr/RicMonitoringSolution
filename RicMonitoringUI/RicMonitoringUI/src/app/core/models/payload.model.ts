export interface IPayload<T> {
    payload: T;
    errors: Error;
    statusCode: number;
}

export class Payload<T> implements IPayload<T> {
    payload: T;
    errors: Error;
    statusCode: number;

    constructor(payload?: IPayload<T>) {
        if (payload) {
            this.payload = payload.payload;
            this.errors = payload.errors;
            this.statusCode = payload.statusCode;
        }
    }
}