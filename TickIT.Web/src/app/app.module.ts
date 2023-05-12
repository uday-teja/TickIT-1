import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './shared/components/header/header.component';
import { LeftNavComponent } from './shared/components/left-nav/left-nav.component';
import { TicketsComponent } from './components/tickets/tickets.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CreateNewTicketComponent } from './shared/components/create-new-ticket/create-new-ticket.component';
import { TicketFiltersComponent } from './components/ticket-filters/ticket-filters.component';
import { TicketsDatagridComponent } from './components/tickets-datagrid/tickets-datagrid.component';
import { NewTicketComponent } from './components/new-ticket/new-ticket.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LeftNavComponent,
    TicketsComponent,
    DashboardComponent,
    CreateNewTicketComponent,
    TicketFiltersComponent,
    TicketsDatagridComponent,
    NewTicketComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FontAwesomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
