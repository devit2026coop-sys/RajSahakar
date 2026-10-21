var AppClasses = angular.module("ApprovalGrievanceMod", ['angularUtils.directives.dirPagination']);

AppClasses.controller("ApprovalGrievanceCnt", function ($scope, $http, $rootScope) {




    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }


    $rootScope.pageRequest = function (pageid, callback) {
        ;
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
        ;
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select').each(function () {
            ;
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


    $scope.ClearSearch = function () {
        $scope.SearchValue = {
            Statusid: '0',
            GCategoryId: '0',
            SearchTodate: '',
            Searchfromdate: '',
            SocietyName: ''
        }
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetGrievanceList();
    }



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
       }
    $scope.Reopenshow = false;

    $scope.ApprovalProcess = function (item) {

        ;
        if (item.CurrentStatusId == 5) {
            $scope.Reopenshow = true;
        }
        else {
            $scope.Reopenshow = false;
        }
        $scope.ComplainStatus = '';
        $scope.Description = '';
        $scope.FileName = '';
        $scope.IsUploadFileName = false;
        $scope.FileName = "";
        $scope.FileType = "";
        $scope.GrievanceId = item.GrievanceId;
        $('#ApprovalList').modal('show');
    }

    $scope.Trailhistroy = function (item) {
        ;
        $scope.GrievanceId = item.GrievanceId;
        $scope.GetGrievanceHistory($scope.GrievanceId);
        $('#TrailList').modal('show');
    }


    $scope.GetGrievanceHistory = function (GrievanceId) {
        ;
        $scope.GrievancHistoryList = [];
        $http.get(SiteUrl + "/User/GetGrievanceHistory/?GrievanceId=" + GrievanceId).
            success(function (data, status) {
      
                $scope.GrievancHistoryList = data;
            }).
            error(function (data, status) {
                ShowModelAlert("Grievance", "Error While Fetching Grievance List");
                $scope.GrievancHistoryList = [];
            });
    };


    $scope.SaveGrievanceApproval = function () {
        
    
        if ($scope.FormValidation() == false) {
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("ApprovalStatus", $scope.ComplainStatus);
        formData.append("Description", $scope.Description);
        formData.append("AttachmentFileName", $scope.FileName);
        $http.post(SiteUrl + "/User/SaveGrievanceApproval", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                ;
                if (data > 0) {
                    ShowModelAlert("Grievance Approval Process", "Save Sucessfully");
                    $scope.GetGrievanceList();
                    $('#ApprovalList').modal('hide');
                    $(".se-pre-con").fadeOut("slow");
                }
                else
                    ShowModelAlert("Grievance", "Error While Save Grievance");
                    $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowModelAlert("Grievance", "Error While Save Grievance");

            });

    }


    $scope.GrievanceCategoryList = [];
    $scope.GetGrievanceCategoryList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GrievanceCategory").
            success(function (data, status) {
                $scope.GrievanceCategoryList = data;
            }).
            error(function (data, status) {
                ShowModelAlert("Grievance", "Error While Fetching Grievance Category List");
                $scope.GrievanceCategoryList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.GetGrievanceCategoryList();

    $scope.FormValidation = function () {
      
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.ComplainStatus == undefined || $scope.ComplainStatus == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Complain Status' : '\n <br /> कृपया शिकायत दर्ज करें|';
        }

        if ($scope.Description == undefined || $scope.Description == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Description Details' : '\n <br /> कृपया  विवरण दर्ज करें |';
        }
       
        //if ($scope.FileName == undefined || $scope.FileName == '') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select File ' : '\n <br /> कृपया फाइल अपलोड करें|';
        //}
        if (message.length > 29) {
            ShowModelAlert("Complain Process", message);
            return false;
        }
        else
            return true;

    }
    $scope.SearchValue = {
        Statusid: '0',
        GCategoryId: '0',
        SearchTodate: '',
        Searchfromdate: '',
        SocietyName: ''
    }


    $scope.GetGrievanceList = function () {
        ;

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Citizen/GetGrievanceList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                ;
                $scope.GrievanceDetailList = JSON.parse(data.DataList);
                //$scope.GetApprovedSocietyACT2001List = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
            }).
            error(function (data, status) {
                ShowModelAlert("Grievance", "Error While Fetching Grievance List");
                $scope.GrievanceDetailList = [];
            });
    };



    $scope.GetGrievanceList();



    $scope.IsUploadFileName = false;
    $scope.FileName = "";

    $scope.FileType = "";

    $scope.AddFile1 = function (type) {
        
        $scope.FileType = type;
        $scope.IsUploadFileName = true;
    }
    $scope.ChangeFile1 = function (type) {
        
        if (type == 1) {
            $scope.IsUploadFileName = false;
            $scope.FileName = "";
        }
    }
    $scope.UploadFile_new = function (id, type) {
        
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                //  $scope.xhr1(SiteUrl + "/SocietyComplaintCourtModule/UploadComplainFile", formData, type);
                $scope.xhr1(SiteUrl + "/User/UploadGrievanceFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr1 = function (url, data, type) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        if (type == 1) {
                            $scope.FileName = request.responseText;
                            //  $scope.Grievance.FileName =  request.responseText;
                        }
                        $scope.$apply();
                    }
                }
            };

            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    $scope.Societyapplicationview = function (SocietyIDInc) {
        if (SocietyIDInc != '') {
            window.location = "../User/SocietyApplicationView?SocietyIDEnc=" + SocietyIDInc + "~_Citizen_SocietyProfile~undefined";
        }

    }

});


AppClasses.directive('validfile1', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 
                        if (file >= 4096) {
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {
                            if (scope.FileType == 1) {
                                scope.IsUploadFileName = true;
                                scope.FileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };



});