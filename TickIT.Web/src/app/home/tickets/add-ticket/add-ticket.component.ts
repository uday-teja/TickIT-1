import { Component, Inject, Input, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators  } from '@angular/forms';
import { TicketsComponent } from '../tickets.component';
import { Output, EventEmitter } from '@angular/core';
import { Ticket } from 'src/models/Ticket';

@Component({
  selector: 'app-add-ticket',
  templateUrl: './add-ticket.component.html',
  styleUrls: ['./add-ticket.component.css']
})
export class AddTicketComponent {

  constructor(private fromBuilder: FormBuilder,@Inject(TicketsComponent) private parent: TicketsComponent) {}

  @Input() ticket: string | undefined
  @Output() newItemEvent = new EventEmitter<string>();

  addTicket = this.fromBuilder.nonNullable.group({
    name:['',[Validators.required, Validators.maxLength(10)]],
    description: ''
  });
  
  get name(){
    return this.addTicket.get('name');
  }

  onSave(): void{
    let value: string = this.addTicket.value.description ?? '';
    localStorage.setItem("tickets",value);
    this.addTicket.reset();
    this.parent.showAddForm = false;
    this.newItemEvent.emit(value);
    }
}
