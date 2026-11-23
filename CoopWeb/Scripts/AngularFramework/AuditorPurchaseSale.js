var MasterMod = angular.module('auditorPurchaseSaleMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("auditorPurchaseSaleCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12; 
    $scope.AuditSPData = [];
    $scope.supportPriceRows = [];
    //$scope.FinancialYearList = [];
    //$scope.SocietyList = [];
    //$scope.KVSSSocietyID = '0';
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
    };

    //$scope.SetFinancialYears = function () {

    //    var today = new Date();

    //    var currentFYStartYear = today.getMonth() >= 3
    //        ? today.getFullYear()
    //        : today.getFullYear() - 1;

    //    // Previous financial year
    //    var previousFYStartYear = currentFYStartYear - 1;

    //    $scope.FinancialYear = previousFYStartYear + "-" +
    //        (previousFYStartYear + 1).toString().substring(2);

    //    // Generate last 10 financial years
    //    $scope.FinancialYearList = [];

    //    for (var i = 0; i < 10; i++) {

    //        var startYear = previousFYStartYear - i;

    //        var financialYear = startYear + "-" +
    //            (startYear + 1).toString().substring(2);

    //        $scope.FinancialYearList.push(financialYear);
    //    }
    //};

    //$scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {

    //    $(".se-pre-con").fadeIn("slow");

    //    $http.post(SiteUrl + "/Society/KVSSGSSbySSOD", {})
    //        .success(function (data, status) {
    //            debugger;
    //            $scope.TableData = data;
    //            $scope.SocietyList = $scope.TableData;
    //            //var selectedSociety = $scope.SocietyList.find(function (item) {
    //            //    return item.SocietyID == $scope.KVSSSocietyID;
    //            //});
    //            //var sname = selectedSociety ? selectedSociety.SocietyName : '';
    //            $(".se-pre-con").fadeOut("slow");

    //        })
    //        .error(function (data, status) {

    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.TableData = [];
    //            $scope.SocietyList = [];

    //        });
    //};
     
    //$scope.GetSelectedSocietyName = function () {
        
    //    var selectedSociety = $scope.SocietyList.find(function (item) {
    //        return item.SocietyID == $scope.KVSSSocietyID;
    //    });

    //    return selectedSociety ? selectedSociety.SocietyName : '';
    //};

    //$scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {
    //    $(".se-pre-con").fadeIn("slow");
    //    $http.post(SiteUrl + "/Society/KVSSGSSbyDRorMDCCB", {
    //        "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
    //    }).success(function (data, status) {
    //        $scope.TableData = data;
    //        //$scope.SocietyList = $scope.TableData;

    //        // Get Top 20 Records
    //        $scope.SocietyList = $scope.TableData.slice(0, 20);

    //        $(".se-pre-con").fadeOut("slow");
    //    }).error(function (data, status) {
    //        $(".se-pre-con").fadeOut("slow");
    //        $scope.TableData = [];
    //        $scope.SocietyList = [];
    //    });
    //};

    //$scope.OnSocietyChange = function () {
    //    $scope.GetAllAuditorSalePurchaseList();
    //}

    $scope.GetAllAuditorSalePurchaseList = function () {
        //$(".se-pre-con").fadeIn("slow");

        //var date = new Date();
        //var month = date.getMonth() + 1;
        //var year = date.getFullYear();

        //// Financial year April to March
        //var currentFYStartYear = month >= 4 ? year : year - 1;

        //// Previous Financial Year
        //var previousFYStartYear = currentFYStartYear - 1;
        //var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        //var finYear = previousFinancialYear;
        //$scope.FinancialYearName = finYear;
        //var kvssId = $scope.KVSSSocietyID;

        var finYear = '@Session["FinancialYear"]';
        var kvssId = '@Session["KVSSId"]';

        var finYear = '';
        var kvssId = 0;

        $http.get(SiteUrl + "/Auditor/GetAllAuditorSalePurchaseList", {
            params: { KVSSId: kvssId, financialYear: finYear }
        }).success(function (data) {
                $scope.AuditSPData = data;
                //console.log('data', data);
            $scope.BindAuditSPData(data);
                $(".se-pre-con").fadeOut("slow");
            }, function (error) {

                console.error("GetAllAuditorSalePurchaseList Error:", error);
                alert("Unable to load Audit Sale Purchase details.");
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.BindAuditSPData = function (data) {

        $scope.AuditSPGroups = [];

        if (!data || data.length === 0) {
            return;
        }

        // Get Parent Items
        var parentItems = data.filter(function (item) {
            return item.ParentItemId == null;
        });

        // Create Parent-Child Structure
        $scope.AuditSPGroups = parentItems.map(function (parent) {

            return {
                ItemId: parent.ItemId,
                ParentItemId: parent.ParentItemId,
                ItemName_Hi: parent.ItemName_Hi,
                ItemName_En: parent.ItemName_En,

                OpeningQty: parent.OpeningQty || 0,
                OpeningRate: parent.OpeningRate || 0,
                OpeningAmount: parent.OpeningAmount || 0,

                ReceiptQty: parent.ReceiptQty || 0,
                ReceiptRate: parent.ReceiptRate || 0,
                ReceiptAmount: parent.ReceiptAmount || 0,

                SalesQty: parent.SalesQty || 0,
                SalesRate: parent.SalesRate || 0,
                SalesAmount: parent.SalesAmount || 0,

                ClosingQty: parent.ClosingQty || 0,
                ClosingRate: parent.ClosingRate || 0,
                ClosingAmount: parent.ClosingAmount || 0,

                ChildItems: data.filter(function (child) {
                    return child.ParentItemId == parent.ItemId;
                })

                //ChildItems: data.filter(function (child) {
                //    return child.ParentItemId == parent.ItemId;
                //}).map(function (child) {

                //    child.IsAmountOnly =
                //        child.ParentItemId === '9' ||
                //    child.ParentItemId === '10';
                //    console.log("Amount Only:", child.IsAmountOnly);
                //    return child;
                //})
            };
        });
    };
     
    $scope.SaveAuditSPData = function () {

        var saveData = [];

        angular.forEach($scope.AuditSPGroups, function (parent) {

            // Parent Item
            saveData.push(parent);

            angular.forEach(parent.ChildItems || [], function (child) {

                // Existing Child Item
                saveData.push(child);

            });

            // Dynamically Added Rows (Parent Level)
            angular.forEach(parent.AdditionalRows || [], function (row) {
                saveData.push(row);
            });

            // Dynamically Added Rows (Child Level)
            angular.forEach(parent.ChildItems || [], function (child) {

                angular.forEach(child.AdditionalRows || [], function (row) {
                    saveData.push(row);
                });

            });

        });

        //var date = new Date();
        //var month = date.getMonth() + 1;
        //var year = date.getFullYear();

        //// Previous Financial Year
        //var currentFYStartYear = month >= 4 ? year : year - 1;
        //var previousFYStartYear = currentFYStartYear - 1;

        //var previousFinancialYear = previousFYStartYear + '-' + String(previousFYStartYear + 1).slice(-2);

        //var request = {
        //    FinancialYear: previousFinancialYear,
        //    KVSSId: $scope.KVSSSocietyID,
        //    StockDetails: saveData
        //};

        var request = {
            FinancialYear: '',
            KVSSId: 0,
            StockDetails: saveData
        };

        console.log("Save Request:", request);

        $http.post(SiteUrl + "/Auditor/SaveAuditorSalePurchase", request)
            .success(function (response) {

                if (response > 0) {
                    alert("Data saved Successfully.");
                    $scope.GetAllAuditorSalePurchaseList();
                } else {
                    alert("Unable to save data.");
                }

            })
            .error(function (error) {

                console.log(error);

                var message = error.data && error.data.Message
                    ? error.data.Message
                    : "An error occurred while saving the MPR form.";

                alert(message);
            });
    };

    //$scope.AdditionalRows = [];

    // Add New Row
    $scope.addRow = function (item) {

        if (!item.AdditionalRows) {
            item.AdditionalRows = [];
        }

        item.AdditionalRows.push({
            ItemId: 0,
            ParentItemId: item.ItemId,
            ItemName_Hi: '',
            ItemName_En: '',

            OpeningQty: 0,
            OpeningRate: 0,
            OpeningAmount: 0,

            ReceiptQty: 0,
            ReceiptRate: 0,
            ReceiptAmount: 0,

            SalesQty: 0,
            SalesRate: 0,
            SalesAmount: 0,

            ClosingQty: 0,
            ClosingRate: 0,
            ClosingAmount: 0 
        });
    };


    // Remove Row
    $scope.removeRow = function (item, index) {

        item.AdditionalRows.splice(index, 1);

    };
     
    $scope.GoBack = function () {
        window.location.href = '/KVSSAuditAccount/KVSSAuditAccount';

    };

    $scope.NextStep = function () {
        window.location.href = '/Auditor/AuditorStockList';
    };

    $scope.OnPageLoad();
    //$scope.GetSocietyBySSO();
    //$scope.GetSelectedSocietyName();
    //$scope.SetFinancialYears();
    $scope.GetAllAuditorSalePurchaseList();

});






