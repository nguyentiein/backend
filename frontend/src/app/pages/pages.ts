import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from "./_component/header/header";
import { Toolbar } from "./_component/toolbar/toolbar";

@Component({
  selector: 'app-pages',
  imports: [RouterOutlet, Header],
  templateUrl: './pages.html',
  styleUrl: './pages.css',
})
export class Pages {

}
