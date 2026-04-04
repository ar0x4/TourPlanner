import { Component, input, output } from '@angular/core';
import { Tour } from '../../models/tour.model';

@Component({
  selector: 'app-tour-list',
  templateUrl: './tour-list.component.html',
  styleUrl: './tour-list.component.scss'
})
export class TourListComponent {
  tours = input<Tour[]>([]);
  selectedTourId = input<string | null>(null);
  selectTour = output<Tour>();
  addTour = output();

  transportIcon(type: string): string {
    const icons: Record<string, string> = {
      bike: 'fi fi-rr-biking',
      hike: 'fi fi-rr-hiking',
      running: 'fi fi-rr-running',
      vacation: 'fi fi-rr-plane-departure'
    };
    return icons[type] ?? 'fi fi-rr-map-marker';
  }

  transportColor(type: string): string {
    const colors: Record<string, string> = {
      bike: 'bg-emerald-100 text-emerald-600',
      hike: 'bg-amber-100 text-amber-600',
      running: 'bg-violet-100 text-violet-600',
      vacation: 'bg-sky-100 text-sky-600'
    };
    return colors[type] ?? 'bg-primary/10 text-primary';
  }
}
