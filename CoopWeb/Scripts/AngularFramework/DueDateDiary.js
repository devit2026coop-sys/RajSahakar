
var MasterMod = angular.module('GccApplicationListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("GccApplicationListCnt", function ($rootScope, $scope, $location, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divClass = false;
    $scope.GccApplicationList = [];
    $scope.TotalGccApplicationList = [];

    const urlParams = new URLSearchParams(window.location.search);
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }
    $scope.search = {};
    $scope.search.LoanBankID = '0';
    //$scope.DataPaging.PageSize = $scope.obj.pageSize ?? 50;

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.obj.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetGccApplicationList(false);
    }


    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,date,input:checkbox').length;
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
                $scope.GetGccApplicationList(false);
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
                callback($scope.SocietyCountId_ss, $scope.DistrictId_ss);
            }
            ++indx;
        });
    }

    $scope.levelId = 0;
   
    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
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

        $scope.levelId = $('#hdnLevelId').val();

        if ($scope.levelId == 1) {
            $scope.LoanBankList();
            $scope.GetGccApplicationList(false);
        }        
        else if ($scope.levelId == 7 || $scope.levelId == 8 || $scope.levelId == 9) {       
            if ($scope.levelId == 7) {            
                $scope.search.LoanBankID = $('#hdnGccId').val();
                $scope.Get_LoanBankBranch();
            }
            else if ($scope.levelId == 8) {
                $scope.search.LoanBranchId = $('#hdnGccId').val();
                $scope.Get_LoanBranch_PACS();
            }
            else if ($scope.levelId == 9) {
                $scope.search.PACS_ID = $('#hdnGccId').val();
            }

            $scope.GetGccApplicationList(false);
            //$scope.searchRequest($scope.GetGccApplicationList(false));
        }
        else
        {

        }
        
    };
    $scope.ApplicationView = function (eId, eRno) {
        if (eId != '' && eRno != '') {          
            var url = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId ;
            window.open(url, '_blank');
        }
    }
    $scope.ApplicationActionView = function (eId, eRno) {
        if (eId != '' && eRno != '') {            
            var url = "../GopalCreditCard/GopalApplicationFormActionView?eId=" + eId;
            window.open(url, '_blank');
        }
    }

    $scope.GetGccApplicationList = function (isExport) {
        $(".se-pre-con").fadeIn("slow");

        var key = "";
        var keyValue = "";
        if ($scope.levelId == 7 || $scope.levelId == 8 || $scope.levelId == 9) {

            var key = "";
            var keyValue = "";

            if ($scope.levelId == 7) {
                key = "CcbId";
                $scope.search.LoanBankID = $('#hdnGccId').val();                
            }
            else if ($scope.levelId == 8) {
                key = "CcbBranchId";
                $scope.search.LoanBranchId = $('#hdnGccId').val();                
            }
            else if ($scope.levelId == 9) {
                key = "PacsId";
                $scope.search.PACS_ID = $('#hdnGccId').val();
            }

            var index = $scope.DataPaging.searchParameters.findIndex(function (p) {  return p.Key === key;});

            if (index !== -1) {
                // If found, replace the value
                $scope.DataPaging.searchParameters[index].Value = $('#hdnGccId').val();
            }
            else {
                var parameter = { Key: key, Value: $('#hdnGccId').val() }
                $scope.DataPaging.searchParameters.push(parameter);
            }
            

            //$scope.GetGccApplicationList(false);
            //$scope.searchRequest($scope.GetGccApplicationList(false));
        }

        if (!isExport)
            $rootScope.DataPagingDetails = {};

        var Exportjson = "";
        if (isExport)
        {
             Exportjson = {
                 "RemoveColumns": ["TotalRecords", "encGccid", "DisbursedDate", "DueDate", "TotalRecoveryAmt", "IsPrePaymentDone","Status"],
                "RenameColumns": {
                    "CCBName": "CCB Name"
                 },
                 "ReportName":"DueDateDiary"
            }
        }

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/MIS/GccLoanRpt", JSON.stringify({ Details: jsonParameters, Exportjson: Exportjson })).
            success(function (responcedata, status) {
                if (isExport) {
                    $scope.downloadFile(responcedata.filename);
                }
                else {
                    if (responcedata.DataList != null && responcedata.DataList.length > 0) {

                        var data = JSON.parse(responcedata.DataList);
                        $scope.GccApplicationList = data; // JSON.parse(data);

                        $rootScope.DataPagingDetails = responcedata.DataPaging;                       
                        $rootScope.SetDataPaging();


                        //if ($scope.DataPaging.currentPageID == 1) {
                        //    $scope.TotalRecords = data[0].TotalRecords;
                        //}
                        //$rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                        //$rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                        //$rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                        //$rootScope.SetDataPaging();
                        $scope.colsort = "";
                        $scope.reverseSort = false;
                        $scope.sortData = function (column) {
                            $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                            $scope.colsort = column;
                        };
                        $scope.getSortClass = function (column) {
                            if ($scope.colsort == column) {
                                return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                            }
                            return '';
                        };
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (responcedata, status) {
               // console.log(data);
                $(".se-pre-con").fadeOut("slow");
                $scope.GccApplicationList = [];
                $scope.TotalGccApplicationList = []
            });
    };
    $scope.exportExcel = function () {
        $scope.GetGccApplicationList(true);
    }
    $scope.downloadFile = function (fileUrl) {
        const fileName = fileUrl.split('/').pop();
        $http({
            url: fileUrl,
            method: 'GET',
            responseType: 'blob'
        }).then(function (response) {
            var blob = new Blob([response.data], { type: response.headers('Content-Type') });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }).catch(function (error) {
            console.error('Error downloading file:', error);
        });
    };

    $scope.LoanBankList = function () {
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=0" ).
            success(function (data, status) {                
                $scope.BankMasterList = data;    
                $scope.search.LoanBranchId = '0';
                $scope.search.PACS_ID = '0';
               
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };
    //document.getElementById('BankID').setAttribute('disabled', 'disabled');
    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?"+ "LoanBankId=" + $scope.search.LoanBankID).
            success(function (data, status) {
                $scope.BankBranches = data;
                $scope.search.LoanBranchId = '0';
                $scope.search.PACS_ID = '0';
                //  $scope.GetGccSummary();
               // $scope.GetGccApplicationList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.search.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;                
                $scope.search.PACS_ID = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.OnPageLoad();
  

});

