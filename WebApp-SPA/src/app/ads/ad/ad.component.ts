import { Component, OnInit, Input } from '@angular/core';
import { Ad } from 'src/app/_models/ad';

@Component({
  selector: 'app-ad',
  templateUrl: './ad.component.html',
  styleUrls: ['./ad.component.css']
})
export class AdComponent implements OnInit {
  @Input() ad: Ad;

  constructor() { }

  ngOnInit() {
  }

}
