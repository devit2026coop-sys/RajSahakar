

var UserMenu = angular.module("AuditOrderListmod", ['angularUtils.directives.dirPagination']);


UserMenu.directive('onlyLettersInput', onlyLettersInput);

function onlyLettersInput() {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z ]/g, '');
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
};

UserMenu.controller("AuditOrdercnt", function ($scope, $http) {


    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 400;
    $scope.OrderReport = [];




    $scope.pageChangeHandler = function (num) {
    };
    //$scope.pldbdata = '';
    $scope.OrderDataList = function (id) {
        
        $(".blockUI").fadeIn("slow");
        $http.get(SiteUrl + "/Auditor/GetOrderListReport").
            success(function (data, status) {
                if (id == undefined) {
                    $scope.OrderReport = data;
                }
                else if (id == 2) {
                    var DData = [];
                    DData = $scope.OrderReport.filter(X => X.RequestStatus === 2);
                    if (DData.length > 0) {
                        $scope.OrderReport = DData;
                        $(".blockUI").fadeOut("slow");
                    }
                }
                else {
                    $scope.OrderReport = data;
                }

            }).
            error(function (data1, status) {
            });
        $(".blockUI").fadeOut("slow");
    }



    $scope.SocietyList = function (id) {
        $(".blockUI").fadeIn("slow");
        $http.get(SiteUrl + "/Auditor/GetOrderListReport/?OrderNo=" + id + "&Flag=" + "1").
            success(function (data, status) {
                $(".blockUI").fadeIn("slow");
                $("#myModal").modal("show");
                $scope.SocietyList = data;
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data1, status) {
            });
        $(".blockUI").fadeOut("slow");
    }
    $scope.ClosePop = function () {
        $("#myModal").modal("hide");
        $scope.SocietyList = [];

    }
    $scope.Delete = function (id) {
        $http.get(SiteUrl + "/Auditor/GetOrderListReport/?OrderNo=" + id + "&Flag=" + "2").
            success(function (data, status) {
                $scope.SocietyList = data;
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data1, status) {
            });

    }


    $scope.Orderno = '';
    $scope.DeleteResion = function (id) {
        $scope.Orderno = id;
        $("#myModalforDelete").modal("show");

    };

    $scope.CloseForDelete = function () {
        $("#myModalforDelete").modal("hide");
    };

    $scope.GoForDelete = function () {
        $http.post(SiteUrl + "/Auditor/GoForDeleteresion", { "DeleteResions": $scope.DeleteResions, "OrderNo": $scope.Orderno }).
            success(function (data, status) {
                $scope.Result = data;
                $(".se-pre-con").fadeOut("slow");
                if ($scope.Result == 1) {
                    alert("delete sucessfully!");
                    $("#myModalforDelete").modal("hide");
                }
            }).
            error(function (data, status) {
                $scope.PLDB = [];
                $(".se-pre-con").fadeOut("slow");
            });


    };
    $scope.OrderDataList();



    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    //start on page load
    $scope.OnPageLoad = function () {

    }



    //Get list of all users on page list
    $scope.getTranslation($scope);

    $scope.OnPageLoad();
    $scope.OrderDataList();

    //Clear user Role

});





















