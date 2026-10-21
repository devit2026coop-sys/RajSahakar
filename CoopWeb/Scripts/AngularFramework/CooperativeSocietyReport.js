
var CooperativeSocietyReportnew = angular.module('CooperativeReportMod', ['angularUtils.directives.dirPagination']);
    CooperativeSocietyReportnew.controller('CooperativeSocietyReportCnt', function ($scope, $http, $rootScope) {


    
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select').each(function () {

           
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
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
        $scope.ActId = '3';



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


    $scope._language = 'en-us';
    $scope.DivisionList = [];
    $scope.DistrictList = [];
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.currentPage = 1;
    $scope.WrSearch = [];
    $scope.translation = [];

        $scope.defaultact = 'Cooperative Society Act 2001';
        $scope.DateStatus = new Date(); 
        $scope.gettextact = function () {
            var ddlFruits = document.getElementById("ddrActId");
            var selectedText = ddrActId.options[ddrActId.selectedIndex].innerHTML;
            $scope.defaultact = selectedText;
        }


    // START : On Page List
    //END : On PAge List

        $scope.CooperativeSocietyList = [];
        $scope.TotalNumberofSocieties = 0;
        $scope.NoofActiveSocieties = 0;
        $scope.NoofdefunctSocieties = 0;
        $scope.NoofSocietiesunderliquidation = 0;
        $scope.NoofactiveSocietiesuploaded = 0;
        $scope.ProfilecompletebySociety = 0;
        $scope.NoofofflineSocieties = 0;
        $scope.AuditReportUpload = 0;
        $scope.CooperativeAllSocietyList = [];


        $scope.SearchData = function () {
            $(".blockUI").fadeIn("slow");
            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
            return $http.post(SiteUrl + "/MIS/GetCooperativeSocietyReport", JSON.stringify({ "ActId": $scope.ActId == "0" ? $("#hdnActId").val() : $scope.ActId, "DIVISION_ID": $scope.DIVISION_ID, "IsOnline": $scope.IsOnline, "DistrictId": $scope.DISTRICT_ID })).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    // $scope.CooperativeSocietyList =
                    $scope.CooperativeAllSocietyList = data.Table;
                    $scope.GroupByData();
                }).
                error(function (data, status) {
                    $scope.WRDetails = [];
                    $(".blockUI").fadeOut("slow");
                    console.log('Error while fetching WR Detail data.');
                });
        }
        $scope.IsShowDistict = true;

        $scope.GroupByData = function () {
            $scope.CooperativeSocietyListGroupby = [];
            if ($scope.GroupBy == '2') {
                $scope.IsShowDistict = false;
                $scope.CooperativeAllSocietyList.forEach(function (item, index) {
                    $scope.Gdata = $scope.CooperativeSocietyListGroupby.filter(function (x) { return x.DIVISION_ENG === item.DIVISION_ENG });
                    if ($scope.Gdata.length > 0) {
                        $scope.Gdata[0].TotalNumberofSocieties += item.TotalNumberofSocieties;
                        $scope.Gdata[0].NoofActiveSocieties += item.NoofActiveSocieties;
                        $scope.Gdata[0].NoofdefunctSocieties += item.NoofdefunctSocieties;
                        $scope.Gdata[0].NoofSocietiesunderliquidation += item.NoofSocietiesunderliquidation;
                        $scope.Gdata[0].NoofactiveSocietiesuploaded += item.NoofactiveSocietiesuploaded;
                        $scope.Gdata[0].ProfilecompletebySociety += item.ProfilecompletebySociety;
                        $scope.Gdata[0].NoofofflineSocieties += item.NoofofflineSocieties;
                        $scope.Gdata[0].AuditReportUpload += item.AuditReportUpload;
                    } else {
                        $scope.CooperativeSocietyListGroupby.push(item);
                    }
                });
                $scope.CooperativeSocietyList = $scope.CooperativeSocietyListGroupby;
            } else {
                $scope.CooperativeSocietyList = $scope.CooperativeAllSocietyList;
                $scope.IsShowDistict = true;

            }
            

            $scope.TotalNumberofSocieties = 0;
            $scope.NoofActiveSocieties = 0;
            $scope.NoofdefunctSocieties = 0;
            $scope.NoofSocietiesunderliquidation = 0;
            $scope.NoofactiveSocietiesuploaded = 0;
            $scope.ProfilecompletebySociety = 0;
            $scope.NoofofflineSocieties = 0;
            $scope.AuditReportUpload = 0;

            $scope.CooperativeSocietyList.forEach(function (item, index) {
                $scope.TotalNumberofSocieties += item.TotalNumberofSocieties;
                $scope.NoofActiveSocieties += item.NoofActiveSocieties;
                $scope.NoofdefunctSocieties += item.NoofdefunctSocieties;
                $scope.NoofSocietiesunderliquidation += item.NoofSocietiesunderliquidation;
                $scope.NoofactiveSocietiesuploaded += item.NoofactiveSocietiesuploaded;
                $scope.ProfilecompletebySociety += item.ProfilecompletebySociety;
                $scope.NoofofflineSocieties += item.NoofofflineSocieties;
                $scope.AuditReportUpload += item.AuditReportUpload;
            });


        }

        $scope.ExportData = function () {
            $(".blockUI").fadeIn("slow");
            
            $scope.ReportExportList = [];
            var filterType = "";
         

            if ($scope.WrSearch.DIVISION_ID != '' && $scope.WrSearch.DIVISION_ID != undefined) {
                filterType += (filterType != '' ? "," : "") + $scope.translation.CityHeading + "#" + $scope.GetNameFromList($scope.DivisionList, $scope.WrSearch.DIVISION_ID, 'City');
             }

            if ($scope.WrSearch.DISTRICT_ID != '' && $scope.WrSearch.DISTRICT_ID != undefined) {
                filterType += (filterType != '' ? "," : "") + $scope.translation.DistrictHeading + "#" + $scope.GetNameFromList($scope.DistrictList, $scope.WrSearch.DISTRICT_ID, 'District');
            }
            


            if ($("#txtFromDate").val() != '' && $("#txtToDate").val() != '') {
                filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtFromDate").val();
                filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtToDate").val();
            }
            else if ($("#txtFromDate").val() != "") {
                filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtFromDate").val();
                filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtFromDate").val();
            } else if ($("#txtToDate").val() != "") {
                filterType += (filterType != '' ? "," : "") + $scope.translation.FromDate + "#" + $("#txtToDate").val();
                filterType += (filterType != '' ? "," : "") + $scope.translation.ToDate + "#" + $("#txtToDate").val();
            }

            $(".blockUI").fadeIn("slow");
            var functionname = _language == 'en-us' ? "GetCooperativeSocietyReportExportEn" : "GetCooperativeSocietyReportExportHi";//"GenerateWarehouseRSWCHi";//
            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
            return $http.post(SiteUrl + "/MIS/" + functionname, JSON.stringify({ Details: jsonParameters, ReportHead: filterType })).
                success(function (data, status) {
                    
                    $(".blockUI").fadeOut("slow");
                    var bytes = $scope.base64ToArrayBuffer(data);
                    var blob = new Blob([bytes], { type: "application/pdf" });
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    var fileName = "Cooperative Society Report";
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


    //GetSociety start
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
                    // $scope.GetSocietyTypeList();
                    $("#ddrActId").attr("disabled", "disabled");
                }

                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };

    //end

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

    //START : Get Division List
    $scope.GetDivionList = function () {
        
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=0").
            success(function (data, status) {
                $scope.DivisionList = data;
            }).
            error(function (data, status) {
                $scope.DivisionList = [];
            });
    };
    //End : Get Division List

    //START : Get District List
    $scope.GetDistrictList = function () {
        $scope.diviid = $scope.DIVISION_ID;
        
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.diviid).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    //END : Get District List


        $scope.GetSocietyActList();
        $scope.getTranslation();
        $scope.GetDivionList();
        $scope.SearchData();

});