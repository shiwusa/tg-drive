import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";


@Component({
  selector: 'app-editor-dialog',
  templateUrl: './editor-dialog.component.html',
  styleUrls: ['./editor-dialog.component.scss'],
})
export class EditorDialogComponent implements OnInit {
  position: string = 'center';

  @Input()
  visible: boolean = false;

  @Output()
  visibleChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input()
  header: string = "";

  @Input()
  message: string = "";

  @Input()
  editingValue: string | undefined = "";

  @Input()
  required: boolean = false;

  @Output()
  valueSaved: EventEmitter<string> = new EventEmitter<string>();

  constructor() {
  }

  ngOnInit(): void {
  }

  onCancelChanges(event: Event) {
    this.visible = false;
    this.visibleChange.emit(this.visible);
  }

  onSaveChanges(event: Event) {
    this.valueSaved.emit(this.editingValue);
    this.visibleChange.emit(this.visible);
  }
}
