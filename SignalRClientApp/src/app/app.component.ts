import { Component } from '@angular/core';
import { SignalRNotificationService } from '../services/signalr-services/signalrNotification.service';
import * as signalR from '@microsoft/signalr';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SignalRClientApp';
  constructor(
    private signalRService: SignalRNotificationService
  )
  {
  }
  ngOnInit(): void {
    this.signalRService.stopSignalRConnection();
    this.signalRService.startSignalRConnection();
    this.signalRService.processIncomingMessage();
    this.signalRService.data$.subscribe((data: string) => {
      this.incomingMessage(data);
    });
  }
  ngOnDestroy(): void {
    this.signalRService.stopSignalRConnection();
  }
  sendMessageToServer(message: string): void {
    this.signalRService.sendSignalRMessage(message);
  }

  incomingMessage(data: string): void {
    console.log(data);
  }
}
