import { Component, OnInit } from '@angular/core';
import { SignalRService, AssetModel } from './services/signal-r.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public title: string;

  constructor(
    public signalRService: SignalRService,
    private http: HttpClient)
  {
    this.title = "Demo SignalR";
  }

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.subscribeToUpdates();
    this.startHttpRequest();
  }

  private startHttpRequest = () => {
    this.http.get('https://localhost:5001/assetOwners')
      .subscribe(res => {
        this.signalRService.data = res as AssetModel[];
        console.log("asset owners:")
        console.log(res);
      })
  }
}
