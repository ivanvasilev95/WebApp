import { Component, Input } from '@angular/core';
import { Ad } from 'src/app/_models/ad';

@Component({
  selector: 'app-ad',
  templateUrl: './ad.component.html',
  styleUrls: ['./ad.component.css']
})
export class AdComponent {
  @Input() ad: Ad;

  constructor() { }
}
