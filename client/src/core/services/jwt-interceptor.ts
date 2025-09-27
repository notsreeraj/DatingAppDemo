import { HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from './account-service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService)

  // this is copy of the currentuser signal. it is not reactive
  const user= accountService.currentUser();


  if(user){
    req = req.clone({
      setHeaders:{
        Authorization : `Bearer ${user.token}`
      }
    })
  }
  return next(req);
};
