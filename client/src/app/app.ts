// imports httpclient service from angular application
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { Home } from '../features/home/home';
import { User } from '../types/user';


@Component({
  // this is the tag to be used to render this component in a html page
  selector: 'app-root',
  // these would be the components used insied this component
  imports: [Nav ,Home],
  // this would be the location of the html file for this component
  // imaging that all these are a single file
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);

  protected  title = 'Dating App';
  // here the signal is used to update the fine grain changes automatically
  protected members =signal<User[]>([])

  // // this is the Constructoe for the app component
  // // here the HttpClient type object or anything is passed into the app componet 
  // // in the contructor itself by the name of http.
  // // hence this is dependancy injection
  // // there is a newer way to do this on top
  // constructor(private http : HttpClient){

  // }
    // func which runs on initializaion
    async ngOnInit() {
    // to acces something inside the class or the component use "this" key word
    // this get method returns an observable of the repsonse body of the js object
    // read the documentation. hover over the method
    // this.http.get('https://localhost:5001/api/members').subscribe({
    //   // here next is the property of the observable object passed into .subscibe method
    //   next: response => this.members.set(response),
    //   error : error => console.log(error),
    //   complete: ()=> console.log("Completed the http request")
    // })
    this.members.set(await this.getMembers())
    this.setCurrentUser();
  }

  // method to load current user from local storage
  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;

    const user = JSON.parse(userString);
    // parsign json to json object
    this.accountService.currentUser.set(user);
  }

  async getMembers(): Promise<User[]> {
    try {
      return await lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/members'));
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
} // class ends here
