
var app = angular.module('assetClassificationMod', []);

app.controller('assetClassificationCtrl', [
    '$scope',
    '$http',
    '$window',
    function ($scope, $http, $window, $timeout) {

        // ============================================
        // 1. INITIALIZATION
        // ============================================

        $scope.TabList = [];
        $scope.HeaderList = [];
        $scope.SummaryList = [];

        $scope.SelectedViewTab = 1;
        $scope.SelectedTabId = null;

        $scope.KVSSSocietyID = 128090;//  $scope.KVSSSocietyID || null;
        $scope.FinancialYear = '2025-26'; //$scope.FinancialYear || '';

        $scope.FinancialYearEnd = '';

        $scope.IsLoading = false;

        $scope.AssetTypeMap = {
            'Loans & Advances': 'Loan',
            'Other Assets including Sundry Debtors': 'Other Assets'
        };

        // ============================================
        // 2. FINANCIAL YEAR END
        // ============================================

        function setFinancialYearEnd() {

            if (!$scope.FinancialYear) {
                $scope.FinancialYearEnd = '';
                return;
            }

            var parts = $scope.FinancialYear.split('-');

            if (parts.length === 2) {
                $scope.FinancialYearEnd = '31 March 20' + parts[1];
            }
        }

        setFinancialYearEnd();

        // ============================================
        // 3. LOAD TABS AND HEADERS
        // ============================================

        $scope.GetAuditAssetClassificationDetails = function () {

            console.log('GetAuditAssetClassificationDetails called');

            console.log('KVSS ID:', $scope.KVSSSocietyID);
            console.log('Financial Year:', $scope.FinancialYear);

            if (!$scope.KVSSSocietyID || !$scope.FinancialYear) {
                console.error('KVSS ID or Financial Year is missing.');
                return;
            }

            $scope.IsLoading = true;

            $http({
                method: 'GET',
                url: SiteUrl + 'Auditor/GetAuditAssetClassificationDetails',
                params: {
                    kvssId: $scope.KVSSSocietyID,
                    financialYear: $scope.FinancialYear,
                    tabId: null
                }
            }).then(function (response) {

                console.log('API Response:', response.data);

                var result = response.data;

                $scope.TabList = result.TabList || [];
                $scope.HeaderList = result.HeaderList || [];

                console.log('TabList:', $scope.TabList);
                console.log('HeaderList:', $scope.HeaderList);

                // Select first tab AFTER loading API data
                if ($scope.TabList.length > 0) {

                    $scope.SelectedTabId = $scope.TabList[0].TabId;

                } else {

                    $scope.SelectedTabId = null;

                }

                console.log('SelectedTabId:', $scope.SelectedTabId);

                // Initialize details
                $scope.InitializeAssetDetails();

                // Calculate summary
                $scope.CalculateSummary();

            }, function (error) {

                console.error('API Error:', error);

                alert('Unable to load asset classification details.');

            }).finally(function () {

                $scope.IsLoading = false;

            });

        };

        // ============================================
        // 4. INITIALIZE ASSET DETAILS
        // ============================================

        $scope.InitializeAssetDetails = function () {

            angular.forEach($scope.HeaderList, function (header) {

                header.AssetDetail = {

                    AssetClassificationId:
                        header.AssetClassificationId || 0,

                    NatureOfFacility:
                        header.NatureOfFacility || '',

                    DateOfAdvance:
                        header.DateOfAdvance
                            ? new Date(header.DateOfAdvance)
                            : null,

                    Principal:
                        Number(header.Principal) || 0,

                    Interest:
                        Number(header.Interest) || 0,

                    TotalOverdue:
                        Number(header.TotalOverdue) || 0,

                    PeriodOfOutstanding:
                        Number(header.PeriodOfOutstanding) || 0,

                    StandardAmount:
                        Number(header.StandardAmount) || 0,

                    SubStandardAmount:
                        Number(header.SubStandardAmount) || 0,

                    DoubtfulIAmount:
                        Number(header.DoubtfulIAmount) || 0,

                    DoubtfulIIAmount:
                        Number(header.DoubtfulIIAmount) || 0,

                    DoubtfulIIIAmount:
                        Number(header.DoubtfulIIIAmount) || 0,

                    LossAssetAmount:
                        Number(header.LossAssetAmount) || 0
                };

            });
        };

        // ============================================
        // 5. GET PARENT HEADERS
        // ============================================

        $scope.GetParentHeadersByTab = function (tabId) {

            console.log('Selected TabId:', tabId);

            console.log('HeaderList Count:', $scope.HeaderList.length);

            angular.forEach($scope.HeaderList, function (header, index) {

                console.log('Header ' + index, {
                    HeaderId: header.HeaderId,
                    ParentHeaderId: header.ParentHeaderId,
                    TabId: header.TabId,
                    HeaderNameEnglish: header.HeaderNameEnglish
                });

            });

            var result = $scope.HeaderList.filter(function (header) {

                return Number(header.TabId) === Number(tabId)
                    && Number(header.ParentHeaderId || 0) === 0;

            });

            console.log('Parent Headers:', result);

            return result;
        };

        // ============================================
        // 6. GET LEAF HEADERS
        // ============================================

        $scope.GetLeafHeadersByParent = function (parentId) {

            var result = $scope.HeaderList.filter(function (header) {

                return Number(header.ParentHeaderId) === Number(parentId);

            }).filter(function (child) {

                var hasChildren = $scope.HeaderList.some(function (header) {

                    return Number(header.ParentHeaderId) ===
                        Number(child.HeaderId);

                });

                return !hasChildren;

            });

            console.log('Leaf Headers for Parent:', parentId, result);

            return result;
        };

        // ============================================
        // 7. GET ASSET TYPE
        // ============================================

        $scope.GetAssetType = function (parent) {

            if (!parent) {
                return '';
            }

            return $scope.AssetTypeMap[
                parent.HeaderNameEnglish
            ] || parent.HeaderNameEnglish;
        };

        // ============================================
        // 8. CALCULATE ROW TOTAL
        // ============================================

        $scope.CalculateRow = function (row) {

            if (!row || !row.AssetDetail) {
                return;
            }

            var detail = row.AssetDetail;

            detail.Principal = Number(detail.Principal) || 0;
            detail.Interest = Number(detail.Interest) || 0;

            detail.TotalOutstanding =
                detail.Principal + detail.Interest;

            $scope.CalculateSummary();
        };

        // ============================================
        // 9. GET GRAND TOTAL
        // ============================================

        $scope.GetGrandTotal = function (field) {

            var total = 0;

            angular.forEach($scope.HeaderList, function (header) {

                if (header.ParentHeaderId &&
                    Number(header.ParentHeaderId) !== 0 &&
                    !$scope.HeaderList.some(function (child) {
                        return Number(child.ParentHeaderId) ===
                            Number(header.HeaderId);
                    })) {

                    var detail = header.AssetDetail || {};

                    total += Number(detail[field]) || 0;
                }

            });

            return total;
        };

        // ============================================
        // 10. CALCULATE CLASSIFICATION SUMMARY
        // ============================================

        $scope.CalculateSummary = function () {

            var summaryFields = [

                {
                    Category: 'Standard',
                    Field: 'StandardAmount',
                    Rate: 0
                },
                {
                    Category: 'Sub-Standard',
                    Field: 'SubStandardAmount',
                    Rate: 0
                },
                {
                    Category: 'Doubtful-I',
                    Field: 'DoubtfulIAmount',
                    Rate: 0
                },
                {
                    Category: 'Doubtful-II',
                    Field: 'DoubtfulIIAmount',
                    Rate: 0
                },
                {
                    Category: 'Doubtful-III',
                    Field: 'DoubtfulIIIAmount',
                    Rate: 0
                },
                {
                    Category: 'Loss Asset',
                    Field: 'LossAssetAmount',
                    Rate: 0
                }

            ];

            $scope.SummaryList = [];

            angular.forEach(summaryFields, function (item) {

                var accountCount = 0;
                var outstanding = 0;

                angular.forEach($scope.HeaderList, function (header) {

                    if (!header.ParentHeaderId ||
                        Number(header.ParentHeaderId) === 0) {
                        return;
                    }

                    var hasChildren = $scope.HeaderList.some(
                        function (child) {
                            return Number(child.ParentHeaderId) ===
                                Number(header.HeaderId);
                        }
                    );

                    if (hasChildren) {
                        return;
                    }

                    var detail = header.AssetDetail || {};

                    var amount = Number(detail[item.Field]) || 0;

                    if (amount > 0) {
                        accountCount++;
                    }

                    outstanding += amount;

                });

                var provisionRequired =
                    outstanding * item.Rate / 100;

                $scope.SummaryList.push({

                    Category: item.Category,

                    AccountCount: accountCount,

                    Outstanding: outstanding,

                    Rate: item.Rate,

                    ProvisionRequired: provisionRequired,

                    OldProvision: 0,

                    RequiredProvision: provisionRequired

                });

            });

        };

        // ============================================
        // 11. SUMMARY TOTAL
        // ============================================

        $scope.GetSummaryTotal = function (field) {

            var total = 0;

            angular.forEach($scope.SummaryList, function (item) {

                total += Number(item[field]) || 0;

            });

            return total;
        };

        // ============================================
        // 12. SELECT VIEW TAB
        // ============================================

        $scope.SelectViewTab = function (tabNo) {

            $scope.SelectedViewTab = tabNo;

            $scope.CalculateSummary();

        };

        // ============================================
        // 13. VALIDATE DETAILS
        // ============================================

        $scope.ValidateAssetDetails = function () {

            var isValid = true;

            angular.forEach($scope.HeaderList, function (header) {

                if (!header.ParentHeaderId ||
                    Number(header.ParentHeaderId) === 0) {
                    return;
                }

                var hasChildren = $scope.HeaderList.some(
                    function (child) {
                        return Number(child.ParentHeaderId) ===
                            Number(header.HeaderId);
                    }
                );

                if (hasChildren) {
                    return;
                }

                var detail = header.AssetDetail || {};

                var classifiedTotal =
                    (Number(detail.StandardAmount) || 0) +
                    (Number(detail.SubStandardAmount) || 0) +
                    (Number(detail.DoubtfulIAmount) || 0) +
                    (Number(detail.DoubtfulIIAmount) || 0) +
                    (Number(detail.DoubtfulIIIAmount) || 0) +
                    (Number(detail.LossAssetAmount) || 0);

                var outstanding =
                    (Number(detail.Principal) || 0) +
                    (Number(detail.Interest) || 0);

                if (classifiedTotal > outstanding) {
                    isValid = false;
                }

            });

            if (!isValid) {

                alert(
                    'Classification amount cannot be greater than ' +
                    'Principal + Interest outstanding.'
                );

            }

            return isValid;
        };

        // ============================================
        // 14. PREPARE SAVE DATA
        // ============================================

        $scope.PrepareSaveData = function () {

            var details = [];

            angular.forEach($scope.HeaderList, function (header) {

                if (!header.ParentHeaderId ||
                    Number(header.ParentHeaderId) === 0) {
                    return;
                }

                var hasChildren = $scope.HeaderList.some(
                    function (child) {
                        return Number(child.ParentHeaderId) ===
                            Number(header.HeaderId);
                    }
                );

                if (hasChildren) {
                    return;
                }

                var detail = header.AssetDetail || {};

                details.push({

                    HeaderId: header.HeaderId,

                    AssetClassificationId:
                        detail.AssetClassificationId || 0,

                    NatureOfFacility:
                        detail.NatureOfFacility || '',

                    DateOfAdvance: detail.DateOfAdvance
                        ? new Date(detail.DateOfAdvance)
                            .toISOString()
                            .split('T')[0]
                        : null,

                    Principal: Number(detail.Principal) || 0,

                    Interest: Number(detail.Interest) || 0,

                    TotalOverdue:
                        Number(detail.TotalOverdue) || 0,

                    PeriodOfOutstanding:
                        Number(detail.PeriodOfOutstanding) || 0,

                    StandardAmount:
                        Number(detail.StandardAmount) || 0,

                    SubStandardAmount:
                        Number(detail.SubStandardAmount) || 0,

                    DoubtfulIAmount:
                        Number(detail.DoubtfulIAmount) || 0,

                    DoubtfulIIAmount:
                        Number(detail.DoubtfulIIAmount) || 0,

                    DoubtfulIIIAmount:
                        Number(detail.DoubtfulIIIAmount) || 0,

                    LossAssetAmount:
                        Number(detail.LossAssetAmount) || 0

                });

            });

            return {

                KVSSId: $scope.KVSSSocietyID,

                FinancialYear: $scope.FinancialYear,

                Details: details

            };
        };

        // ============================================
        // 15. SAVE DATA
        // ============================================

        $scope.SaveAssetClassification = function (isFinalSubmit) {

            if (!$scope.ValidateAssetDetails()) {
                return;
            }

            var saveData = $scope.PrepareSaveData();

            console.log('Asset Classification Save Data:', saveData);

            /*
                IMPORTANT:

                Connect this function to your actual
                Save Asset Classification MVC action.

                Expected payload:

                {
                    KVSSId: 1,
                    FinancialYear: '2025-26',
                    Details: [...]
                }

                The GET action supplied in your question
                is only for retrieving data.
            */

            alert(
                'Save API is not connected yet. ' +
                'Please configure the save action.'
            );

        };

        // ============================================
        // 16. SAVE AS DRAFT
        // ============================================

        $scope.SaveAsDraft = function () {

            $scope.SaveAssetClassification(false);

        };

        // ============================================
        // 17. SAVE & NEXT
        // ============================================

        $scope.SaveAndNext = function () {

            if ($scope.SelectedViewTab === 1) {

                $scope.SelectedViewTab = 2;

                $scope.CalculateSummary();

            } else {

                $scope.SaveAssetClassification(true);

            }

        };

        // ============================================
        // 18. BACK
        // ============================================

        $scope.BackTo = function () {

            if ($scope.SelectedViewTab === 2) {

                $scope.SelectedViewTab = 1;

                return;
            }

            $window.history.back();

        };

        // ============================================
        // INITIAL LOAD - WATCH NG-INIT VALUES
        // ============================================

         


        // Initial API call after ng-init values are assigned
        $timeout(function () {

            console.log('KVSSSocietyID:', $scope.KVSSSocietyID);
            console.log('FinancialYear:', $scope.FinancialYear);

            if ($scope.KVSSSocietyID && $scope.FinancialYear) {

                $scope.GetAuditAssetClassificationDetails();

            } else {

                console.log('KVSS ID or Financial Year is missing.');

            }

        }, 0);

    }
]);
