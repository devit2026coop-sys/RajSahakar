var AppClasses = angular.module("WRSummaryMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("WRSummaryCnt", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 150,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
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
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
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

        //$scope.DataPaging.searchParameters = [];
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

    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = {};
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetWarehouseList();
    }
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


    $scope.AddNewModel = {
        Id: 0,
        Receipt_No: "",
        OldReceipt_No: "",
        Wharehouse_Id: "0",
        Lisence_No: "EXEMPTED",
        Received_From_Address: "0",
        Item_Kind: "0",
        Quality: "",
        Packages_Lots: "",
        Packages_Quantity: "",
        Packages_Quantity_Unit: "0",
        NetQuantity: "",
        Godown_Id: "0",
        Stack_No: "",
        Valid_From_Date: "",
        Valid_To_Date: "",
        QuantityNote: "",
        DistrictId: "0",
        KVSSId: "0",
        PurchaseCenter_Id: "0",
        NameOfSampler: "",
        ConditionOfGoods: "1",
        IsEditable: false,
        OtherNetQuantity: "",
        PrivateMarkofDepositor: "",
        StorageRateCharge: "",
        IndemnifiedInsurance: "",
        InsuredName: "",
        DoneAmount: "0",
        MarketRate: "0"
    };

    $scope.OTP = "";
    $scope.Certtrn = "";
    $scope.CertSocietyIdEnc = "";
    $scope.CertElectionMasterId = "";
    $scope.CertIndex = "";
    $scope.CertMemberId = "";
    $scope.ListType = "2";

    $scope.GetWarehouseFilterList = function (type) {
        $scope.ListType = type;
        $scope.ClearSearch();
    }
    $scope.RSWCViewList = [];

   
    $scope.GetWarehouseList = function () {

        //$scope.Filter.Agency = "";

        if ($('#hdnRoleId').val() == "10199" || $('#hdnRoleId').val() == "10237") {
            if ($('#hdnRoleId').val() == "10199") {
                $scope.Filter.Agency = 'NAFED';          
            }
            else if ($('#hdnRoleId').val() == "10237") {
                $scope.Filter.Agency = 'NCCF';         
            }

            $("#agency").attr("disabled", "disabled");
            var parameter = { Key: 'Agency', Value: $scope.Filter.Agency }
            $scope.DataPaging.searchParameters.push(parameter);
        }
       

        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetWRSummaryList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.RSWCViewList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.RSWCViewList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetWarehouseList();


    $scope.ResetAddNewModel = function () {
        $scope.AddNewModel = {
            Id: 0,
            Receipt_No: $scope.AddNewModel.Receipt_No,
            OldReceipt_No: $scope.AddNewModel.OldReceipt_No,
            Wharehouse_Id: $scope.AddNewModel.Wharehouse_Id,
            Lisence_No: $scope.AddNewModel.Lisence_No,
            Received_From_Address: $scope.AddNewModel.Received_From_Address,
            Item_Kind: $scope.AddNewModel.Item_Kind,
            Quality: "",
            Packages_Lots: "",
            Packages_Quantity: "",
            Packages_Quantity_Unit: "0",
            Godown_Id: $scope.GodownList[0].GoDownId,
            Stack_No: "",
            Valid_From_Date: $scope.AddNewModel.Valid_From_Date,
            Valid_To_Date: $scope.AddNewModel.Valid_To_Date,
            DistrictId: $scope.AddNewModel.DistrictId,
            KVSSId: $scope.AddNewModel.KVSSId,
            PurchaseCenter_Id: $scope.AddNewModel.PurchaseCenter_Id,
            NameOfSampler: "",
            ConditionOfGoods: "1",
            IsEditable: false,
            OtherNetQuantity: "",
            PrivateMarkofDepositor: $scope.AddNewModel.PrivateMarkofDepositor,
            StorageRateCharge: $scope.AddNewModel.StorageRateCharge,
            IndemnifiedInsurance: $scope.AddNewModel.IndemnifiedInsurance,
            InsuredName: $scope.AddNewModel.InsuredName,
            DoneAmount: "0",
            MarketRate: "0"
        };
    }
    $scope.RSWCList = [];
    $scope.RSWCList.push($scope.AddNewModel);

    $scope.UpdateType = "";
    $scope.UpdateRSWC = function () {

        if ($scope.ValidateData()) {
            $scope.showLoader = true;
            // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
            var val = $("#Valid_From_Date").val().split('/')[2] + "/" + $("#Valid_From_Date").val().split('/')[1] + "/" + $("#Valid_From_Date").val().split('/')[0]
            $(".se-pre-con").fadeIn("slow");
            var formData = new FormData();
            $http.post(SiteUrl + "/FertilizerSupply/UpdateRSWC", { ReceiptNo: $scope.AddNewModel.Receipt_No, Value: val, Type: $scope.UpdateType }).
                success(function (data, status) {

                    $(".se-pre-con").fadeOut("slow");

                    if (data.status == "1") {
                        ShowOkMessage("", _language == 'en-us' ? 'successfully updated' : "आदेश सफलतापूर्वक जारी हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/Warehouseloanlist';
                        });
                    } else {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });

                    }




                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    //return false;
                });
        }

    };



    $scope.AddNew = function (item) {

        $scope.AddNewModel = {
            Id: 0,
            Receipt_No: item.receipt_no,
            Valid_From_Date: ""
        };
        if (item.Loan_start != '') {
            $scope.UpdateType = "loan_close_date";
            $scope.AddNewModel.Valid_To_Date = item.Loan_start;
            $("#Valid_To_Date").val(item.Loan_start);
            $(".Fromdate").datepicker("option", "minDate", item.Loan_start);
        } else {
            $scope.UpdateType = "loan_start_date";
            $(".Fromdate").datepicker("option", "minDate", item.from_date);
        }



        $("#addModal").modal('show');
    }

    $scope.AddNewItem = function () {
        $scope.ResetAddNewModel();
        $scope.RSWCList.push($scope.AddNewModel);
    }

    $scope.ValidateData = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");
        $(".validateNewAdd").removeClass("notValid");
        if ($("#Valid_From_Date").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  date' : '\n <br /> कृपया दिनांक चुनें |';
            $("#Valid_From_Date").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }
    $scope.showPriView = false;
    $scope.GenerateRequest = function () {
        $scope.showPriView = true;
    }
    $scope.GenerateRequestBack = function () {
        $scope.showPriView = false;
    }

    $scope.OrderList = [];
    $scope.SelectDemand = function (th) {
        if (th.IsChecked) {
            $scope.OrderList.push(th);
        } else {
            var index = $scope.OrderList.indexOf(th);
            $scope.OrderList.splice(index, 1);
        }
    }
    $scope.otpModel = {
        IsChecked: false
    }
    $scope.SendOTP = function () {

        if ($scope.ValidateData()) {


            if (!$scope.otpModel.IsChecked) {
                ShowOkMessage("", _language == 'en-us' ? 'Please allow system for your AADHAAR based Authentication/ eKYC.\n' : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।|\n', function () {
                });
                return;
            }


            $(".se-pre-con").fadeIn("slow");
            $.ajax({
                url: '../FertilizerSupply/GenerateAadhaarOTPForEsing',
                //data: JSON.stringify({
                //    "objElectionSocietyData": ArrDates,
                //    "ElectionMasterIdEnc": $('#btnautoclick').val(),
                //}),
                type: 'GET',
                contentType: 'application/json',
                // dataType: 'json',
                beforeSend: function () {
                },
                success: function (data) {

                    if (data.indexOf('NO#') >= 0) {
                        ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    } else {
                        if (data != "LOGOUT") {
                            $scope.Certtrn = data;
                            $('#myOTPModal').modal("show");
                            ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                                $scope.OTP = "";
                                //$('#myOTPModal').modal("show");
                            });
                        }
                        else if (data == "LOGOUT") {
                            ShowOkMessage("", 'Session out.\n', function () {
                                window.location = 'https://sso.rajasthan.gov.in/signin';
                            });

                        }
                        // window.location = '../Election/ElectionSocietyList';
                        $(".se-pre-con").fadeOut("slow");
                    }
                },
                error: function (e) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    ShowOkMessage("", message, function () {
                    });
                    $(".se-pre-con").fadeOut("slow");
                },
                complete: function () {
                    $(".se-pre-con").fadeOut("slow");
                }
            });
        }
    }


    $scope.VerifyOTP = function () {

        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");


            var formData = new FormData();

            var reqestData = [];
            $scope.OrderList.forEach(function (item, index) {
                reqestData.push({
                    Id: item.Id,
                    Receipt_No: item.receipt_no
                });
            });

            //formData.append("ReqestData", ReqestData);
            //formData.append("OTP", $scope.OTP);
            //formData.append("TXN", $scope.Certtrn);
            var functionname = "InsertWRSummary";//_language == 'en-us' ? "GenerateWarehouseRSWCEng" : "GenerateWarehouseRSWCHi";//"GenerateWarehouseRSWCHi";//
            $http.post(SiteUrl + "/FertilizerSupply/" + functionname, { OTP: $scope.OTP, TXN: $scope.Certtrn, DetailsList: reqestData }).
                success(function (data, status) {

                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'Order successfully generated' : "आदेश सफलतापूर्वक जारी हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/WRSummary';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "PDF Not Generate") {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    }

                    else if (data == "LOGOUT") {
                        ShowOkMessage("", 'Session out.\n', function () {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        });
                    } else {
                        ShowOkMessage("", data, function () {

                        });
                    }



                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    //return false;
                });
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter OTP." : "कृपया OTP दर्ज करें।", function () { return false; });
        }
        $scope.showLoader = false;
    };

    $scope.SelectAll = function () {
        if ($("#chk_all").is(":checked")) {
            $scope.RSWCViewList.forEach(function (itm, index) {
                itm.IsChecked = true;
                $scope.OrderList.push(itm);
            });
        } else {
            $scope.RSWCViewList.forEach(function (itm, index) {
                itm.IsChecked = false;
            });
            $scope.OrderList = [];
        }
    }
    $scope.WarehouseList = [];

    $scope.WarehouseList = function () {
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": "0" }).
            success(function (data, status) {
                $scope.WarehouseList = data;
            }).
            error(function () {
                $scope.Warehouses = [];
            });
    };

    $scope.WarehouseList();

    $scope.ROList = [];
    $scope.GetRAJFEDRO = function () {
        $scope.ROList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RAJFEDRO&MasterCode=0").
            success(function (data, status) {
                $scope.ROList = data;
            }).
            error(function (data, status) {
                // alert("Error while fetching Post list")

            });
    }
    $scope.GetRAJFEDRO();

    $scope.ItemList = [];
    // $scope.ItemList = [{ ItemId: "1", ItemNameEn: "Moong", ItemNameHi: "मूंग" }];
    $scope.GetItemList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMMODITY&MasterCode=0").
            success(function (data, status) {
                $scope.ItemList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
    }
    $scope.GetItemList();

    $scope.ExportWRSummaryReportData = function () {
        $(".blockUI").fadeIn("slow");
        $scope.ReportExportList = [];


        var filterType = "";

        //if ($scope.Filter.FinancialYear != '' && $scope.Filter.FinancialYear != undefined) {
        //    filterType = $scope.translation.FincialYearHeading + "#" + $scope.Filter.FinancialYear;
        //}

        //if ($scope.Filter.WarehouseId != '' && Filter.WrSearch.WarehouseId != undefined) {
        //    filterType += (filterType != '' ? "," : "") + $scope.translation.Warehouse + "#" + $scope.GetNameFromList($scope.WarehouseList, $scope.WrSearch.WarehouseId, 'Warehous');
        //}
        if ($scope.Filter.CommodityId != '' && $scope.Filter.CommodityId != undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.Commodity + "#" + $scope.GetNameFromList($scope.Commodities, $scope.WrSearch.CommodityId, 'Commodity');
        }

        //if ($scope.Filter.RegionalOffice != '' && $scope.Filter.RegionalOffice != undefined) {
        //    filterType += (filterType != '' ? "," : "") + $scope.translation.RegionalOffice + "#" + $scope.GetNameFromList($scope.Commodities, $scope.WrSearch.CommodityId, 'RegionalOffice');
        //}
        if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
            filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtFromDate").val();
            filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtToDate").val();
        } else if ($("#txtFromDate").val() != "") {
            filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtFromDate").val();
            filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtFromDate").val();
        } else if ($("#txtToDate").val() != "") {
            filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtToDate").val();
            filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtToDate").val();
        }






        var functionname = _language == 'en-us' ? "GetWRSummaryReportExportEn" : "GetWRSummaryReportExportEn";//"GenerateWarehouseRSWCHi";//
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
        return $http.post(SiteUrl + "/MIS/" + functionname, JSON.stringify({ Details: jsonParameters, ReportHead: filterType, ReportType: $scope.ReportType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var bytes = $scope.base64ToArrayBuffer(data);
                var blob = new Blob([bytes], { type: "application/pdf" });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                var fileName = "WRSummaryReport_" + $scope.ReportType;
                link.download = fileName;
                link.click();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });

    }


    $scope.ExportWRSummaryDetailsReportData = function () {
        $(".blockUI").fadeIn("slow");
        $scope.ReportExportList = [];


        var filterType = "Summary No.#" + $scope.SummaryNo;

        //if ($scope.Filter.FinancialYear != '' && $scope.Filter.FinancialYear != undefined) {
        //    filterType += (filterType != '' ? "," : "") +$scope.translation.FincialYearHeading + "#" + $scope.Filter.FinancialYear;
        //}

        //if ($scope.Filter.WarehouseId != '' && Filter.WrSearch.WarehouseId != undefined) {
        //    filterType += (filterType != '' ? "," : "") + $scope.translation.Warehouse + "#" + $scope.GetNameFromList($scope.WarehouseList, $scope.WrSearch.WarehouseId, 'Warehous');
        //}
        //if ($scope.Filter.CommodityId != '' && $scope.Filter.CommodityId != undefined) {
        //    filterType += (filterType != '' ? "," : "") + $scope.translation.Commodity + "#" + $scope.GetNameFromList($scope.Commodities, $scope.WrSearch.CommodityId, 'Commodity');
        //}

        //if ($scope.Filter.RegionalOffice != '' && $scope.Filter.RegionalOffice != undefined) {
        //    filterType += (filterType != '' ? "," : "") + $scope.translation.RegionalOffice + "#" + $scope.GetNameFromList($scope.Commodities, $scope.WrSearch.CommodityId, 'RegionalOffice');
        //}






        var functionname = _language == 'en-us' ? "GetWRSummaryDetailsReportExportEn" : "GetWRSummaryDetailsReportExportEn";//"GenerateWarehouseRSWCHi";//
        //var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
        return $http.post(SiteUrl + "/MIS/" + functionname, JSON.stringify({ RequestNo: $scope.SummaryNo, ReportHead: filterType, ReportType: $scope.ReportType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var bytes = $scope.base64ToArrayBuffer(data);
                var blob = new Blob([bytes], { type: "application/pdf" });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                var fileName = "WRSummaryReport_" + $scope.ReportType;
                link.download = fileName;
                link.click();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });

    }


    $scope.base64ToArrayBuffer = function (base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }


    $scope.SummaryNo = "";
    $scope.SummaryList = [];
    $scope.ShowSRSummary = function (th) {

        $scope.SummaryNo = th.request_no;
        $("#SummaryModal").modal("show");
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/MIS/GetWRSummaryDetailsList?RequestNo=" + th.request_no).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SummaryList = JSON.parse(data.DataList);
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.RSWCViewList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    }

});