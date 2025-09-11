import { HttpInterceptorFn } from '@angular/common/http';
import { ToastService } from './toast-service';
import { inject, model } from '@angular/core';
import { catchError } from 'rxjs';
import { routes } from '../../app/app.routes';
import { NavigationExtras, Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const toast = inject(ToastService);
  const router = inject(Router)
  return next(req).pipe(
    catchError(error =>{
      if(error){
        switch(error.status){
          case 400:
            if(error.error.errors){
              const modelStateErrors = [];

              for(const key in error.error.errors){
                if(error.error.errors[key]){
                  modelStateErrors.push(error.error.errors[key])
                }
                
              }

              throw modelStateErrors.flat()
            }
            else{
              toast.error(error.error)
            }
            break;
          
          case 401 :
            toast.error("Unauthorized")   
            break;

          case 404 :
            alert("404 errror caught")
            router.navigateByUrl('/not-found')
            break;

          case 500 :
            // setting up navigation extras
            alert(error.error.statusCode)
            const navigationExtra : NavigationExtras = {state:{error :error.error}}
            router.navigateByUrl('/server-error',navigationExtra)
            break;

            default :
            toast.error("Unfortunately Something happened")
            break;

        }
      }

      throw error;
    })
  );
};
