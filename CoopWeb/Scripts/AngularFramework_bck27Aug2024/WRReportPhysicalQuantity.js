var CommodityWiseWRDetailModule = angular.module('WRReportMod', ['angularUtils.directives.dirPagination']);
CommodityWiseWRDetailModule.controller("WRReportCont", function ($rootScope, $scope, $http) {
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
        $scope.SearchData();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    //$scope._language = 'en-us';
    $scope.DistrictList = [];
    $scope.Commodities = [];
    $scope.KVSSList = [];
    $scope.PurchaseCenterList = [];

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.currentPage = 1;
    $scope.WrSearch = [];



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

    var dd=new Date().getDate()
    var mm = new Date().getMonth() + 1;
    var yy = new Date().getFullYear();

    var toDaydate = (dd.toString().length==1?("0"+dd.toString()):dd.toString())+"/"+(mm.toString().length==1?("0"+mm.toString()):mm.toString())+"/"+yy.toString();
    $("#txtToDate").val(toDaydate);
    $scope.BindFY();




    // Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language === 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    // Code for Language label setting

    //START : Get District List
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };



    $scope.GetAllDistrictListByRO = function () {
        $(".blockUI").fadeIn("slow");
        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICTBYRO&MasterCode=" + $scope.WrSearch.RegionalOffice).
            success(function (data, status) {
                $scope.DistrictList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                $(".blockUI").fadeOut("slow");
            });
    }



    //END : Get District List

    //START : Get Commodity List
    $scope.GetCommodityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMMODITY&MasterCode=0").
            success(function (data, status) {
                $scope.Commodities = data;
            }).
            error(function (data, status) {
                Console.log('Error while getting commoties');
            });
    };
    //END : Get Commodity List

    //START : Get warehouse List


    $scope.GetWarehouseList = function () {
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": "0" }).
            success(function (data, status) {
                $scope.WarehouseList = [];
                $scope.WarehouseList = data;
                $scope.GetKVSSList('');
                $scope.GetPurchaseCenterList('');
            }).
            error(function () {
                $scope.Warehouses = [];
            });
    };





    $scope.GetWarehouseList();
    //START : Get warehouse List

    //START : Get KVSS List
    $scope.GetKVSSList = function (id) {
        $(".blockUI").fadeIn("slow");

        return $http.post(SiteUrl + "/FertilizerSupply/GetKVSS", { "DistrictId": +$scope.WrSearch.DistrictId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
                $scope.KVSSList = data;
                $scope.GetPurchaseCenterList('0');
                //$scope.WrSearch.KVSSId = id;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
            });
    };
    //END : Get KVSS List

    //START : Get Purchase Center List
    $scope.GetPurchaseCenterList = function (id) {
        $(".blockUI").fadeIn("slow");
        //$scope.WrSearch["PurchaseCenterId"] = "0";
        return $http.post(SiteUrl + "/FertilizerSupply/GetPurchaseCenterList", { "DistrictId": + $scope.WrSearch.DistrictId, "CommodityId": $scope.WrSearch.CommodityId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = [];
                $scope.PurchaseCenterList = data;
                //$scope.WrSearch.PurchaseCenterId = id;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = [];
            });
    };
    //END : Get KVSS List

    $scope.ReportType = 'FY'

    $scope.GetWarehouseFilterList = function (type) {
        $scope.ReportType = type;
        $scope.searchRequest($scope.SearchData);
    }

    $scope.TotalNetQuantity = 0;
    $scope.TotalNetQuantityValue = 0;
    $scope.TotalWR = 0;
    // START : WR Details
    $scope.SearchData = function () {
        $(".blockUI").fadeIn("slow");
        //var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.get(SiteUrl + "/MIS/GetWRPhysicalQuantityReport/?Fy=" + ($scope.Filter.FinancialYear == undefined ? "" : $scope.Filter.FinancialYear) + "&commodityid=" + ($scope.Filter.commoditytype == undefined ? "" : $scope.Filter.commoditytype) + "&WharehousId=0&fromdatId=" + $("#txtToDate").val() + "&TodatId=0").
       
        //return $http.post(SiteUrl + "/MIS/GetWRPhysicalQuantityReport", JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType })).
            //$http.post(SiteUrl + "/MIS/GetWRDetails", { "CommodityId": $scope.WrSearch.CommodityId, "DistrictId": $scope.WrSearch.DistrictId, "WarehouseId": $scope.WrSearch.WarehouseId, "KVSSId": $scope.WrSearch.KVSSId, "PurchaseCenterId": $scope.WrSearch.PurchaseCenterId, "Search": $scope.WrSearch.Search }).
            success(function (data, status) {
                // $scope.OrderList = [];
                $scope.WRDetails = JSON.parse(data.DataList);
               // $rootScope.DataPagingDetails = data.DataPaging;
               // $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

                $scope.TotalWR = 0;
                $scope.PhysicalQuantity = 0;
                $scope.PhysicalTotalValue = 0;
                $scope.EWR = 0;
                $scope.EWRQuantity = 0;
                $scope.EWRValue = 0;
                $scope.TotalWREWR = 0;
                $scope.PhysicalQuantityEWRValue = 0;
                $scope.PhysicalTotalValueEWRValue = 0;

                $scope.WRDetails.forEach(function (item, indx) {
                    $scope.TotalWR += item.TotalWR
                    $scope.PhysicalQuantity += item.PhysicalQuantity;
                    $scope.PhysicalTotalValue += item.PhysicalTotalValue;
                    $scope.EWR += item.EWR
                    $scope.EWRQuantity += item.EWRQuantity;
                    $scope.EWRValue += item.EWRValue;
                    $scope.TotalWREWR += item.TotalWREWR
                    $scope.PhysicalQuantityEWRValue += item.PhysicalQuantityEWRValue;
                    $scope.PhysicalTotalValueEWRValue = item.PhysicalTotalValueEWRValue;
                });

                $scope.PhysicalQuantity = $scope.PhysicalQuantity.toFixed(2);
                $scope.PhysicalTotalValue = $scope.PhysicalTotalValue.toFixed(2);
                $scope.EWRQuantity = $scope.EWRQuantity.toFixed(2);
                $scope.EWRValue = $scope.EWRValue.toFixed(2);
                $scope.PhysicalQuantityEWRValue = $scope.PhysicalQuantityEWRValue.toFixed(2);
                $scope.PhysicalTotalValueEWRValue = $scope.PhysicalTotalValueEWRValue.toFixed(2);

            }).
            error(function (data, status) {
                $scope.WRDetails = [];
                $(".blockUI").fadeIn("slow");
                console.log('Error while fetching WR Detail data.');
            });
    };


    $scope.ROList = [];
    $scope.GetRAJFEDRO = function () {
        $scope.ROList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RAJFEDRO&MasterCode=0").
            success(function (data, status) {
                $scope.ROList = data;
            }).
            error(function (data, status) {
                // alert("Error while fetching Post list")

            });
    }
    $scope.GetRAJFEDRO();

    $scope.SearchData();
    // END : WR Details



    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");

        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "WR Physical Quantity Report_" + $("#txtToDate").val()+".xls";
        var tabledata = $("#tblExport").html();

        var excelData = "<table  border='1'><tr><th colspan='13' style='text-align:center;font-size:20px'>Daily Information related to Physical WRs and E-WRs as on  " + $("#txtToDate").val() + "</th></tr><tr><td>" + tabledata + "</td></tr></table>";


        excelData = excelData.replace(/₹/g, "").replace(/\n/g, "");
        var ctx = { worksheet: "WR Physical Quantity Report" || 'Worksheet', table: excelData }
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









    // START : On Page List
    $scope.getTranslation();
    $scope.GetCommodityList();
    $scope.GetDistrictList();
    //END : On PAge List




});
