import { Component, OnInit } from '@angular/core';
import { HomeService } from 'src/app/services/home.service';
import { ServerResponse } from 'src/app/models/response';

@Component({
  selector: 'app-backend',
  templateUrl: './backend.component.html'
})
export class BackendComponent implements OnInit {

  constructor(private homeService: HomeService) { }

  ngOnInit() {
    this.homeService.index((res: ServerResponse) => {});
  }
}
