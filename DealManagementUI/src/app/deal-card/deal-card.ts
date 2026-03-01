import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Deal } from '../services/deal';

@Component({
  selector: 'app-deal-card',
  imports: [],
  templateUrl: './deal-card.html',
  styleUrl: './deal-card.scss',
})
export class DealCard {

  @Input()
  deal!: Deal;

  @Output()
  open = new EventEmitter<Deal>();

  onClick() {
    this.open.emit(this.deal);
  }

  formatDate(d: string | Date): string {
    const dt = typeof d === 'string' ? new Date(d) : d;
    if (dt === null) return String(d);
    const dd = String(dt.getDate()).padStart(2, '0');
    const mm = String(dt.getMonth() + 1).padStart(2, '0');
    const yy = String(dt.getFullYear()).slice(-2);
    return `${dd}-${mm}-${yy}`;
  }

}
