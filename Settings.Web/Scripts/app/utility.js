define('utility-js', ['jquery', 'toastr'], function ($, toastr) {
    'use strict';
    return {
        showToast: function (type, message) {
            toastr.options.positionClass = "toast-bottom-center";
            toastr[type](message);
        }
    };
});