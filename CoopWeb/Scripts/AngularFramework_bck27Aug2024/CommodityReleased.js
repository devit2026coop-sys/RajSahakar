
var AppClasses = angular.module("CommodityReleasedMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("CommodityReleasedCnt", function ($scope, $http, $rootScope) {
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
        if ($scope.Filter.FilterType == 'wrs.created_date') {
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

    $scope.isPreview = false;
    $scope.AddNewModel = {
        Receipt_No: "",
        TotalPackages_Lots: "",
        Packages_Quantity_Unit: "",
        ReleasedPackagesLots: "0",
        Packages_Lots: "",
        Id: "",
        CommodityName: "",
        LoanClosedDate: "",
        LoanStartDate: "",
        ReceiverAadharNo: "",
        Order_Id: "0",
        SerialNo: "",
        Godown_Id: "0",
        StackNo: "0",
        WRNo: "0"
    };

    $scope.OTP = "";
    $scope.Certtrn = "";
    $scope.CertSocietyIdEnc = "";
    $scope.CertElectionMasterId = "";
    $scope.CertIndex = "";
    $scope.CertMemberId = "";
    $scope.ReleasedNofQty = 0;
    $scope.RSWCViewList = [];
    $scope.ListType = "PandingDO";
    $scope.GetWarehouseFilterList = function (type) {
        $scope.ListType = type;
        $scope.ClearSearch();
    }

    $scope.GetWarehouseList = function () {
        
        $(".blockUI").fadeIn("slow");
        var url = "GetWarehouseDeliveryOrder";
        if ($scope.ListType == 'OpenWR' || $scope.ListType == 'CloseWR') {
            var url = "GetWarehouseDeliveryOrderWRList";
        }
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/FertilizerSupply/" + url, JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType  })).
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
            Receipt_No: "",
            TotalPackages_Lots: "",
            Packages_Quantity_Unit: "",
            ReleasedPackagesLots: "0",
            Packages_Lots: "",
            Id: "",
            CommodityName: "",
            LoanClosedDate: "",
            LoanStartDate: "",
            ReceiverAadharNo: "",
            Order_Id: "0",
            SerialNo: "",
            Godown_Id: "0",
            StackNo: "0",
            WRNo: "0"

        };
    }
    $scope.RSWCList = [];
    // $scope.RSWCList.push($scope.AddNewModel);

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

    $scope.BackToList = function () {
        $scope.ShowOrderDetails = false;
    }

    $scope.AddNew = function (item) {
        if (item.IsOldEntry == 1) {
            $scope.isValidAadharNo = true;
        } else {
            $scope.isValidAadharNo = false;
        }
        $scope.GetWarehouseDetailsList(item);
        //$("#addModal").modal('show');
        $scope.ShowOrderDetails = true;
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


    $scope.GenerateRequestBack = function () {
        $scope.showPriView = false;
    }

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
    $scope.RSWCList = [];
    $scope.RSWCMasterList = [];
    $scope.Commodityname = "";
    $scope.TotalPackages = 0;
    $scope.ReleasedPackages = 0;
    $scope.ReceiverAadharNo = "";
    $scope.IsValida = false;
    $scope.IsDisabledEditQty = false;
    $scope.Receipt_No = "";
    $scope.TotalPackages_Lots = 0

    $scope.CompanyName = "";
    $scope.Address = "";
    $scope.MobileNo = "";
    $scope.EmailId = ""

    $scope.GodownList = [];
    $scope.StackList = [];
    $scope.WRList = [];

    $scope.IsPageLoad = false;


    $scope.GetWarehouseDetailsList = function (th) {
        $scope.GodownList = [];
        $scope.StackList = [];
        $scope.WRList = [];
        $(".blockUI").fadeIn("slow");
        $scope.OTP = "";
        $scope.IsValida = false;
        $scope.ReleasedNofQty = th.release_quantity - th.Released_quantity;
        $scope.ReceiverAadharNo = th.receiver_aadhar_no;
        $scope.AddNewModel.ReceiverAadharNo = "XXXXXXXX" + th.receiver_aadhar_no.substring(8, 13);
        $scope.ReceiverAadharNo = th.aadhar_no;
        $scope.ReceiverName = th.receiver_name;
        $scope.ReceiverFName = th.receiver_fname;
        $scope.IsDisabledEditQty = true;
        $scope.Order_Id = th.release_order_id;
        $scope.Receipt_No = th.receipt_number;
        $scope.TotalPackages_Lots = 0
        $scope.CompanyName = th.CompanyName;
        $scope.Address = th.Address;
        $scope.MobileNo = th.MobileNo;
        $scope.EmailId = th.Email;
        return $http.post(SiteUrl + "/FertilizerSupply/GetDODetailsWR", { DelivberyNo: th.request_no }).
            success(function (data, status) {
                var data = JSON.parse(data.DataList);
                $scope.RSWCList = [];
                $scope.RSWCMasterList = [];
                
                var totalqty = 0;



                data.forEach(function (item, index) {
                    //       //var selectedRoles = vm.roles.filter(function (x) { return x.id === role.id; });
                    if ($scope.GodownList.filter(function (x) { return x.godown_id === item.godown_id }).length == 0) {
                        $scope.GodownList.push({
                            GoDownNameEng: item.GoDownNameEng,
                            GoDownNameHi: item.GoDownNameHi,
                            godown_id: item.godown_id
                        })
                    }
                    if ($scope.StackList.filter(function (x) { return x.stack_no === item.stack_no }).length == 0) {
                        $scope.StackList.push({
                            stack_no: item.stack_no
                        })
                    }
                    if ($scope.WRList.filter(function (x) { return  x.receipt_no === item.receipt_no }).length == 0) {
                        $scope.WRList.push({
                            receipt_no: item.receipt_no
                        })
                    }


              
                    var list = $scope.ItemList.find(function (x) { return x.CommodityId.toString() === item.item_kind.toString() });


                    var newModel = {
                        
                        Receipt_No: item.receipt_no,
                        Net_Quantity: item.net_quantity,
                        ReleasedQuantity: item.ReleasedQuantity,
                        GodownStack: (_language === 'en-us' ? item.GoDownNameEng : item.GoDownNameHi) + '/' + item.stack_no,
                        TotalPackages_Lots: item.packages_lots,
                        Packages_Quantity_Unit: item.packages_quantity_unit.toString() + ($scope._language == 'en-us' ? " KG" : " किलोग्राम"),
                        ReleasedPackagesLots: item.ReleasedPackagesLots,
                        Packages_Lots:"",// data.length == 1 ? $scope.ReleasedNofQty : item.packages_lots,
                        LoanClosedDate: item.Loan_closed,
                        LoanStartDate: item.Loan_start,
                        SerialNo: item.receipt_serial_no,   
                        ReceiverAadharNo: "XXXXXXXX" + th.receiver_aadhar_no.substring(8, 13),
                        Id: item.id,
                        IsDisabled: true,
                        Godown_Id: item.godown_id,
                        StackNo: item.stack_no,
                        ReleaseOrderId: item.release_order_id,
                        IsShow: true
                    }
                    if (item.Loan_start == '') {
                        $scope.IsValida = true
                    } else if (item.Loan_start != '' && item.Loan_closed != '') {
                        $scope.IsValida = true
                    } else {
                        $scope.IsValida = false;
                    }
                    $scope.ReleasedPackages += item.ReleasedPackagesLots;
                    $scope.TotalPackages += item.packages_lots;
                    totalqty += item.packages_lots;
                    $scope.TotalPackages_Lots = item.packages_lots
                    $scope.CommodityName = _language == 'en-us' ? list.CommodityNameEn : list.CommodityNameHi;
                    $scope.RSWCList.push(newModel);
                    $scope.RSWCMasterList.push(newModel);
                });
                if ($scope.RSWCList.length > 1 && $scope.ReleasedNofQty < totalqty) {
                    $scope.IsDisabledEditQty = false;
                    $scope.RSWCList.forEach(function (item, index) {
                        $scope.AddNewModel.Packages_Lots = "";
                    });
                }
             
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.RSWCList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };

    $scope.GetFilterWarehouseDetailsList = function () {
        $(".blockUI").fadeIn("slow");
        //$scope.RSWCList = [];
        $scope.RSWCList.forEach(function (itm, index) {
            if ($scope.AddNewModel.Godown_Id != '0' && $scope.AddNewModel.StackNo != '0' && $scope.AddNewModel.WRNo != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString() && itm.StackNo.toString() == $scope.AddNewModel.StackNo && itm.Receipt_No == $scope.AddNewModel.WRNo) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else if ($scope.AddNewModel.Godown_Id != '0' && $scope.AddNewModel.StackNo != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString() && itm.StackNo.toString() == $scope.AddNewModel.StackNo) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else if ($scope.AddNewModel.Godown_Id != '0' && $scope.AddNewModel.WRNo != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString() && itm.Receipt_No.toString() == $scope.AddNewModel.WRNo) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else if ($scope.AddNewModel.StackNo != '0' && $scope.AddNewModel.WRNo != '0') {
                if (itm.StackNo.toString() == $scope.AddNewModel.StackNo.toString() && itm.Receipt_No.toString() == $scope.AddNewModel.WRNo) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else if ($scope.AddNewModel.Godown_Id != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString()) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else if ($scope.AddNewModel.StackNo != '0') {
                if (itm.StackNo.toString() == $scope.AddNewModel.StackNo.toString()) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else if ($scope.AddNewModel.WRNo != '0') {
                if (itm.Receipt_No.toString() == $scope.AddNewModel.WRNo.toString()) {
                    itm.IsShow = true;  //$scope.RSWCList.push(itm);
                } else {
                    itm.IsShow = false;
                }
            } else {
                itm.IsShow = true;   //$scope.RSWCList.push(itm);
            }
        });

        $(".blockUI").fadeOut("slow");
    };


    $scope.GetFilterWarehouseDetailsList123 = function () {
        $(".blockUI").fadeIn("slow");
        $scope.RSWCList = [];
        $scope.RSWCMasterList.forEach(function (itm, index) {
            if ($scope.AddNewModel.Godown_Id != '0' && $scope.AddNewModel.StackNo != '0' && $scope.AddNewModel.WRNo != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString() && itm.StackNo.toString() == $scope.AddNewModel.StackNo && itm.Receipt_No == $scope.AddNewModel.WRNo) {
                    $scope.RSWCList.push(itm);
                }
            } else if ($scope.AddNewModel.Godown_Id != '0' && $scope.AddNewModel.StackNo != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString() && itm.StackNo.toString() == $scope.AddNewModel.StackNo) {
                    $scope.RSWCList.push(itm);
                }
            } else if ($scope.AddNewModel.Godown_Id != '0' && $scope.AddNewModel.WRNo != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString() && itm.Receipt_No.toString() == $scope.AddNewModel.WRNo) {
                    $scope.RSWCList.push(itm);
                }
            } else if ($scope.AddNewModel.StackNo != '0' && $scope.AddNewModel.WRNo != '0') {
                if (itm.StackNo.toString() == $scope.AddNewModel.StackNo.toString() && itm.Receipt_No.toString() == $scope.AddNewModel.WRNo) {
                    $scope.RSWCList.push(itm);
                }
            } else if ($scope.AddNewModel.Godown_Id != '0') {
                if (itm.Godown_Id.toString() == $scope.AddNewModel.Godown_Id.toString()) {
                    $scope.RSWCList.push(itm);
                }
            } else if ($scope.AddNewModel.StackNo != '0') {
                if (itm.StackNo.toString() == $scope.AddNewModel.StackNo.toString()) {
                    $scope.RSWCList.push(itm);
                }
            } else if ($scope.AddNewModel.WRNo != '0') {
                if (itm.Receipt_No.toString() == $scope.AddNewModel.WRNo.toString()) {
                    $scope.RSWCList.push(itm);
                }
            } else {
                $scope.RSWCList.push(itm);
            }
        });

        $(".blockUI").fadeOut("slow");
    };


    $scope.GetWarehouseDetailsList_bac = function (th) {
        $(".blockUI").fadeIn("slow");
        $scope.OTP = "";
        $scope.IsValida = false;
        $scope.ReleasedNofQty = th.release_quantity;
        $scope.ReceiverAadharNo = th.receiver_aadhar_no;
        $scope.AddNewModel.ReceiverAadharNo = "XXXXXXXX" + th.receiver_aadhar_no.substring(8, 13);
        $scope.ReceiverName = th.receiver_name;
        $scope.ReceiverFName = th.receiver_fname;
        $scope.IsDisabledEditQty = true;
        $scope.Order_Id = th.release_order_id;
        $scope.Receipt_No = th.receipt_number;
        $scope.TotalPackages_Lots = 0
        $scope.CompanyName = th.CompanyName;
        $scope.Address = th.Address;
        $scope.MobileNo = th.MobileNo;
        $scope.EmailId = th.Email;
        return $http.post(SiteUrl + "/FertilizerSupply/GetWarehouseRSWCReceiptDetails", { ReceiptNo: th.receipt_no }).
            success(function (data, status) {
                var data = JSON.parse(data.DataList);
                $scope.RSWCList = [];
                
                var totalqty = 0;
                data.forEach(function (item, index) {
                    $scope.ResetAddNewModel();
                    $scope.TotalPackages_Lots = item.packages_lots
                    $scope.AddNewModel.Receipt_No = item.receipt_no;
                    $scope.AddNewModel.TotalPackages_Lots = item.packages_lots;
                    $scope.AddNewModel.Packages_Quantity_Unit = item.packages_quantity_unit.toString() + ($scope._language == 'en-us' ? " KG" : " किलोग्राम");
                    $scope.AddNewModel.ReleasedPackagesLots = item.ReleasedPackagesLots;
                    $scope.AddNewModel.Packages_Lots = data.length == 1 ? $scope.ReleasedNofQty : item.packages_lots;
                    $scope.AddNewModel.Id = item.id;
                    $scope.TotalPackages += item.packages_lots;
                    $scope.ReleasedPackages += item.ReleasedPackagesLots;
                    $scope.AddNewModel.LoanClosedDate = item.Loan_closed;
                    $scope.AddNewModel.LoanStartDate = item.Loan_start;
                    //$scope.AddNewModel.Order_Id = item.release_order_id;
                    if (item.Loan_start == '') {
                        $scope.IsValida = true
                    } else if (item.Loan_start != '' && item.Loan_closed != '') {
                        $scope.IsValida = true
                    } else {
                        $scope.IsValida = false;
                    }
                    $scope.AddNewModel.ReceiverAadharNo = "XXXXXXXX" + th.receiver_aadhar_no.substring(8, 13);
                    var list = $scope.ItemList.find(function (x) { return x.CommodityId.toString() === item.item_kind.toString() });
                    $scope.CommodityName = _language == 'en-us' ? list.CommodityNameEn : list.CommodityNameHi;
                    $scope.AddNewModel.IsDisabled = true;
                    totalqty += item.packages_lots;
                    $scope.RSWCList.push($scope.AddNewModel);
                });
                if ($scope.RSWCList.length > 1 && $scope.ReleasedNofQty < totalqty) {
                    $scope.IsDisabledEditQty = false;
                    $scope.RSWCList.forEach(function (item, index) {
                        item.Packages_Lots = "";
                    });
                }
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.RSWCList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.UnitList = [];
    $scope.UnitList = [{ UnitId: "35", UnitNameEng: "35 KG", UnitNameHi: "30 किलोग्राम" }, { UnitId: "50", UnitNameEng: "50 KG", UnitNameHi: "50 किलोग्राम" }];

    $scope.GenerateRequest = function () {
        if ($scope.ValidateReleaseDATA()) {
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
    $scope.isValidAadharNo = false;
    $scope.VerifyOTP = function () {
        
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        //if ($scope.OTP != '') {
        //if ($scope.ValidateData()) {
            $(".se-pre-con").fadeIn("slow");


            var formData = new FormData();

        var reqestData = [];
        $scope.RSWCList.filter(function (x) { return x.isPreview }).forEach(function (item, index) {


            var netqty = 0;
            $scope.RSWCMasterList.filter(function (x) { return x.Receipt_No === item.Receipt_No }).forEach(function (itm, id) {
                netqty += itm.Net_Quantity;
            });

                reqestData.push({
                    Id: item.Id,
                    Receipt_No: item.Receipt_No,
                    OldReceipt_No: $scope.AddNewModel.OldReceipt_No,
                    Wharehouse_Id: $scope.AddNewModel.Wharehouse_Id,
                    Lisence_No: $scope.AddNewModel.Lisence_No,
                    Received_From_District: $scope.AddNewModel.DistrictId,
                    Received_From_Address: $scope.AddNewModel.Received_From_Address,
                    Item_Kind: $scope.AddNewModel.Item_Kind,
                    Quality: item.Quality,
                    Packages_Lots: item.TotalPackages_Lots,
                    Packages_Quantity: item.Packages_Quantity,
                    Packages_Quantity_Unit: item.Packages_Quantity_Unit,
                    Godown_Id: item.Godown_Id,
                    Stack_No: item.Stack_No,
                    Valid_From_Date: $("#Valid_From_Date").val(),
                    Valid_To_Date: $("#Valid_To_Date").val(),
                    NetQuantity: netqty,//item.Net_Quantity,
                    QuantityNote: item.QuantityNote,
                    PurchaseCenterId: $scope.AddNewModel.PurchaseCenter_Id,
                    ReceivedFromKVSS: $scope.AddNewModel.KVSSId,
                    SamplerName: item.NameOfSampler,
                    ConditionOfGoods: $scope.AddNewModel.ConditionOfGoods,
                    ReleaseQuantity: item.Quantity,
                    ReleasedQuantity: item.ReleasedPackagesLots,
                    PrivateMarkofDepositor: $scope.AddNewModel.PrivateMarkofDepositor,
                    StorageRateCharge: $scope.AddNewModel.StorageRateCharge,
                    IndemnifiedInsurance: $scope.AddNewModel.IndemnifiedInsurance,
                    InsuredName: $scope.AddNewModel.InsuredName,
                    Order_Id: $scope.Order_Id,
                    QuantityLossGain: item.QuantityGain == "" ? (item.QuantityLoss == "" ? "0" :"-"+ item.QuantityLoss) : item.QuantityGain,                    
                    ReleasedPackages_Lots: item.Packages_Lots,
                    ReleaseOrderId: item.ReleaseOrderId
                });
            });

            //formData.append("ReqestData", ReqestData);
            //formData.append("OTP", $scope.OTP);
            //formData.append("TXN", $scope.Certtrn);
            var functionname = "DOCommodityReleased";//_language == 'en-us' ? "GenerateWarehouseRSWCEng" : "GenerateWarehouseRSWCHi";//"GenerateWarehouseRSWCHi";//
        $http.post(SiteUrl + "/FertilizerSupply/" + functionname, { OTP: $scope.OTP, TXN: $scope.Certtrn, DataList: reqestData }).
                success(function (data, status) {
                    
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'Order successfully released' : "आदेश सफलतापूर्वक जारी हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/CommodityReleased';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "PDF Not Generate") {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be released. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
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
       // }
       // else {
            //ShowOkMessage("Alert", _language == 'en-us' ? "Please enter OTP." : "कृपया OTP दर्ज करें।", function () { return false; });
       // }
        $scope.showLoader = false;
    };


    $scope.ValidateData = function () {
        
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");



        $(".validateNewAdd").removeClass("notValid");

        if ($("#Valid_From_Date").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select release date ' : '\n <br /> कृपया रिलीज की तारीख चुनें |';
            $("#Valid_From_Date").addClass("notValid");
        }


        var quantity = 0;

        $scope.RSWCList.forEach(function (item, index) {
            if (item.Packages_Lots == '' && item.TotalPackages_Lots > item.ReleasedPackagesLots) {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.PackagesLots + '' : '\n <br /> कृपया ' + $scope.translation.PackagesLots + ' दर्ज करें |';
                $("#Packages_Lots_" + index).addClass("notValid");
            }
            if (item.Packages_Lots != '' && item.Packages_Lots > (item.TotalPackages_Lots - item.ReleasedPackagesLots)) {
                message += $scope._language == 'en-us' ? '\n <br /> Entered quantity is greter then total remaining quantity. Please enter correct value ' : '\n <br /> दर्ज की गई मात्रा कुल शेष मात्रा से अधिक है। कृपया सही मान दर्ज करें |';
                $("#Packages_Lots_" + index).addClass("notValid");
                quantity += item.Packages_Lots;
            }
            if (item.Packages_Lots != '') {
                quantity += parseInt(item.Packages_Lots);
            }

        });
        if (quantity > $scope.ReleasedNofQty) {
            message += $scope._language == 'en-us' ? '\n <br /> Entered quantity is greter then total allowed quantity. Please enter correct value ' : '\n <br /> दर्ज की गई मात्रा कुल अनुमत मात्रा से अधिक है। कृपया सही मान दर्ज करें |';
        } else if (quantity < $scope.ReleasedNofQty) {
            message += $scope._language == 'en-us' ? '\n <br /> Entered quantity is less then total allowed quantity. Please enter eqal value ' : '\n <br /> दर्ज की गई मात्रा कुल अनुमत मात्रा से अधिक है। कृपया सही मान दर्ज करें |';
        }



        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }

    $scope.otpModel = {
        IsChecked: false
    }
    $scope.otpModel1 = {
        IsChecked: false
    }
    $scope._EncryptedAadharNo = "";
    $scope.SendAadharOTP = function () {


        if (!$scope.otpModel.IsChecked) {
            ShowOkMessage("", _language == 'en-us' ? 'Please allow system for your AADHAAR based Authentication/ eKYC.\n' : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।|\n', function () {
            });
            return;
        }

        //if ($scope.ValidateData()) {
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



        $http.post(SiteUrl + "/FertilizerSupply/SendOTPOnAADHAR", { _AadharNo: $scope.ReceiverAadharNo }).
            success(function (data, status) {
                
                $(".se-pre-con").fadeOut("slow");
                if (data.Result == false) {
                    ShowOkMessage("", _language == 'en-us' ? "Invalid aaddhaar number or something else. Please try after some time." : "अमान्य आधार संख्या या कुछ और। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    // window.location = '../Election/ElectionSocietyList';
                } else {


                    $scope.EncryptedAadharNo = data.AadharNo;
                    $scope.Certtrn = data.txnid;
                    $('#myOTPModal1').modal("show");
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

        // }
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
                        $('#myOTPModal1').modal("hide");
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
    $scope.OrderList=[]
    $scope.SelectAll = function () {
        if ($("#chk_all").is(":checked")) {
            $scope.RSWCList.forEach(function (itm, index) {
                itm.IsChecked = true;
                $scope.OrderList.push(itm);
            });
        } else {
            $scope.RSWCList.forEach(function (itm, index) {
                itm.IsChecked = false;
            });
            $scope.OrderList = [];
        }
    }

    $scope.SelectWR = function (th) {
       
            if (th.IsChecked) {
                $scope.OrderList.push(th);
            } else {
                var index = $scope.OrderList.indexOf(th);
                $scope.OrderList.splice(index, 1);
            }
       
    }

    

    $scope.SetLossGain = function (th,index) {
        th.QuantityGain = "";
        th.QuantityLoss = "";
        $("#Packages_Lots_" + index).removeClass("notValid");
        if (parseInt(th.Packages_Lots) > (parseInt(th.TotalPackages_Lots) - parseInt(th.ReleasedPackagesLots))) {
            ShowOkMessage("Warning", $scope._language == 'en-us' ? "Entered No. of Packages or Lots must be less than or eqial to Remaining No. of Packages or Lots" : "दर्ज बंडलों की संख्या, शेष बंडलों की संख्या से कम या बराबर होनी चाहिए", function () { });
            $("#Packages_Lots_" + index).addClass("notValid")
        } else if (parseInt(th.Packages_Lots) == (parseInt(th.TotalPackages_Lots) - parseInt(th.ReleasedPackagesLots))) {
            if (parseFloat(th.Net_Quantity) > (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))) {
                th.QuantityLoss = (parseFloat(th.Net_Quantity) - (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))).toFixed(2);
            } if (parseFloat(th.Net_Quantity) < (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))) {
                th.QuantityGain = ((parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity)) - parseFloat(th.Net_Quantity)).toFixed(2);
            }
        }
    }

    
    $scope.NextToShow = function () {
        $scope.OrderList = []
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var totalNetQty = 0;
        var isSelectedWR = false;
        $scope.RSWCList.forEach(function (th, index) {
            $("#Packages_Lots_" + index).removeClass("notValid");
            $("#ReleaseQuantity_" + index).removeClass("notValid");
            if (th.IsChecked) {
                th.isPreview = true;
                th.IsShow = true;
                $scope.OrderList.push(th);
                isSelectedWR = true;
                th.QuantityGain = "";
                th.QuantityLoss = "";
                totalNetQty += parseFloat(th.Quantity)
              

                if (th.Packages_Lots == "" || th.Packages_Lots == undefined || parseInt(th.Packages_Lots) == 0) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter  ' + $scope.translation.PackagesLots + '' : 'कृपया ' + $scope.translation.PackagesLots + ' दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.PackagesLots + '' : '\n <br /> कृपया ' + $scope.translation.PackagesLots + ' दर्ज करें |';
                    }
                    $("#Packages_Lots_" + index).addClass("notValid")
                }
                if (th.Quantity == "" || th.Quantity == undefined || parseFloat(th.Quantity) == 0) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter  ' + $scope.translation.ReleaseQuantity + '' : 'कृपया ' + $scope.translation.ReleaseQuantity + ' दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.ReleaseQuantity + '' : '\n <br /> कृपया ' + $scope.translation.ReleaseQuantity + ' दर्ज करें |';

                    }
                    $("#ReleaseQuantity_" + index).addClass("notValid")
                }
                if (parseInt(th.Packages_Lots) > (parseInt(th.TotalPackages_Lots) - parseInt(th.ReleasedPackagesLots))) {
                    message += $scope._language == 'en-us' ? "Entered No. of Packages or Lots must be less than or eqial to Remaining No. of Packages or Lots" : "दर्ज बंडलों की संख्या, शेष बंडलों की संख्या से कम या बराबर होनी चाहिए";
                    $("#Packages_Lots_" + index).addClass("notValid")
                } else if (parseInt(th.Packages_Lots) == (parseInt(th.TotalPackages_Lots) - parseInt(th.ReleasedPackagesLots))) {
                    if (parseFloat(th.Net_Quantity) > (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))) {
                        th.QuantityLoss = (parseFloat(th.Net_Quantity) - (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))).toFixed(2);
                    } if (parseFloat(th.Net_Quantity) < (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))) {
                        th.QuantityGain = ((parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity)) - parseFloat(th.Net_Quantity)).toFixed(2);
                    }
                }
            }
        });


        //if (!isSelectedWR) {
        //    message += $scope._language == 'en-us' ? "Please select at least one WR for proceed." : "कुल दर्ज रिलीज मात्रा " + TotalNetQty + "कृपया आगे बढ़ने के लिए कम से कम एक WR चुनें।";
        //}

        //if (totalNetQty > parseFloat( $scope.ReleasedNofQty)) {
        //    message += $scope._language == 'en-us' ? "Total Entered Release Quantity " + totalNetQty + " is greater than the Allowed Quantity " + $scope.ReleasedNofQty + ". Please enter equal quantity of release order quantity." : "कुल दर्ज रिलीज मात्रा " + TotalNetQty + " अनुमति मात्रा " + $scope.ReleasedNofQty +" से अधिक है। कृपया रिलीज ऑर्डर मात्रा के बराबर मात्रा दर्ज करें।";
        //}

        //if (message.length > 29) {
        //    ShowModelAlert("Warning", message);
        //    return false;
        //} else {
        //    $scope.isPreview = true;
        //}   
        $scope.CalculateTotalValues();
        $scope.isPreview = true;
    }



    $scope.ValidateReleaseDATA = function () {
        $scope.OrderList = []
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var totalNetQty = 0;
        var isSelectedWR = false;
        $scope.RSWCList.forEach(function (th, index) {
            $("#Packages_Lots_" + index).removeClass("notValid");
            $("#ReleaseQuantity_" + index).removeClass("notValid");
            if (th.IsChecked) {
                th.isPreview = true;
                th.IsShow = true;
                $scope.OrderList.push(th);
                isSelectedWR = true;
                th.QuantityGain = "";
                th.QuantityLoss = "";
                totalNetQty += parseFloat(th.Quantity)
                if (th.Packages_Lots == "" || th.Packages_Lots == undefined || parseInt(th.Packages_Lots) == 0) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter  ' + $scope.translation.PackagesLots + '' : 'कृपया ' + $scope.translation.PackagesLots + ' दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.PackagesLots + '' : '\n <br /> कृपया ' + $scope.translation.PackagesLots + ' दर्ज करें |';
                    }
                    $("#Packages_Lots_" + index).addClass("notValid")
                }
                if (th.Quantity == "" || th.Quantity == undefined || parseFloat(th.Quantity) == 0) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter  ' + $scope.translation.ReleaseQuantity + '' : 'कृपया ' + $scope.translation.ReleaseQuantity + ' दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please enter  ' + $scope.translation.ReleaseQuantity + '' : '\n <br /> कृपया ' + $scope.translation.ReleaseQuantity + ' दर्ज करें |';

                    }
                    $("#ReleaseQuantity_" + index).addClass("notValid")
                }
                if (parseInt(th.Packages_Lots) > (parseInt(th.TotalPackages_Lots) - parseInt(th.ReleasedPackagesLots))) {
                    message += $scope._language == 'en-us' ? "Entered No. of Packages or Lots must be less than or eqial to Remaining No. of Packages or Lots" : "दर्ज बंडलों की संख्या, शेष बंडलों की संख्या से कम या बराबर होनी चाहिए";
                    $("#Packages_Lots_" + index).addClass("notValid")
                } else if (parseInt(th.Packages_Lots) == (parseInt(th.TotalPackages_Lots) - parseInt(th.ReleasedPackagesLots))) {
                    if (parseFloat(th.Net_Quantity) > (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))) {
                        th.QuantityLoss = (parseFloat(th.Net_Quantity) - (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))).toFixed(2);
                    } if (parseFloat(th.Net_Quantity) < (parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity))) {
                        th.QuantityGain = ((parseFloat(th.ReleasedQuantity) + parseFloat(th.Quantity)) - parseFloat(th.Net_Quantity)).toFixed(2);
                    }
                }
            }
        });


        if (!isSelectedWR) {
            message += $scope._language == 'en-us' ? "Please select at least one WR for proceed." : "कुल दर्ज रिलीज मात्रा " + TotalNetQty + "कृपया आगे बढ़ने के लिए कम से कम एक WR चुनें।";
        }

        if (totalNetQty > parseFloat( $scope.ReleasedNofQty)) {
            message += $scope._language == 'en-us' ? "Total Entered Release Quantity " + totalNetQty + " is greater than the Allowed Quantity " + $scope.ReleasedNofQty + ". Please enter equal quantity of release order quantity." : "कुल दर्ज रिलीज मात्रा " + TotalNetQty + " अनुमति मात्रा " + $scope.ReleasedNofQty +" से अधिक है। कृपया रिलीज ऑर्डर मात्रा के बराबर मात्रा दर्ज करें।";
        }

        if (message.length > 29) {
            ShowModelAlert("Warning", message);
            return false;
        } else {
            return true;
        }   
        $scope.CalculateTotalValues();
        $scope.isPreview = true;
    }


    $scope.TotalSelectedBAGS = 0;
    $scope.TotalSelectedQTY = 0;
    $scope.TotalSelectedGAIN = 0;
    $scope.TotalSelectedLOSS = 0;


    $scope.CalculateTotalValues = function(){
        $scope.TotalSelectedBAGS = 0;
        $scope.TotalSelectedQTY = 0;
        $scope.TotalSelectedGAIN = 0;
        $scope.TotalSelectedLOSS = 0;

        var TotalSelectedBAGS = 0;
        var TotalSelectedQTY = 0;
        var TotalSelectedGAIN = 0;
        var TotalSelectedLOSS = 0;

        $scope.RSWCList.filter(function (x) { return x.isPreview }).forEach(function (item, i) {
            TotalSelectedBAGS += parseInt(item.Packages_Lots == '' || item.Packages_Lots == undefined ? 0 : item.Packages_Lots);
            TotalSelectedQTY += parseFloat(item.Quantity == '' || item.Quantity == undefined ? 0 : item.Quantity);
            TotalSelectedGAIN += parseFloat(item.QuantityGain == '' || item.QuantityGain == undefined ? 0 : item.QuantityGain);
            TotalSelectedLOSS += parseFloat(item.QuantityLoss == '' || item.QuantityLoss == undefined ? 0 : item.QuantityLoss);

            $scope.TotalSelectedBAGS = TotalSelectedBAGS;
            $scope.TotalSelectedQTY = parseFloat(TotalSelectedQTY).toFixed(2);
            $scope.TotalSelectedGAIN = parseFloat(TotalSelectedGAIN).toFixed(2);
            $scope.TotalSelectedLOSS = parseFloat(TotalSelectedLOSS).toFixed(2);
        });
    }

    $scope.BackToShow = function () {
        $scope.isPreview = false;
        $scope.RSWCList.forEach(function (th, index) {
            th.isPreview = false;
        });
    }
});