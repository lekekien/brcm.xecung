import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, ChangeDetectionStrategy, OnChanges } from '@angular/core';
import { DatePipe } from '@angular/common';
import { BsDaterangepickerDirective, BsDatepickerConfig, BsDatepickerViewMode } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-datepickercustom',
  templateUrl: './datepickercustom.component.html',
  styleUrls: ['./datepickercustom.component.css'],
  providers: [DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DatepickercustomComponent implements OnInit, OnChanges {

  @Input() idSelector = '';
  @Input() nameSelector: string;
  @Input() placeholderSelector = '';
  @Input() monthMode = false;
  @Input() defaultValue: string;
  @Output() changedModelDate = new EventEmitter();
  @Input() isBirthday = false;
  minMode: BsDatepickerViewMode;
  bsConfig: Partial<BsDatepickerConfig>;
  pipe: DatePipe;
  dateInputFormat: string;
  dateFormat: string;
  bsValueDate: Date;
  minDate: Date;
  maxDate: Date;
  constructor(private cdRef: ChangeDetectorRef) {
    this.pipe = new DatePipe('en-US');
    this.minDate = new Date('0001-01-01T00:00:00');
    this.maxDate = new Date();
  }
  ngOnInit() {
    if (this.monthMode) {
      this.minMode = 'month';
      this.dateInputFormat = 'MM/YYYY';
      this.dateFormat = 'MM/yyyy';
    } else {
      this.minMode = 'day';
      this.dateInputFormat = 'MM/DD/YYYY';
      this.dateFormat = 'MM/dd/yyyy';
    }
    this.bsConfig = Object.assign({}, {
      minMode: this.minMode,
      dateInputFormat: this.dateInputFormat,
      selectWeek: true,
      selectFromOtherMonth: true,
      maxDate: this.maxDate,
      customTodayClass: 'todayClass-datepiker'
    });  
    const date = new Date(this.defaultValue);
    if (this.defaultValue !== '' && this.defaultValue !== null && this.defaultValue !== undefined && date > this.minDate) {
      this.bsValueDate = date;
    } else {
      this.bsValueDate = null;
    }
    // Nếu là birthday thì maxDate sẽ là hiện tại - 10 năm
    if (this.isBirthday) {
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    }
  }
  ngOnChanges() {
    if(this.defaultValue !== null && this.defaultValue !== undefined)
    {
      this.bsValueDate = new Date(this.defaultValue);
    } 
  }
  changedDate(value: Date) {
    if (value === null || value.getTime() === this.minDate.getTime()) {
      this.changedModelDate.emit(null);
      this.bsValueDate = null;
    } else {
      this.changedModelDate.emit(this.formateDate(value.toString()));
      this.bsValueDate = value;
    }
  }
  formateDate(stringDate: string) {
    const date = new Date(stringDate);
    stringDate = date > this.minDate ? stringDate : '';
    if (stringDate !== '') {
      stringDate = this.pipe.transform(stringDate, this.dateFormat);
    }
    return stringDate;
  }
}
