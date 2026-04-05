import { Component, input, output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TourLog } from '../../models/tour-log.model';

@Component({
  selector: 'app-tour-log-form',
  imports: [ReactiveFormsModule],
  templateUrl: './tour-log-form.component.html',
  styleUrl: './tour-log-form.component.scss'
})
export class TourLogFormComponent implements OnInit {
  log = input<TourLog | null>(null);
  save = output<Partial<TourLog>>();
  cancel = output();

  form!: FormGroup;
  readonly levels = [1, 2, 3, 4, 5];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    const l = this.log();
    this.form = this.fb.group({
      dateTime: [l?.dateTime ? this.toDateInputValue(l.dateTime) : '', Validators.required],
      comment: [l?.comment ?? ''],
      difficulty: [l?.difficulty ?? '', [Validators.required, Validators.min(1), Validators.max(5)]],
      totalDistance: [l?.totalDistance ?? '', [Validators.required, Validators.min(0)]],
      totalTime: [l?.totalTime ?? '', Validators.required],
      rating: [l?.rating ?? '', [Validators.required, Validators.min(1), Validators.max(5)]]
    });
  }

  private toDateInputValue(iso: string): string {
    return iso.substring(0, 16);
  }

  setRating(value: number) {
    this.form.get('rating')?.setValue(value);
    this.form.get('rating')?.markAsTouched();
  }

  setDifficulty(value: number) {
    this.form.get('difficulty')?.setValue(value);
    this.form.get('difficulty')?.markAsTouched();
  }

  onSubmit() {
    if (this.form.valid) {
      const value = { ...this.form.value };
      value.dateTime = new Date(value.dateTime).toISOString();
      value.difficulty = Number(value.difficulty);
      value.totalDistance = Number(value.totalDistance);
      value.rating = Number(value.rating);
      this.save.emit(value);
    } else {
      this.form.markAllAsTouched();
    }
  }

  hasError(field: string, error: string): boolean {
    const control = this.form.get(field);
    return !!control && control.hasError(error) && control.touched;
  }
}
