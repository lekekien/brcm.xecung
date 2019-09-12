// Sweet Alert
var showMessage = {
   showSuccess: function (msg, title = "", func) {
      swal(title, msg, "success")
         .then(function (e) {
            if (func != null && func != undefined && func != "") {
               func();
            }
         });
   },
   showWarning: function (msg, title = "") {
      swal(title, msg, "warning");
   },
   showError: function (msg, title = "") {
      swal(title, msg, "error");
   },
   showConfirm: function (mes, func, title = "") {
      swal({
         title: title,
         text: mes,
         type: "question",
         showCancelButton: true,
         cancelButtonText: "No",
         cancelButtonClass: 'btn btn-danger m-btn m-btn--custom',
         confirmButtonText: "Yes!",
      }).then(
         function (e) {
            if (e.value) {
               func();
            }
         })
   }
}
//Custom Close Model
var CloseModal = {
   close: function () {
      $("button.close").trigger('click');
   }
}
//Custom switch tab
var SwitchTab = {
   init: function () {
      $(".switch-tab").on("click", function () {
         if (!$(this).hasClass(".m-tabs__item--active")) {
            $(".switch-tab").removeClass("m-tabs__item--active");
            $(".switch-tab-content").removeClass("m-tabs-content__item--active");
            var id = $(this).data("tab-target");
            $(this).addClass("m-tabs__item--active");
            $(id).addClass("m-tabs-content__item--active");
         }
      });
   }
}
//Custom sidebar
var CustomerSideBar = {
   init: function () {
      if ($("body.sidebar-collapse").length > 0) {
         $("body").find(".pe-7s-angle-right-circle").removeClass("pe-7s-angle-right-circle").addClass("pe-7s-angle-left-circle");
      }else{
         $("body").find(".pe-7s-angle-left-circle").removeClass("pe-7s-angle-left-circle").addClass("pe-7s-angle-right-circle");
      }
   }
}
// Get size monitor
var SizeMonitor ={
   init: function(){
      var widthMornitor = $(document).width();
      return widthMornitor;
   }
}
