var AppClasses = angular.module("CommodityReleaseOrderMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("CommodityReleaseOrderCnt", function ($scope, $http, $rootScope) {
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
        $scope.GetCommodityReleaseOrderList();
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


    $scope.ChangeAddNewFY = function (th) {
      
        var year = $scope.AddNewModel.FinancialYear.split('-')[0];
        var minDate = "01/04/" + year ;
        var maxDate = "31/03/" + (parseInt(year) + 1) ;
        $(".SetCalender").datepicker("option",  "minDate", minDate);
        $(".SetCalender").datepicker("option", "maxDate", maxDate);

       // $(".Todate").datepicker("option", "minDate", year + "/04/01");

    }


    $scope.AddNewModel = {
        Id: 0,
        FinancialYear: "",
        Titel: "",
        FromDate: "0",
        ToDate: "EXEMPTED"       
    };

    $scope.OTP = "";

    $scope.CertSocietyIdEnc = "";
    $scope.CertElectionMasterId = "";
    $scope.CertIndex = "";
    $scope.CertMemberId = "";
    $scope.EncryptedAadharNo = "";
    $scope.Certtrn = "";
    $scope.RSWCViewList = [];

    $scope.ListType = "PandingDO";

    $scope.GetWarehouseFilterList = function (type) {
        $scope.ListType = type;
        $scope.ClearSearch();
    }

    $scope.GetCommodityReleaseOrderList = function () {
        $(".blockUI").fadeIn("slow");
        var url = "GetDeliveryOrder";
        if ($scope.ListType == 'OpenWR' || $scope.ListType == 'CloseWR') {
            var url = "GetDeliveryOrderWRList";
        }
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        var jsonFilterParameters = { "WarehousId": $scope.Filter.WarehousId, "CommodityId": $scope.Filter.CommodityId, "FromDate": $scope.Filter.FromDate, "ToDate": $scope.Filter.ToDate };
        return $http.post(SiteUrl + "/FertilizerSupply/" + url, JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType, Filter: jsonFilterParameters })).
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


    $scope.GetCommodityReleaseOrderList();
    // START : Get Data List
    $scope.WarehouseList = function () {
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": "0" }).
            success(function (data, status) {
                $scope.Warehouses = data;
                $scope.TotalWarehouses = data.length;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function () {
                $scope.Warehouses = [];
            });
    };
    // END : Get Data List

    //Get Commodity List for dropdown
    $scope.GetCommodityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMMODITY&MasterCode=-1").
            success(function (data, status) {
                $scope.CommodityList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching commodity list");
                $scope.CommodityList = [];
            });
    };

    //Get Post List for dropdown

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
            ReceiverAadharNo: "",
            ReceiverName: "",
            ReceiverFName: "",
            ReleaseQuantity: "",
            CompanyName: "",
            Address: "",
            MobileNo: "",
            Email: "",
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

        //$scope.AddNewModel = {
        //    Id: 0,
        //    Receipt_No: item.receipt_no,
        //    Valid_From_Date: ""
        //};
        //if (item.Loan_start != '') {
        //    $scope.UpdateType = "loan_close_date";
        //    $scope.AddNewModel.Valid_To_Date = item.Loan_start;
        //    $("#Valid_To_Date").val(item.Loan_start);
        //    $(".Fromdate").datepicker("option", "minDate", item.Loan_start);
        //} else {
        //    $scope.UpdateType = "loan_start_date";
        //    $(".Fromdate").datepicker("option", "minDate", item.from_date);
        //}

        // $('#myOTPModal').modal("show");


        // $scope.ResetAddNewModel();


        $scope.isValidAadharNo = true;

        $scope.AddNewModel.ReleasedQuantity = 0;
        $scope.AddNewModel.TotalCommodity = 0;
        $scope.AddNewModel.ReceiverAadharN = "";
        $scope.otpModel.IsChecked = false;
        $scope.otpModel1.IsChecked = false
        //$scope.OrderList = [];
        //$scope.OrderList.push({
        //    Wharehouse_Id: $scope.AddNewModel.Wharehouse_Id,
        //    WarehousName: $scope.GetNameFromList($scope.Warehouses, $scope.Filter.WarehousId, 'Warehouse'),
        //    CommodityName: $scope.GetNameFromList($scope.CommodityList, $scope.Filter.CommodityId, 'Commodity'),
        //    FY: "" + $scope.GetNameFromList($scope.CommodityList, $scope.Filter.CommodityId, 'IsKharif'),
        //    Item_Kind: $scope.Filter.CommodityId,
        //    ReleasedQuantity: "",
        //    Packages_Lots:""
        //})


        $("#addModal").modal('show');
    }


    $scope.GetAvailableCommodity = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/FertilizerSupply/GetAvailableCommodity?Wharehouse_id=" + $scope.AddNewModel.WarehousId + "&CommodityId=" + $scope.AddNewModel.CommodityId + "&FinancialYear=" + $scope.AddNewModel.FinancialYear).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                var jdata = JSON.parse(data.DataList)
                $scope.AddNewModel.TotalCommodity = jdata[0].Total;
                $scope.AddNewModel.ReleasedQuantity = jdata[0].ReleasedTotal;
            }).
            error(function (data, status) {
                // $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }



    $scope.FinancialYearList = [];
    $scope.BindFY = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }



    $scope.BindFY();
    $scope.AddNewItem = function () {
        $scope.ResetAddNewModel();
        $scope.RSWCList.push($scope.AddNewModel);
    }

    $scope.ValidateData = function () {
        
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");
        $(".validateNewAdd").removeClass("notValid");
        if ($scope.AddNewModel.ReceiverAadharNo == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Aaddhar Number' : '\n <br /> कृपया आधार नंबर दर्ज करें |';
            $("#ReceiverAadharNo").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }



    $scope.ValidateSubmitData = function () {
        
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");



        $(".validateNewAdd").removeClass("notValid");

        if ($scope.AddNewModel.MobileNo == '' || $scope.AddNewModel.MobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.MobileNo + '' : '\n <br /> कृपया ' + $scope.translation.MobileNo + ' दर्ज करें |';
            $("#MobileNo").addClass("notValid");
        }
        if ($scope.AddNewModel.Email == '' || $scope.AddNewModel.Email == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.EmailId + '' : '\n <br /> कृपया ' + $scope.translation.EmailId + ' दर्ज करें |';
            $("#Email").addClass("notValid");
        }
        if ($scope.AddNewModel.CompanyName == '' || $scope.AddNewModel.CompanyName == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.CompanyName + '' : '\n <br /> कृपया ' + $scope.translation.CompanyName + ' दर्ज करें |';
            $("#CompanyName").addClass("notValid");
        }
        if ($scope.AddNewModel.Address == '' || $scope.AddNewModel.Address == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.Address + '' : '\n <br /> कृपया ' + $scope.translation.Address + ' दर्ज करें |';
            $("#Address").addClass("notValid");
        }

        if ($scope.AddNewModel.WarehousId == '' || $scope.AddNewModel.WarehousId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  ' + $scope.translation.Warehouse + '' : '\n <br /> कृपया ' + $scope.translation.Warehouse + ' दर्ज करें |';
            $("#wharehouse_id").addClass("notValid");
        }

        if ($scope.AddNewModel.CommodityId == '' || $scope.AddNewModel.CommodityId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  ' + $scope.translation.Commodity + '' : '\n <br /> कृपया ' + $scope.translation.Commodity + ' दर्ज करें |';
            $("#CommodityId").addClass("notValid");
        }

        if ($scope.AddNewModel.FinancialYear == '' || $scope.AddNewModel.FinancialYear == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  ' + $scope.translation.FincialYearHeading + '' : '\n <br /> कृपया ' + $scope.translation.FincialYearHeading + ' दर्ज करें |';
            $("#FinancialYear").addClass("notValid");
        }

        if ($scope.AddNewModel.Packages_Lots == '' || $scope.AddNewModel.Packages_Lots == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.CommodityTobeRelease + '' : '\n <br /> कृपया ' + $scope.translation.CommodityTobeRelease + ' दर्ज करें |';
            $("#Packages_Lots").addClass("notValid");
        }

        if (parseFloat($scope.AddNewModel.Packages_Lots) > (parseFloat($scope.AddNewModel.TotalCommodity) - parseFloat($scope.AddNewModel.ReleasedQuantity))) {
            message += $scope._language == 'en-us' ? '\n <br /> Entered quantity is greter than total remaining quantity. Please enter correct value' : '\n <br /> दर्ज की गई मात्रा कुल शेष मात्रा से अधिक है। कृपया सही मान दर्ज करें |';
            $("#Packages_Lots").addClass("notValid");
        }

        if (parseFloat($scope.AddNewModel.Packages_Lots) <= 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Entered quantity is less than total remaining quantity. Please enter correct value' : '\n <br /> दर्ज की गई मात्रा कुल शेष मात्रा से अधिक है। कृपया सही मान दर्ज करें |';
            $("#Packages_Lots").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }



    $scope.GenerateRequestBack = function () {
        $scope.showPriView = false;
    }

    $scope.isValidAadharNo = false;

    $scope.otpModel = {
        IsChecked: false
    };
    $scope.otpModel1 = {
        IsChecked: false
    };



    $scope.SendAadharOTP = function () {
        if ($scope.ValidateData()) {


            if (!$scope.otpModel.IsChecked) {
                ShowOkMessage("", _language == 'en-us' ? 'Please allow system for your AADHAAR based Authentication/ eKYC.\n' : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।|\n', function () {
                });
                return;
            }

            $(".se-pre-con").fadeIn("slow");
            //$http.post.ajax({
            //    url: '../FertilizerSupply/SendOTPOnAADHAR?_AadharNo=' + $scope.AddNewModel.ReceiverAadharNo,
            //    //data: JSON.stringify({
            //    //    "objElectionSocietyData": ArrDates,
            //    //    "ElectionMasterIdEnc": $('#btnautoclick').val(),
            //    //}),
            //    type: 'GET',
            //    contentType: 'application/json',
            //    // dataType: 'json',
            //    beforeSend: function () {
            //    },



            $http.post(SiteUrl + "/FertilizerSupply/SendOTPOnAADHAR", { _AadharNo: $scope.AddNewModel.ReceiverAadharNo }).
                success(function (data, status) {
                    
                    $(".se-pre-con").fadeOut("slow");
                    if (data.Result == false) {
                        ShowOkMessage("", _language == 'en-us' ? "Invalid aaddhaar number or something else. Please try after some time." : "अमान्य आधार संख्या या कुछ और। कृपया कुछ देर बाद प्रयास करें।", function () { });
                        // window.location = '../Election/ElectionSocietyList';
                    } else {


                        $scope.EncryptedAadharNo = data.AadharNo;
                        $scope.Certtrn = data.txnid;
                        $('#myOTPModal').modal("show");
                        ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for  aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                            $scope.OTP = "";
                            //$('#myOTPModal').modal("show");
                        });

                        // window.location = '../Election/ElectionSocietyList';
                        $(".se-pre-con").fadeOut("slow");
                    }




                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    //return false;
                });

















        }
    }


    $scope.VerifyAADHARNo = function () {

        if ($scope.OTP == '') {
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            ShowOkMessage("", msg, function () { });
            //return;
        } else {
            $(".se-pre-con").fadeIn("slow");


            // Un-comment below code after testing
            $http.post(SiteUrl + "/FertilizerSupply/VeryifyOTPsentOnAADHAR", { OTP: $scope.OTP, TXN: $scope.Certtrn, _EncryptedAadharNo: $scope.EncryptedAadharNo }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data.Result === false) {
                        ShowOkMessage("", "Invalid OTP", function () { });
                        return;
                    }
                    else if (data.Result === true) {
                        $('#myOTPModal').modal("hide");
                        $scope.isValidAadharNo = true;
                        $scope.OTP = "";
                        // [{"Column1":"Success","Column2":"Balwant Chaudhary","Column3":"M","Column4":"20-09-1984","Column5":"../Content/NewRegistration/member_259ad6e690d04f6a90bac2a08afbf25e.jpg","Column6":"बलवंत चौधरी","Column7":"T1+Bhe8Ju9s4QjBaM66sAg==","Column8":" Mohan Lal Chaudhary","Column9":" मोहन लाल चौधरी"}]
                        //GENDER
                        $scope.AddNewModel.ReceiverName = data.AADHARDetails[0].Column2;
                        $scope.AddNewModel.ReceiverFName = data.AADHARDetails[0].Column9;

                    }
                    else {
                        $scope.isValidAadharNo = false;
                    }
                }).
                error(function (data, status) {
                    $scope.SocietyRegdatanew = [];
                });
        }
    }



    $scope.GenerateRequest = function () {

        if ($scope.ValidateSubmitData()) {


            if (!$scope.otpModel1.IsChecked) {
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
                            $('#myOrderOTPModal').modal("show");
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
            // $scope.OrderList.forEach(function (item, index) {
            reqestData.push({
                //Id: item.Id,
                //Receipt_No: item.receipt_no,
                ReceiverAadharNo: $scope.AddNewModel.ReceiverAadharNo,
                ReceiverName: $scope.AddNewModel.ReceiverName,
                ReceiverFName: $scope.AddNewModel.ReceiverFName,
                ReleaseQuantity: $scope.AddNewModel.Packages_Lots,
                CompanyName: $scope.AddNewModel.CompanyName,
                Address: $scope.AddNewModel.Address,
                MobileNo: $scope.AddNewModel.MobileNo,
                Email: $scope.AddNewModel.Email,
                Wharehouse_Id: $scope.AddNewModel.WarehousId,
                Item_Kind: $scope.AddNewModel.CommodityId,
                FY_Id: $scope.AddNewModel.FinancialYear,
                WarehousName: $scope.GetNameFromList($scope.Warehouses, $scope.AddNewModel.WarehousId, 'Warehouse'),
                CommodityName: $scope.GetNameFromList($scope.CommodityList, $scope.AddNewModel.CommodityId, 'Commodity'),
                FY: $scope.GetNameFromList($scope.CommodityList, $scope.AddNewModel.CommodityId, 'IsKharif') + " / " + $scope.GetNameFromList($scope.FinancialYearList, $scope.AddNewModel.FinancialYear, 'FY'),
                NetQuantity: $scope.AddNewModel.TotalCommodity,
                ReleasedQuantity: $scope.AddNewModel.ReleasedQuantity,

                //WarehousName:""

                // });
            });

            //formData.append("ReqestData", ReqestData);
            //formData.append("OTP", $scope.OTP);
            //formData.append("TXN", $scope.Certtrn);
            var functionname = "InsertUpdateReleaseOrder";//_language == 'en-us' ? "GenerateWarehouseRSWCEng" : "GenerateWarehouseRSWCHi";//"GenerateWarehouseRSWCHi";//
            $http.post(SiteUrl + "/FertilizerSupply/" + functionname, { OTP: $scope.OTP, TXN: $scope.Certtrn, DetailsList: reqestData }).
                success(function (data, status) {
                    
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'Order successfully generated' : "आदेश सफलतापूर्वक जारी हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/ReleaseOrder';
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


    $scope.OrderList = [];
    $scope.SelectDemand = function (th) {
        if (th.IsChecked) {
            $scope.OrderList.push(th);
        } else {
            var index = $scope.OrderList.indexOf(th);
            $scope.OrderList.splice(index, 1);
        }

        if ($scope.RSWCViewList.filter(function (x) { x.IsChecked === true }).length == 0) {
            $scope.RSWCViewList.forEach(function (item, index) {

                item.IsDisabled = false;

            });
        } else {
            $scope.RSWCViewList.forEach(function (item, index) {
                if (item.wharehouse_id == th.wharehouse_id) {
                    item.IsDisabled = false;
                } else {
                    item.IsDisabled = true;
                }
            });
        }
    }
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
    $scope.WarehouseList();
    $scope.GetCommodityList();


    $scope.SummaryList = [];
    $scope.ShowDOSummary = function (th) {
        
        $("#SummaryModal").modal("show");
        $(".blockUI").fadeIn("slow");

        return $http.post(SiteUrl + "/FertilizerSupply/GetCommodityReleaseOrderDetails?RequestNo=" + th.request_no).



            // return $http.post(SiteUrl + "/FertilizerSupply/GetWRSummaryList", JSON.stringify({ Details: jsonParameters, RequestType: 0 })).
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