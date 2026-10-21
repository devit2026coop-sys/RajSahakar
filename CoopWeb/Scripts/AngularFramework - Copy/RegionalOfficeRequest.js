var AppClasses = angular.module("GSSDemandMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("GSSDemandCnt", function ($scope, $http, $rootScope) {


    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
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
        if ($scope.Filter.FilterType == 'created_dated' || $scope.Filter.FilterType == 'request_date') {
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
        $scope.KVSSRequestList();
    }
    /*------------End Paging and Filter-----------------*/







    $scope._language = 'en-us';

    $scope.SocietyList = [];
    $scope._language = _language;
    $scope.TodayDate = "";
    $scope.MasterModel = {
        SocietyID: "0",
        Quantity: "",
        FertilizerId: "0",
        RoutChart: ""
    }
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

    $scope.RequestType = "New";

    $scope.SelectRequestType = function (RequestType) {
        $scope.RequestType = RequestType;
        $scope.ClearSearch();
    }

    $scope.DemandList = [];
    $scope.KVSSRequestList = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/FertilizerSupply/GetKVSSData", JSON.stringify({ Details: jsonParameters, RequestType: $scope.RequestType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.DemandList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
                $scope.DemandList.forEach(function (item, index) {
                    $scope.OrderList.forEach(function (itm, indx) {
                        if (item.demand_id == itm.demand_id) {
                            item.IsChecked = true;
                        }
                    });
                });

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.OrderList = [];
    $scope.OrganisationsDetails = [];
    $scope.GetOrganisationsDetails = function () {
        //$(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/FertilizerSupply/GetAllOrganisations").
            success(function (data, status) {
                //$(".blockUI").fadeOut("slow");
                $scope.OrganisationsDetails = data;
            }).
            error(function (data, status) {
                $scope.OrganisationsDetails = [];
                //$(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetOrganisationsDetails();
    $scope.GetSocietyList = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetSocietyList", { "WhereClause": " " }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyList = data;
                if ($scope.SocietyList.length == 1) {
                    $scope.MasterModel.SocietyID = $scope.SocietyList[0].SocietyID.toString();
                }
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };

    $scope.KVSSRequestList().then(function () {
        //$scope.GetSocietyList();
    });


    $scope.AddNew = function () {
        $("#addModal").modal("show");
    }
    $scope.FertilizerList = [];
    $scope.GetFertilizerList = function () {

        return $http.post(SiteUrl + "/FertilizerSupply/GetFertilizerList", { "WhereClause": " " }).
            success(function (data, status) {
                //  $(".blockUI").fadeOut("slow");
                $scope.FertilizerList = data;
                if ($scope.FertilizerList.length == 1) {
                    $scope.MasterModel.FertilizerId = $scope.FertilizerList[0].fertilizer_id.toString();
                    $scope.MasterModel.Unit = $scope._language == 'en-us' ? $scope.FertilizerList[0].unit_en : $scope.FertilizerList[0].unit_hi;
                }
            }).
            error(function (data, status) {
                $scope.FertilizerList = [];
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetFertilizerList();

    $scope.GSSList = []
    $scope.GetGSSList = function () {
        // $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/FertilizerSupply/GetGSSBySSOId").
            success(function (data, status) {
                // $(".blockUI").fadeOut("slow");
                $scope.GSSList = [];
                $scope.GSSList = data;
            }).
            error(function (data, status) {
                $scope.GSSList = [];
                //$(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };
    $scope.GetGSSList();



    $scope.ResetAddNew = function () {
        $scope.MasterModel.SocietyID = "0";
        $scope.MasterModel.Quantity = "";
    }
    $scope.ShowGenarateOrder = false;
    $scope.TotalOrderQty = 0;
    $scope.SelectDemand = function (th) {
        if ($scope.DemandList.filter(x => x.IsChecked === true).length > 0) {
            $scope.ShowGenarateOrder = true;
        } else {
            $scope.ShowGenarateOrder = false;
        }

        $scope.TotalOrderQty = 0;
        if (th.IsChecked) {
            $scope.OrderList.push(th);
        } else {
            //$scope.OrderList.pop(th);
            var index = $scope.OrderList.indexOf(th);
            $scope.OrderList.splice(index, 1);
        }
        $scope.OrderList.forEach(function (itm, indx) {
            $scope.TotalOrderQty = $scope.TotalOrderQty + itm.quantity;
        });

    }
    $scope.ShowOrderDetails = false;
    $scope.GenerateOrder = function () {
        $scope.ShowOrderDetails = true;
    }

    $scope.GenerateRequestBack = function () {
        $scope.ShowOrderDetails = false;
    }

    $scope.GetCurrentDate = function () {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        $scope.TodayDate = dd + '/' + mm + '/' + yyyy;
        //return today;
    }
    $scope.GetCurrentDate();



    $scope.OTP = "";
    $scope.Certtrn = "";
    $scope.CertSocietyIdEnc = "";
    $scope.CertElectionMasterId = "";
    $scope.CertIndex = "";
    $scope.CertMemberId = "";




    $scope.GenerateRequest = function () {
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
                debugger;
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

    $scope.VerifyOTP = function () {
        debugger;
      //  $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
       if ($scope.OTP != '') {

        //ShowAlertMessage("Alert", _language == 'en-us' ? "Are you want to submit request. ?" : "क्या आप अनुरोध सबमिट करना चाहते हैं ?", function () {

            $(".se-pre-con").fadeIn("slow");


            var formData = new FormData();

            var reqestData = [];
            $scope.OrderList.forEach(function (itm, index) {
                reqestData.push({
                    RequestId: itm.request_no,
                    DemandId: itm.demand_id,
                    ROName: itm.RONameHn,
                    FertilizerName: itm.fertilizer_name_hi,
                    KVSSName: itm.SocietyName,
                    Quantity: itm.quantity
                });
            });

            //formData.append("ReqestData", ReqestData);
            //formData.append("OTP", $scope.OTP);
            //formData.append("TXN", $scope.Certtrn);

            $http.post(SiteUrl + "/FertilizerSupply/SentRORequest", { OTP: $scope.OTP, TXN: $scope.Certtrn, ReqestData: reqestData }).
                success(function (data, status) {
                    debugger;
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'Request successfully submited' : "अनुरोध  सफलतापूर्वक सबमिट हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../FertilizerSupply/RegionalOfficeRequest';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else if (data == "PDF Not Generate") {
                        ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
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
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });

                });
            }
             else {
                ShowOkMessage("Alert", _language == 'en-us' ? "Please enter OTP." : "कृपया OTP दर्ज करें।", function () { });
            }
            
    };

 



    $scope.SaveDemand = function () {
        $("#SocietyID").removeClass("notValid");
        $("#FertilizerId").removeClass("notValid");
        $("#Quantity").removeClass("notValid");
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.SocietyID == "0") {
            message += $scope._language == 'en-us' ? '\n <br />Please Choose Society.' : '\n <br />कृपया सोसायटी चुनें।';
            $("#SocietyID").addClass("notValid");
        }
        if ($scope.MasterModel.FertilizerId == "0") {
            message += $scope._language == 'en-us' ? '\n <br />Please Choose Fertilizer.' : '\n <br />कृपया उर्वरक चुनें।';
            $("#FertilizerId").addClass("notValid");
        }
        if ($scope.MasterModel.Quantity == "") {
            message += $scope._language == 'en-us' ? '\n <br />Please Enter Quantity.' : '\n <br />कृपया मात्रा दर्ज करें।';
            $("#Quantity").addClass("notValid");
        }


        if (message.length > 29) {
            ShowModelAlert("GSS Demand", message);
            return false;
        } else {

            $(".blockUI").fadeIn("slow");
            var data = {
                SocietyId: $scope.MasterModel.SocietyID,
                Quantity: $scope.MasterModel.Quantity,
                FertilizerId: $scope.MasterModel.FertilizerId,
                RequestId: 1,
                RoutChart: $scope.MasterModel.RoutChart
            }
            $http.post(SiteUrl + "/FertilizerSupply/GSSDemand", data).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $("#addModal").modal("show");
                    $scope.ResetAddNew();
                    if (data.Status > 0) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Demand Successfully submited" : "मांग सफलतापूर्वक सबमिट की गई", function () {
                            window.location = "/FertilizerSupply/KVSSRequest";
                        });
                    } else {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });

                    }
                }).
                error(function (data, status) {

                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });

                    //alert('Error while fetching society list data.');
                });
        }
    };











    $scope.ResetAddNew = function () {
        $scope.MasterModel.SocietyID = "0";
        $scope.MasterModel.Quantity = "";
        $scope.MasterModel.FertilizerId = "0";
        $scope.MasterModel.RoutChart = "";
    }

    $scope.ChangeFertilizer = function () {

        if ($scope.MasterModel.FertilizerId == "0") {
            $scope.MasterModel.Unit = "";
        } else if ($scope._language == 'en-us') {
            $scope.MasterModel.Unit = $scope.FertilizerList.filter(x => x.fertilizer_id.toString() == $scope.MasterModel.FertilizerId.toString())[0].unit_en;
        } else {
            $scope.MasterModel.Unit = $scope.FertilizerList.filter(x => x.fertilizer_id.toString() == $scope.MasterModel.FertilizerId.toString())[0].unit_hi;
        }
        // $scope.MasterModel.Unit = $scope._language == 'en-us' ? $scope.FertilizerList[0].unit_en : $scope.FertilizerList[0].unit_hi;         
    }

});