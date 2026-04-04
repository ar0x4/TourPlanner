import { Component, OnInit, signal } from '@angular/core';
import { TourService } from '../../services/tour.service';
import { TourLogService } from '../../services/tour-log.service';
import { RouteService, RouteInfo } from '../../services/route.service';
import { Tour } from '../../models/tour.model';
import { TourLog } from '../../models/tour-log.model';
import { TourListComponent } from '../../components/tour-list/tour-list.component';
import { TourDetailComponent } from '../../components/tour-detail/tour-detail.component';
import { TourFormComponent } from '../../components/tour-form/tour-form.component';
import { TourLogFormComponent } from '../../components/tour-log-form/tour-log-form.component';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-tours',
  imports: [
    TourListComponent,
    TourDetailComponent,
    TourFormComponent,
    TourLogFormComponent,
    ConfirmDialogComponent
  ],
  templateUrl: './tours.component.html',
  styleUrl: './tours.component.scss'
})
export class ToursComponent implements OnInit {
  tours = signal<Tour[]>([]);
  selectedTour = signal<Tour | null>(null);
  tourLogs = signal<TourLog[]>([]);

  showTourForm = signal(false);
  editingTour = signal<Tour | null>(null);

  showLogForm = signal(false);
  editingLog = signal<TourLog | null>(null);

  confirmMessage = signal('');
  showConfirm = signal(false);
  private pendingAction: (() => void) | null = null;

  constructor(
    private tourService: TourService,
    private tourLogService: TourLogService,
    private routeService: RouteService,
    public authService: AuthService
  ) {}

  ngOnInit() {
    this.loadTours();
  }

  loadTours() {
    this.tourService.getAll().subscribe(tours => this.tours.set(tours));
  }

  selectTour(tour: Tour) {
    this.selectedTour.set(tour);
    this.loadLogs(tour.id);
  }

  loadLogs(tourId: string) {
    this.tourLogService.getAll(tourId).subscribe(logs => this.tourLogs.set(logs));
  }

  openAddTour() {
    this.editingTour.set(null);
    this.showTourForm.set(true);
  }

  openEditTour() {
    this.editingTour.set(this.selectedTour());
    this.showTourForm.set(true);
  }

  saveTour(data: Partial<Tour>) {
    const editing = this.editingTour();
    if (editing) {
      if (data.from && data.to && data.transportType) {
        this.routeService.getRoute(data.from, data.to, data.transportType).subscribe({
          next: (route) => {
            data.distance = route.distance;
            data.estimatedTime = route.estimatedTime;
            (data as any).routeCoordinates = JSON.stringify(route.coordinates);
            this.updateTour(editing.id, data);
          },
          error: () => {
            this.updateTour(editing.id, data);
          }
        });
      } else {
        this.updateTour(editing.id, data);
      }
    } else {
      if (data.from && data.to && data.transportType) {
        this.routeService.getRoute(data.from, data.to, data.transportType).subscribe({
          next: (route) => {
            data.distance = route.distance;
            data.estimatedTime = route.estimatedTime;
            (data as any).routeCoordinates = JSON.stringify(route.coordinates);
            this.createTour(data);
          },
          error: () => {
            this.createTour(data);
          }
        });
      } else {
        this.createTour(data);
      }
    }
  }

  private updateTour(id: string, data: Partial<Tour>) {
    this.tourService.update(id, data).subscribe(updated => {
      this.loadTours();
      this.selectedTour.set(updated);
      this.showTourForm.set(false);
    });
  }

  private createTour(data: Partial<Tour>) {
    this.tourService.create(data).subscribe(created => {
      this.loadTours();
      this.selectedTour.set(created);
      this.loadLogs(created.id);
      this.showTourForm.set(false);
    });
  }

  requestDeleteTour() {
    this.confirmMessage.set('Are you sure you want to delete this tour? This will also delete all associated logs.');
    this.pendingAction = () => {
      const tour = this.selectedTour();
      if (!tour) return;
      this.tourService.delete(tour.id).subscribe(() => {
        this.selectedTour.set(null);
        this.tourLogs.set([]);
        this.loadTours();
      });
    };
    this.showConfirm.set(true);
  }

  openAddLog() {
    this.editingLog.set(null);
    this.showLogForm.set(true);
  }

  openEditLog(log: TourLog) {
    this.editingLog.set(log);
    this.showLogForm.set(true);
  }

  saveLog(data: Partial<TourLog>) {
    const tour = this.selectedTour();
    if (!tour) return;

    const editing = this.editingLog();
    if (editing) {
      this.tourLogService.update(tour.id, editing.id, data).subscribe(() => {
        this.loadLogs(tour.id);
        this.showLogForm.set(false);
      });
    } else {
      this.tourLogService.create(tour.id, data).subscribe(() => {
        this.loadLogs(tour.id);
        this.showLogForm.set(false);
      });
    }
  }

  requestDeleteLog(log: TourLog) {
    this.confirmMessage.set('Are you sure you want to delete this log entry?');
    this.pendingAction = () => {
      const tour = this.selectedTour();
      if (!tour) return;
      this.tourLogService.delete(tour.id, log.id).subscribe(() => {
        this.loadLogs(tour.id);
      });
    };
    this.showConfirm.set(true);
  }

  onConfirm() {
    this.pendingAction?.();
    this.showConfirm.set(false);
    this.pendingAction = null;
  }

  onCancelConfirm() {
    this.showConfirm.set(false);
    this.pendingAction = null;
  }
}
