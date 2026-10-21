

var MasterMod = angular.module("IndexModule", ['angularUtils.directives.dirPagination']);
MasterMod.controller("IndexCont", function ($scope, $http) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 20;

    // get insert officer civil list PDF data
    $scope.GetCivilList = function () {
        $http.get(SiteUrl + "/User/GetCivilList").
            success(function (data, status) {
                $scope.CooperativesListData = data;
            }).
            error(function (data1, status) {
                $scope.CooperativesListData = [];
            });
    }
    // get insert officer civil list data


    $scope.GetOfficerCivilListNew = function () {
        $http.get(SiteUrl + "/User/GetOfficerCivilList").
            success(function (data, status) {
                $scope.GetOfficerCivilList = data;
            }).
            error(function (data1, status) {
                $scope.GetOfficerCivilList = [];
            });
    }



    $scope.translation = [];
    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
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

    };
    // Validation insert officer civil list data
    $scope.DataFormValidate = function () {
        var message = '';
        if ($scope.OfficerType == undefined || $scope.OfficerType == null || $scope.OfficerType == '') {
            message += 'Please Select Officer Type<br/>';
        }
        if ($scope.NameTitle == undefined || $scope.NameTitle == null || $scope.NameTitle == '') {
            message += 'Please Enter Name Title<br/>';
        }
        if ($scope.FirstName == undefined || $scope.FirstName == null || $scope.FirstName == '') {
            message += 'Please Enter First Name<br/>';
        }
        if ($scope.LastName == undefined || $scope.LastName == null || $scope.LastName == '') {
            message += 'Please Enter Last Name<br/>';
        }
        if ($('#DateOfBirth').val() == undefined || $('#DateOfBirth').val() == null || $('#DateOfBirth').val() == '' || $('#DateOfBirth').val() == '01-01-0001') {
            message += 'Please Enter Date Of Birth<br/>';
        }
        if ($('#DateOfJoining').val() == undefined || $('#DateOfJoining').val() == null || $('#DateOfJoining').val() == '' || $('#DateOfJoining').val() == '01-01-0001') {
            message += 'Please Enter Date Of Joining<br/>';
        }
        if ($('#DateOfRetirement').val() == undefined || $('#DateOfRetirement').val() == null || $('#DateOfRetirement').val() == '' || $('#DateOfRetirement').val() == '01-01-0001') {
            message += 'Please Enter Date Of Retirement<br/>';
        }
        if ($scope.Title == undefined || $scope.Title == null || $scope.Title == '') {
            message += 'Please Enter Title<br/>';
        }
        if ($scope.HomeDistrict == undefined || $scope.HomeDistrict == null || $scope.HomeDistrict == '') {
            message += 'Please Enter Home District<br/>';
        }
        if ($scope.Qualification == undefined || $scope.Qualification == null || $scope.Qualification == '') {
            message += 'Please Enter Qualification<br/>';
        }
        if ($scope.PostName == undefined || $scope.PostName == null || $scope.PostName == '') {
            message += 'Please Enter Post Name<br/>';
        }
        if ($scope.PresentPosting == undefined || $scope.PresentPosting == null || $scope.PresentPosting == '') {
            message += 'Please Enter Present Posting<br/>';
        }
        if ($('#FromDate').val() == undefined || $('#FromDate').val() == null || $('#FromDate').val() == '' || $('#FromDate').val() == '01-01-0001') {
            message += 'Please Enter From Date<br/>';
        }
        if (message.length > 250) {
            $("#ErroBox").modal("show");
            $("#ErroBox .modal-body").html(message);
            return false;
        }
        else {
            return true;
        }

    }
    $scope.SaveCivilList = function () {
        debugger;
        if (!$scope.DataFormValidate()) {
            return false;
        }
        var formData = new FormData();
        formData.append("BiodataListId", $scope.BiodataListId == undefined ? '0' : $scope.BiodataListId);
        formData.append("OfficerType", $scope.OfficerType);
        formData.append("NameTitle", $scope.NameTitle);
        formData.append("FirstName", $scope.FirstName);
        formData.append("LastName", $scope.LastName);
        formData.append("DateOfBirth", $('#DateOfBirth').val());
        formData.append("DateOfJoining", $('#DateOfJoining').val());
        formData.append("DateOfRetirement", $('#DateOfRetirement').val());
        formData.append("Title", $scope.Title);
        formData.append("HomeDistrict", $scope.HomeDistrict);
        formData.append("Qualification", $scope.Qualification);
        formData.append("PostName", $scope.PostName);
        formData.append("PresentPosting", $scope.PresentPosting);
        formData.append("FromDate", $('#FromDate').val());
        if (document.getElementById("OfficerImage").files.length > 0) {
            formData.append("OfficerImage", document.getElementById("OfficerImage").files[0]);
        }
        else {
            formData.append("OfficerImage", $scope.OfficerImage);
        }
        var TblCivilHisttoryDetails = [];
        $('#Civillisthistory tr').each(function (index) {
            TblCivilHisttoryDetails.push(
                {
                    "District": $(this).find('td:eq(0)').find('select').val(),
                    "JoingDateCivilList": $(this).find('td:eq(1)').find('input').val(),
                    "RelivingDateCivilList": $(this).find('td:eq(2)').find('input').val(),
                    "PostName1": $(this).find('td:eq(3)').find('input').val(),
                    "BiodateOfOffihistoryID": $(this).find('td:eq(4)').find('input').val() == "" ? 0 : $(this).find('td:eq(4)').find('input').val()
                });
        });
        formData.append("DataArray", JSON.stringify(TblCivilHisttoryDetails));
        $http.post(SiteUrl + "/User/InsertCivilList", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {

                    alert("DataValue Success");
                    window.location.reload();

                }).
                error(function (data, status) {
                    alert("Error while Civil List Data insert");
                });
        

    }
    //reset insert civil list data form
    $scope.RestCivilList = function () {
        $('.civilFormBox').find('.form-control').val("");
        window.location.reload();
    }
    // Filter CivilList
    $scope.FilterCivilList = function () {
        var data = {
            OfficerType: $scope.OfficerType,
            FirstName: $scope.FirstName,
            HomeDistrict: $scope.HomeDistrict
        }
        $http.post(SiteUrl + "/User/FilterCivilList", { "_Model": data }).
            success(function (data1, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.GetOfficerCivilList = data1;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error Data Filter");
            });
    }
    $scope.TblCivilHisttoryDetails = [];
    $scope.AddMoreRow = function () {
        $scope.GetDistrictList();
        $scope.TblCivilHisttoryDetails.push({ District: '', JoingDateCivilList: '', RelivingDateCivilList: '', PostName1: '' });
    };
    $scope.RemoveRow = function (index) {
        $scope.TblCivilHisttoryDetails.splice(index, 1);
    };
    // Delete Civil List
    $scope.DeleteCivilList = function (ID) {
        
        if (confirm("Are you sure you want to delete this Officer Name")) {
                $http.post(SiteUrl + "/User/DeleteCivilList/?Id=" + ID).
                success(function (data, status) {
                    $(".se-pre-con").fadeIn("slow");
                    $scope.GetOfficerCivilListNew();
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    alert("Record Delete Error");
                });
        }
    };
    // Edit Civil List
    $scope.EditCivilList = function (items) {       
        $http.post(SiteUrl + "/User/GetCivilListforedit/?Id=" + items).
            success(function (data, status) {
                debugger;
                $scope.EditList = data.Table;
                var EditList = $scope.EditList;
                  $scope.GetDistrictList();
                  $scope.BiodataListId = data.Table[0].BiodataListId,
                  $scope.OfficerType = data.Table[0].OfficerType,
                  $scope.NameTitle = data.Table[0].NameTitle,
                  $scope.FirstName = data.Table[0].FirstName,
                  $scope.LastName = data.Table[0].LastName,
                  $scope.DateOfBirth = data.Table[0].DateOfBirth;
                  $scope.DateOfJoining = data.Table[0].DateOfJoining;
                  $scope.DateOfRetirement = data.Table[0].DateOfRetirement;
                  $scope.Title = data.Table[0].Title,
                  $scope.HomeDistrict = data.Table[0].HomeDistrict,
                  $scope.Qualification = data.Table[0].Qualification,
                  $scope.PostName = data.Table[0].PostName,
                  $scope.PresentPosting = data.Table[0].PresentPosting,
                  $scope.FromDate = data.Table[0].FromDate;
                  $scope.District = data.Table[0].District;
                  $scope.RelivingDateCivilList = data.Table[0].RelivingDateCivilList;
                  $scope.JoingDateCivilList = data.Table[0].JoingDateCivilList;
                  $scope.OfficerImage = data.Table[0].OfficerImage;
                  if (data.Table[0].District != '' && data.Table[0].District != null) {
                      $scope.Addhistory = 1;
                   }
                $scope.OfficerImage = data.Table[0].OfficerImage;
                var TblCivilHisttoryDetails = [];
                for (var x = 0; x < EditList.length; x++) {
                    TblCivilHisttoryDetails.push(
                        {
                            "District": data.Table[x].District,
                            "JoingDateCivilList": data.Table[x].JoingDateCivilList,
                            "RelivingDateCivilList": data.Table[x].RelivingDateCivilList,
                            "PostName1": data.Table[x].PostName1,
                            "BiodateOfOffihistoryID": data.Table[x].BiodateOfOffihistoryID
                        });
                }
                $scope.TblCivilHisttoryDetails = TblCivilHisttoryDetails;
            }).
            error(function (data, status) {
                alert("Record Delete Error");
            });        

    };
    $scope.GetDistrictListData = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/User/DISTRICT_LIST").
            success(function (data, status) {
                $scope.GetDistrictListData = data;
            }).
            error(function (data1, status) {
                $scope.GetDistrictListData = [];
            });
    }
    // Get Civil Officer Type List
    $scope.GetCivilOfficerType = function () {
        $http.get(SiteUrl + "/User/GetCivilOfficerType").
            success(function (data, status) {
                $scope.GetCivilOfficerTypeData = data;
            }).
            error(function (data1, status) {
                $scope.GetCivilOfficerTypeData = [];
            });
    }

    $scope.GetCivilOfficerType = function () {
        $http.get(SiteUrl + "/user/GetCivilOfficerType").
            success(function (data, status) {
                $scope.GetCivilOfficerTypeData = data;
            }).
            error(function (data1, status) {
                $scope.GetCivilOfficerTypeData = [];
            });
    }

    $scope.OnPageLoad = function () {
        $scope.GetOfficerCivilListNew();
    }

    $scope.getTranslation();
    //    $scope.OnPageLoad();
    $scope.GetOfficerCivilListNew();
    $scope.GetDistrictList();
    $scope.GetCivilOfficerType();
    // $scope.GetCivilOfficerType();
    //GetSuccessStoryList
});







