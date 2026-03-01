import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Deal } from '../services/deal';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-deal-details',
  imports: [CommonModule, FormsModule], 
  templateUrl: './deal-details.html',
  styleUrl: './deal-details.scss',
  standalone: true
})
export class DealDetails {

  @Input()
  deal: Deal | null = null;

  @Output() save = new EventEmitter<Deal>();
  @Output() cancel = new EventEmitter<void>();
  @Output() changeStatus = new EventEmitter<Deal>();

  onSave() {
    if (this.deal)
      this.save.emit(this.deal);
  }

  onCancel() {
    this.cancel.emit();
  }

  onChangeStatus() {
    if (this.deal) {

      if (this.deal.status === 'New') this.deal.status = 'InProgress';
      else if (this.deal.status === 'InProgress') this.deal.status = 'Closed';

      this.changeStatus.emit(this.deal);
    }
  }
}
