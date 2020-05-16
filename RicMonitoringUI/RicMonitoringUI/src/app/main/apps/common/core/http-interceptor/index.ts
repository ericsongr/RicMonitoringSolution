import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { AuthInterceptor } from "./AuthInterceptor";

/*** Http Interceptor providers */
export const httpInterceptorProvider = [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}
]