import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  template: `
    <div class="modal-overlay" (click)="cancel.emit()">
      <div class="bg-white rounded-2xl p-8 min-w-[360px] max-w-[90vw] shadow-2xl text-center" (click)="$event.stopPropagation()">
        <div class="w-14 h-14 bg-danger/10 rounded-full flex items-center justify-center mx-auto mb-4">
          <svg class="w-7 h-7 text-danger" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd"/>
          </svg>
        </div>
        <h3 class="m-0 mb-2 text-lg font-display text-primary">Confirm Action</h3>
        <p class="m-0 mb-6 text-sm text-ink-light leading-relaxed">{{ message() }}</p>
        <div class="flex justify-center gap-3">
          <button class="btn-cancel" (click)="cancel.emit()">Cancel</button>
          <button
            class="px-5 py-2.5 bg-danger hover:bg-danger-dark text-white text-sm font-semibold rounded-lg transition-colors duration-200 cursor-pointer border-none shadow-sm"
            (click)="confirm.emit()">Confirm</button>
        </div>
      </div>
    </div>
  `
})
export class ConfirmDialogComponent {
  message = input('Are you sure?');
  confirm = output();
  cancel = output();
}
