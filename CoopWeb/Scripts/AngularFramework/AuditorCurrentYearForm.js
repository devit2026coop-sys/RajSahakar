
var MasterMod = angular.module('ApplicationAuditorCurrentYearFormModule', ['angularUtils.directives.dirPagination']);

MasterMod.controller("CooperativeAuditorCurrentYearFormCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;

    // MPR Current Year Form
    $scope.AuditorMasterData = [];
    $scope.auditMprList = [];
    $scope.auditMprHeader = {
        FinancialYear: '',
        Month: ''
    };

    $scope.DistrictId = null;
    $scope.DistrictName = '';

    $scope.FinancialYearList = [];
    $scope.MonthList = [
        { id: 1, name: 'April' },
        { id: 2, name: 'May' },
        { id: 3, name: 'June' },
        { id: 4, name: 'July' },
        { id: 5, name: 'August' },
        { id: 6, name: 'September' },
        { id: 7, name: 'October' },
        { id: 8, name: 'November' },
        { id: 9, name: 'December' },
        { id: 10, name: 'January' },
        { id: 11, name: 'February' },
        { id: 12, name: 'March' }
    ];

    // ON PAGE LOAD
    $scope.OnPageLoad = function () {
        //alert("hello");
        $(".se-pre-con").fadeIn("slow");

        var translationFile =
            $scope._language == 'en-us' ? "/Translations/Translation_EN.json" : "/Translations/Translation_HI.json";

        $http.get(SiteUrl + translationFile).then(function (response) {
            $scope.translation = response.data;
        }, function () {
            alert("Error while translation");
        });

        // Load department data if required by the existing page.
        $http.get(
            SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1"
        ).then(function (response) {
            $scope.Department = response.data || [];
        }, function () {
            $scope.Department = [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
        $scope.SetCurrentFY();
        $scope.GetUserDetailsBySSO();
        $scope.SetPreviousMonth();
        // Call MPR data loader after the page is initialized.
        $scope.LoadAuditMprForm();
    };

    $scope.SetCurrentFY = function () {
        $http.get(SiteUrl + "/Society/GetCurrentFinancialYear")
            .success(function (data) {
                $scope.FY = data[0].CurrentFY;
                console.log('$scope.FY', $scope.FY);
                $scope.GetFinancialYear();
            })
            .error(function () {
                alert("Unable to load Current Financial Year.");
            });
    };

    $scope.GetFinancialYear = function () {
        
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data, status) {
                $scope.FinancialYearList = data;
                console.log('$scope.FinancialYearList', $scope.FinancialYearList);

                // Get Financial Year Name using FY ID
                var selectedFY = $scope.FinancialYearList.filter(function (x) {
                    return x.FIN_YEAR_MASTR_ID == $scope.FY;
                });

                if (selectedFY.length > 0) {
                    $scope.FinancialYearName = selectedFY[0].FIN_YEAR_NAME;
                    console.log('$scope.FinancialYearName', $scope.FinancialYearName);
                }

                $(".se-pre-con").fadeOut("slow");

            })
            .error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetUserDetailsBySSO = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Auditor/GetUserDetailsAuditBySSO")
            .success(function (data) {
                $scope.UserDetails = data[0];
                console.log('$scope.UserDetails', $scope.UserDetails);
                $scope.OfficeId = data[0].OfficeId;
                $scope.DISTRICT_ENG = data[0].DISTRICT_ENG;
                $scope.ShowTable = true;
                //scope.GetAuditorCurrentYearFormNew();

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SetPreviousMonth = function () {

        var months = [
            { Id: 1, Name: "January" },
            { Id: 2, Name: "February" },
            { Id: 3, Name: "March" },
            { Id: 4, Name: "April" },
            { Id: 5, Name: "May" },
            { Id: 6, Name: "June" },
            { Id: 7, Name: "July" },
            { Id: 8, Name: "August" },
            { Id: 9, Name: "September" },
            { Id: 10, Name: "October" },
            { Id: 11, Name: "November" },
            { Id: 12, Name: "December" }
        ];

        var currentMonth = new Date().getMonth(); // 0-11

        if (currentMonth == 0) {
            $scope.PreviousMonthId = 12;
            $scope.PreviousMonth = "December";
        }
        else {
            $scope.PreviousMonthId = months[currentMonth - 1].Id;
            $scope.PreviousMonth = months[currentMonth - 1].Name;
        }
    };

    // LOAD MPR DATA
    $scope.LoadAuditMprForm = function () {
        //alert("hello"); 
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.SelectedDate) {
            alert("Please select date.");
            $scope.auditMprList = []; 
            return;
        }

        var date = new Date($scope.SelectedDate);

        var year = date.getFullYear();
        var month = date.getMonth();

        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        // Previous Financial Year
        var nextFYStartYear = currentFYStartYear + 1;

        var nextFinancialYear = currentFYStartYear + '-' + String(nextFYStartYear).slice(-2);

        console.log("Previous Financial Year:", nextFinancialYear);

        var distId = $scope.UserDetails.DISTRICT_ID;
        var finYear = nextFinancialYear;

        // Replace with your actual MVC/API GET action.
        $http.get(SiteUrl + "/Auditor/GetCurrentYearMPR", {
            params: {
                month: month,
                financialYear: finYear,
                districtId: distId
            }

        }).then(function (response) {

                // Adjust this mapping to match the actual response.
                var data = response.data || {};
                console.log('data', data);

                $scope.auditMprList =
                    angular.isArray(data) ? data : (data.Records || []);

                $scope.DistrictId = data.DistrictId || null;
                $scope.DistrictName = data.DistrictName || '';

                $scope.FinancialYearList =
                    data.FinancialYearList || [];

            }, function (error) {
                $scope.auditMprList = [];
                console.error("Error loading MPR data.", error);
            }).finally(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };



    // CALCULATE COLUMN TOTAL
    $scope.getColumnTotal = function (columnName) {
        var total = 0;

        angular.forEach($scope.auditMprList, function (item) {
            var value = parseFloat(item[columnName]);

            if (!isNaN(value)) {
                total += value;
            }
        });

        return total;
    };

    $scope.calculateProposalAllocationRemaining = function (item) {

        item.ProposalAllocationRemaining =
            (Number(item.TotalCommitteesCurrentYear) || 0) -
            (
                (Number(item.ProposalDepartment) || 0) +
                (Number(item.ProposalCA) || 0) +
                (Number(item.AllocationDepartment) || 0) +
                (Number(item.AllocationCA) || 0)
            );
    };

    // SAVE MPR DATA

    $scope.SaveAuditMprForm = function () {

        // Validate before save
        var invalidRecord = $scope.auditMprList.find(function (item) {

            var proposalDepartment = Number(item.ProposalDepartment) || 0;
            var previousAuditedDepartment = Number(item.PreviousAuditedProposalDepartment) || 0;
            var currentAuditedDepartment = Number(item.CurrentAuditedProposalDepartment) || 0;

            var proposalCA = Number(item.ProposalCA) || 0;
            var previousAuditedCA = Number(item.PreviousAuditedProposalCA) || 0;
            var currentAuditedCA = Number(item.CurrentAuditedProposalCA) || 0;

            var allocationDepartment = Number(item.AllocationDepartment) || 0;
            var previousAuditedAlloDept = Number(item.PreviousAuditedAllocationDepartment) || 0;
            var currentAuditedAlloDept = Number(item.CurrentAuditedAllocationDepartment) || 0;

            var allocationCA = Number(item.AllocationCA) || 0;
            var previousAuditedAlloCA = Number(item.PreviousAuditedAllocationCA) || 0;
            var currentAuditedAlloCA = Number(item.CurrentAuditedAllocationCA) || 0;

            return (
                proposalDepartment < (previousAuditedDepartment + currentAuditedDepartment) ||
                proposalCA < (previousAuditedCA + currentAuditedCA) ||
                allocationDepartment < (previousAuditedAlloDept + currentAuditedAlloDept) ||
                allocationCA < (previousAuditedAlloCA + currentAuditedAlloCA)
            );
        });

        if (invalidRecord) {
            alert("Proposal Department and CA,Allocation Department and CA must be greater than or equal to the sum of their Previous Audited and Current Audited values.");
            return;
        }

        var date = new Date($scope.SelectedDate);

        var year = date.getFullYear();
        var month = date.getMonth();

        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        // Previous Financial Year
        var nextFYStartYear = currentFYStartYear + 1;

        var nextFinancialYear = currentFYStartYear + '-' + String(nextFYStartYear).slice(-2);
        
        var distId = $scope.UserDetails.DISTRICT_ID;
        var finYear = nextFinancialYear;

        var payload = {
            //FinancialYear: $scope.auditMprHeader.FinancialYear,
            //Month: parseInt($scope.auditMprHeader.Month, 10),
            //DistrictId: $scope.DistrictId,
            FinancialYear: finYear,
            Month: month,
            DistrictId: distId,

            Records: $scope.auditMprList.map(function (item) {
                return {
                    CommitteeTypeId: item.CommitteeTypeId,

                    TotalCommitteesCurrentYear:
                        item.TotalCommitteesCurrentYear || 0,

                    ProposalDepartment:
                        item.ProposalDepartment || 0,

                    ProposalCA:
                        item.ProposalCA || 0,

                    AllocationDepartment:
                        item.AllocationDepartment || 0,

                    AllocationCA:
                        item.AllocationCA || 0,

                    ProposalAllocationRemaining:
                        item.ProposalAllocationRemaining || 0,

                    PreviousAuditedProposalDepartment:
                        item.PreviousAuditedProposalDepartment || 0,

                    PreviousAuditedProposalCA:
                        item.PreviousAuditedProposalCA || 0,

                    PreviousAuditedAllocationDepartment:
                        item.PreviousAuditedAllocationDepartment || 0,

                    PreviousAuditedAllocationCA:
                        item.PreviousAuditedAllocationCA || 0,

                    CurrentAuditedProposalDepartment:
                        item.CurrentAuditedProposalDepartment || 0,

                    CurrentAuditedProposalCA:
                        item.CurrentAuditedProposalCA || 0,

                    CurrentAuditedAllocationDepartment:
                        item.CurrentAuditedAllocationDepartment || 0,

                    CurrentAuditedAllocationCA:
                        item.CurrentAuditedAllocationCA || 0
                };
            })
        };

        console.log("MPR payload:", payload);

        $(".se-pre-con").fadeIn("slow");

        $http.post(
            SiteUrl + "/Auditor/SaveCurrentYearMPR",
            payload
        ).then(function (response) {

            var result = response.data;

            if (result && result.Success) {
                alert(result.Message || "MPR form saved successfully.");
            } else {
                alert(
                    (result && result.Message) ||
                    "Unable to save MPR form."
                );
            }

        }, function (error) {

            console.error("Error saving MPR form:", error);

            var message = error.data && error.data.Message
                ? error.data.Message
                : "An error occurred while saving the MPR form.";

            alert(message);

        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };


    // CLEAR FORM VALUES
    $scope.ClearAuditMprForm = function () {

        angular.forEach($scope.auditMprList, function (item) {
            item.TotalCommitteesCurrentYear = null;
            item.ProposalDepartment = null;
            item.ProposalCA = null;
            item.AllocationDepartment = null;
            item.AllocationCA = null;
            item.ProposalAllocationRemaining = null;
            item.PreviousAuditedProposalDepartment = null;
            item.PreviousAuditedProposalCA = null;
            item.PreviousAuditedAllocationDepartment = null;
            item.PreviousAuditedAllocationCA = null;
            item.CurrentAuditedProposalDepartment = null;
            item.CurrentAuditedProposalCA = null;
            item.CurrentAuditedAllocationDepartment = null;
            item.CurrentAuditedAllocationCA = null;
        });
    };
     
    // INITIALIZE
    $scope.OnPageLoad();
}
);
