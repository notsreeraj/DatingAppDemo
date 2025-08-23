import { Injectable ,inject } from '@angular/core';
import { AccountService } from './account-service';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private accountService = inject(AccountService);



init (){
    const userString = localStorage.getItem('user');
    if(!userString) return of(null);

    const user = JSON.parse(userString);
    // parsign json to json object
    this.accountService.currentUser.set(user);

    // returning an obsevable of
    return of (null)
}

}
