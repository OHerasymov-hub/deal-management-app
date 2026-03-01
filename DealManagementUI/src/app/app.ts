import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { KeycloakService } from 'keycloak-angular';
import { DealService, Deal } from './services/deal';
import { DealCard } from './deal-card/deal-card';
import { DealDetails } from './deal-details/deal-details';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, DealCard, DealDetails],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  deals: Deal[] = [];
  isLoggedIn = false;
  newTitle = '';
  newAmount = 0;

  selectedDeal: Deal | null = null;

  constructor(
    private keycloak: KeycloakService,
    private dealService: DealService
  ) { }

  async ngOnInit() {
    this.isLoggedIn = await this.keycloak.isLoggedIn();
    if (this.isLoggedIn) {
      this.loadDeals();
    }
  }

  login() { this.keycloak.login(); }
  logout() { this.keycloak.logout(window.location.origin); }

  loadDeals() {
    this.dealService.getDeals().subscribe({
      next: (data) => this.deals = data,
      error: (err) => console.error('Ошибка загрузки сделок', err)
    });
  }

  saveDeal() {
    this.dealService.createDeal({ title: this.newTitle, amount: this.newAmount }).subscribe(() => {
      this.loadDeals();
      this.newTitle = '';
      this.newAmount = 0;
    });
  }

  handleSave(updatedDeal: Deal) {
    // calling .Net api service
    this.dealService.updateDeal(updatedDeal).subscribe({
      next: () => {
        this.loadDeals(); // update list
        this.selectedDeal = null; // close the panel
      },
      error: (err) => alert('Error: ' + err.error.detail)
    });
  }

  onOpen($event: Deal) {
    this.selectedDeal = $event;
  }

  changeStatus(updatedDeal: Deal) {
    this.dealService.changeStatusDeal(updatedDeal).subscribe(
      {
        next: () => {
          this.loadDeals();
          this.selectedDeal = null;
        },
        error: (err) => alert('Error: ' + err.error.detail)
      }
    );
  }
}
