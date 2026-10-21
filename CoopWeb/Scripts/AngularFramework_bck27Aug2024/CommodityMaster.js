var CommodityMasterMod = angular.module('CommodityMasterModule', ['angularUtils.directives.dirPagination']);
CommodityMasterMod.controller("CommodityMasterCont", function ($scope, $http, $rootScope) {


    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 10,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $scope.CommodityMaster = {};
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


    $scope.ClearSearch = function () {
       // $scope.CommodityMaster.SearchText = "";
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetCommoditymasterList();
    }
    /*------------End Paging and Filter-----------------*/


    $scope.OnPageLoad = function () {
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
    // END : Page load Function

    $scope.divShowdata = true;
    $scope.AddNew = function () {
        $scope.divInsert = true;
        $scope.divShowdata = false;
        $scope.CommodityMaster.CommodityNameEn = '';
        $scope.CommodityMaster.CommodityNameHi = '';
        $('#IsKharif').prop("checked", false);
        $('#IsActive').prop("checked", false);
    };


    $scope.CommodityMaster.Flag = 1;
    $scope.SaveCommodityMaster = function () {
        
        if ($scope.ValidateForm()) {
            $http.post(SiteUrl + "/Stock/InsertCommodityMaster", { "model": $scope.CommodityMaster }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if ($scope.CommodityMaster.Flag==1) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Commodity master successfully submited." : "कमोडिटी मास्टर  सफलतापूर्वक सबमिट किया गया", function () {
                            $scope.GetCommoditymasterList();
                            $scope.divInsert = false;
                            $scope.divShowdata = true;
                        });
                    }
                    else if ($scope.CommodityMaster.Flag == 2) {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Commodity master successfully updated." : "कमोडिटी मास्टर सफलतापूर्वक अपडेट किया गया", function () {
                            $scope.GetCommoditymasterList();
                            $scope.divInsert = false;
                            $scope.divShowdata = true;
                        });
                    }
                    else {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
      
    }
    $scope.EditRecord = function (item) {
        $scope.divInsert = true;
        $scope.divShowdata = false;
        $scope.CommodityMaster.CommodityId = item.CommodityId;
        $scope.CommodityMaster.CommodityNameEn = item.CommodityNameEn;
        $scope.CommodityMaster.CommodityNameHi = item.CommodityNameHi;
        $scope.CommodityMaster.IsKharif = item.IsKharif

        if (item.IsActive == 1) {
            $scope.CommodityMaster.IsActive = true;
        }
        else {
            $scope.CommodityMaster.IsActive = false;
        }
        $scope.CommodityMaster.Flag = 2;

    }

    $scope.ChangeStatus = function (item, flag) {
        debugger
        $scope.CommodityMaster.CommodityId = item.CommodityId;
        $scope.CommodityMaster.IsActive = item.IsActive;
        $scope.CommodityMaster.Flag = flag;
        $http.post(SiteUrl + "/Stock/InsertCommodityMaster", { "model": $scope.CommodityMaster }).
            success(function (data, status) {
                
                $(".blockUI").fadeOut("slow");
                if (flag==3) {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Commodity Master successfully Inactive." : "कमोडिटी मास्टर सफलतापूर्वक निष्क्रिय किया गया", function () {
                        $scope.GetCommoditymasterList();
                        $scope.divInsert = false;
                        $scope.divShowdata = true;
                    });
                }
                else if (flag == 4) {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Commodity Master successfully Deleted." : "कमोडिटी मास्टर सफलतापूर्वक हटा दिया गया।", function () {
                        $scope.GetCommoditymasterList();
                        $scope.divInsert = false;
                        $scope.divShowdata = true;
                    });
                }
                else {
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                }
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
            });

    }

    $scope.ValidateForm = function () {
        var message = '';
        if ($scope.CommodityMaster.CommodityNameEn === "" || $scope.CommodityMaster.CommodityNameEn === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Commodity Name (in Eng) ' : '\n <br /> कृपया कमोडिटी नाम (इंग्लिश) दर्ज करें |';
        }
        if ($scope.CommodityMaster.CommodityNameHi == undefined || $scope.CommodityMaster.CommodityNameHi === "") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Commodity Name (in Hindi)' : '\n <br /> कृपया कमोडिटी नाम (हिंदी) दर्ज करें  |';
        }
        if (message.length > 5) {
            ShowModelAlert($scope.translation.CommodityMaster, message);
            return false;
        }
        return true;
    }

    $scope.CommoditymasterList = [];
    $scope.GetCommoditymasterList = function () {
        $(".blockUI").fadeIn("slow");
        $scope.PriceList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetCommoditymaster", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.CommoditymasterList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.CommoditymasterList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    }
    $scope.CommodityPrice = {};


    $scope.AddpriceCommudity = function (item) {
        $scope.priceshowfaq = true;
        $scope.priceshowURS = true;
        $scope.CommodityPrice.FinancialYear = '0';
        $scope.CommodityPrice.PriceFAQ = ''; 
        $scope.CommodityPrice.PriceURS = '';
        $('#IsActivePrice').prop("checked", false);
        $scope.CommodityPrice.CommodityId = item.CommodityId;
        $scope.GetFinancialYear();
        $scope.GetCommodityPrice($scope.CommodityPrice.CommodityId);
        $('#myModal').modal('show');
    }

    $scope.priceshowfaq = true;
    $scope.priceshowURS = true;
    $scope.Flag_price = 1;
    $scope.EditRecordPrice = function (item) {
        $scope.Flag_price = 2;
        $scope.CommodityPrice.FinancialYear = item.FYEAR_ID.toString();
        $scope.CommodityPrice.IsActive = item.IsActive;
        if (item.IsActive == 1) {
            $scope.CommodityPrice.IsActive = true;
            $("#IsActivePrice").prop("checked", true); 
        }
        else {
            $scope.CommodityPrice.IsActive = false;
        }
        $scope.CommodityPrice.Msp_Id = item.Msp_Id;
        $scope.CommodityPrice.CommodityId = item.CommodityId;
        $scope.CommodityPrice.UnitId = item.UnitId;
        if (item.UnitId == 1) {
            $scope.CommodityPrice.PriceFAQ = item.Price;
            $scope.priceshowfaq = true;
            $scope.priceshowURS = false;
        }
        else {
            $scope.CommodityPrice.PriceURS = item.Price;
            $scope.priceshowfaq = false;
            $scope.priceshowURS = true;
        }
    }
   
    $scope.SavePriceCommudity = function () {

        
        $scope.CommodityPrice.Flag = $scope.Flag_price;
        if ($scope.ValidatePriceForm()) {
            $http.post(SiteUrl + "/Stock/InsertCommodityPrice", { "model": $scope.CommodityPrice }).
                success(function (data, status) {
                    if (data.Result == true) {
                        if ($scope.CommodityPrice.Flag == 1) {
                            $scope.GetCommodityPrice($scope.CommodityPrice.CommodityId);
                            $scope.CommodityPrice.FinancialYear = '0';
                            $scope.CommodityPrice.PriceFAQ = '';
                            $scope.CommodityPrice.PriceURS = '';
                            $("#IsActivePrice").prop("checked", false); 
                        }
                        if ($scope.CommodityPrice.Flag == 2) {
                            $scope.GetCommodityPrice($scope.CommodityPrice.CommodityId);
                            $scope.CommodityPrice.FinancialYear = '0';
                            $scope.CommodityPrice.PriceFAQ = '';
                            $scope.CommodityPrice.PriceURS = '';
                            $scope.priceshowfaq = true;
                            $scope.priceshowURS = true;
                            $scope.Flag_price = 1;
                            $("#IsActivePrice").prop("checked", false); 
                        }
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
             }
    }


    $scope.CancelPriceCommudity = function () {
        $scope.CommodityPrice.FinancialYear = '0';
        $scope.CommodityPrice.PriceFAQ = '';
        $scope.CommodityPrice.PriceURS = '';
        $scope.priceshowfaq = true;
        $scope.priceshowURS = true;
        $scope.Flag_price = 1;
        $("#IsActivePrice").prop("checked", false);
        $('#myModal').modal('hide');
    }
    $scope.ValidatePriceForm = function () {
        var message = '';
        if ($scope.Flag_price==1) {
            if ($scope.CommodityPrice.FinancialYear === "" || $scope.CommodityPrice.FinancialYear === undefined || $scope.CommodityPrice.FinancialYear=='0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Enter FinancialYear ' : '\n <br /> कृपया FinancialYear दर्ज करें |';
            }
            if ($scope.CommodityPrice.PriceFAQ == undefined || $scope.CommodityPrice.PriceFAQ === "") {
                message += $scope._language === 'en-us' ? '\n <br /> Please Enter FAQ Price' : '\n <br /> कृपया FAQ मूल्य दर्ज करें |';
            }

            if ($scope.CommodityPrice.PriceURS == undefined || $scope.CommodityPrice.PriceURS === "") {
                message += $scope._language === 'en-us' ? '\n <br /> Please Enter URS Price' : '\n <br />  कृपया URS मूल्य दर्ज करें|';
            }
        }
        if ($scope.Flag_price == 2) {
            if ($scope.CommodityPrice.UnitId == 1) {
                if ($scope.CommodityPrice.FinancialYear === "" || $scope.CommodityPrice.FinancialYear === undefined || $scope.CommodityPrice.FinancialYear == '0') {
                    message += $scope._language === 'en-us' ? '\n <br /> Please Enter FinancialYear' : '\n <br /> कृपया Financial Year दर्ज करें |';
                }
                if ($scope.CommodityPrice.PriceFAQ == undefined || $scope.CommodityPrice.PriceFAQ === "") {
                    message += $scope._language === 'en-us' ? '\n <br /> Please Enter FAQ Price' : '\n <br /> कृपया FAQ मूल्य दर्ज करें |';
                }
            }
            else {
                if ($scope.CommodityPrice.FinancialYear === "" || $scope.CommodityPrice.FinancialYear === undefined || $scope.CommodityPrice.FinancialYear == '0') {
                    message += $scope._language === 'en-us' ? '\n <br /> Please Enter FinancialYear' : '\n <br /> कृपया FinancialYear दर्ज करें|';
                }
                if ($scope.CommodityPrice.PriceURS == undefined || $scope.CommodityPrice.PriceURS === "") {
                    message += $scope._language === 'en-us' ? '\n <br /> Please Enter URS Price' : '\n <br /> कृपया URS मूल्य दर्ज करें |';
                }
            }
        }
        if (message.length > 5) {
            ShowModelAlert($scope.translation.CommodityPrice, message);
            return false;
        }
        return true;
    }


    $scope.ChangeStatusPrice = function (item, flag) {
        $scope.CommodityPrice.IsActive = item.IsActive;
        $scope.CommodityPrice.Msp_Id = item.Msp_Id;
        $scope.CommodityPrice.CommodityId = item.CommodityId;
        $scope.CommodityPrice.Flag = flag;
        $http.post(SiteUrl + "/Stock/InsertCommodityPrice", { "model": $scope.CommodityPrice }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                if (flag == 3) {
                    //ShowOkMessage("Success", $scope._language == 'en-us' ? "Commodity Price successfully Inactive." : "कमोडिटी मास्टर सफलतापूर्वक निष्क्रिय किया गया", function () { //});
                    $scope.GetCommodityPrice();
                }
                 if (flag == 4) {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Commodity Price successfully Deleted." : "कमोडिटी मास्टर सफलतापूर्वक हटा दिया गया।", function () {
                        $scope.GetCommodityPrice(); });
                  }
                //else {
                //    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                //}
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
            });

    }

    $scope.CommodityPriceList = [];

    $scope.GetCommodityPrice = function (CommodityId) {
        
        $(".blockUI").fadeIn("slow");
        $scope.CommodityPriceList = [];
     
        return $http.post(SiteUrl + "/Stock/GetCommodityPriceList", { CommodityId: CommodityId }).
            success(function (data, status) {
                $scope.CommodityPriceList = JSON.parse(data.DataList);
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CommoditymasterList = [];
                $(".blockUI").fadeOut("slow");
            });
    }


        $scope.GetFinancialYear = function ()
        {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
            };

    $scope.OnPageLoad();
    $scope.GetCommoditymasterList();

});