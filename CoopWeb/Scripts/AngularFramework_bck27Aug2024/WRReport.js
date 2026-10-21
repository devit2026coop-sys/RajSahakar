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
        $scope.TotalNetQuantity = 0;
        $scope.TotalNetQuantityValue = 0;
        $scope.TotalWR = 0;
        $scope.PackagesLots = 0;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetWRReport", JSON.stringify({ Details: jsonParameters, ReportType: $scope.ReportType })).
            //$http.post(SiteUrl + "/MIS/GetWRDetails", { "CommodityId": $scope.WrSearch.CommodityId, "DistrictId": $scope.WrSearch.DistrictId, "WarehouseId": $scope.WrSearch.WarehouseId, "KVSSId": $scope.WrSearch.KVSSId, "PurchaseCenterId": $scope.WrSearch.PurchaseCenterId, "Search": $scope.WrSearch.Search }).
            success(function (data, status) {
                // $scope.OrderList = [];
                $scope.WRDetails = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

                $scope.WRDetails.forEach(function (item, indx) {
                    $scope.TotalNetQuantity += item.net_quantity;//.toFixed(2);
                    $scope.TotalNetQuantityValue += item.TotalValue;//.toFixed(2);
                    $scope.TotalWR += item.TotalWR;
                    $scope.PackagesLots += item.packages_lots;
                    //$scope.TotalNetQuantity = (parseFloat($scope.TotalNetQuantity) + parseFloat(item.net_quantity)).toFixed(2);
                    //$scope.TotalNetQuantityValue = (parseFloat($scope.TotalNetQuantityValue) + parseFloat(item.TotalValue)).toFixed(2);
                    //$scope.TotalWR = $scope.TotalWR + item.TotalWR;
                    //$scope.PackagesLots = (parseFloat($scope.PackagesLots) + parseFloat(item.packages_lots));;
                });
                $scope.TotalNetQuantity = $scope.TotalNetQuantity.toFixed(2);
                $scope.TotalNetQuantityValue = $scope.TotalNetQuantityValue.toFixed(2);
                //$scope.PackagesLots = $scope.PackagesLots.toFixed(2);

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
        $scope.ReportExportList = [];


        var filterType = "";

        if ($scope.Filter.FinancialYear != '' && $scope.Filter.FinancialYear!=undefined) {
            filterType = $scope.translation.FincialYearHeading + "#" + $scope.Filter.FinancialYear;
        }
        if ($scope.WrSearch.DistrictId != '' && $scope.WrSearch.DistrictId!= undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.DistrictHeading + "#" + $scope.GetNameFromList($scope.DistrictList, $scope.WrSearch.DistrictId, 'District');
        }
        if ($scope.WrSearch.WarehouseId != '' && $scope.WrSearch.WarehouseId != undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.Warehouse + "#" + $scope.GetNameFromList($scope.WarehouseList, $scope.WrSearch.WarehouseId, 'Warehous');
        }
        if ($scope.WrSearch.CommodityId != '' && $scope.WrSearch.CommodityId  != undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.Commodity + "#" + $scope.GetNameFromList($scope.Commodities, $scope.WrSearch.CommodityId, 'Commodity');
        }
        if ($scope.WrSearch.RegionalOffice != '' && $scope.WrSearch.RegionalOffice != undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.RegionalOffice + "#" + $scope.GetNameFromList($scope.ROList, $scope.WrSearch.RegionalOffice, 'RegionalOffice');
        }
        if ($scope.WrSearch.PurchaseCenterId != '' && $scope.WrSearch.PurchaseCenterId != undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.PurchaseCenter + "#" + $scope.GetNameFromList($scope.PurchaseCenterList, $scope.WrSearch.PurchaseCenterId, 'PurchaseCenter');
        }

        if ($scope.WrSearch.KVSSId != '' && $scope.WrSearch.KVSSId!= undefined) {
            filterType += (filterType != '' ? "," : "") + $scope.translation.KVSSMPRForm + "#" + $scope.GetNameFromList($scope.KVSSList, $scope.WrSearch.KVSSId, 'KVSS');
        }

        if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
            filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtFromDate").val();
            filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtToDate").val();
        } else if ($("#txtFromDate").val()!="") {
            filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtFromDate").val();
            filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtFromDate").val();
        } else if ($("#txtToDate").val() != "") {
            filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtToDate").val();
            filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtToDate").val();
        }

       

    




        var functionname = _language == 'en-us' ? "GetWRReportExportEn" : "GetWRReportExportHi";//"GenerateWarehouseRSWCHi";//
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
        return $http.post(SiteUrl + "/MIS/" + functionname, JSON.stringify({ Details: jsonParameters, ReportHead: filterType, ReportType: $scope.ReportType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var bytes = $scope.base64ToArrayBuffer(data);
                var blob = new Blob([bytes], { type: "application/pdf" });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                var fileName = "WRReport_" + $scope.ReportType;
                link.download = fileName;
                link.click();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });

    }

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






    $scope.GetNameFromList = function (list, id, type) {
        ;
        if (type == 'Godown') {
            var data = list.filter(function (x) { return x.GoDownId.toString() === id.toString() })[0];
            return _language == 'en-us' ? data.GoDownNameEng : data.GoDownNameHi;
        } else if (type == 'Unit') {
            var data = list.filter(function (x) { return x.UnitId.toString() === id })[0];
            return _language == 'en-us' ? data.UnitNameEng : data.UnitNameHi;
        } else if (type == 'District') {
            var data = list.filter(function (x) { return x.DISTRICT_ID.toString() === id })[0];
            return _language == 'en-us' ? data.DISTRICT_ENG : data.DISTRICT_ENG;
        } else if (type == 'KVSS') {
            var data = list.filter(function (x) { return x.KvssId.toString() === id })[0];
            return _language == 'en-us' ? data.KVSS : data.KVSS;
        } else if (type == 'PurchaseCenter') {
            var data = list.filter(function (x) { return x.Center_Id.toString() === id })[0];
            return _language == 'en-us' ? data.CenterNameEn : data.CenterNameHi;
        } else if (type == 'Commodity') {
            var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
            return _language == 'en-us' ? data.CommodityNameEn : data.CommodityNameHi;
        } else if (type == 'Warehous') {
            var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
            return _language == 'en-us' ? data.WarehousNameEn : data.CommodityNameEn;
        } else if (type == 'CommodityPrice') {
            var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
            return _language == 'en-us' ? data.Price : data.Price;
        } else if (type == 'LicenseNo') {
            var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
            return _language == 'en-us' ? data.LicenceNo : data.LicenceNo;
        } else if (type == 'IsKharif') {
            var data = list.filter(function (x) { return x.CommodityId.toString() === id })[0];
            return (data.IsKharif == null ? false : true) == true ? "रबी" : "खरीफ";
        } else if (type == 'LicenceValidUpTo') {
            var data = list.filter(function (x) { return x.WarehousId.toString() === id })[0];
            return data.LicenceValidUpTo;
        } else if (type == 'RegionalOffice') {
            var data = list.filter(function (x) { return x.RajfedROId.toString() === id })[0];
            return _language == 'en-us' ? data.RONameEn : data.RONameHn;
        }

        
    }


});
