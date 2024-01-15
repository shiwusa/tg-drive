import { Component } from '@angular/core';
import { DirectoryDto } from 'src/app/proxy-services/models';

@Component({
  selector: 'app-file-management',
  templateUrl: './file-management.component.html',
  styleUrls: ['./file-management.component.scss']
})
export class FileManagementComponent {
  selectedDirectory: DirectoryDto = undefined!;
}
