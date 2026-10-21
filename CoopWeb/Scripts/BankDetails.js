var MasterMod = angular.module('BankDetailsMasterMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BankDetailsMasterCONT", function ($compile,$scope, $http) {
    $scope.activateView = function (html) {
        $compile(html.contents())($scope);

        if (!$scope.$$phase)
            $scope.$apply();
    };
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.MasterModel = {
        master_id: 0,
        name_en: "",
        name_hi: "",
        is_active: true,
        scheme_id: "0",
        Farming_NonFarming: "0",
        sector_id: "",
        effective_date: "",
        normal_interest_rate: "",
        penal_interest_rate: "",
        purpose_id: '0',

        Farming_NonFarming: "",
        RepaymentPeriod: "",
        DueInstallment: "",
        GracePeriod: ""
    }

    $scope.ClearSearch = function () {
        $scope.q = "";
    }
    $scope.AddNewScheme = function () {
        $("#addModal").modal("show");
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
        $scope.GridBankDetails = true;
        $scope.AddBankDetails = false;
    };

    //$scope.AddNew = function () {
    //    //$(".se-pre-con").fadeIn("slow");
    //    $("#ModelList").hide();
    //    $("#addModal").show();
    //    $("#addModal").removeClass("modal-backdrop");
    //   // $(".se-pre-con").fadeOut("slow");
    //};
    
    $scope.pageChangeHandler = function (num) {

    };
    $scope.OnPageLoad();
});






