var MasterMod = angular.module('auditorAppropriationMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("auditorAppropriationCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false; 

    // MPR Auditor Appropriation Form
    $scope.AppropriationList = [];
    $scope.DrList = [];
    $scope.CrList = [];

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
        //$(".se-pre-con").fadeIn("slow");
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
        $scope.LoadAppropriationForm();
    };

    $scope.LoadAppropriationForm = function () {
         
        var date = new Date();

        var year = date.getFullYear();
        var month = date.getMonth();
         
        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        // Previous Financial Year
        var previousFYStartYear = currentFYStartYear - 1;
        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);
        var kvssId = 128090; 
        var finYear = previousFinancialYear; 

        $http.get(SiteUrl + "Auditor/GetAppropriationDetails", {
            params: { KVSSId: kvssId, financialYear: finYear }
        }).success(function (response) {

            //var data = response || {};
            $scope.AppropriationList = response; 
            console.log('AppropriationList', $scope.AppropriationList); 
            $scope.BindAppropriationDetails(response);

            $(".se-pre-con").fadeOut("slow");
        }, function (error) {

            console.error("GetAppropriationDetails Error:", error);
            alert("Unable to load Appropriation details.");
            $(".se-pre-con").fadeOut("slow");
        });

    }

    $scope.BindAppropriationDetails = function (data) {

        $scope.AppropriationList = data || [];

        // Debit Records
        $scope.DrList = $scope.AppropriationList.filter(function (x) {
            return x.ParticularType == 'Dr';
        });

        // Credit Records
        $scope.CrList = $scope.AppropriationList.filter(function (x) {
            return x.ParticularType == 'Cr';
        });
    };

    // Total Amount
    $scope.getAppropriationTotal = function (list) {

        var total = 0;

        angular.forEach(list || [], function (item) {
            total += parseFloat(item.CurrentYearAmount) || 0;
        });

        return total;
    };

    $scope.SaveAppropriationDetails = function () {

        var saveList = [];

        // Add Dr. Records
        angular.forEach($scope.DrList, function (item) {

            saveList.push({
                ParticularId: item.ParticularId,
                CurrentYearAmount: parseFloat(item.CurrentYearAmount) || 0
            });

        });

        // Add Cr. Records
        angular.forEach($scope.CrList, function (item) {

            saveList.push({
                ParticularId: item.ParticularId,
                CurrentYearAmount: parseFloat(item.CurrentYearAmount) || 0
            });

        });

        var kvssId = 128090;
        var finYear = '2025-26';

        var request = {
            FinancialYear: finYear,
            KVSSId: kvssId, 
            AppropriationList: saveList
        };

        console.log("Save Request:", request);

        // Call Save API here

        if (saveList.length === 0) {
            alert("No records found.");
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Auditor/SaveAuditorAppropriation", request)
            .success(function (data) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    alert("Record Saved Successfully.");
                    location.reload();
                }
                else {
                    alert("Unable to save.");
                }
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while saving.");
            });
    };

    $scope.BackTo = function () {
        window.location.href = '/Auditor/AuditorProfitAndLossAc';
    };

    $scope.NextStep = function () {
        window.location.href = '/Auditor/AuditorBalanceSheet';
    };
     
    $scope.OnPageLoad(); 

});






