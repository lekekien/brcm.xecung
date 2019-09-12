var Select2 = {
    init: function () {
        $("select.m-select2").each(function () {
            var _this = $(this);
            
            var txtPlaceholder = _this.data("placeholder");
            var dropdownParent = _this.data("dropdownparent");
            if (txtPlaceholder == '' || txtPlaceholder == null || txtPlaceholder == undefined) {
                txtPlaceholder = "--Select an options--";
            }
            _this.off('change');
            _this.select2({
                placeholder: txtPlaceholder,
                allowClear: true,
                dropdownParent: $(dropdownParent)
            });
        })
    }
};
jQuery(document).ready(function () { Select2.init() });