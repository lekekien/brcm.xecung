import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { DataSharingService } from 'src/app/services/data-sharing.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, AfterViewInit {
  constructor(
    private router: Router,
    private dataSharingService: DataSharingService
  ) { }

  ngOnInit() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.dataSharingService.isUserLoggedIn.next(true);
      }
    });
  }

  ngAfterViewInit(): void {
    window.addEventListener('storage', (e) => {
      if (e.key === 'currentUser') {
        setTimeout(() => { location.reload(); }, 5000);
      }
    });
  }
}
