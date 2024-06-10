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
import { ConfirmationService, MessageService } from 'primeng/api';

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
  editingFiles: { [id: number]: File } = {};
  loading: boolean = false;

  @Input()
  currentDirectory = {} as DirectoryDto;

  constructor(
    private fileService: FileService,
    private tgFileService: TgFileService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
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

  onRowEditInit(file: File) {
    this.editingFiles[file.data.id] = {
      data: { ...file.data },
      link: file.link,
    };
  }

  onRowEditSave(file: File) {
    if (!file.data.name || file.data.name === "") {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Filename cannot be empty!',
        sticky: true,
      });

      return;
    }

    const dto = file.data;
    Promise.all([
      this.fileService.changeName(dto.id, dto.name),
      this.fileService.changeDescription(dto.id, dto.description!)])
      .then(
        (file) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'File successfully updated!',
            sticky: true,
          });
        },
        (error) => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Update of file was not successful!',
            sticky: true,
        });
      });
  }

  onRowEditCancel(file: File, rowIndex: number) {
    this.files[rowIndex] = this.editingFiles[file.data.id];
    delete this.editingFiles[file.data.id];
  }

  removeFile(file: File) {
    this.confirmationService.confirm({
      accept: () => {
        const result = this.fileService.removeFile(file.data.id)
        .then(
          (file) => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'File successfully deleted',
              sticky: true,
            });
          },
          (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Update of file was not successful!',
              sticky: true,
            });
          });

        if (!!result) {
          this.files = this.files.filter(x => x.data.id !== file.data.id);
        }
      },
      closeOnEscape: true,
      message: `Are you sure you want to delete the file: ${file.data?.name}?\n
        The file will be lost permanently,though you can\n
        find the related messages in your storage channel.`,
    });
  }

  onFileUpload(event: Event) {

  }
}
