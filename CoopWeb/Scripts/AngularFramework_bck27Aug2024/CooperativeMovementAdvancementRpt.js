
var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("AppEntryCooperativeMovementRpt", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("CooperativeMovementRptcnt", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.FYear = '';
    
    function FileDataWithRow() {
        this.UserId = '';
    }
   

    //GET Financial Year
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };

  
    $scope.GetYearTargetData = function () {
        $http.post(SiteUrl + "/EntryForms/GetYearTargetData", $scope.BhandarForm).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.BhandarForm.ControlledBusinessTarget = data[0].ControlledBusinessTarget;
                    $scope.BhandarForm.UnControlledBusinessTarget = data[0].UnControlledBusinessTarget;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });
    }

    $scope.GridData = function () {
        $http.post(SiteUrl + "/MIS/CooperativeMovementRpt/?FYear=" + $scope.FYear).
             success(function (data, status) {
                // debugger;
                 if (data.length > 0) {
                     $scope.CopData = data;

                 }
                 else
                     $scope.CopData = [];
               
            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });

    }

  
   
    // on page load
    $scope.OnPageLoad = function () {
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
      
         $scope.GetFinancialYearList();
      
      
    };
    // Language Selector at page load


    $scope.OnPageLoad();
    $scope.GridData();
    $scope.DivtringGrid = true;
});

