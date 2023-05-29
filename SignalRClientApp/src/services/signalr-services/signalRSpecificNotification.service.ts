import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class SignalRSpecificNotificationService {
  private hubConnection: signalR.HubConnection;
  private dataSubject: Subject<string> = new Subject<string>();
  public data$: Observable<string> = this.dataSubject.asObservable();
  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalrSpecificNotificationUrl) // Replace with your SignalR server URL
      .build();
    this.oncloseCheck();
   
  }
  oncloseCheck() {
    this.hubConnection.onclose(() => {
      console.log('SignalRSpecificNotificationService connection closed');
    });
  }
  startSignalRConnection(): void {
    this.hubConnection.start()
      .then(() => {
        console.log('SignalR connection started');
      })
      .catch((err) => console.error('Error starting SignalRSpecificNotification connection:', err));
  }

  stopSignalRConnection(): void {
    this.hubConnection.stop()
      .then(() => {
        console.log('SignalRSpecificNotification connection stopped');
      })
      .catch((err) => console.error('Error stopping SignalRSpecificNotification connection:', err));
  }

  sendSignalRMessage(message: string, subscriptionId: string): void {
    this.hubConnection.invoke(environment.signalrSpecificNotificationMethodName, message, subscriptionId)
      .catch((err) =>
        console.error('SignalRSpecificNotification: Error sending message:', err),
      );
  }
  processIncomingMessage(): void {
    this.hubConnection.on(environment.specificNotificationReceiveMethod, (message: string) => {
      this.dataSubject.next(message);
      //console.log(`${message}`);
    });
  }
}
