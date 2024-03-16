import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxMaskModule } from 'ngx-mask';
import { NgxCountryFlagModule } from 'ngx-country-flag';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserRegistrationComponent } from './user-registration/user-registration.component';

@NgModule({
  declarations: [AppComponent, UserRegistrationComponent],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgxCountryFlagModule,
    ModalModule.forRoot(),
    NgxMaskModule.forRoot(), // Importando NgxMaskModule ao invés de MaskModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
