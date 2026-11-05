
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
        //$scope.SetCurrentFY();
        $scope.GetUserDetailsBySSO();
        //$scope.SetPreviousMonth();
        // Call MPR data loader after the page is initialized.
        $scope.LoadAuditMprForm();
    };

    //$scope.SetCurrentFY = function () {
    //    $http.get(SiteUrl + "/Society/GetCurrentFinancialYear")
    //        .success(function (data) {
    //            $scope.FY = data[0].CurrentFY;
    //            console.log('$scope.FY', $scope.FY);
    //            $scope.GetFinancialYear();
    //        })
    //        .error(function () {
    //            alert("Unable to load Current Financial Year.");
    //        });
    //};

    //$scope.GetFinancialYear = function () {
        
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.FinancialYearList = [];

    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
    //        .success(function (data, status) {
    //            $scope.FinancialYearList = data;
    //            console.log('$scope.FinancialYearList', $scope.FinancialYearList);
                 
    //            var selectedFY = $scope.FinancialYearList.filter(function (x) {
    //                return x.FIN_YEAR_MASTR_ID == $scope.FY;
    //            });

    //            if (selectedFY.length > 0) {
    //                $scope.FinancialYearName = selectedFY[0].FIN_YEAR_NAME;
    //                console.log('$scope.FinancialYearName', $scope.FinancialYearName);
    //            }

    //            $(".se-pre-con").fadeOut("slow");

    //        })
    //        .error(function (data, status) {
    //            $scope.FinancialYearList = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};

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

    $scope.SetPreviousMonth = function (month = 0) {

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

        //var currentMonth = new Date().getMonth(); // 0-11
        var currentMonth = month;
         
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
            //alert("Please select date.");
            $scope.auditMprList = []; 
            return;
        }

        var date = new Date($scope.SelectedDate);

        var year = date.getFullYear();
        var month = date.getMonth();
        $scope.SetPreviousMonth(month);
        // Financial year April to March
        var currentFYStartYear = month >= 4 ? year : year - 1;

        // Previous Financial Year
        var previousFYStartYear = currentFYStartYear - 1;
        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        //var nextFYStartYear = currentFYStartYear + 1;
        //var nextFinancialYear = currentFYStartYear + '-' + String(nextFYStartYear).slice(-2);
        //console.log("Previous Financial Year:", previousFinancialYear);

        var distId = $scope.UserDetails.DISTRICT_ID;
        var finYear = previousFinancialYear;
        $scope.FinancialYearName = finYear; 
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

                $scope.auditMprList = angular.isArray(data) ? data : (data.Records || []);

                //$scope.DistrictId = data.DistrictId || null;
                //$scope.DistrictName = data.DistrictName || '';

                //$scope.FinancialYearList = data.FinancialYearList || [];

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

    //$scope.calculateProposalAllocationRemaining = function (item) {

    //    item.ProposalAllocationRemaining =
    //        (Number(item.TotalCommitteesCurrentYear) || 0) -
    //        (
    //            (Number(item.ProposalDepartment) || 0) +
    //            (Number(item.ProposalCA) || 0) +
    //            (Number(item.AllocationDepartment) || 0) +
    //            (Number(item.AllocationCA) || 0)
    //        );
    //};

    $scope.calculateProposalAllocationRemaining = function (item, field) {

        var total = Number(item.TotalCommitteesCurrentYear) || 0;
        var proposalDepartment = Number(item.ProposalDepartment) || 0;
        var proposalCA = Number(item.ProposalCA) || 0;
        var allocationDepartment = Number(item.AllocationDepartment) || 0;
        var allocationCA = Number(item.AllocationCA) || 0;

        // Negative values not allowed
        if (total < 0 || proposalDepartment < 0 || proposalCA < 0 || allocationDepartment < 0 || allocationCA < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.calculateProposalAllocationRemaining(item);
            return;
        }

        // Allotment cannot exceed total committees
        if ((proposalDepartment + proposalCA + allocationDepartment + allocationCA) > total) {

            alert("कुल आवंटित समितियां(प्रस्ताव विभाग + चार्टर्ड अकाउंटेंट + आवंटन विभाग + चार्टर्ड अकाउंटेंट) का योग कुल समितियों से अधिक नहीं हो सकता।");

            if (field === "ProposalDepartment") {
                item.ProposalDepartment = 0;
            }
            else if (field === "ProposalCA") {
                item.ProposalCA = 0;
            }
            else if (field === "AllocationDepartment") {
                item.AllocationDepartment = 0;
            }
            else if (field === "AllocationCA") {
                item.AllocationCA = 0;
            }
            else if (field === "TotalCommitteesCurrentYear") {
                item.TotalCommittees = proposalDepartment + proposalCA + allocationDepartment + allocationCA;
            }

            total = Number(item.TotalCommitteesCurrentYear) || 0;
            proposalDepartment = Number(item.ProposalDepartment) || 0;
            proposalCA = Number(item.ProposalCA) || 0;
            allocationDepartment = Number(item.AllocationDepartment) || 0;
            allocationCA = Number(item.AllocationCA) || 0;
        }

        // Calculate remaining allocation
        item.ProposalAllocationRemaining = total - (proposalDepartment + proposalCA + allocationDepartment + allocationCA);
    };

    $scope.ValidateProposalDepartment = function (item, field) {

        var total = Number(item.ProposalDepartment) || 0;
        var department = Number(item.PreviousAuditedProposalDepartment) || 0;
        var ca = Number(item.CurrentAuditedProposalDepartment) || 0;
         
        if (total < 0 || department < 0 || ca < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.ValidateProposalDepartment(item);
            return;
        }
         
        if ((department + ca) > total) {

            alert("गत माह के प्रस्ताव विभाग + माह के प्रस्ताव विभाग का योग कुल आवंटित समितियां के प्रस्ताव विभाग से अधिक नहीं हो सकता।");

            if (field === "PreviousAuditedProposalDepartment") {
                item.PreviousAuditedProposalDepartment = 0;
            }
            else if (field === "CurrentAuditedProposalDepartment") {
                item.CurrentAuditedProposalDepartment = 0;
            }
            else if (field === "ProposalDepartment") {
                item.ProposalDepartment = department + ca;
            }

            total = Number(item.ProposalDepartment) || 0;
            department = Number(item.PreviousAuditedProposalDepartment) || 0;
            ca = Number(item.CurrentAuditedProposalDepartment) || 0;
        } 
    };

    $scope.ValidateProposalCA = function (item, field) {

        var total = Number(item.ProposalCA) || 0;
        var department = Number(item.PreviousAuditedProposalCA) || 0;
        var ca = Number(item.CurrentAuditedProposalCA) || 0;

        if (total < 0 || department < 0 || ca < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.ValidateProposalCA(item);
            return;
        }

        if ((department + ca) > total) {

            alert("गत माह के प्रस्ताव चार्टर्ड अकाउंटेंट + माह के प्रस्ताव चार्टर्ड अकाउंटेंट का योग कुल आवंटित समितियां के प्रस्ताव चार्टर्ड अकाउंटेंट से अधिक नहीं हो सकता।");

            if (field === "PreviousAuditedProposalCA") {
                item.PreviousAuditedProposalCA = 0;
            }
            else if (field === "CurrentAuditedProposalCA") {
                item.CurrentAuditedProposalCA = 0;
            }
            else if (field === "ProposalCA") {
                item.ProposalCA = department + ca;
            }

            total = Number(item.ProposalCA) || 0;
            department = Number(item.PreviousAuditedProposalCA) || 0;
            ca = Number(item.CurrentAuditedProposalCA) || 0;
        }
    };

    $scope.ValidateAllocationDepartment = function (item, field) {

        var total = Number(item.AllocationDepartment) || 0;
        var department = Number(item.PreviousAuditedAllocationDepartment) || 0;
        var ca = Number(item.CurrentAuditedAllocationDepartment) || 0;

        if (total < 0 || department < 0 || ca < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.ValidateAllocationDepartment(item);
            return;
        }

        if ((department + ca) > total) {

            alert("गत माह के आवंटन विभाग + माह के आवंटन विभाग का योग कुल आवंटित समितियां के आवंटन विभाग से अधिक नहीं हो सकता।");

            if (field === "PreviousAuditedAllocationDepartment") {
                item.PreviousAuditedAllocationDepartment = 0;
            }
            else if (field === "CurrentAuditedAllocationDepartment") {
                item.CurrentAuditedAllocationDepartment = 0;
            }
            else if (field === "AllocationDepartment") {
                item.AllocationDepartment = department + ca;
            }

            total = Number(item.AllocationDepartment) || 0;
            department = Number(item.PreviousAuditedAllocationDepartment) || 0;
            ca = Number(item.CurrentAuditedAllocationDepartment) || 0;
        }
    };

    $scope.ValidateAllocationCA = function (item, field) {

        var total = Number(item.AllocationCA) || 0;
        var department = Number(item.PreviousAuditedAllocationCA) || 0;
        var ca = Number(item.CurrentAuditedAllocationCA) || 0;

        if (total < 0 || department < 0 || ca < 0) {
            alert("Negative value allowed nahi hai.");
            item[field] = 0;
            $scope.ValidateAllocationCA(item);
            return;
        }

        if ((department + ca) > total) {

            alert("गत माह के आवंटन चार्टर्ड अकाउंटेंट + माह के आवंटन चार्टर्ड अकाउंटेंट का योग कुल आवंटित समितियां के आवंटन चार्टर्ड अकाउंटेंट से अधिक नहीं हो सकता।");

            if (field === "PreviousAuditedAllocationCA") {
                item.PreviousAuditedAllocationCA = 0;
            }
            else if (field === "CurrentAuditedAllocationCA") {
                item.CurrentAuditedAllocationCA = 0;
            }
            else if (field === "AllocationCA") {
                item.AllocationCA = department + ca;
            }

            total = Number(item.AllocationCA) || 0;
            department = Number(item.PreviousAuditedAllocationCA) || 0;
            ca = Number(item.CurrentAuditedAllocationCA) || 0;
        }
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
        var previousFYStartYear = currentFYStartYear - 1;
        var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        //var nextFYStartYear = currentFYStartYear + 1;
        //var nextFinancialYear = currentFYStartYear + '-' + String(nextFYStartYear).slice(-2);
        
        var distId = $scope.UserDetails.DISTRICT_ID;
        var finYear = previousFinancialYear;

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

    $scope.NextStep = function () {
        window.location.href = '/Auditornew/AuditorCurrentyearFormNew';
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
