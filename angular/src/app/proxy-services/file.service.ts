import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { FileDto } from './models';
import { createControllerUrlCreator } from './proxy-utils';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  createUrl = createControllerUrlCreator('File');

  constructor(private http: HttpClient) {}

  public getFiles(directoryId: number): Promise<FileDto[]> {
    return lastValueFrom(
      this.http.get<FileDto[]>(this.createUrl('GetFiles'), {
        params: { directoryId },
      }));
  }

  public changeName(fileId: number, newName: string): Promise<FileDto> {
    return lastValueFrom(
      this.http.put<FileDto>(this.createUrl('ChangeName'), {}, {
        params: { fileId, newName },
      }));
  }

  public changeDescription(fileId: number, newDescription: string): Promise<FileDto> {
    return lastValueFrom(
      this.http.put<FileDto>(this.createUrl('ChangeDescription'), {}, {
        params: { fileId, newDescription },
      }));
  }

  public removeFile(fileId: number): Promise<FileDto> {
    return lastValueFrom(
      this.http.delete<FileDto>(this.createUrl('Remove'), {
        params: { fileId },
      })
    );
  }
}
