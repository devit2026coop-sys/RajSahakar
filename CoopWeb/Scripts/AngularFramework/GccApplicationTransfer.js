
var MasterMod = angular.module('ApplicationFlowModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationFlowCont", function ($scope, $rootScope, $http) {
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.ShowOTPDiv = false;
    $scope.ObjectionFlagBasic = false;
    $scope.ObjectionFlagAdmin = false;
    $scope.ObjectionFlagOffice = false;
    $scope.RoleId = 0;


    //$scope.ClearAppFlowForm = function () {
    //    $scope.ApplicationFlowMaster.Remark = '';
    //    $scope.ApplicationFlowMaster.StatusId = '0';
    //    $scope.ApplicationFlowMaster.OfficeId = '0';
    //    $scope.ReportSocietyFile = '';
    //    $scope.ShowOTPDiv = false;
    //}

    
    $scope.GopalCreditCardModel = {};

    $scope.FromUserData = [];

    $scope.CheckedAll = false; 


    $scope.hdnUserId = $('#hdnUserId').val();
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
       
        if ($('#hdnLevelId').val() == 7) {
            $scope.GopalCreditCardModel.LoanBankID = $('#hdnGccId').val();
            $scope.Get_LoanBankBranch();
        }
        else if ($('#hdnLevelId').val() == 8) {
            $scope.GopalCreditCardModel.LoanBranchId = $('#hdnGccId').val();
            $scope.Get_LoanBranch_PACS();

        }
        else if ($('#hdnRoleId').val() == 10228 || $('#hdnUserId').val()==1) {
            $scope.LoanBankList();
        }
        //else {
        //    $(".se-pre-con").fadeOut("slow");
        //}

        $(".se-pre-con").fadeOut("slow");
    }; 
   

    $scope.LoanBankList = function () {  
        $scope.ApplicationFlowList = [];
        $scope.FromUserList = [];
        $scope.ToUserList = [];
        $scope.ddrFromUser = '';
        $scope.ddrToUser = '';
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + 0).
            success(function (data, status) {
                $scope.BankMasterList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Get_LoanBankBranch = function () {
        $scope.ApplicationFlowList = [];
        $scope.FromUserList = [];
        $scope.ToUserList = [];
        $scope.ddrFromUser = '';
        $scope.ddrToUser = '';
        $(".se-pre-con").fadeIn("slow");
        $scope.PACSNameList = [];
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanBankId=" + $scope.GopalCreditCardModel.LoanBankID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Get_LoanBranch_PACS = function () {
        $scope.FromUserList = [];
        $scope.ToUserList = [];
        $scope.ddrFromUser = '';
        $scope.ddrToUser = '';
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.FromUserList = [];
    $scope.GetUserList = function () {
        $scope.FromUserList = [];
        $scope.ApplicationFlowList = [];
        $scope.ddrFromUser = '';
        $scope.ddrToUser = '';
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GetUserListForGccAssign/?CCB_BankID=" + $scope.GopalCreditCardModel.LoanBankID + "&CCB_BranchID=" + $scope.GopalCreditCardModel.LoanBranchId + "&CCB_PacsID=" + $scope.GopalCreditCardModel.PACS_ID ).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");        
                $scope.FromUserList = JSON.parse(data.DataList);
                console.log($scope.FromUserList);
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                //location.reload();
            });
    }


    $scope.ApplicationFlowList = [];
    $scope.GetUserGccList = function () {
        $scope.ApplicationFlowList = [];
        $scope.CheckedAll = false; 
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GetUserGccList/?UserId=" + ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[0]) + "&MappingID=" + ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[1]) ).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.ApplicationFlowList = JSON.parse(data.DataList);
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                //location.reload();
            });
    }
    $scope.GetTransferToUserList_old = function () {
        $scope.ApplicationFlowList = [];
        $scope.ddrToUser = "";
        var userRoleId = $scope.ddrFromUser.split('-')[1];
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GetTransferToUserList/?UserId=" + ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[0]) + "&MappingID=" + ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[1])).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.ToUserList = JSON.parse(data.DataList);
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                //location.reload();
            });
    }

    $scope.GetTransferToUserList = function () {
        $scope.ApplicationFlowList = [];
        $scope.ddrToUser = "";
        var userRoleId = $scope.ddrFromUser.split('-')[1];

        $scope.ToUserList = $scope.FromUserList.filter(x => (x.IsActive == 1) && x.UserId != ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[0]))

       // $scope.ToUserList = $scope.FromUserList.filter(x => (x.IsActive == 0) && x.UserId != ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[0]))

        //$(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/GopalCreditCard/GetTransferToUserList/?UserId=" + ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[0]) + "&MappingID=" + ($scope.ddrFromUser == undefined ? 0 : $scope.ddrFromUser.split('-')[1])).
        //    success(function (data, status) {
        //        $(".se-pre-con").fadeOut("slow");
        //        $scope.ToUserList = JSON.parse(data.DataList);
        //    }).
        //    error(function (data1, status) {
        //        $(".se-pre-con").fadeOut("slow");
        //        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
        //        //location.reload();
        //    });
    }
    $scope.SelectAllGcc = function () {
        if ($scope.CheckedAll) {
            $scope.ApplicationFlowList.forEach(function (itm) {
                itm.IsChecked = true;
            })
        } else {
            $scope.ApplicationFlowList.forEach(function (itm) {
                itm.IsChecked = false;
            });
        }
    }


    $scope.SelectGcc = function () {
        debugger;
        $scope.CheckedAll = $scope.ApplicationFlowList.filter(function (x) { return x.IsChecked }).length == $scope.ApplicationFlowList.length;
    }
    $scope.TransferGcc = function () {
        
            if ($scope.ddrFromUser == "" || $scope.ddrFromUser == undefined) {
                alert("Please select user");
            } else
            if (!$scope.CheckedAll && $scope.ApplicationFlowList.filter(function (x) { return x.IsChecked }).length <= 0) {
                 alert("Please select Application");
            } else {
                var GccIDS = "";

                if ($scope.CheckedAll) {
                    if (!confirm("Are you sure you want to move all applications?")) {
                        return;
                    }
                    else {
                        $scope.ApplicationFlowList.filter(function (x) { return x.IsChecked }).forEach(function (itm) {
                            GccIDS += (GccIDS == "" ? "" : ",") + itm.GccId;
                        });
                    }
                    
                }

                 if (!$scope.CheckedAll) {
                     $scope.ApplicationFlowList.filter(function (x) { return x.IsChecked }).forEach(function (itm) {
                         GccIDS += (GccIDS == "" ? "" : ",") + itm.GccId;
                     });
                 }
                 
                $(".blockUI").show();
                
                var requestData = {
                    UserId: $scope.ddrToUser.split('-')[0],
                    OldUserId: $scope.ddrFromUser.split('-')[0],
                    GccIDS: GccIDS
                };

                // Make the POST request
                $http.post(SiteUrl + "/GopalCreditCard/TransferGcc", requestData)
                    .then(
                        function successCallback(response) {
                            $(".se-pre-con").fadeOut("slow");

                            if (response.data == 1) {
                                alert(_language == 'en-us'? 'Application Transfer Successfully \n': 'आवेदन सफलतापूर्वक स्थानांतरित हुआ| \n');
                                $scope.ApplicationFlowList = [];
                                $scope.ToUserList = [];
                                $scope.FromUserList = [];
                            }
                        },
                        function errorCallback(error) {
                            $(".se-pre-con").fadeOut("slow");
                            alert(_language == 'en-us' ? 'Some Error is Occured \n': 'कुछ त्रुटि हुई है \n');
                        }
                    );





                //$http.post(SiteUrl + "/GopalCreditCard/TransferGcc/?UserId=" + $scope.ddrToUser.split('-')[0] + "&OldUserId=" + $scope.ddrFromUser.split('-')[0] + "&GccIDS=" + GccIDS).
                //    success(function (data, status) {
                //        $(".se-pre-con").fadeOut("slow");
                      
                //        if (data ==1) {
                //            alert(_language == 'en-us' ? 'Application Transfer Successfully \n' : 'आवेदन सफलतापूर्वक स्थानांतरित हुआ| \n');
                //            $scope.ApplicationFlowList = [];
                //            $scope.ToUserList = [];
                //            $scope.FromUserList = [];
                //        }
                //    }).
                //    error(function (data1, status) {
                //        $(".se-pre-con").fadeOut("slow");
                //        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                //        //location.reload();
                //    });
             }
    }
    $scope.ApplicationView = function (eId) {
        if (eId != '' ) {      
            var url = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId;
            window.open(url, '_blank');
        }
    }

    $scope.OnPageLoad();

});






