import { Component, input, output } from '@angular/core';
import { Tour } from '../../models/tour.model';
import { TourLog } from '../../models/tour-log.model';
import { TourLogListComponent } from '../tour-log-list/tour-log-list.component';
import { TourMapComponent } from '../tour-map/tour-map.component';

@Component({
  selector: 'app-tour-detail',
  imports: [TourLogListComponent, TourMapComponent],
  templateUrl: './tour-detail.component.html',
  styleUrl: './tour-detail.component.scss'
})
export class TourDetailComponent {
  tour = input<Tour | null>(null);
  logs = input<TourLog[]>([]);

  editTour = output();
  deleteTour = output();
  addLog = output();
  editLog = output<TourLog>();
  deleteLog = output<TourLog>();

  transportIcon(type: string): string {
    const icons: Record<string, string> = {
      bike: 'fi fi-rr-biking',
      hike: 'fi fi-rr-hiking',
      running: 'fi fi-rr-running',
      vacation: 'fi fi-rr-plane-departure'
    };
    return icons[type] ?? 'fi fi-rr-map-marker';
  }
}
