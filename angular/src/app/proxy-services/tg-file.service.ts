import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { createControllerUrlCreator } from './proxy-utils';

@Injectable({
  providedIn: 'root'
})
export class TgFileService {
  createUrl = createControllerUrlCreator('TgFile');

  constructor(private http: HttpClient) {}

  public sendFile(fileId: number): Promise<number> {
    return lastValueFrom(
      this.http.post<number>(this.createUrl('SendFile'), {}, {
        params: { fileId },
      })
    );
  }
}
