import {months} from "../../assets/months";
import seedrandom from "seedrandom";
import {PostDTO} from "../models/post/postDTO";
import {UserDTO} from "../models/user/userDTO";
import {PagePostDTO} from "../models/post/pagePostDTO";
import {CommentDTO} from "../models/coment/commentDTO";
import {Random} from "random";
export default class PostFormatter{

  public static isInputLengthTooBig(input: string): boolean {
    return input.length > 500;
  }

  public static numberToReadable(number: number): string {
    if(number < 1000) return number.toString();
    if(number < 1000000) return Math.floor(number/100)/10 + "K";
    if(number < 1000000000) return Math.floor(number/100000)/10 + "M";
    return Math.floor(number/100000000)/10 + "B";
  }

  public static mapPostToPagePost(postInput: PostDTO): PagePostDTO {
    return  {
      id: postInput.id,
      user: {
        id: postInput.author.id,
        username: postInput.author.username,
        avatar: postInput.author.avatar,
        bio: postInput.author.bio,
        followers: postInput.author.followersIds.length,
        following: postInput.author.followingIds.length
      },
      textContent: postInput.textContent,
      dateCreated: postInput.createdAt,
      imagesUrls: postInput.images.map(i => i.url),
      likesAmount: postInput.likesIds.length,
      commentsAmount: postInput.commentsIds.length,
      repostsAmount: postInput.repostersIds.length,
      viewsAmount: postInput.viewedBy.length,
      bookmarksAmount: postInput.bookmarks
    }
  }

  public static mapCommentToPagePost(commentInput: CommentDTO, userInput: UserDTO): PagePostDTO {
    var rnd = new Random();
    let numberOfImages = rnd.int(1, 4);
    let imagesUrls : string[] = [];
    for(let i = 0; i < numberOfImages; i++){
      imagesUrls.push("https://picsum.photos/500/500")
    }
    return  {
      id: commentInput.id,
      user: {
        id: userInput.id,
        username: userInput.username,
        avatar: userInput.avatar,
        bio: userInput.bio,
        followers: userInput.followersIds.length,
        following: userInput.followingIds.length
      },
      textContent: commentInput.textContent,
      dateCreated: commentInput.createdAt,
      imagesUrls: commentInput.images.map(i => i.url),
      likesAmount: commentInput.likesIds.length,
      commentsAmount: commentInput.commentsIds.length,
      repostsAmount: 0,
      viewsAmount: commentInput.viewedBy.length,
      bookmarksAmount: 0
    }
  }

  public static getCircleColor(username:string): string {
    const colorArray = ["red", "green", "yellow", "purple", "cornflowerblue", "orange", "blue"];
    return colorArray[Math.floor(seedrandom(username).double() * colorArray.length)];
  }
  public static getDateFormattedElapsed(date: Date): string {
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

  public static getTimeFormatted(date: Date): string {
    return date.getHours() + ":" + (date.getMinutes() < 10 ? "0" : "") + date.getMinutes() + " " + (date.getHours() < 12 ? "AM" : "PM");
  }

  public static getDateFormatted(date: Date): string {
    return months[date.getMonth()].name.substring(0, 3) + " " + date.getDate() + ", " + date.getFullYear();
  }

  private static minutesToReadable(date: Date) {
    return Math.floor((Date.now() - date.getTime()) / 60000) + "m";
  }
}
