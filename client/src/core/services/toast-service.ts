import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  
  constructor (){
    this.createToastContainer();
  };

  // method to create toast container
  private createToastContainer(){
    if(!document.getElementById('toast-container')){
      const container = document.createElement('div')
      container.id = 'toast-container'
      container.className = 'toast toast-bottom toast-end'
      document.body.appendChild(container);
    }
  }

  private createToastElement(message : string , alertClass: string , duration = 5000){
    const toastContainer = document.getElementById('toast-container');

    if(!toastContainer)return;
    
    const toast = document.createElement('div')
    toast.classList.add('alert', alertClass , 'shadow-lg');
    toast.innerHTML=`
      <span>${message}</span>
      <button class = "ml-4 btn-sm btn-ghost">x</button>

    `

    toast.querySelector('button')?.addEventListener('click',()=>{
      // remove the toast from the container when x is cliced
      toastContainer.removeChild(toast);
    });

    // to let the toast append to  the container
    toastContainer.append(toast)

    setTimeout(()=> {
      if(toastContainer.contains(toast)){
        toastContainer.removeChild(toast);
      }
    },duration);
  }


  success(message:string , duration?: number){
    this.createToastElement(message,'alert-success',duration)
  }

    error(message:string , duration?: number){
    this.createToastElement(message,'alert-error',duration)
  }
    warning(message:string , duration?: number){
    this.createToastElement(message,'alert-warning',duration)
  }
    info(message:string , duration?: number){
    this.createToastElement(message,'alert-info',duration)
  }

}
