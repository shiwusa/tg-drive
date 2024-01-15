import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-telegram-login-widget',
  template: `
<div #script style="display:none">
  <ng-content></ng-content>
</div>`,
  styleUrls: ['./telegram-login-widget.component.scss']
})
export class TelegramLoginWidgetComponent implements AfterViewInit {
  @ViewChild('script', {static: true}) script: ElementRef = null as any;

  convertToScript() {
    debugger;
    const element = this.script.nativeElement;
    const script = document.createElement('script');
    script.src = 'https://telegram.org/js/telegram-widget.js?21';
    script.setAttribute('data-telegram-login', "ArchiveParser111_bot");
    script.setAttribute('data-size', 'large');
    script.setAttribute('data-onauth', 'loginViaTelegram(user)');
    script.setAttribute('data-request-access', 'write');
    script.setAttribute('data-userpic', 'false');
    element.parentElement.replaceChild(script, element);
  }

  ngAfterViewInit() {
    this.convertToScript();
  }
}
