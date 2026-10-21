
var MasterMod = angular.module('loaneePRMod', ['angularUtils.directives.dirPagination']);
MasterMod.directive('onlySelectDate', function () {
    return {
        restrict: 'A',
        link: function (scope, element) {
            element.on('keydown paste', function (event) {
                event.preventDefault();
            });
        }
    };
});

MasterMod.controller("loaneePRCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.LoaneePRRecDetails = [];
    $scope.colspan = 8;
    $scope.PLDB_TransationDate = '';

    $scope.isLBDisabled = true;
    $scope.ReportType = 1;
    $scope.Islinkcalled = false;

    $scope.search = {
        PLDB_TransationDate: new Date()
    };

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $scope.PLDBListData();
        //$scope.GetPldbLoaneeReceiptDetail()
        $scope.SearchReceiptDetails();
        //$scope.ReportTypeChange();
    };
    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); $scope.search.PLDB_ID = "0"; $scope.search.PLDB_Branch_ID = "0"; $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();
                }

                //$scope.GetPldbOtsPendencySummaryDetails();
            }).error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

    };

    $scope.PLDBListData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                $(".se-pre-con").fadeOut("slow");

                if ($("#hdnPldbId").val() != "0") {
                    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }


            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.search.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }

                $(".se-pre-con").fadeOut("slow");
                $scope.ReportTypeChange();

            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.SearchReceiptDetails = function () {
        $scope.PLDB_TransationDate = $scope.DateFormated($scope.search.PLDB_TransationDate);
        $scope.GetPldbLoaneeReceiptDetail();
    };
    $scope.GetPldbLoaneeReceiptDetail = function () {
        $scope.actionType = "";

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }

        if ($scope.ReportType == '1') { $scope.actionType = 'LoaneeReceiptByCurrentDate'; }
        else if ($scope.ReportType == '2') {
            if ($scope.search.PLDB_ID == 0 && $scope.search.PLDB_Branch_ID == 0) { $scope.actionType = "LoaneeReceiptByDetails" }
            else { $scope.PLDB_TransationDate = ''; $scope.actionType = 'LoaneeReceiptByPldb'; }
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MIS/GetLoaneeReceiptDetails/?FilterDate=" + $scope.PLDB_TransationDate + "&actionType=" + $scope.actionType + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&pldbId=" + ($scope.search.PLDB_ID ?? 0) + "&pldbBranchId=" + ($scope.search.PLDB_Branch_ID ?? 0)).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.LoaneeReceiptDetail != null) {
                    $scope.LoaneePRRecDetails = JSON.parse(data.Result.Data.LoaneeReceiptDetail);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ReportTypeChange = function () {
        $scope.LoaneePRRecDetails = [];
        if ($scope.ReportType == "1") {
            $scope.search.PLDB_ID = "0";
            $scope.search.PLDB_Branch_ID = "0";
            $scope.isLBDisabled = true;
            $scope.Islinkcalled = false;
            $scope.SearchReceiptDetails();

        } else if ($scope.ReportType == "2") {
            $scope.search.PLDB_TransationDate = new Date();
            $scope.isLBDisabled = false;
            $scope.Islinkcalled = true;
            $scope.SearchReceiptDetails();
        }
    }

    $scope.BackToPage = function () {
        $scope.search = {
            PLDB_TransationDate: new Date()
        };
        $scope.SearchReceiptDetails();
    }
    $scope.DateFormated = function (input) {
        if (!input) return '';
        var date = new Date(input);
        var yyyy = date.getFullYear();
        var mm = ('0' + (date.getMonth() + 1)).slice(-2);
        var dd = ('0' + date.getDate()).slice(-2);
        return `${yyyy}-${mm}-${dd}`;
    }

    $scope.OnPageLoad();

});
MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});






