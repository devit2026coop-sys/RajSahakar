var UserMenu = angular.module("CommodityMSPMasterMod", []);


UserMenu.controller("CommodityMSPMasterCnt", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.BuisnesData = [];
    $scope.Buisness = [];

    $scope.SaveCommodityMSP = function () {
        alert("alert now");
        var formData = new FormData();
        formData.append("Item_Kind", $scope.AddNewModel.Item_Kind);
        formData.append("Packages_Quantity_Unit", $scope.item.Packages_Quantity_Unit);
        formData.append("FinancialYear", $scope.AuditorAssign.FinancialYear);
        formData.append("Price", $scope.AddNewModel.Price);
        formData.append("NafedChargesa", $scope.AddNewModel.NafedChargesa);
        formData.append("highCharges", $scope.AddNewModel.highCharges);

        $http.post(SiteUrl + "/Master/SaveCommodityMSP", formData).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                var k = data;
                var message = '';

                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    $(".se-pre-con").fadeOut("slow");
                }
                alert(message);
                window.location = '/master/BuissnessForm';
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }
                $scope.GetAllBusinessList();

                if (data.Status) {
                    $scope.Buisness = null;
                    $scope.ClearBuisnessForm();
                    $scope.GetAllBusinessList();
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });
    };

    // START : Clear form
    $scope.ClearBuisnessForm = function () {
        $scope.Buisness.BusinessNameEn = '';
        $scope.Buisness.BusinessNameHi = '';
        $scope.Buisness.IsActive = false;
        $scope.Buisness.BusinessId = 0;
    };

    $scope.update = function (item) {
        $scope.Buisness.BusinessNameEn = item.BusinessNameEn;
        $scope.Buisness.BusinessNameHi = item.BusinessNameHi;
        $scope.Buisness.IsActive = item.IsActive == 1 ? true : false;
        $scope.Buisness.BusinessId = item.BusinessId;

    };
    $scope.pageChangeHandler = function (num) {
    };

    $scope.SetStatus = function (BusinessId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;
        $http.post(SiteUrl + "/Master/ChangeStatusBusiness", { Id: BusinessId, Action: ActionId }).
            success(function (data, status) {
                $scope.GetAllBusinessList();
            }).
            error(function (data, status) {
            });
    };
    // Get List of CLASS Template
    $scope.GetAllBuisness = function () {

        $http.get(SiteUrl + "/BuissnessForm/BusinessList").
            success(function (data, status) {
                if (angular.toJson(data).length < 1) {
                    $scope.DivSms = false;
                    $scope.newdiv = (_language == 'en-us' ? 'Data is not available' : 'डेटा उपलब्ध नहीं है ');
                    return;
                }
                $scope.BuisnesData = data;
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
            error(function (data, status) {
                alert("Error while fetching Sms Template.");
                $scope.showLoader = false;
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

            $scope.ClearBuisnessForm = function () {
                $scope.Buisness = null;
            };
        }
        //Get list of sms template page list
        $scope.GetAllBusinessList();
        $(".se-pre-con").fadeOut("slow");
    };
    // Language Selector at page load

    $scope.GetAllBusinessList = function () {
        $http.get(SiteUrl + "/Master/BusinessList").
            success(function (data, status) {
                $scope.BuisnesData = data;
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
    }

    $scope.OnPageLoad();


});

