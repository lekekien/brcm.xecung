import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base/base.component';
import { Role } from 'src/app/enums/roles';
import { DataSharingService } from 'src/app/services/data-sharing.service';
import { AuthenticatedUser } from 'src/app/models/authenticateduser';

@Component({
  selector: 'app-menu-bar',
  templateUrl: './menu-bar.component.html',
  styleUrls: ['./menu-bar.component.css']
})
export class MenuBarComponent extends BaseComponent implements OnInit {

  isSaler: boolean;
  userHeader: AuthenticatedUser;
  constructor(private dataSharingService: DataSharingService) {
    super();
    this.dataSharingService.isUserLoggedIn.subscribe(value => {
      this.isAdmin = this.hasRole(Role.Admin);
      this.isManager = this.hasRole(Role.Manager);
      //this.isSaler = this.hasRole(Role.Saler);
    });
  }

  ngOnInit() {
    $(document).on('click', (e) => {
      if ($(e.target).closest('.main-sidebar').length === 0 && $(e.target).closest('.main-header').length === 0) {
        $('body').removeClass('sidebar-open');
      }
    });

    let lastScrollTop = 0;
    let st = 0;
    const top = $('.navbar').offset().top;
    $('.main-header .logo').addClass('fixedInWeb');
    $(window).on('scroll', function() {
        st = $(this).scrollTop();
        if (st < lastScrollTop) {
          if ($(this).scrollTop() > top) {
            $('.logo').addClass('hide-element-phone');
            $('.main-header').addClass('fixMenu');
          } else {
            $('.logo').removeClass('hide-element-phone');
            $('.main-header').removeClass('fixMenu');
          }
        } else {
          $('.logo').addClass('hide-element-phone');
          $('.main-header').removeClass('fixMenu');
        }
        lastScrollTop = st;
    });
  }

  closeMenu() {
    $('body').removeClass('sidebar-open');
  }
}
