import { Component } from '@angular/core';
import { Ticket } from 'src/models/Ticket';
import { Category, Priority, Status } from 'src/models/enums';

@Component({
  selector: 'app-tickets-datagrid',
  templateUrl: './tickets-datagrid.component.html',
  styleUrls: ['./tickets-datagrid.component.css']
})
export class TicketsDatagridComponent {
  public tickets: Ticket[];
  public currentPage:number;
  public pageSize:number =10;

  constructor() {
    this.tickets = [];
    this.currentPage=1;
    this.seedTickets();
  }
  seedTickets() {
    for (let i = 0; i < 25; i++) {
      this.tickets.push(new Ticket({ 
        id: i, 
        name: "Ticket " + i,
        description:"service ticket "+i, 
        category:Category[i%4],
        priority:Priority[i%3],
        status:Status[i%4],
        dateCreated:new Date(Math.floor(Math.random() * Date.now())).toLocaleDateString()
      }))
    }
  }
}
