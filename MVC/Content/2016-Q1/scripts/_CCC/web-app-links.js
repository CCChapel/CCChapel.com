﻿//************************************************
// WEB APP LINKS
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Methods
    //***********************************************/
    var iWebkit;

    CCChapel.setupWebAppLinks = function () {
        $(document).ready(function () {
            if (("standalone" in window.navigator) && window.navigator.standalone) {
                // For iOS Apps
                $('a').on('click', function (e) {
                    e.preventDefault();

                    var new_location = $(this).attr('href');

                    if (new_location != undefined && new_location.substr(0, 1) != '#' && $(this).attr('data-method') == undefined) {
                        window.location = new_location;
                    }
                });
            }
        });
    };
}(window.CCChapel = window.CCChapel || {}, jQuery));