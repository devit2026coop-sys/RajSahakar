var MasterMod = angular.module('auditorEquityLiabilitiesMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("auditorEquityLiabilitiesCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.AuditorMasterData = [];
    $scope.ShareCapitalOtherRows = [];
    $scope.ReservesFundsOtherRows = [];
    $scope.grantsSubsidiesOtherRows = [];
    $scope.memberDepositOtherRows = [];
    $scope.otherDepositRows = [];
    $scope.borrowingsRows = [];
    $scope.amt = { memberDepsoitAmt: "0" }
    $scope.TabList = [];
    $scope.HeaderList = []; 

    //$scope.KVSSSocietyID = '128090';
    $scope.SelectedTabId = 1;
    $scope.SocietyList = [];
    $scope.FinancialYearList = [];

    // on page load
    $scope.OnPageLoad = function () {
       // alert("hello");
        $scope._language = 'en-us';
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
           
            $scope.ClearAuditorForm = function () {
                $scope.AuditorMaster = null;
                $(".se-pre-con").fadeOut("slow");

            };

            

        }


       // $scope.KVSSSocietyID = 0;



        //$scope.GetAuditAccountHeaderMaster(1);
        //$scope.GetSocietyBySSO();
        //$scope.GetSelectedSocietyName();
        //$scope.SetFinancialYears();

       
        $scope.SetFinancialYears();
        $scope.GetSocietyBySSO();

        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });

    };



    $scope.OnSocietyChange = function () {

        if (!$scope.KVSSSocietyID || $scope.KVSSSocietyID == 0) {
            $scope.HeaderList = [];
            $scope.TabList = [];
            return;
        }

        // Call GetAuditAccountHeaderMaster on dropdown change
        $scope.GetAuditAccountHeaderMaster($scope.SelectedTabId || 1 );
    };
 
    // Load tabs and parent/child headings from database
    $scope.GetAuditAccountHeaderMaster = function (tabId) {

        $http.get(SiteUrl + "/Auditor/GetAuditAccountHeaderMaster", {
            params: {
                tabId: tabId,
                kvssId: $scope.KVSSSocietyID,
                financialYear: $scope.FinancialYear
            }
        })
            .success(function (data) {

                $scope.TabList = data.TabList || [];
                $scope.HeaderList = data.HeaderList || [];

                // Set selected tab
                if ( $scope.TabList.length > 0) {
                    $scope.SelectedTabId = tabId;
                }

            })
            .error(function (error) {
                alert("Error while loading account headings.");
            });
    };

    // Get parent headings for selected tab
    $scope.GetParentHeadersByTab = function (tabId) {

        return $scope.HeaderList.filter(function (item) {

            return item.TabId == tabId &&
                (item.ParentHeaderId == null ||
                    item.ParentHeaderId == undefined);
        });
    };


    // Get child headings for selected parent
    $scope.GetChildHeaders = function (parentHeaderId) {

        return $scope.HeaderList.filter(function (item) {

            return item.ParentHeaderId == parentHeaderId;
        });
    };




    $scope.GetAllDescendants = function (parentHeaderId) {

        var result = [];

        function GetChildren(parentId, level) {

            var children = $scope.GetChildHeaders(parentId) || [];

            angular.forEach(children, function (child) {

                child.Level = level;

                result.push(child);

                GetChildren(child.HeaderId, level + 1);
            });
        }

        GetChildren(parentHeaderId, 1);

        return result;
    };


    // Change selected tab
    $scope.SelectAccountTab = function (tabId) {
        $scope.SelectedTabId = tabId;
        $scope.GetAuditAccountHeaderMaster(tabId);
    };


    $scope.AdditionalRows = {};

    $scope.AddMainHeaderRow = function (parent) {

        if (!parent || !parent.HeaderId) {
            alert("Parent heading not found.");
            return;
        }

        if (!$scope.AdditionalRows) {
            $scope.AdditionalRows = {};
        }

        if (!$scope.AdditionalRows[parent.HeaderId]) {
            $scope.AdditionalRows[parent.HeaderId] = [];
        }

        $scope.AdditionalRows[parent.HeaderId].push({
            HeaderId: 0,
            ParentHeaderId: parent.HeaderId,
            HeaderName: '',
            OpeningBalance: 0,
            KVSSSocietyId: parent.KVSSSocietyId || 0,
            FinalSubmit: 0,
            IsDeleted: 0
        });
    };

    $scope.GetAdditionalRowsByHeader = function (headerId) {

        return $scope.AdditionalRows[headerId] || [];

    };


    $scope.RemoveAdditionalRowExist = function (parentHeaderId, child) {

        if (!child || !child.HeaderId) {
            alert("Invalid heading.");
            return;
        }

        if (child.KVSSSocietyId <= 0 || child.FinalSubmit != 0) {
            alert("This heading cannot be deleted.");
            return;
        }

        if (!confirm("Are you sure you want to delete this heading?")) {
            return;
        }

        $http.post('/Auditor/DeleteAuditAccountCustomHeader', {
            HeaderId: child.HeaderId,
            KVSSSocietyId: child.KVSSSocietyId
        }).then(function (response) {

            if (response.data.Status == 1) {

                // Mark deleted in AngularJS object
                child.IsDeleted = 1;

                // Remove from displayed database child list
                var childList = $scope.GetChildHeaders(parentHeaderId);

                var index = childList.findIndex(function (x) {
                    return x.HeaderId == child.HeaderId;
                });

                if (index !== -1) {
                    childList.splice(index, 1);
                }

                alert(response.data.Message);
                $scope.GetAuditAccountHeaderMaster($scope.SelectedTabId);
            }
            else {
                alert(response.data.Message);
            }

        }, function (error) {
            alert("Unable to delete heading.");
        });
    };




    $scope.RemoveAdditionalRow = function (headerId, index) {

        if ($scope.AdditionalRows[headerId]) {

            $scope.AdditionalRows[headerId].splice(index, 1);

        }

    };



    // Prepare rows for saving
    $scope.GetOpeningBalanceSaveDetails = function () {

        var details = [];
        var parents = $scope.GetParentHeadersByTab($scope.SelectedTabId) || [];

        angular.forEach(parents, function (parent) {

            var children = $scope.GetChildHeaders(parent.HeaderId) || [];

            angular.forEach(children, function (child) {

                details.push({
                    HeaderId: child.HeaderId,
                    OpeningBalance: parseFloat(child.OpeningBalance) || 0
                });

            });

        });

        return details;
    };


    // Common save function
    $scope.SaveOpeningBalanceRows = function (details, moveNext) {

        if (!details || details.length === 0) {
            alert("No entries found to save.");
            return;
        }

        var request = {
            KVSSId: $scope.KVSSSocietyID,
            TabId: $scope.SelectedTabId,
            FinancialYear: $scope.FinancialYear,
            Details: details
        };

        $(".se-pre-con").fadeIn("slow");

        $http.post(
            SiteUrl + "Auditor/SaveAuditAccountOpeningBalance",
            request
        ).success(function (response) {

            $(".se-pre-con").fadeOut("slow");

            if (response.Status === 1) {

                alert(response.Message);

                if (moveNext) {
                    // $scope.NextStep();

                    // Find the next tab
                    var currentIndex = $scope.TabList.findIndex(function (tab) {
                        return tab.TabId == $scope.SelectedTabId;
                    });

                    if (currentIndex !== -1 &&
                        currentIndex < $scope.TabList.length - 1) {

                        var nextTab = $scope.TabList[currentIndex + 1];

                        $scope.SelectAccountTab(nextTab.TabId);
                    }
                    else {
                        window.location.href = "/KVSSAuditAccount/KVSSAuditAccount";
                    }
                }

            }
            else {
                alert(response.Message);
            }

        }).error(function () {

            $(".se-pre-con").fadeOut("slow");
            alert("Error while saving opening balance.");

        });
    };


    // Save & Next button
    $scope.SaveAndNext = function () {


        if (!$scope.KVSSSocietyID || $scope.KVSSSocietyID == 0) {
            alert("Please select society.");
            return;
        }
        var details = $scope.GetOpeningBalanceSaveDetails();

        $scope.SaveOpeningBalanceRows(details, true);

    };




    $scope.GetParentHeaderTotal = function (parentHeaderId) {

        var total = 0;

        // 1. Include custom entries directly under parent
        var parentRows = $scope.GetAdditionalRowsByHeader(parentHeaderId) || [];

        angular.forEach(parentRows, function (row) {
            total += parseFloat(row.OpeningBalance) || 0;
        });

        // 2. Get all children, grandchildren and further-level headings
        var descendants = $scope.GetAllDescendants(parentHeaderId) || [];

        angular.forEach(descendants, function (node) {

            // Include this heading's opening balance
            total += parseFloat(node.OpeningBalance) || 0;

            // 3. Include custom entries under this heading
            var customRows = $scope.GetAdditionalRowsByHeader(node.HeaderId) || [];

            angular.forEach(customRows, function (row) {
                total += parseFloat(row.OpeningBalance) || 0;
            });

        });

        return total;
    };


    $scope.UpdateParentTotal = function (parentHeaderId) {
        var total = 0;

        var children = $scope.GetChildHeaders(parentHeaderId) || [];

        angular.forEach(children, function (child) {
            total += parseFloat(child.OpeningBalance) || 0;
        });

        var additionalRows = $scope.GetAdditionalRowsByHeader(parentHeaderId) || [];

        angular.forEach(additionalRows, function (row) {
            total += parseFloat(row.OpeningBalance) || 0;
        });

        var parent = $scope.HeaderList.find(function (item) {
            return item.HeaderId == parentHeaderId;
        });

        if (parent) {
            parent.TotalOpeningBalance = total;
        }
    };


    $scope.SaveAdditionalRow = function (parentHeaderId, row) {

        if (!row.HeaderName || row.HeaderName.trim() === '') {
            alert("Please enter particulars.");
            return;
        }

        var request = {
            HeaderId: row.HeaderId || 0,
            ParentHeaderId: parentHeaderId,
            TabId: $scope.SelectedTabId,
            SocietyTypeId: $scope.SocietyTypeId,
            HeaderNameEnglish: row.HeaderName,
            HeaderNameHindi: row.HeaderName,
            KVSSId: $scope.KVSSSocietyID,
            FinancialYear: $scope.FinancialYear,
            OpeningBalance: parseFloat(row.OpeningBalance) || 0
        };

        console.log("ParentHeaderId:", request.ParentHeaderId);
        console.log("Save Request:", request);

        $(".se-pre-con").fadeIn("slow");

        $http.post(
            SiteUrl + "/Auditor/SaveAuditAccountCustomHeader",
            request
        ).success(function (response) {

            $(".se-pre-con").fadeOut("slow");

            if (response.Status === 1) {

                alert(response.Message);

                // Remove temporary input row
                if ($scope.AdditionalRows[parentHeaderId]) {

                    var index = $scope.AdditionalRows[parentHeaderId].indexOf(row);

                    if (index !== -1) {
                        $scope.AdditionalRows[parentHeaderId].splice(index, 1);
                    }
                }

                // Refresh selected tab from database
                $scope.GetAuditAccountHeaderMaster($scope.SelectedTabId);

            }
            else {
                alert(response.Message);
            }

        }).error(function () {

            $(".se-pre-con").fadeOut("slow");
            alert("Error while saving subheading.");

        });
    };



    $scope.GetGrandTotal = function () {

        var grandTotal = 0;

        var parents = $scope.GetParentHeadersByTab($scope.SelectedTabId) || [];

        angular.forEach(parents, function (parent) {
            grandTotal += $scope.GetParentHeaderTotal(parent.HeaderId);
        });

        return grandTotal;
    };

    $scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {

        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Society/KVSSGSSbySSOD", {})
            .then(function (response) {

                $scope.SocietyList = response.data || [];

                // Convert SocietyID to number
                angular.forEach($scope.SocietyList, function (item) {
                    item.SocietyID = parseInt(item.SocietyID, 10);
                });

                // Read the session value already assigned through Razor ng-init
                var sessionSocietyId = parseInt($scope.KVSSSocietyID, 10);

                if (!isNaN(sessionSocietyId) && sessionSocietyId > 0) {

                    // Check whether the session society exists in the dropdown list
                    var societyExists = $scope.SocietyList.some(function (item) {
                        return item.SocietyID === sessionSocietyId;
                    });

                    if (societyExists) {
                        $scope.KVSSSocietyID = sessionSocietyId;
                    }
                    else {
                        console.log("Session Society ID not found in SocietyList:", sessionSocietyId);
                    }
                }
                else {
                    $scope.KVSSSocietyID = null;
                }

                console.log("Session KVSS ID:", $scope.KVSSSocietyID);
                console.log("Society List:", $scope.SocietyList);

                // Load headers after society list and selected ID are ready
                if ($scope.KVSSSocietyID) {
                    $scope.GetAuditAccountHeaderMaster(1);
                }

                $(".se-pre-con").fadeOut("slow");

            })
            .catch(function (error) {

                console.log("GetSocietyBySSO Error:", error);

                $scope.SocietyList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };



    $scope.GetSelectedSocietyName = function () {
        var selectedSociety = $scope.SocietyList.find(function (item) {
            return item.SocietyID == $scope.KVSSSocietyID;
        });

        return selectedSociety ? selectedSociety.SocietyName : '';
    };


    $scope.SetFinancialYears = function () {

        var today = new Date();

        var currentFYStartYear = today.getMonth() >= 3
            ? today.getFullYear()
            : today.getFullYear() - 1;

        // Previous financial year
        var previousFYStartYear = currentFYStartYear - 1;

        $scope.FinancialYear = previousFYStartYear + "-" +
            (previousFYStartYear + 1).toString().substring(2);

        // Generate last 10 financial years
        $scope.FinancialYearList = [];

        for (var i = 0; i < 10; i++) {

            var startYear = previousFYStartYear - i;

            var financialYear = startYear + "-" +
                (startYear + 1).toString().substring(2);

            $scope.FinancialYearList.push(financialYear);
        }
    };



    $scope.OnFinancialYearChange = function () {

        if (!$scope.FinancialYear) {
            return;
        }

        // Reload account headers for selected Financial Year
        $scope.GetAuditAccountHeaderMaster(
            $scope.SelectedTabId || 1
        );
    };



    $scope.GoToPreviousTab = function () {

        $scope.SelectedTabId = 1;

        $scope.GetAuditAccountHeaderMaster(1);
    };

    // end new js

    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };
    $scope.GetAllAuditorList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Auditor/AuditorList").
            success(function (data, status) {
                $scope.AuditData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.update = function (item) {
        $scope.AuditorMaster.DepartmentId = item.departmentId;
        $scope.AuditorMaster.UnitNameEn = item.UnitNameEn;
        $scope.AuditorMaster.UnitNameHi = item.UnitNameHi;
        $scope.AuditorMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.AuditorMaster.UnitId = item.UnitId;
    };

    $scope.NextStep = function () {
        window.location.href = '/Auditor/AuditorSaleLiabilities';

    };


    //Added new dyanamic rows as per new
    $scope.shareCapitalAddRow = function () {
        $scope.ShareCapitalOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.shareCapitalRemoveRow = function (index) {
        $scope.ShareCapitalOtherRows.splice(index, 1);
    };

    $scope.reservesFundsAddRow = function () {
        $scope.ReservesFundsOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.reservesFundsRemoveRow = function (index) {
        $scope.ReservesFundsOtherRows.splice(index, 1);
    };

    $scope.grantsSubsidiesAddRow = function () {
        $scope.grantsSubsidiesOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.grantsSubsidiesRemoveRow = function (index) {
        $scope.grantsSubsidiesOtherRows.splice(index, 1);
    };

    $scope.memberDepositAddRow = function () {
        $scope.memberDepositOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.memberDepositRemoveRow = function (index) {
        $scope.memberDepositOtherRows.splice(index, 1);
        $scope.calculateMemberDepositAmount();
    };

    $scope.calculateMemberDepositAmount = function () {
        let subTotal = 0;
        $scope.memberDepositOtherRows.forEach(function (row) {
            subTotal += parseFloat(row.TotalAmt ?? 0);
        });
        $scope.amt.memberDepsoitAmt = subTotal.toFixed(2);
    };
    $scope.otherDepositAddRow = function () {
        $scope.otherDepositRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };
    $scope.otherDepositRemoveRow = function (index) {
        $scope.otherDepositRows.splice(index, 1);
    };



    $scope.borrowingsAddRow = function () {
        $scope.borrowingsRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };
    $scope.borrowingsRemoveRow = function (index) {
        $scope.borrowingsRows.splice(index, 1);
    };

    $scope.OnPageLoad();
    $scope.GetAllAuditorList();

});






