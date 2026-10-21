
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

   
    // on page load
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.PLDBListData();
                    $("#ddrDistrictId").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();

    $scope.GetOTSList = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/PLDB/GetOTSList" , JSON.stringify({ Details: jsonParameters })).
            //$http.get(SiteUrl + "/ApplicationFlow/GetSocietyAppliFlowList").
            success(function (responcedata, status) {
                var data = JSON.parse(responcedata.DataList);

                $scope.OTSList = data;
                $rootScope.DataPagingDetails = responcedata.DataPaging;
                $rootScope.SetDataPaging();

                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $(".se-pre-con").fadeOut("slow");
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };

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
      
        $scope.showLoader = false;
        $scope.GetOTSList();
    };

   
    $scope.OnPageLoad();


    //madhuri

    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";        

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" +( $scope.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;


                if ($("#hdnPldbId").val() != "0") {
                    $scope.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }

            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.PLDB_ID??0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.PLDB_Branch_ID = $("#hdnPldbBranchId").val();                    
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }


                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.toggleSelection = function (OTS) {
        
        if (!$scope.SelectedOTSList) {           
            $scope.SelectedOTSList = [];
        }    
        if (OTS.selected) {          
            $scope.SelectedOTSList.push(OTS);
        } else {            
            $scope.CheckedAll = false;
            $scope.SelectedOTSList = $scope.SelectedOTSList.filter(
                member => member.LoanAccNo !== OTS.LoanAccNo
            );
        }
        if ($scope.SelectedOTSList.length > 0)
            $scope.chkBulkSelected = true;
        else $scope.chkBulkSelected = false;
    };

    $scope.CheckedAllOTS = function () {      
        $scope.SelectedOTSList = [];     
        if ($scope.CheckedAll) {
            angular.forEach($scope.OTSList, function (item) {
                item.selected = true;
            });
            $scope.SelectedOTSList = $scope.OTSList;
        }
        else {
            angular.forEach($scope.OTSList, function (item) {
                item.selected = false;
            });

        }

        if ($scope.SelectedOTSList.length > 0)
            $scope.chkBulkSelected = true;
        else $scope.chkBulkSelected = false;
      //  $scope.DownloadMultipleOTSForm();
    }

    $scope.downloadFile = function (OTS) {
        angular.forEach($scope.OTSList, function (item) {
            item.selected = false;
        });
        $scope.SelectedOTSList = [];
        $scope.SelectedOTSList.push(OTS);
        $scope.DownloadMultipleOTSForm();
    }
    

    $scope.DownloadMultipleOTSForm = function () {      
        if ( $scope.SelectedOTSList.length <=0) {
            alert("Please select Pdf which should be download");
            return;
        }
        $scope.SelectedOTS = {};
        $scope.SelectedOTS.PLDB_OTSSchemeData = $scope.SelectedOTSList;

        $http.post(SiteUrl + "/PLDB/BulkOTSFilesGenerated", { "_Model": $scope.SelectedOTS }, {
            responseType: 'arraybuffer'
        }).

            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.byteLength > 0 ) {

                    const blob = new Blob([data], { type: 'application/pdf' });

                    // Create a download link
                    const url = URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    var filename = "";
                    if ($scope.SelectedOTSList.length == 1) {
                         filename = $scope.SelectedOTSList[0].ApplicantName + "_" + $scope.SelectedOTSList[0].LoanAccNo.replace(/\//g, '-')+".pdf";                       
                    }
                    else if ($scope.SelectedOTSList.length > 1 && $("#hdnPldbBranchId").val() != "0") {
                        filename = $scope.SelectedOTSList[0].PLDBBranch + "OtsApplicationList.pdf";

                    }
                    else if ($scope.SelectedOTSList.length > 1 && $("#hdnPldbId").val() != "0") {
                        filename = $scope.SelectedOTSList[0].PLDBName + "OtsApplicationList.pdf";

                    }
                    else {
                        filename = "BulkOtsApplicationList.pdf";
                    }
                    a.download = filename;
                    document.body.appendChild(a); // Needed for Firefox
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);

                    angular.forEach($scope.OTSList, function (item) {
                        item.selected = false;
                    });
                    $scope.SelectedOTSList = [];
                }              
                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });


    }

   // $scope.PLDBList();
    $scope.UploadeReportFileInDB = function (OTS) {
        

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        if (OTS.DocId==0)
        OTS.DocId = 1;//AcceptanceLetter
       // $scope.OTS.id = 0;
        formData.append("id", 0);
        formData.append("DocId", OTS.DocId);
        formData.append("OTSId", OTS.PLDB_OTS_SchemeID);        
        formData.append("OTSReportFile", $scope.OTSReportFile);
      
        $http.post(SiteUrl + "/PLDB/SaveOTSVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //$scope.ApplicationFlowMaster.DocumentURL = data.fileName;
                //$scope.ApplicationFlowMaster.DocId = data.docId;
                $scope.GetOTSList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.UploadReportFile = function (OTS, file) {      
        if (!confirm('Are you sure you want to upload file?')) {
            var fileInputs = document.getElementsByClassName('fileuploader');

            for (var i = 0; i < fileInputs.length; i++) {
                fileInputs[i].value = '';
            }
            return;
        }


        var maxSize = 5 * 1024 * 1024; // element.getAttribute("checkmaxsize") 
        if (file[0].size > maxSize) { return; }

        if (file) {
            if (file[0].type === 'application/pdf') {
                $scope.OTSReportFile = file[0];
                $scope.UploadeReportFileInDB(OTS);
            } else {
                alert('Please upload a PDF file only.');
            }
        }

    }

   

    $scope.Redirect = function (eId,type) {
        if (eId != '') {      

            if (type =='PreLoanPayment')
                window.location = "../PLDB/PldbPrLoanPayment?otsEncId=" + eId;
            else if (type == 'Edit')
                window.location = "../PLDB/OTSApplicationForm?eId=" + eId;
            else if (type == 'View')
                window.location = "../PLDB/OTSApplicationForm?eId=" + eId + "&mode=v";

            if (type == 'UploadTaamil')
                window.location = "../PLDB/PldbTaamil?otsEncId=" + eId;
                
        }

    }

    
});
