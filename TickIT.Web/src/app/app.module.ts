import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home/home.component';
import { LetNavComponent } from './home/nav-bar/let-nav/let-nav.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TicketsComponent } from './home/tickets/tickets.component';
import { AddTicketComponent } from './home/tickets/add-ticket/add-ticket.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LetNavComponent,
    TicketsComponent,
    AddTicketComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [HomeComponent]
})
export class AppModule { }
