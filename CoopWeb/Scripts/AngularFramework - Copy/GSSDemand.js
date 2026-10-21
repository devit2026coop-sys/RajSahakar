var AppClasses = angular.module("GSSDemandMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("GSSDemandCnt", function ($scope, $http, $rootScope) {
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
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GSSDemandList();
    }
    /*------------End Paging and Filter-----------------*/


    $scope._language = 'en-us';
    $scope.SocietyList = [];
    $scope._language = _language;
    $scope.MasterModel = {
        SocietyID: "0",
        Quantity: "",
        FertilizerId: "0",
        RoutChart:""
    }
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

    $scope.DemandList = [];
    $scope.GSSDemandList = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/FertilizerSupply/GSSDemandList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
               //$scope.DemandList = data;       
                $scope.DemandList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
            }).
            error(function (data, status) {
                $scope.DemandList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };

    $scope.FertilizerList = [];
    $scope.GetFertilizerList = function () {
       
       return $http.post(SiteUrl + "/FertilizerSupply/GetFertilizerList", { "WhereClause": " " }).
            success(function (data, status) {
              //  $(".blockUI").fadeOut("slow");
                $scope.FertilizerList = data;
                if ($scope.FertilizerList.length == 1) {
                    $scope.MasterModel.FertilizerId = $scope.FertilizerList[0].fertilizer_id.toString();
                    $scope.MasterModel.Unit = $scope._language == 'en-us' ? $scope.FertilizerList[0].unit_en : $scope.FertilizerList[0].unit_hi; 
                }
            }).
            error(function (data, status) {
                $scope.FertilizerList = [];              
                //alert('Error while fetching society list data.');
            });
    };


    $scope.GetSocietyList = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetSocietyList", { "WhereClause": " " }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyList = data;
                if ($scope.SocietyList.length == 1) {
                    $scope.MasterModel.SocietyID = $scope.SocietyList[0].SocietyID.toString();
                }
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };

    $scope.GSSDemandList().then(function () {
        $scope.GetFertilizerList().then(function () { $scope.GetSocietyList(); });      
    });

    
    $scope.AddNew = function () {
        //$scope.ResetAddNew();
        $("#addModal").modal("show");     
    }
    $scope.SaveDemand = function () {
        $("#SocietyID").removeClass("notValid");
        $("#FertilizerId").removeClass("notValid");
        $("#Quantity").removeClass("notValid");
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.SocietyID == "0") {
            message += $scope._language == 'en-us' ? '\n <br />Please Choose Society.' : '\n <br />कृपया सोसायटी चुनें।';
            $("#SocietyID").addClass("notValid");
        }
        if ($scope.MasterModel.FertilizerId == "0") {
            message += $scope._language == 'en-us' ? '\n <br />Please Choose Fertilizer.' : '\n <br />कृपया उर्वरक चुनें।';
            $("#FertilizerId").addClass("notValid");
        }
        if ($scope.MasterModel.Quantity == "") {
            message += $scope._language == 'en-us' ? '\n <br />Please Enter Quantity.' : '\n <br />कृपया मात्रा दर्ज करें।';
            $("#Quantity").addClass("notValid");
        }
      

        if (message.length > 29) {
            ShowModelAlert("GSS Demand", message);
            return false;
        } else {

            $(".blockUI").fadeIn("slow");
            var data = {
                SocietyId: $scope.MasterModel.SocietyID,
                Quantity: $scope.MasterModel.Quantity,
                FertilizerId: $scope.MasterModel.FertilizerId,
                RequestId: 1,
                RoutChart: $scope.MasterModel.RoutChart
            }
            $http.post(SiteUrl + "/FertilizerSupply/GSSDemand", data).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $("#addModal").modal("show"); 
                    $scope.ResetAddNew();
                    if (data.Status > 0) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Demand Successfully submited" : "मांग सफलतापूर्वक सबमिट की गई", function () {
                            window.location = "/FertilizerSupply/GSSDemand";
                        });
                    } else {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });

                    }
                }).
                error(function (data, status) {
                    
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });

                    //alert('Error while fetching society list data.');
                });
        }
    };
    $scope.ResetAddNew = function () {
        $scope.MasterModel.SocietyID = "0";
        $scope.MasterModel.Quantity = "";
        $scope.MasterModel.FertilizerId = "0";
        $scope.MasterModel.RoutChart = "";
    }

    $scope.ChangeFertilizer = function () {

        if ($scope.MasterModel.FertilizerId == "0") {
            $scope.MasterModel.Unit = "";
        } else if ($scope._language == 'en-us'){
            $scope.MasterModel.Unit = $scope.FertilizerList.filter(x => x.fertilizer_id.toString() == $scope.MasterModel.FertilizerId.toString())[0].unit_en;
        } else {
            $scope.MasterModel.Unit = $scope.FertilizerList.filter(x => x.fertilizer_id.toString() == $scope.MasterModel.FertilizerId.toString())[0].unit_hi;
        }
       // $scope.MasterModel.Unit = $scope._language == 'en-us' ? $scope.FertilizerList[0].unit_en : $scope.FertilizerList[0].unit_hi;         
    }

});