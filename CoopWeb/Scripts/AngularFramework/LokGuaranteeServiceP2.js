var AppClasses = angular.module("LokGuaranteeServiceP2", ['angularUtils.directives.dirPagination']);
AppClasses.controller("LokGuaranteeServiceP2Cont", function ($scope, $http, $rootScope) {
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
        var DateOne = "";
        DateOne = $('#txtMonth').Value
        var parameter = { Key: "DateOne", Value: DateOne }
        $scope.DataPaging.searchParameters.push(parameter);

        var Datefrom = "";
        Datefrom = $('#txtFromMonth').Value

        var parameter = { Key: "Datefrom", Value: Datefrom }
        $scope.DataPaging.searchParameters.push(parameter);

        var DateTo = "";
        Datefrom = $('#txtToMonth').Value
        var parameter = { Key: "DateTo", Value: DateTo }
        $scope.DataPaging.searchParameters.push(parameter);

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
        $scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;

        var DateOne = "";

        DateOne = $('#txtMonth').Value
        var parameter = { Key: "DateOne", Value: DateOne }
        $scope.DataPaging.searchParameters.push(parameter);

        var Datefrom = "";
        Datefrom = $('#txtFromMonth').Value

        var parameter = { Key: "Datefrom", Value: Datefrom }
        $scope.DataPaging.searchParameters.push(parameter);

        var DateTo = "";
        Datefrom = $('#txtToMonth').Value
        var parameter = { Key: "DateTo", Value: DateTo }
        $scope.DataPaging.searchParameters.push(parameter);
        callback();
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
        //        $scope.DataPaging.currentPageID = 1;
        //        callback();
        //    }
        //    ++indx;
        //});
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



    var monthStartDate = "01/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();
    var currentDate = (new Date().getDate().toString().length <= 1 ? "0" + new Date().getDate().toString() : new Date().getDate().toString()) + "/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();


    $("#txtFromDate").val(monthStartDate);
    $("#txtToDate").val(currentDate);

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

    $scope.GetMonthlySummary = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

                return $http.post(SiteUrl + "/MIS/GetLokGuaranteeService", JSON.stringify({ Details: jsonParameters })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];  
                        $scope.MonthlySummaryRegister = [];
                        $scope.DigitalRegisterList = JSON.parse(data.DataList);

                        $scope.NoOfApplicationReceived = 0;
                        $scope.NoOfApplicationSubmited = 0;
                        $scope.NoOfApplicationPending = 0;
                        $scope.NoOfApplicationReceivedCurrMonth = 0;
                        $scope.Col5N6 = 0;
                        $scope.NoOfApplicationSubmitedCurrMonth = 0;
                        $scope.OutOfDate = 0;
                        $scope.UnderDate = 0;
                        $scope.TotC0l10n11 = 0;

                        $scope.DigitalRegisterList.forEach(function (itm, index) {
                            $scope.NoOfApplicationReceived += itm.NoOfApplicationReceived;
                            $scope.NoOfApplicationSubmited += itm.NoOfApplicationSubmited;
                            $scope.NoOfApplicationPending += itm.NoOfApplicationPending;
                            $scope.NoOfApplicationReceivedCurrMonth += itm.NoOfApplicationReceivedCurrMonth;
                            $scope.Col5N6 += itm.Col5N6;
                            $scope.NoOfApplicationSubmitedCurrMonth += itm.NoOfApplicationSubmitedCurrMonth;
                            $scope.OutOfDate += itm.OutOfDate;
                            $scope.UnderDate = itm.UnderDate;
                            $scope.TotC0l10n11 = itm.TotC0l10n11;
                        });
                        //$scope.MonthlySummaryRegister.push({
                        //    dt1: "Total",
                        //    dt2: "",
                        //    dt3: "", dt4: "", dt5: "",
                        //    dt6: $scope.TotalNoOfApplicationReceived,
                        //    dt7: $scope.TotalNoOfApplicationRegistered,
                        //    dt8: $scope.TotalNoOfApplicationPendingAtCitizen,
                        //    dt9: $scope.TotalNoOfApplicationPendingAtDepartment,
                        //    dt10: $scope.TotalNoOfApplicationRejected,
                        //    dt11: $scope.TotalNoOfApplicationAutoClose,
                        //    dt12: $scope.TotalFeeDepositedInTheMonth
                        //});


                        $scope.DigitalRegisterList.forEach(function (itm, index) {
                            if ($scope.MonthlySummaryRegister.filter(function (x) { return }).length <= 0) {
                                var Ydata = {
                                    dt2: itm.SocietyName,
                                    dt3: itm.Act, dt4: itm.NoOfApplicationReceived, dt5: itm.NoOfApplicationSubmited,
                                    dt6: itm.NoOfApplicationPending, dt7: itm.NoOfApplicationReceivedCurrMonth, dt8: itm.Col5N6, dt9: itm.NoOfApplicationSubmitedCurrMonth, dt10: itm.OutOfDate, dt11: itm.UnderDate, dt12: itm.TotC0l10n11
                                }
                                $scope.DigitalRegisterList.filter(function (x) { return }).forEach(function (x, i) {
                                    Ydata.dt4 += x.NoOfApplicationReceived;
                                    Ydata.dt5 += x.NoOfApplicationSubmited;

                                    Ydata.dt6 += x.NoOfApplicationPending;
                                    Ydata.dt7 += x.NoOfApplicationReceivedCurrMonth;

                                    Ydata.dt8 += x.Col5N6;
                                    Ydata.dt9 += x.NoOfApplicationSubmitedCurrMonth;
                                    Ydata.dt10 += x.OutOfDate;
                                    Ydata.dt11 += x.UnderDate;
                                    Ydata.dt12 += x.TotC0l10n11;
                                });
                                $scope.MonthlySummaryRegister.push(Ydata);

                                $scope.DigitalRegisterList.filter(function (a) { return }).forEach(function (z, i) {
                                    if ($scope.MonthlySummaryRegister.filter(function (a) { return }).length <= 0) {
                                        var Mdata = {
                                            dt2: itm.SocietyName,
                                            dt3: z.Act, dt4: "", dt5: "",
                                            dt6: 0, dt7: 0, dt8: 0, dt9: 0, dt10: 0, dt11: 0, dt12: 0
                                        }

                                        $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year && a.Month === z.Month }).forEach(function (b, i) {
                                            Mdata.dt4 += x.NoOfApplicationReceived;
                                            Mdata.dt5 += x.NoOfApplicationSubmited;
                                            Mdata.dt6 += x.NoOfApplicationPending;
                                            Mdata.dt7 += x.NoOfApplicationReceivedCurrMonth;
                                            Mdata.dt8 += x.Col5N6;
                                            Mdata.dt9 += x.NoOfApplicationSubmitedCurrMonth;
                                            Mdata.dt10 += x.OutOfDate;
                                            Mdata.dt11 += x.UnderDate;
                                            Mdata.dt12 += x.TotC0l10n11;

                                        });
                                        $scope.MonthlySummaryRegister.push(Mdata);
                                        $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year && a.Month === z.Month }).forEach(function (b, i) {
                                            $scope.MonthlySummaryRegister.push({
                                                //dt1: itm.Year,
                                                dt2: b.SocietyName,
                                                dt3: b.Act,
                                                dt4: b.NoOfApplicationReceived,
                                                dt5: b.NoOfApplicationSubmited,
                                                dt6: b.NoOfApplicationPending,
                                                dt7: b.NoOfApplicationReceivedCurrMonth,
                                                dt8: b.Col5N6,
                                                dt9: b.NoOfApplicationSubmitedCurrMonth,
                                                dt10: b.OutOfDate,
                                                dt11: b.UnderDate,
                                                dt12: b.TotC0l10n11
                                            });
                                        });
                                    }

                                });
                            }
                        });

                        //$scope.Year = []
                        //$scope.TotalNoOfApplicationReceived = 0;
                        //$scope.TotalNoOfApplicationRegistered = 0;
                        //$scope.TotalNoOfApplicationRejected = 0;
                        //$scope.TotalNoOfApplicationPendingAtCitizen = 0;
                        //$scope.TotalNoOfApplicationPendingAtDepartment = 0;
                        //$scope.TotalNoOfApplicationAutoClose = 0;
                        //$scope.TotalFeeDepositedInTheMonth = 0;




                        //$scope.DigitalRegisterList.forEach(function (itm, index) {
                        //    if ($scope.Year.filter(function (x) { return x.Year === itm.Year }).length <= 0) {
                        //        var Ydata = {
                        //            Year: itm.Year, Total: "",
                        //            TotalNoOfApplicationReceived: 0,
                        //            TotalNoOfApplicationRegistered: 0,
                        //            TotalNoOfApplicationRejected: 0,
                        //            TotalNoOfApplicationPendingAtCitizen: 0,
                        //            TotalNoOfApplicationPendingAtDepartment: 0,
                        //            TotalNoOfApplicationAutoClose: 0,
                        //            TotalFeeDepositedInTheMonth: 0
                        //        }
                        //        $scope.DigitalRegisterList.filter(function (x) { return x.Year === itm.Year }).forEach(function (x, i) {
                        //            Ydata.TotalNoOfApplicationReceived += x.NoOfApplicationReceived;
                        //            Ydata.TotalNoOfApplicationRegistered += x.NoOfApplicationRegistered;
                        //            Ydata.TotalNoOfApplicationRejected += x.NoOfApplicationRejected;
                        //            Ydata.TotalNoOfApplicationPendingAtCitizen += x.NoOfApplicationPendingAtCitizen;
                        //            Ydata.TotalNoOfApplicationPendingAtDepartment += x.NoOfApplicationPendingAtDepartment;
                        //            Ydata.TotalNoOfApplicationAutoClose += x.NoOfApplicationAutoClose;
                        //            Ydata.TotalFeeDepositedInTheMonth += x.FeeDepositedInTheMonth;
                        //        });
                        //        $scope.Year.push(Ydata);
                        //    }

                        //    $scope.TotalNoOfApplicationReceived += itm.NoOfApplicationReceived;
                        //    $scope.TotalNoOfApplicationRegistered += itm.NoOfApplicationRegistered;
                        //    $scope.TotalNoOfApplicationRejected += itm.NoOfApplicationRejected;
                        //    $scope.TotalNoOfApplicationPendingAtCitizen += itm.NoOfApplicationPendingAtCitizen;
                        //    $scope.TotalNoOfApplicationPendingAtDepartment += itm.NoOfApplicationPendingAtDepartment;
                        //    $scope.TotalNoOfApplicationAutoClose += itm.NoOfApplicationAutoClose;
                        //    $scope.TotalFeeDepositedInTheMonth += itm.FeeDepositedInTheMonth;
                        //});

                        //$scope.MonthlySummaryList = $scope.Year;

                        //$scope.MonthlySummaryList.forEach(function (item, index) {
                        //    //item.Month = $scope.DigitalRegisterList.filter(function (x) { return x.Year === item.Year })
                        //    item.Month = [];
                        //    $scope.DigitalRegisterList.filter(function (x) { return x.Year === item.Year }).forEach(function (m, i) {
                        //        if (item.Month.filter(function (y) { return y.Month === m.Month }).length <= 0) {

                        //            var Mdata = {
                        //                Month: m.Month, Total: "", SummaryList: $scope.DigitalRegisterList.filter(function (z) { return z.Year === item.Year && z.Month === m.Month }),
                        //                TotalNoOfApplicationReceived: 0,
                        //                TotalNoOfApplicationRegistered: 0,
                        //                TotalNoOfApplicationRejected: 0,
                        //                TotalNoOfApplicationPendingAtCitizen: 0,
                        //                TotalNoOfApplicationPendingAtDepartment: 0,
                        //                TotalNoOfApplicationAutoClose: 0,
                        //                TotalFeeDepositedInTheMonth: 0
                        //            }
                        //            $scope.DigitalRegisterList.filter(function (a) { return a.Year === m.Year && a.Month === m.Month }).forEach(function (z, i) {
                        //                Mdata.TotalNoOfApplicationReceived += z.NoOfApplicationReceived;
                        //                Mdata.TotalNoOfApplicationRegistered += z.NoOfApplicationRegistered;
                        //                Mdata.TotalNoOfApplicationRejected += z.NoOfApplicationRejected;
                        //                Mdata.TotalNoOfApplicationPendingAtCitizen += z.NoOfApplicationPendingAtCitizen;
                        //                Mdata.TotalNoOfApplicationPendingAtDepartment += z.NoOfApplicationPendingAtDepartment;
                        //                Mdata.TotalNoOfApplicationAutoClose += z.NoOfApplicationAutoClose;
                        //                Mdata.TotalFeeDepositedInTheMonth += z.FeeDepositedInTheMonth;
                        //            });
                        //            item.Month.push(Mdata);
                        //        }
                        //    });
                        //});

                        //$rootScope.DataPagingDetails = data.DataPaging;
                        //$rootScope.SetDataPaging();
                        $(".blockUI").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $scope.DigitalRegisterList = [];
                        $(".blockUI").fadeOut("slow");
                        //alert('Error while fetching society list data.');
                    });
            }

        });


    };
    $scope.GetMonthlySummary();

    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");
        //$scope.ReportExportList = [];
        //$http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
        //    if (SessionStatus.Status == false) {
        //        ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
        //    } else {
        //        $(".blockUI").fadeIn("slow");
        //        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        //        return $http.post(SiteUrl + "/MIS/MonthlySummaryExport", JSON.stringify({ Details: jsonParameters })).
        //            success(function (data, status) {
        $(".blockUI").fadeOut("slow");
        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "Monthly Summary Report";

        var excelData = "<table  border='1'><tr><th colspan='12' style='text-align:center;font-size:20px'>" + fileName + "</th></tr> " + $("#divExportPrint").html() + "</table>"
        excelData = excelData.replace(/₹/g, "");

        var ctx = { worksheet: "Monthly Summary" || 'Worksheet', table: excelData }
        //window.location.href = uri + $scope.base64($scope.format(template, ctx))


        var bytes = $scope.base64ToArrayBuffer($scope.base64($scope.format(template, ctx)));
        //var byte = $scope.toUTF8Array(data);
        var blob = new Blob([bytes], { type: "application/vnd.ms-excel" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);

        link.download = fileName;
        link.click();
        //            }).
        //            error(function (data, status) {
        //                $(".blockUI").fadeOut("slow");
        //                //alert('Error while fetching society list data.');
        //            });
        //    }
        //});

    }

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