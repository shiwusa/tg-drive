import { Component } from '@angular/core';
import { TelegramLoginService } from 'src/app/share/telegram-login.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  constructor(private loginService: TelegramLoginService) {}

  signOut() {
    this.loginService.signOut();
  }
}
