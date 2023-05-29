import { Component } from '@angular/core';
import { SignalRNotificationService } from '../services/signalr-services/signalrNotification.service';
import * as signalR from '@microsoft/signalr';
import { SignalRSpecificNotificationService } from '../services/signalr-services/signalRSpecificNotification.service';
import { v4 as uuidv4 } from 'uuid';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SignalRClientApp';
  currentGUID: string="";
  constructor(
    private signalRService: SignalRSpecificNotificationService//SignalRNotificationService
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
    this.currentGUID = uuidv4();
    console.log(this.currentGUID); 
    this.signalRService.sendSignalRMessage(message, this.currentGUID);
  }

  incomingMessage(data: string): void {
    console.log("App Component Incoming:" + data?.toString());
  }
}
