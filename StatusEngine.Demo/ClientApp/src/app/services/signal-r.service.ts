import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";

export interface AssetModel {
  name: string;
  colour: string;
  reason: string;
}

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public data: AssetModel[];
  private hubConnection: signalR.HubConnection;  

  constructor() {
    this.hubConnection = new signalR
      .HubConnectionBuilder()
      .withUrl('https://localhost:5001/chatHub')
      .withAutomaticReconnect()
      .build();
    this.data = [];
  }

  public startConnection = () => {
    this.hubConnection

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public subscribeToUpdates = () => {
    this.hubConnection.on('assetownerupdate', (updates) => {
      var asset = updates as AssetModel;
      console.log("assetownerupdate:");
      console.log(updates);
      var assets = this.data
        .filter(a => a.name === asset.name);

      if (assets.length == 0)
        this.data.push(updates);
      else
        assets.forEach(a => {
          a.colour = asset.colour;
          a.reason = asset.reason});
    });
  }
}
