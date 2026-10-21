var SupportData = angular.module("Support", []);
SupportData.controller("SData", function ($scope, $http) {
    $scope.Support = function () {
        $http.post("https://rajsahakar.rajasthan.gov.in/GetSupport/GetSupportData", $scope.SupportList).//$scope.userrole
            success(function (data, status) {
                if (data.length > 0) {
                    if (data == "-9") {
                        alert('Query Does Not Contain Where Clause');
                        return false;
                    }
                    var myJson = data;
                    var keycount = Object.keys(myJson[0]);
                    var range = [];
                    $scope.ShowbtnExport = true;
                    for (var i = 0; i < keycount.length; i++) {
                        range.push(keycount[i]);
                    }
                    $scope.myJson = myJson;
                    $scope.range = range;
                }
                else {
                    alert('Data NoT found');
                }
            }).
            error(function (data, status) {
                alert("Error while Load SupportList");
            });
    };

    $scope.ClearForm = function () {
        $scope.SupportList = null;
    };

    $scope.OnPageLoad = function () {

    };

    $scope.UserName = "";
    $scope.Password = "";
    $scope.showfireDiv = false;
    $scope.showloginDiv = true;
    $scope.Login = function () {

        $scope.showfireDiv = true;
        $scope.showloginDiv = false;


        //$http.post(SiteUrl +"/Home/Checklogin", { "Username": $scope.UserName, "Password": $scope.Password }).
        //    success(function (data, status) {
        //        if (data == 1) {
        //            $scope.showfireDiv = true;
        //            $scope.showloginDiv = false;
        //        }
        //        else {
                   
        //            $scope.showfireDiv = false;
        //            $scope.showloginDiv = true;
        //            alert("Please Check Your UserName And Password !");
        //            window.location = "/home/getsupport";
        //        }
        //    });
    };
    $scope.OnPageLoad();
});