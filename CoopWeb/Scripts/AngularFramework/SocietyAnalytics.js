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


      
        $('#ddlYear').find('input[type="checkbox"]:checked').each(function () {
            $(this).prop("checked", false);
            $("#ddlYearSelect").find("span").eq(1).html("");
            $("#ddlYearSelect").find("span").eq(0).show();
            $('#ddlYearSelect').attr("title", "");
        });
        $scope.FilterActId = 0;
        $scope.FilterStatusId = 0;
        $scope.FilterActIndex = -1;
        $scope.FilterStatusIndex = -1;
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

        $("#ddlYear").append('<input type="checkbox" onclick=SelectListElement("ddlYear","ddlYearSelect") dat-val="' + (i + '-' + (i + 1)) + '" value="' + (i + '-' + (i + 1)) + '"  /><label style="font-weight:bold!important; margin-left: 5px; ">' + (i + '-' + (i + 1).toString().substring(2, 4)) + '</label><br />');


    }





    var monthStartDate = "01/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();
    var currentDate = (new Date().getDate().toString().length <= 1 ? "0" + new Date().getDate().toString() : new Date().getDate().toString()) + "/" + ((new Date().getMonth() + 1).toString().length <= 1 ? "0" + (new Date().getMonth() + 1).toString() : (new Date().getMonth() + 1).toString()) + "/" + new Date().getFullYear();


    $("#txtFromDate").val(monthStartDate);
    $("#txtToDate").val(currentDate);

    $scope.TotalAmountActWiseList = 0;
    $scope.TotalAmountStatusWiseList = 0;
    $scope.TotalAmountDistrictWiseList = 0;

    $scope.TotalActWiseList = 0;
    $scope.TotalStatusWiseList = 0;
    $scope.TotalDistrictWiseList = 0;
   
    $scope.ActWiseList = [];
    $scope.StatusWiseList = [];
    $scope.DistrictWiseList = [];

    $scope.FilterActIndex = -1;
    $scope.FilterStatusIndex = -1;
    $scope.FilterDistrictIndex = -1;


    $scope.FilterDistrictId = 0;
    $scope.FilterActId = 0;
    $scope.FilterStatusId = 0;


    $scope.GetMonthlySummary = function () {
        $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
            if (SessionStatus.Status == false) {
                ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
            } else {
                $(".blockUI").fadeIn("slow");
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
                return $http.post(SiteUrl + "/MIS/GetSocietySatausAnalytics", JSON.stringify({ Details: jsonParameters, ActId: $scope.FilterActId, StatusId: $scope.FilterStatusId, DistrictId: $scope.FilterDistrictId  })).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        $scope.TotalAmountActWiseList = 0;
                        $scope.TotalAmountStatusWiseList = 0;
                        $scope.TotalAmountDistrictWiseList = 0;

                        $scope.TotalActWiseList = 0;
                        $scope.TotalStatusWiseList = 0;
                        $scope.TotalDistrictWiseList = 0;

                        $scope.ActWiseList = JSON.parse(data.DataList).Table;
                        $scope.StatusWiseList = JSON.parse(data.DataList).Table1;
                        $scope.DistrictWiseList = JSON.parse(data.DataList).Table2;

                        $scope.StatusWiseList.forEach(function (item, index) {
                            $scope.TotalAmountStatusWiseList += item.TotalAmount;
                            $scope.TotalStatusWiseList += item.TotalSocieties;

                            if ($scope.FilterStatusIndex==index) {
                                item.IsActive = true;
                            } else {
                                item.IsActive = false;
                            }

                        });

                        $scope.ActWiseList.forEach(function (item, index) {
                            $scope.TotalAmountActWiseList += item.TotalAmount;
                            $scope.TotalActWiseList += item.TotalSocieties;
                            if ($scope.FilterActIndex == index) {
                                item.IsActive = true;                                
                            } else {
                                item.IsActive = false;
                            }


                        });
                        $scope.DistrictWiseList.forEach(function (item, index) {
                            $scope.TotalAmountDistrictWiseList += item.TotalAmount;
                            $scope.TotalDistrictWiseList += item.TotalSocieties;
                            if ($scope.FilterDistrictIndex == index) {
                                item.IsActive = true;
                            } else {
                                item.IsActive = false;
                            }
                        });

                       

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





    $scope.FilterByActs = function (item, index) {
        $scope.FilterActIndex = index;
        $scope.FilterStatusIndex = -1;
        if ($("#tdActWiseList > tr").eq(index).hasClass("ActiveRow")) {
            $("#tdActWiseList > tr").removeClass("ActiveRow");
            $("#tdStatusWiseList > tr").removeClass("ActiveRow");
            $scope.FilterActId = 0; 
            $scope.FilterStatusIndex = -1;
            $scope.FilterActIndex =-1
        } else {
            $("#tdActWiseList > tr").removeClass("ActiveRow");
            $("#tdActWiseList > tr").eq(index).addClass("ActiveRow");
            $scope.FilterActId = item.ActId; 
        }        
       
        $scope.FilterStatusId = 0;
        $scope.FilterDistrictIndex = -1;
        $scope.FilterDistrictId = 0;
        $scope.GetMonthlySummary(1);

        //ActiveRow
    }

    $scope.FilterByStatus = function (item, index) {
        //$scope.FilterActIndex = index;
        $scope.FilterStatusIndex = index;
        if ($("#tdStatusWiseList > tr").eq(index).hasClass("ActiveRow")) {
            $("#tdStatusWiseList > tr").removeClass("ActiveRow");
            $scope.FilterStatusId = 0;
            $scope.FilterStatusIndex = -1;
        } else {
            $("#tdStatusWiseList > tr").removeClass("ActiveRow");
            $("#tdStatusWiseList > tr").eq(index).addClass("ActiveRow");
            $scope.FilterStatusId = item.StatusId;
        }  
        $scope.FilterDistrictIndex = -1;
        $scope.FilterDistrictId = 0;
        //$("#tdStatusWiseList > tr").removeClass("ActiveRow");
          
        $scope.GetMonthlySummary(2);
    }

    $scope.FilterByDistrict = function (item, index) {
        //$scope.FilterActIndex = index;
        $scope.FilterDistrictIndex = index;
        if ($("#tdDistrictWiseList > tr").eq(index).hasClass("ActiveRow")) {
            $("#tdDistrictWiseList > tr").removeClass("ActiveRow");
            //$scope.FilterStatusId = 0;
            $scope.FilterDistrictIndex = -1;
        } else {
            $("#tdDistrictWiseList > tr").removeClass("ActiveRow");
            $("#tdDistrictWiseList > tr").eq(index).addClass("ActiveRow");
            $scope.FilterDistrictId = item.DISTRICT_ID;
            $scope.GetMonthlySummary(3);
        }

        //$("#tdStatusWiseList > tr").removeClass("ActiveRow");

     
    }


    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");
      
        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = _language == 'en-us' ? "Societies Registration Analytical Report" : "सोसायटी पंजीकरण विश्लेषणात्मक रिपोर्ट";

        var excelData = "<table  border='1'><tr><th colspan='5'  style='text-align:center;font-size:20px'>" + fileName + "</th></tr> <tr><td><table  border='1'>" + $("#divExportPrint1").html() + "</table></td><td></td><td><table  border='1'>" + $("#divExportPrint2").html() + "</table></td><td></td><td><table  border='1'>" + $("#divExportPrint3").html() + "</table></td></tr></table>"
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
        $(".blockUI").fadeOut("slow");
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