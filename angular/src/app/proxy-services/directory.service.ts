import { Injectable } from '@angular/core';
import { DirectoryDto } from './models';
import { HttpClient } from '@angular/common/http';
import { createControllerUrlCreator } from './proxy-utils';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DirectoryService {
  createUrl = createControllerUrlCreator('Directory');

  constructor(private http: HttpClient) {}

  public addDirectory(directory: DirectoryDto): Promise<DirectoryDto> {
    return lastValueFrom(
      this.http.post<DirectoryDto>(this.createUrl('AddDirectory'), directory, {
        withCredentials: true,
      })
    );
  }

  public getRoot(): Promise<DirectoryDto[]> {
    return lastValueFrom(
      this.http.get<DirectoryDto[]>(this.createUrl('GetRoot'))
    );
  }

  public getChildren(directoryId: number): Promise<DirectoryDto[]> {
    return lastValueFrom(
      this.http.get<DirectoryDto[]>(this.createUrl('GetChildren'), {
        params: { directoryId },
      })
    );
  }

  public removeDirectory(directoryId: number): Promise<DirectoryDto> {
    return lastValueFrom(
      this.http.delete<DirectoryDto>(this.createUrl('RemoveDirectory'), {
        params: { directoryId },
      })
    );
  }
}
