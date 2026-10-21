var WarehouseController = angular.module("WarehouseCnt", ['angularUtils.directives.dirPagination']);
WarehouseController.controller("WarehouseMod", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.PurchaseCenterMaster = [];
    $scope.PurchaseCenterData = [];
    $scope.PurchaseCenterDataList = [];
    
    $scope.divInsert = false;
    $scope.GridWarehouses = true;

    $scope.ShowDivForInsert = function () {
        $scope.PurchaseCenterMaster.PurchaseCenterId = '0';
        $scope.PurchaseCenterMaster.DistrictId = "";
        $scope.PurchaseCenterMaster.CommodityId = "";
        $scope.divInsert = true;
        $scope.GridWarehouses = false;
        
    }

    $scope.BackToList = function () {
        $scope.divInsert = false;
        $scope.GridWarehouses = true;
        $scope.PurchaseCenterMaster.PurchaseCenterId = '0';
    }


    $scope.SavePurchaseCenterForm = function () {
        //$scope.GetAllRajfed_PurchaseCenterList();
        if ($scope.PurchaseCenterMaster.DistrictId == "" || $scope.PurchaseCenterMaster.DistrictId== undefined) {
            alert("Please select District");
        } else if ($scope.PurchaseCenterMaster.CommodityId == "" || $scope.PurchaseCenterMaster.CommodityId == undefined) {
            alert("Please select Commodity");
        } else if ($scope.PurchaseCenterMaster.PurchaseCenterName_En == "" || $scope.PurchaseCenterMaster.PurchaseCenterName_En == undefined) {
            alert("Please enter Purchase Center Name English");
        } else if ($scope.PurchaseCenterMaster.PurchaseCenterName_Hi == "" || $scope.PurchaseCenterMaster.PurchaseCenterName_Hi == undefined) {
            alert("Please enter Purchase Center Name Hindi");
        } else if ($scope.PurchaseCenterMaster.KVSS_En == "" || $scope.PurchaseCenterMaster.KVSS_En == undefined) {
            alert("Please enter KVSS Name English");
        }else if ($scope.PurchaseCenterMaster.KVSS_Hi == "" || $scope.PurchaseCenterMaster.KVSS_Hi == undefined) {
            alert("Please enter KVSS Name Hindi");
        } else {
            $(".se-pre-con").fadeIn("slow");
            var data = {
                "PurchaseCenterId": $scope.PurchaseCenterMaster.PurchaseCenterId == undefined ? "0" : $scope.PurchaseCenterMaster.PurchaseCenterId,
                "DistrictCode": $scope.PurchaseCenterMaster.DistrictId,
                "PurchaseCenterName_En": $scope.PurchaseCenterMaster.PurchaseCenterName_En,
                "PurchaseCenterName_Hi": $scope.PurchaseCenterMaster.PurchaseCenterName_Hi,
                "CommodityId": $scope.PurchaseCenterMaster.CommodityId,
                "KVSS_Hi": $scope.PurchaseCenterMaster.KVSS_Hi,
                "KVSS_En": $scope.PurchaseCenterMaster.KVSS_En,
                "Rajfed_PCID": "0",
                "Rajfed_KVSS_Id": "0"
            }
            $http.post(SiteUrl + "/Master/SavePurchaseCenter", data).

                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    var k = data;
                    var message = '';
                    //console.log(data);
                    if (data == "Purchase Center Added Successfully") {

                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                        alert(message);
                        window.location = '/master/AddPurchaseCenter';
                    }
                    else {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                   
                    


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
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while subclass insert");
                });
        }
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
        $scope.GetAllRajfed_PurchaseCenterList();
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

    $scope.Edit = function (item) {
        $scope.divInsert = true;
        $scope.GridWarehouses = false;
        $scope.PurchaseCenterMaster.PurchaseCenterId = item.PurchaseCenterId;

        

        $scope.PurchaseCenterMaster.DistrictId = $scope.Districts.filter(function (x) { return x.DISTRICT_ID == item.DistrictId })[0].DISTRICT_CODE;
        $scope.PurchaseCenterMaster.PurchaseCenterName_En = item.PurchaseCenterName_En;
        $scope.PurchaseCenterMaster.PurchaseCenterName_Hi = item.PurchaseCenterName_Hi;
        $scope.PurchaseCenterMaster.CommodityId = item.CommodityId.toString();
        $scope.PurchaseCenterMaster.KVSS_Hi = item.KVSS;
        $scope.PurchaseCenterMaster.KVSS_En = item.KVSSEn;
    }

    $scope.GetAllRajfed_PurchaseCenterList = function () {
        $scope.PurchaseCenterData = [];
        $http.get(SiteUrl + "/Master/PurchaseCenterList").
            success(function (data, status) {
                
                $scope.PurchaseCenterData = data;
                $scope.PurchaseCenterData.length;
               
               // $scope.colsort = "";
               // $scope.reverseSort = false;

                //$scope.sortData = function (column) {
                //    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                //    $scope.colsort = column;
                //};


                //$scope.getSortClass = function (column) {
                //    if ($scope.colsort == column) {
                //        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                //    }
                //    return '';
                //};

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
   

});