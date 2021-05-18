export class DataItem {
 trainNumber: string;
  stationName: string;
  minuteDelay: number;
  scheduledTime: string;
  isBold: boolean

  constructor(trainNumber: string, stationName: string, minuteDelay: number, scheduledTime: string, isBold: boolean) {
    this.trainNumber = trainNumber
    this.stationName = stationName
    this.minuteDelay = minuteDelay
    this.scheduledTime = scheduledTime
    this.isBold = isBold
  }
}
