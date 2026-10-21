var SupportData = angular.module("Support", []);
SupportData.controller("SData", function ($scope, $http) {
    $scope.SupportUsername = "";
    $scope.SupportPassword = "";
    $scope.Support = function () {
        $http.post(SiteUrl + "/GetSupport/GetSupportData", { "_Model": $scope.SupportList, "Username": $scope.SupportUsername, "Password": $scope.SupportPassword }).//$scope.userrole
            success(function (data, status) {
                if (data.length > 0) {
                    if (data == "Error") {
                        alert("Please check Username and Password")
                        $scope.myJson = "";
                        $scope.range = [];
                    }
                    else {
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
                }
                else
                {
                    $scope.myJson = "";
                    $scope.range = [];
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

    $scope.OnPageLoad();
});