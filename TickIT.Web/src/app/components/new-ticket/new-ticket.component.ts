import { Component } from '@angular/core';
import { Editor } from 'ngx-editor';

@Component({
  selector: 'app-new-ticket',
  templateUrl: './new-ticket.component.html',
  styleUrls: ['./new-ticket.component.css']
})
export class NewTicketComponent {
  public editor: Editor;
  constructor() {
    this.editor = new Editor();
  }
  ngOnDestroy():void{
    this.editor.destroy();
  }
}
