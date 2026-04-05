import { Component, input, output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Tour } from '../../models/tour.model';

@Component({
  selector: 'app-tour-form',
  imports: [ReactiveFormsModule],
  templateUrl: './tour-form.component.html',
  styleUrl: './tour-form.component.scss'
})
export class TourFormComponent implements OnInit {
  tour = input<Tour | null>(null);
  save = output<Partial<Tour>>();
  cancel = output();

  form!: FormGroup;
  transportTypes = ['bike', 'hike', 'running', 'vacation'];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    const t = this.tour();
    this.form = this.fb.group({
      name: [t?.name ?? '', [Validators.required, Validators.minLength(3)]],
      description: [t?.description ?? ''],
      from: [t?.from ?? '', Validators.required],
      to: [t?.to ?? '', Validators.required],
      transportType: [t?.transportType ?? '', Validators.required],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const value = { ...this.form.value };
      value.distance = value.distance != null ? Number(value.distance) : 0;
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
