import { throwError as observableThrowError, Observable } from 'rxjs';

import { catchError, map } from 'rxjs/operators';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpHeaders,
    HttpResponse
} from '@angular/common/http';

import { ActivatedRouteSnapshot, Router } from '@angular/router';

import { Injector, Inject } from '@angular/core';

import { environment } from '../../environments/environment';
import { UrlModel } from '../Models/url-model';
import { LoaderService } from '../Services/Loader/loader.service';

export class HttpClientInterceptor implements HttpInterceptor {

    requestCount = 0;
    private routeSnapshot: ActivatedRouteSnapshot;
    private router: Router;

    constructor(private injector: Injector,
        private loaderService: LoaderService
    ) {
    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.showLoader();

        request = request.clone({
            url: this.updateUrl(request.url),
            headers: new HttpHeaders({
                'Authorization': `Bearer ${this.getToken()}`,                
            })
        });

        return next.handle(request).pipe(
            map((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    this.onEnd();
                }
                return event;
            }),
            catchError(error => {
                this.onEnd();
                this.handleError(error);
                return observableThrowError(error);
            }
            )); 
    }

    //Hides loader service on end
    private onEnd(): void {
        this.hideLoader();
    }

    //Shows the loader service
    private showLoader(): void {
        this.requestCount++;
        if (this.requestCount == 1)
            this.loaderService.show();
    }

    //Hides the loader service
    private hideLoader(): void {
        this.requestCount--;
        if (this.requestCount == 0)
            this.loaderService.hide();
    }

    getToken() { // appends the token to the request headers
        return localStorage.getItem('app_token');
    }

    private updateUrl(req: string): string { // update the url to the proper URL        
        return environment.ApiBaseURL + req;
    }

    //Handles bad requests from the API
    private handleError(error: any) {
        if (error.status === 401) { // unauthorized
            this.getNewToken();
        }
        else if (error.status == 400) {
            this.router = this.injector.get(Router);
            localStorage.setItem("error_message", JSON.stringify(error.error));
            this.router.navigate(['/error']);
        }
    }

    getNewToken() {
        this.routeSnapshot = this.injector.get(ActivatedRouteSnapshot);
        const parameters = this.routeSnapshot.queryParams;
        const url = this.routeSnapshot.url.join('/');
        const urlObject: UrlModel = {
            URL: url,
            QueryParams: parameters,
        };

        localStorage.setItem('currentPage', JSON.stringify(urlObject));
        window.location.href = `${environment.StsURL}/oauth2/authorize?response_type=token&scope=openid` +
            `&client_id=${environment.ClientId}&resource=${environment.Resource}&redirect_uri=${environment.RedirectURI}`;        
    }
}
