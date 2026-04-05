import { Component, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TourLog } from '../../models/tour-log.model';

@Component({
  selector: 'app-tour-log-list',
  imports: [DatePipe],
  templateUrl: './tour-log-list.component.html',
  styleUrl: './tour-log-list.component.scss'
})
export class TourLogListComponent {
  logs = input<TourLog[]>([]);
  editLog = output<TourLog>();
  deleteLog = output<TourLog>();
  addLog = output();
  readonly levels = [1, 2, 3, 4, 5];
}
