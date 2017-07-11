import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  styleUrls: ['./pagination.component.scss'],
  templateUrl: './pagination.component.html'
})
export class PaginationComponent {
  @Input() id: string;
  @Input() maxSize: number = 7;
  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();
}
