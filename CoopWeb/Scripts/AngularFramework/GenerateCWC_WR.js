var AppClasses = angular.module("WarehouseMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("WarehouseCnt", function ($scope, $http, $rootScope) {
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
    $scope.Filter = [];
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

    $scope.messagebox = false;
    $scope.OtpDilogBox = false;
    $scope.FillOtp = false;

    $scope.ShowOtpDilogBox = function () {
        $("#myOtpDilogBox").modal("show");
        $('#SSOAuth_OTP').hide();

        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
    }
    $scope.HideCheckAllowMessage = function () {
        $scope.messagebox = false;
        $scope.FillOtp = false;
        $('#SSOAuth_OTP').hide();
        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
    }
    $scope.CloseOtpBox = function () {
        $scope.messagebox = false;
        $scope.OtpDilogBox = false;
        $scope.FillOtp = false;
        $('#SSOAuth_OTP').hide();
        $("#myOtpDilogBox").modal("hide");
        $('#ssoauthloading').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
    };



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
        StorageRateCharge: "0",
        IndemnifiedInsurance: "",
        InsuredName: "",
        DoneAmount: "0",
        MarketRate: "0",
        IndemnifiedInsuranceType: "0",
        Moisture: "",
        IsHeighCharges: false
    };

    $scope.OTP = "";
    $scope.Certtrn = "";
    $scope.CertSocietyIdEnc = "";
    $scope.CertElectionMasterId = "";
    $scope.CertIndex = "";
    $scope.CertMemberId = "";
    $scope.ListType = "1";

    $scope.GetWarehouseFilterList = function (type) {
        $scope.ListType = type;
        $scope.ClearSearch();
    }
    $scope.RSWCViewList = [];
    $scope.GetWarehouseList = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/FertilizerSupply/GetWarehouseCWCList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType })).
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
            MarketRate: "0",
            IndemnifiedInsuranceType: "0",
            Moisture: "",
            IsHeighCharges: $scope.AddNewModel.IsHeighCharges

        };
    }
    $scope.RSWCList = [];
    $scope.RSWCList.push($scope.AddNewModel);


    $scope.GenerateRequest = function () {

        if ($('#SSOAuth_chkConsent').prop("checked")) {
            if ($scope.ValidateData()) {
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
                        ;
                        if (data.indexOf('NO#') >= 0) {
                            ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                            // window.location = '../Election/ElectionSocietyList';
                            //alert(data[0].txnid.split('#')[1]);
                            $('#ssoauthloading').hide();
                            $('#SSOAuth_OTP').hide();
                        } else {
                            if (data != "LOGOUT") {
                                $scope.Certtrn = data;
                                //$('#myOTPModal').modal("show");

                                $scope.messagebox = false;

                                $('#SSOAuth_OTP').show();
                                $('#ssoauthloading').hide();
                                $('#btnSSOAuth_SendOTP').hide();
                                $('#btnSSOAuth_ValidateOTP').show();



                                ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                                    $scope.OTP = "";
                                    //$('#myOTPModal').modal("show");
                                });
                            }
                            else if (data == "LOGOUT") {

                                alert('Session out.\n');
                                window.location = 'https://sso.rajasthan.gov.in/signin';


                            }
                            // window.location = '../Election/ElectionSocietyList';
                            $(".se-pre-con").fadeOut("slow");
                        }
                    },
                    error: function (e) {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        alert(message)
                        $(".se-pre-con").fadeOut("slow");
                    },
                    complete: function () {
                        $(".se-pre-con").fadeOut("slow");
                    }
                });
            }
        } else {
            $scope.messagebox = true;
            $scope.FillOtp = false;
        }



    }


    $scope.SaveData = function () {
        ;
        if ($scope.ValidateData()) {
            $scope.showLoader = true;
            // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);

            $(".se-pre-con").fadeIn("slow");


            var formData = new FormData();

            var reqestData = [];
            $scope.RSWCList.forEach(function (item, index) {
                reqestData.push({
                    Id: item.Id,
                    Receipt_No: '',
                    OldReceipt_No: $scope.AddNewModel.OldReceipt_No,
                    Wharehouse_Id: $scope.AddNewModel.Wharehouse_Id,
                    Lisence_No: $scope.AddNewModel.Lisence_No,
                    Received_From_District: $scope.AddNewModel.DistrictId,
                    Received_From_Address: $scope.AddNewModel.Received_From_Address,
                    Item_Kind: $scope.AddNewModel.Item_Kind,
                    Quality: item.Quality,
                    Packages_Lots: item.Packages_Lots,
                    Packages_Quantity: item.Packages_Quantity,
                    Packages_Quantity_Unit: item.Packages_Quantity_Unit,
                    Godown_Id: item.Godown_Id,
                    Stack_No: item.Stack_No,
                    Valid_From_Date: $("#Valid_From_Date").val(),
                    Valid_To_Date: $("#Valid_To_Date").val(),
                    NetQuantity: item.OtherNetQuantity,
                    QuantityNote: $scope.AddNewModel.QuantityNote,
                    PurchaseCenterId: $scope.AddNewModel.PurchaseCenter_Id,
                    ReceivedFromKVSS: $scope.AddNewModel.KVSSId,
                    SamplerName: item.NameOfSampler,
                    ConditionOfGoods: $scope.AddNewModel.ConditionOfGoods,
                    PrivateMarkofDepositor: $scope.AddNewModel.PrivateMarkofDepositor,
                    StorageRateCharge: $scope.AddNewModel.StorageRateCharge,
                    IndemnifiedInsurance: $scope.AddNewModel.IndemnifiedInsuranceType == 'Self' ? $scope.AddNewModel.IndemnifiedInsuranceType : $scope.AddNewModel.IndemnifiedInsurance,
                    InsuredName: $scope.AddNewModel.InsuredName,
                    IsSubmit: false,
                    AutoCalculateQuantity: item.NetQuantity,
                    Moisture: $scope.AddNewModel.Moisture,
                    IsCWR: true,
                    DateOfIssue: $("#DateOfIssue").val(),
                    Crop_year: $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'Crop_year')

                });
            });

            //formData.append("ReqestData", ReqestData);
            //formData.append("OTP", $scope.OTP);
            //formData.append("TXN", $scope.Certtrn);            
            var functionname = "GenerateWarehouseCWCEng";//_language == 'en-us' ? "GenerateWarehouseRSWCEng" : "GenerateWarehouseRSWCHi";//"GenerateWarehouseRSWCHi";//
            $http.post(SiteUrl + "/FertilizerSupply/" + functionname, { OTP: $scope.OTP, TXN: $scope.Certtrn, DetailsList: reqestData }).
                success(function (data, status) {
                    ;
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'WR successfully saved' : "डब्ल्यूआर सफलतापूर्वक सहेजा गया", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/GenerateCWC_WR';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "-1") {
                        ShowOkMessage("", _language == 'en-us' ? "WR already ganareted with physical receipt no '" + $scope.AddNewModel.OldReceipt_No + "'." : "डब्ल्यूआर पहले से ही भौतिक रसीद संख्या '" + $scope.AddNewModel.OldReceipt_No + "' के साथ तैयार है।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    } else if (data == "0") {
                        ShowOkMessage("", _language == 'en-us' ? "WR could not be generated. Please try after some time." : "डब्ल्यूआर जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    }

                    else if (data == "LOGOUT") {
                        ShowOkMessage("", 'Session out.\n', function () {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        });
                    } else {
                        ShowOkMessage("", _language == 'en-us' ? "WR could not be saved. Please try after some time." : "डब्ल्यूआर जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });

                    }



                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    //return false;
                });

            $scope.showLoader = false;
        }
    };



    $scope.VerifyOTP = function (isSubmit) {
        $scope.OTP = "12345";/*Set default for Not Esign*/
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");


            var formData = new FormData();

            var reqestData = [];
            $scope.RSWCList.forEach(function (item, index) {
                reqestData.push({
                    Id: item.Id,
                    Receipt_No: '',
                    OldReceipt_No: $scope.AddNewModel.OldReceipt_No,
                    Wharehouse_Id: $scope.AddNewModel.Wharehouse_Id,
                    Lisence_No: $scope.AddNewModel.Lisence_No,
                    Received_From_District: $scope.AddNewModel.DistrictId,
                    Received_From_Address: $scope.AddNewModel.Received_From_Address,
                    Item_Kind: $scope.AddNewModel.Item_Kind,
                    Quality: item.Quality,
                    Packages_Lots: item.Packages_Lots,
                    Packages_Quantity: item.Packages_Quantity,
                    Packages_Quantity_Unit: item.Packages_Quantity_Unit,
                    Godown_Id: item.Godown_Id,
                    Stack_No: item.Stack_No,
                    Valid_From_Date: $("#Valid_From_Date").val(),
                    Valid_To_Date: $("#Valid_To_Date").val(),
                    NetQuantity: item.OtherNetQuantity,
                    QuantityNote: $scope.AddNewModel.QuantityNote,
                    PurchaseCenterId: item.PurchaseCenter_Id,
                    ReceivedFromKVSS: $scope.AddNewModel.KVSSId,
                    SamplerName: item.NameOfSampler,
                    ConditionOfGoods: $scope.AddNewModel.ConditionOfGoods,
                    PrivateMarkofDepositor: $scope.AddNewModel.PrivateMarkofDepositor,
                    StorageRateCharge: $scope.AddNewModel.StorageRateCharge,
                    IndemnifiedInsurance: $scope.AddNewModel.IndemnifiedInsuranceType == 'Self' ? $scope.AddNewModel.IndemnifiedInsuranceType : $scope.AddNewModel.IndemnifiedInsurance,
                    InsuredName: $scope.AddNewModel.InsuredName,
                    IsSubmit: isSubmit,
                    WarehousName: $scope.GetNameFromList($scope.WarehouseList, $scope.AddNewModel.Wharehouse_Id, 'Warehous'),
                    DISTRICTNAME: $scope.GetNameFromList($scope.DistrictList, $scope.AddNewModel.DistrictId, 'District'),
                    KvssName: $scope.GetNameFromList($scope.KVSSList, $scope.AddNewModel.KVSSId, 'KVSS'),
                    PurchaseCenter: $scope.GetNameFromList($scope.PurchaseCenterList, item.PurchaseCenter_Id, 'PurchaseCenter'),
                    CommodityName: $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'Commodity'),
                    Price: $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'CommodityPrice'),
                    GoDownName: $scope.GetNameFromList($scope.GodownList, item.Godown_Id, 'Godown'),
                    AutoCalculateQuantity: item.NetQuantity,
                    Moisture: $scope.AddNewModel.Moisture,
                    Kharif: $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'IsKharif'),
                    LicenceValidUpTo: $scope.GetNameFromList($scope.WarehouseList, $scope.AddNewModel.Wharehouse_Id, 'LicenceValidUpTo'),
                    IsCWR: true,
                    Address: $scope.GetNameFromList($scope.WarehouseList, $scope.AddNewModel.Wharehouse_Id, 'Address'),
                    DateOfIssue: $("#DateOfIssue").val(),
                    Crop_year: $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'Crop_year')
                });
            });

            //formData.append("ReqestData", ReqestData);
            //formData.append("OTP", $scope.OTP);
            //formData.append("TXN", $scope.Certtrn);            
            var functionname = "GenerateWarehouseCWCEng";//_language == 'en-us' ? "GenerateWarehouseRSWCEng" : "GenerateWarehouseRSWCHi";//"GenerateWarehouseRSWCHi";//
            $http.post(SiteUrl + "/FertilizerSupply/" + functionname, { OTP: $scope.OTP, TXN: $scope.Certtrn, DetailsList: reqestData }).
                success(function (data, status) {
                    ;
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'WR successfully generated' : "डब्ल्यूआर सफलतापूर्वक जारी हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/GenerateCWC_WR';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "-1") {
                        ShowOkMessage("", _language == 'en-us' ? "WR already ganareted with physical receipt no '" + $scope.AddNewModel.OldReceipt_No + "'." : "डब्ल्यूआर पहले से ही भौतिक रसीद संख्या '" + $scope.AddNewModel.OldReceipt_No + "' के साथ तैयार है।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    } else if (data == "0") {
                        ShowOkMessage("", _language == 'en-us' ? "WR could not be generated. Please try after some time." : "डब्ल्यूआर जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
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


    $scope.AddNew = function () {
        // $("#myOtpDilogBox").modal("show");
        $scope.EditOtherNetQuantity = 0;
        $scope.AddNewModel = {
            Id: 0,
            Receipt_No: "",
            OldReceipt_No: "",
            Wharehouse_Id: $scope.AddNewModel.Wharehouse_Id,
            Lisence_No: "EXEMPTED",
            Received_From_Address: "0",
            Item_Kind: "0",
            Quality: "",
            Packages_Lots: "",
            Packages_Quantity: "",
            Packages_Quantity_Unit: "0",
            NetQuantity: "",
            Godown_Id: $scope.AddNewModel.Godown_Id,
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
            StorageRateCharge: "0",
            IndemnifiedInsurance: "",
            InsuredName: "",
            DoneAmount: "0",
            MarketRate: "0",
            IndemnifiedInsuranceType: "0",
            Moisture: "",
            IsHeighCharges: $scope.AddNewModel.IsHeighCharges
        };
        $scope.totalPrice = "0";
        $scope.RSWCList = [];
        $scope.RSWCList.push($scope.AddNewModel);

        $("#Valid_From_Date").val($("#fromdateserver").val());
        $("#Valid_To_Date").val($("#todateserver").val());

        $scope.AddNewModel.Valid_From_Date = $("#fromdateserver").val();
        $scope.AddNewModel.Valid_To_Date = $("#todateserver").val();

        $("#addModal").modal('show');
    }

    $scope.AddNewItem = function () {
        $scope.ResetAddNewModel();
        $scope.RSWCList.push($scope.AddNewModel);
    }

    $scope.ValidateData = function () {
        ;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");

        $(".validateNewAdd").removeClass("notValid");
        if ($scope.AddNewModel.OldReceipt_No == '' || $scope.AddNewModel.OldReceipt_No == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Receipt Number' : '\n <br /> कृपया रसीद संख्या दर्ज करें |';
            $("#SerialNoOfReceipt").addClass("notValid");
        }

        if ($scope.AddNewModel.Wharehouse_Id == '0' || $scope.AddNewModel.Wharehouse_Id == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please choose Warehouse' : '\n <br /> कृपया वेयरहाउस चुनें |';
            $("#Wharehouse_Id").addClass("notValid");
        }


        if ($scope.AddNewModel.Item_Kind == '0' || $scope.AddNewModel.Item_Kind == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please choose commodity' : '\n <br /> कृपया कमोडिटी चुनें |';
            $("#Item_Kind").addClass("notValid");
        }

        if ($scope.AddNewModel.DistrictId == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please choose district' : '\n <br /> कृपया जिला चुनें |';
            $("#DistrictId").addClass("notValid");
        }

        if ($scope.AddNewModel.KVSSId == '0') {
            if ($scope.AddNewMember == undefined || $scope.AddNewMember.ContactNo.toString().length != 10) {
                message += $scope._language == 'en-us' ? '\n <br /> Please choose kvss' : '\n <br /> कृपया के वी एस एस चुनें |';
                $("#KVSSId").addClass("notValid");
            }
        }

        if ($scope.AddNewModel.Moisture != '' && $scope.AddNewModel.Moisture != undefined && parseFloat($scope.AddNewModel.Moisture) > 100) {

            message += $scope._language == 'en-us' ? '\n <br />Moisture of commodity should be less than 100 %.  Please enter correct moisture value' : '\n <br />कमोडिटी की नमी 100% से कम होनी चाहिए. कृपया सही नमी मान दर्ज करें |';
            $("#KVSSId").addClass("notValid");

        }


        //if ($scope.AddNewModel.PurchaseCenter_Id == "0") {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please choose purchase center' : '\n <br /> कृपया खरीद केंद्र चुनें |';
        //    $("#PurchaseCenter_Id").addClass("notValid");
        //}


        if ($("#Valid_From_Date").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  date' : '\n <br /> कृपया दिनांक चुनें |';
            $("#Valid_From_Date").addClass("notValid");
        }


        if ($("#Valid_To_Date").val() == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  date.' : '\n <br /> कृपया दिनांक चुनें |';
            $("#Valid_To_Date").addClass("notValid");
        }

        if ($("#DateOfIssue").val() == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  Date of issue.' : '\n <br /> कृपया जारी करने की दिनांक चुनें |';
            $("#DateOfIssue").addClass("notValid");
        }
      
        //if ($scope.AddNewMember === undefined || $scope.AddNewMember.PrivateMarkofDepositor === "" || $scope.AddNewMember.PrivateMarkofDepositor.length < 0) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please select  date.' : '\n <br /> कृपया दिनांक चुनें |';
        //    $("#Valid_To_Date").addClass("notValid");
        //}

        //if ($scope.AddNewModel === undefined || $scope.AddNewModel.StorageRateCharge === "" || $scope.AddNewModel.StorageRateCharge.length < 0) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please enter Rate of storage and other charges.' : '\n <br /> कृपया स्टोरेज और अन्य शुल्क दर्ज करें |';
        //}

        //if ($scope.AddNewModel === undefined || $scope.AddNewModel.IndemnifiedInsurance === "" || $scope.AddNewModel.IndemnifiedInsurance.length < 0) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please enter Indemnified / Insured for Fire /Theft/Riots / Other Contingency with.' : '\n <br /> कृपया अग्नि / चोरी / दंगे / अन्य आकस्मिकता के लिए क्षतिपूर्ति / बीमाकृत दर्ज करें|';
        //}

        //if ($scope.AddNewMember === undefined || $scope.AddNewMember.InsuredName === "" || $scope.AddNewMember.InsuredName.length < 0) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please select  date.' : '\n <br /> कृपया दिनांक चुनें |';
        //    $("#Valid_To_Date").addClass("notValid");
        //}


        $scope.RSWCList.forEach(function (item, index) {


            if (item.PurchaseCenter_Id == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please choose purchase center' : '\n <br /> कृपया खरीद केंद्र चुनें |';
                $("#PurchaseCenter_Id_" + index).addClass("notValid");
            }



            if (item.Quality == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.KindQuality + '' : '\n <br /> कृपया ' + $scope.translation.KindQuality + ' दर्ज करें |';
                $("#Quality_" + index).addClass("notValid");
            }


            if (item.Packages_Lots == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.PackagesLots + '' : '\n <br /> कृपया ' + $scope.translation.PackagesLots + ' दर्ज करें |';
                $("#Packages_Lots_" + index).addClass("notValid");
            }

            if (item.Packages_Quantity_Unit == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  ' + $scope.translation.Unit + '' : '\n <br /> कृपया ' + $scope.translation.Unit + ' चुनें |';
                $("#Packages_Quantity_Unit_" + index).addClass("notValid");
            }

            //if (item.Packages_Quantity == '') {
            //    message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.QuantityParUnit + '' : '\n <br /> कृपया ' + $scope.translation.QuantityParUnit + ' दर्ज करें |';
            //    $("#Packages_Quantity_" + index).addClass("notValid");
            //}

            if (item.NameOfSampler == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.NameOfSampler + '' : '\n <br /> कृपया ' + $scope.translation.NameOfSampler + ' दर्ज करें |';
                $("#NameOfSampler_" + index).addClass("notValid");
            }

            if (item.Godown_Id == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  ' + $scope.translation.Godown + '' : '\n <br /> कृपया ' + $scope.translation.Godown + ' चुनें |';
                $("#Godown_Id_" + index).addClass("notValid");
            }

            if (item.Stack_No == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.StackNo + '' : '\n <br /> कृपया ' + $scope.translation.StackNo + ' दर्ज करें |';
                $("#Stack_No_" + index).addClass("notValid");
            }

            if (item.Stack_No != '' && parseInt(item.Stack_No) > 300) {
                message += $scope._language == 'en-us' ? '\n <br /> ' + $scope.translation.StackNo + ' must be less than or equal to 300' : '\n <br /> कृपया ' + $scope.translation.StackNo + ' 300 से कम या इसके बराबर दर्ज करें |';
                $("#Stack_No_" + index).addClass("notValid");
            }



            if (item.NetQuantity == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.NetQuantity + '' : '\n <br /> कृपया ' + $scope.translation.NetQuantity + ' दर्ज करें |';
                $("#NetQuantity_" + index).addClass("notValid");
            }
            if (item.OtherNetQuantity == '' || item.OtherNetQuantity == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.NetQuantity + '' : '\n <br /> कृपया ' + $scope.translation.NetQuantity + ' दर्ज करें |';
                $("#OtherNetQuantity_" + index).addClass("notValid");
            }






        });

        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }

    $scope.RemoveItem = function (index) {
        $scope.RSWCList.splice(index, 1);
    }

    $scope.UnitList = [];
    $scope.UnitList = [{ UnitId: "35", UnitNameEng: "35 KG", UnitNameHi: "30 किलोग्राम" }, { UnitId: "50", UnitNameEng: "50 KG", UnitNameHi: "50 किलोग्राम" }];

    // START : Get Procust List
    $scope.GetUnitList = function () {
        $http.post(SiteUrl + "/Stock/UnitMasterList", { "UnitId": "0" }).
            success(function (data, status) {
                $scope.UnitList = data;
            }).
            error(function (data1, status) {
                $scope.UnitList = [];
            });
    };
    //$scope.GetUnitList();
    // END : Get Procust List


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

    // START : Get Data List
    $scope.WarehouseList = [];
    $scope.BindWarehouseList = function () {
        $http.post(SiteUrl + "/FertilizerSupply/GetWarehouseList?IsCWC=true").
            success(function (data, status) {
                //$scope.WarehouseList = [];
                
                $scope.WarehouseList = data;
                //$scope.AddNewModel.Wharehouse_Id = $scope.WarehouseList[0].WarehousId;
                //$scope.AddNewModel.Lisence_No = $scope.WarehouseList[0].LicenceNo;
                //$scope.AddNewModel.IsHeighCharges = $scope.WarehouseList[0].ishighRated == "True" ? true : false;
                $scope.GetGodownList();
            }).
            error(function () {
                //$scope.WarehouseList = [];
            });
    };


    $scope.BindWarehouseList();

    $scope.ChangeWharehouse = function () {  
        $scope.WarehouseList.forEach(function (item, index) {
            if ($scope.AddNewModel.Wharehouse_Id == item.WarehousId) {
                $scope.AddNewModel.Lisence_No = item.LicenceNo;
            }
        })        
    }

    // END : Get Data List
    $scope.DistrictList = [];
    // $scope.ItemList = [{ ItemId: "1", ItemNameEn: "Moong", ItemNameHi: "मूंग" }];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                //$scope.DistrictList = [];
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")
                $scope.DistrictList = [];
            });
    }
    $scope.GetDistrictList();

    $scope.KVSSList = [];
    $scope.GetKVSSList = function (id) {
        $(".blockUI").fadeIn("slow");
        $scope.AddNewModel.KVSSId = "0";
        return $http.post(SiteUrl + "/FertilizerSupply/GetKVSS", { "DistrictId": +$scope.AddNewModel.DistrictId }).
            success(function (data, status) {
                //$(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
                $scope.KVSSList = data;
                $scope.GetPurchaseCenterList('0');
                $scope.AddNewModel.KVSSId = id;

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
            });
    };
    $scope.PurchaseCenterList = []

    $scope.GetPurchaseCenterList = function (id) {
        $(".blockUI").fadeIn("slow");
        $scope.AddNewModel.PurchaseCenter_Id = "0";
        return $http.post(SiteUrl + "/FertilizerSupply/GetPurchaseCenterList", { "DistrictId": +$scope.AddNewModel.DistrictId, "CommodityId": $scope.AddNewModel.Item_Kind }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = data;

                $scope.AddNewModel.PurchaseCenter_Id = id


                $scope.RSWCList.forEach(function (item, index) {
                    $scope.SetNetQuantity1(index);
                });

                $scope.ItemList.forEach(function (item, index) {
                    if (item.CommodityId == $scope.AddNewModel.Item_Kind) {
                        
                        if (!$scope.AddNewModel.IsHeighCharges) {
                            $scope.AddNewModel.StorageRateCharge = item.NafedChargesa;
                        } else {
                            $scope.AddNewModel.StorageRateCharge = item.NafedHighCharges;
                        }
                    }
                });



            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = [];
            });
    };

    $scope.GodownList = [];

    $scope.GetGodownList = function () {
        $http.post(SiteUrl + "/Master/RajfedGoDownList?GoDownId=0&WarehouseId=" + $scope.AddNewModel.Wharehouse_Id).
            success(function (data, status) {
                $scope.GodownList = data;
                //var dt = {
                //    GoDownId: "-1",
                //    GoDownNameEng: $scope.WarehouseList[0].WarehousNameEn,
                //    GoDownNameHi: $scope.WarehouseList[0].WarehousNameEn,
                //};
                //$scope.GodownList.push(dt);
                //data.forEach(function (itm, index) {
                //    $scope.GodownList.push(itm);
                //})
                ////$scope.GodownList.splice(0, 0, dt);
                //$scope.RSWCList[0].Godown_Id = $scope.GodownList[0].GoDownId;

            }).
            error(function (data, status) {
                //alert("Error while fetching Post list")
                $scope.GodownList = [];
            });
    }
    $scope.totalPrice = 0;
    $scope.itemPrice = 0;
    $scope.totalLots = 0;
    $scope.totalNetQty = 0;
    $scope.SetNetQuantity = function (index) {
        $scope.totalPrice = 0;
        $scope.totalLots = 0;
        $scope.totalNetQty = 0;
        $scope.itemPrice = $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'CommodityPrice');
        $scope.RSWCList.forEach(function (item, indx) {
            ;
            if (index == indx) {

                item.NetQuantity = item.Packages_Lots * item.Packages_Quantity_Unit / 100;
                //if (item.OtherNetQuantity == '') {
                //     item.OtherNetQuantity = item.Packages_Lots * item.Packages_Quantity_Unit / 100;
                //}
            }


            $scope.totalPrice = $scope.totalPrice + (item.OtherNetQuantity * $scope.itemPrice);// + (item.OtherNetQuantity*$scope.AddNewModel.StorageRateCharge);
            $scope.totalLots = $scope.totalLots + item.Packages_Lots;
            $scope.totalNetQty = $scope.totalNetQty + item.NetQuantity;

        });
    }
    $scope.EditOtherNetQuantity = 0;
    $scope.SetNetQuantity1 = function (index) {
        $scope.totalPrice = 0;
        $scope.totalLots = 0;
        $scope.totalNetQty = 0;
        $scope.itemPrice = $scope.GetNameFromList($scope.ItemList, $scope.AddNewModel.Item_Kind, 'CommodityPrice');
        $scope.RSWCList.forEach(function (item, indx) {
            ;
            if (index == indx) {

                item.NetQuantity = item.Packages_Lots * item.Packages_Quantity_Unit / 100;
                if (item.EditOtherNetQuantity != 0 && item.EditOtherNetQuantity != undefined) {
                    item.OtherNetQuantity = item.EditOtherNetQuantity;
                } else {
                    item.OtherNetQuantity = item.Packages_Lots * item.Packages_Quantity_Unit / 100;
                }
            }



            $scope.totalPrice = $scope.totalPrice + (item.OtherNetQuantity * $scope.itemPrice);// + (item.OtherNetQuantity*$scope.AddNewModel.StorageRateCharge);
            $scope.totalLots = $scope.totalLots + item.Packages_Lots;
            $scope.totalNetQty = $scope.totalNetQty + item.NetQuantity;

        });
    }



    $scope.GetNameFromList = function (list, id, type) {
        try {
            if (type == 'Godown') {
                var data = list.filter(function (x) { return x.GoDownId.toString() === id.toString() })[0];
                return _language == 'en-us' ? data.GoDownNameEng : data.GoDownNameHi;
            } else if (type == 'Unit') {
                var data = list.filter(function (x) { return x.UnitId.toString() === id })[0];
                return _language == 'en-us' ? data.UnitNameEng : data.UnitNameHi;
            } else if (type == 'District') {
                var data = list.filter(function (x) { return x.DISTRICT_ID.toString() === id })[0];
                return _language == 'en-us' ? data.DISTRICT_ENG : data.DISTRICT_ENG;
            } else if (type == 'KVSS') {
                var data = list.filter(function (x) { return x.KvssId.toString() === id })[0];
                return _language == 'en-us' ? data.KVSS : data.KVSS;
            } else if (type == 'PurchaseCenter') {
                var data = list.filter(function (x) { return x.Center_Id.toString() === id })[0];
                return _language == 'en-us' ? data.CenterNameEn : data.CenterNameHi;
            } else if (type == 'Commodity') {
                var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
                return _language == 'en-us' ? data.CommodityNameEn : data.CommodityNameHi;
            } else if (type == 'Warehous') {
                var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
                return _language == 'en-us' ? data.WarehousNameEn : data.CommodityNameEn;
            } else if (type == 'CommodityPrice') {
                var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
                return _language == 'en-us' ? data.Price : data.Price;
            } else if (type == 'LicenseNo') {
                var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
                return _language == 'en-us' ? data.LicenceNo : data.LicenceNo;
            } else if (type == 'IsKharif') {
                var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
                return (data.IsKharif == null ? false : true) == true ? "खरीफ" : "रबी";
            } else if (type == 'LicenceValidUpTo') {
                var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
                return data.LicenceValidUpTo;
            } else if (type == 'Address') {
                var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
                return data.Address;
            } else if (type == 'Crop_year') {
                var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
                return data.Crop_year;
            }

        } catch (ex) {
            return "";
        }
    }
    $scope.showPriView = false;

    $scope.totalPrice = 0;
    $scope.totalLots = 0;
    $scope.totalNetQty = 0;


    $scope.GenerateRequestView = function () {
        if ($scope.ValidateData()) {
            $scope.AddNewModel.Valid_From_Date = $("#Valid_From_Date").val();
            $scope.AddNewModel.Valid_To_Date = $("#Valid_To_Date").val();
            $scope.AddNewModel.DateOfIssue = $("#DateOfIssue").val();
            $scope.showPriView = true;
        }
    }
    $scope.GenerateRequestBack = function () {
        $scope.showPriView = false;
    }

    $scope.EditWR = function (item) {
        $scope.EditOtherNetQuantity = 0;
        $scope.GetWarehouseDetailsList(item);
        $("#addModal").modal('show');
    }
    $scope.GetWarehouseDetailsList = function (th) {
        $(".blockUI").fadeIn("slow");

        $scope.OTP = "";
        $scope.IsValida = false;
        $scope.ReleasedNofQty = th.release_quantity;
        $scope.ReceiverAadharNo = th.receiver_aadhar_no;
        $scope.AddNewModel.ReceiverAadharNo = th.receiver_aadhar_no;
        $scope.ReceiverName = th.receiver_name;
        $scope.ReceiverFName = th.receiver_fname;
        $scope.IsDisabledEditQty = true;
        $scope.Order_Id = th.release_order_id;
        $scope.Receipt_No = th.receipt_number;
        $scope.TotalPackages_Lots = 0
        return $http.post(SiteUrl + "/FertilizerSupply/GetWarehouseRSWCReceiptDetails", { ReceiptNo: th.receipt_no }).
            success(function (data, status) {
                var data1 = JSON.parse(data.DataList);
                $scope.RSWCList = [];
                ;
                var totalqty = 0;
                data1.forEach(function (item, index) {
                    $scope.ResetAddNewModel();
                    $scope.AddNewModel = {
                        Id: item.id,
                        Receipt_No: item.receipt_no,
                        OldReceipt_No: item.receipt_serial_no,
                        Wharehouse_Id: item.wharehouse_id.toString(),
                        Lisence_No: item.lisence_no,
                        Received_From_Address: item.Received_From_Address,
                        Item_Kind: item.item_kind.toString(),
                        Quality: item.quality,
                        Packages_Lots: item.packages_lots,
                        Packages_Quantity: item.packages_quantity,
                        Packages_Quantity_Unit: item.packages_quantity_unit.toString(),
                        Godown_Id: item.godown_id.toString(),
                        Stack_No: item.stack_no,
                        Valid_From_Date: item.from_date,
                        Valid_To_Date: item.to_date,
                        DistrictId: item.received_from_district.toString(),
                        KVSSId: item.received_from_kvss.toString(),
                        PurchaseCenter_Id: item.purchase_center_id.toString(),
                        NameOfSampler: item.sampler_name,
                        ConditionOfGoods: item.goods_condition,
                        //IsEditable : item,
                        // OtherNetQuantity : item,
                        PrivateMarkofDepositor: item.PrivateMarkofDepositor,
                        StorageRateCharge: item.StorageRateCharge,
                        IndemnifiedInsurance: item.IndemnifiedInsurance,
                        InsuredName: item.InsuredName,
                        QuantityNote: item.QuantityNote,

                        // DoneAmount : item,
                        //MarketRate : item,
                        IndemnifiedInsuranceType: item.IndemnifiedInsurance,
                        Moisture: item.Moisture,
                        EditOtherNetQuantity: item.net_quantity,
                        IsHeighCharges: $scope.AddNewModel.IsHeighCharges
                    }
                    $scope.GetKVSSList($scope.AddNewModel.KVSSId);
                    $scope.GetPurchaseCenterList($scope.AddNewModel.PurchaseCenter_Id);
                    $scope.RSWCList.push($scope.AddNewModel);

                    //$scope.RSWCList[index].Godown_Id = $scope.GodownList[0].GoDownId;
                    $scope.RSWCList[index].Packages_Quantity_Unit = $scope.AddNewModel.Packages_Quantity_Unit;
                    $scope.RSWCList[index].Godown_Id = $scope.AddNewModel.Godown_Id.toString();
                    $scope.SetNetQuantity(index);
                    $scope.RSWCList[index].OtherNetQuantity = item.net_quantity;

                    if ($scope.AddNewModel.IndemnifiedInsuranceType != "Self") {
                        $scope.AddNewModel.IndemnifiedInsuranceType = "Depositor";
                    }
                });

                $("#Valid_From_Date").val($("#fromdateserver").val());
                $("#Valid_To_Date").val($("#todateserver").val());

                $scope.AddNewModel.Valid_From_Date = $("#fromdateserver").val();
                $scope.AddNewModel.Valid_To_Date = $("#todateserver").val();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.RSWCList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };


    $scope.DeleteWR = function (id) {


        ShowAlertMessage("Alert", _language == 'en-us' ? "Are you sure you want to delete this saved WR. ?" : "क्या आप वाकई इस सहेजे गए WR को हटाना चाहते हैं? ?", function () {
            $(".blockUI").fadeIn("slow");
            $scope.AddNewModel.PurchaseCenter_Id = "0";
            return $http.post(SiteUrl + "/FertilizerSupply/DeleteWR", { Receipt_No: id }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status) {
                        ShowOkMessage("Success", _language == 'en-us' ? "WR entry is deleted." : "WR प्रविष्टि हटा दी गई है।", function () {

                            window.location = '../FertilizerSupply/Warehouse';
                        });
                    } else {
                        ShowOkMessage("Success", _language == 'en-us' ? "WR could not be deleted. Please try after some time.." : "WR डिलीट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।।", function () { });

                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.PurchaseCenterList = [];
                });
        });
    };


});