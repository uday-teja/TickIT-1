import { Component } from '@angular/core';
import { Ticket } from 'src/models/Ticket';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent {

  showAddForm:boolean = false;
  tickets: Array<Ticket> = [];

  addTicket()
  {
    this.showAddForm = true;
  }

  addItem(newItem: string) {
    this.tickets.push({name: "Iam name", description:newItem})
  }

  editTicket()
  {
    this.tickets.push({name: "Iam name", description:"from edit"})
  }
}
