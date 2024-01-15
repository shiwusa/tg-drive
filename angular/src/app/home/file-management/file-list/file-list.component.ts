import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { FileService } from 'src/app/proxy-services/file.service';
import { DirectoryDto, FileDto } from 'src/app/proxy-services/models';
import { TgFileService } from 'src/app/proxy-services/tg-file.service';

interface File {
  data: FileDto;
  link: string;
}

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.scss'],
})
export class FileListComponent implements OnInit, OnChanges {
  files: File[] = [];
  loading: boolean = false;

  @Input()
  currentDirectory = {} as DirectoryDto;

  constructor(
    private fileService: FileService,
    private tgFileService: TgFileService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (!!changes['currentDirectory']?.currentValue) {
      this.loadDirectory(this.currentDirectory.id);
    }
  }

  ngOnInit(): void {}

  async loadDirectory(directoryId: number) {
    this.loading = true;
    const files = await this.fileService.getFiles(directoryId);
    this.files = files.map((x) => ({
      data: x,
      link: `https://t.me/c/${x.chatId.toString().substring(4)}/${x.messageId}`,
    }));
    this.loading = false;
  }

  async sendFile(file: File) {
    this.loading = true;
    await this.tgFileService.sendFile(file.data.id);
    this.loading = false;
  }
}
