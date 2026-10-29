
var MasterMod = angular.module(
    'ApplicationAuditorCurrentYearFormModule',
    ['angularUtils.directives.dirPagination']
);

 
 
 
MasterMod.controller(
    "CooperativeAuditorCurrentYearFormCont",
    function ($scope, $http) {

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
            alert("hello");
            $(".se-pre-con").fadeIn("slow");

            var translationFile =
                $scope._language == 'en-us'
                    ? "/Translations/Translation_EN.json"
                    : "/Translations/Translation_HI.json";

            $http.get(SiteUrl + translationFile)
                .then(function (response) {
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

            // Call MPR data loader after the page is initialized.
            $scope.LoadAuditMprForm();
        };

        // LOAD MPR DATA
        $scope.LoadAuditMprForm = function () {
            alert("hello");
            $(".se-pre-con").fadeIn("slow");

            // Replace with your actual MVC/API GET action.
            $http.get(SiteUrl + "/Auditor/GetCurrentYearMPR")
                .then(function (response) {

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
           
 

        // SAVE MPR DATA

        $scope.SaveAuditMprForm = function () {

           

            var payload = {
                FinancialYear: $scope.auditMprHeader.FinancialYear,
                Month: parseInt($scope.auditMprHeader.Month, 10),
                DistrictId: $scope.DistrictId,

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
