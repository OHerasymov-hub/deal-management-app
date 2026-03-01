import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

// DealDto
export interface Deal {
  id: string;
  title: string;
  amount: number;
  status: string;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class DealService {
  private apiUrl = 'http://localhost:5226/api/Deals'; // .NET API

  constructor(private http: HttpClient) { }

  getDeals(): Observable<Deal[]> {
    return this.http.get<Deal[]>(this.apiUrl);
  }

  createDeal(deal: { title: string, amount: number }): Observable<string> {
    return this.http.post<string>(this.apiUrl, deal);
  }

  updateDeal(deal: Deal): Observable<any> {
    var res = this.http.put(`${this.apiUrl}/${deal.id}`, { "Id": deal.id, "Amount": deal.amount, "Title": deal.title });
    return res;
  }

  changeStatusDeal(deal: Deal): Observable<any> {
    console.log(deal.status);
    return this.http.patch(`${this.apiUrl}/${deal.id}/status`, JSON.stringify(deal.status), {
      headers: { 'Content-Type': 'application/json' }
    }
    );
  }
}
