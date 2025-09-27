import { Component ,Input,OnInit,signal} from '@angular/core';
import { Register } from '../account/register/register';
import { inject } from '@angular/core';
import { AccountService } from '../../core/services/account-service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [Register, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {

  // this being a signal lets the angular know that yo the registermode is turned on and you have to chang
  // the content in home.html
  protected registerMode = signal(false);
  protected accountService = inject(AccountService);

  ngOnInit(): void {
    console.log("This is the current user" + this.accountService.getCurrentUser());
    // check if there is already a cuurent user
    if(this.accountService.getCurrentUser()!= null){
      this.registerMode.set(false)
    }
  }
  showRegister(value: boolean){
    this.registerMode.set(value);
  }
}
