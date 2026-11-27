var AssetClassificationMod = angular.module('assetClassificationMod', []);

AssetClassificationMod.controller(
    "assetClassificationCtrl",
    function ($scope, $http) {

        // ------------------------------------------------
        // SCOPE VARIABLES
        // ------------------------------------------------

        $scope._language = 'en-us';

        $scope.TabList = [];
        $scope.HeaderList = [];
        $scope.DetailList = [];

        $scope.SelectedTabId = 1;
        $scope.SelectedViewTab = 1;

        $scope.KVSSSocietyID = $scope.KVSSSocietyID || 0;
        $scope.FinancialYear = $scope.FinancialYear || '';

        $scope.FinancialYearEnd = '';

        $scope.SummaryList = [];

        // ------------------------------------------------
        // PAGE LOAD
        // ------------------------------------------------

        $scope.OnPageLoad = function () {

            $(".se-pre-con").fadeIn("slow");

            $scope._language = 'en-us';

            var translationUrl =
                $scope._language == 'en-us'
                    ? "/Translations/Translation_EN.json"
                    : "/Translations/Translation_HI.json";

            $http.get(SiteUrl + translationUrl)
                .success(function (data) {
                    $scope.translation = data;
                })
                .error(function () {
                    alert("Error while loading translation.");
                });

            if (!$scope.KVSSSocietyID || $scope.KVSSSocietyID == 0) {
                alert("Society not selected.");
                $(".se-pre-con").fadeOut("slow");
                return;
            }

            if (!$scope.FinancialYear) {
                alert("Financial year not found.");
                $(".se-pre-con").fadeOut("slow");
                return;
            }

            var fyParts = $scope.FinancialYear.split('-');

            if (fyParts.length > 0) {
                $scope.FinancialYearEnd =
                    "31 March 20" + fyParts[1];
            }

            $scope.GetAuditAccountHeaderMaster(1);
        };


        // ------------------------------------------------
        // GET HEADERS / TABS
        // ------------------------------------------------

        $scope.GetAuditAccountHeaderMaster = function (tabId) {

            $(".se-pre-con").fadeIn("slow");

            $http.get(
                SiteUrl + "/Auditor/GetAuditAccountHeaderMasterAssestClassification",
                {
                    params: {
                        tabId: tabId,
                        kvssId: $scope.KVSSSocietyID,
                        financialYear: $scope.FinancialYear
                    }
                }
            )
                .success(function (data) {

                    $scope.TabList = data.TabList || [];
                    $scope.HeaderList = data.HeaderList || [];

                    if ($scope.TabList.length > 0) {

                        var selectedTabExists = $scope.TabList.some(function (tab) {
                            return tab.TabId == tabId;
                        });

                        $scope.SelectedTabId =
                            selectedTabExists
                                ? tabId
                                : $scope.TabList[0].TabId;
                    }

                    // Load saved classification entries
                    $scope.GetAssetClassificationDetails();

                })
                .error(function () {

                    $(".se-pre-con").fadeOut("slow");

                    alert("Error while loading account headings.");

                });
        };


        // ------------------------------------------------
        // GET SAVED DETAIL RECORDS
        // ------------------------------------------------

        $scope.GetAssetClassificationDetails = function () {

            $http.get(
                SiteUrl + "/Auditor/GetAuditAssetClassificationDetails",
                {
                    params: {
                        kvssId: $scope.KVSSSocietyID,
                        financialYear: $scope.FinancialYear,
                        tabId: $scope.SelectedTabId
                    }
                }
            )
                .success(function (data) {

                    $scope.DetailList = data || [];

                    // Attach saved details to corresponding headings
                    angular.forEach($scope.HeaderList, function (header) {

                        var saved = $scope.DetailList.find(function (item) {
                            return item.HeaderId == header.HeaderId;
                        });

                        if (saved) {

                            saved.DateOfAdvance = saved.DateOfAdvance
                                ? new Date(saved.DateOfAdvance + "T00:00:00")
                                : null;

                            header.AssetDetail = saved;

                        }
                        else {

                            header.AssetDetail = $scope.GetEmptyAssetDetail(
                                header.HeaderId
                            );

                        }

                    });

                    $scope.CalculateSummary();

                    $(".se-pre-con").fadeOut("slow");

                })
                .error(function () {

                    $(".se-pre-con").fadeOut("slow");

                    alert("Error while loading asset classification details.");

                });
        };


        // ------------------------------------------------
        // EMPTY DETAIL OBJECT
        // ------------------------------------------------

        $scope.GetEmptyAssetDetail = function (headerId) {

            return {
                Id: 0,
                HeaderId: headerId,
                NatureOfFacility: '',
                DateOfAdvance: null,
                Principal: 0,
                Interest: 0,
                TotalOverdue: 0,
                PeriodOfOutstanding: null,
                StandardAmount: 0,
                SubStandardAmount: 0,
                DoubtfulIAmount: 0,
                DoubtfulIIAmount: 0,
                DoubtfulIIIAmount: 0,
                LossAssetAmount: 0
            };

        };


        // ------------------------------------------------
        // PARENT HEADERS
        // ------------------------------------------------

        $scope.GetParentHeadersByTab = function (tabId) {

            return $scope.HeaderList.filter(function (item) {

                return item.TabId == tabId &&
                    (item.ParentHeaderId == null ||
                        item.ParentHeaderId == undefined);

            });

        };


        // ------------------------------------------------
        // CHILD HEADERS
        // ------------------------------------------------

        $scope.GetChildHeaders = function (parentHeaderId) {

            return $scope.HeaderList.filter(function (item) {

                return item.ParentHeaderId == parentHeaderId;

            });

        };


        // ------------------------------------------------
        // ALL DESCENDANTS
        // ------------------------------------------------

        $scope.GetAllDescendants = function (parentHeaderId) {

            var result = [];

            function GetChildren(parentId, level) {

                var children =
                    $scope.GetChildHeaders(parentId) || [];

                angular.forEach(children, function (child) {

                    child.Level = level;

                    result.push(child);

                    GetChildren(child.HeaderId, level + 1);

                });

            }

            GetChildren(parentHeaderId, 1);

            return result;

        };


        // ------------------------------------------------
        // LEAF HEADINGS
        // Only leaf headings are editable
        // ------------------------------------------------

        $scope.GetLeafHeadersByParent = function (parentHeaderId) {

            var descendants =
                $scope.GetAllDescendants(parentHeaderId);

            return descendants.filter(function (item) {

                return $scope.GetChildHeaders(item.HeaderId).length == 0;

            });

        };


        // ------------------------------------------------
        // ASSET TYPE
        // ------------------------------------------------

        $scope.GetAssetType = function (parent) {

            return parent.HeaderNameEnglish || '';

        };


        // ------------------------------------------------
        // CHANGE VIEW TAB
        // ------------------------------------------------

        $scope.SelectViewTab = function (tabId) {

            $scope.SelectedViewTab = tabId;

            if (tabId == 2) {
                $scope.CalculateSummary();
            }

        };


        // ------------------------------------------------
        // CHANGE DATABASE TAB
        // ------------------------------------------------

        $scope.SelectAccountTab = function (tabId) {

            $scope.SelectedTabId = tabId;

            $scope.GetAuditAccountHeaderMaster(tabId);

        };


        // ------------------------------------------------
        // CALCULATE ROW
        // ------------------------------------------------

        $scope.CalculateRow = function (row) {

            if (!row.AssetDetail) {
                row.AssetDetail =
                    $scope.GetEmptyAssetDetail(row.HeaderId);
            }

            row.AssetDetail.Principal =
                parseFloat(row.AssetDetail.Principal) || 0;

            row.AssetDetail.Interest =
                parseFloat(row.AssetDetail.Interest) || 0;

            $scope.CalculateSummary();

        };


        // ------------------------------------------------
        // GET ALL EDITABLE LEAF HEADERS
        // ------------------------------------------------

        $scope.GetAllAssetRows = function () {

            var result = [];

            var parents =
                $scope.GetParentHeadersByTab($scope.SelectedTabId);

            angular.forEach(parents, function (parent) {

                var rows =
                    $scope.GetLeafHeadersByParent(parent.HeaderId);

                angular.forEach(rows, function (row) {

                    if (!row.AssetDetail) {
                        row.AssetDetail =
                            $scope.GetEmptyAssetDetail(row.HeaderId);
                    }

                    result.push(row);

                });

            });

            return result;

        };


        // ------------------------------------------------
        // GRAND TOTAL
        // ------------------------------------------------

        $scope.GetGrandTotal = function (fieldName) {

            var total = 0;

            var rows = $scope.GetAllAssetRows();

            angular.forEach(rows, function (row) {

                if (row.AssetDetail) {

                    total +=
                        parseFloat(row.AssetDetail[fieldName]) || 0;

                }

            });

            return total;

        };


        // ------------------------------------------------
        // CLASSIFICATION SUMMARY
        // ------------------------------------------------

        $scope.CalculateSummary = function () {

            var categories = [

                {
                    Category: 'Standard Assets',
                    Field: 'StandardAmount',
                    Rate: 0.25
                },
                {
                    Category: 'Sub-Standard Assets (<= 12 Months)',
                    Field: 'SubStandardAmount',
                    Rate: 10
                },
                {
                    Category: 'Doubtful-I (1 to 3 Years NPA)',
                    Field: 'DoubtfulIAmount',
                    Rate: 20
                },
                {
                    Category: 'Doubtful-II (3 to 5 Years NPA)',
                    Field: 'DoubtfulIIAmount',
                    Rate: 30
                },
                {
                    Category: 'Doubtful-III (Above 5 Years NPA)',
                    Field: 'DoubtfulIIIAmount',
                    Rate: 100
                },
                {
                    Category: 'Loss Assets',
                    Field: 'LossAssetAmount',
                    Rate: 100
                }

            ];

            var rows = $scope.GetAllAssetRows();

            $scope.SummaryList = [];

            angular.forEach(categories, function (category) {

                var outstanding = 0;
                var accountCount = 0;

                angular.forEach(rows, function (row) {

                    var amount = 0;

                    if (row.AssetDetail) {

                        amount =
                            parseFloat(
                                row.AssetDetail[category.Field]
                            ) || 0;

                    }

                    outstanding += amount;

                    if (amount > 0) {
                        accountCount++;
                    }

                });

                var provisionRequired =
                    outstanding * category.Rate / 100;

                // Old provision is currently not stored in the detail table.
                var oldProvision = 0;

                $scope.SummaryList.push({

                    Category: category.Category,
                    AccountCount: accountCount,
                    Outstanding: outstanding,
                    Rate: category.Rate,
                    ProvisionRequired: provisionRequired,
                    OldProvision: oldProvision,
                    RequiredProvision:
                        Math.max(0, provisionRequired - oldProvision)

                });

            });

        };


        // ------------------------------------------------
        // SUMMARY TOTAL
        // ------------------------------------------------

        $scope.GetSummaryTotal = function (fieldName) {

            var total = 0;

            angular.forEach($scope.SummaryList, function (item) {

                total += parseFloat(item[fieldName]) || 0;

            });

            return total;

        };


        // ------------------------------------------------
        // PREPARE SAVE DETAILS
        // ------------------------------------------------

        $scope.GetAssetClassificationSaveDetails = function () {

            var details = [];

            var rows = $scope.GetAllAssetRows();

            angular.forEach(rows, function (row) {

                var item = row.AssetDetail ||
                    $scope.GetEmptyAssetDetail(row.HeaderId);

                details.push({

                    HeaderId: row.HeaderId,

                    NatureOfFacility:
                        item.NatureOfFacility || '',

                    DateOfAdvance: item.DateOfAdvance
                        ? item.DateOfAdvance.toISOString().substring(0, 10)
                        : null,

                    Principal:
                        parseFloat(item.Principal) || 0,

                    Interest:
                        parseFloat(item.Interest) || 0,

                    TotalOverdue:
                        parseFloat(item.TotalOverdue) || 0,

                    PeriodOfOutstanding:
                        item.PeriodOfOutstanding
                            ? parseInt(item.PeriodOfOutstanding, 10)
                            : null,

                    StandardAmount:
                        parseFloat(item.StandardAmount) || 0,

                    SubStandardAmount:
                        parseFloat(item.SubStandardAmount) || 0,

                    DoubtfulIAmount:
                        parseFloat(item.DoubtfulIAmount) || 0,

                    DoubtfulIIAmount:
                        parseFloat(item.DoubtfulIIAmount) || 0,

                    DoubtfulIIIAmount:
                        parseFloat(item.DoubtfulIIIAmount) || 0,

                    LossAssetAmount:
                        parseFloat(item.LossAssetAmount) || 0

                });

            });

            return details;

        };


        // ------------------------------------------------
        // COMMON SAVE FUNCTION
        // ------------------------------------------------

        $scope.SaveAssetClassification = function (moveNext) {

            if (!$scope.KVSSSocietyID ||
                $scope.KVSSSocietyID == 0) {

                alert("Please select society.");
                return;

            }

            if (!$scope.FinancialYear) {

                alert("Please select financial year.");
                return;

            }

            var details =
                $scope.GetAssetClassificationSaveDetails();

            if (!details || details.length == 0) {

                alert("No asset classification headings found.");
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
                SiteUrl + "/Auditor/SaveAuditAssetClassificationDetails",
                request
            )
                .success(function (response) {

                    $(".se-pre-con").fadeOut("slow");

                    if (response.Status == 1) {

                        alert(response.Message);

                        $scope.GetAssetClassificationDetails();

                        if (moveNext) {
                            $scope.NextStep();
                        }

                    }
                    else {

                        alert(response.Message ||
                            "Unable to save asset classification.");

                    }

                })
                .error(function () {

                    $(".se-pre-con").fadeOut("slow");

                    alert("Error while saving asset classification.");

                });

        };


        // ------------------------------------------------
        // SAVE & NEXT
        // ------------------------------------------------

        $scope.SaveAndNext = function () {

            $scope.SaveAssetClassification(true);

        };


        // ------------------------------------------------
        // SAVE AS DRAFT
        // ------------------------------------------------

        $scope.SaveAsDraft = function () {

            $scope.SaveAssetClassification(false);

        };


        // ------------------------------------------------
        // NEXT PAGE
        // ------------------------------------------------

        $scope.NextStep = function () {

            // Replace this URL with the actual next audit form route.
            // Do not navigate until the save operation succeeds.

            // window.location.href =
            //     SiteUrl + "/Auditor/YourNextAuditForm";

        };


        // ------------------------------------------------
        // BACK
        // ------------------------------------------------

        $scope.BackTo = function () {

            // Replace this URL with the actual previous audit form route.

            window.history.back();

        };


        // ------------------------------------------------
        // INITIALIZE
        // ------------------------------------------------

        $scope.OnPageLoad();

    }
);