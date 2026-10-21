var AppClasses = angular.module("AuditProgressBlock", ['angularUtils.directives.dirPagination']);
AppClasses.controller("AuditProgressBlockCont", function ($scope, $http, $rootScope) {
    $scope.currentPage = 1;
    $scope.pageSize = 25;
    $scope.divTabulation = true;
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
        //$scope.DataPaging.currentPageID = parseInt(pageid);
        //$scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        //var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        //var indx = 1;

        //var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        //var indx = 1;
        //$('#' + $scope.DataPaging.searchBoxDivId).find('input:text').each(function () {


        //    if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
        //        var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
        //        $scope.DataPaging.searchParameters.push(parameter);
        //    }

        //    if (indx == ln) {
        //        //  _currentPageID = 1;
        //        callback();
        //    }
        //    ++indx;
        //});
        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
    }
    $scope.OnPageLoad = function () {
        $('#From_Time').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $scope.GetCCSListNew();
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
        //$scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        //var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        //var indx = 1;
        //var Datefrom = "";
        //Datefrom = $("#txtFromMonth").val()
        //var parameter = { Key: "DateFrm", Value: Datefrom }
        //$scope.DataPaging.searchParameters.push(parameter);

        //var DateTo = "";
        //DateTo = $("#txtToMonth").val()
        //var parameter = { Key: "DtTo", Value: DateTo }
        //$scope.DataPaging.searchParameters.push(parameter);
        callback();
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
        $scope.Filter.CCSId = '0';
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];


        //$('#ddlDistrict').find('input[type="checkbox"]:checked').each(function () {
        //    $(this).prop("checked", false);
        //    $("#ddlDistrictSelect").find("span").eq(1).html("");
        //    $("#ddlDistrictSelect").find("span").eq(0).show();
        //    $('#ddlDistrictSelect').attr("title", "");

        //});
        //$('#ddlAct').find('input[type="checkbox"]:checked').each(function () {
        //    $(this).prop("checked", false);
        //    $("#ddlActSelect").find("span").eq(1).html("");
        //    $("#ddlActSelect").find("span").eq(0).show();
        //    $('#ddlActSelect').attr("title", "");
        //});
        //$('#ddlTypelist').find('input[type="checkbox"]:checked').each(function () {
        //    $(this).prop("checked", false);
        //    $("#ddlActTypeSelect").find("span").eq(1).html("");
        //    $("#ddlActTypeSelect").find("span").eq(0).show();
        //    $('#ddlActTypeSelect').attr("title", "");
        //});
        //$('#ddlYear').find('input[type="checkbox"]:checked').each(function () {
        //    $(this).prop("checked", false);
        //    $("#ddlYearSelect").find("span").eq(1).html("");
        //    $("#ddlYearSelect").find("span").eq(0).show();
        //    $('#ddlYearSelect').attr("title", "");
        //});
        //$('#ddlMonth').find('input[type="checkbox"]:checked').each(function () {
        //    $(this).prop("checked", false);
        //    $("#ddlYearSelect").find("span").eq(1).html("");
        //    $("#ddlYearSelect").find("span").eq(0).show();
        //    $('#ddlYearSelect').attr("title", "");
        //});

        //$("#txtFromDate").val("");
        //$("#txtToDate").val("");
        $scope.GetMonthlySummary();
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
    $scope.NoOfApplicationReceived = 0;
    $scope.NoOfApplicationSubmited = 0;
    $scope.NoOfApplicationPending = 0;
    $scope.NoOfApplicationReceivedCurrMonth = 0;
    $scope.Col5N6 = 0;
    $scope.NoOfApplicationSubmitedCurrMonth = 0,
        $scope.OutOfDate = 0;
    $scope.UnderDate = 0;
    $scope.TotC0l10n11 = 0;

    $scope.DigitalRegisterList = [];
  
    $scope.MonthlySummaryRegister = [];

    $scope.GetLokGuranteeService = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
               // var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

                return $http.post(SiteUrl + "/MIS/GetAuditProgressBlock").
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];  
                        $scope.MonthlySummaryRegister = [];
                        $scope.DigitalRegisterList = JSON.parse(data.DataList);

                        //$scope.TotalNoOfApplicationReceived = 0;
                        //$scope.TotalNoOfApplicationRegistered = 0;
                        //$scope.TotalNoOfApplicationRejected = 0;
                        //$scope.TotalNoOfApplicationPendingAtCitizen = 0;
                        //$scope.TotalNoOfApplicationPendingAtDepartment = 0;
                        //$scope.TotalNoOfApplicationAutoClose = 0;
                        //$scope.TotalFeeDepositedInTheMonth = 0;

                        $scope.DigitalRegisterList.forEach(function (itm, index) {
                            //$scope.TotalNoOfApplicationReceived += itm.NoOfApplicationReceived;
                            //$scope.TotalNoOfApplicationRegistered += itm.NoOfApplicationRegistered;
                            //$scope.TotalNoOfApplicationRejected += itm.NoOfApplicationRejected;
                            //$scope.TotalNoOfApplicationPendingAtCitizen += itm.NoOfApplicationPendingAtCitizen;
                            //$scope.TotalNoOfApplicationPendingAtDepartment += itm.NoOfApplicationPendingAtDepartment;
                            //$scope.TotalNoOfApplicationAutoClose += itm.NoOfApplicationAutoClose;
                            //$scope.TotalFeeDepositedInTheMonth += itm.FeeDepositedInTheMonth;
                        });
                        $scope.MonthlySummaryRegister.push({
                            ////dt1: "Total",
                            ////dt2: "",
                            ////dt3: "", dt4: "", dt5: "",
                            ////dt6: $scope.TotalNoOfApplicationReceived,
                            ////dt7: $scope.TotalNoOfApplicationRegistered,                          
                            ////dt8: $scope.TotalNoOfApplicationPendingAtCitizen,
                            ////dt9: $scope.TotalNoOfApplicationPendingAtDepartment,
                            ////dt10: $scope.TotalNoOfApplicationRejected,
                            ////dt11: $scope.TotalNoOfApplicationAutoClose,
                            ////dt12: $scope.TotalFeeDepositedInTheMonth
                        });


                        $scope.DigitalRegisterList.forEach(function (itm, index) {
                            if ($scope.MonthlySummaryRegister.filter(function (x) { return x.DIVISION_ENG === itm.DIVISION_ENG }).length <= 0) {
                                var Ydata = {
                                    dt2: itm.DIVISION_ENG,
                                   dt3: "", dt4: "", dt5: "", dt6: "",
                                   dt7: 0, dt8: ""
                                }
                                $scope.DigitalRegisterList.filter(function (x) { return x.DIVISION_ENG === itm.DIVISION_ENG }).forEach(function (x, i) {
                                    //Ydata.dt3 += x.DISTRICT_ENG;
                                    //Ydata.dt4 += 0;
                                    //Ydata.dt5 += 0;
                                    //Ydata.dt6 += 0;
                                    //Ydata.dt7 += 0;
                                    //Ydata.dt8 += 0;
                                    
                                });
                               // $scope.MonthlySummaryRegister.push(Ydata);

                                $scope.DigitalRegisterList.filter(function (a) { return a.DIVISION_ENG === itm.DIVISION_ENG }).forEach(function (z, i) {
                                    if ($scope.MonthlySummaryRegister.filter(function (a) { return a.dt2 === itm.DIVISION_ENG && a.dt3 === z.DIVISION_ENG }).length <= 0) {
                                        var Mdata = {
                                            dt2: itm.DIVISION_ENG,
                                            dt3: "", dt4: "", dt5: "",
                                            dt6: "", dt7: "", dt8: ""
                                        }
                                        $scope.DigitalRegisterList.filter(function (a) { return a.DIVISION_ENG === itm.DIVISION_ENG && a.Month === z.Month }).forEach(function (b, i) {
                                           // Mdata.dt4 += 0;
                                            Mdata.dt5 += "";
                                            Mdata.dt6 += "";
                                            Mdata.dt7 += "";
                                            Mdata.dt8 += "";                                           
                                        });
                                        $scope.MonthlySummaryRegister.push(Mdata);
                                        $scope.DigitalRegisterList.filter(function (a) { return a.DIVISION_ENG === itm.DIVISION_ENG }).forEach(function (b, i) {
                                            $scope.MonthlySummaryRegister.push({
                                                dt2: itm.DIVISION_ENG,
                                                dt3: b.DISTRICT_ENG,
                                                dt4: 0,
                                                dt5: 0,
                                                dt6: 0,
                                                dt7:0,
                                                dt8: 0
                                            });
                                        });
                                    }
                                });
                            }
                        });
                        $(".blockUI").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $scope.DigitalRegisterList = [];
                        $(".blockUI").fadeOut("slow");
                    });
            }
        });
    };
    $scope.GetLokGuranteeService();

    
    $scope.base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
    $scope.format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

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

    // $scope.SendMSG();
});