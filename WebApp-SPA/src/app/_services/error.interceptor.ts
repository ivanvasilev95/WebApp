import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse) {
                    // unknown error
                    if (error.status === 0) {
                        return throwError(error.statusText);
                    }
                    // unauthorized, not found, bad request or internal server error (global exception)
                    if (error.status === 401 || error.status === 404
                    || (error.status === 400 && !error.error.errors) || error.status === 500) {
                        if (typeof error.error === 'object') { // if not then it's string
                            if (error.error.constructor === Array) { // if it's an array object
                                let errorData = '';
                                error.error.forEach(err => {
                                    errorData += err.description;
                                });
                                return throwError(errorData);
                            }
                            return throwError(error.statusText); // 'Грешка на сървъра'
                        }
                        return throwError(error.error); // string error message
                    }
                    /*
                    // global exception error
                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError) {
                        return throwError(applicationError);
                    }
                    */
                    // model state error (returns 400 bad request)
                    const serverError = error.error.errors;
                    let modelStateErrors = '';
                    if (serverError && typeof serverError === 'object') {
                        for (const key in serverError) {
                            if (serverError[key]) {
                                modelStateErrors += serverError[key] + '\n';
                            }
                        }
                    }
                    return throwError(modelStateErrors || serverError || 'Грешка на сървъра');
                }
            })
        );
    }
}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};
