import { Component, OnInit } from '@angular/core';
import { OtpPrivateKey, UrlOtp } from 'src/app/models/otpprivatekey';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute } from '@angular/router';
import { ResponseCode } from 'src/app/enums/responsecode';
import { ServerResponse } from 'src/app/models/response';
declare var showMeassage: any;
@Component({
  selector: 'app-otpprivatekey',
  templateUrl: './otpprivatekey.component.html',
  styleUrls: ['./otpprivatekey.component.css']
})
export class OtpprivatekeyComponent implements OnInit {
  dataQRCode: OtpPrivateKey;
  urlOtpModels: UrlOtp;
  constructor(
    private userService: UserService,
    private activeRouter: ActivatedRoute,
  ) {
    this.dataQRCode = new OtpPrivateKey();
    this.urlOtpModels = new UrlOtp();
   }

  ngOnInit() {
    this.activeRouter.params.subscribe(params => {
      const urlParams = params.param;
      if (urlParams !== undefined && urlParams !== null) {
        this.urlOtpModels.UrlOtp = urlParams;
        this.userService.getOTP(this.urlOtpModels, (res: ServerResponse) => {
          if (res.Code === ResponseCode.Success) {
            this.dataQRCode = res.Data.OTPData as OtpPrivateKey;
          } else {
            showMeassage.showWarning(res.Message);
          }
        });
      }
    });
  }
}
