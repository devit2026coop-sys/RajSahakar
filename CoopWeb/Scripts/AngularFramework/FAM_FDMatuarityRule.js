var MasterMod = angular.module('FAM_FDMatuarityRuleModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("FAM_FDMatuarityRuleCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    var filedata = [];
    $scope.Showgride = true;
    $scope.appgrid = false;
    $scope.respgrid = false;
    $scope.advgrid = false;
    $scope.oicgrid = false;
    $scope.CaseRegEncloDetailsDataLenth = 0;
    $scope.CaseRegApplntDetailsDataLenth = 0;
    $scope.CaseRegRespoDetailsDataLenth = 0;
    $scope.CaseRegAdvocDetailsDataLenth = 0;
    $scope.lstAppliantScope = [];
    $scope.lstRespondScope = [];
    $scope.lstAdvocateScope = [];
    $scope.lstOICScope = [];
    var records = [];
    var lstEnclouserupload = [];
    function FileDataWithRow() {
        this.File = '';
        this.RowNo = '';
        this.Flag = '';
        this.FileName = '';
    }
    $scope.ShowForm = false;
    $scope.Showtabl = true;
    $scope.Openform = function () {
        $scope.ShowForm = true;
        $scope.Showtabl = false;

    }
    $scope.closeform = function () {
        $scope.ShowForm = false;
        $scope.Showtabl = true;

    }

    $scope.UploadEnclouserFile = function (file, row) {
        var DDLIdLocal = $(row).attr("DDLID");
        var DDLIdVal = $("#" + DDLIdLocal).val();
        var TempData = lstEnclouserupload.filter(x => x.RowNo == DDLIdVal)
        if (TempData.length > 0) {
            alert('Related Document Already Exist');
            return;
        }
        else {
            var FileDataWithRowObj = new FileDataWithRow();
            FileDataWithRowObj.File = file[0];
            FileDataWithRowObj.RowNo = DDLIdVal;
            lstEnclouserupload.push(FileDataWithRowObj);
        }
    }

    $scope.ClearCaseRegister = function () {
        $scope.CaseRegistration.Court = '0';
        $scope.CaseRegistration.Act = '0';
        $scope.CaseRegistration.section = '0';
        $scope.CaseRegistration.Case = '0';
        $scope.CaseRegistration.TypeOfCase = '0';
        $scope.CaseRegistration.CaseDescription = '';
        $scope.CaseRegistration.citytypeid = '0';
        $scope.CaseRegistration.cityblockid = '0';
        $scope.CaseRegistration.wardgpid = '0';
        $scope.CaseRegistration.DIVISION_ID = '0';
        $scope.CaseRegistration.districtid = '0';
        $scope.CaseRegistration.cityblockid = '0';
        $scope.CaseRegistration.cityblockid = '0';
        $scope.CaseRegistration.wardgpid = '0';
        $scope.CaseRegistration.villageid = '0';
        $scope.SocietyRegistration.DateOfAffiliation = '';
        //$('#txtAffiliationDate').val() = '';
    }

    $scope.ClearFormCaseDetails = function () {
        $scope.CaseDetails.AppellantName = '';
        $scope.CaseDetails.FatherHasbandName = '';
        $scope.CaseDetails.MobileNo = '';
        $scope.CaseDetails.EmailID = '';
        $scope.CaseDetails.AddharID = '';
        $scope.CaseDetails.Gender = undefined;
        $scope.CaseDetails.Category = '0';
        $scope.CaseDetails.AppellantType = '0';
        $scope.CaseDetails.Address = '0';
        $scope.CaseDetails.State = 'Rajasthan';
        $scope.CaseDetails.DistrictCouncil = '0';
        $scope.CaseDetails.IsPrimery = undefined;
    }

    $scope.SaveCaseRegister = function () {
        if ($scope.ValidationCase()) {
            var formData = new FormData();
            formData.append("CaseRegID", $scope.CaseRegistration.CaseRegID);
            formData.append("Court", $scope.CaseRegistration.Court);
            formData.append("Act", $scope.CaseRegistration.Act);
            formData.append("section", $scope.CaseRegistration.section);
            formData.append("Case", $scope.CaseRegistration.Case);
            formData.append("TypeOfCase", $scope.CaseRegistration.TypeOfCase);
            formData.append("CaseDescription", $scope.CaseRegistration.CaseDescription);
            formData.append("DateOfAffiliation", $('#txtAffiliationDate').val().split("/").reverse().join("-"));
            formData.append("isruralurban", $scope.CaseRegistration.isruralurban);
            formData.append("districtid", $scope.CaseRegistration.districtid);
            formData.append("citytypeid", $scope.CaseRegistration.citytypeid);
            formData.append("cityblockid", $scope.CaseRegistration.cityblockid);
            formData.append("wardgpid", $scope.CaseRegistration.wardgpid);
            formData.append("villageid", $scope.CaseRegistration.villageid);
            $http.post(SiteUrl + "/Citizen/SaveCaseRegistraion", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if ($scope.CaseRegistration.CaseRegID != data) {
                        var k = data;
                        $scope.CaseRegistration.CaseRegID = k;
                        var message = 'Data Save Successfully';
                        angular.element(document.querySelector("#defaultOpen")).addClass("tablinks active");
                        angular.element(document.querySelector("#Enclouser")).addClass("disshow");
                        angular.element(document.querySelector("#iddapplant")).addClass("tablinks stpclick");
                        angular.element(document.querySelector("#iddResponed")).addClass("tablinks stpclick");
                        angular.element(document.querySelector("#iddAdvocate")).addClass("tablinks stpclick");
                        angular.element(document.querySelector("#OIC")).addClass("tablinks stpclick");
                        alert(message);
                        $scope.caseformShow = false;
                        $scope.SubformShow = true;
                        $scope.ClearCaseRegister();

                    }
                    else {
                        var message = 'Data Update Successfully';
                        alert(message);
                        $scope.BackToPage();
                        $scope.CaseRegDetailList();
                    }
                    if (data.Status) {
                    }
                }).
                error(function (data, status) {
                    alert("Error while case registration insert");
                    $scope.ClearCaseRegister();
                    $scope.SubformShow = false;
                });
        }
    };
    $scope.SaveCaseDetails = function (flag) {
        var formData = new FormData();
        formData.append("lstAppliantScope", JSON.stringify($scope.lstAppliantScope));
        $http.post(SiteUrl + "/Citizen/SaveCaseDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var k = data;
                $scope.lstAppliantScope = [];
                $scope.appgrid = false;
                $scope.respgrid = false;
                var message = 'Data Save Successfully';

                if ($scope.CaseRegApplntDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#defaultOpen")).addClass("tablinks stpclick");
                    angular.element(document.querySelector("#iddapplant")).removeClass("tablinks active");
                    angular.element(document.querySelector("#iddapplant")).addClass("tablinks");
                    angular.element(document.querySelector("#iddResponed")).addClass("tablinks active");
                    angular.element(document.querySelector("#Appellant")).removeClass("disshow");
                    angular.element(document.querySelector("#Appellant")).addClass("dishide");
                    angular.element(document.querySelector("#Responed")).addClass("disshow");
                }
                alert(message);
                $scope.ClearFormCaseDetails();
                if (data.Status) {
                }
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });
        //}
    };

    $scope.SaveCaseResDetails = function (flag) {
        var formData = new FormData();
        formData.append("lstRespondScope", JSON.stringify($scope.lstRespondScope));
        $http.post(SiteUrl + "/Citizen/SaveRespCaseDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var k = data;
                $scope.lstRespondScope = [];
                $scope.appgrid = false;
                $scope.respgrid = false;
                var message = 'Data Save Successfully';

                if ($scope.CaseRegRespoDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#Appellant")).addClass("dishide");
                    angular.element(document.querySelector("#iddResponed")).removeClass("tablinks active");
                    angular.element(document.querySelector("#iddResponed")).addClass("tablinks");
                    angular.element(document.querySelector("#iddAdvocate")).addClass("tablinks active");
                    angular.element(document.querySelector("#Responed")).removeClass("disshow");
                    angular.element(document.querySelector("#Responed")).addClass("dishide");
                    angular.element(document.querySelector("#Advocate")).addClass("disshow");
                }
                alert(message);
                $scope.ClearFormCaseDetails();
                if (data.Status) {
                }

            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });
        //}
    };

    $scope.SaveAdvocateDetails = function () {
        var formData = new FormData();
        formData.append("lstAdvocateScope", JSON.stringify($scope.lstAdvocateScope));
        $http.post(SiteUrl + "/Citizen/SaveAdvocateDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var k = data;
                var message = 'Data Save Successfully';
                $scope.advgrid = false;

                if ($scope.CaseRegAdvocDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#Responed")).addClass("dishide");
                    angular.element(document.querySelector("#iddAdvocate")).removeClass("tablinks active");
                    angular.element(document.querySelector("#iddAdvocate")).addClass("tablinks");
                    angular.element(document.querySelector("#OIC")).addClass("tablinks active");
                    angular.element(document.querySelector("#Advocate")).removeClass("disshow");
                    angular.element(document.querySelector("#Advocate")).addClass("dishide");
                    angular.element(document.querySelector("#OICDetail")).addClass("disshow");
                }
                alert(message);

                $scope.CaseRegDetailList();
                if (data.Status) {
                }
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });
        // }
    };

    $scope.SaveOICDetails = function () {
        if ($scope.ValidationOICDetails()) {
            $(".se-pre-con").fadeIn("slow");
            var formData = new FormData();
            formData.append("CaseRegID", $scope.CaseRegistration.CaseRegID);
            formData.append("OICName", $scope.OICDetails.OICName);
            formData.append("OICType", $scope.OICDetails.OICType);
            formData.append("OICMobileNo", $scope.OICDetails.OICMobileNo);
            $http.post(SiteUrl + "/Citizen/SaveOICDetails", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    var k = data;
                    var message = 'Data Save Successfully';
                    $(".se-pre-con").fadeOut("slow");
                    alert(message);
                    $scope.BackToPage();
                    $scope.CaseRegDetailList();
                    if (data.Status) {
                    }
                }).
                error(function (data, status) {
                    alert("Error while Menu insert");
                });
        }
    };

    $scope.SaveEnclouserDetails = function () {
        var Count = 0;
        if ($scope.ValidationEnclouserDetails()) {
            var DDLENCVAL = $('#enclouserval').val() - 1;

            if (lstEnclouserupload.length == $scope.EnclouserDetails.NoofEnclouser - 1) {
                var formData = new FormData();
                formData.append("CaseRegID", $scope.CaseRegistration.CaseRegID);
                formData.append("NoofEnclouser", $scope.EnclouserDetails.NoofEnclouser);
                for (var Y = 0; Y < lstEnclouserupload.length; Y++) {
                    formData.append((lstEnclouserupload[Y].RowNo), lstEnclouserupload[Y].File);
                }
                $http.post(SiteUrl + "/Citizen/SaveEnclouserDetails", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {

                        var k = data;
                        var message = 'Data Save Successfully';
                        if ($scope.CaseRegEncloDetailsDataLenth == 0) {
                            angular.element(document.querySelector("#defaultOpen")).removeClass("tablinks active");
                            angular.element(document.querySelector("#defaultOpen")).addClass("tablinks");
                            angular.element(document.querySelector("#iddapplant")).addClass("tablinks active");
                            angular.element(document.querySelector("#Enclouser")).removeClass("disshow");
                            angular.element(document.querySelector("#Enclouser")).addClass("dishide");
                            angular.element(document.querySelector("#Appellant")).addClass("disshow");
                            $scope.recordstblshow = false;
                            $scope.enclgrid = true;
                        }
                        alert(message);
                        if (data.Status) {
                        }
                    }).
                    error(function (data, status) {
                        alert("Error while Menu insert");
                    });
            }
            else {
                alert("Please upload all document");
            }

        }
    };


    $scope.GetCaseDetails = function (SID) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Citizen/GetCaseDetails/?CaseRegID=" + SID).
            success(function (data, status) {
                if (data > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.CaseRegDetailList();
                    ShowModelAlert("Case Registration", "Data Save Successfully");
                }
                else {
                    ShowModelAlert("Case Registration", "Error While Save Case Registration.");
                }
            }).
            error(function (data, status) {
                $scope.CaseRegDetailListData = [];
            });
    }

    $scope.GetCaseHistory = function (SID) {
        $(".se-pre-con").fadeIn("slow");
        $scope.CaseRegHistoryData = null;
        $http.post(SiteUrl + "/Citizen/GetCaseHistory", { "CaseRegNo": SID }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CaseRegHistoryData = '';
                $scope.CaseRegHistoryData = data.result[0].row;
                $('#myModal').modal('show');
            }).
            error(function (data, status) {
                $scope.CaseRegHistoryData = [];
            });

    }

    $scope.GetCaseDetailsByCaseId = function (SID) {
        $scope.CaseRegID = SID;
        $(".se-pre-con").fadeIn("slow");
        $scope.CaseRegDetailsData = null;
        $http.post(SiteUrl + "/Citizen/GetCaseDetailsByCaseId", { "CaseRegId": SID }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CaseRegDetailsData = '';
                $scope.CaseRegDetailsData = data.Table;
                $scope.CaseRegDetailsData = $scope.CaseRegDetailsData[0];
                $scope.CaseRegApplntDetailsData = '';
                $scope.CaseRegApplntDetailsData = data.Table1;
                $scope.CaseRegRespoDetailsData = '';
                $scope.CaseRegRespoDetailsData = data.Table2;
                $scope.CaseRegAdvocDetailsData = '';
                $scope.CaseRegAdvocDetailsData = data.Table4;
                $scope.CaseRegEncloDetailsData = '';
                $scope.CaseRegEncloDetailsData = data.Table3;
                $scope.CaseRegOICDetailsData = '';
                $scope.CaseRegOICDetailsData = data.Table5;
            }).
            error(function (data, status) {
                $scope.CaseRegDetailsData = [];
            });
    }

    $scope.GetCourtList = function () {
        $http.get(SiteUrl + "/Citizen/CourtList").
            success(function (data, status) {
                $scope.CourtListData = data;

            }).
            error(function (data1, status) {
                $scope.CourtListData = [];
            });
    }

    $scope.GetCourtActList = function () {
        $http.get(SiteUrl + "/Citizen/CourtActList").
            success(function (data, status) {
                $scope.CourtActListData = data;

            }).
            error(function (data1, status) {
                $scope.CourtActListData = [];
            });
    }

    $scope.GetCourtSectionList = function () {
        $http.get(SiteUrl + "/Citizen/CourtSectionList").
            success(function (data, status) {
                $scope.CourtSectionListData = data;

            }).
            error(function (data1, status) {
                $scope.CourtSectionListData = [];
            });
    }

    $scope.GetCourtCaseTypeList = function () {
        $http.get(SiteUrl + "/Citizen/CourtCaseTypeList").
            success(function (data, status) {
                $scope.CourtCaseTypeListData = data;

            }).
            error(function (data1, status) {
                $scope.CourtCaseTypeListData = [];
            });
    }

    $scope.CourtPurposeList = function () {
        $http.get(SiteUrl + "/Citizen/CourtPurposeList").
            success(function (data, status) {
                $scope.CourtPurposeListData = data;

            }).
            error(function (data1, status) {
                $scope.CourtPurposeListData = [];
            });
    }

    $scope.EnclouserDocList = function () {

        $http.get(SiteUrl + "/Citizen/EnclouserDocList").
            success(function (data, status) {
                $scope.EnclouserDocListData = data;
            }).
            error(function (data1, status) {
                $scope.EnclouserDocListData = [];
            });
    }

    $scope.AdvocateTypeList = function () {
        $http.get(SiteUrl + "/Citizen/AdvocateTypeList").
            success(function (data, status) {
                $scope.AdvocateTypeListData = data;
            }).
            error(function (data1, status) {
                $scope.AdvocateTypeListData = [];
            });
    }

    $scope.CourtAdvocateList = function () {
        $http.get(SiteUrl + "/Citizen/CourtAdvocateList").
            success(function (data, status) {
                $scope.CourtAdvocateListData = data;
            }).
            error(function (data1, status) {
                $scope.CourtAdvocateListData = [];
            });
    }

    $scope.CaseApplicationView = function (CIDEnc) {
        if (CIDEnc != '') {
            $scope.GetCaseDetailsByCaseId(CIDEnc);
            window.location = "../Citizen/CaseDetailView?Case=" + CIDEnc;
        }
    }

    $scope.GetCityTypeList = function () {
        $http.get(SiteUrl + "/Citizen/CityTypeList").
            success(function (data, status) {
                $scope.CityTypeList = data;
            }).
            error(function (data1, status) {
                $scope.CityTypeList = [];
            });
    }


    $scope.GetAllCaseList = function () {
        $http.post(SiteUrl + "/Citizen/CaseList", $scope.CaseRegistration).
            success(function (data, status) {
                $scope.CaseRegDetailListData = data;
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
            }).
            error(function (data1, status) {
                $scope.CaseRegDetailListData = [];
            });
    }


    $scope.CaseRegDetailList = function () {
        $scope.DateOfAffiliation = $('#txtfAffiliationDate').val() == '' ? '' : $('#txtfAffiliationDate').val().split("/").reverse().join("-");
        $scope.CaseStatus = $("input[name='CaseStatus']:checked").val() == null ? '' : $("input[name='CaseStatus']:checked").val();
        $http.post(SiteUrl + "/Citizen/CaseRegDetailList", { Date: $scope.DateOfAffiliation, Status: $scope.CaseStatus }).
            success(function (data, status) {
                $scope.CaseRegDetailListData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? '' : ''
                    }
                    return '';
                };

            }).
            error(function (data1, status) {
                $scope.CaseRegDetailListData = [];
            });
    }

    $scope.delete = function () {
        $http.post(SiteUrl + "/Citizen/DeleteEnclouser", { Id: $scope.CaseRegistration.CaseRegID }).
            success(function (data, status) {
                ShowModelAlert("Case Registration", "Data deleted Successfully");
            }).
            error(function (data, status) {
            });
    };

    $scope.deleteoic = function () {
        $http.post(SiteUrl + "/Citizen/DeleteOIC", { Id: $scope.CaseRegistration.CaseRegID }).
            success(function (data, status) {
                ShowModelAlert("Case Registration", "Data deleted Successfully");
            }).
            error(function (data, status) {
            });
    };

    $scope.deleteadvocate = function () {
        $http.post(SiteUrl + "/Citizen/DeleteAdvocateDetail", { Id: $scope.CaseRegistration.CaseRegID }).
            success(function (data, status) {
                ShowModelAlert("Case Registration", "Data deleted Successfully");
            }).
            error(function (data, status) {
            });
    };



    $scope.update = function (item) {
        $scope.SubformShow = true;
        $scope.caseformShow = true;
        $scope.Showgride = false;
        $scope.CaseRegistration.CaseRegID = item.ID;
        $scope.CaseRegistration.Court = item.CourtId;
        $scope.CaseRegistration.Act = item.ActId;
        $scope.CaseRegistration.section = item.SectionId;
        $scope.CaseRegistration.Case = item.CaseId;
        $scope.CaseRegistration.TypeOfCase = item.TypeOfCase;
        $scope.CaseRegistration.DateOfAffiliation = new Date(item.AffilationDate);
        $scope.CaseRegistration.isruralurban = item.isruralurban == 1 ? true : false;
        $scope.RuralUrbanBlock();
        $scope.CaseRegistration.citytypeid = item.citytypeid;
        $scope.GetCityListByCityTypes(1);

        $scope.CaseRegistration.cityblockid = item.cityblockid;
        $scope.GetWardList(1);

        $scope.CaseRegistration.districtid = item.districtid;
        $scope.CaseRegistration.villageid = item.villageid;
        $scope.CaseRegistration.wardgpid = item.wardgpid;
        $scope.CaseRegistration.CaseDescription = item.CaseDescription;
        $(".se-pre-con").fadeIn("slow");
        $scope.CaseRegDetailsData = null;
        $http.post(SiteUrl + "/Citizen/GetCaseDetailsByCaseId", { "CaseRegId": item.ID }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CaseRegDetailsData = '';
                $scope.CaseRegDetailsData = data.Table;
                $scope.CaseRegDetailsData = $scope.CaseRegDetailsData[0];
                $scope.CaseRegApplntDetailsData = '';
                $scope.CaseRegApplntDetailsData = data.Table1;
                angular.forEach($scope.CaseRegApplntDetailsData, function (item) {
                    $scope.lstAppliantScope.push(
                        {
                            "CaseRegID": $scope.CaseRegistration.CaseRegID,
                            "AppellantType": item.AppellantType,
                            "AppellantTypeId": item.AppellantTypeId,
                            "FatherHasbandName": item.FatherHasbandName,
                            "Gender": item.Gender,
                            "GenderID": item.GenderID,
                            "Category": item.Category,
                            "CategoryID": item.CategoryID,
                            "AppellantName": item.AppellantName,
                            "MobileNo": item.MobileNo,
                            "EmailID": item.EmailID,
                            "AddharID": item.AddharID,
                            "Address": item.Address,
                            "State": item.State,
                            "District": item.District,
                            "DistrictID": item.DistrictID,
                            "IsActive": item.IsActive,
                            "IsPrimery": item.IsPrimery,
                            "IsActiveID": item.IsActiveID,
                            "IsPrimeryID": item.IsPrimeryID,
                            "flag": 1
                        }
                    );
                    $scope.appgrid = true;
                });

                $scope.CaseRegApplntDetailsDataLenth = $scope.CaseRegApplntDetailsData.length;
                $scope.CaseRegRespoDetailsData = '';
                $scope.CaseRegRespoDetailsData = data.Table2;

                angular.forEach($scope.CaseRegRespoDetailsData, function (item) {
                    $scope.lstRespondScope.push(
                        {
                            "CaseRegID": $scope.CaseRegistration.CaseRegID,
                            "AppellantType": item.AppellantType,
                            "AppellantTypeId": item.AppellantTypeId,
                            "FatherHasbandName": item.FatherHasbandName,
                            "Gender": item.Gender,
                            "GenderID": item.GenderID,
                            "Category": item.Category,
                            "CategoryID": item.CategoryID,
                            "AppellantName": item.AppellantName,
                            "MobileNo": item.MobileNo,
                            "EmailID": item.EmailID,
                            "AddharID": item.AddharID,
                            "Address": item.Address,
                            "State": item.State,
                            "District": item.District,
                            "DistrictID": item.DistrictID,
                            "IsActive": item.IsActive,
                            "IsPrimery": item.IsPrimery,
                            "IsActiveID": item.IsActiveID,
                            "IsPrimeryID": item.IsPrimeryID,
                            "flag": 0
                        }
                    );
                    $scope.respgrid = true;
                });


                $scope.CaseRegRespoDetailsDataLenth = $scope.CaseRegRespoDetailsData.length;
                $scope.CaseRegAdvocDetailsData = '';
                $scope.CaseRegAdvocDetailsData = data.Table4;
                angular.forEach($scope.CaseRegAdvocDetailsData, function (item) {
                    $scope.lstAdvocateScope.push(
                        {
                            "CaseRegID": $scope.CaseRegistration.CaseRegID,
                            "AdvocateName": item.AdvocateName,
                            "AdvocateType": item.AdvocateType,
                            "AdvocateTypeId": item.AdvocateTypeId,
                            "AdvocateMobileNo": item.AdvocateMobileNo,
                        }
                    );
                    $scope.advgrid = true;
                });

                $scope.CaseRegAdvocDetailsDataLenth = $scope.CaseRegAdvocDetailsData.length;
                $scope.CaseRegEncloDetailsData = '';
                $scope.CaseRegEncloDetailsData = data.Table3;
                $scope.CaseRegEncloDetailsDataLenth = $scope.CaseRegEncloDetailsData.length;
                if ($scope.CaseRegEncloDetailsDataLenth > 0) {
                    $scope.enclgrid = true;
                }
                $scope.CaseRegOICDetailsData = '';
                $scope.CaseRegOICDetailsData = data.Table5;
                $scope.CaseRegOICDetailsDataLenth = $scope.CaseRegOICDetailsData.length;
                if ($scope.CaseRegOICDetailsDataLenth > 0) {
                    $scope.oicgrid = true;
                }

                if ($scope.CaseRegEncloDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#defaultOpen")).click(openCity(event, 'Enclouser', true));
                    return;
                }

                else if ($scope.CaseRegApplntDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#iddapplant")).click(openCity(event, 'Appellant', true));
                    return;
                }

                else if ($scope.CaseRegRespoDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#iddResponed")).click(openCity(event, 'Responed', true));
                    return;
                }

                else if ($scope.CaseRegAdvocDetailsDataLenth == 0) {
                    angular.element(document.querySelector("#iddAdvocate")).click(openCity(event, 'Advocate', true));
                    return;
                }


                else if ($scope.CaseRegOICDetailsData.length == 0) {
                    angular.element(document.querySelector("#OIC")).click(openCity(event, 'OICDetail', true));
                    return;
                }

            }).
            error(function (data, status) {
                $scope.CaseRegDetailsData = [];
            });

    };

    $scope.ValidationCase = function () {
        message = "";
        if ($scope.CaseRegistration.Court == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Court' : '\nकृपया कोर्ट का चयन करें |';
            message += '<br />';
        }
        if ($scope.CaseRegistration.Act == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Act' : '\nकृपया अधिनियम  का चयन करे |';
            message += '<br />';
        }
        if ($scope.CaseRegistration.section == '0') {
            message += $scope._language == 'en-us' ? '\n Please Select the Section' : '\nकृपया अनुभाग का चयन करें |';
            message += '<br />';

        }
        if ($scope.CaseRegistration.Case == '0') {
            message += $scope._language == 'en-us' ? '\n Please Select the Case' : '\n कृपया मामला का चयन करें |';
            message += '<br />';
        }
        if ($scope.CaseRegistration.TypeOfCase == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Type Of Case' : '\n केस का प्रकार का चयन करें |';
            message += '<br />';
        }

        if ($scope.CaseRegistration.CaseDescription == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter Case Description' : '\nकेस का विवरण दर्ज करें |';
            message += '<br />';
        }

        if (message.length > 5) {
            ShowModelAlert("Case registration", message);
            return false;
        }
        return true;
    }

    $scope.ValidationAdvocateDetails = function () {
        var message = "";
        if ($scope.AdvocateDetails.AdvocateName == '' || $scope.AdvocateDetails.AdvocateName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Advocate Name' : '\nकृपया अधिवक्ता का नाम दर्ज करें |';
            message += '<br />';
        }
        if ($scope.AdvocateDetails.AdvocateType == '0' || $scope.AdvocateDetails.AdvocateType == 'undefined') {
            message += $scope._language == 'en-us' ? '\nPlease Select Advocate Type' : '\nकृपया एडवोकेट टाइप  का चयन करे |';
            message += '<br />';
        }
        if ($scope.AdvocateDetails.AdvocateMobileNo == '' || $scope.AdvocateDetails.AdvocateMobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\n Please Enter Valid Advocate Mobile No' : '\nकृपया वैध एडवोकेट मोबाइल नं दर्ज करें |';
            message += '<br />';
        }
        if (message.length > 10) {
            ShowModelAlert("Advocate Details", message);
            return false;
        }
        return true;
    }
    $scope.ValidationOICDetails = function () {
        var message = "";
        if ($scope.OICDetails.OICName == '' || $scope.OICDetails.OICName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter OIC Name' : '\nकृपया OIC का नाम दर्ज करें |';
            message += '<br />';
        }
        if ($scope.OICDetails.OICType == '0' || $scope.OICDetails.OICType == 'undefined') {
            message += $scope._language == 'en-us' ? '\nPlease Select OIC Type' : '\nकृपया OIC टाइप का चयन करे |';
            message += '<br />';
        }
        if ($scope.OICDetails.OICMobileNo == '' || $scope.OICDetails.OICMobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\n Please Enter Valid OIC Mobile No' : '\nकृपया वैध OIC मोबाइल नं दर्ज करें |';
            message += '<br />';
        }
        if (message.length > 10) {
            ShowModelAlert("Advocate Details", message);
            return false;
        }
        return true;
    }

    $scope.ValidationCaseDetails = function () {
        message = "";
        if ($scope.CaseDetails.AppellantName == '' || $scope.CaseDetails.AppellantName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Appellant Name' : '\nकृपया अपीलार्थी का चयन करें |';
            message += '<br />';
        }
        if (($scope.CaseDetails.FatherHasbandName == '' || $scope.CaseDetails.FatherHasbandName == undefined) && $scope.CaseDetails.AppellantType == '2' || $scope.CaseDetails.AppellantType == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Enter Father/Hasband Name' : '\nकृपया पिता / पति का दर्ज करे |';
            message += '<br />';
        }
        if ($scope.CaseDetails.MobileNo == '' || $scope.CaseDetails.MobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\n Please Enter Valid MobileNo' : '\nकृपया वैध मोबाइल नंबर दर्ज करें |';
            message += '<br />';

        }
        if ($scope.CaseDetails.EmailID == '' || $scope.CaseDetails.EmailID == undefined) {
            message += $scope._language == 'en-us' ? '\n Please Enter Valid EmailID' : '\n कृपया वैध ईमेल आईडी दर्ज करें |';
            message += '<br />';
        }

        if (($scope.CaseDetails.Gender == '' || $scope.CaseDetails.Gender == undefined) && $scope.CaseDetails.AppellantType == '2' || $scope.CaseDetails.AppellantType == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Gender' : '\nलिंग का चयन करें |';
            message += '<br />';
        }
        if (($scope.CaseDetails.Category == '0' || $scope.CaseDetails.Category == undefined) && $scope.CaseDetails.AppellantType == '2' || $scope.CaseDetails.AppellantType == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Category' : '\nवर्ग का चयन करें |';
            message += '<br />';
        }
        if ($scope.CaseRegistration.AppellantType == '0' || $scope.CaseDetails.AppellantType == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select AppellantType' : '\nअपीलार्थी प्रकार का चयन करें |';
            message += '<br />';
        }
        if ($scope.CaseDetails.Address == '' || $scope.CaseDetails.Address == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Address' : '\nपता दर्ज करें |';
            message += '<br />';
        }
        if ($scope.CaseDetails.State == '' || $scope.CaseDetails.State == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter State' : '\nराज्य दर्ज करें |';
            message += '<br />';
        }
        if ($scope.CaseDetails.DistrictCouncil == '' || $scope.CaseDetails.DistrictCouncil == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nजिला का चयन करें |';
            message += '<br />';
        }

        if (message.length > 5) {
            ShowModelAlert("Case registration", message);
            return false;
        }
        return true;
    }

    $scope.ValidationResCaseDetails = function () {
        message = "";
        if ($scope.CaseDetails.AppellantName == '' || $scope.CaseDetails.AppellantName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Responed Name' : '\nकृपया प्रतिक्रिया का चयन करें |';
            message += '<br />';
        }
        if (($scope.CaseDetails.FatherHasbandName == '' || $scope.CaseDetails.FatherHasbandName == undefined) && $scope.CaseDetails.AppellantType == '2' || $scope.CaseDetails.AppellantType == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Enter Father/Hasband Name' : '\nकृपया पिता / पति का दर्ज करे |';
            message += '<br />';
        }
        if ($scope.CaseDetails.MobileNo == '' || $scope.CaseDetails.MobileNo == undefined) {
            message += $scope._language == 'en-us' ? '\n Please Enter Valid MobileNo' : '\nकृपया वैध मोबाइल नंबर दर्ज करें |';
            message += '<br />';

        }
        if ($scope.CaseDetails.EmailID == '' || $scope.CaseDetails.EmailID == undefined) {
            message += $scope._language == 'en-us' ? '\n Please Enter Valid EmailID' : '\n कृपया वैध ईमेल आईडी दर्ज करें |';
            message += '<br />';
        }

        if (($scope.CaseDetails.Gender == '' || $scope.CaseDetails.Gender == undefined) && $scope.CaseDetails.AppellantType == '2' || $scope.CaseDetails.AppellantType == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Gender' : '\nलिंग का चयन करें |';
            message += '<br />';
        }
        if (($scope.CaseDetails.Category == '0' || $scope.CaseDetails.Category == undefined) && $scope.CaseDetails.AppellantType == '2' || $scope.CaseDetails.AppellantType == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Category' : '\nवर्ग का चयन करें |';
            message += '<br />';
        }
        if ($scope.CaseRegistration.AppellantType == '0' || $scope.CaseDetails.AppellantType == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select AppellantType' : '\nअपीलार्थी प्रकार का चयन करें |';
            message += '<br />';
        }
        if ($scope.CaseDetails.Address == '' || $scope.CaseDetails.Address == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Address' : '\nपता दर्ज करें |';
            message += '<br />';
        }
        if ($scope.CaseDetails.State == '' || $scope.CaseDetails.State == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter State' : '\nराज्य दर्ज करें |';
            message += '<br />';
        }
        if ($scope.CaseDetails.DistrictCouncil == '' || $scope.CaseDetails.DistrictCouncil == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nजिला का चयन करें |';
            message += '<br />';
        }

        if (message.length > 5) {
            ShowModelAlert("Case registration", message);
            return false;
        }
        return true;
    }

    $scope.ValidationEnclouserDetails = function () {
        message = "";

        if ($scope.EnclouserDetails.NoofEnclouser == '1') {
            message += $scope._language == 'en-us' ? '\nPlease Select No of Enclouser' : '\nकृपया संलग्नक की नहीं का चयन करें |';
            message += '<br />';
        }
        if ($scope.EnclouserDetails.Enclouserupload = undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Enclouserupload' : '\nकृपया संलग्नक अपलोड करें का चयन करे |';
            message += '<br />';
        }
        if ($scope.EnclouserDetails.DocType = undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Enclouser' : '\nकृपया संलग्नक का चयन करे |';
            message += '<br />';
        }

        if (message.length > 5) {
            ShowModelAlert("Enclouser Details", message);
            return false;
        }
        return true;
    }
    // on page load
    $scope.OnPageLoad = function () {
        $scope.ResCitizen = true;
        $scope.AppCitizen = true;
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        $scope.BackToPage = function () {
            window.location = "../Citizen/CaseRegistraion";
        }

        $(".se-pre-con").fadeOut("slow");
        var temp = window.location.search.split('=');
        if (temp.length > 1) {
            $scope.GetCaseDetailsByCaseId(temp[1]);
            return;
        }

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
    };

    $scope.uploadEnclouser = function () {
        $scope.recordstblshow = true;
        $scope.records = [];
        var X = $scope.EnclouserDetails.NoofEnclouser;
        records = [];
        for (var x = 1; x < X; x++) {
            records.push(
                {
                    "Id": x,
                    "DDLID": "DDLDocTypeID" + x
                });
        }
        $scope.records = records;
    }

    $scope.OnPageLoad();
    $scope.GetCourtList();
    $scope.GetCityTypeList();
    $scope.GetCourtActList();
    $scope.GetCourtSectionList();
    $scope.EnclouserDocList();
    $scope.GetCourtCaseTypeList();
    $scope.CourtPurposeList();

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.RuralUrbanBlock = function () {
        if ($scope.CaseRegistration.isruralurban == '0') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
        }
        else if ($scope.CaseRegistration.isruralurban == '1') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }

    }

    $scope.StateCitizenBlock = function () {
        if ($scope.CaseDetails.AppellantType == '1') {
            $scope.AppCitizen = false;
        }
        else {
            $scope.AppCitizen = true;
        }
    }

    $scope.StateRespCitizenBlock = function () {
        if ($scope.CaseDetails.AppellantType == '1') {
            $scope.ResCitizen = false;
        }
        else {
            $scope.ResCitizen = true;
        }
    }

    $scope.GetCityListByCityTypes = function (id) {

        if ($scope.CaseRegistration.citytypeid != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.CaseRegistration.citytypeid).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }

    };

    // START : Get Ward List
    $scope.GetWardList = function (id) {
        if ($scope.CaseRegistration.cityblockid != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.CaseRegistration.cityblockid).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }
    };

    $scope.GetDistrictList = function (id) {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + '0').
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.Divisions = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
    }
    // START : Get List of Villages
    $scope.VillageList = function (id) {
        if ($scope.CaseRegistration.wardgpid != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.CaseRegistration.wardgpid).
                success(function (data, status) {
                    $scope.Villages = data;

                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }

    };
    // END : Get List of Villages

    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        $scope.Blocks = [];
        if ($scope.CaseRegistration.districtid != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.CaseRegistration.districtid).
                success(function (data, status) {

                    $scope.Blocksdata = data;
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
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }

    };
    //end
    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {
        if ($scope.CaseRegistration.cityblockid != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.CaseRegistration.cityblockid).
                success(function (data, status) {
                    $scope.GramPanchayats = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }

    };
    // END : Get List of GramPanchayat

    $scope.AddAppliantbtn = function () {
        if ($scope.ValidationCaseDetails()) {
            $scope.lstAppliantScope.push(
                {
                    "CaseRegID": $scope.CaseRegistration.CaseRegID,
                    "AppellantType": $('#ddlappltype option:selected').text() == null ? 0 : $('#ddlappltype option:selected').text(),
                    "AppellantTypeId": $('#ddlappltype option:selected').val() == null ? 0 : $('#ddlappltype option:selected').val(),
                    "FatherHasbandName": $('#applfname').val() == null ? '' : $('#applfname').val(),
                    "Gender": $("input[name='AdministrativeBoard']:checked").parent('label').text() == null ? '' : $("input[name='AdministrativeBoard']:checked").parent('label').text(),
                    "GenderID": $("input[name='AdministrativeBoard']:checked").val() == null ? 0 : $("input[name='AdministrativeBoard']:checked").val(),
                    "Category": $('#applcategory option:selected').text() == null ? 0 : $('#applcategory option:selected').text(),
                    "CategoryID": $('#applcategory option:selected').val() == null ? 0 : $('#applcategory option:selected').val(),
                    "AppellantName": $('#applname').val() == null ? 0 : $('#applname').val(),
                    "MobileNo": $('#appmobileno').val() == null ? '' : $('#appmobileno').val(),
                    "EmailID": $('#appemail').val() == null ? '' : $('#appemail').val(),
                    "AddharID": $('#applaadhar').val() == null ? '' : $('#applaadhar').val(),
                    "Address": $('#appladdress').val() == null ? 0 : $('#appladdress').val(),
                    "State": $('#applstate').val() == null ? '' : $('#applstate').val(),
                    "District": $('#appdistrict option:selected').text() == null ? '' : $('#appdistrict option:selected').text(),
                    "DistrictID": $('#appdistrict option:selected').val() == null ? '' : $('#appdistrict option:selected').val(),
                    "IsActive": $("input[name='active']:checked").val() == null ? 0 : 'Yes',
                    "IsPrimery": $("input[name='primery']:checked").val() == null ? 0 : 'Yes',
                    "IsActiveID": $("input[name='active']:checked").val() == null ? 0 : 1,
                    "IsPrimeryID": $("input[name='primery']:checked").val() == null ? 0 : 1,
                    "flag": 1
                }
            );
            $scope.appgrid = true;
            $scope.ClearFormCaseDetails();
        }
    }


    $scope.AddRespondbtn = function () {
        if ($scope.ValidationResCaseDetails()) {
            $scope.lstRespondScope.push(
                {
                    "CaseRegID": $scope.CaseRegistration.CaseRegID,
                    "AppellantType": $('#ddlresltype option:selected').text() == null ? '' : $('#ddlresltype option:selected').text(),
                    "AppellantTypeId": $('#ddlresltype option:selected').val() == null ? '0' : $('#ddlresltype option:selected').val(),
                    "FatherHasbandName": $('#applfname').val() == null ? '' : $('#applfname').val(),
                    "Gender": $("input[name='AdministrativeBoard']:checked").parent('label').text() == null ? '' : $("input[name='AdministrativeBoard']:checked").parent('label').text(),
                    "GenderID": $("input[name='AdministrativeBoard']:checked").val() == null ? 0 : $("input[name='AdministrativeBoard']:checked").val(),
                    "Category": $('#applcategory option:selected').text() == null ? 0 : $('#applcategory option:selected').text(),
                    "CategoryID": $('#applcategory option:selected').val() == null ? 0 : $('#applcategory option:selected').val(),
                    "AppellantName": $('#applname').val() == null ? 0 : $('#applname').val(),
                    "MobileNo": $('#appmobileno').val() == null ? '' : $('#appmobileno').val(),
                    "EmailID": $('#appemail').val() == null ? '' : $('#appemail').val(),
                    "AddharID": $('#applaadhar').val() == null ? '' : $('#applaadhar').val(),
                    "Address": $('#appladdress').val() == null ? 0 : $('#appladdress').val(),
                    "State": $('#applstate').val() == null ? '' : $('#applstate').val(),
                    "District": $('#appdistrict option:selected').text() == null ? '' : $('#appdistrict option:selected').text(),
                    "DistrictID": $('#appdistrict option:selected').val() == null ? '' : $('#appdistrict option:selected').val(),
                    "IsActive": $("input[name='active']:checked").val() == null ? 0 : 'Yes',
                    "IsPrimery": $("input[name='primery']:checked").val() == null ? 0 : 'Yes',
                    "IsActiveID": $("input[name='active']:checked").val() == null ? 0 : 1,
                    "IsPrimeryID": $("input[name='primery']:checked").val() == null ? 0 : 1,
                    "flag": 0
                }
            );
            $scope.respgrid = true;
            $scope.ClearFormCaseDetails();
        }
    }

    $scope.AddAdvocatebtn = function () {
        if ($scope.ValidationAdvocateDetails()) {
            $scope.lstAdvocateScope.push(
                {
                    "CaseRegID": $scope.CaseRegistration.CaseRegID,
                    "AdvocateName": $('#advname').val() == null ? '' : $('#advname').val(),
                    "AdvocateType": $('#ddladvtype option:selected').text() == null ? '' : $('#ddladvtype option:selected').text(),
                    "AdvocateTypeId": $('#ddlappltype option:selected').val() == null ? 0 : 1,
                    "AdvocateMobileNo": $('#advmobno').val() == null ? '' : $('#advmobno').val(),
                }
            );
            $scope.advgrid = true;
        }
    }

    //START
    $scope.RemoveRowFromList = function (e) {
        var index = $scope.lstAppliantScope.indexOf(e);
        $scope.lstAppliantScope.splice(index, 1);
    }

    $scope.RemoveRowResFromList = function (e) {
        var index = $scope.lstRespondScope.indexOf(e);
        $scope.lstRespondScope.splice(index, 1);
    }

    $scope.AddNewCaseRegistraion = function () {
        $scope.caseformShow = true;
        $scope.Showgride = false;
    }

    $scope.AdvocateTypeList();
    $scope.CourtAdvocateList();
    $scope.GetDistrictList();
    $scope.Divisions();
   
});






