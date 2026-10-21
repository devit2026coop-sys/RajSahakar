var AppClasses = angular.module("CCSComplainMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("CCSComplainCnt", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 300,
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



    $scope.LetterData = {
        ComplaintId: "",
        Date: "",
        Time: "",
        Place: "",
        CEOName: "",
        CEOMobile: "",
        complainant_letter: "",
        ceo_letter: "",
        ComplainerName: "",
        MobileNo: "",
        SocietyName:""
    }


    $scope.CCSComplaintList = [];
    $scope.GetCCSComplaint = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (status) {
            if (status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
                return $http.post(SiteUrl + "/Society/GetCCSComplaintList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];
                        $scope.CCSComplaintList = JSON.parse(data.DataList);
                        $rootScope.DataPagingDetails = data.DataPaging;
                        $rootScope.SetDataPaging();
                        $(".blockUI").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $scope.CCSComplaintList = [];
                        $(".blockUI").fadeOut("slow");
                        //alert('Error while fetching society list data.');
                    });
            }

        });


    };
    $scope.GetCCSComplaint();

    $scope.ShowDetail = function (item) {
        $scope.LetterData = {
            ComplaintId: item.ComplaintId,
            Date: "",
            ccsDate: "",
            Time: "",
            Place: "",
            CEOName: item.CEOName,
            CEOMobile: item.CEOMobile,
            complainant_letter: "",
            ceo_letter: "",
            ComplainerName: item.ComplainerName,
            MobileNo: item.MobileNo,
            SocietyName: item.SocietyName
        }
        $("#addModal").modal("show");
    }

    $scope.Certtrn = "";
    $scope.OTP = "";

    $scope.SendOTP = function () {

        if ($scope.ValidateData()) {
            //if (!$scope.otpModel.IsChecked) {
            //    ShowOkMessage("", _language == 'en-us' ? 'Please allow system for your AADHAAR based Authentication/ eKYC.\n' : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।|\n', function () {
            //    });
            //    return;
            //}

            $http.get(SiteUrl + "/login/CheckUserSession").then(function (status) {
                if (status == false) {
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                } else {
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
            });
        }

    }

    $scope.GenerateLetter = function () {
        debugger;

        if ($scope.OTP != '') {

            $http.get(SiteUrl + "/login/CheckUserSession").then(function (status) {
                if (status == false) {
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/"; });
                } else {
                    $(".se-pre-con").fadeIn("slow");
                    var formData = new FormData();
                    var reqestData = [];
                    $scope.LetterData.Date = $("#From_Date").val() + " " + $scope.LetterData.Time;

                    $http.post(SiteUrl + "/Society/GenerateCCSLetter", { OTP: $scope.OTP, TXN: $scope.Certtrn, LetterDetails: $scope.LetterData }).
                        success(function (data, status) {
                            debugger;
                            $(".se-pre-con").fadeOut("slow");
                            //$('#myOTPModal').modal("hide");
                            if (data == "Success") {
                                ShowOkMessage("", _language == 'en-us' ? 'Complaint Letter successfully generated' : "आदेश सफलतापूर्वक जारी हुआ", function () {
                                    $('#myOTPModal').modal("hide");
                                    window.location = '../Society/CCSComplaint';
                                });
                                //window.location = '../Election/ElectionSocietyList';
                            } else if (data == "PDF Not Generate") {
                                ShowOkMessage("", _language == 'en-us' ? "Complaint Letter could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
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
                            ShowOkMessage("", _language == 'en-us' ? "Complaint Letter could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                            //return false;
                        });
                }
            })
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter OTP." : "कृपया OTP दर्ज करें।", function () { return false; });
        }



    };


    $scope.ValidateData = function () {
        debugger;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");
        $(".validateNewAdd").removeClass("notValid");
        if ($("#From_Date").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  date' : '\n <br />  Please select  date';
            $("#From_Date").addClass("notValid");
        }

        if ($("#From_Time").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  Time' : '\n <br /> Please select  Time';
            $("#From_Time").addClass("notValid");
        }

        if ($scope.LetterData.Place == '' || $scope.LetterData.Place == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  place' : '\n <br /> Please enter  place';
            $("#Place").addClass("notValid");
        }

        if ($scope.LetterData.CEOName == '' || $scope.LetterData.CEOName == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  CEO Name' : '\n <br /> Please enter  CEO Name';
            $("#CEOName").addClass("notValid");
        }

        if ($scope.LetterData.CEOMobile == '' || $scope.LetterData.CEOMobile == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  CEO Mobile' : '\n <br /> Please enter  CEO Mobile';
            $("#CEOMobile").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }

});