var AppClasses = angular.module("DailyWheatPurchaseMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("DailyWheatPurchaseCnt", function ($scope, $http, $rootScope) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        //var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        //var indx = 1;
        //$('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

        //    if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
        //        if ($(this).is(":checked")) {
        //            var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
        //            $scope.DataPaging.searchParameters.push(parameter);
        //        }
        //    } else {
        //        if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
        //            var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
        //            $scope.DataPaging.searchParameters.push(parameter);
        //        }
        //    }
        //    if (indx == ln) {
        //        //  _currentPageID = 1;
        //        callback();
        //    }
        //   // ++indx;
        //});

        callback();
        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
    }



    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {

        
        $scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    //$scope.showSearchDate = false;
    //$scope.showSearchText = true;
    //$scope.SearchText = "";
    //$scope.Filter = [];
    //$scope.FilterTypeChange = function () {
    //    $scope.Filter.SearchText = "";
    //    $scope.Filter.FromDate = "";
    //    $scope.Filter.ToDate = "";
    //    if ($scope.Filter.FilterType == 'created_dated') {
    //        $scope.showSearchDate = true;
    //        $scope.showSearchText = false;
    //        // $scope.AddCalender();
    //    } else {
    //        $scope.showSearchDate = false;
    //        $scope.showSearchText = true;
    //    }
    //}
    //$scope.ClearSearch = function () {
    //    $scope.Filter.SearchText = "";
    //    $scope.Filter.FromDate = "";
    //    $scope.Filter.ToDate = "";
    //    $scope.Filter.FilterType = "";
    //    $scope.showSearchDate = false;
    //    $scope.showSearchText = true;
    //    $scope.DataPaging.currentPageID = 1;
    //    $scope.DataPaging.searchParameters = [];
    //    $scope.GetWarehouseList();
    //}
    /*------------End Paging and Filter-----------------*/

    $scope._language = 'en-us';
    $scope._language = _language;
    if (_language == 'en-us') {
        $http.get("../Translations/Translation_EN.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
    }
    else {
        $http.get("../Translations/Translation_HI.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }

    $scope.DailyPurchase = {
        DailyPurchaseId: 0,
        CommodityId: '0',
        DISTRICT_ID: '0',
        PurchaseCenter_Id: '0',
        QuantityNote: '',
        PaymentReleased: '',
        PendingPayment: '',
        QtySent: '',
        QtyLying: '',
        GunnyBags: '',
        GunnyBagsStock: '',
        NoofFarmersBenefite: '',
        WCMRecdtilldate:''

    };


    $scope.AddNewModel1 = {
        DailyPurchaseQualityId: 0,
        QualityID: "0",
        Quantity: "",
        NoofBags: "",
        QualityPrice: "",
        TotalPrice: ""

    };
    $scope.AddNew = function () {

        $scope.DWPList = [];
        $scope.DWPList.push($scope.AddNewModel1);
        $("#Valid_From_Date").val($("#fromdateserver").val());
        $("#addModal").modal('show');
    }

    $scope.validateAddNewItem = function () {
        
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';



        $scope.DWPList.forEach(function (item, index) {

            if (item.QualityID == '' || item.QualityID == undefined || item.QualityID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  Quality Product' : '\n <br /> कृपया गुणवत्ता उत्पाद चुनें |';
                $("#QualityID_" + index).addClass("notValid");
            }
            else {
                $("#QualityID_" + index).removeClass("notValid");
            }

            if (item.Quantity == '' || item.Quantity == undefined || item.Quantity == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Quantity (Quintal)' : '\n <br />कृपया मात्रा दर्ज करें (क्विंटल में) |';
                $("#Quantity_" + index).addClass("notValid");
            } else {
                $("#Quantity_" + index).removeClass("notValid");
            }

            if (item.NoofBags == '' || item.NoofBags == undefined || item.NoofBags == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Number of Bags' : '\n <br /> कृपया बैग की संख्या दर्ज करें |';
                $("#NoofBags_" + index).addClass("notValid");
            } else {
                $("#NoofBags_" + index).removeClass("notValid");
            }

        });
        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;

    }

    $scope.ResetAddNewModel = function () {

        $scope.AddNewModel1 = {
            DailyPurchaseQualityId:0,
            QualityID: "0",
            Quantity: "",
            NoofBags: "",
            QualityPrice: "",
            TotalPrice: ""
        };
    }
    $scope.AddNewItem = function () {

        if ($scope.validateAddNewItem()) {
            $scope.ResetAddNewModel();
            $scope.DWPList.push($scope.AddNewModel1);
        }
    }



    $scope.SaveData = function () {

        
        var formData = new FormData();

        var DWPList1 = [];
        $scope.DWPList.forEach(function (itm) {
            DWPList1.push({
                DailyPurchaseQualityId: itm.DailyPurchaseQualityId,
                QualityID: itm.QualityID,
                Quantity: itm.Quantity,
                NoofBags: itm.NoofBags,
                QualityPrice: itm.QualityPrice,
                TotalQualityPrice: itm.TotalPrice
            });
        });


        var Valid_From_Date = $("#Valid_From_Date").val();
        var applicationDetails = {
            DailyPurchaseId: $scope.DailyPurchase.DailyPurchaseId == undefined ? 0 : $scope.DailyPurchase.DailyPurchaseId,
            CommodityId: $scope.DailyPurchase.CommodityId,
            DISTRICT_ID: $scope.DailyPurchase.DISTRICT_ID,
            PurchaseCenter_Id: $scope.DailyPurchase.PurchaseCenter_Id,
            Valid_From_Date: Valid_From_Date,
            QuantityNote: $scope.DailyPurchase.QuantityNote,
            PaymentReleased: $scope.DailyPurchase.PaymentReleased,
            PendingPayment: $scope.DailyPurchase.PendingPayment,
            QtySent: $scope.DailyPurchase.QtySent,
            QtyLying: $scope.DailyPurchase.QtyLying,
            GunnyBags: $scope.DailyPurchase.GunnyBags,
            GunnyBagsStock: $scope.DailyPurchase.GunnyBagsStock,
            NoofFarmersBenefite: $scope.DailyPurchase.NoofFarmersBenefite,
            WCMRecdtilldate: $scope.DailyPurchase.WCMRecdtilldate,
            QualityPricePurchase: DWPList1,
            
        }


        formData.append("Modeldata", JSON.stringify(applicationDetails));
        formData.append("QualityPricePurchase", JSON.stringify(DWPList1));


        $http.post(SiteUrl + "/Stock/SaveDailyWheatPurchase", JSON.stringify({ Modeldata: applicationDetails })).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            
            if (data.Result != "-1" && data.Result != "0") {
                ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                   // window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintCourtModule";
                    $scope.GetDailyWheatPurchase();
                    $("#addModal").modal('hide');
                });
            } else {
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                });
            }
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                });
            });




    }

    $scope.SaveDataValidate = function () {
        if ($scope.ValidateForm()) {
            $scope.SaveData()
        }
    }

    $scope.ValidateForm  = function ()
    {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.DailyPurchase.CommodityId == '' || $scope.DailyPurchase.CommodityId == undefined || $scope.DailyPurchase.CommodityId == '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select  Commodity' : '\n <br />कृपया कमोडिटी का चयन करें |';
            
            $("#ddlCommodityId").addClass("notValid");
        } else {
            $("#ddlCommodityId").removeClass("notValid");
        }
        if ($scope.DailyPurchase.DISTRICT_ID == '' || $scope.DailyPurchase.DISTRICT_ID == undefined || $scope.DailyPurchase.DISTRICT_ID == '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br />    कृपया जिला चुनें |';   
            $("#dllDistrictId").addClass("notValid");
        } else {
            $("#dllDistrictId").removeClass("notValid");
        }
        if ($scope.DailyPurchase.PurchaseCenter_Id == '' || $scope.DailyPurchase.PurchaseCenter_Id == undefined || $scope.DailyPurchase.PurchaseCenter_Id == '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select  Purchase Center.' : '\n <br /> कृपया क्रय केंद्र चुनें |';   
            $("#dllPurchaseCenter_Id").addClass("notValid");
        } else {
            $("#dllPurchaseCenter_Id").removeClass("notValid");
        }

        if ($("#Valid_From_Date").val() == '') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Valid Date .' : '\n <br /> कृपया मान्य दिनांक दर्ज करें |';   
            $("#Valid_From_Date").addClass("notValid");
        } else {
            $("#Valid_From_Date").removeClass("notValid");
        }
        if ($scope.DailyPurchase.QuantityNote == '' || $scope.DailyPurchase.QuantityNote == undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Quantity Note .' : '\n <br /> कृपया मात्रा नोट दर्ज करें |';   
            $("#QuantityNote").addClass("notValid");
        } else {
            $("#QuantityNote").removeClass("notValid");
        }
        if ($scope.DailyPurchase.PaymentReleased == '' || $scope.DailyPurchase.PaymentReleased == undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Payment Released Rs.In (Lacs).' : '\n <br /> कृपया भुगतान जारी करें दर्ज करें। (लाख) |';   
            $("#PaymentReleased").addClass("notValid");
        } else {
            $("#PaymentReleased").removeClass("notValid");
        }
        if ($scope.DailyPurchase.PendingPayment == '' || $scope.DailyPurchase.PendingPayment == undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Pending Payment Rs. In (Lacs).' : '\n <br /> कृपया लंबित भुगतान दर्ज करें रु। इन(लाख) |';   
            $("#PendingPayment").addClass("notValid");
        } else {
            $("#PendingPayment").removeClass("notValid");
        }


        if ($scope.DailyPurchase.QtySent == '' || $scope.DailyPurchase.QtySent == undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> please enter Qty.' : '\n <br /> कृपया Qty दर्ज करें। |';   
            $("#QtySent").addClass("notValid");
        } else {
            $("#QtySent").removeClass("notValid");
        }
        if ($scope.DailyPurchase.QtyLying == '' || $scope.DailyPurchase.QtyLying == undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> please enter Qty. Lying for Storage Bags' : '\n <br /> कृपया Qty दर्ज करें भंडारण बैग के लिए। । |';   
            $("#QtyLying").addClass("notValid");
        } else {
            $("#QtyLying").removeClass("notValid");
        }

        if ($scope.DailyPurchase.GunnyBags == '' || $scope.DailyPurchase.GunnyBags == undefined) {
            message += $scope._language === 'en-us' ? '\n <br />please enter Gunny Bags Sent up to Date Bags' : '\n <br />कृपया दिनांक बैग तक भेजे गए गन्नी बैग दर्ज करें । |';   
            $("#GunnyBags").addClass("notValid");
        } else {
            $("#GunnyBags").removeClass("notValid");
        }
        if ($scope.DailyPurchase.GunnyBagsStock == '' || $scope.DailyPurchase.GunnyBagsStock == undefined) {
            message += $scope._language === 'en-us' ? '\n <br />please enter Gunny Bags Stock on Day Bags' : '\n <br />  कृपया डे बैग पर गनी बैग स्टॉक दर्ज करें । |';   
            $("#GunnyBagsStock").addClass("notValid");
        } else {
            $("#GunnyBagsStock").removeClass("notValid");
        }

        if ($scope.DailyPurchase.NoofFarmersBenefite == '' || $scope.DailyPurchase.NoofFarmersBenefite == undefined) {
            message += $scope._language === 'en-us' ? '\n <br />please enter Number of Farmers Benefite' : '\n <br />कृपया कृषक लाभ की संख्या दर्ज करें। |';   
            
            $("#NoofFarmersBenefite").addClass("notValid");
        } else {
            $("#NoofFarmersBenefite").removeClass("notValid");
        }
        if ($scope.DailyPurchase.WCMRecdtilldate == '' || $scope.DailyPurchase.WCMRecdtilldate == undefined) {
            message += $scope._language === 'en-us' ? '\n <br />please enter WCM Recd till date bags' : '\n <br />कृपया आज तक के बैग में WCM Recd डालें। |';     
            $("#WCMRecdtilldate").addClass("notValid");
        } else {
            $("#WCMRecdtilldate").removeClass("notValid");
        }
        $scope.DWPList.forEach(function (item, index) {

            if (item.QualityID == '' || item.QualityID == undefined || item.QualityID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  Quality Product' : '\n <br /> कृपया गुणवत्ता उत्पाद चुनें |';
                $("#QualityID_" + index).addClass("notValid");
            }
            else {
                $("#QualityID_" + index).removeClass("notValid");
            }
            if (item.Quantity == '' || item.Quantity == undefined || item.Quantity == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Quantity (Quintal)' : '\n <br />कृपया मात्रा दर्ज करें (क्विंटल में) |';
                $("#Quantity_" + index).addClass("notValid");
            } else {
                $("#Quantity_" + index).removeClass("notValid");
            }
            if (item.NoofBags == '' || item.NoofBags == undefined || item.NoofBags == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Number of Bags' : '\n <br /> कृपया बैग की संख्या दर्ज करें |';
                $("#NoofBags_" + index).addClass("notValid");
            } else {
                $("#NoofBags_" + index).removeClass("notValid");
            }
        });
        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }
    $scope.DailyWheatPurchaseList = [];

    $scope.GetDailyWheatPurchase = function () {
        
        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetDailyWheatPurchase", JSON.stringify({ Details: jsonParameters})).
            success(function (data, status) {
                $scope.DailyWheatPurchaseList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DailyWheatPurchaseList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetPurchaseCenterList = function () {
       
       // $http.get(SiteUrl + "/Stock/GetPurchaseCenter", JSON.stringify({ Details: '', DISTRICT_ID: 0 })).
        $scope.PurchaseCenterList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetPurchaseCenter", JSON.stringify({ Details: jsonParameters, DISTRICT_ID: $scope.DailyPurchase.DISTRICT_ID })).
            success(function (data, status) {
                $scope.PurchaseCenterList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
            }).
            error(function (data, status) {
                $scope.PurchaseCenterList = [];
                alert('Error while fetching district list data.');
            });
    }

   
    $scope.GetCommodityPrice = function (CommodityId) {

       // alert(CommodityId)
        
        $(".blockUI").fadeIn("slow");
        $scope.CommodityPriceList1 = [];

        return $http.post(SiteUrl + "/Stock/GetCommodityPriceList", { CommodityId: CommodityId }).
            success(function (data, status) {
                $scope.CommodityPriceList1 = JSON.parse(data.DataList);
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CommodityPriceList1 = [];
                $(".blockUI").fadeOut("slow");
            });
    }

    $scope.GetCommodityList = function () {
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetCommodityMaster", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $scope.CommodityList = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $scope.CommodityrList = [];
                alert('Error while fetching district list data.');
            });
    }



    $scope.SetNetQuantity1 = function (index, unitid) {

        $scope.CommodityPriceList1.forEach(function (item, indx) {
                if (item.UnitId == unitid) {
                    $scope.CommodityPriceList1.filter(function (x) { return x.UnitId === parseInt(unitid) }).forEach(function (item1, index1) {
                        $scope.DWPList.forEach(function (item2, indx) {
                            if (index == indx) {
                                item2.QualityPrice = item1.Price;
                               $scope.itemPrice = item1.Price;
                            }
                        });
                    });
                  }
            });
    }



    $scope.SetNetQuantity = function (index) {
        
        $scope.totalPrice = 0;
        $scope.totalLots = 0;
        $scope.totalNetQty = 0;


        $scope.DWPList.forEach(function (item, indx) {
           
            item.NetQuantity = item.QualityPrice * item.Quantity;
            item.TotalPrice = item.NetQuantity
             //$scope.TotalPrice += item.NetQuantity;
          

        });

    }
    $scope.GetDailyWheatPurchase();
    $scope.GetCommodityList();
    $scope.GetDistrictList();
});