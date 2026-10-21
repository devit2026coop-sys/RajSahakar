var MasterMod = angular.module('PLDBLoanMastersMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PLDBLoanMastersCONT", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.SMS = {
        Message: ''
    }
    $scope.ClearSearch = function () {
        $scope.q = "";
    }
    $scope.SendSMSPopUp = function () {
        $("#addModal").modal("show");
    }
    $scope.Back = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");
    }
    $scope.ResetAddNew = function () {
        $scope.SMS.Message = '';
    }

    // on page load
    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $scope._language = 'hi-in';
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
            // $(".se-pre-con").fadeOut("slow");
        }
        $(".blockUI").fadeIn("slow");
        $scope.GetAllApprovedUnitList();
        $(".blockUI").fadeOut("slow");
    };

    if ($scope.checkForSMS == true) {
        alter("hello")
    }

    $scope.UserReqData = [];
    $scope.GetAllApprovedUnitList = function () {

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/User/GetUserDetailList").
            success(function (data, status) {
                $scope.UserReqData = data;
                $(".se-pre-con").fadeOut("slow");

                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };


                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {

            });
    }

    $scope.checkUncheckAll = function () {


        if ($scope.checkall) {
            $scope.checkall = true;
            $scope.SMSButton = true;
        } else {
            $scope.checkall = false;
            $scope.SMSButton = false;
        }
        angular.forEach($scope.UserReqData, function (user) {
            user.checked = $scope.checkall;
        });
    };

    $scope.SMSButton = false;

    $scope.updateCheckall = function ($index, user) {

        var userTotal = $scope.UserReqData.length;
        var count = 0;
        angular.forEach($scope.UserReqData, function (item) {
            if (item.checked) {
                count++;
            }
        });


        if (count > 0) {
            $scope.SMSButton = true;
        }
        else {
            $scope.SMSButton = false;
        }

        if (userTotal == count) {
            $scope.checkall = true;
        } else {
            $scope.checkall = false;

        }
    };

    $scope.selectedData =[];
    $scope.selectedItemsUserID = [];
    $scope.selectedItemsMobileNo = [];

    $scope.SaveAndSendSMS = function () {
        if ($scope.ValidateMesssage()) {

            $(".blockUI").fadeIn("slow");

            var userTotal = $scope.UserReqData.length;
            var count = 0;
            angular.forEach($scope.UserReqData, function (item) {
                if (item.checked) {
                    $scope.selectedItemsUserID.push(item.UserId);
                    $scope.selectedItemsMobileNo.push(item.MobileNo);
                    count++;
                }

            });

            var formData = {
            }
            $http.post(SiteUrl + "/User/SaveAndSendSMS", { selectedItemsUserID: $scope.selectedItemsUserID, selectedItemsMobileNo: $scope.selectedItemsMobileNo, message: $scope.SMS.Message }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");

                    if (data.Status == "-1") {

                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                        $scope.selectedItemsUserID = [];
                        $scope.selectedItemsMobileNo = [];
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Successfully submited." : "सफलतापूर्वक सबमिट किया गया", function () {
                            $("#addModal").modal("hide");
                            window.location.reload();
                            $(".blockUI").fadeOut("slow");
                        });
                    }

                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }

   }
    

$scope.ValidateMesssage = function () {

    var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
    
    if ($scope.SMS.Message == "" || $scope.SMS.Message == undefined) {
        message += _language == 'en-us' ? '\n <br /> Please Enter the SMS Message .' : '\n <br /> कृपया SMS संदेश दर्ज करें।';
        $("#Message").addClass("notValid");
    }
    else {
        $("#Message").removeClass("notValid");
    }

    
    if (message.length > 29) {
        ShowModelAlert("Send SMS To User", message);
        return false;
    }
    return true;
}

 

$scope.OnPageLoad();


});






