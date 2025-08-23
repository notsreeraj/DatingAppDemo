// imports httpclient service from angular application
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';

import { Nav } from "../layout/nav/nav";

import { Router, RouterOutlet } from "@angular/router";



@Component({
  // this is the tag to be used to render this component in a html page
  selector: 'app-root',
  // these would be the components used insied this component
  imports: [Nav, RouterOutlet],
  // this would be the location of the html file for this component
  // imaging that all these are a single file
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App  {
  // private http = inject(HttpClient);
  protected router = inject(Router)

  // protected  title = 'Dating App';
  // // here the signal is used to update the fine grain changes automatically
  // protected members =signal<User[]>([])

  // // this is the Constructoe for the app component
  // // here the HttpClient type object or anything is passed into the app componet 
  // // in the contructor itself by the name of http.
  // // hence this is dependancy injection
  // // there is a newer way to do this on top
  // constructor(private http : HttpClient){

  // }
    // func which runs on initializaion
  //   async ngOnInit() {
  //   // to acces something inside the class or the component use "this" key word
  //   // this get method returns an observable of the repsonse body of the js object
  //   // read the documentation. hover over the method
  //   // this.http.get('https://localhost:5001/api/members').subscribe({
  //   //   // here next is the property of the observable object passed into .subscibe method
  //   //   next: response => this.members.set(response),
  //   //   error : error => console.log(error),
  //   //   complete: ()=> console.log("Completed the http request")
  //   // })
  //   this.members.set(await this.getMembers())

  // }


} // class ends here
