import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';


@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected  title = 'Dating App';
  // here the signal is used to update the fine grain changes automatically
  protected members =signal<any>([])

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
  }

  async getMembers(){
    try{
        return lastValueFrom(this.http.get('https://localhost:5001/api/members'))
    }
    catch (error){
      console.log(error);
    }
     
  }
} // class ends here
