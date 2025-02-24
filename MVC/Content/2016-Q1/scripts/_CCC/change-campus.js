﻿//************************************************
// CHANGE CAMPUS
//***********************************************/
(function (CCChapel, $, undefined) {
    /************************************************
    // Private Properties
    //***********************************************/
    var linkClass = ".change-campus";
    var dataAttr = "data-campus";

    //************************************************
    // Public Methods
    //***********************************************/
    CCChapel.setupCampusLinks = function () {
        $("a.change-campus").each(function () {
            var newDomain = "";

            //Get Current Domain
            var currentDomain = window.location.hostname;

            //Get Campus to Change To
            var newCampus = $(this).attr(dataAttr);

            if (newCampus !== undefined) {
                newCampus = newCampus.replace(" ", "").replace("-", "").toLowerCase();

                //Break Into Pieces
                var pieces = currentDomain.split(".");

                //Check for subdomain
                if (pieces.length == 2) {
                    //No subdomain
                    newDomain = newCampus + "." + currentDomain;
                }
                else if (pieces.length == 3) {
                    //Existing subdomain
                    newDomain = newCampus + "." + pieces[1] + "." + pieces[2];
                }

                /* EDIT: 4.7
                //Create full URL
                var newUrl = location.protocol + "//" + newDomain + window.location.pathname;
                */

                //Create Path
                var path = "/new-here";
                switch (newCampus) {
                    case "hudson":
                        path += "/hudson";
                        break;
                    case "aurora":
                        path += "/aurora";
                        break;
                    case "stow":
                        path += "/stow";
                        break;
                    case "highlandsquare":
                        path += "/highland-square";
                        break;
                }

                var newUrl = location.protocol + "//" + newDomain + path;

                //Set Href
                $(this).attr("href", newUrl);
            }
            else {
                console.error("Link is missing data-campus attribute: " + $(this).attr("class"));
            }
        })
    }
}(window.CCChapel = window.CCChapel || {}, jQuery));