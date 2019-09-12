import { Injectable } from '@angular/core';
declare var $: any;
@Injectable({
  providedIn: 'root'
})
export class BlockUiService {

  constructor() { }
  block(uiElement: any = null) {
    if (uiElement == null) {
      uiElement = $('#main-content');
    }

    uiElement.block({
      message: '<img src="./assets/images/ajax-loading.gif" align="">',
      centerY: true,
      css: {
          top: '10%',
          border: 'none',
          padding: '2px',
          width: '100%',
          backgroundColor: 'none'
      },
      overlayCSS: {
          backgroundColor: '#000',
          opacity: 0.05,
          cursor: 'wait'
      }
    });
  }
  unblock(uiElement: any = null) {
    if (uiElement == null) {
      uiElement = $('#main-content');
    }
    uiElement.unblock({
      onUnblock: function () {
          if (!$('#main-content').is('.modal, .in')) {
            $('#main-content').removeAttr('style');
          }
      }
    });
  }
}
