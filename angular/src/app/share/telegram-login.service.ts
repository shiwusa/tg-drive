import { Injectable, NgZone } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TelegramLoginService {
  loggedIn$ = new BehaviorSubject<boolean>(sessionStorage.getItem('logged-in') === 'true');

  constructor(
    private ngZone: NgZone,
    private cookieService: CookieService,
    ) {
    this.init();
  }

  init() {
    debugger;
    (window as any)['loginViaTelegram'] = (loginData: any) =>
      this.loginViaTelegram(loginData);
  }

  signOut() {
    this.ngZone.run(() =>
    {
        this.cookieService.delete('tg-hash');
        this.cookieService.delete('tg-data');
        sessionStorage.setItem('logged-in', 'false');
        this.loggedIn$.next(false);
    });
  }

  private loginViaTelegram(loginData: any) {
    debugger;
    this.ngZone.run(() =>
    {
      debugger;
      if (!!loginData) {
        const dataCheckString = this.getDataCheckString(loginData);
        sessionStorage.setItem('tg-hash', loginData.hash);
        sessionStorage.setItem('tg-data', dataCheckString);
        sessionStorage.setItem('logged-in', 'true');
        this.loggedIn$.next(true);
      }
    });
  }

  private getDataCheckString(loginData: any) {
    const keyValues: string[] = [];
    for (const key in loginData) {
      if (key !== 'hash' && Object.prototype.hasOwnProperty.call(loginData, key)) {
        const value = loginData[key];
        keyValues.push(`${key}=${value}`);
      }
    }

    keyValues.sort((a, b) => {
      if (a < b) return -1;
      if (a > b) return 1;
      return 0;
    });
    return keyValues.join('\n');
  }
}
