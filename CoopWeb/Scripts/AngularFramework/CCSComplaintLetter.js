var AppClasses = angular.module("CCSComplainMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("CCSComplainCnt", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 700,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $scope.dataPayload = {};
    $scope.esignData = {};
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
    $scope.OnPageLoad = function () {
        //$('#From_Time').timepicker({
        //    showOn: "both",
        //    buttonImage: "../images/Time.png",
        //    buttonImageOnly: true,
        //    showPeriod: true,
        //    showLeadingZero: true
        //});
        $scope.GetCCSListNew();
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 20 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 19;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 19 ? $rootScope.DataPagingDetails.EndPageNumber - 19 : $rootScope.DataPagingDetails.StartPageNumber;
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
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();```1    
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearMenuForm = function () {
        $scope.CCSComplaintList = $scope.NewDt;
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.Filter.CCSId = '0';
        $scope.Filter.Status = '0';
    }
    $scope.ClearSearch = function () {
        $scope.Filter.CCSId = '0';
        $scope.Filter.Status = '0';
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.CCSComplaintList = $scope.NewDt;
        //$scope.CCSComplaintList();
    }
    /*------------End Paging and Filter-----------------*/


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
        SocietyName: "",
        ReceiptNo: ""
    }
    $scope.GetFilterbyAction = function (Aid) {
        $scope.CCSComplaintList = $scope.CCSComplaintList.filter(x => x.ActionId == Aid);
    }


    $scope.CCSComplaintList = [];
    $scope.GetCCSComplaint = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
                return $http.post(SiteUrl + "/Society/GetCCSComplaintList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];                      
                        $scope.CCSComplaintList = JSON.parse(data.DataList);
                        $scope.NewDt = $scope.CCSComplaintList;
                        $rootScope.DataPagingDetails = data.DataPaging;
                        $rootScope.SetDataPaging();
                        $(".blockUI").fadeOut("slow");

                        if ($scope.CCSComplaintListSelected.length > 0) {
                            $scope.CCSComplaintListSelected.forEach(function (itm, idx) {
                                if ($scope.CCSComplaintList.filter(function (x) { return x.ComplaintId === itm.ComplaintId }).length > 0) {
                                    $scope.CCSComplaintList.filter(function (x) { return x.ComplaintId === itm.ComplaintId })[0].IsChecked = true;
                                }
                            });
                        }

                        $scope.GetCCSListNew();
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

    $scope.ShowDetail = function (Cid) {
        //$("#addModal").modal("hide");
        //                    AadhaarAuth("", $http, "OTP", true, function (data) {
        //                        $("#addModal").modal("show");                         
        //                        alert(data.TransId);                                
        //                    });

        $scope.Noticechecked = true;

        var item = $scope.CCSComplaintList.filter(x => x.ComplaintId == Cid)[0];
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
            SocietyName: item.SocietyName,
            ReceiptNo: item.ReceiptNo
            //Ischecked: $scope.LetterDataIscheck.Ischecked
        }

        $("#addModal").modal("show");
    }


    $scope.getcompid = function (id) {
        $scope.contactNo = id.MobileNo;
        $scope.ComplaintIds = id;
        $scope.ShowFile = false;
        $scope.DisposeFile = "";
        $scope.LetterData.actionId = 0;
        $scope.LetterData.IsSysytemGen = null;
        $scope.LetterData.descriptiondetails = "";
        $("#OtherNoticeFile").val("");

    }

    $scope.UploadDisposeFile = function (file) {
        $scope.DisposeFile = file[0];

    }
    $scope.SearchType = '0';
    $('input[type=radio][name=rdoSelect]:checked').val(1);
    $scope.ChangeSelction = function () {
        $scope.SearchType = $('input[type=radio][name=rdoSelect]:checked').val();
        $scope.searchRequest($scope.GetCCSComplaint);
    };
    //"complainerId": $scope.ComplaintIds.ComplaintId, "actionId": $scope.Introduction.actionId, "Description": $scope.Introduction.descriptiondetails, "Place": $scope.LetterData.Place, "DateTime": Date

    $scope.SendMSG = function () {
        $http.post("/Society/SendMSGFor").
            success(function (data, status) {
                alert('Post ho gaya');
            });

    }


    $scope.actionval = '';
    $scope.IdData = '';
    $scope.IsnotShow = true;
    $scope.Isnext = true;
    $scope.Isnext2 = true;
    $scope.Isnext3 = true;
    $scope.getcompAlldata = function (id) {
        //$scope.SendMSGForCEOandCitizendata(id);
        $scope.IdData = id;
        $http.post(SiteUrl + "/Society/getAllActionList", { "CompId": id }).
            success(function (data, status) {
                $scope.ActionDetailsdata = data;
                $scope.ActionIddetails = data[0];
                if ($scope.ActionDetailsdata.length > 0) {

                    var i = 1;
                    angular.forEach($scope.ActionDetailsdata, function (value, key) {
                        if (value.ActionId == "3") {
                            if (i == 3) {
                                $scope.Isnext3 = false;
                            }
                            else {
                                $scope.Isnext3 = true;
                            }

                            //$scope.Isnext = false;
                            $scope.Isnext2 = true;
                            $scope.IsnotShow = true;
                            i++;
                        }
                        else if (value.ActionId == "1" || value.ActionId == "2" || value.ActionId == "4") {
                            $scope.Isnext = true;
                            $scope.IsnotShow = false;
                            $scope.Isnext2 = false;
                            $scope.Isnext3 = false;
                        }
                        else {
                            $scope.Isnext = true;
                            $scope.IsnotShow = true;
                            $scope.Isnext2 = true;
                        }
                    });
                }
                else {
                    $scope.Isnext = true;
                    $scope.IsnotShow = true;
                    $scope.Isnext2 = true;
                    $scope.Isnext3 = true;
                }
            }).
            error(function (data, status) {
                alert("Error while fetching list");
            });

    }

    $scope.ShowFile = false;
    $scope.ShowdatetimeDetails = function (id) {
        if (id == 1) {
            // document.getElementById('#NoticeCheck').checked;
            $scope.ShowFile = true;
            $scope.Noticechecked = true;
        }
        if (id == 2) {
            $scope.ShowFile = false;
        }

        else {
            //$scope.ShowdatetimeControl = false;
            $scope.ShowFile = true;
        }

        $scope.actionval = id;
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

            $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                if (SessionStatus.Status == false) {
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
    $scope.showGenratebutton = true;
    $scope.showGenratebuttons = false;
    $scope.Checkboxvalue = function () {
        $scope.showGenratebutton = $scope.CCSComplaintList.filter(function (x) { return x.Ischecked }).length < 9;
        if ($scope.showGenratebutton == true) {
            $scope.showGenratebuttons = true;
        }
        else {
            $scope.showGenratebuttons = true;
        }
    }

    $scope.showcheckbox = function () {
        $scope.truecheckbox = true;

    }

    $scope.GenerateLetter = function () {
        if ($scope.ValidateDataforAction()) {
            if ($scope.LetterData.actionId == 1 && $scope.LetterData.IsSysytemGen == 1) {
                AadhaarAuth("", $http, "", true, function (authData) {
                    $scope.InsertAction(authData.TransId);

                });
            } else {
                $scope.InsertAction("0");
            }
        }


        //if ($scope.OTP != '') {
        //    $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
        //        if (SessionStatus.Status == false) {
        //            ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/"; });
        //        } else {
        //            $(".se-pre-con").fadeIn("slow");
        //            var formData = new FormData();
        //            var reqestData = [];

        //            $scope.LetterData.Date = $("#From_Date").val() + " " + $("#Time_HH").val() + ":" + $("#Time_MM").val() + " " + $("#Time_AP").val();

        //            $scope.LetterDataIscheck = [];
        //            $scope.LetterDataIscheck = $scope.CCSComplaintList.filter(x => x.ComplaintId == ComId);
        //            $scope.LetterData.SocietyDetails = $scope.LetterDataIscheck;
        //            $http.post(SiteUrl + "/Society/GenerateCCSLetter", { OTP: $scope.OTP, TXN: $scope.Certtrn, LetterDetails: $scope.LetterData }).
        //                success(function (data, status) {

        //                    $(".se-pre-con").fadeOut("slow");
        //                    //$('#myOTPModal').modal("hide");
        //                    if (data == "Success") {
        //                        ShowOkMessage("", _language == 'en-us' ? 'Complaint Letter successfully generated' : "आदेश सफलतापूर्वक जारी हुआ", function () {
        //                            $('#myOTPModal').modal("hide");
        //                            $("#addModal").modal("hide");
        //                            document.getElementById('#NoticeCheckNoticeCheck').checked;
        //                            $scope.Noticechecked = true;
        //                        });
        //                        //window.location = '../Election/ElectionSocietyList';
        //                    } else if (data == "PDF Not Generate") {
        //                        ShowOkMessage("", _language == 'en-us' ? "Complaint Letter could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
        //                        // window.location = '../Election/ElectionSocietyList';
        //                    }

        //                    else if (data == "LOGOUT") {
        //                        ShowOkMessage("", 'Session out.\n', function () {
        //                            window.location = 'https://sso.rajasthan.gov.in/signin';
        //                        });
        //                    } else {
        //                        ShowOkMessage("", data, function () {
        //                        });
        //                    }
        //                }).
        //                error(function (data, status) {
        //                    $(".se-pre-con").fadeOut("slow");
        //                    ShowOkMessage("", _language == 'en-us' ? "Complaint Letter could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
        //                    //return false;
        //                });
        //        }
        //    })
        //}
        //else {
        //    ShowOkMessage("Alert", _language == 'en-us' ? "Please enter OTP." : "कृपया OTP दर्ज करें।", function () { return false; });
        //}



    };
    $scope.CCSList = [];
    $scope.GetCCSList = function () {
        $(".blockUI").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $('#hdfLoggedInUserDistrictId').val() }];
        $http.post(SiteUrl + "/Citizen/GetCreditCooperativeSocieties", { paramlist: model }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.Complaint.CCSId = '0';
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".blockUI").fadeOut("slow");
            });
        $(".blockUI").fadeOut("slow");
    };
    $scope.GetCCSListNew = function () {
        // alert($scope.CCSReport.DistrictId);
        //$('#hdfLoggedInUserDistrictId').val($scope.CCSReport.DistrictId);
        //$(".se-pre-con").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID" }];
        $http.post(SiteUrl + "/Society/GetCCSData", { paramlist: model, Type: $scope.SearchType }).
            success(function (data, status) {
                $scope.CCSList = data;
                //$(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                // alert("Error while fetching list");
                // $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeIn("slow");
    };
    $scope.ValidateData = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#addModal").find(".dvgender").removeClass("notValid");
        $(".validateNewAdd").removeClass("notValid");
        if ($("#From_Date").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  date' : '\n <br />  Please select  date';
            $("#From_Date").addClass("notValid");
        }

        if ($("#Time_HH").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  Time Hour' : '\n <br /> Please select  Time Hour';
            $("#Time_HH").addClass("notValid");
        }


        if ($("#Time_MM").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  Time Minutes' : '\n <br /> Please select  Time Minutes';
            $("#Time_MM").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;

    }

    $scope.ValidateDataforAction = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.LetterData.actionId == undefined || $scope.LetterData.actionId == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  Action Type' : '\n <br /> Please select  Action Type';

        }
        if ($scope.LetterData.actionId == 1 || $scope.LetterData.actionId == '1') {
            if ($scope.LetterData.IsSysytemGen == '' || $scope.LetterData.IsSysytemGen == undefined)
                message += $scope._language == 'en-us' ? '\n <br /> Please select Notice Type' : '\n <br /> Please select Notice Type';



        }
        if ($scope.LetterData.actionId == 4 || $scope.LetterData.actionId == '4') {
            if ($('#OtherNoticeFile').val() == '' || $('#OtherNoticeFile').val() == undefined)
                message += $scope._language == 'en-us' ? '\n <br /> Please upload letter file' : '\n <br /> Please upload letter file';

        }
        if ($scope.LetterData.IsSysytemGen == 1 || $scope.LetterData.IsSysytemGen == '1') {
            if ($('#From_Date').val() == '' || $('#From_Date').val() == undefined)
                message += $scope._language == 'en-us' ? '\n <br /> Please select Letter Generated Date' : '\n <br /> Please select Letter Generated Date';



        }



        if ($scope.LetterData.descriptiondetails == '' || $scope.LetterData.descriptiondetails == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  Remarks details' : '\n <br /> Please enter  Remarks details';



        }

        if ($scope.LetterData.IsSysytemGen != "1") {
            if ($('#OtherNoticeFile').val() == '' || $('#OtherNoticeFile').val() == undefined)
                message += $scope._language == 'en-us' ? '\n <br /> Please upload letter file' : '\n <br /> Please upload letter file';
        }

        if (message.length > 29) {
            ShowModelAlert("CSS Letter", message);
            return false;
        }
        return true;



    }

    $scope.OnPageLoad();
    $scope.GetDistrictList = function () {
        $scope.diviid = $scope.DIVISION_ID;

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.GetDistrictList();
    $scope.TransferDetails = {};
    $scope.showTransModel = function (item) {
        $("#transModals").modal('show');
        $scope.transCCSId = item.ComplaintId;
        $scope.TransferDetails = item;
        $scope.TransferDetails.TransferDistraict = "";
        $scope.TransferDetails.TransferRemarks = "";

    }
    $scope.ClearTransFormForm = function () {
        $scope.TransferDetails.TransferDistraict = "";
        $scope.TransferDetails.TransferRemarks = "";
    }

    $scope.SubmitTransferRequest = function () {
        if ($scope.TransferDetails.TransferDistraict == undefined || $scope.TransferDetails.TransferDistraict == "") {
            var msg = _language == 'en-us' ? 'Please select district' : 'Please select district';
            ShowOkMessage("", msg, function () { });
        } else if ($scope.TransferDetails.TransferRemarks == undefined || $scope.TransferDetails.TransferRemarks == "") {
            var msg = _language == 'en-us' ? 'Please enter remark for transfer request' : 'Please enter remark for transfer request';
            ShowOkMessage("", msg, function () { });
        } else if ($scope.TransferDetails.TransferRegion == undefined || $scope.TransferDetails.TransferRegion == "") {
            var msg = _language == 'en-us' ? 'Please select region for transfer request' : 'Please select region for transfer request';
            ShowOkMessage("", msg, function () { });
        } else {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveCCSCompaintTransferRequest", { ComplaintId: $scope.transCCSId, TransferDistraict: $scope.TransferDetails.TransferDistraict, TransferFromDistraict: $scope.TransferDetails.DISTRICT_ID, TransferRemarks: $scope.TransferDetails.TransferRemarks, status: "", TransferId: 0, ResionName: $scope.TransferDetails.TransferRegion }).
                success(function (data, status) {
                    if (data == 1) {
                        $(".se-pre-con").fadeOut("slow");
                        $("#transModals").modal('hide');
                        ShowOkMessage("", "Transfer Request has been successfully submited.", function () { $scope.pageRequest($scope.DataPaging.currentPageID, $scope.GetCCSComplaint); });

                    } else {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("", "There is some problem please try after some time.", function () { });
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", "There is some problem please try after some time.", function () { });
                });
        }
    }
    $scope.IsShowCountMessage = false;
    $scope.CCSComplaintListSelected = [];
    $scope.OpenBulkNoticePopUp = function () {
        $scope.CCSListNoticeIsue = [];
        $scope.IsShowCountMessage = false;
        $("#BulkNoticeModals").modal("show");
        $scope.CCSComplaintListSelected = [];
        $("#From_Date1").val("");
        $scope.LetterData.descriptiondetails1 = "";
    };

    $scope.CCSComplaintListSearch = [];
    $scope.SearchTextForBulk = function () {
        $scope.CCSComplaintListSearch = [];
        $scope.CCSComplaintListSearch = $scope.CCSComplaintList.filter(function (x) { return x.ReceiptNo.indexOf($scope.Filter.SearchText1) > -1 || x.ComplainerName.indexOf($scope.Filter.SearchText1) > -1 || x.BranchName.indexOf($scope.Filter.SearchText1) > -1 || x.SocietyName.indexOf($scope.Filter.SearchText1) > -1 })
    }

    $scope.selectCCSForNotice = function (item) {
        if ($scope.CCSComplaintListSelected.length < 10) {
            $scope.CCSComplaintListSelected.push({
                ReceiptNo: item.ReceiptNo,
                ComplainerName: item.ComplainerName,
                BranchName: item.BranchName,
                SocietyName: item.SocietyName,
                ComplainerFatherName: item.ComplainerFatherName,
                Address: item.Address,
                ResionNameHi: item.ResionNameHi,
                ComplaintId: item.ComplaintId,
                MobileNo: item.MobileNo,
                Email: item.Email
            });
        } else {
            ShowOkMessage("", "Only 10 Record is allow to add.", function () { });
        }
    }
    $scope.RegionList = [];
    $scope.GetDistrictRegion = function () {
        $scope.RegionList = [];
        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Society/GetResionByDistrict", { "DistrictId": $scope.TransferDetails.TransferDistraict, "ActinDate": $("#From_Date1").val() }).
            success(function (data, status) {
                $scope.RegionList = JSON.parse(data.DataList);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    }

    //$scope.SelectCCSForIssueNotice = function (index, thisItem) {
    //    var newArr = [];
    //    if (thisItem.IsChecked) {
    //        if ($scope.CCSComplaintListSelected.length >= 10) {
    //            ShowOkMessage("", "You can select only 10 comlaint. ", function () { })
    //        } else {
    //            $scope.CCSComplaintListSelected.push(thisItem);
    //        }
    //    } else {
    //        $scope.CCSComplaintListSelected.forEach(function (item, index) {
    //            if (thisItem.ComplaintId != item.ComplaintId) {
    //                newArr.push(item);
    //            }
    //        });
    //        $scope.CCSComplaintListSelected = newArr;
    //    }
    //}
    $scope.SelectCCSForIssueNotice = function (index, thisItem) {
        var newArr = [];
        if (thisItem.IsChecked) {
            angular.forEach($scope.CCSComplaintList, function (item, i) {
                if (i != index) {
                    item.IsChecked = false;
                }
            });

            $scope.CCSComplaintListSelected = [];
            $scope.CCSComplaintListSelected.push(thisItem);
        } else {
            $scope.CCSComplaintListSelected.forEach(function (item) {
                if (thisItem.ComplaintId != item.ComplaintId) {
                    newArr.push(item);
                }
            });
            $scope.CCSComplaintListSelected = newArr;
        }
    };

    $scope.InsertAction = function (tranid, Prsentdate) {
        $scope.Presentdate = $("#From_Date").val();
        $scope.tempCCSComplaintList = $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId);
        var CompName = $scope.tempCCSComplaintList[0].ComplainerName;
        var SocietyName = $scope.tempCCSComplaintList[0].SocietyName;
        var ComplainerFatherName = $scope.tempCCSComplaintList[0].ComplainerFatherName;
        var Address = $scope.tempCCSComplaintList[0].Address;
        var District = $scope.tempCCSComplaintList[0].ResionNameHi;
        var OfficeAddress = $scope.tempCCSComplaintList[0].ResionAddress;
        var HeaderImage = $scope.tempCCSComplaintList[0].HeaderImage;
        var formData = new FormData();
        formData.append("complainerId", $scope.ComplaintIds.ComplaintId);
        formData.append("OfficeAddress", OfficeAddress);
        formData.append("HeaderImage", HeaderImage);
        formData.append("ReceiptNo", $scope.ComplaintIds.ReceiptNo);
        formData.append("actionId", $scope.LetterData.actionId);
        formData.append("Description", $scope.LetterData.descriptiondetails);
        formData.append("DisposeFile", $scope.DisposeFile == undefined ? '' : $scope.DisposeFile);
        if ($scope.LetterData.actionId == 1) {
            formData.append("IsSystemGen", $scope.LetterData.IsSysytemGen);
            formData.append("Presentdate", $scope.Presentdate);
            formData.append("Date", $scope.Presentdate);
            formData.append("TXN", tranid);
            formData.append("ComplainerName", CompName);
            formData.append("SocietyName", SocietyName);
            formData.append("ComplainerFatherName", ComplainerFatherName);
            formData.append("Address", Address);
            formData.append("MobileNo", $scope.tempCCSComplaintList[0].MobileNo);
            if ($scope.tempCCSComplaintList[0].Email != undefined && $scope.tempCCSComplaintList[0].Email != '' && $scope.tempCCSComplaintList[0].Email != null) {
                formData.append("Email", $scope.tempCCSComplaintList[0].Email);
            }
            formData.append("District", District);
        }
        $(".blockUI").fadeIn("slow");
        $http.post("/Society/saveActiondetails", formData, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (data, status) {
            if (data.split('#')[0] == 1) {
                $("#myModals").modal("hide");
                //alert('Data save Sucsessfully!');
                //window.location = '/Society/CCSComplaint';  
                ShowOkMessage("", "Data save Sucsessfully", function () {
                    // $scope.searchRequest($scope.GetCCSComplaint);
                    $scope.pageRequest($scope.DataPaging.currentPageID, $scope.GetCCSComplaint);
                });


                //if ($scope.LetterData.actionId == 3) {
                //    $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId)[0].ActionName = "Not Relevant for Act - 2019";
                //} else if ($scope.LetterData.actionId == 2) {
                //    $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId)[0].ActionName = "Istagasa issue";
                //} else if ($scope.LetterData.actionId == 4) {
                //    $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId)[0].ActionName = "Dispose as per request from complainer";
                //} else if ($scope.LetterData.actionId == 5) {
                //    $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId)[0].ActionName = "Dispose by cort";
                //} else if ($scope.LetterData.actionId == 1) {
                //    $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId)[0].ActionName = "Notice isssue";
                //    $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId)[0].complainant_letter = data.split('#')[1];
                //}    
            }
            else {
                ShowOkMessage("", "There is some error !", function () { });
                //alert('there is some error !');
            }
            $(".blockUI").fadeOut("slow");
        }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");

                alert("Error while fetching list");
            });

    }


    $scope.CCSNoticIssueCount = 0;

    $scope.GetCCSNoticIssueCount = function () {
        $scope.CCSNoticIssueCount = 0;
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/Society/GetCCSNoticIssueCount", { "DistrictId1": 0, "ActinDate": $("#From_Date1").val() }).
            success(function (data, status) {
                $scope.CCSNoticIssueCount = JSON.parse(data.DataList)[0].TotalNotice;
                $scope.IsShowCountMessage = true;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    }


    $scope.IssueNotice = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($('#From_Date1').val() == undefined || $('#From_Date1').val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select Letter Generated Date' : '\n <br /> Please select Letter Generated Date';
        }

        if ($scope.LetterData.descriptiondetails1 == undefined || $scope.LetterData.descriptiondetails1 == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  Remarks details' : '\n <br /> Please enter  Remarks details';
        }

        if (message.length > 29) {
            ShowModelAlert("CSS Issue Notice", message);
            return false;
        } else {
            $scope.GetCCSNoticIssueCount().then(function (d) {
                if ($scope.CCSNoticIssueCount > 5) {
                    var mg = _language == 'en-us' ? 'Notice has been given to ' + $scope.CCSNoticIssueCount + ' people on the date: ' + $scope.LetterData.ccsDate1 + '. Are you sure you want to issue other notice on this date.' : 'दिनांक ' + $scope.LetterData.ccsDate1 + ' पर ' + $scope.CCSNoticIssueCount + ' लोगों को नोटिस दिया गया है। क्या आप सुनिश्चित हैं कि आप इस तिथि को अन्य नोटिस जारी करना चाहते हैं।'
                    ShowAlertMessage("Alert", mg, function () {
                        AadhaarAuth("", $http, "", true, function (authData) {
                            $scope.IssueNoticeAction(authData.TransId);
                        });
                    })
                } else {
                    AadhaarAuth("", $http, "", true, function (authData) {
                        $scope.IssueNoticeAction(authData.TransId);
                    });
                }
            });
        }
    }



    $scope.IssueNoticeAction = function (tranid, Prsentdate) {
        $scope.Presentdate = $("#From_Date1").val();
        var postdata = [];
        $scope.CCSComplaintListSelected.forEach(function (item, indx) {
            postdata.push({
                complainerId: item.ComplaintId,
                OfficeAddress: item.ResionAddress,
                HeaderImage: item.HeaderImage,
                ReceiptNo: item.ReceiptNo,
                actionId: 1,
                Description: $scope.LetterData.descriptiondetails1,
                //DisposeFile: $scope.DisposeFile == undefined ? '' : $scope.DisposeFile,
                IsSystemGen: true,
                Presentdate: $scope.Presentdate,
                Date: $scope.Presentdate,
                TXN: tranid,
                ComplainerName: item.ComplainerName,
                SocietyName: item.SocietyName,
                ComplainerFatherName: item.ComplainerFatherName,
                Address: item.Address,
                MobileNo: item.MobileNo,
                Email: item.Email != undefined && item.Email != '' && item.Email != null ? item.Email : "",
                District: item.ResionNameHi
            });
        });


        $(".blockUI").fadeIn("slow");
        $http.post("/Society/SaveBulkActiondetails", JSON.stringify({ Details: postdata })).
            success(function (data, status) {
                if (data.split('#')[0] == 1) {
                    $("#myModals").modal("hide");
                    //alert('Data save Sucsessfully!');
                    //window.location = '/Society/CCSComplaint';  
                    ShowOkMessage("", "Data save Sucsessfully", function () {
                        $scope.CCSComplaintListSelected = [];
                        $("#From_Date1").val("");
                        $scope.LetterData.descriptiondetails1 = "";

                        $scope.pageRequest($scope.DataPaging.currentPageID, $scope.GetCCSComplaint);

                    });
                }
                else {
                    ShowOkMessage("", "There is some error !", function () { });
                    //alert('there is some error !');
                }
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching list");
            });

    }

    $scope.CCSComplaintList = [];
    $scope.SearchForBulk = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");

                $scope.DataPaging.searchParameters = [];

                $scope.DataPaging.searchParameters.push({ key: "searchText", value: $scope.Filter.SearchText1 == undefined ? "" : $scope.Filter.SearchText1 });
                $scope.DataPaging.searchParameters.push({ key: "CCSStatusId", value: $scope.Filter.Status1 == undefined ? "" : $scope.Filter.Status1 });
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
                return $http.post(SiteUrl + "/Society/GetCCSComplaintList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];                      
                        $scope.CCSComplaintList = JSON.parse(data.DataList);
                        $scope.NewDt = $scope.CCSComplaintList;
                        $rootScope.DataPagingDetails = data.DataPaging;
                        $rootScope.SetDataPaging();
                        $(".blockUI").fadeOut("slow");

                        if ($scope.CCSComplaintListSelected.length > 0) {
                            $scope.CCSComplaintListSelected.forEach(function (itm, idx) {
                                if ($scope.CCSComplaintList.filter(function (x) { return x.ComplaintId === itm.ComplaintId }).length > 0) {
                                    $scope.CCSComplaintList.filter(function (x) { return x.ComplaintId === itm.ComplaintId })[0].IsChecked = true;
                                }
                            });
                        }

                        $scope.GetCCSListNew();
                    }).
                    error(function (data, status) {
                        $scope.CCSComplaintList = [];
                        $(".blockUI").fadeOut("slow");
                        //alert('Error while fetching society list data.');
                    });
            }

        });


    };

    //Esign 2.1 Added By Narpat Kumawat on 12-06-2026
    $scope.RedirectToESPportal = function () {
        if ($scope.ValidateDataforAction()) {
            if ($scope.LetterData.actionId == 1 && $scope.LetterData.IsSysytemGen == 1) {
                $scope.EsignCCSComplaintLatter();
                var data = $('#hdnResponse').val();
                if (data == "1") { ShowOkMessage("", "Data save Sucsessfully", function () { }); } else {
                    ShowOkMessage("", data, function () { });
                }
            } else {
                $scope.InsertAction("0");
            }
        }
    };
    $scope.EsignCCSComplaintLatter = function () {
        $scope.GenerateCCSLatter().then(function (data) {
            $scope.dataPayload = data;
            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.response.DocType,
                ReturnPath: $scope.dataPayload.response.ReturnPath,
                DocumentPath: $scope.dataPayload.response.DocPath,
                DocumentName: $scope.dataPayload.response.DocName,
                docid: $scope.dataPayload.requestData.complainerId,
                SocietyIDEnc: $scope.dataPayload.response.DisposeFile,
                Remark: $scope.LetterData.descriptiondetails,
                StatusId: $scope.LetterData.actionId,
                IsAmendment: "",
                AmendmendAmount: "",
                ReceiptNumber: "",
                details: $scope.dataPayload.requestData,
                Location: $scope.dataPayload.response.Location,
                Desination: $scope.dataPayload.response.Designation
            };
            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }
            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsignCSS($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {
            console.error(err);
            alert("Error generating eSign certificate");
        });
    };
    $scope.GenerateCCSLatter = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Presentdate = $("#From_Date").val();
        $scope.tempCCSComplaintList = $scope.CCSComplaintList.filter(x => x.ComplaintId == $scope.ComplaintIds.ComplaintId);
        var CompName = $scope.tempCCSComplaintList[0].ComplainerName;
        var SocietyName = $scope.tempCCSComplaintList[0].SocietyName;
        var ComplainerFatherName = $scope.tempCCSComplaintList[0].ComplainerFatherName;
        var Address = $scope.tempCCSComplaintList[0].Address;
        var District = $scope.tempCCSComplaintList[0].ResionNameHi;
        var OfficeAddress = $scope.tempCCSComplaintList[0].ResionAddress;
        var HeaderImage = $scope.tempCCSComplaintList[0].HeaderImage;
        var formData = new FormData();
        formData.append("complainerId", $scope.ComplaintIds.ComplaintId);
        formData.append("OfficeAddress", OfficeAddress);
        formData.append("HeaderImage", HeaderImage);
        formData.append("ReceiptNo", $scope.ComplaintIds.ReceiptNo);
        formData.append("actionId", $scope.LetterData.actionId);
        formData.append("Description", $scope.LetterData.descriptiondetails);
        formData.append("DisposeFile", $scope.DisposeFile == undefined ? '' : $scope.DisposeFile);
        if ($scope.LetterData.actionId == 1) {
            formData.append("IsSystemGen", $scope.LetterData.IsSysytemGen);
            formData.append("Presentdate", $scope.Presentdate);
            formData.append("Date", $scope.Presentdate);
            formData.append("ComplainerName", CompName);
            formData.append("SocietyName", SocietyName);
            formData.append("ComplainerFatherName", ComplainerFatherName);
            formData.append("Address", Address);
            formData.append("MobileNo", $scope.tempCCSComplaintList[0].MobileNo);
            if ($scope.tempCCSComplaintList[0].Email != undefined && $scope.tempCCSComplaintList[0].Email != '' && $scope.tempCCSComplaintList[0].Email != null) {
                formData.append("Email", $scope.tempCCSComplaintList[0].Email);
            }
            formData.append("District", District);
        }
        var requestData = {};
        formData.forEach(function (value, key) {
            if (key === "DisposeFile") { return; }
            requestData[key] = value;
        });
        return $http.post(SiteUrl + "/Society/GenerateCCSComplainantLetter", formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            return {
                response: response.data,
                requestData: requestData
            };
        }).catch(function (error) {
            console.error(error);
            alert("Error generating certificate");
            throw error;
        })
    };

    $scope.EsignIssueNotice = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($('#From_Date1').val() == undefined || $('#From_Date1').val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select Letter Generated Date' : '\n <br /> Please select Letter Generated Date';
        }

        if ($scope.LetterData.descriptiondetails1 == undefined || $scope.LetterData.descriptiondetails1 == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter  Remarks details' : '\n <br /> Please enter  Remarks details';
        }

        if (message.length > 29) {
            ShowModelAlert("CSS Issue Notice", message);
            return false;
        } else {
            $scope.GetCCSNoticIssueCount().then(function (d) {
                if ($scope.CCSNoticIssueCount > 5) {
                    var mg = _language == 'en-us' ? 'Notice has been given to ' + $scope.CCSNoticIssueCount + ' people on the date: ' + $scope.LetterData.ccsDate1 + '. Are you sure you want to issue other notice on this date.' : 'दिनांक ' + $scope.LetterData.ccsDate1 + ' पर ' + $scope.CCSNoticIssueCount + ' लोगों को नोटिस दिया गया है। क्या आप सुनिश्चित हैं कि आप इस तिथि को अन्य नोटिस जारी करना चाहते हैं।'
                    $scope.NoticeLetterEsign();
                } else {
                    $scope.NoticeLetterEsign();
                }
            });
        }
    }

    $scope.NoticeLetterEsign = function () {
        $scope.IssueNoticeLetterAction().then(function (data) {
            $scope.dataPayload = data;
            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.response.DocType,
                ReturnPath: $scope.dataPayload.response.ReturnPath,
                DocumentPath: $scope.dataPayload.response.DocPath,
                DocumentName: $scope.dataPayload.response.DocName,
                docid: $scope.dataPayload.requestData[0].complainerId,
                SocietyIDEnc: $scope.dataPayload.requestData[0].ReceiptNo,
                Remark: $scope.dataPayload.requestData[0].Description,
                StatusId: $scope.dataPayload.requestData[0].actionId,
                IsAmendment: "",
                AmendmendAmount: "",
                ReceiptNumber: "",
                details: $scope.dataPayload.requestData,
                Location: $scope.dataPayload.response.Location,
                Desination: $scope.dataPayload.response.Designation
            };
            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }
            console.log('esign', $scope.esignData);
            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsignCSS($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {
            console.error(err);
            alert("Error generating eSign certificate");
        });
    };

    //$scope.IssueNoticeLetterAction = function () {
    //    $scope.Presentdate = $("#From_Date1").val();
    //    var postdata = [];
    //    $scope.CCSComplaintListSelected.forEach(function (item, indx) {
    //        postdata.push({
    //            complainerId: item.ComplaintId,
    //            OfficeAddress: item.ResionAddress,
    //            HeaderImage: item.HeaderImage,
    //            ReceiptNo: item.ReceiptNo,
    //            actionId: 1,
    //            Description: $scope.LetterData.descriptiondetails1,
    //            IsSystemGen: true,
    //            Presentdate: $scope.Presentdate,
    //            Date: $scope.Presentdate,
    //            ComplainerName: item.ComplainerName,
    //            SocietyName: item.SocietyName,
    //            ComplainerFatherName: item.ComplainerFatherName,
    //            Address: item.Address,
    //            MobileNo: item.MobileNo,
    //            Email: item.Email != undefined && item.Email != '' && item.Email != null ? item.Email : "",
    //            District: item.ResionNameHi
    //        });
    //    });
    //    $(".blockUI").fadeIn("slow");
    //    return $http.post("/Society/NoticeIssueLetter", JSON.stringify({ Details: postdata })).
    //        success(function (data, status) {
    //            debugger;
    //            return {
    //                response: data,
    //                requestData: postdata
    //            };
    //        }).error(function (data, status) {
    //            $(".blockUI").fadeOut("slow");
    //            alert("Error while fetching list");
    //        });

    //}


    $scope.IssueNoticeLetterAction = function () {
        $scope.Presentdate = $("#From_Date1").val();
        var postdata = [];
        $scope.CCSComplaintListSelected.forEach(function (item, indx) {
            postdata.push({
                complainerId: item.ComplaintId,
                OfficeAddress: item.ResionAddress,
                HeaderImage: item.HeaderImage,
                ReceiptNo: item.ReceiptNo,
                actionId: 1,
                Description: $scope.LetterData.descriptiondetails1,
                IsSystemGen: true,
                Presentdate: $scope.Presentdate,
                Date: $scope.Presentdate,
                ComplainerName: item.ComplainerName,
                SocietyName: item.SocietyName,
                ComplainerFatherName: item.ComplainerFatherName,
                Address: item.Address,
                MobileNo: item.MobileNo,
                Email: item.Email != undefined && item.Email != '' && item.Email != null ? item.Email : "",
                District: item.ResionNameHi
            });
        });

        $(".blockUI").fadeIn("slow");
        return $http.post("/Society/NoticeIssueLetter", JSON.stringify({ Details: postdata })).then(function (response) {
            $(".blockUI").fadeOut("slow");
            return { response: response.data, requestData: postdata };
        }, function (error) {
            $(".blockUI").fadeOut("slow");
            alert("Error while fetching list");
            return error;
        });
    };

});