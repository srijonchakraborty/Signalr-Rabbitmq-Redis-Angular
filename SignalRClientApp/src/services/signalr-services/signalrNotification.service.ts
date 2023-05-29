import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class SignalRNotificationService {
  private hubConnection: signalR.HubConnection;
  private dataSubject: Subject<string> = new Subject<string>();
  public data$: Observable<string> = this.dataSubject.asObservable();
  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalrNotificationUrl) // Replace with your SignalR server URL
      .build();

    this.hubConnection.onclose(() => {
      console.log('SignalR connection closed');
    });
  }

  startSignalRConnection(): void {
    this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection started');
      })
      .catch((err) => console.error('Error starting SignalR connection:', err));
  }


  stopSignalRConnection(): void {
    this.hubConnection.stop()
      .then(() => {
        console.log('SignalR connection stopped');
      })
      .catch((err) => console.error('Error stopping SignalR connection:', err));
  }

  // Method to send a message to the SignalR server
  sendSignalRMessage(message: string): void {
    this.hubConnection.invoke(environment.signalrNotificationMethodName, 'Srijon', 'Hello!')
      .catch((err) =>
        console.error('Error sending message:', err),
      );
  }

  processIncomingMessage(): void {
    //this.hubConnection.on('ReceiveMessage', (user:string,message: string) => {
    //  console.log('Received message:', message);
    //  this.dataSubject.next(message);
    //});
    this.hubConnection.on('TPP', (user: string, message: string) => {
      //this.messages.push(`${user}: ${message}`);
      console.log(`${user}: ${message}`);
    });
  }
}
