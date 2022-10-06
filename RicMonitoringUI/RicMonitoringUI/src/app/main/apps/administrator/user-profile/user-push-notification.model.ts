
export class UserPushNotification {
    portalUserId    : string 
    deviceId        : string 
    source          : string

    constructor(portalUserId, deviceId, source) {
        this.portalUserId = portalUserId 
        this.deviceId = deviceId 
        this.source = source 
    }
}