var ElectionProgrammeModule = angular.module('ElectionProgrammeMod', ['angularUtils.directives.dirPagination']);
ElectionProgrammeModule.controller("ElectionProgrammeCnt", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.OTP = '';
    $scope.TXN = '';
    // on page load
    $scope.OnPageLoad = function () {
        $(".blockUI").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
            //$(".se-pre-con").fadeOut("slow");
        }
    };

    // START :Get List of Societies
    $scope.GetSocietyListForElectionOfficer = function () {
        $(".blockUI").fadeIn("slow");
        $http.post("../Election/GetElectionPrograameForSociety/").
            success(function (data, status) {
                $scope.ElectionOfficerData = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                alert('Error while fetching Society List For Election Officer.');
                $(".blockUI").fadeOut("slow");
            });
    };
    // END :Get List of Societies

    $scope.OnPageLoad();
    $scope.GetSocietyListForElectionOfficer();
});


var ElectionProgrammePublishModule = angular.module('ElectionProgrammePublishMod', ['angularUtils.directives.dirPagination']);
ElectionProgrammePublishModule.controller("ElectionProgrammePublishCnt", function ($scope, $http, $location) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    // on page load
    $scope.OnPageLoad = function () {
        $(".blockUI").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Error while translation");
                    $(".blockUI").fadeOut("slow");
                });
            //$(".se-pre-con").fadeOut("slow");
        }
    };

    // START :Get List of Societies
    $scope.GetPublishedData = function () {
        var id = $location.absUrl().split('?')[1].replace('Id=', '');
       // alert(id);
        $(".blockUI").fadeIn("slow");
        $http.post("/Election/GetElectionProgrammeData", { "Id": id}).
            success(function (data, status) {
                $scope.PublishData = data;
                console.log(data);
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                alert('Error while fetching Society List For Election Officer.');
                $(".blockUI").fadeOut("slow");
            });
    };
    // END :Get List of Societies

    // START :Get List of Societies
    $scope.Publish = function () {
        var id = $location.absUrl().split('?')[1].replace('Id=', '')
        $(".blockUI").fadeIn("slow");
        $http.post("../Election/GenPDFHinForElectionInfo", { "Id": id }).
            success(function (data, status) {
                debugger;
                if (data == "PDF Not Generate") {
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be Publishing. Please try after some time." : "आदेश प्रकाशित नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                } else
                    if (data.indexOf('NO#') >= 0) {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be Publishing. OTP is not being sent for the sign due to some problem. Please try after some time." : "आदेश प्रकाशित नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    } else {
                        if (data != "LOGOUT") {
                            // alert(data);
                            $('#lbltrn').html(data);
                            $('#ShowDateDiv').hide();
                            $('#ShowOTPDiv').show();

                            ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });
                        }
                        else {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        }
                    }
                // window.location = '../Election/ElectionSocietyList';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                ShowOkMessage("", 'Error while Publishing Election Programme Order PDF.', function () { });
                $(".blockUI").fadeOut("slow");
            });
    };
    // END :Get List of Societies
    $scope.VerifyOTP = function () {
        debugger;
        var id = $location.absUrl().split('?')[1].replace('Id=', '')
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/VerifyAadhaarOTPForElectionPublish", { "OTP": $scope.OTP, "TXN": $('#lbltrn').html(), "ElectionMasterIdEnc": $('#btnautoclick').val(), Id: id}).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data != "Success") {
                        ShowOkMessage("", data, function () { });
                    }
                    else if (data == "Success") {
                        ShowOkMessage("", 'Society election order successfully published', function () {
                            window.location = '../Election/ElectionProgrammeForSociety';
                        });
                     
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
        $scope.showLoader = false;
    };

    $scope.OnPageLoad();
    $scope.GetPublishedData();
});



