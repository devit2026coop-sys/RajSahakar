
var MasterMod = angular.module("PurchaseCenterMastersModule", ['angularUtils.directives.dirPagination'])
MasterMod.controller("PurchaseCenterMasterCont", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.PurchaseCenterMaster = [];

    $scope.SavePurchaseCenterForm = function () {
        $http.post(SiteUrl + "/Master/SavePurchaseCenterForm", { "PurchaseCenterId": $scope.PurchaseCenterMaster.PurchaseCenterId, "DistrictId": $scope.PurchaseCenterMaster.DistrictId, "PurchaseCenterName_En": $scope.PurchaseCenterMaster.PurchaseCenterName_En, "PurchaseCenterName_Hi": $scope.PurchaseCenterMaster.PurchaseCenterName_Hi, "CommodityId": $scope.PurchaseCenterMaster.CommodityId, "IsActive": $scope.PurchaseCenterMaster.IsActive }).
            success(function (data, status) {
                var k = data;
                var message = '';
                //console.log(data);
                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    window.location = '/master/Rajfed_PurchaseCenterList';
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                }
                alert(message);

             
                //if ($scope.ParentId == '') {
                //    $scope.ParentId = 0;
                //}
                $scope.GetAllRajfed_PurchaseCenterList();
                if (data.Status) {

                    $scope.PurchaseCenterMaster = null;
                    $scope.ClearPurchaseCenterForm();
                    //$scope.GetAllSubClassesList();
                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });
    };


    // on page load
    $scope.OnPageLoad = function () {
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
        $scope.pageChangeHandler = function (num) {

        };
        $scope.GetDistricts();
        $(".se-pre-con").fadeOut("slow");

        $scope.GetItemList();
      //  $scope.GetKVSSList();
    };


    //Get District List
    $scope.GetDistricts = function () {

        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = '';
            });

    };


    //Get Commodity List
    $scope.ItemList = [];
    // $scope.ItemList = [{ ItemId: "1", ItemNameEn: "Moong", ItemNameHi: "मूंग" }];
    $scope.GetItemList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMMODITY&MasterCode=0").
            success(function (data, status) {
                $scope.ItemList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list");

            });
    };
 

    $scope.ClearPurchaseCenterForm = function () {
        $scope.PurchaseCenterMaster.PurchaseCenterId = '0';
        $scope.PurchaseCenterMaster.DistrictId = '0';
        $scope.PurchaseCenterMaster.PurchaseCenterName_En = '';
        $scope.PurchaseCenterMaster.PurchaseCenterName_Hi = '';
        $scope.PurchaseCenterMaster.CommodityId = '0';
        $scope.PurchaseCenterMaster.KVSSId = '0';
        $scope.PurchaseCenterMaster.IsActive = false;
    };

    //Delete Status Purchase Center
    $scope.SetStatus = function (PurchaseCenterId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangedeleteStatusPurchaseCenter", { "Id": PurchaseCenterId, "ActionId": ActionId }).
            success(function (data, status) {
                $scope.GetAllRajfed_PurchaseCenterList();
            }).
            error(function (data, status) {
            });
    };

    $scope.GetAllRajfed_PurchaseCenterList = function () {
        $http.get(SiteUrl + "/Master/PurchaseCenterList").
            success(function (data, status) {
                $scope.PurchaseCenterData = data;
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

    };

    $scope.update = function (item) {
        $scope.PurchaseCenterMaster.PurchaseCenterId = item.PurchaseCenterId;
        $scope.PurchaseCenterMaster.DistrictId = item.DistrictId;
        $scope.PurchaseCenterMaster.PurchaseCenterName_En = item.PurchaseCenterName_En;
        $scope.PurchaseCenterMaster.PurchaseCenterName_Hi = item.PurchaseCenterName_Hi;
        $scope.PurchaseCenterMaster.CommodityId = item.CommodityId;
        $scope.PurchaseCenterMaster.IsActive = item.IsActive == 1 ? true : false;
    };

    $scope.OnPageLoad();
    $scope.GetAllRajfed_PurchaseCenterList();

});


