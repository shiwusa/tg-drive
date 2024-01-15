import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgParticlesModule } from 'ng-particles';
import { CookieService } from 'ngx-cookie-service';
import { ButtonModule } from 'primeng/button';
import { TreeModule } from 'primeng/tree';
import { SplitterModule } from 'primeng/splitter';
import { ContextMenuModule } from 'primeng/contextmenu';
import { InputTextModule } from 'primeng/inputtext';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TableModule } from 'primeng/table';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LandingComponent } from './landing/landing.component';
import { TelegramLoginWidgetComponent } from './share/telegram-login-widget/telegram-login-widget.component';
import { HeaderComponent } from './home/header/header.component';
import { FileManagementComponent } from './home/file-management/file-management.component';
import { DirectoryTreeComponent } from './home/file-management/directory-tree/directory-tree.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { WithCredentialsInterceptor } from './share/with-credentials-interceptor';
import { ConfirmationService } from 'primeng/api';
import { FileListComponent } from './home/file-management/file-list/file-list.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LandingComponent,
    TelegramLoginWidgetComponent,
    HeaderComponent,
    FileManagementComponent,
    DirectoryTreeComponent,
    FileListComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    ButtonModule,
    NgParticlesModule,
    TreeModule,
    SplitterModule,
    ContextMenuModule,
    OverlayPanelModule,
    InputTextModule,
    FormsModule,
    ConfirmDialogModule,
    TableModule,
  ],
  providers: [
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: WithCredentialsInterceptor,
      multi: true,
    },
    ConfirmationService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
