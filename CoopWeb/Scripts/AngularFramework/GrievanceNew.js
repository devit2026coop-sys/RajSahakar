var GrievanceMod = angular.module('GrievanceModule', ['angularUtils.directives.dirPagination']);
GrievanceMod.controller("GrievanceCont", function ($scope, $http, $rootScope) {
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $("#DivRecord").hide();
    $("#DivNoRecord").show();
    $("#DivReportRecord").hide();
    $("#DivReportNoRecord").show();
    $("#DivTrailRecord").hide();
    $("#DivTrailNoRecord").show();
    $("#DivListRecord").hide();
    $("#DivListNoRecord").show();
    $("#ddlDeveloper").hide();
    $("#ddlPriority").hide();
    $('#ddlDeveloperBA').hide();
    $('#ddlPriorityBA').hide();
    $("#Desposehdn").hide();
    $("#ddlIssueRelated").hide();
    $("#ddlGrivCategory").hide();
    $("#ddlGrivsubCategory").hide();
    $scope.SSOGrievanceList = [];
    $scope.IsForm = false;
    $scope.Dashboard = false;
    $scope.GrievanceId = 0;
    $scope.IsNew = false;
    $scope.IsAssignedToBA = false;
    $scope.IsAssignedToDeveloper = false;
    $scope.IsDispodedByDeveloper = false;
    $scope.IsDispodedByBA = false;
    $scope.IsDispodedByIA = false;
    $scope.ReopenByBA = false;
    $scope.Grievance = [];
    $scope.Grievance.Developer = '0';
    $scope.Grievance.Priority = '0';
    $scope.Grievance.ComplainNo = '';
    $scope.Grievance.GrivanceCategoryid = '0';
    $scope.Grievance.GrivanceSubCategoryid = '0';
    $scope.IsDisbaled = false;
    // $scope.Grievance.SubModuleId = '0';
    $scope.RoleName = '';
    $scope.Grievance.Type = '';
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        $(".se-pre-con").fadeIn("slow");
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
        }
        $(".se-pre-con").fadeOut("slow");
        $scope.RoleName = $('#HdnRoleName').val();
        //$scope.GetGrievanceList();
        $scope.GetDepartments();
        //$scope.GetGrievanceListBySSO();
        $scope.GetGrievanceCount();
        $scope.DistrictState();
        $scope.GetAllStatus();
        $scope.GetGrivanceCategoryList();
        $scope.GetGrevianceList();

        var UserSSOID = $('#hdnSSOIDId').val();
        console.log('UserSSOID', UserSSOID);


        //$scope.OpenGrievanceForm();
        if ($scope.RoleName == "Developer") {     //        Developer
            $("#DivListNoRecord").hide();
            $scope.GetModule();
            $scope.GetIssueRelatedList();
            $scope.GetGrivanceCategoryList();
        }

    };
    $scope.OpenGrievanceForm = function () {
        $scope.IsForm = $scope.IsForm == false ? true : false;
        $scope.GetGrievanceCategoryIdList();
        $scope.GetGrievanceModuleIdList();

        var UserSSOID = $('#hdnSSOIDId').val();
        console.log('UserSSOID', UserSSOID);

        $scope.GetMobileNoBySSOID(UserSSOID);
    };
    $scope.OpenDashboard = function () {
        $scope.Dashboard = $scope.Dashboard == false ? true : false;
    };
    //Start: Get Category List
    $scope.GetCategories = function () {
        $scope.Categories = [];
        $http.post(SiteUrl + "Grievance/GetCategoryList/").
            success(function (data, status) {
                if ($scope.Grievance.IssueFrom == '1') {
                    $scope.Categories = data.filter(function (x) { return x.AgriLovCode == 'grievancecategorymobile' });
                }
                else if ($scope.Grievance.IssueFrom == '2') {
                    $scope.Categories = data.filter(function (x) { return x.AgriLovCode == 'grievancecategoryweb' });
                }
            }).
            error(function (data, status) {
                $scope.Categories = [];
            });
    };
    //Start: Get Department List
    $scope.GetDepartments = function () {
        $scope.Departments = [];
        $http.post(SiteUrl + "Grievance/GetDepartmentList/").
            success(function (data, status) {
                $scope.Departments = data;
                $scope.Modules = [];
                //  $scope.SubModules = [];
                $scope.Grievance.ModuleId = '0';
                //  $scope.Grievance.SubModuleId = '0';
            }).
            error(function (data, status) {
                $scope.Departments = [];
            });
    };

    $scope.GrevianceList = [];
    $scope.GetGrevianceList = function () {
        debugger;
        $http.get(SiteUrl + "/Grievance/GetGrievanceList")
            .success(function (data) {
                $scope.GrevianceList = data;
                console.log('$scope.GrevianceList', $scope.GrevianceList);
            });
    };


    //Start: Get Module List
    $scope.GetModule = function () {
        $scope.Modules = [];
        var formData = new FormData();
        if ($scope.RoleName == "Developer") {    //        Developer
            formData.append("Department", 'GetModuleForDeveloper');
        }
        else {
            formData.append("Department", $scope.Grievance.Department);
        }

        $http.post(SiteUrl + "Grievance/GetModuleList", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $scope.Modules = data;
                //$scope.SubModules = [];
                $scope.Grievance.ModuleId = '0';
                // $scope.Grievance.SubModuleId = '0';
            }).
            error(function (data, status) {
                $scope.Modules = [];
            });
    };

    //jaya    

    $scope.GrievanceCategoryLists = [];
    $scope.GetGrievanceCategoryIdList = function () {
        $http.get(SiteUrl + "/Godown/GetNewLovValuesByListId/?ListCode=GrievanceCategory")
            .success(function (data, status) {
                $scope.GrievanceCategoryLists = data;
                console.log('$scope.GrievanceCategoryLists', $scope.GrievanceCategoryLists);
            })
            .error(function () {
                $scope.GrievanceCategoryLists = [];
            });
    };


    $scope.GrievanceModuleLists = [];
    $scope.GetGrievanceModuleIdList = function () {
        debugger;
        $http.get(SiteUrl + "/Godown/GetNewLovValuesByListId/?ListCode=GrievanceModule")
            .success(function (data, status) {

                $scope.AllModules = angular.copy(data);   
                console.log('$scope.AllModules', $scope.AllModules);
                $scope.GrievanceModuleLists = data;
                console.log('$scope.GrievanceModuleLists', $scope.GrievanceModuleLists);
            })
            .error(function () {
                $scope.GrievanceModuleLists = [];
            });
    };


    $scope.IsMobileNoDisabled = false;

    $scope.GetMobileNoBySSOID = function (SSOID) {
        debugger;
        $http.get(SiteUrl + "/Grievance/GetMobileNoBySSOID/?SSOID=" + SSOID)
            .success(function (data, status) {
                if (data != null && data.length > 0) {                              
                    $scope.Grievance.UserMobileNo = data[0].MobileNo;                
                    $scope.IsMobileNoDisabled = true;
                }               
            })
            .error(function () {
                $scope.MobileNo = "";
            });
    };


    $scope.SelectGrievanceFile = function (files) {

        if (files && files.length > 0) {

            if (!$scope.Grievance) {
                $scope.Grievance = {};
            }

            if (!$scope.Grievance.Attachment) {
                $scope.Grievance.Attachment = {};
            }

            $scope.Grievance.Attachment = files[0].name;

            $scope.UploadDocument(files);
        }
    };

    $scope.UploadDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("Attachment", file);

        $http.post(SiteUrl + "/Grievance/UploadGrievanceDocument", fd, {transformRequest: angular.identity,headers: { 'Content-Type': undefined }})
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.Grievance.Attachment = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };

    $scope.GrievanceSectionList = [];
    $scope.GetGrievanceSectionByModuleId = function (ModuleId) {

        $http.get(SiteUrl + "Grievance/GetGrievanceSectionByModuleId", { params: { ModuleId: ModuleId }  })
            .success(function (data) {
                $scope.GrievanceSectionList = data;
                console.log('$scope.GrievanceSectionList', $scope.GrievanceSectionList);
            })
            .error(function () {
                $scope.GrievanceSectionList = [];
            });
    };

    //End

    $scope.GetIssueRelatedList = function () {
        $scope.IssueRelatedList = [];
        $http.post(SiteUrl + "Grievance/GetIssueRelatedList").
            success(function (data, status) {
                $scope.IssueRelatedList = data;
                //$scope.Grievance.GrievanceIssueRelatedId = '0';
            }).
            error(function (data, status) {
                $scope.IssueRelatedList = [];
            });
    };


    $scope.IssueRelatedChanged = function () {
        debugger;
        if ($scope.Grievance.IssueFrom == "1") {

            $scope.GrievanceModuleLists = $scope.AllModules.filter(function (item) {
                return item.Lovdescription == "Society Registration" || item.Lovdescription == "Society Registration Amendment (1958)";
            });
        }
        else {
            $scope.GrievanceModuleLists = angular.copy($scope.AllModules);

            console.log('$scope.GrievanceModuleLists', $scope.GrievanceModuleLists);

        }
       
       // $scope.Grievance.ModuleId = "0";
        //$scope.Grievance.SectionId = "0";
    };


    $scope.SubmitGrievance = function () {
        debugger;

        if ($scope.Grievance.IssueFrom == "0" ) {
            alert('Please Select the Issue Related.');
            return;
        }

        if ($scope.Grievance.ModuleId == "0") {
            alert('Please Select the Module.');
            return;
        }

        if ($scope.Grievance.DepartmentId == "0") {
            alert('Please Select the Department.');
            return;
        }

        if ($scope.Grievance.CategoryId =="0") {
            alert('Please Select the Category.');
            return;
        }

        if (!$scope.Grievance.Subject) {
            alert('Please Fill the Subject.');
            return;
        }

        if (!$scope.Grievance.Description) {
            alert('Please Fill the Discription');
            return;
        }

        if ($scope.Grievance.GrievanceSectionId == "0") {
            alert('Please Select the Section');
            return;
        }    

        var formData = new FormData();
        formData.append("IssueFrom", parseInt($scope.Grievance.IssueFrom));
        formData.append("ModuleId", parseInt($scope.Grievance.ModuleId));
        formData.append("DepartmentId", parseInt($scope.Grievance.DepartmentId));
        formData.append("CategoryId", parseInt($scope.Grievance.CategoryId));
        formData.append("SectionId", parseInt($scope.Grievance.GrievanceSectionId));
        formData.append("Subject", $scope.Grievance.Subject);
        formData.append("Description", $scope.Grievance.Description);
        formData.append("UserMobileNo", $scope.Grievance.UserMobileNo);
        formData.append("MobileNo", $scope.Grievance.MobileNo);
        formData.append("ApplicationId", $scope.Grievance.ApplicationId);  
        formData.append("Attachment", $scope.Grievance.Attachment); 
        $http.post(SiteUrl + "Grievance/SaveGrievanceDepartmentForm", formData, { withCredentials: true, headers: { 'Content-Type': undefined }, transformRequest: angular.identity  }).
            success(function (data, status) {
                if (data > 0) {                   
                    var message = '';
                    message = _language == "en-us" ? "Your Complain Sucessfully Submited. Complain No- " + data + "\n" : "आपकी शिकायत सफलतापूर्वक सबमिट की गई। शिकायत संख्या- " + data + "\n";
                    ShowModelAlert($scope._language == 'en-us' ? 'Grievance' : 'शिकायत', message);
                    $scope.ClearForm();
                }
                else {
                    PleaseWaitStop();
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    ShowModelAlert($scope._language == 'en-us' ? 'Grievance' : 'शिकायत', message);
                }
            }).
            error(function (data, status) {
                PleaseWaitStop();
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance' : 'शिकायत', message);
            });
    };
    
    $scope.ClearForm = function () {
        window.location.href = SiteUrl + "/Grievance/SubmitGrievance";
    }

    $scope.NewCount = 0;
    $scope.AssignedCount = 0;
    $scope.DisposedByBACount = 0;
    $scope.ClosedCount = 0;
    $scope.TotalCount = 0;
    $scope.GetGrievanceCount = function () {
        if ($("#txtFromDate").val() == '' || $("#txtFromDate").val() == null || $("#txtFromDate").val() == undefined
            || $("#txtToDate").val() == '' || $("#txtToDate").val() == null || $("#txtToDate").val() == undefined) {
        }
        var formData = new FormData();
        formData.append("FromDate", $("#txtFromDate").val());
        formData.append("ToDate", $("#txtToDate").val());
        $http.post(SiteUrl + "Grievance/GetGrievanceCount", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.NewCount = data[0].New;
                    $scope.AssignedCount = data[0].Assigned;
                    $scope.DisposedByBACount = data[0].DisposedByBA;
                    $scope.ClosedCount = data[0].Closed;
                    $scope.TotalCount = data[0].Total;
                }
            }).
            error(function (data, status) {
                $scope.NewCount = 0;
                $scope.AssignedCount = 0;
                $scope.DisposedByDeveloperCount = 0;
                $scope.ClosedCount = 0;
                $scope.TotalCount = 0;
            });
    };
    $scope.GetGrievanceListBySSO = function () {
        $('.loading').show();
        $scope.SSOGrievanceList = [];
        $http.post(SiteUrl + "Grievance/GetGrievanceListBySSO/").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.SSOGrievanceList = data;
                    console.log('$scope.SSOGrievanceList', $scope.SSOGrievanceList);
                    //$("#DivRecord").show();
                    //$("#DivNoRecord").hide();
                    $('.loading').hide();
                }
                else {
                    $scope.SSOGrievanceList = [];
                    //$("#DivRecord").hide();
                    //$("#DivNoRecord").show();
                    $('.loading').hide();
                }
            }).
            error(function (data, status) {
                $scope.SSOGrievanceList = [];
                $("#DivRecord").hide();
                $("#DivNoRecord").show();
            });
    };
    $scope.GetGrievanceList = function () {
        $('.loading').show();
        $scope.GrievanceList = [];
        if ($scope.Grievance.CategoryId == undefined || $scope.Grievance.CategoryId == null) {
            $scope.Grievance.CategoryId = '0';
        }
        if ($scope.Grievance.Department == undefined || $scope.Grievance.Department == null) {
            $scope.Grievance.Department = '0';
        }
        if ($scope.Grievance.ModuleId == undefined || $scope.Grievance.ModuleId == null) {
            $scope.Grievance.ModuleId = '0';
        }
        //if ($scope.Grievance.SubModuleId == undefined || $scope.Grievance.SubModuleId == null) {
        //    $scope.Grievance.SubModuleId = '0';
        //}
        if ($scope.Grievance.DeveloperId == undefined || $scope.Grievance.DeveloperId == null) {
            $scope.Grievance.DeveloperId = '0';
        }
        if ($scope.Grievance.ComplainNo == undefined || $scope.Grievance.ComplainNo == null) {
            $scope.Grievance.ComplainNo = '';
        }
        var formData = new FormData();
        formData.append("CategoryId", $scope.Grievance.CategoryId);
        formData.append("Department", $scope.Grievance.Department);
        formData.append("ModuleId", $scope.Grievance.ModuleId);
        // formData.append("SubModuleId", $scope.Grievance.SubModuleId);
        formData.append("DeveloperId", $scope.Grievance.DeveloperId);
        formData.append("ComplainNo", $scope.Grievance.ComplainNo);
        formData.append("GrivanceCategoryid", $scope.Grievance.GrivanceCategoryid);
        formData.append("GrivanceSubCategoryid", $scope.Grievance.GrivanceSubCategoryid);

        $http.post(SiteUrl + "Grievance/GetGrievanceList", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $('.loading').show();
                if (data.length > 0) {
                    if ($scope.Grievance.ComplainNo != '') {
                        $scope.GrievanceList = data;
                        $scope.Grievance.Type = '';
                        $('.loading').hide();
                    }
                    else {

                        if ($scope.RoleName == 'Developer') {
                            $scope.GrievanceList = data.filter(function (x) { return x.IsAssignedToDeveloper == true });
                            $('.loading').hide();
                        }
                        else if ($scope.RoleName == 'Grievance Helpdesk') {

                            if ($scope.Grievance.Type == 'New') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsAssignedToBA == true });
                            }
                            else if ($scope.Grievance.Type == 'AssignedToDeveloper') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsAssignedToDeveloper == true });
                            }
                            else if ($scope.Grievance.Type == 'DisposedByDeveloper') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsDispodedByDeveloper == true });
                            }
                            else if ($scope.Grievance.Type == 'DispodedByBA') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsDispodedByBA == true || x.IsDispodedByIA == true });
                            }
                            else if ($scope.Grievance.Type == 'InformationRequired') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsMoreInformationRequired == true });
                            }
                            else {
                                $scope.GrievanceList = data;
                            }
                        }
                        else if ($scope.RoleName == 'Informatics Assistant') {
                            var str = $('.DDLRole').val();
                            $scope.Agri_UserId = str.split('~')[0]
                            if ($scope.Grievance.Type == 'New') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsNew == true && x.Agri_UserId == $scope.Agri_UserId });
                            }
                            else if ($scope.Grievance.Type == 'Assigned') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsAssignedToBA == true || x.IsAssignedToDeveloper == true || x.IsDispodedByDeveloper == true });
                            }
                            else if ($scope.Grievance.Type == 'Disposed') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsDispodedByBA == true });
                            }
                            else if ($scope.Grievance.Type == 'Closed') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsDispodedByIA == true });
                            }
                            else if ($scope.Grievance.Type == 'InformationRequired') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsMoreInformationRequired == true });
                            }
                            else if ($scope.Grievance.Type == 'DisposedByDeveloper') {
                                $scope.GrievanceList = data.filter(function (x) { return x.IsDispodedByDeveloper == true });
                            }
                            else {
                                $scope.GrievanceList = data;
                            }
                        }
                    }
                    if ($scope.GrievanceList.length > 0) {

                        $("#DivListRecord").show();
                        $("#DivListNoRecord").hide();
                        $('#btnExcel').show();
                        $('.loading').hide();
                    }
                    else {
                        $scope.GrievanceList = [];
                        $("#DivListRecord").hide();
                        $("#DivListNoRecord").show();
                        $('#btnExcel').hide();
                        $('.loading').hide();
                    }
                }
                $('.loading').hide();
            }).
            error(function (data, status) {
                $scope.GrievanceList = [];
                $("#DivListRecord").hide();
                $("#DivListNoRecord").show();
                $('.loading').hide();
            });
    };
    
    
  
    $scope.ActionByIA = function () {

        if ($scope.Grievance.Action == '' || $scope.Grievance.Action == null || $scope.Grievance.Action == undefined) {
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please Select Action.');
            return false;
        }
        //if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
        //    ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please fill remark.');
        //    return false;
        //}
        else if ($scope.Grievance.Action == 'AssignToDeveloper') {
            if ($scope.Grievance.Developer == '0' || $scope.Grievance.Developer == null || $scope.Grievance.Developer == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Developer.');
                return false;
            }
            if ($scope.Grievance.IssueRelated == '0' || $scope.Grievance.IssueRelated == null || $scope.Grievance.IssueRelated == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Issue Types.');
                return false;
            }
            if ($scope.Grievance.Priority == '0' || $scope.Grievance.Priority == null || $scope.Grievance.Priority == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Priority.');
                return false;
            }
            if ($scope.Grievance.GrivanceCategoryid == '0' || $scope.Grievance.GrivanceCategoryid == null || $scope.Grievance.GrivanceCategoryid == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Grivance Category.');
                return false;
            }
            if ($scope.Grievance.GrivanceCategoryid != '7' && $scope.Grievance.GrivanceCategoryid != '8' && $scope.Grievance.GrivanceCategoryid != '9' && $scope.Grievance.GrivanceCategoryid != '10' && $scope.Grievance.GrivanceCategoryid != '11' && $scope.Grievance.GrivanceCategoryid != '12') {
                if ($scope.Grievance.GrivanceSubCategoryid == '0' || $scope.Grievance.GrivanceSubCategoryid == null || $scope.Grievance.GrivanceSubCategoryid == undefined) {
                    ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Grivance Sub Category.');
                    return false;
                }
            }
            else {

                $scope.Grievance.GrivanceSubCategoryid == '0'
            }

            if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please fill remark.');
                return false;
            }

        }
        else if ($scope.Grievance.Action == 'Reopen') {
            if ($scope.Grievance.Developer == '0' || $scope.Grievance.Developer == null || $scope.Grievance.Developer == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Developer.');
                return false;
            }
            if ($scope.Grievance.IssueRelated == '0' || $scope.Grievance.IssueRelated == null || $scope.Grievance.IssueRelated == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Issue Types.');
                return false;
            }
            if ($scope.Grievance.Priority == '0' || $scope.Grievance.Priority == null || $scope.Grievance.Priority == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Priority.');
                return false;
            }
            if ($scope.Grievance.GrivanceCategoryid == '0' || $scope.Grievance.GrivanceCategoryid == null || $scope.Grievance.GrivanceCategoryid == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Grivance Category.');
                return false;
            }
            if ($scope.Grievance.GrivanceSubCategoryid == '0' || $scope.Grievance.GrivanceSubCategoryid == null || $scope.Grievance.GrivanceSubCategoryid == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Grivance Sub Category.');
                return false;
            }
            if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please fill remark.');
                return false;
            }
        }
        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("DeveloperId", $scope.Grievance.Developer);
        formData.append("IssueRelatedTO", $scope.Grievance.IssueRelated);
        formData.append("GrivanceCategoryid", $scope.Grievance.GrivanceCategoryid);
        formData.append("GrivanceSubCategoryid", $scope.Grievance.GrivanceSubCategoryid);
        formData.append("Priority", $scope.Grievance.Priority);
        formData.append("Remark", $scope.Grievance.Remark);
        formData.append("ActionType", $scope.Grievance.Action);
        if (document.getElementById("AttachedDocFileInput").files.length > 0) {
            formData.append("Attachment", document.getElementById("AttachedDocFileInput").files[0]);
        }

        if (document.getElementById("AttachedDocFileInput").files.length > 0) {
            formData.append("Attachment", document.getElementById("AttachedDocFileInput").files[0]);
        }
        $http.post(SiteUrl + "Grievance/ActionByIA", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Data Save Sucessfully.');
                $scope.Grievance.Remark = '';
                $scope.Grievance.Action = '';
                $scope.fileRemove('AttachedDoc');
                $scope.GetGrievanceList();
                $scope.GetGrievanceCount();
                $('#GrievanceRemarkModal').modal('hide');
                window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.GetGrievanceList();
                return;
            });
    }
    $scope.ChangeActionValue = function (items) {
        if (items == 'Close') {
            $('#txtRemarks').show();
        }
        //if ($scope.Grievance.Action == 'Close') {
        //    $scope.IsDisbaled = true;
        //}
        else if (items == 'Reopen') {
            $('#txtRemarks').show();
            //$scope.IsDisbaled = false;
        }
    };
    $scope.ChangeActionValueBA = function (items) {
        if (items == 'Close') {
            $('#txtRemarks').show();
            $('#ddlDeveloperBA').hide();
            $('#ddlPriorityBA').hide();
        }
        else if (items == 'AssignToDeveloper') {
            $('#txtRemarks').show();
            $('#ddlDeveloperBA').show();
            $('#ddlPriorityBA').show();
        }
    };
    $scope.AssignToDeveloper = function () {


        if ($scope.Grievance.ActionBA == undefined || $scope.Grievance.ActionBA == '' || $scope.Grievance.ActionBA == null) {
            ShowModelAlert($scope._language == 'en-us' ? 'Close Grievance' : 'Close Grievance', 'Please select one option.');
            return false;
        }
        if ($scope.Grievance.ActionBA == 'Close') {
            if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Close Grievance' : 'Close Grievance', 'Please fill remark.');
                return false;
            }
        }
        else if ($scope.Grievance.ActionBA == 'AssignToDeveloper') {
            if ($scope.Grievance.Developer == '0' || $scope.Grievance.Developer == null || $scope.Grievance.Developer == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Developer.');
                return false;
            }
            if ($scope.Grievance.IssueRelated == '0' || $scope.Grievance.IssueRelated == null || $scope.Grievance.IssueRelated == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Issue Types.');
                return false;
            }
            if ($scope.Grievance.Priority == '0' || $scope.Grievance.Priority == null || $scope.Grievance.Priority == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please Select Priority.');
                return false;
            }
            if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Please fill remark.');
                return false;
            }
        }
        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("DeveloperId", $scope.Grievance.Developer);
        formData.append("IssueRelatedTO", $scope.Grievance.IssueRelated);
        formData.append("Priority", $scope.Grievance.Priority);
        formData.append("Remark", $scope.Grievance.Remark);
        formData.append("ActionType", $scope.Grievance.ActionBA);
        if (document.getElementById("AttachedDocFileInput").files.length > 0) {
            formData.append("Attachment", document.getElementById("AttachedDocFileInput").files[0]);
        }
        $http.post(SiteUrl + "Grievance/AssignToDeveloper", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {

                if ($scope.Grievance.ActionBA == 'Close') {
                    ShowModelAlert($scope._language == 'en-us' ? 'Close Grievance' : 'Close Grievance', 'Data Save Sucessfully.');
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'Assign To Developer' : 'Assign To Developer', 'Data Save Sucessfully.');
                }

                $scope.Grievance.Developer = '0';
                $scope.Grievance.Priority = '0';
                $scope.Grievance.Remark = '';
                $scope.GetGrievanceList();
                $('#GrievanceRemarkModal').modal('hide');
                // window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.GetGrievanceList();
                return;
            });
    }
    $scope.DisposeGrievance = function () {
        if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
            ShowModelAlert($scope._language == 'en-us' ? 'Dispose Grievance' : 'Dispose Grievance', 'Please fill remark.');
            return false;
        }
        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("Remark", $scope.Grievance.Remark);
        if (document.getElementById("AttachedDocFileInput").files.length > 0) {
            formData.append("Attachment", document.getElementById("AttachedDocFileInput").files[0]);
        }
        $http.post(SiteUrl + "Grievance/DisposeGrievance", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Dispose Grievance' : 'Dispose Grievance', 'Data Save Sucessfully.');
                $scope.Grievance.Remark = '';
                $scope.GetGrievanceList();
                $('#GrievanceRemarkModal').modal('hide');
                // window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.GetGrievanceList();
                return;
            });
    };
    $scope.ReopenByUser = function () {
        if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please fill remark.');
            return false;
        }
        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("Remark", $scope.Grievance.Remark);
        if (document.getElementById("ReAttachedDocFileInput").files.length > 0) {
            formData.append("File", document.getElementById("ReAttachedDocFileInput").files[0]);
        }
        $http.post(SiteUrl + "Grievance/ReopenByUser", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Reopen' : 'Grievance Reopen', 'Reopen Sucessfully.');
                $scope.Grievance.Remark = '';
                $scope.GetGrievanceListBySSO();
                $('#GrievanceRemarkModal').modal('hide');
                //window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.GetGrievanceListBySSO();
                return;
            });
    }
    $scope.RemarkByUser = function () {
        if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please fill remark.');
            return false;
        }
        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("Remark", $scope.Grievance.Remark);

        if (document.getElementById("RemarkAttachedDocFileInput").files.length > 0) {
            formData.append("Attachment", document.getElementById("RemarkAttachedDocFileInput").files[0]);
        }
        $http.post(SiteUrl + "Grievance/RemarkByUser", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Remark' : 'Grievance Remark', 'Remark Done Sucessfully.');
                $scope.Grievance.Remark = '';
                $scope.GetGrievanceListBySSO();
                $('#RemarkModal').modal('hide');
                // window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.GetGrievanceListBySSO();
                return;
            });
    }
    $scope.FinalAction = function () {
        if ($scope.Grievance.Action == 'AssignToDeveloper') {
            if ($scope.Grievance.Developer == '0' || $scope.Grievance.Developer == null || $scope.Grievance.Developer == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please Select Developer.');
                return false;
            }
            if ($scope.Grievance.Priority == '0' || $scope.Grievance.Priority == null || $scope.Grievance.Priority == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please Select Priority.');
                return false;
            }
        }
        else if ($scope.Grievance.Action == 'Close' || $scope.Grievance.Action == 'Reopen') {
            if ($scope.Grievance.Remark == '' || $scope.Grievance.Remark == null || $scope.Grievance.Remark == undefined) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Please fill remark.');
                return false;
            }
        }

        var formData = new FormData();
        formData.append("GrievanceId", $scope.GrievanceId);
        formData.append("DeveloperId", $scope.Grievance.Developer);
        formData.append("Priority", $scope.Grievance.Priority);
        formData.append("Remark", $scope.Grievance.Remark);
        formData.append("ActionType", $scope.Grievance.Action);
        if (document.getElementById("AttachedDocFileInput").files.length > 0) {
            formData.append("Attachment", document.getElementById("AttachedDocFileInput").files[0]);
        }
        $http.post(SiteUrl + "Grievance/FinalAction", formData, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).
            success(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Grievance Action' : 'Grievance Action', 'Data Save Sucessfully');
                $scope.Grievance.Developer = '0';
                $scope.Grievance.Remark = '';
                $scope.GetGrievanceList();
                $('#GrievanceRemarkModal').modal('hide');
                // window.location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert($scope._language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.GetGrievanceList();
                return;
            });
    };
    $scope.GetGrievancetReport = function () {

        if ($scope.Grievance.Type == '') {
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance Report' : 'Grievance Report', 'Please Select Grievance Type');
            return false;
        }

        $('.loading').show();
        $scope.GrievanceReportList = [];
        if ($("#txtFromDate").val() == '' || $("#txtFromDate").val() == null || $("#txtFromDate").val() == undefined) {
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance Report' : 'Grievance Report', 'Please Select From Date.');
            return false;
        }
        if ($("#txtToDate").val() == '' || $("#txtToDate").val() == null || $("#txtToDate").val() == undefined) {
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance Report' : 'Grievance Report', 'Please Select To Date.');
            return false;
        }
        
        $scope.Grievance.Status = "";
        //var selected = $("#ddlAS_Circle option:selected");
        $("#input_tags5").find('option:selected').each(function () {
            $scope.Grievance.Status += $(this).val() + ",";
        });
        $http.post(SiteUrl + "Grievance/GetGrievancetReport/?Distid=" + $scope.Grievance.DistId + "&Department=" + $scope.Grievance.Department + "&ModuleId=" + $scope.Grievance.ModuleId + "&FromDate=" + $("#txtFromDate").val() + "&ToDate=" + $("#txtToDate").val() + "&ComplainNo=" + $scope.Grievance.ComplainNo + "&Status=" + $scope.Grievance.Status + "&GrivanceCategoryid=" + $scope.Grievance.GrivanceCategoryid + "&GrivanceSubCategoryid=" + $scope.Grievance.GrivanceSubCategoryid).
            success(function (data, status) {
                if ($scope.State_id == 1) {
                    $scope.GrievanceReportList = data;
                    // $scope.GetGrievanceReportDetails();
                }
                if (data.length > 0) {
                    $scope.GrievanceReportList = [];
                    if ($scope.Grievance.Type == 'Open') {
                        $scope.GrievanceReportList = data.filter(function (x) { return x.StatusId != '6' && x.StatusId != '10' });  // && x.StatusId != '10'

                        // console.log($scope.GrievanceReportList)
                    }
                    else if ($scope.Grievance.Type == 'Disposed') {
                        $scope.GrievanceReportList = data.filter(function (x) { return x.StatusId == '6' || x.StatusId == '10' });  // || x.StatusId == '11' 
                        //console.log($scope.GrievanceReportList)
                    }
                    else {
                        $scope.GrievanceReportList = data;

                    }
                    $scope.GetGrievanceReportDetails();
                    $('.loading').hide();
                    $("#DivReportRecord").show();
                    $("#DivReportNoRecord").hide();
                    $('#btnExcel').show();
                }
                else {
                    $scope.GrievanceReportList = [];
                    $('.loading').hide();
                    $("#DivReportRecord").hide();
                    $("#DivReportNoRecord").show();
                    $('#btnExcel').hide();
                }
                // Default SortColumn.
                //$scope.colsort = "CreateDate";
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.SortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                }
                $scope.GetSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                }
            }).
            error(function (data, status) {
                $scope.GrievanceList = [];
                $("#DivRecord").hide();
                $("#DivNoRecord").show();
                $('#btnExcel').hide();
            });
    };
    $scope.GetGrievanceTrail = function (GrievanceId, Description) {
        $scope.Description = Description
        $scope.GrievanceTrail = [];
        $scope.ComplainNo = '10000' + GrievanceId;
        //$scope.Grievance.Description;
        $http.get(SiteUrl + "Grievance/GetGrievanceTrail/?GrievanceId=" + GrievanceId).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.GrievanceTrail = data;
                    $("#DivTrailRecord").show();
                    $("#DivTrailNoRecord").hide();
                }
                else {
                    $scope.GrievanceTrail = [];
                    $("#DivTrailRecord").hide();
                    $("#DivTrailNoRecord").show();
                }
            }).
            error(function (data, status) {
                $scope.GrievanceTrail = [];
                $("#DivTrailRecord").hide();
                $("#DivTrailNoRecord").show();
            });
    };
    $scope.FileforUpload = function (file, documentname) {
        _validFileExtensions = [".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv", ".txt",".rar"];

        var sFileName = file.value;
        var blnValid = false;
        for (var j = 0; j < _validFileExtensions.length; j++) {
            var sCurExtension = _validFileExtensions[j];
            if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                blnValid = true;
                break;
            }
        }
        if (!blnValid) {
            var message = $scope._language == 'en-us' ? 'Sorry, File is invalid, allowed extensions are: ' + _validFileExtensions.join(", ") + ' \n' : 'क्षमा करें, फ़ाइल अमान्य है, अनुमत एक्सटेंशन हैं:' + _validFileExtensions.join(", ") + ' \n';
            ShowModelAlert($scope._language == 'en-us' ? 'Grievance' : 'शिकायत', message);
            return false;
        }
        //if (CheckValidateFile(file, $(file).val().split('.').pop().toLowerCase()))
        //{
        documentname = documentname.replace("FileInput", "");
        if (file.files[0].size < 11000000) {
            $('#' + documentname + 'input').css('display', 'block');
            $('#' + documentname + 'file').css('display', 'block');
            $('#' + documentname + 'Name').text(file.files[0].name);
            $('#Mob' + documentname).text(file.files[0].name);
            $('#hyp' + documentname).css("visibility", "hidden");
        }
        // }
        return true;
    };
    $scope.fileRemove = function (documentname) {
        $('#' + documentname + 'input').css('display', 'block');
        $('#' + documentname + 'file').css('display', 'none');
        $('#' + documentname + 'FileInput').val('');
        $('#Mob' + documentname).text('');
        $('#viewer' + documentname).css("display", "none");
    };
    $scope.filePreview = function (documentname) {
        documentname = documentname.replace("View", "");
        input = document.getElementById(documentname + 'FileInput');
        file = input.files[0];
        readURL(file, documentname);
    };
    function readURL(input, documentname) {
        var reader = new FileReader();
        pdffile_url = URL.createObjectURL(input);
        $('#viewerdocumentname').css("display", "block");
        $('#viewerdocumentname').attr('src', pdffile_url);
    };
    $scope.fileCheck = function (file) {
        if (CheckValidateFile(file, $(file).val().split('.').pop().toLowerCase())) {
        }
    };
    $scope.DistrictState = function () {
        $scope.DistrictState = [];
        $http.post(SiteUrl + "Grievance/DistrictState/").
            success(function (data, status) {
               /* $scope.DistrictState = data;*/
                $scope.DistrictState = data.filter(function (x) { return x.DISTRICT_ID != 652 && x.DISTRICT_ID != 653 });
            }).error(function (data, staus) {
                $scope.DistrictState = [];
            })
    };
    $scope.GetAllStatus = function () {
        $scope.Status = [];
        $http.post(SiteUrl + "Grievance/GetAllStatus/").
            success(function (data, status) {
                $scope.Status = data;
            }).error(function (data, staus) {
                $scope.Status = [];
            })

    };
    var view = function (cell, formatterParams) {
        return "<i class='fa fa-bars'></i>"
    };
    $scope.GetGrievanceReportDetails = function () {
        var GRD = "fitDataFill";
        $scope.TableData = new Tabulator("#GrievanceReportDetails", {
            height: 500,
            data: $scope.GrievanceReportList,
            layout: "fitColumns",
            pagination: "local",
            paginationSize: 500,
            tooltips: false,
            movableRows: false,
            groupBy: "",
            langs: {
                "hi-hi": {
                    "columns": {
                        "SNNO": "एसएन नं",
                    },
                    "title": {
                        "Work Info": "काम की जानकारी",
                    },
                    "pagination": {
                        "first": "पहला",
                        "first_title": "पहला पृष्ठ",
                        "last": "अंतिम",
                        "last_title": "अंतिम पृष्ठ",
                        "prev": "पिछला",
                        "prev_title": "पिछला पृष्ठ",
                        "next": "अगला",
                        "next_title": "अगला पृष्ठ",
                        "all": "सभी",
                    },
                },
            },
            columns: [
                { title: "SR.NO", sorter: "int", headerHozAlign: "center", formatter: "rownum", headerHozAlign: "center" },
                { title: "Complaint_No", field: "ComplainNo", sorter: "int", hozAlign: "center", headerFilter: "input", headerHozAlign: "center" },
                { title: "District", field: "DISTRICT_ENG", headerHozAlign: "center", headerHozAlign: "center" },
                { title: "Status", field: "Status", hozAlign: "center", headerFilter: "input", headerHozAlign: "center" },
                { title: "Date", field: "CreateDate", hozAlign: "center", headerHozAlign: "center" },
                { title: "Subject", field: "Subject", hozAlign: "center", headerHozAlign: "center" },
                { title: "Description", field: "Description", headerHozAlign: "center", headerHozAlign: "center" },
                { title: "Bug From", field: "IssueFromEn", hozAlign: "center", headerFilter: "input", headerHozAlign: "center" },
                { title: "Category", field: "AgriLovEn", hozAlign: "center", headerHozAlign: "center" },
                { title: "Module", field: "ModuleEn", hozAlign: "center", headerFilter: "input", headerHozAlign: "center" },
                { title: "Priority", field: "Priority", hozAlign: "center", headerHozAlign: "center" },
                {
                    title: "Attachment", field: "Attachment", hozAlign: "center",
                    formatter: function (cell, formatterParams) {
                        var value = cell.getValue();
                        if (value != "") {
                            return "<i class='fa fa-eye'></i>";
                        } else {
                            return value;
                        }
                    },
                    cellClick: function (e, Attachment) {
                        debugger;
                        window.open('https://rajkisan.rajasthan.gov.in/ContentNew/Grievance/' + $(Attachment)[0]._cell.initialValue, '_blank');

                    }, headerHozAlign: "center"
                },
                { title: "Ssoid", field: "SSOId", hozAlign: "center", headerHozAlign: "center" },
                { title: "Assigned IA", field: "AssignedIA", hozAlign: "center", headerHozAlign: "center" },
                { title: "Mobile", field: "Mobile", hozAlign: "center", headerHozAlign: "center" },
                {
                    formatter: view, title: "Trail View", field: "GrievanceId", hozAlign: "center",
                    cellClick: function (e, GrievanceId) {
                        $scope.GetGrievanceTrailList($(GrievanceId)[0]._cell.initialValue);
                        $scope.Description = $(GrievanceId)[0]._cell.row['data'].Description;
                        $("#PopGrievanceTrailview").modal('show');
                    }, headerHozAlign: "center"
                },
                { title: "Issue realted to ", field: "IssueRelatedTO", hozAlign: "center", headerHozAlign: "center" },
                { title: " Category ", field: "GrivenceCategoryName", hozAlign: "center", headerHozAlign: "center", headerFilter: "input" },
                { title: " Sub  Category ", field: "GrivenceSubCategoryName", hozAlign: "center", headerHozAlign: "center", headerFilter: "input" },
                { title: "User MobileNo", field: "UserMobileNo", hozAlign: "center", headerHozAlign: "center", headerFilter: "input" ,width: 100 },
                { title: "ApplicationNo ", field: "ApplicationId", hozAlign: "center", headerHozAlign: "center", headerFilter: "input", width: 100  },
            ]
        });
    };
    $scope.DownloadXLSX = function () {
        $scope.TableData.download("xlsx", "GrievanceReportDetails.xlsx", { sheetName: "Grievance Report Details" });
    };
    $scope.PrintTable = function () {
        $scope.TableData.print(false, true);
    };
    $scope.DownloadPDF = function () {
        $scope.TableData.download("pdf", "GrievanceReportDetails.pdf", {
            orientation: "portrait",
            title: "Grievance Report Details",
            autoTable: {
                styles: {
                    fillColor: [244, 244, 244]
                },
                columnStyles: {
                    id: { fillColor: 244 }
                },
                margin: { top: 60 },
            },
        });
    };
    $scope.GetGrievanceTrailList = function (GrievanceId) {
        $scope.Description = [];
        $scope.GrievanceTrail = [];
        $scope.ComplainNo = '10000' + GrievanceId;
        $http.get(SiteUrl + "Grievance/GetGrievanceTrail/?GrievanceId=" + GrievanceId).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.GrievanceTrail = data;
                    $("#DivTrailRecord").show();
                    $("#DivTrailNoRecord").hide();
                    var GT = "fitDataFill";
                    $scope.TableData = new Tabulator("#GrievanceTrailDetails", {
                        height: 500,
                        data: $scope.GrievanceTrail,
                        layout: "fitColumns",
                        pagination: "local",
                        paginationSize: 500,
                        tooltips: false,
                        movableRows: false,
                        groupBy: "",
                        langs: {
                            "hi-hi": {
                                "columns": {
                                    "SNNO": "एसएन नं",
                                },
                                "title": {
                                    "Work Info": "काम की जानकारी",
                                },
                                "pagination": {
                                    "first": "पहला",
                                    "first_title": "पहला पृष्ठ",
                                    "last": "अंतिम",
                                    "last_title": "अंतिम पृष्ठ",
                                    "prev": "पिछला",
                                    "prev_title": "पिछला पृष्ठ",
                                    "next": "अगला",
                                    "next_title": "अगला पृष्ठ",
                                    "all": "सभी",
                                },
                            },
                        },
                        columns: [
                            { title: "SR.NO", sorter: "int", hozAlign: "center", formatter: "rownum", headerHozAlign: "center" },
                            { title: "Date", field: "CreateDate", sorter: "int", hozAlign: "center", headerHozAlign: "center" },
                            { title: "Remarks", field: "Remark", hozAlign: "center", headerHozAlign: "center" },
                            {
                                title: "Attachment", field: "Attachment", hozAlign: "center",
                                formatter: function (cell, formatterParams) {
                                    var value = cell.getValue();
                                    if (value != "") {
                                        return "<i class='fa fa-eye'></i>";
                                    } else {
                                        return value;
                                    }
                                },
                                cellClick: function (e, Attachment) {
                                    //console.log($(Attachment)[0]._cell.initialValue.toString());
                                    window.open('https://rajkisan.rajasthan.gov.in/ContentNew/Grievance/' + $(Attachment)[0]._cell.initialValue, '_blank');
                                }, headerHozAlign: "center"
                            },
                            { title: "Status", field: "Status", hozAlign: "center", headerHozAlign: "center" },
                            { title: "Aging", field: "Days", hozAlign: "center", headerHozAlign: "center" },
                        ]
                    });
                }
                else {
                    $scope.GrievanceTrail = [];
                    $("#DivTrailRecord").hide();
                    $("#DivTrailNoRecord").show();
                }
            }).
            error(function (data, status) {
                $scope.GrievanceTrail = [];
                $("#DivTrailRecord").hide();
                $("#DivTrailNoRecord").show();
            });
    };
    $scope.DownloadXLSX = function () {
        $scope.TableData.download("xlsx", "GrievanceTrailDetails.xlsx", { sheetName: "Grievance Trail Details" });
    };
    $scope.PrintTable = function () {
        $scope.TableData.print(false, true);
    };
    $scope.GetGrivanceTrail = function (GrievanceId, ComplainNo, Description) {
        $scope.Description = Description;
        $scope.GrievanceTrail = [];
        $("#GrivanceId").text(ComplainNo);
        $http.post(SiteUrl + "Grievance/GetGrievanceTrailUser/?GrievanceId=" + GrievanceId).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.GrievanceTrail = data;
                    $("#DivTrailRecord").show();
                    $("#DivTrailNoRecord").hide();
                    var GT = "fitDataFill";
                    $scope.TableData = new Tabulator("#GrivanceTrailCount", {
                        height: 500,
                        data: $scope.GrievanceTrail,
                        layout: "fitColumns",
                        pagination: "local",
                        paginationSize: 500,
                        tooltips: false,
                        movableRows: false,
                        groupBy: "",
                        langs: {
                            "hi-hi": {
                                "columns": {
                                    "SNNO": "एसएन नं",
                                },
                                "title": {
                                    "Work Info": "काम की जानकारी",
                                },
                                "pagination": {
                                    "first": "पहला",
                                    "first_title": "पहला पृष्ठ",
                                    "last": "अंतिम",
                                    "last_title": "अंतिम पृष्ठ",
                                    "prev": "पिछला",
                                    "prev_title": "पिछला पृष्ठ",
                                    "next": "अगला",
                                    "next_title": "अगला पृष्ठ",
                                    "all": "सभी",
                                },
                            },
                        },
                        columns: [
                            { title: "SR.NO", sorter: "int", hozAlign: "center", formatter: "rownum", headerHozAlign: "center" },
                            { title: "Date", field: "CreateDate", sorter: "int", hozAlign: "center", headerHozAlign: "center" },
                            { title: "Remarks", field: "Remark", hozAlign: "center", headerHozAlign: "center" },
                            {
                                title: "Attachment", field: "Attachment", hozAlign: "center",
                                formatter: function (cell, formatterParams) {
                                    var value = cell.getValue();
                                    if (value != "") {
                                        return "<i class='fa fa-eye'></i>";
                                    } else {
                                        return value;
                                    }
                                },
                                cellClick: function (e, Attachment) {
                                    //console.log($(Attachment)[0]._cell.initialValue.toString());
                                    window.open('https://rajkisan.rajasthan.gov.in/ContentNew/Grievance/' + $(Attachment)[0]._cell.initialValue, '_blank');
                                }, headerHozAlign: "center"
                            },
                            { title: "Status", field: "Status", hozAlign: "center", headerHozAlign: "center" },
                        ]
                    });
                }
                else {
                    $scope.GrievanceTrail = [];
                    $("#DivTrailRecord").hide();
                    $("#DivTrailNoRecord").show();
                }
            }).
            error(function (data, status) {
                $scope.GrievanceTrail = [];
                $("#DivTrailRecord").hide();
                $("#DivTrailNoRecord").show();
            });
    };

    $scope.GrievanceActionIA = function (items) {
        if (items == 'Dispose') {
            $("#ddlDeveloper").hide();
            $("#ddlPriority").hide();
            $("#hdnbtnAll").hide();
            $("#Desposehdn").show();
            $("#ddlIssueRelated").hide();
            $("#ddlGrivCategory").hide();
            $("#ddlGrivsubCategory").hide();
            $scope.Grievance.Remark = '';
        }
        else if (items == 'AssignToBA') {
            $("#ddlDeveloper").hide();
            $("#ddlPriority").hide();
            $("#hdnbtnAll").show();
            $("#Desposehdn").hide();
            $("#ddlIssueRelated").hide();
            $("#ddlGrivCategory").hide();
            $("#ddlGrivsubCategory").hide();
            $scope.Grievance.Remark = '';
        }
        else if (items == 'MoreInformation') {
            $("#ddlDeveloper").hide();
            $("#ddlPriority").hide();
            $("#hdnbtnAll").hide();
            $("#Desposehdn").show();
            $("#ddlIssueRelated").hide();
            $("#ddlGrivCategory").hide();
            $("#ddlGrivsubCategory").hide();
            $scope.Grievance.Remark = '';
        }
        else if (items == 'AssignToDeveloper') {
            $("#ddlDeveloper").show();
            $("#ddlPriority").show();
            $("#hdnbtnAll").show();
            $("#ddlIssueRelated").show();
            $("#ddlGrivCategory").show();
            $("#ddlGrivsubCategory").hide();
            $("#Desposehdn").hide();
            $scope.Grievance.Remark = '';
            $scope.Grievance.Priority = '0';
            $scope.Grievance.Developer = '0';
            $scope.Grievance.IssueRelated = '0';
            $scope.Grievance.GrivanceCategoryid = '0';
            $scope.Grievance.GrivanceSubCategoryid = '0';
            $scope.GetIssueRelatedList();
            $scope.GetGrivanceCategoryList();

        }
        else if (items == 'Reopen') {
            $("#ddlDeveloper").show();
            $("#ddlPriority").show();
            $("#hdnbtnAll").show();
            $("#ddlIssueRelated").show();
            $("#ddlGrivCategory").show();
            $("#ddlGrivsubCategory").hide();
            $("#Desposehdn").hide();
            $scope.Grievance.Remark = '';
            $scope.Grievance.Priority = '0';
            $scope.Grievance.Developer = '0';
            $scope.Grievance.IssueRelated = '0';
            $scope.Grievance.GrivanceCategoryid = '0';
            $scope.Grievance.GrivanceSubCategoryid = '0';
            $scope.GetIssueRelatedList();
            $scope.GetGrivanceCategoryList();
        }
    };
    $scope.GrievanceActionIADisposebyGC = function (items) {
        if (items == 'Dispose') {
            $("#ddlDeveloper").hide();
            $("#ddlPriority").hide();
            $("#hdnbtnAll").hide();
            $("#ddlIssueRelated").hide();
            $("#Desposehdn").show();
            $scope.Grievance.Remark = '';
        }
        else if (items == 'Reopen') {
            $("#ddlDeveloper").show();
            $("#ddlPriority").show();
            $("#hdnbtnAll").show();
            $("#ddlIssueRelated").show();
            $("#Desposehdn").hide();
            $scope.Grievance.Remark = '';
            $scope.Grievance.Priority = '0';
            $scope.Grievance.Developer = '0';
            $scope.Grievance.IssueRelated = '0';
            $scope.GetIssueRelatedList();
        }
        else if (items == 'MoreInformation') {
            $("#ddlDeveloper").hide();
            $("#ddlPriority").hide();
            $("#hdnbtnAll").hide();
            $("#ddlIssueRelated").hide();
            $("#Desposehdn").show();
            $scope.Grievance.Remark = '';

        }
    };
    $scope.GetIssueRelatedList = function () {
        $scope.GetIssueRelatedList = [];
        $http.post(SiteUrl + "Grievance/GetIssueRelatedList").
            success(function (data, status) {
                $scope.GetIssueRelatedList = data;
            }).error(function (data, status) {
                alert("Geting Some Errors In Issue List");
            })
    };
    $scope.GetGrivanceCategoryList = function () {
        $scope.GrivanceCategoryList = [];
        $http.post(SiteUrl + "Grievance/GetGrivanceCategoryList").
            success(function (data, status) {
                $scope.GrivanceCategoryList = data;
            }).error(function (data, status) {
                alert("Geting Some Errors In Issue List");
            })
    };
    $scope.GetGrivanceSubCategoryList = function () {
        $scope.GrivanceSubCategoryList = [];
        $http.post(SiteUrl + "Grievance/GetGrivanceSubCategoryList", { GriveanceCategoryId: $scope.Grievance.GrivanceCategoryid }).
            success(function (data, status) {
                $scope.GrivanceSubCategoryList = data;
                if ($scope.Grievance.GrivanceCategoryid != '7' && $scope.Grievance.GrivanceCategoryid != '8' && $scope.Grievance.GrivanceCategoryid != '9' && $scope.Grievance.GrivanceCategoryid != '10' && $scope.Grievance.GrivanceCategoryid != '11' && $scope.Grievance.GrivanceCategoryid != '12') {
                    $("#ddlGrivsubCategory").show();
                }
                else {
                    $("#ddlGrivsubCategory").hide();
                }


            }).error(function (data, status) {
                alert("Geting Some Errors In Issue List");
            })
    };

    $scope.getRowTextColor = function (item) {
        if (item.Remark_by_OIC !== '' && item.Remark_by_OIC !== null) {
            return 'Green';
        }
        //if (item.Remark_by_OIC !== '' && item.Remark_by_OIC !== null && item.DaysLastUpdate > 5) {
        //    return 'Green';
        //}
        else if (item.DaysLastUpdate > 5 && (item.Remark_by_OIC === '' || item.Remark_by_OIC === null)) {
            return 'Red';
        }
        else {
            return 'Black';
        }
    };
    $scope.OnPageLoad();
});