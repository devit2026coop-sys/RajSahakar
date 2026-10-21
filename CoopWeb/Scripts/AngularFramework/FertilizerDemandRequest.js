var FertilizerDemandController = angular.module("FertilizerDemandCnt", ['angularUtils.directives.dirPagination']);
FertilizerDemandController.controller("FertilizerDemandMod", function ($scope, $http, $window) {

    $scope.todayDate = new Date().toISOString().split('T')[0];
    $scope.colspan = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.translation = [];
    $scope.IsFreeze = true;
    $scope.OtherFertilizer = false;
    $scope.Fertilizers = [];
    $scope.FertilizerDataList = [];
    $scope.FertilizerCreditList = [];
    $scope.FertilizerDemandList = [];
    $scope.FertilizerPurchaseList = [];
    $scope.SocietySearch = "";
    $scope.IsSocietyDropdownOpen = false;
    $scope.BlockDataList = [];
    $scope.GramPancyatList = [];

    $scope.DemandList = [{
        DemandId: "",
        FertilizerItemId: "",
        FertilizerCompany: "",
        FertilizerQuantity: "",
        MonthId: "",
        YearId: ""
    }];

    $scope.AddDemand = function () {
        if ($scope.DemandList.length > 0) {

            var lastRow = $scope.DemandList[$scope.DemandList.length - 1];
            if (!lastRow.FertilizerItemId || !lastRow.MonthId || !lastRow.YearId ||
                !lastRow.FertilizerCompany ||
                !lastRow.FertilizerQuantity) {
                alert("Please fill the current row before adding a new row.");
                return;
            }
        }

        $scope.DemandList.push({
            FertilizerItemId: "",
            FertilizerCompany: "",
            FertilizerQuantity: "",
            MonthId: "",
            YearId: ""
        });
    };

    $scope.FDRData = {
        IsCreditFacility: true,
        IsLicense: true,
        LicenseNo: "",
        SocietyType: "",
        BlockId: "0",
        GPId: "0"
    };

    $scope.RemoveDemand = function (index) {
        $scope.DemandList.splice(index, 1);
    };

    $scope.IsFertilizerSelected = function (fitemId, currentItem) {
        for (var i = 0; i < $scope.DemandList.length; i++) {

            if ($scope.DemandList[i] != currentItem &&
                $scope.DemandList[i].FertilizerItemId == fitemId) {
                return true;
            }
        }
        return false;
    };

    $scope.CreditFacilityList = [{
        CreditFacilityId: "",
        FertilizerItemId: "",
        FertilizerSale: "",
        FertilizerQuantity: "",
        MonthId: "",
        YearId: "",
        PresentAvailability: "",
        CreditLimit: ""
    }];

    $scope.AddCreditFacility = function () {
        if ($scope.CreditFacilityList.length > 0) {
            var lastRow = $scope.CreditFacilityList[$scope.CreditFacilityList.length - 1];
            if (!lastRow.MonthId || !lastRow.YearId || !lastRow.FertilizerItemId || !lastRow.FertilizerSale || !lastRow.FertilizerQuantity ||
                !lastRow.PresentAvailability || !lastRow.CreditLimit) {
                alert("Please fill the current row before adding a new row.");
                return;
            }
        }

        $scope.CreditFacilityList.push({
            FertilizerItemId: "",
            FertilizerSale: "",
            FertilizerQuantity: "",
            MonthId: "",
            YearId: "",
            PresentAvailability: "",
            CreditLimit: ""
        });
    };

    $scope.RemoveCreditFacility = function (index) {
        $scope.CreditFacilityList.splice(index, 1);
    };

    $scope.IsCreditFacilitySelected = function (fitemId, currentItem) {
        for (var i = 0; i < $scope.CreditFacilityList.length; i++) {
            if ($scope.CreditFacilityList[i] != currentItem && $scope.CreditFacilityList[i].FertilizerItemId == fitemId) {
                return true;
            }
        }
        return false;
    };

    $scope.PurchaseList = [{
        PurchaseId: "",
        FertilizerItemId: "",
        FertilizerQuantity: ""
    }];

    $scope.AddPurchase = function () {
        if ($scope.PurchaseList.length > 0) {

            var lastRow = $scope.PurchaseList[$scope.PurchaseList.length - 1];
            if (!lastRow.FertilizerItemId ||
                !lastRow.FertilizerQuantity) {
                alert("Please fill the current row before adding a new row.");
                return;
            }
        }

        $scope.PurchaseList.push({
            FertilizerItemId: "",
            FertilizerQuantity: ""
        });
    };


    $scope.RemovePurchase = function (index) {
        $scope.PurchaseList.splice(index, 1);
    };

    //New
    $scope.InsertFDRDataForm = function () {
        if (!$scope.validatdataSubmit()) {
            return false;
        } else {

            var formData = new FormData();

            var isUpdate = ($scope.FDRData.FDRId && $scope.FDRData.FDRId > 0);
            formData.append("FDRId", $scope.FDRData.FDRId || 0);
            formData.append("SocietyID", $scope.FDRData.SocietyID);
            formData.append("RegistrationNo", $scope.FDRData.RegistrationNo);
            formData.append("DistrictName", $scope.FDRData.DistrictName);
            formData.append("SecretaryName", $scope.FDRData.SecretaryName);
            formData.append("SecretaryMobNo", $scope.FDRData.SecretaryMobNo);
            formData.append("IsLicense", $scope.FDRData.IsLicense);
            formData.append("LicenseNo", $scope.FDRData.LicenseNo = $scope.FDRData.LicenseNo ? $scope.FDRData.LicenseNo.trim() : "");
            formData.append("DemandMonth", "0");
            formData.append("DemandYear", "0");
            formData.append("DistrictId", $scope.FDRData.DistrictFID);
            formData.append("BlockId", $scope.FDRData.BlockId);
            formData.append("GPId", $scope.FDRData.GPId);
            formData.append("SocietyType", $scope.FDRData.SocietyType);
            formData.append("IFMSPOSId", $scope.FDRData.IFMSPOSId);
            formData.append("IsCreditFacility", $scope.FDRData.IsCreditFacility);

            var demandList = angular.copy($scope.DemandList);
            angular.forEach(demandList, function (d) {
                if (d.FertilizerItemId == 0 || d.FertilizerItemId == null || d.FertilizerItemId === "") {
                    demandList = null;
                }
            });

            formData.append("DemandList", angular.toJson(demandList));

            var creditFacilityList = angular.copy($scope.CreditFacilityList);
            angular.forEach(creditFacilityList, function (v) {
                if (v.FertilizerItemId == 0 || v.FertilizerItemId == null || v.FertilizerItemId === "") {
                    creditFacilityList = null;
                }
            });
            formData.append("CreditFacilityList", angular.toJson(creditFacilityList));

            var purchaseList = angular.copy($scope.PurchaseList);
            angular.forEach(purchaseList, function (p) {
                if (p.FertilizerItemId == 0 || p.FertilizerItemId == null || p.FertilizerItemId === "") {
                    purchaseList = null;
                }
            });
            formData.append("PurchaseList", angular.toJson(purchaseList));

            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveFertilizerForm", formData, { withCredentials: true, headers: { 'Content-Type': undefined }, transformRequest: angular.identity }).success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (parseInt(data, 10) > 0) {
                    $scope.GetFertilizerDetailByUser();
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
                    alert("Error while Fertilizer demand insert");
                });
        }
    };

    $scope.validatdataSubmit = function () {

        var message = $scope._language == 'en-us' ? 'निम्नलिखित की पुष्टि करें : ' : 'निम्नलिखित की पुष्टि करें:';
        if ($scope.FDRData.SocietyID == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Select KVSS/GSS' : '\n <br /> Select KVSS/GSS';
        }
        if ($scope.FDRData.DistrictName == undefined || $scope.FDRData.DistrictName == '' || $scope.FDRData.DistrictName == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter District Name ' : '\n <br />  Enter District Name';
        }
        if ($scope.FDRData.RegistrationNo == undefined || $scope.FDRData.RegistrationNo == '' || $scope.FDRData.RegistrationNo == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Registration No' : '\n <br /> Enter Registration No';
        }
        if ($scope.FDRData.SecretaryName == undefined || $scope.FDRData.SecretaryName == '' || $scope.FDRData.SecretaryName == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Society Manager Name' : '\n <br /> Enter Society Manager Name';
        }
        if ($scope.FDRData.SecretaryMobNo == undefined || $scope.FDRData.SecretaryMobNo == '' || $scope.FDRData.SecretaryMobNo == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter Society Manager Mobile No' : '\n <br /> Enter Society Manager Mobile No';
        }
        if ($scope.FDRData.IsLicense == true) {
            if ($scope.FDRData.LicenseNo == undefined || $scope.FDRData.LicenseNo == '' || $scope.FDRData.LicenseNo == null) {
                message += $scope._language == 'en-us' ? '\n <br /> Enter License No' : '\n <br /> Enter License No';
            }
        }
        if ($scope.FDRData.SocietyType == '' || $scope.FDRData.SocietyType == null || $scope.FDRData.SocietyType == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Society Type' : '\n <br /> Select Society Type';
        }
        if ($scope.FDRData.BlockId == '0' || $scope.FDRData.BlockId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Select Block' : '\n <br /> Select Block';
        }
        if ($scope.FDRData.IFMSPOSId == '' || $scope.FDRData.IFMSPOSId == undefined || $scope.FDRData.IFMSPOSId == null) {
            message += $scope._language == 'en-us' ? '\n <br /> Enter IFMSPOS Id' : '\n <br /> Enter IFMSPOS Id';
        }
        var demandList = angular.copy($scope.DemandList);
        angular.forEach(demandList, function (d) {
            if (d.FertilizerItemId == 0 || d.FertilizerItemId == null || d.FertilizerItemId === "" || d.FertilizerQuantity == 0 || d.FertilizerQuantity == null || d.FertilizerQuantity === "") {
                message += $scope._language == 'en-us' ? '\n <br /> Enter at least one row in fertilizer demand list' : '\n <br /> Enter at least one row in fertilizer demand list';
            }
        });
        if ($scope.FDRData.IsCreditFacility == false) {
            var creditFacilityList = angular.copy($scope.CreditFacilityList);
            angular.forEach(creditFacilityList, function (v) {
                if (v.FertilizerItemId == 0 || v.FertilizerItemId == null || v.FertilizerItemId === "") {
                    message += $scope._language == 'en-us' ? '\n <br /> Enter at least one row in fertilizer Credit Facility List' : '\n <br /> Enter at least one row in fertilizer Credit Facility List';
                }
            });
        }
        var purchaseList = angular.copy($scope.PurchaseList);
        angular.forEach(purchaseList, function (p) {
            if (p.FertilizerItemId == 0 || p.FertilizerItemId == null || p.FertilizerItemId === "") {
                message += $scope._language == 'en-us' ? '\n <br /> Enter at least one row in fertilizer purchase list' : '\n <br /> Enter at least one row in fertilizer purchase list';
            }
        });
        //if ($scope.FDRData.IsCreditFacility == false) {
        //    var creditFacilityList = angular.copy($scope.CreditFacilityList);
        //    angular.forEach(creditFacilityList, function (v) {
        //        if (v.FertilizerItemId == 0 || v.FertilizerItemId == null || v.FertilizerItemId === "" || v.FertilizerQuantity == 0 || v.FertilizerQuantity == null || v.FertilizerQuantity === "") {
        //            message += $scope._language == 'en-us' ? '\n <br /> Enter at least one row in fertilizer Credit Facility List' : '\n <br /> Enter at least one row in fertilizer Credit Facility List';
        //        }
        //    });
        //}
        //var purchaseList = angular.copy($scope.PurchaseList);
        //angular.forEach(purchaseList, function (p) {
        //    if (p.FertilizerItemId == 0 || p.FertilizerItemId == null || p.FertilizerItemId === "" || p.FertilizerQuantity == 0 || p.FertilizerQuantity == null || p.FertilizerQuantity === "") {
        //        message += $scope._language == 'en-us' ? '\n <br /> Enter at least one row in fertilizer purchase list' : '\n <br /> Enter at least one row in fertilizer purchase list';
        //    }
        //});
        if (message.length > 30) {
            ShowModelAlert("Fertilizer Demand Request", message);
            return false;
        }
        return true;
    };

    $scope.calculateAmount = function (demandLst) {
        var quantity = parseFloat(demandLst.FertilizerQuantity) || 0;
        var rate = parseFloat(demandLst.FertilizerRate) || 0;
        demandLst.FertilizerCost = quantity * rate;
    };

    $scope.calculateQtyPrice = function (creditFacilityLst) {
        var quantity = parseFloat(creditFacilityLst.FertilizerQuantity) || 0;
        var price = parseFloat(creditFacilityLst.FertilizerRate) || 0;
        creditFacilityLst.FertilizerCost = quantity * price;
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

    $scope.GetSocietyList = function () {
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Godown/GetSocietyData/?Type=" + $scope.type + "&DistrictId=" + $('#district_MemberAdd').val() + "&BlockId=" + ($scope.UralUrban == 1 ? $("#ddlblocki_MemberAdd").val() : 0) + "&CityId=" + ($scope.UralUrban == 1 ? 0 : $("#ddlcity_MemberAdd").val()) + "&GPId=" + ($scope.UralUrban == 1 ? $("#ddlGP_MemberAdd").val() : 0)).
            success(function (data, status) {
                $scope.SocietyList = data;

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching SocietyList  data.');
            });
    };

    $scope.GetMonths = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").success(function (data) {

            var currentMonth = new Date().getMonth() + 1; // January = 1

            $scope.MonthList = data.filter(function (item) {
                return Number(item.Month_Id) >= currentMonth;
            });
            //$scope.MonthList = data;
            //console.log('item', currentMonth);

        }).error(function () {
            $scope.MonthList = [];
        });
    };

    $scope.GetFinancialYear = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data) {
                if (data != null) {
                    $scope.FinancialYearList = data
                        .map(function (item) {
                            return {
                                FinYearId: item.FIN_YEAR_MASTR_ID,
                                FinYearName: item.FIN_YEAR_START
                            };
                        })
                        .filter(function (item) {
                            return item.FinYearId == '1086';
                        });

                    if ($scope.FinancialYearList.length > 0) {
                        $scope.FinYear = '1086';
                    }
                }
                else {
                    $scope.FinancialYearList = [];
                }

            }).error(function () {
                $scope.FinancialYearList = [];
            });
    };

    $scope.GetFertilizerList = function () {
        $scope.OtherFertilizer = false;
        $http.get(SiteUrl + "/Society/GetFertilizerCategory").success(function (data) {
            $scope.Fertilizers = data;
        }).error(function () {
            $scope.Fertilizers = [];
            $scope.OtherFertilizer = false;
        });
    };
    $scope.ChangeFertilizer = function (demandLst) {
        if (!demandLst || !demandLst.FertilizerItemId) {
            demandLst.OtherFertilizer = false;
            demandLst.OtherFertilizerName = "";
            return;
        }

        var selectedFertilizer = $scope.Fertilizers.find(function (x) {
            return x.FertilizerId == demandLst.FertilizerItemId;
        });

        if (selectedFertilizer && selectedFertilizer.FertilizerName_En === "Other") {
            demandLst.OtherFertilizer = true;
        } else {
            demandLst.OtherFertilizer = false;
            demandLst.OtherFertilizerName = "";
        }
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

    $scope.getGrampanchyat = function (blockId) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + blockId).success(function (data, status) {
            $scope.GramPancyatList = data;
            //$scope.PanchayatId = '0';
            $(".se-pre-con").fadeOut("slow");          
        }).
            error(function (data, status) {
                $scope.GramPancyatList = [];
                $(".se-pre-con").fadeOut("slow");

            });

    };

    $scope.IsFertilizerSelected = function (fertilizerId, currentRow) {
        var fertilizer = $scope.Fertilizers.find(function (x) {
            return x.FertilizerId == fertilizerId;
        });

        if (fertilizer && fertilizer.FertilizerName_En === "Other") {
            return false;
        }

        return $scope.DemandList.some(function (item) {
            return item !== currentRow && item.FertilizerItemId == fertilizerId;
        });
    };
    $scope.IsCFFertilizerSelected = function (fertilizerId, currentRow) {
        var fertilizer = $scope.Fertilizers.find(function (x) {
            return x.FertilizerId == fertilizerId;
        });

        if (fertilizer && fertilizer.FertilizerName_En === "Other") {
            return false;
        }
        return $scope.CreditFacilityList.some(function (item) {
            return item !== currentRow && item.FertilizerItemId == fertilizerId;
        });
    };
    $scope.IsPFertilizerSelected = function (fertilizerId, currentRow) {
        var fertilizer = $scope.Fertilizers.find(function (x) {
            return x.FertilizerId == fertilizerId;
        });

        if (fertilizer && fertilizer.FertilizerName_En === "Other") {
            return false;
        }
        return $scope.PurchaseList.some(function (item) {
            return item !== currentRow && item.FertilizerItemId == fertilizerId;
        });
    };
    $scope.ChangeCreditFacilityFertilizer = function (creditFacilityLst) {
        if (!creditFacilityLst || !creditFacilityLst.FertilizerItemId) {
            creditFacilityLst.OtherFertilizer = false;
            creditFacilityLst.OtherFertilizerName = "";
            return;
        }
        var selectedFertilizer = $scope.Fertilizers.find(function (x) {
            return x.FertilizerId == creditFacilityLst.FertilizerItemId;
        });

        if (selectedFertilizer && selectedFertilizer.FertilizerName_En === "Other") {
            creditFacilityLst.OtherFertilizer = true;

        } else {
            creditFacilityLst.OtherFertilizer = false;
            creditFacilityLst.OtherFertilizerName = "";
        }
    };
    $scope.GetFertilizerSubCategoryList = function (ferilizerID) {
        $scope.OtherFertilizer = false;
        $http.get(SiteUrl + "/Society/GetFertilizerCategory").
            success(function (data, status) {
                var selectedFertilizer = $scope.Fertilizers.find(function (x) {
                    return x.FertilizerId == ferilizerID;
                });

                if (selectedFertilizer && selectedFertilizer.FertilizerName_En === "Other") {
                    $scope.OtherFertilizer = true;
                }

                $scope.FertilizerSubCatData = data;
            }).
            error(function (data, status) {
                $scope.FertilizerSubCatData = [];
            });
    };
    $scope.OnSocietyChange = function () {
        //debugger;
        $scope.FDRData.RegistrationDate = "";
        $scope.FDRData.RegistrationNo = "";
        $scope.FDRData.DistrictFID = "";
        $scope.FDRData.DistrictName = "";
        $scope.FDRData.SecretaryName = "";
        $scope.FDRData.SecretaryMobNo = "";

        if (!$scope.SocietyDataList || $scope.SocietyDataList.length == 0) {
            console.warn("SocietyDataList is empty");
            return;
        }

        var row = $scope.SocietyDataList.find(function (x) {
            return x.SocietyID.toString() === $scope.FDRData.SocietyID.toString();
        });

        if (!row) {
            console.warn("No matching society found for ID:", $scope.FDRData.SocietyID);
            return;
        }

        $scope.FDRData.SocietyID = row.SocietyID.toString();
        //$scope.FDRData.RegistrationDate = row.RegistrationDate;
        $scope.FDRData.RegistrationNo = row.RegistrationNo;
        $scope.FDRData.DistrictFID = row.DistrictId;
        $scope.FDRData.DistrictName = row.DistrictName;
        $scope.FDRData.SecretaryName = row.SecretaryName;
        $scope.FDRData.SecretaryMobNo = row.SecretaryMobNo;
        $scope.getblock($scope.FDRData.DistrictFID);
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

    $scope.GetFertilizerDetailByUser = function () {
        $scope.FertilizerDataList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Mis/GetFertilizerDetailByUser?actionType=GetFertilizerDetail").then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.FertilizerDataList = JSON.parse(response.data.Result.Data.FertilizerDetail);
            } else {
                $scope.FertilizerDataList = [];
            }
        }, function (error) {
            console.error("Error loading data:", error);
            $scope.FertilizerDataList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.ViewCreditRequest = function (item) {
        $scope.FertilizerDataList.forEach(function (x) {
            if (x !== item) {
                x.ShowCreditDetails = false;
            }
        });
        item.ShowCreditDetails = !item.ShowCreditDetails;
        if (!item.ShowCreditDetails) {
            $scope.FertilizerCreditList = [];
            return;
        }

        $scope.FertilizerCreditList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Mis/GetFertilizerDetailByUser?actionType=GetFertilizerCreditDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.FertilizerCreditList = JSON.parse(response.data.Result.Data.FertilizerDetail);
            } else {
                $scope.FertilizerCreditList = [];
            }

        }, function (error) {
            console.error("Error loading data:", error);
            $scope.FertilizerCreditList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.ViewDemandRequest = function (item) {
        $scope.FertilizerDataList.forEach(function (x) {
            if (x !== item) {
                x.ShowDemandDetails = false;
            }
        });

        item.ShowDemandDetails = !item.ShowDemandDetails;
        if (!item.ShowDemandDetails) {
            $scope.FertilizerDemandList = [];
            return;
        }

        $scope.FertilizerDemandList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Mis/GetFertilizerDetailByUser?actionType=GetFertilizerDemendDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.FertilizerDemandList = JSON.parse(response.data.Result.Data.FertilizerDetail);
            } else {
                $scope.FertilizerDemandList = [];
            }

        }, function (error) {
            console.error("Error loading data:", error);
            $scope.FertilizerDemandList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.ViewPurchaseRequest = function (item) {
        $scope.FertilizerDataList.forEach(function (x) {
            if (x !== item) {
                x.ShowPurchaseDetails = false;
            }
        });
        item.ShowPurchaseDetails = !item.ShowPurchaseDetails;
        if (!item.ShowPurchaseDetails) {
            $scope.FertilizerPurchaseList = [];
            return;
        }
        $scope.FertilizerPurchaseList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Mis/GetFertilizerDetailByUser?actionType=GetFertilizerPurchaseDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.FertilizerPurchaseList = JSON.parse(response.data.Result.Data.FertilizerDetail);
            } else {
                $scope.FertilizerPurchaseList = [];
            }
        }, function (error) {
            console.error("Error loading data:", error);
            $scope.FertilizerPurchaseList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.updateFertilizer = function (item) {
        $scope.GetFertilizerDetailById(item).then(function (res) {
            if (res != null && res.length > 0) {
                $scope.FDRData = res[0];
                $scope.FDRData.SocietyID = res[0].SocietyId + '';
                $scope.SocietySearch = res[0].SocietyName;
                $scope.FDRData.IsLicense = res[0].IsLicense == "True" ? true : false;
                $scope.FDRData.LicenseNo = $scope.FDRData.IsLicense == true ? $scope.FDRData.LicenseNo : "";
                $scope.FDRData.IsCreditFacility = $scope.FDRData.IsCreditFacility == "True" ? true : false;
                $scope.FDRData.Month = res[0].DemandMonth + '';
                $scope.FDRData.Year = res[0].DemandYear + '';
                $scope.FDRData.SocietyType = res[0].SocietyType + '';
                $scope.FDRData.IFMSPOSId = res[0].IFMSPOSId;
                $scope.FDRData.DistrictFID = res[0].DistrictID;
                $scope.getblock($scope.FDRData.DistrictFID);
                $scope.FDRData.BlockId = res[0].BlockId.toString();
                $scope.getGrampanchyat($scope.FDRData.BlockId);
                $scope.FDRData.GPId = res[0].GPId.toString();
                $scope.GetDemandRequestData(item).then(function (data) {
                    $scope.DemandList = data;
                    $scope.DemandList.forEach(function (x) {
                        x.FertilizerItemId = x.FertilizerItemId + '';
                        x.FertilizerQuantity = Number(x.FertilizerQuantity);
                        //x.FertilizerRate = Number(x.FertilizerRate);
                        //x.FertilizerCost = Number(x.FertilizerCost);
                        x.OtherFertilizer = x.OtherFertilizerName != null && x.OtherFertilizerName.trim() != "";
                        x.MonthId = x.MonthId + '';
                        x.YearId = x.YearId + '';
                    });
                });
                if ($scope.FDRData.IsCreditFacility == false) {
                    $scope.GetCreditRequestData(item).then(function (data) {
                        $scope.CreditFacilityList = data;
                        $scope.CreditFacilityList.forEach(function (x) {
                            x.FertilizerItemId = x.FertilizerItemId + '';
                            x.FertilizerSale = Number(x.FertilizerSale);
                            x.PresentAvailability = Number(x.PresentAvailability);
                            x.CreditLimit = Number(x.CreditLimit);
                            x.FertilizerQuantity = Number(x.FertilizerQuantity);
                            //x.FertilizerRate = Number(x.FertilizerRate);
                            //x.FertilizerCost = Number(x.FertilizerCost);
                            x.OtherFertilizer = x.OtherFertilizerName != null && x.OtherFertilizerName.trim() != "";
                            x.MonthId = x.MonthId + '';
                            x.YearId = x.YearId + '';
                        });
                    });
                }
                $scope.GetPurchaseData(item).then(function (data) {
                    $scope.PurchaseList = data;
                    $scope.PurchaseList.forEach(function (x) {
                        x.FertilizerItemId = x.FertilizerItemId + '';
                        x.FertilizerQuantity = Number(x.FertilizerQuantity);
                    });
                });
            }
        });

    };
    $scope.GetFertilizerDetailById = function (item) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/GetFertilizerDetail?actionType=GetFertilizerDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null && response.data.Result.Data.FertilizerDetail != null) {
                var data = JSON.parse(response.data.Result.Data.FertilizerDetail);
                return data;
            }
            return [];
        }, function (error) {
            console.error("Error loading data:", error);
            $scope.DemandList = [];
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.GetDemandRequestData = function (item) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/GetFertilizerDetail?actionType=GetFertilizerDemendDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null && response.data.Result.Data.FertilizerDetail != null) {
                var data = JSON.parse(response.data.Result.Data.FertilizerDetail);
                return data;
            }

            $scope.DemandList = [];
            return [];

        }, function (error) {
            console.error("Error loading data:", error);
            $scope.DemandList = [];
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetCreditRequestData = function (item) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/GetFertilizerDetail?actionType=GetFertilizerCreditDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null && response.data.Result.Data.FertilizerDetail != null) {
                var data = JSON.parse(response.data.Result.Data.FertilizerDetail);
                return data;
            }

            $scope.CreditFacilityList = [];
            return [];

        }, function (error) {
            console.error("Error loading data:", error);
            $scope.CreditFacilityList = [];
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetPurchaseData = function (item) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Society/GetFertilizerDetail?actionType=GetFertilizerPurchaseDetail&DemandRequestId=" + item.FDRId).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null && response.data.Result.Data.FertilizerDetail != null) {
                var data = JSON.parse(response.data.Result.Data.FertilizerDetail);
                return data;
            }

            $scope.PurchaseList = [];
            return [];

        }, function (error) {
            console.error("Error loading data:", error);
            $scope.PurchaseList = [];
            return [];
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.isFertilizerLicense = function () {
        if ($scope.FDRData.IsLicense === false) {
            $scope.FDRData.LicenseNo = "";
        }
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
        $scope.FDRData.SocietyID = item.SocietyID.toString();
        $scope.SocietySearch = item.SocietyName;
        $scope.IsSocietyDropdownOpen = false;
        $scope.OnSocietyChange();
    };

    $scope.OnSocietySearchChange = function () {
        $scope.FDRData.DistrictName = "";
        $scope.FDRData.RegistrationNo = "";
        $scope.IsSocietyDropdownOpen = true;
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetSocietyBySSO();
        $scope.GetMonths();
        $scope.GetFinancialYear();
        $scope.GetFertilizerList();
        $scope.GetFertilizerDetailByUser();
    };

    //$scope.getTranslation();
    $scope.OnPageLoad();
});;