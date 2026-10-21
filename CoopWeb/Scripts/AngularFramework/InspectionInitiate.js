var ArrSelectSociety = new Array();

function SocietySelected() {
    this.SocietyIDEnc = '0';
    this.DDLOfficerId = '0';
}
var MasterMod = angular.module('InspectionInitiateMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("InspectionInitiateCONT", function ($scope, $http) {


    var ArrSelectSociety = new Array();
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    // on page load
    $scope.OnPageLoad = function () {
        //var url = window.location.search;
        //$scope.OrderId = (url.split('Id')[1]);
        //$scope.OrderId = $scope.OrderId.substring(1, $scope.OrderId.length);
        $(".se-pre-con").fadeIn("slow");
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
            $(".se-pre-con").fadeOut("slow");
        //$scope.GetSocietyList();
        $scope.GetSocietyListByInspectionOffice();
        $scope.GetFinancialYearList();
    };

   

    $scope.GetSocietyList = function () {
        $http.get(SiteUrl + "/Inspection/GetSocietyListPrimary/?FYID=" + $scope.Inspection.Year).
            success(function (data, status) {
                $scope.SocietyListdataview = data;
            }).
            error(function (data, status) {
                $scope.SocietyListdataview = [];
                alert('Error while fetching society list data.');
            });
    };

    //GET Financial Year
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data.filter(x => x.FIN_YEAR_START>2000);
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };

    $scope.GetSocietyListByInspectionOffice = function () {
        $http.get(SiteUrl + "/Inspection/InspectionOfficerForSociety").
            success(function (data, status) {
                $scope.SocietyListInspectionOfficer = data;
                
            }).
            error(function (data, status) {
                $scope.SocietyListInspectionOfficer = [];
                alert('Error while fetching society list data.');
            });
    };

    $scope.Chkdsociety = function (SocietyIDEnc, Evt) {
        var _SelectedSocietyForElection = new SocietySelected();
        _SelectedSocietyForElection.SocietyIDEnc = SocietyIDEnc;
        _SelectedSocietyForElection.DDLOfficerId = "DDLOfficer" + Evt;
        ArrSelectSociety.push(_SelectedSocietyForElection);
        // alert(JSON.stringify(ArrSelectSociety));
    }

    $scope.allChkdsociety = function () {
        ArrSelectSociety = [];
        angular.forEach($scope.SocietyListdataview, function (value, index) {
            var _SelectedSocietyForElection = new SocietySelected();
            _SelectedSocietyForElection.SocietyIDEnc = value.SocietyIDEnc;
            _SelectedSocietyForElection.DDLOfficerId = "DDLOfficer" + value.SocietyID;
            if ($('#select-all').prop('checked') == true) {
                ArrSelectSociety.push(_SelectedSocietyForElection);
            }
            else {
                ArrSelectSociety = [];
            }
        })
        //alert(JSON.stringify(ArrSelectSociety));
    }

    $scope.SaveInspectionData = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : '  निम्नलिखित   मान्य   करें  :';
        var ShowAlert = 0;
        var InspectionOffIdSocietyIdData = [];
        if ($scope.Inspection.Year == undefined || $scope.Inspection.Year == '' || $scope.Inspection.Year == '0') {
            message += $scope._language == 'en-us' ? '\n  Please Select Financial Year.' : '\n  कृपया वित्तीय वर्ष दर्ज करें|';
        }
        angular.forEach(ArrSelectSociety, function (value, index) {
            var DDLOffVal = $("#" + value.DDLOfficerId).val();
            InspectionOffIdSocietyIdData.push(
                {
                    "SocietyIdEnc": value.SocietyIDEnc,
                    "OfficerId": DDLOffVal,
                    "FYearId": $scope.Inspection.Year
                });
            if (DDLOffVal == null || DDLOffVal == "0" || DDLOffVal == 'undefined') {
                ShowAlert = 1;
            }
        });

        if (ShowAlert == 1) {
            message += $scope._language == 'en-us' ? '\n  Please select inspection officer of all selected society.' : '\n  कृपया सभी चयनित सोसायटी के निरीक्षण अधिकारी का चयन करें|';
        }
        else if (InspectionOffIdSocietyIdData.length == 0) {
            message += $scope._language == 'en-us' ? '\n  Please select at least one Society.' : '\n  कृपया कम से कम एक सोसायटी का चयन करें|';
        }

        if (message.length > 23) {
            alert(message);
        }
        else {
            var formData = new FormData();
            formData.append("DataArray", JSON.stringify(InspectionOffIdSocietyIdData));
            $http.post(SiteUrl + "/Inspection/SaveInspectionOfficerSocietyData", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    alert("Inspection officers for Society assigned successfully");
                    location.reload();
                }).
                error(function (data, status) {
                    alert("Error While assigning Inspection officers");
                });
        }
    }
    $scope.OnPageLoad();
});