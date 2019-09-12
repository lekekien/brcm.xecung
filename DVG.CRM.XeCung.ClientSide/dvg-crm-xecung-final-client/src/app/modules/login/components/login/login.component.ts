import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { GetdropdownlistService } from 'src/app/services/getdropdownlist.service';
import { first } from 'rxjs/operators';
import { ResponseCode } from 'src/app/enums/responsecode';
import { ServerResponse } from 'src/app/models/response';
import { ListLocation } from 'src/app/models/listdropdown';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';
  warring = '';
  checkFailValid = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private location: GetdropdownlistService
  ) {

  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      otpprivatekey: ['', Validators.required]
    });
    // get return url from route parameters or default to '/'
    // tslint:disable-next-line:no-string-literal
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }
  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.authenticationService.login(this.f.username.value, this.f.password.value, this.f.otpprivatekey.value)
      .pipe(first())
      .subscribe(
        data => {
          this.loading = false;
          if (data.Code === ResponseCode.Success) {
            this.router.navigate([this.returnUrl]);          
          } else {
            this.checkFailValid = true;
            this.warring = data.Message;
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }
}
