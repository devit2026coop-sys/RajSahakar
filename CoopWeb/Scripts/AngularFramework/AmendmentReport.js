var AppClasses = angular.module("AmendmentMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("AmendmentCnt", function ($scope, $http, $rootScope) {
    $scope.DistrictList = [];
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
        $("#txtFromDate").val("");
        $("#txtToDate").val("");
        //$scope.searchRequest($scope.GetDigitalRegister);
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

    //$scope.DistrictList = [];
    //$scope.GetDistrictList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0")
    //        .success(function (data, status) {
    //            $scope.DistrictList = data;
    //            var hdnDistId = $('#hdnDistrictId').val();

    //            if (hdnDistId && hdnDistId != "0") {
    //                $scope.search.districtId = hdnDistId;
    //                $scope.isLBDisabled = true;
    //            }
    //            else {
    //                $scope.search.districtId = "0";
    //                $scope.isLBDisabled = false;
    //            }

    //        })
    //        .error(function (data, status) {
    //            $scope.DistrictList = [];
    //        });
    //};

    //$scope.GetDistrictList();

    $scope.Actlist = [];
    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                data.forEach(function (item, index) {
                    if ($("#hdnActId").val().split(',').indexOf(item.ActId) > -1) {
                        $scope.Actlist.push(item);
                    }
                });

                if ($("#hdnActId").val().split(',').length == 1) {
                    $scope.ActId = $("#hdnActId").val();
                    $scope.GetSocietyTypeList();
                    $("#ddrActId").attr("disabled", "disabled");
                }

                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };
    $scope.SocietyView = function (SocietyIdEnc) {
        window.location = ' /Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_MIS_SocietyRegisterDistWise~0';
    }

    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $scope.Typelist = [];
        $scope.SocietyTypeId = '';
        if ($scope.ActId != '') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.ActId).
                success(function (data, status) {
                    $scope.Typelist = data;
                }).
                error(function (data, status) {
                    $scope.Typelist = [];
                });
        }
    };
    var monthStartDate = "01/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();
    var currentDate = (new Date().getDate().toString().length <= 1 ? "0" + new Date().getDate().toString() : new Date().getDate().toString()) + "/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();
    $scope.GetSocietyActList();

    $("#txtFromDate").val(monthStartDate);
    $("#txtToDate").val(currentDate);

    //$('#txtFromDate').datepicker();
    //$('#txtToDate').datepicker();

    //$('#txtFromDate').datepicker('setDate', monthStartDate);
    //$('#txtToDate').datepicker('setDate', currentDate);
    $scope.DistrictId = 0;
    $scope.ListType = 0;
    $scope.AmendmentDetailsList = [];
    $scope.showDetailsReport = false;
    $scope.GetDetailsList = function (DistrictId, ListType) {
        $scope.DistrictId = DistrictId;
        $scope.ListType = ListType;
        $scope.showDetailsReport = true;
        $scope.GetDigitalRegister();
    }
    $scope.DetailsBack = function () {
        $scope.showDetailsReport = false;
        $scope.GetAmendmentReport();
    }
    $scope.GetDigitalRegister = function () {
        $scope.AmendmentDetailsList = [];
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
                return $http.post(SiteUrl + "/MIS/GetAmendmentDetailsReport?DistrictId=" + $scope.DistrictId + "&&ListType=" + $scope.ListType+"", JSON.stringify({ Details: jsonParameters })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");                                        
                        $scope.AmendmentDetailsList = JSON.parse(data.DataList);
                        $rootScope.DataPagingDetails = data.DataPaging;
                        $rootScope.SetDataPaging();
                        $(".blockUI").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $scope.AmendmentDetailsList = [];
                        $(".blockUI").fadeOut("slow");
                        //alert('Error while fetching society list data.');
                    });
            }

        });
    };


    $scope.AmendmentList = [];
    $scope.GetAmendmentReport = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var DistrictId = $("#hdnDistrictId").val() == undefined ? "0" : $("#hdnDistrictId").val();
                return $http.post(SiteUrl + "/MIS/GetAmendmentReport?DistrictId=" + DistrictId).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");                                             
                        $scope.AmendmentList = JSON.parse(data.DataList);   
                    }).
                    error(function (data, status) {
                        $scope.AmendmentList = [];
                        $(".blockUI").fadeOut("slow");
                        //alert('Error while fetching society list data.');
                    });
            }

        });
    };

    $scope.GetAmendmentReport();


    $scope.ExportData = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
                return $http.post(SiteUrl + "/MIS/GetDigitalRegister", JSON.stringify({ Details: jsonParameters })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        // $scope.OrderList = [];                      

                        $(".blockUI").fadeOut("slow");

                        var fileName = "Registered Societies";
                        var excelData = "<table border='1'><tr><th colspan='10' style='text-align:center;font-size:20px'>" + fileName + "</th></tr>";
                        excelData += "<tr><th>Name of Society</th><th>Society Type</th><th>Act</th><th>Address</th><th>Date</th><th>Registration No</th><th>CEO</th><th>Mobile</th><th>Fee</th><th>Share Capital</th></tr>";
                        JSON.parse(data.DataList).forEach(function (itm, i) {
                            excelData += "<tr><td>" + itm.SocietyName + "</td><td>" + (_language == 'en-us' ? itm.TypeameEng : itm.TypenameHin) + "</td><td>" + (_language == 'en-us' ? itm.ActNameEn : itm.ActNameHi) + "</td><td>" + (_language == 'en-us' ? itm.AddressEn : itm.AddressHi) + "</td><td>" + itm.Date + "</td><td>" + itm.RegistrationNo + "</td><td>" + itm.CeoName + "</td><td>" + itm.CeoContactNo + "</td><td>" + itm.Amount + "</td><td>" + itm.ShareCapital + "</td></tr>";
                        })
                        excelData += "</table>";

                        var uri = 'data:application/vnd.ms-excel;base64,'
                        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'


                        //var excelData = "<table><tr><th colspan='5' style='text-align:center;font-size:20px'>" + fileName + "</th></tr> <tr><td>" + $("#divExportPrint").html() + "</td></tr></table>"
                        excelData = excelData.replace(/₹/g, "");

                        var ctx = { worksheet: "Summary" || 'Worksheet', table: excelData }
                        //window.location.href = uri + $scope.base64($scope.format(template, ctx))


                        var bytes = $scope.base64ToArrayBuffer($scope.base64($scope.format(template, ctx)));
                        //var byte = $scope.toUTF8Array(data);
                        var blob = new Blob([bytes], { type: "application/vnd.ms-excel" });
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);

                        link.download = fileName;
                        link.click();











                    }).
                    error(function (data, status) {
                        $scope.DigitalRegisterList = [];
                        $(".blockUI").fadeOut("slow");
                        //alert('Error while fetching society list data.');
                    });
            }

        });
    };
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
   //$scope.searchRequest($scope.GetDigitalRegister);
    $scope.SocietyView = function (AppFlow) {
        //if (AppFlow.IsAmendment) {
        //    $scope.showLoader = true;
        //    window.location = "../ApplicationFlow/SocietyActionViewAmendment?SocietyIDEnc=" + AppFlow.SocietyIDEnc + "~_ApplicationFlow_ApplicationFlow";
        //    $scope.showLoader = false;
        //} else {
        $scope.showLoader = true;
        window.location = "../ApplicationFlow/SocietyActionView?SocietyIDEnc=" + AppFlow.SocietyIDEnc + "~_ApplicationFlow_ApplicationFlow";
        $scope.showLoader = false;
        //  }
    };
    // $scope.SendMSG();
});