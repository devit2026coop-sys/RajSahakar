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
    $scope.SocietySearch = "";
    $scope.IsSocietyDropdownOpen = false;
    $scope.BlockDataList = [];

    $scope.TagData = {
        ProductTypeId: "0",
        ProductId: "0",
        SocietyType: "",
        BlockId: "0",
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

            formData.append("SocietyID", $scope.TagData.SocietyID);
            formData.append("RegistrationNo", $scope.TagData.RegistrationNo);
            formData.append("DistrictName", $scope.TagData.DistrictName);
            formData.append("DistrictId", $scope.TagData.DistrictFID);
            formData.append("BlockId", $scope.TagData.BlockId);
            formData.append("SocietyType", $scope.TagData.SocietyType);

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
                    var Msg = (_language == 'en-us' ? 'Record already exists.' : 'डुप्लिकेट डेटा।');
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
        if ($scope.TagData.SocietyID == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Select KVSS/GSS' : '\n <br /> Select KVSS/GSS';
        }
        if ($scope.TagData.DistrictName == undefined || $scope.TagData.DistrictName == '' || $scope.TagData.DistrictName == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter District Name ' : '\n <br />  Enter District Name';
        }
        //if ($scope.TagData.RegistrationNo == undefined || $scope.TagData.RegistrationNo == '' || $scope.TagData.RegistrationNo == null) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Enter Registration No' : '\n <br /> Enter Registration No';
        //}
        if ($scope.TagData.SocietyType == '' || $scope.TagData.SocietyType == null || $scope.TagData.SocietyType == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Society Type' : '\n <br /> Select Society Type';
        }
        if ($scope.TagData.BlockId == '0' || $scope.TagData.BlockId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Block' : '\n <br /> Select Block';
        }
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
        //if ($scope.TagData.PurchaseDate == undefined || $scope.TagData.PurchaseDate == '' || $scope.TagData.PurchaseDate == null) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Enter Date of Purchase/Tagging' : '\n <br />  Enter Date of Purchase/Tagging';
        //}
        if ($scope.TagData.PurchaseDate == undefined || $scope.TagData.PurchaseDate == '' || $scope.TagData.PurchaseDate == null) {
            message += '\n <br /> Enter Date of Purchase/Tagging';
        } else {
            var purchaseDate = new Date($scope.TagData.PurchaseDate);
            var today = new Date(); 
            // Ignore current time; compare only dates
            today.setHours(0, 0, 0, 0);
            purchaseDate.setHours(0, 0, 0, 0); 
            if (purchaseDate > today) {
                message += '\n <br /> Date of Purchase/Tagging cannot be a future date';
            }
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
        if (!confirm("Are you sure you want to delete this tagged item?")) {
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/DeleteTaggedItemById?TagId=" + tagId).then(function (response) {
            if (response.data != null) {
                alert("Tagged item deleted successfully.");
                $scope.GetTaggingItemByUser();
                return data;
            }
            return [];
        }, function (error) {
            console.error("Error loading data:", error); 
            alert("Failed to delete tagged item. Please try again.");
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetSocietyBySSO();
        $scope.GetProductType();
        $scope.GetCompany();
        $scope.GetTaggingItemByUser();
    };


    $scope.OnSocietyChange = function () {
        //debugger;
        $scope.TagData.RegistrationNo = "";
        $scope.TagData.DistrictFID = "";
        $scope.TagData.DistrictName = "";

        if (!$scope.SocietyDataList || $scope.SocietyDataList.length == 0) {
            console.warn("SocietyDataList is empty");
            return;
        }

        var row = $scope.SocietyDataList.find(function (x) {
            return x.SocietyID.toString() === $scope.TagData.SocietyID.toString();
        });

        if (!row) {
            console.warn("No matching society found for ID:", $scope.TagData.SocietyID);
            return;
        }

        $scope.TagData.SocietyID = row.SocietyID.toString();
        $scope.TagData.RegistrationNo = row.RegistrationNo;
        $scope.TagData.DistrictFID = row.DistrictId;
        $scope.TagData.DistrictName = row.DistrictName;
        $scope.getblock($scope.TagData.DistrictFID);
    };

    $scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/KVSSGSSbyDRorMDCCB", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data;
            $scope.SocietyDataList = $scope.TableData;
            $scope.SocietyList = $scope.SocietyDataList;
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };

    $scope.OnSocietySearchChange = function () {
        $scope.TagData.DistrictName = "";
        $scope.TagData.RegistrationNo = "";
        $scope.IsSocietyDropdownOpen = true;
    };


    $scope.OpenSocietyDropdown = function () {
        $scope.IsSocietyDropdownOpen = true;
    };


    $scope.SocietyFilter = function (item) {
        if (!$scope.SocietySearch) {
            return true;
        }

        var search = $scope.SocietySearch.toLowerCase().trim();
        var societyName = item.SocietyName ? item.SocietyName.toLowerCase() : "";
        var registrationNo = item.RegistrationNo ? item.RegistrationNo.toLowerCase() : "";
        return societyName.indexOf(search) !== -1 || registrationNo.indexOf(search) !== -1;
    };
    $scope.SelectSociety = function (item) {
        $scope.TagData.SocietyID = item.SocietyID.toString();
        $scope.SocietySearch = item.SocietyName;
        $scope.IsSocietyDropdownOpen = false;
        $scope.OnSocietyChange();
    };

    $scope.getblock = function (item) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + item).
            then(function (resp) {
                $scope.BlockDataList = resp.data;
                //$scope.BlockId = '0';
                $(".se-pre-con").fadeOut("slow");
                return resp.data;
            }).
            catch(function () {
                $scope.BlockDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };


    $scope.OnPageLoad();
});;