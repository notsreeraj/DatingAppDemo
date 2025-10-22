import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCreds, RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';


// this class is injectable
// sevices are singletons
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  
  private http = inject(HttpClient);
  // union type , it can null or user
  currentUser = signal<User | null>(null);

   private baseUrl= environment.apiUrl
 

  register(creds: RegisterCreds){
    return this.http.post<User>(this.baseUrl+"account/register",creds).pipe(
      tap(user =>{
        if(user){
          this.setCurrentUser(user)
        }
      })
    )
  }


  login(creds:LoginCreds){
  // the pip key word is a Rsjx operator
  return this.http.post<User>(this.baseUrl + 'account/login', creds).pipe(
    tap(user => {
      console.log("this is from inside if use condition in account-service.ts")
      if(user){
        
        this.setCurrentUser(user);
        
      }
    })
  );
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUser.set(null) ;
  }

  // method to set current user
  setCurrentUser(user : User){
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUser.set(user);
  }

  getCurrentUser(){
     return this.currentUser()?.displayName;
  }

}// class ends here


