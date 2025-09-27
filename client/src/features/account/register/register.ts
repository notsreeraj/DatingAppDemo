import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  private accountService = inject(AccountService) 
  // here we are specifiyign this property as an output property of event
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register(){
    this.accountService.register(this.creds).subscribe({
      // next is one of the property of the object passed in
      next: response =>{
          console.log(response);
          this.cancel();
      },
      error: error=>console.log(error)
    })
  }

  // emuting the boolean variable outside the component , which is revieved by home comonent
  // it is emited as an event
  cancel(){
    this.cancelRegister.emit(false);
  }
}
