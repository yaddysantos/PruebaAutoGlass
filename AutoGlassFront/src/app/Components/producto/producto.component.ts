import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-producto',
  templateUrl: './producto.component.html',
  styleUrls: ['./producto.component.css']
})
export class ProductoComponent implements OnInit {

  accion = "Agregar";
  // form: FormGroup;

  constructor() {
    
  }

  ngOnInit(): void {
  }

  GuardarProducto(){

  }
}
