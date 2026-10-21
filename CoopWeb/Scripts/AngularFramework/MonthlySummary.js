var AppClasses = angular.module("MonthlySummaryMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("MonthlySummaryCnt", function ($scope, $http, $rootScope) {
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
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;

        var district = "";
        $('#ddlDistrict').find('input[type="checkbox"]:checked').each(function () {
            district += (district != "" ? "," : "") + $(this).val();
        });
        var parameter = { Key: "district_id", Value: district }
        $scope.DataPaging.searchParameters.push(parameter);

        var ddlAct = "";
        $('#ddlAct').find('input[type="checkbox"]:checked').each(function () {
            ddlAct += (ddlAct != "" ? "," : "") + $(this).val();
        });
        var parameter = { Key: "ActId", Value: ddlAct }
        $scope.DataPaging.searchParameters.push(parameter);

        var ddlActTypelist = "";
        $('#ddlTypelist').find('input[type="checkbox"]:checked').each(function () {
            ddlActTypelist += (ddlActTypelist != "" ? "," : "") + $(this).val();
        });
        var parameter = { Key: "ActType", Value: ddlActTypelist }
        $scope.DataPaging.searchParameters.push(parameter);

        var ddlYear = "";
        $('#ddlYear').find('input[type="checkbox"]:checked').each(function () {
            ddlYear += (ddlYear != "" ? "," : "") + $(this).val();
        });
        var parameter = { Key: "Year", Value: ddlYear }
        $scope.DataPaging.searchParameters.push(parameter);

        var ddlMonth = "";
        $('#ddlMonth').find('input[type="checkbox"]:checked').each(function () {
            ddlMonth += (ddlMonth != "" ? "," : "") + $(this).val();
        });
        var parameter = { Key: "Month", Value: ddlMonth }
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


        $('#ddlDistrict').find('input[type="checkbox"]:checked').each(function () {
            $(this).prop("checked", false);
            $("#ddlDistrictSelect").find("span").eq(1).html("");
            $("#ddlDistrictSelect").find("span").eq(0).show();
            $('#ddlDistrictSelect').attr("title", "");

        });
        $('#ddlAct').find('input[type="checkbox"]:checked').each(function () {
            $(this).prop("checked", false);
            $("#ddlActSelect").find("span").eq(1).html("");
            $("#ddlActSelect").find("span").eq(0).show();
            $('#ddlActSelect').attr("title", "");
        });
        $('#ddlTypelist').find('input[type="checkbox"]:checked').each(function () {
            $(this).prop("checked", false);
            $("#ddlActTypeSelect").find("span").eq(1).html("");
            $("#ddlActTypeSelect").find("span").eq(0).show();
            $('#ddlActTypeSelect').attr("title", "");
        });
        $('#ddlYear').find('input[type="checkbox"]:checked').each(function () {
            $(this).prop("checked", false);
            $("#ddlYearSelect").find("span").eq(1).html("");
            $("#ddlYearSelect").find("span").eq(0).show();
            $('#ddlYearSelect').attr("title", "");
        });
        $('#ddlMonth').find('input[type="checkbox"]:checked').each(function () {
            $(this).prop("checked", false);
            $("#ddlYearSelect").find("span").eq(1).html("");
            $("#ddlYearSelect").find("span").eq(0).show();
            $('#ddlYearSelect').attr("title", "");
        });








        $("#txtFromDate").val("");
        $("#txtToDate").val("");
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

    $scope.YearList = [];
    var currentYear = new Date().getFullYear();
    for (var i = currentYear; i >= 1950; i--) {
        $scope.YearList.push({ Year: i });

        $("#ddlYear").append('<input type="checkbox" onclick=SelectListElement("ddlYear","ddlYearSelect") dat-val="' + i + '" value="' + i + '"  /><label style="font-weight:bold!important; margin-left: 5px; ">' + i + '</label><br />');


    }



    $scope.Actlist = [];
    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                data.forEach(function (item, index) {
                    if ($("#hdnActId").val().split(',').indexOf(item.ActId) > -1) {
                        $scope.Actlist.push(item);

                        $("#ddlAct").append('<input type="checkbox" ' + ($("#hdnActId").val().split(',').length == 1 ? "checked=true" : "") + '  onclick=SelectListElement("ddlAct","ddlActSelect") dat-val="' + ($scope._language == 'en-us' ? item.ActNameEn : item.ActNameHi) + '" value="' + item.ActId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + ($scope._language == 'en-us' ? item.ActNameEn : item.ActNameHi) + '</label><br />');

                    }
                });

                if ($("#hdnActId").val().split(',').length == 1) {
                    $scope.ActId = $("#hdnActId").val();
                    $scope.GetSocietyTypeList();
                    $("#ddlAct").find("input").attr("disabled", "disabled");
                }

                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };
    $scope.GetSocietyTypeList = function () {
        $scope.Typelist = [];
        $scope.SocietyTypeId = '0';
        if ($scope.ActId != '0') {
            $(".blockUI").fadeIn("slow");
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=0").
                success(function (data, status) {
                    //$scope.Typelist = data;
                    data.forEach(function (item, index) {
                        $("#ddlTypelist").append('<input type="checkbox"   onclick=SelectListElement("ddlTypelist","ddlActTypeSelect") dat-val="' + ($scope._language == 'en-us' ? item.TypeameEng : item.TypeNameHin) + '" value="' + item.SocietyTypeId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + ($scope._language == 'en-us' ? item.TypeameEng : item.TypeNameHin) + '</label><br />');
                    });
                    $(".blockUI").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.Typelist = [];
                });
        }
    };
    $scope.GetSocietyTypeList();
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;

                data.forEach(function (item, index) {

                    if ($("#hdnDistrictId").val() != "0") {
                        if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() == item.DISTRICT_ID) {
                            $("#ddlDistrict").append('<input type="checkbox" disabled="disabled" onclick=SelectListElement("ddlDistrict","ddlDistrictSelect") ' + ($("#hdnDistrictId").val() == item.DISTRICT_ID ? "checked=true" : "") + ' value="' + item.DISTRICT_ID + '" dat-val="' + ($scope._language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL) + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + ($scope._language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL) + '</label><br />');
                        }
                    } else {

                        $("#ddlDistrict").append('<input type="checkbox" onclick=SelectListElement("ddlDistrict","ddlDistrictSelect") ' + ($("#hdnDistrictId").val() == item.DISTRICT_ID ? "checked=true" : "") + ' value="' + item.DISTRICT_ID + '" dat-val="' + ($scope._language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL) + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + ($scope._language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL) + '</label><br />');
                    }
                 
                });
                //if ($("#hdnDistrictId").val() != "0") {
                //    $scope.Filter.DistrictId = $("#hdnDistrictId").val()
                //}
                //$scope.BindFY();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetSocietyActList();
    $scope.GetDistrictList();


    var monthStartDate = "01/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();
    var currentDate = (new Date().getDate().toString().length <= 1 ? "0" + new Date().getDate().toString() : new Date().getDate().toString()) + "/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();


    $("#txtFromDate").val(monthStartDate);
    $("#txtToDate").val(currentDate);

    $scope.TotalNoOfApplicationReceived = 0;
    $scope.TotalNoOfApplicationRegistered = 0;
    $scope.TotalNoOfApplicationRejected = 0;
    $scope.TotalNoOfApplicationPendingAtCitizen = 0;
    $scope.TotalNoOfApplicationPendingAtDepartment = 0;
    $scope.TotalNoOfApplicationAutoClose = 0;
    $scope.TotalFeeDepositedInTheMonth = 0;

    $scope.DigitalRegisterList = [];
    $scope.MonthlySummaryRegister = [];

    $scope.GetMonthlySummary = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

                return $http.post(SiteUrl + "/MIS/GetMonthlySummary", JSON.stringify({ Details: jsonParameters })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];  
                        $scope.MonthlySummaryRegister = [];
                        $scope.DigitalRegisterList = JSON.parse(data.DataList);

                        $scope.TotalNoOfApplicationReceived = 0;
                        $scope.TotalNoOfApplicationRegistered = 0;
                        $scope.TotalNoOfApplicationRejected = 0;
                        $scope.TotalNoOfApplicationPendingAtCitizen = 0;
                        $scope.TotalNoOfApplicationPendingAtDepartment = 0;
                        $scope.TotalNoOfApplicationAutoClose = 0;
                        $scope.TotalFeeDepositedInTheMonth = 0;

                        $scope.DigitalRegisterList.forEach(function (itm, index) {
                            $scope.TotalNoOfApplicationReceived += itm.NoOfApplicationReceived;
                            $scope.TotalNoOfApplicationRegistered += itm.NoOfApplicationRegistered;
                            $scope.TotalNoOfApplicationRejected += itm.NoOfApplicationRejected;
                            $scope.TotalNoOfApplicationPendingAtCitizen += itm.NoOfApplicationPendingAtCitizen;
                            $scope.TotalNoOfApplicationPendingAtDepartment += itm.NoOfApplicationPendingAtDepartment;
                            $scope.TotalNoOfApplicationAutoClose += itm.NoOfApplicationAutoClose;
                            $scope.TotalFeeDepositedInTheMonth += itm.FeeDepositedInTheMonth;
                        });
                        $scope.MonthlySummaryRegister.push({
                            //dt1: "Total",
                            //dt2: "",
                            //dt3: "", dt4: "", dt5: "",
                            //dt6: $scope.TotalNoOfApplicationReceived,
                            //dt7: $scope.TotalNoOfApplicationRegistered,                          
                            //dt8: $scope.TotalNoOfApplicationPendingAtCitizen,
                            //dt9: $scope.TotalNoOfApplicationPendingAtDepartment,
                            //dt10: $scope.TotalNoOfApplicationRejected,
                            //dt11: $scope.TotalNoOfApplicationAutoClose,
                            //dt12: $scope.TotalFeeDepositedInTheMonth
                        });


                        $scope.DigitalRegisterList.forEach(function (itm, index) {
                            if ($scope.MonthlySummaryRegister.filter(function (x) { return x.dt1 === itm.Year }).length <= 0) {
                                var Ydata = {
                                    dt1: itm.Year,
                                    dt2: "Total", dt3: "", dt4: "", dt5: "",
                                    dt6: 0, dt7: 0, dt8: 0, dt9: 0, dt10: 0, dt11: 0, dt12: 0
                                }
                                $scope.DigitalRegisterList.filter(function (x) { return x.Year === itm.Year }).forEach(function (x, i) {
                                    Ydata.dt6 += x.NoOfApplicationReceived;
                                    Ydata.dt7 += x.NoOfApplicationRegistered;                                    
                                    Ydata.dt8 += x.NoOfApplicationPendingAtCitizen;
                                    Ydata.dt9 += x.NoOfApplicationPendingAtDepartment;
                                    Ydata.dt10 += x.NoOfApplicationRejected;
                                    Ydata.dt11 += x.NoOfApplicationAutoClose;
                                    Ydata.dt12 += x.FeeDepositedInTheMonth;
                                });
                                $scope.MonthlySummaryRegister.push(Ydata);

                                $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year }).forEach(function (z, i) {
                                    if ($scope.MonthlySummaryRegister.filter(function (a) { return a.dt1 === itm.Year && a.dt2 === z.Month }).length <= 0) {
                                        var Mdata = {
                                            dt1: itm.Year,
                                            dt2: z.Month, dt3: "Total", dt4: "", dt5: "",
                                            dt6: 0, dt7: 0, dt8: 0, dt9: 0, dt10: 0, dt11: 0, dt12: 0
                                        }

                                        $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year && a.Month === z.Month }).forEach(function (b, i) {
                                            Mdata.dt6 += b.NoOfApplicationReceived;
                                            Mdata.dt7 += b.NoOfApplicationRegistered;                                            
                                            Mdata.dt8 += b.NoOfApplicationPendingAtCitizen;
                                            Mdata.dt9 += b.NoOfApplicationPendingAtDepartment;
                                            Mdata.dt10 += b.NoOfApplicationRejected;
                                            Mdata.dt11 += b.NoOfApplicationAutoClose;
                                            Mdata.dt12 += b.FeeDepositedInTheMonth;
                                        });
                                        $scope.MonthlySummaryRegister.push(Mdata);
                                        $scope.DigitalRegisterList.filter(function (a) { return a.Year === itm.Year && a.Month === z.Month }).forEach(function (b, i) {
                                            $scope.MonthlySummaryRegister.push({
                                                dt1: itm.Year,
                                                dt2: b.Month,
                                                dt3: b.District,
                                                dt4: b.Act,
                                                dt5: b.TypesofSociety,
                                                dt6: b.NoOfApplicationReceived,
                                                dt7: b.NoOfApplicationRegistered,                                              
                                                dt8: b.NoOfApplicationPendingAtCitizen,
                                                dt9: b.NoOfApplicationPendingAtDepartment,
                                                dt10: b.NoOfApplicationRejected,
                                                dt11: b.NoOfApplicationAutoClose,
                                                dt12: b.FeeDepositedInTheMonth
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