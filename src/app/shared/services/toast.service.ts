import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type ToastType = 'success' | 'error';

export interface Toast {
  id: number;
  message: string;
  type: ToastType;
}

@Injectable({ providedIn: 'root' })
export class ToastService {
  private nextId = 0;
  private toastsSubject = new BehaviorSubject<Toast[]>([]);

  toasts$ = this.toastsSubject.asObservable();

  show(message: string, type: ToastType = 'success', durationMs = 3500): void {
    const id = ++this.nextId;
    const current = this.toastsSubject.getValue();
    this.toastsSubject.next([...current, { id, message, type }]);

    setTimeout(() => this.dismiss(id), durationMs);
  }

  success(message: string): void {
    this.show(message, 'success');
  }

  error(message: string): void {
    this.show(message, 'error');
  }

  dismiss(id: number): void {
    const current = this.toastsSubject.getValue();
    this.toastsSubject.next(current.filter(t => t.id !== id));
  }
}
