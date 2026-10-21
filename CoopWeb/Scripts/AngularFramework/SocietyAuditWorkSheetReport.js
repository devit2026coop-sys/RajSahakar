var CitizenModule = angular.module("CitizenSocietyAuditMod", ['angularUtils.directives.dirPagination']);
CitizenModule.controller("CitizenSocietyAuditCnt", function ($scope, $rootScope , $http) {
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


    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.GetDistrictList();

    $scope.AuditWorkSheetList = [];
    $scope.GetAuditWorkSheetReport = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetAuditWorkSheetReport", JSON.stringify({ Details: jsonParameters, Type: 1 })).
           success(function (data, status) {
                // $scope.OrderList = [];
                $scope.AuditWorkSheetList  = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
           }).
            error(function (data, status) {
                $scope.AuditWorkSheetList  = [];
                $(".blockUI").fadeIn("slow");
                console.log('Error while fetching data.');
            });
    };

    $scope.GetAuditWorkSheetReport();

    $scope.checkFileLengthPage = function (fileSized) {

        if (fileSized < 2100000) {
            return true;
        }
        return false;
    } 

    var dt = new Date();
    var year = dt.getFullYear();
    var day = dt.getDate();
    var month = dt.getMonth() + 1;
    $scope.LetterData = {};
    $scope.LetterData.AuditStartDate = day + "/" + month + "/" + year;
    $scope.LetterData.AuditEndDate = day + "/" + month + "/" + year;

    // START : Upload Files
    $scope.UploadEnclouserFile = function (file, row) {
        var DDLIdLocal = $(row).attr("DDLID");
        var DDLIdVal = $("#" + DDLIdLocal).val();

        if (!$scope.checkFileLengthPage(file[0].size)) {
            alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 2 MB or Equivalent" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 2 MB से कम करें|");
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            return;
        } 

        var _Counter = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));

        var rowDataSplitter = DDLIdLocal.split('_');
        
        if (rowDataSplitter[1] == 'PDF') {
            if (!(file[0].name.indexOf(".pdf") > -1)) {
                alert("Please PDF File Only.")
                file[0] = null;
                $(row).val(null);
                return false;
            }
        }
        else if ($(row).attr("filetype") == 'worddocument') {
            if (!(file[0].name.indexOf(".doc") > -1)) {
                alert("You have selected invalid Filetype. \n Valid Filetype .doc,.docx.")
                file[0] = null;
                $(row).val(null);
                return false;
            }
        }
        else if ($(row).attr("filetype") == 'excel') {
            if (!(file[0].name.indexOf(".doc") > -1)) {
                alert("You have selected invalid Filetype. \n Valid Filetype .xlsx")
                file[0] = null;
                $(row).val(null);
                return false;
            }
        }

        var _Title = 'NA';
        if (parseInt(rowDataSplitter[2], 10) > _Counter) {
            _Title = $('#Name' + rowDataSplitter[2]).val() == undefined ? 'NA' : $('#Name' + rowDataSplitter[2]).val();
        }
        if ($scope.FileCollection.length == 0 && file.length > 0) {
            $scope.FileCollection.push({ 'Id': rowDataSplitter[2], 'FileType': rowDataSplitter[1], 'File': file[0], 'Title': _Title });
        }
        else {
            if ($scope.FileCollection.filter(function (X) { return X.Id == rowDataSplitter[2] && X.FileType == rowDataSplitter[1] }).length == 0) {
                $scope.FileCollection.push({ 'Id': rowDataSplitter[2], 'FileType': rowDataSplitter[1], 'File': file[0], 'Title': _Title });
            }
            else {
                if (file.length == 0) {
                    angular.forEach($scope.FileCollection, function (i, item) {
                        if (item.Id == rowDataSplitter[2] && item.FileType == rowDataSplitter[1]) {
                            $scope.FileCollection.splice(i, 1);
                        }
                    });
                }
                else {
                    angular.forEach($scope.FileCollection, function (item) {
                        if (item.Id == rowDataSplitter[2] && item.FileType == rowDataSplitter[1]) {
                            item.File = file[0];
                        }
                    });
                }
            }
        }
    }
    //END : Upload Files
    $scope.AuditSociety = "";

    $scope.ShowEntry = function (item) {
        $scope.AuditSociety = item.SocietyName;
        $scope.AuditSocietyIdEncr = item.SocietyIDEnc;
     
        $("#addModal").modal('show');
    }

    //START: Generate Add More Files DIV
    $scope.GetAddMoreFilesDiv = function () {

        $scope.divMoreFilesAdd = true;
        $scope.records = [];
        var X = $scope.OtherFileUpload.AddMoreFile;
        records = [];
        var _Counter = $scope.MaxCountAddMoreFile = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));
        for (var x = 1; x <= X; x++) {
            records.push(
                {
                    "Id": _Counter + x,
                    "DDLID": "DDLDocTypeID" + (_Counter + x),
                    "UploadFileNamePDF": "",
                    "UploadFileNameXLS": "",
                    "Name": ""
                });
        }
        $scope.records = records;
    }
    $scope.GetAddMoreFilesDivOldData = function () {

        $scope.divMoreFilesAdd = true;
        $scope.records = [];
        var X = $scope.OtherFileUpload.AddMoreFile;
        records = [];
        var _Counter = $scope.MaxCountAddMoreFile = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));
        for (var x = 1; x <= X; x++) {
            records.push(
                {
                    "Id": _Counter + x,
                    "DDLID": "DDLDocTypeID" + (_Counter + x),
                    "UploadFileNamePDF": $scope.recordsOther[x - 1].UploadFileNamePDF,
                    "UploadFileNameXLS": $scope.recordsOther[x - 1].UploadFileNameXLS,
                    "Name": $scope.recordsOther[x - 1].Name
                });
        }
        $scope.records = records;
    }
    //END : Generate Add More Files DIV

    //START :FileUploadTypeList
    $scope.FileUploadTypeList = function () {
        $http.post(SiteUrl + "/Society/GetUploadFileType/").
            success(function (data, status) {
                $scope.FileUploadTypes = data;
            }).
            error(function (data, status) {
                $scope.FileUploadTypes = [];
                alert('Error while File Upload Type List fetch.');
            });
    }
    //END: FileUploadTypeList
    
    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DashboardData();
        $('#AuditStartDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#AuditEndDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $scope._language = _language; // Set LAnguiage
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
            $(".se-pre-con").fadeOut("slow");
        }
        $scope.FileUploadTypeList();


        $scope.getSocietyfilterBySocietyType = function () {
            $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);
        };

        $scope.getSocietyfilter = function () {

            $scope.SocietyRegistration.SocietyTypeId = '0';
            $scope.SocietyRegistration.ClassId = '0';
            $scope.GetSocietyTypeList();
            if ($scope.SocietyRegistration.ActId == 7) {
                $scope.SocietyClassList = [];
            }
            else {
                $scope.GetSocietyClassList();
            }
            $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);

        };
        $scope.getSocietyfilterByClass = function () {

            $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);

        };
        // GET Society Type
        $scope.GetSocietyTypeList = function () {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.SocietyRegistration.ActId).
                success(function (data, status) {
                    $scope.SocietyTypeList = data;
                }).
                error(function (data, status) {
                    $scope.SocietyTypeList = [];

                });
        };
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });


        // GET Society Class
        $scope.GetSocietyClassList = function () {
            $scope.showLoader = true;
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=-1").
                success(function (data, status) {
                    $scope.SocietyClassList = data;
                }).
                error(function (data, status) {
                    $scope.SocietyClassList = [];
                });

        };


        //GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });

        // Get List of all ACT
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
               
            });
   
        $(".se-pre-con").fadeOut("slow");

    };
    // END : Call default functions on page load

    $scope.UploadRegistrationCertificateFile = function (file) {
        $scope.RegistrationCertificateFile = file[0];
    }

    $scope.UploadBylawsFile = function (file) {
        $scope.BylawsFile = file[0];
    }

    $scope.UploadRulesAndRegulationFile = function (file) {
        $scope.RulesAndRegulation = file[0];
    }

    $scope.UploadSocietyAuditFile = function (file) {
        $scope.AuditReport = file[0];
    }
    $scope.SocietyAuditorAssigmentsId = 0;
    $scope.AddAdministrativeDetails = function (SID, ShowTab, SocietyName, FinancialYear, SocietyAuditorAssigmentsId,IsFarm) {
        $scope.showLoader = true;
        $scope.SocietyName = SocietyName;
        $scope.SocietyAuditReport.FinancialYear = FinancialYear;
        $('#DdlFyear').prop('disabled', true);
        $scope.divSocietyAuditReport = true;
        $scope.divTabulation = false;
        $scope.divSocietyAuditReportShow = false;
        $scope.divAddBtn = false;
        $scope.SocietyIdForSocietyAuditDetailsCitizen = SID;
        $scope.GetAuditorAssignReportDataShow();
        $scope.showLoader = false;
        $scope.SocietyAuditorAssigmentsId = SocietyAuditorAssigmentsId;
        $http.post(SiteUrl + "/Auditor/GetSocietyAuditDataByFinYearId", { "SocietyIdENC": SID, "FinYearId": FinancialYear }).
            success(function (data, status) {
                if (data.Table.length > 0) {
                    $scope.SocietyAuditReport = data.Table[0];
                    $scope.SocietyAuditReport.SocietyAuditorAssigmentsId = data.Table[0].Id;
                    // SocietyAuditReportId,b.IsApprovedByRegistrar,b.AuditReport,b.UserType,b.IsSocietyWorking,b.ApprovedBy
                    if (data.Table[0].SocietyAuditReportId == 'null' || data.Table[0].SocietyAuditReportId == null) {
                        $scope.SocietyAuditReport.IsApproved = 'false';
                        $scope.SocietyAuditReport.AuditReport = '';
                        $scope.SocietyAuditReport.UserType = '';
                        $scope.SocietyAuditReport.IsSocietyWorking = 'false';
                        $scope.SocietyAuditReport.ApprovedBy = 'false';
                    }
                    else{
                    if (data.Table[0].IsApprovedByRegistrar) {
                        $scope.SocietyAuditReport.IsApproved = data.Table[0].IsApprovedByRegistrar.toString()
                        $scope.newdiv = true;
                    }
                    else {
                        $scope.SocietyAuditReport.IsApproved = data.Table[0].IsApprovedByRegistrar.toString()
                        $scope.newdiv = false;
                        }  
                    $scope.SocietyAuditReport.IsSocietyWorking = data.Table[0].IsSocietyWorking.toString()
                    }
                    if (IsFarm == 2)
                    {
                        $scope.newdiv = false;
                    }
                    

                    $scope.FileUploadTypes = [];
                    $scope.FileUploadTypes = data.Table1;
                    $scope.recordsOther = [];
                    $scope.recordsOther = data.Table2;
                    if (data.Table2.length > 0) {
                        $scope.OtherFileUpload.AddMoreFile = data.Table2.length.toString();
                        $scope.GetAddMoreFilesDivOldData();
                    }
                    else {
                        $scope.OtherFileUpload.AddMoreFile = '1';
                        $scope.GetAddMoreFilesDiv();
                    }
                    $scope.SocietyAuditReport.FinancialYear = FinancialYear;
                    //alert(JSON.stringify($scope.records));

                }
                else {
                    $scope.SocietyAuditReport.FinancialYear = FinancialYear;
                    $scope.SocietyAuditReport = [];
                }
                // $scope.DeshboardLeft = data.Table1;
            }).
            error(function (data, status) {
                $scope.SocietyAuditReport = [];
                alert('Error while fetching society list data.');
            });
       
    }



    // START : Get Sub Class
    $scope.GetSubClass = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYSUBCLASS&MasterCode=" + $scope.SocietyRegistration.ClassId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });

    };
    // END : Get Sub Class

    // START : Save Society Profiling
    $scope.SaveSocietyProfiling = function () {

        var ddMMyyyyDate = new Date($scope.SocietyRegistration.GeneralMeetingDate);
        var dd = ddMMyyyyDate.getDate();
        var mm = ddMMyyyyDate.getMonth() + 1; //January is 0!

        var yyyy = ddMMyyyyDate.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }

        var RDddMMyyyyDate = new Date($scope.SocietyRegistration.RegistrationDate);
        var RDdd = RDddMMyyyyDate.getDate();
        var RDmm = RDddMMyyyyDate.getMonth() + 1; //January is 0!

        var RDyyyy = RDddMMyyyyDate.getFullYear();
        if (RDdd < 10) {
            RDdd = '0' + RDdd;
        }
        if (RDmm < 10) {
            RDmm = '0' + RDmm;
        }


        var formData = new FormData();
        formData.append("BRN", $scope.SocietyRegistration.BRN);
        formData.append("ActId", $scope.SocietyRegistration.ActId);
        formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
        formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
        formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);
        formData.append("ClassId", $scope.SocietyRegistration.ClassId);
        formData.append("SubClassId", $scope.SocietyRegistration.SubClassId);
        formData.append("ByeLawsSocietyFile", $scope.ByeLawsSocietyFile);
        formData.append("RegistrationDate", $('#txtRegistrationDate').val());
        formData.append("GeneralMeetingProceedingFile", $scope.GeneralMeetingProceeding);
        formData.append("BalanceSheetFile", $scope.BalanceSheetFile);
        formData.append("RegistrationCertificateFile", $scope.RegistrationCertificateFile);
        formData.append("BylawsFile", $scope.BylawsFile);
        formData.append("RulesAndRegulation", $scope.RulesAndRegulation);


        $http.post(SiteUrl + "/Society/SaveSocietyProfiling", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data > 0) {
                    ShowModelAlert("Society Profiling", "Society Profiling Insert Sucessfully");
                }
                //LoaderStart();
                //$('#modelAlertMessage').html('');
                //var errorLst = [];
                //for (var i = 0; i < Object.keys(data).length; i++) {
                //    errorLst.push(data.data[i]);
                //}
                //var errorMsg = errorLst.join("</br>");
                //ShowModelAlert("User Registration", errorMsg);
                //if (data.Status) {
                $scope.ClearForm();
                //    $scope.UserList();
                //}
                //LoaderStop();
            }).
            error(function (data, status) {

                ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
            });

    };
    // END :Save Society Profiling

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.SocietyRegistration = null;
        $scope.SocietyRegistration.RegistrationCertificateFile = null;
    };
    //END : Delete User
    // START : Clear Society Profile Section
    $scope.ClearSocietyDetailsForm = function () {

        $scope.SocietyRegistration.ActId = '0';
        $scope.SocietyRegistration.BRN = '';
        $scope.SocietyRegistration.RegistrationNo = '';
        $scope.SocietyRegistration.SocietyName = '';
        $scope.SocietyRegistration.RegistrationDate = '';
        $scope.SocietyRegistration.SocietyTypeId = '0';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietySubClassList = [];
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.RegistrationCertificateFile = null;
    };
    // END : Clear Society Profile Section

    // START : Save Society Audit Report Details
    $scope.SaveSocietyAuditDetails = function (IsPermanentSave) {
        var Msg = '';
        var IsFileMissing = false;
        $('#TblEnclouser tbody tr').each(function () {
            var id = $($(this).find('td')[0]).find('label').html();
            if ((id == "1" || id == "2") && IsPermanentSave == 0) {
                // alert(id);
                var vidFileLength = $("#File_PDF_" + id)[0].files.length;
                if (vidFileLength === 0) {
                    var valHfPDFfile = $("#hfpdf" + id).val();
                    if (valHfPDFfile.length == 0) {
                        $("#File_PDF_" + id).css('border', 'solid 1px red');
                        IsFileMissing = true;
                    }
                }
                else {
                    $("#File_PDF_" + id).css('border', 'solid 1px white');

                }
                //var vidFileLength2 = $("#File_Excel_" + id)[0].files.length;
                //if (vidFileLength2 === 0) {
                //    var valHffile = $("#hfxls" + id).val();
                //    if (valHffile.length == 0) {
                //        $("#File_Excel_" + id).css('border', 'solid 1px red');
                //        IsFileMissing = true;
                //    }
                //}
                //else {
                //    $("#File_Excel_" + id).css('border', 'solid 1px white');
                //}
            }
            if ((id == "1" || id == "2" || id == "3" || id == "4" || id == "5" || id == "6" || id == "7" || id == "8" || id == "9" || id == "10" || id == "11" || id == "12") && IsPermanentSave == 1) {
                // alert(id);
                var vidFileLength = $("#File_PDF_" + id)[0].files.length;
                if (vidFileLength === 0) {
                    var valHfPDFfile = $("#hfpdf" + id).val();
                    if (valHfPDFfile.length == 0) {
                        $("#File_PDF_" + id).css('border', 'solid 1px red');
                        IsFileMissing = true;
                    }
                }
                else {
                    $("#File_PDF_" + id).css('border', 'solid 1px white');

                }
                //var vidFileLength2 = $("#File_Excel_" + id)[0].files.length;
                //if (vidFileLength2 === 0) {
                //    var valHffile = $("#hfxls" + id).val();
                //    if (valHffile.length == 0) {
                //        $("#File_Excel_" + id).css('border', 'solid 1px red');
                //        IsFileMissing = true;
                //    }
                //}
                //else {
                //    $("#File_Excel_" + id).css('border', 'solid 1px white');
                //}
            }
        });

            $('#AddMoreFilestbl tbody tr').each(function () {
                var id = $($(this).find('td')[0]).find('label').html();
                var txtFileNameval = $($(this).find('td')[1]).find('input').val();

                //if (txtFileNameval == '' || txtFileNameval == undefined) {
                //    $($($(this).find('td')[1]).find('input')).css('border', 'solid 1px red');
                //}
                //else {
                //    $($($(this).find('td')[1]).find('input')).css('border', 'solid 1px white');
                //}
                if (txtFileNameval != '' && txtFileNameval != undefined) {
                    var id_Pdf = $($($(this).find('td')[2]).find('input'))[0].files.length;
                    if (id_Pdf == 0) {
                        var valHfPDFofile = $("#hfpdfOther" + id).val();
                        //alert(valHfPDFofile);
                        if (valHfPDFofile.length == 0) {
                            $($($(this).find('td')[2]).find('input')).css('border', 'solid 1px red');
                            IsFileMissing = true;
                        }
                    }
                    else {
                        $($($(this).find('td')[2]).find('input')).css('border', 'solid 1px white');
                    }

                    var id_excel = $($($(this).find('td')[3]).find('input'))[0].files.length;
                    if (id_excel == 0) {
                        var valHfofile = $("#hfxlsOther" + id).val();
                        //alert(valHfofile);
                        if (valHfofile.length == 0) {
                            $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px red');
                            IsFileMissing = true;
                        }
                    }
                    else {
                        $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px white');
                    }
                }
            });

        if (IsFileMissing == true) {
            Msg += $scope._language == 'en-us' ? '\nPlease upload file where contain red border' : '\nकृपया फ़ाइल अपलोड करें जहाँ लाल सीमा है |';
            Msg  += '<br />';
            }
       
        if ($scope.SocietyAuditReport.IsApproved == undefined || $scope.SocietyAuditReport.IsApproved == false || $scope.SocietyAuditReport.IsApproved=='false') {
            Msg += (_language == 'en-us' ? 'Is Audit Report Approved ? \n' : 'क्या लेखा परीक्षा रिपोर्ट अनुमोदित है\n');
            Msg += '<br />';
        }

        if ($scope.SocietyAuditReport.IsSocietyWorking == undefined || $scope.SocietyAuditReport.IsSocietyWorking == false || $scope.SocietyAuditReport.IsSocietyWorking == 'false') {
            Msg += (_language == 'en-us' ? 'Is Society Working ? \n' : 'क्या सोसाइटी कार्यशील है ?\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAuditReport.FinancialYear == undefined || $scope.SocietyAuditReport.FinancialYear == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Financial Year \n' : 'वित्तीय वर्ष का चयन करे\n');
            Msg += '<br />';
        }
            if ($("#AuditStartDate").val() == '') {
                Msg += $scope._language == 'en-us' ? '\n Please fill Audit Start Date' : '\n कृपया लेखा परीक्षा प्रारंभ दिनांक भरें |';
                Msg += '<br />';
            }

            if ($("#AuditEndDate").val() == '') {
                Msg += $scope._language == 'en-us' ? '\n Please fill Audit End Date' : '\n कृपया लेखा परीक्षा समाप्ति दिनांक भरें |';
                Msg += '<br />';
            }
        if ($scope.SocietyAuditReport.IsApproved == 'true' || $scope.SocietyAuditReport.IsApproved == true) {
            if ($scope.SocietyAuditReport.ApprovedBy == undefined || $scope.SocietyAuditReport.ApprovedBy == false || $scope.SocietyAuditReport.ApprovedBy == 'false') {
                Msg += (_language == 'en-us' ? 'Please select Approved By \n' : 'कृपया किसके द्वारा अनुमोदित है की स्थिति का चयन करे \n');
                Msg += '<br />';
            }
        }
        //if ($scope.AuditReport == undefined) {
        //    Msg += $scope._language == 'en-us' ? '\nPlease Upload Audit Report(PDF)' : '\nकृपया लेखा परीक्षा रिपोर्ट अपलोड करें(पीडीएफ) |';
        //    Msg += '<br />';
        //}

        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;


            ShowModelAlert("Society Audit Details", Msg);
            return;
        }

        var formData = new FormData();
        var _Counter = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));
        for (var Y = 0; Y < $scope.FileCollection.length; Y++) {
            var _TEMP1 = $scope.FileCollection[Y];
            formData.append(_TEMP1.FileType + 'β' + _TEMP1.Id + 'β' + _TEMP1.Title, _TEMP1.File);
        }
        //alert($scope.SocietyAuditReport.IsApproved);
        formData.append("SocietyIdEnc", $scope.SocietyIdForSocietyAuditDetailsCitizen);
        formData.append("IsApproved", $scope.SocietyAuditReport.IsApproved);
        formData.append("SocietyAuditReportId", $scope.SocietyAuditReport.SocietyAuditReportId);
        formData.append("SocietyAuditorAssigmentsId", $scope.SocietyAuditorAssigmentsId);
        formData.append("IsSocietyWorking", $scope.SocietyAuditReport.IsSocietyWorking);
        formData.append("FinancialYear", $scope.SocietyAuditReport.FinancialYear);
        formData.append("AuditReport", $scope.AuditReport);
        formData.append("AuditStartDate", $("#AuditStartDate").val().split("/").reverse().join("-"));
        formData.append("AuditEndDate", $("#AuditEndDate").val().split("/").reverse().join("-"));
        formData.append("ApprovedBy", $scope.SocietyAuditReport.ApprovedBy);
        if (IsPermanentSave == 1)
            formData.append("IsPermanentSave", true);
        else
            formData.append("IsPermanentSave", false);

        $http.post(SiteUrl + "/Society/SaveSocietyAuditReportDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var Result = parseInt(data, 10);

                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    $scope.GetAuditorAssignReportDataShow();
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }

                alert(AltMsg);
                window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert("Society Audit Report", "Error While Society Audit Report Insert");
            });
    };
    // END :  Save Society Audit Report Details

    // START : Clear Society Audit Report Details
    $scope.ClearSocietyAuditReportForm = function () {

        $scope.SocietyAuditReport.IsApproved = false;
        $scope.SocietyAuditReport.IsSocietyWorking = false;
        // $scope.SocietyAuditReport.FinancialYear = '0';
        $('#DdlFyear').prop('disabled', true);
        $scope.SocietyAuditReport.ApprovedBy = 0;
        $("#flAuditReport").val('');
    };
    // END : Clear Society Audit Report Details



    // START Back To Page
    $scope.BackToPage = function () {
        $scope.divTabulation = true;
        $scope.divAddBtn = false;
        $scope.divSocietyAuditReport = false;
        $scope.divSocietyAuditReportShow = false;
    }
    // START Back To Page

    //start show func
    $scope.SHownew = function (item) {
        if (item == 1) {
            $scope.newdiv = true;
        }
        else {
            $scope.newdiv = false;
        }
    }
    //end

    // START : Save Society Administrative Details
    $scope.SaveSocietyAdministrativeDetails = function () {
        var SocietyRegistrationData = [];
        SocietyRegistrationData.push({ "Name": $scope.SocietyRegistration.CEOName, "ContactNo": $scope.SocietyRegistration.CEOContactNo, "UserType": "1" });
        SocietyRegistrationData.push({ "Name": $scope.SocietyRegistration.ChairmanName, "ContactNo": $scope.SocietyRegistration.ChairmanContactNo, "UserType": "2" });

        $('.table tbody > tr').each(function () {

            SocietyRegistrationData.push({ "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
        });

        $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetails", { "SocietyRegistrationData": SocietyRegistrationData, "ClassA": $scope.SocietyRegistration.TotalMembersClassA, "ClassB": $scope.SocietyRegistration.TotalMembersClassB, "ClassC": $scope.SocietyRegistration.TotalMembersClassC }).
            success(function (data, status) {
                ShowModelAlert("Society Administrative Details", data.data[0]);
                $scope.ClearSocietyAdministrativeDetails();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    // END : Save Society Administrative Details

    $scope.ClearSocietyAdministrativeDetails = function () {
        $('input').val('');
    }

    // START : Add New Registration
    $scope.AddNewSocietyAudit = function () {
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocietyAuditReport = true;

    }
    // END : Add New Registration

    // START : Add New Registration
    $scope.AddDetails = function (SID) {
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = true;
        $scope.divSocietyProfilingShow = false;
        $scope.btnBasicDetails = true;
        $scope.btnAdministrativeDetails = false;
    }
    // END : Add New Registration


    // START : Get Sub Class

    $scope.GetAuditorAssignReportDataShow = function () {

        $http.get(SiteUrl + "/Society/GetAuditorAssignReportDataShow/?SocietyIDEnc=" + $scope.SocietyIdForSocietyAuditDetailsCitizen).

            success(function (data, status) {
                $scope.GetAuditorAssignReportDataShow1 = data;
            }).
            error(function (data, status) {

            });

    };

    $scope.ValidationForAuditInfo = function () {
        var Msg = "";
        if ($("#StartDates").val() == '') {
            Msg += $scope._language == 'en-us' ? '\n Please fill  Start Date' : '\n कृपया आरंभ करने की तिथि भरें |';
            Msg += '<br />';
        }

        if ($("#StartTime_HHs").val() == '') {
            Msg += $scope._language == 'en-us' ? '\n Please fill  Start Time Hour' : '\n कृपया आरंभ करने की समय भरें |';
            Msg += '<br />';
        }
        if ($("#StartTime_MMs").val() == '') {
            Msg += $scope._language == 'en-us' ? '\n Please fill  Start Time Munits' : '\n कृपया आरंभ करने क समय भरें |';
            Msg += '<br />';
        }




        if ($("#EndDates").val() == '') {
            Msg += $scope._language == 'en-us' ? '\n Please fill End Date' : '\n कृपया एन्ड करने की तिथि भरें |';
            Msg += '<br />';
        }

        if ($("#EndTime_HHs").val() == '') {
            Msg += $scope._language == 'en-us' ? '\n Please fill  End Time Hour' : '\n कृपया एन्ड करने की समय भरें |';
            Msg += '<br />';
        }
        if ($("#EndTime_MMs").val() == '') {
            Msg += $scope._language == 'en-us' ? '\n Please fill  End Time Munits' : '\n कृपया एन्ड करने की समय भरें |';
            Msg += '<br />';
        }

        if ($("#StartDates").val() != '' && $("#EndDates").val() != '' && $("#StartTime_HHs").val() != '' && $("#StartTime_MMs").val() != '' && $("#EndTime_HHs").val() != '' && $("#EndTime_MMs").val() != '') {
            var dateS = new Date(($('#StartDates').val().split('/')[2] + "/" + $('#StartDates').val().split('/')[1] + "/" + $('#StartDates').val().split('/')[0]) + " " + $("#StartTime_HHs").val() + ":" + $("#StartTime_MMs").val() + " " + $("#StartTime_APs").val());
            var dateE = new Date(($('#EndDates').val().split('/')[2] + "/" + $('#EndDates').val().split('/')[1] + "/" + $('#EndDates').val().split('/')[0]) + " " + $("#EndTime_HHs").val() + ":" + $("#EndTime_MMs").val() + " " + $("#EndTime_APs").val());
            if (dateS > dateE) {
                Msg += $scope._language == 'en-us' ? '\n  end date time must be greater than start date time' : '\n अंतिम तिथि समय प्रारंभ तिथि समय से अधिक होना चाहिए |';
                Msg += '<br />';
            }
        }


        if ($("#ttime").val() == '' || $("#ttime").val() == '0') {
            Msg += (_language == 'en-us' ? 'Please fill hours \n' : 'कृपया वित्तीय घंटे भरें \n');
            Msg += '<br />';
        }
        if ($("#ddetails").val() == '' ) {
            Msg += (_language == 'en-us' ? 'Please fill Details \n' : 'कृपया विवरण भरें\n');
            Msg += '<br />';
        }

    
        if (Msg.length > 4) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Audit Schedule Details", Msg);
            return false;
        } else {
            return true;
        }
    }

    $scope.SaveDataLetter = function () {        
        if ($scope.ValidationForAuditInfo()) {           
            var formData = new FormData();
            formData.append("SocietyIdEnc", $scope.AuditSocietyIdEncr);
            formData.append("StartDate", $('#StartDates').val() + " " + $("#StartTime_HHs").val() + ":" + $("#StartTime_MMs").val() + " " + $("#StartTime_APs").val());
            formData.append("EndDate", $('#EndDates').val() + " " + $("#EndTime_HHs").val() + ":" + $("#EndTime_MMs").val() + " " + $("#EndTime_APs").val());
            formData.append("Hours", $scope.LetterData.Time);
            formData.append("AuditWorkDetails", $scope.LetterData.Details);
            $(".blockUI").fadeIn("slow");
            $http.post(SiteUrl + "/Citizen/SaveLetterDataDetails", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data > 0) {
                        ShowModelAlert("Audit Schedule Information", " Information Insert Sucessfully");
                        $("#addModal").modal('hide');
                    }

                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowModelAlert("Audit Schedule information", "Error While Society information Insert");
                    //$("#addModal").modal('hide');
                });
        }
    }


    // END : Get Sub Class
    //GET Load Table data
    $scope.DashboardData = function (ActId, SocietyTypeId, ClassId) {
                $scope.TableData = [];
        $http.post(SiteUrl + "/Citizen/UserSocietyDataForAuditRPT").
            success(function (data, status) {
                data = data.filter(function (x) { return x.IsAdministrativeDetails == '3' })
                $scope.TableData = data;
            }).
            error(function (data, status) {
                $scope.TableData = [];
            });
    }
    //END

    // START : Call Functions on Page Load
    $scope.OnPageLoad();

    //  $scope.GetAuditorAssignReportDataShow();
    //END : Call Functions on Page Load


    $scope.SetStatus = function (SocietyAuditReportId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Society/DeleteAuditReport", { Id: SocietyAuditReportId, Action: ActionId }).
            success(function (data, status) {
                $scope.
                    $scope.GetAuditorAssignReportDataShow();

            }).
            error(function (data, status) {
            });
    };

    $scope.TimeCheck =function(){
        try {
            var dateS = new Date(($('#StartDates').val().split('/')[2] + "/" + $('#StartDates').val().split('/')[1] + "/" + $('#StartDates').val().split('/')[0]) + " " + $("#StartTime_HHs").val() + ":" + $("#StartTime_MMs").val() + " " + $("#StartTime_APs").val());
            var dateE = new Date(($('#EndDates').val().split('/')[2] + "/" + $('#EndDates').val().split('/')[1] + "/" + $('#EndDates').val().split('/')[0]) + " " + $("#EndTime_HHs").val() + ":" + $("#EndTime_MMs").val() + " " + $("#EndTime_APs").val());

            var HH = Math.floor((dateE.getTime() - dateS.getTime()) / 1000 / 60 / 60);
            var MM = Math.floor(((dateE.getTime() - dateS.getTime()) / 1000 / 60) % 60);
            if (HH > 0) {
                $scope.LetterData.Time = (HH.toString().length == 1 ? "0" + HH.toString() : HH.toString()) + "." + (MM.toString().length == 1 ? "0" + MM.toString() : MM.toString());
                $("#ttime").val($scope.LetterData.Time);
            }
        } catch (e) {
            $scope.LetterData.Time = ""
            $("#ttime").val();
            var gh = "";
        }
    }
});

