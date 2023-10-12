import {months} from "../../assets/months";
import seedrandom from "seedrandom";
export default class PostFormatter{

  public static isInputLengthInvalid(input: string): boolean {
    return input.length > 500 || input.length === 0;
  }

  public static getCircleColor(username:string): string {
    const colorArray = ["red", "green", "yellow", "purple", "pink", "orange", "blue"];
    return colorArray[Math.floor(seedrandom(username).double() * colorArray.length)];
  }
  public static getDateFormattedString(date: Date): string {
    console.log(date.getHours());
    if(Date.now()-date.getTime() < 3600000){
      return this.minutesToReadable(date);
    }
    if(Date.now()-date.getTime() < 86400000){
      return this.hoursToReadable(date);
    }
    if(Date.now()-date.getTime() < 31536000000){
      return months[date.getMonth()].name.substring(0, 3) + " " + date.getDate();
    }
    return months[date.getMonth()].name.substring(0, 3) + " " + date.getDate() + ", " + date.getFullYear();
  }
  private static hoursToReadable(date: Date) {
    return Math.floor((Date.now() - date.getTime()) / 3600000) + "h";
  }

  private static minutesToReadable(date: Date) {
    return Math.floor((Date.now() - date.getTime()) / 60000) + "m";
  }
}
