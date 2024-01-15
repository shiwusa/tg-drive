import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class WithCredentialsInterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const tgHash = sessionStorage.getItem('tg-hash');
    const tgData = sessionStorage.getItem('tg-data');
    const headers =
      !!tgHash && !!tgData
        ? request.headers.append('tg-hash', encodeURI(tgHash)).append('tg-data', encodeURI(tgData))
        : request.headers;
    request = request.clone({
      withCredentials: true,
      headers,
    });

    return next.handle(request);
  }
}
