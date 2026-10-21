var CCSComplaintReportModule = angular.module('GetCCSComplaintReportMod', ['angularUtils.directives.dirPagination']);
CCSComplaintReportModule.controller("GetCCSComplaintReportCnt", function ($rootScope, $scope, $http) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
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
        $scope.qery = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.searchRequest($scope.GetCCSCompalainteReportList);
    }
    /*------------End Paging and Filter-----------------*/

    // SCOPE

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divStateData = true;
    $scope.SearchType = 1;
    $scope.GetActionIdCCSComplaintReport = [];
    $scope.DistrictId = "0";
    $scope.CCSReport = [];
    $scope.CCSReport.CCSId1 = '0';


    $scope.ReportListType = "0";
    //$scope.Filter.CCSId = '0';
    $scope.IsnotShow = false;

    $scope.CCSReport = { DistrictId:0 };

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        }

        
        
        $scope.GetDistricts().then(function () {

            if ($('#hdfLoggedInUserDistrictId').val() != "0") {
                $scope.IsControlDisable = true;
                //alert($('#hdfLoggedInUserDistrictId').val());
                $scope.CCSReport.DistrictId = $('#hdfLoggedInUserDistrictId').val();                
            }
            else {
                $scope.IsControlDisable = false;
            }            
            $scope.GetCCSComplaintStatusReport();
        });

        

        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetComplaintReportCounts = function () {

        //+"&IsDistrictWise="+id
        var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;
        $(".se-pre-con").fadeIn("slow");
        $scope.GetActionIdCCSComplaintReport = [];
        $scope.GetCCSCompalainteReportData = [];
        
        $http.post(SiteUrl + "/MIS/GetActionIdCCSComplaintReport?ActionId=0&DistrictId=" + districtId + "&IsRowCount=true").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetActionIdCCSComplaintReport = JSON.parse(data.DataList);
                $scope.IsDistrictwise = false;
                $scope.IsDistrictwiseNot = true;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetActionIdCCSComplaintReport = [];
            });
    }
    $scope.DatabyDistricts = [];



    $scope.clean = function () {
    $scope.Total = 0;
    $scope.NoticeIsssue = 0;
    $scope.IstagasaIssue = 0;
    $scope.NotRelevant = 0;
    $scope.DisposeAsPerRequestFromComplainer = 0;
    $scope.DisposeByCort = 0;
    $scope.Reject = 0;
    $scope.Dispose = 0;
    $scope.InstgasaPreparedandmadeavailabletoPP = 0;
    $scope.IstgasaSubmittedtoCourt = 0;

    }



    $scope.GetCCSComplaintStatusReport = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DatabyDistricts = [];
        var districtId = $scope.CCSReport.DistrictId == undefined ? "0" : $scope.CCSReport.DistrictId;
        var CCSId1 = $scope.CCSReport.CCSId1 == undefined ? "0" : $scope.CCSReport.CCSId1;
       // alert(districtId);

       // 23427
      //  $http.get(SiteUrl + "/MIS/GetCCSComplaintStatusReport?DistrictId=" + districtId    ). "&Css=" + CCSId1 */ 
        $http.get(SiteUrl + "/MIS/GetCCSComplaintStatusReport?DistrictId=" + districtId + "&Css=" + CCSId1 ).
            success(function (data, status) {
                $scope.clean();
                $scope.DatabyDistricts = data;
                $scope.DatabyDistricts.forEach(function (item, indx) {
                    $scope.Total += parseInt( item.Total);
                    $scope.NoticeIsssue += parseInt( item.NoticeIsssue);
                    $scope.IstagasaIssue += parseInt( item.IstagasaIssue);
                    $scope.NotRelevant += parseInt( item.NotRelevant);
                    $scope.DisposeAsPerRequestFromComplainer += parseInt( item.DisposeAsPerRequestFromComplainer);
                    $scope.DisposeByCort += parseInt( item.DisposeByCort);
                    $scope.Reject += parseInt( item.Reject);
                    $scope.Dispose += parseInt(item.Dispose);
                    $scope.InstgasaPreparedandmadeavailabletoPP += parseInt(item.InstgasaPreparedandmadeavailabletoPP);               
                    $scope.IstgasaSubmittedtoCourt += parseInt(item.IstgasaSubmittedtoCourt);
                   
                  //  $scope.GetCCSListNew();
                })
                


                $scope.IsDistrictwiseNot = false;
                $scope.IsDistrictwise = true;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Districts = '';
            });
    };

  

    //Get District List
    $scope.GetDistricts = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {

                $scope.Districts = data;

                $scope.CCSReport.DistrictId = $scope.CCSReport.DistrictId;
            }).
            error(function (data, status) {
                $scope.Districts = '';
            });
    };

    $scope.BackToList = function ()
    {
        $scope.IsnotShow = false;
    }
    $scope.IsMultiState = false;

    $scope.GetCCSList = function (ActionId, item) {
        
        $scope.DISTRICT_ID = item.DISTRICT_ID;
        $scope.ReportListType = ActionId;
        $scope.IsMultiState = item.DISTRICT_ENG.indexOf('Multi State') > -1 ? 1 : item.DISTRICT_ENG.indexOf('Single State') > -1 ? 2 : 0;
        $scope.searchRequest($scope.GetCCSCompalainteReportList);
    }

    $scope.GetCCSListNN = function ( item) {
        $scope.DISTRICT_ID = item.DISTRICT_ID;
        //$scope.ReportListType = ActionId;
       // $scope.IsMultiState = item.DISTRICT_ENG.indexOf('Multi State') > -1 ? 1 : item.DISTRICT_ENG.indexOf('Single State') > -1 ? 2 : 0;
        $scope.searchRequest($scope.GetCCSCompalainteReportList);
    }


    $scope.GetCCSCompalainteReportData = [];
    $scope.GetCCSCompalainteReportList = function () {
        $(".blockUI").fadeIn("slow");
        var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetActionIdCCSComplaintReport", JSON.stringify({ Details: jsonParameters, ActionId: $scope.ReportListType, DistrictId: (districtId == 0 ? $scope.DISTRICT_ID : districtId), IsRowCount: false, Iscountreport: '1', MultiState: $scope.IsMultiState })).

            //$http.get(SiteUrl + "/MIS/GetActionIdCCSComplaintReport?ActionId=" + ActionId + "&DistrictId=0" ).
            success(function (data, status) {
                
                $scope.GetCCSListNew();
               // $scope.GetCCSListNN();
                $scope.IsnotShow = true;
                // $scope.OrderList = [];
                $scope.GetCCSCompalainteReportData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();

            }).
            error(function (data, status) {
                $scope.GetCCSCompalainteReportData = [];
            });
    };
    
   $scope.CCSList = [];
    $scope.GetCCSListNew = function () {
        
        $(".se-pre-con").fadeIn("slow");
        var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;

        if (districtId == 0) {
           
            districtId= $scope.CCSReport.DistrictId == undefined ? "0" : $scope.CCSReport.DistrictId;
           // districtId = $scope.CCSReport.DistrictId;
        }
         //CCSReport.DistrictId
        $http.post(SiteUrl + "/MIS/GetCCSData", { ActionId: $scope.IsMultiState == 1 ? 2 : $scope.IsMultiState==2?1:0, DistrictId: (districtId == 0 ? $scope.DISTRICT_ID : districtId) }).        
            success(function (data, status) {
                            
               // $scope.CCSList = (data.DataList);
                $scope.CCSList = (data);
                $(".se-pre-con").fadeOut("slow");
                console.log (data);
              
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $scope.CCSList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
   // $scope.re = false;
    $scope.GetCCSListCall = function (District) {
        
        $(".se-pre-con").fadeIn("slow");
       // var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;
        $scope.CCSReport.CCSId1 = '0';
        //CCSReport.DistrictId
        $http.post(SiteUrl + "/MIS/GetCCSData", { ActionId: $scope.IsMultiState == 1 ? 2 : $scope.IsMultiState == 2 ? 1 : 0, DistrictId: District }).
            success(function (data, status) {

                // $scope.CCSList = (data.DataList);
                $scope.CCSList = (data);
                $(".se-pre-con").fadeOut("slow");
                console.log(data);

            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $scope.CCSList = [];
                $(".se-pre-con").fadeOut("slow");
            });

      //  $scope.re = true;
        $scope.GetCCSComplaintStatusReport();

    };


    $scope.GetCCSListNew();
    //Get Compliant Report With District Id
    $scope.GetCompliantReportWithDistrictIdList = [];
    $scope.GetCompliantReportWithDistrictId = function () {
        alert($scope.Districts.DistrictId);

        $http.post(SiteUrl + "/MIS/GetActionIdCCSComplaintReport?ActionId=0" + "&DistrictId=" + $scope.Districts.DistrictId).
            success(function (data, status) {
                $scope.GetActionIdCCSComplaintReport = data.Table;
                //  $scope.GetCCSCompalainteReportList($scope.Districts.DistrictId);
                //$scope.tblgetsuport.show();
                //$scope.DivShowDetail = true;
            }).
            error(function (data, status) {
                $scope.GetCCSCompalainteReportData = [];
            });

    };

    $scope.getcompid = function (id) {
        $scope.contactNo = id.MobileNo;
        $scope.ComplaintIds = id;
    }

    $scope.Checkboxvalue = function () {
        $scope.showGenratebutton = $scope.CCSComplaintList.filter(function (x) { return x.Ischecked }).length > 0;

    }

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
                            $scope.Isnext = true;
                            $scope.Isnext2 = true;
                            $scope.IsnotShow = true;
                            i++;
                        }
                        else if (value.ActionId == "1" || value.ActionId == "2") {
                            $scope.Isnext = false;
                            $scope.IsnotShow = false;
                            $scope.Isnext2 = false;
                            $scope.Isnext3 = false;
                            $scope.IsnotShow = true;
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

    $scope.OnPageLoad();

});