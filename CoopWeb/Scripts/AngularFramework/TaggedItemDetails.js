var TaggedItemController = angular.module("TaggedItemCnt", ['angularUtils.directives.dirPagination']);
TaggedItemController.controller("TaggedItemMod", function ($scope, $http, $window) {

    $scope.todayDate = new Date().toISOString().split('T')[0];
    $scope.colspan = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.translation = [];
    $scope.IsFreeze = true;
    $scope.IsCompanyName = false;
    $scope.ProductTypeDataList = [];
    $scope.ProductDataList = [];
    $scope.CompanyDataList = [];
    $scope.TaggingItemDataList = [];

    $scope.TagData = {
        ProductTypeId: "0",
        ProductId: "0",
        CompanyId: "0"
    };

    $scope.calculateAmount = function (qty, price) {
        var quantity = parseFloat(qty) || 0;
        var rate = parseFloat(price) || 0;
        $scope.TagData.TotalPrice = quantity * rate;
    };
    function formatDate(dateValue) {
        if (!dateValue) return "";

        var date = new Date(dateValue);

        return date.getFullYear() + "-" +
            String(date.getMonth() + 1).padStart(2, '0') + "-" +
            String(date.getDate()).padStart(2, '0');
    }

    //New
    $scope.InsertTagDataForm = function () {
        if (!$scope.validatdataSubmit()) {
            return false;
        } else {

            var formData = new FormData();

            var isUpdate = ($scope.TagData.TagId && $scope.TagData.TagId > 0);
            formData.append("TagId", $scope.TagData.TagId || 0);
            formData.append("ProductTypeId", $scope.TagData.ProductTypeId);
            formData.append("ProductId", $scope.TagData.ProductId);
            formData.append("CompanyId", $scope.TagData.CompanyId);
            formData.append("PurchaseDate", formatDate($scope.TagData.PurchaseDate));
            formData.append("ExpirationDate", formatDate($scope.TagData.ExpirationDate));
            formData.append("PackagingType", $scope.TagData.PackagingType);
            formData.append("MeasurementUnit", $scope.TagData.MeasurementUnit);
            formData.append("ItemQuantity", $scope.TagData.ItemQuantity);
            formData.append("ItemPrice", $scope.TagData.ItemPrice);
            formData.append("TotalPrice", $scope.TagData.TotalPrice);
            formData.append("CompanyName", $scope.TagData.CompanyName);
            //formData.append("IFMSPOSId", $scope.TagData.IFMSPOSId);
            //formData.append("IsCreditFacility", $scope.TagData.IsCreditFacility);

            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveTaggingItemForm", formData, { withCredentials: true, headers: { 'Content-Type': undefined }, transformRequest: angular.identity }).success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (parseInt(data, 10) > 0) {
                    $scope.GetTaggingItemByUser();
                    var Msg = '';

                    if (isUpdate) {
                        Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया');
                    }
                    else {
                        Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    }

                    alert(Msg);
                    window.location.reload();
                }
                else if (parseInt(data, 10) == -2) {
                    var Msg = (_language == 'en-us' ? 'Record already exists for the selected Month, Year, Fertilizer Name, and Society. Duplicate demand entries are not allowed..' : 'डुप्लिकेट डेटा।');
                    alert(Msg);
                }
                else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                    var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    alert(Msg);
                }
            }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while Tagging Item insert");
                });
        }
    };

    $scope.validatdataSubmit = function () {

        var message = $scope._language == 'en-us' ? 'निम्नलिखित की पुष्टि करें : ' : 'निम्नलिखित की पुष्टि करें:';
        if ($scope.TagData.ProductTypeId == '0' || $scope.TagData.ProductTypeId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Product Type' : '\n <br /> Select Product Type';
        }
        if ($scope.TagData.ProductId == '0' || $scope.TagData.ProductId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Product Name' : '\n <br /> Select Product Name';
        }
        if ($scope.IsCompanyName == false) {
            if ($scope.TagData.CompanyId == '0' || $scope.TagData.CompanyId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Select Company Name' : '\n <br /> Select Company Name';
            }
        }
        if ($scope.IsCompanyName == true) {
            if ($scope.TagData.CompanyName == '' || $scope.TagData.CompanyName == undefined || $scope.TagData.CompanyName == null) {
                message += $scope._language == 'en-us' ? '\n <br /> Enter Company Name' : '\n <br /> Enter Company Name';
            }
        }
        if ($scope.TagData.PurchaseDate == undefined || $scope.TagData.PurchaseDate == '' || $scope.TagData.PurchaseDate == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Date of Purchase/Tagging' : '\n <br />  Enter Date of Purchase/Tagging';
        }
        if ($scope.TagData.ExpirationDate == undefined || $scope.TagData.ExpirationDate == '' || $scope.TagData.ExpirationDate == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Date of Expiration' : '\n <br /> Enter Date of Expiration';
        }
        if ($scope.TagData.PurchaseDate && $scope.TagData.ExpirationDate) {
            var purchaseDate = new Date($scope.TagData.PurchaseDate);
            var expirationDate = new Date($scope.TagData.ExpirationDate);
            if (purchaseDate >= expirationDate) {
                message += $scope._language == 'en-us' ? '\n <br /> Purchase Date must be earlier than Expiration Date' : '\n <br /> Purchase Date must be earlier than Expiration Date';
            }
        }
        if ($scope.TagData.PackagingType == '' || $scope.TagData.PackagingType == null || $scope.TagData.PackagingType == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Packaging Type' : '\n <br /> Select Packaging Type';
        }
        if ($scope.TagData.MeasurementUnit == '' || $scope.TagData.MeasurementUnit == null || $scope.TagData.MeasurementUnit == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Measurement Unit' : '\n <br /> Select Measurement Unit';
        }
        if ($scope.TagData.ItemQuantity == undefined || $scope.TagData.ItemQuantity == '0' || $scope.TagData.ItemQuantity == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Quantity' : '\n <br /> Enter Quantity';
        }
        if ($scope.TagData.ItemPrice == undefined || $scope.TagData.ItemPrice == '0' || $scope.TagData.ItemPrice == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Price Per Unit' : '\n <br /> Enter Price Per Unit';
        }
        if ($scope.TagData.TotalPrice == '0' || $scope.TagData.TotalPrice == undefined || $scope.TagData.TotalPrice == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Total Purchase Price' : '\n <br /> Enter Total Purchase Price';
        }
        if (message.length > 30) {
            ShowModelAlert("Tagging Item Details", message);
            return false;
        }
        return true;
    };

    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }

    };

    $scope.GetTaggingItemByUser = function () {
        $scope.TaggingItemDataList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Society/GetTaggingItemByUser").then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.TaggingItemDataList = JSON.parse(response.data.Result.Data.TaggedItemDetail);
            } else {
                $scope.TaggingItemDataList = [];
            }
        }, function (error) {
            console.error("Error loading data:", error);
            $scope.TaggingItemDataList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetProductType = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PRODUCTTYPE&MasterCode=-1").then(function (resp) {
            $scope.ProductTypeDataList = resp.data;
            $(".se-pre-con").fadeOut("slow");
            return resp.data;
        }).
            catch(function () {
                $scope.ProductTypeDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.GetProduct = function (productTypeId) {
        $scope.TagData.ProductId = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PRODUCT&MasterCode=" + productTypeId).success(function (data, status) {
            $scope.ProductDataList = data;
            if (productTypeId === "9") {
                $scope.IsCompanyName = true;
                $scope.TagData.CompanyId = "0";
            } else {
                $scope.IsCompanyName = false;
                $scope.TagData.CompanyName = "";
            }
            $(".se-pre-con").fadeOut("slow");
        }).
            error(function (data, status) {
                $scope.ProductDataList = [];
                $(".se-pre-con").fadeOut("slow");

            });

    };

    $scope.GetCompany = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMPANY&MasterCode=-1").then(function (resp) {
            $scope.CompanyDataList = resp.data;
            $(".se-pre-con").fadeOut("slow");
            return resp.data;
        }).
            catch(function () {
                $scope.CompanyDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
     
    $scope.updateTaggedItem = function (tagId) {
        $scope.GetTaggedItemDetailById(tagId).then(function (res) {
            if (res != null && res.length > 0) { 
                $scope.TagData.TagId = res[0].TagId;
                $scope.TagData.ProductTypeId = res[0].ProductTypeId.toString();
                $scope.GetProduct($scope.TagData.ProductTypeId);
                $scope.TagData.ProductId = res[0].ProductId.toString();
                if (res[0].CompanyId.toString() == "0") {
                    $scope.IsCompanyName = true;
                    $scope.TagData.CompanyName = res[0].CompanyNameV + '';
                } else {
                    $scope.IsCompanyName = false;
                    $scope.TagData.CompanyId = res[0].CompanyId.toString();
                }
                $scope.TagData.PurchaseDate = res[0].PurchaseDate ? new Date(res[0].PurchaseDate) : null;
                $scope.TagData.ExpirationDate = res[0].ExpirationDate ? new Date(res[0].ExpirationDate) : null;
                $scope.TagData.PackagingType = res[0].PackagingType + '';
                $scope.TagData.MeasurementUnit = res[0].MeasurementUnit + '';
                $scope.TagData.ItemQuantity = Number(res[0].ItemQuantity); 
                $scope.TagData.ItemPrice = Number(res[0].ItemPrice);
                $scope.TagData.TotalPrice = res[0].TotalPrice;
            }
        }); 
    };

    $scope.GetTaggedItemDetailById = function (tagId) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/GetTaggedItemDetailById?TagId=" + tagId).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null && response.data.Result.Data.TaggedItemDetailById != null) {
                var data = JSON.parse(response.data.Result.Data.TaggedItemDetailById);
                return data;
            }
            return [];
        }, function (error) {
            console.error("Error loading data:", error); 
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.deleteTaggedItem = function (tagId) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/DeleteTaggedItemById?TagId=" + tagId).then(function (response) {
            if (response.data != null) {
                $scope.GetTaggingItemByUser();
                return data;
            }
            return [];
        }, function (error) {
            console.error("Error loading data:", error); 
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetProductType();
        $scope.GetCompany();
        $scope.GetTaggingItemByUser();
    };

    $scope.OnPageLoad();
});;