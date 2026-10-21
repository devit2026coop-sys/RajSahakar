var Role = angular.module("GodownRole", ['angularUtils.directives.dirPagination']);

Role.directive('onlyLettersInput', onlyLettersInput);
function onlyLettersInput() {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z ]/g, '');
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
};

Role.controller("Godown", function ($scope, $http, $window) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    var Mdata = [];
    $scope.TblGodownTimeBoundDetails = [];
    $scope.TblDetails = [];
    $scope.godown = { isGSSKVSS: "1" };
    $scope.IsVisible = false;
    $scope.assemblyId = "0";
    //Insert user role details in db
    $scope.UploadFile = function (file) {
        $scope.File = file[0];
    }

    $scope.TblGodownTimeBoundDetails.push({ SocietyID: 0, MTCapacityId: 0, SchemeId: 0, FinancialYearID: 0 });

    $scope.AddMoreRow = function () {
        $scope.TblGodownTimeBoundDetails.push({ SocietyID: 0, MTCapacityId: 0, SchemeId: 0, FinancialYearID: 0 });
    };

    $scope.RemoveRow = function (index) {
        $scope.TblGodownTimeBoundDetails.splice(index, 1);
    };

    $scope.GenerateGodownRows = function () {

        $scope.TblGodownTimeBoundDetails = [];

        var count = parseInt($scope.godown.NoOfGodown || 0);

        for (var i = 0; i < count; i++) {
            $scope.TblGodownTimeBoundDetails.push({
                SocietyID: '',
                MTCapacityId: '',
                FinancialYearId: '',
                SchemeId: ''
            });
        }
    };

    var addressdataset1 = {
        Housebuilding: "",
        State: "6",
        District: " " + $("#hdnDistrictId").val() + " ",
        Lanestreet: "",// + ' ' + addressdata1[1],
        Locality: "",//+ ' ' + addressdata1[11],
        Pincode: ""
    }
    //New
    $scope.InsertGodownForm = function () {
        debugger;
        console.log("$scope.godown", $scope.godown);
        var formData = new FormData();
        var isUpdate = ($scope.godown.ProposalId && $scope.godown.ProposalId > 0);
        $scope.DISTRICT_ID = $("#district_MemberAdd").val();
        $scope.VILLAGE_ID = $("#ddlVillage_MemberAdd").val();
        $scope.BLOCK_ID = $("#ddlblocki_MemberAdd").val();
        $scope.GP_ID = $("#ddlGP_MemberAdd").val();
        formData.append("GodownId", $scope.godown.ProposalId || 0);
        formData.append("DISTRICT_ID", $scope.DISTRICT_ID);
        formData.append("VILLAGE_ID", $scope.VILLAGE_ID);
        formData.append("GP_ID", $scope.GP_ID);
        formData.append("BLOCK_ID", $scope.BLOCK_ID);
        formData.append("SanctionAmt", $scope.godown.Amt);
        formData.append("SocietyID", $scope.godown.SocietyID);
        formData.append("CCB", $scope.godown.CCB);
        formData.append("CCBBranchId", $scope.godown.CCBBranchId);
        formData.append("MTCapacityId", $scope.godown.MTCapacityId);
        formData.append("SocietyAvailabilityId", $scope.godown.SocietyAvailabilityId);
        formData.append("ApplicantName", $scope.godown.ApplicantName);
        formData.append("DocumentPath", $scope.godown.DocumentPath);
        formData.append("SocietyAvailabilityRemark", $scope.godown.SocietyAvailabilityRemark ?? '');
        formData.append("LandSize1", $scope.godown.LandSize1);
        formData.append("LandSize2", $scope.godown.LandSize2);
        formData.append("TotalLandSize1", $scope.godown.TotalLandSize1);
        formData.append("TotalLandSize2", $scope.godown.TotalLandSize2);
        formData.append("IFSCCode", $scope.godown.IFSCCode);
        formData.append("AccountNumber", $scope.godown.AccountNumber);
        formData.append("NoOfGodown", $scope.godown.NoOfGodown);
        formData.append("AssemblyId", $scope.assemblyId);
        formData.append("GodownDocuments", angular.toJson($scope.GodownDocuments));
        $scope.TblGodownTimeBoundDetails.forEach(function (r, v) {
            formData.append("TblGodownTimeBoundDetails[" + v + "].SocietyID", r.SocietyID);
            formData.append("TblGodownTimeBoundDetails[" + v + "].MTCapacityId", r.MTCapacityId);
            formData.append("TblGodownTimeBoundDetails[" + v + "].SchemeId", r.SchemeId);
            formData.append("TblGodownTimeBoundDetails[" + v + "].FinancialYearID", r.FinancialYearID);
            formData.append("TblGodownTimeBoundDetails[" + v + "].ID", r.ID == "" ? 0 : r.Id);
        });
        console.log("$scope.TblGodownTimeBoundDetails", $scope.TblGodownTimeBoundDetails);

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Godown/SaveAddGodownForm", formData, { withCredentials: true, headers: { 'Content-Type': undefined }, transformRequest: angular.identity }).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (parseInt(data, 10) > 0) {

                var Msg = '';

                if (isUpdate) {
                    Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया');
                    window.location.href = SiteUrl + "/godown/GodownFormationAction";
                }
                else {
                    Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    window.location.href = SiteUrl + "/godown/GodownFormationAction";
                }

                alert(Msg);
                window.location.href = SiteUrl + "/godown/GodownFormationAction";
                //window.location.reload();
            }
            else if (parseInt(data, 10) == -2) {
                var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                window.location.href = SiteUrl + "/godown/GodownFormationAction";
                alert(Msg);
            }
            else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                alert(Msg);
            }
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while auditor assignment insert");
            });
    };


    $scope.FinancialYearList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYears = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYears = data;
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
                $scope.FinancialYears = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetGodownFormEditData = function (GodownApplicationEncId) {
        $http.post(SiteUrl + "/Godown/GetGodownListApplicationFormViewData/?GodownApplicationEncId=" + GodownApplicationEncId).
            success(function (data, status) {
                $scope.GodownApplicationData = JSON.parse(data.DataList)[0];
                $scope.godown = $scope.GodownApplicationData;
                $scope.TblGodownTimeBoundDetails = JSON.parse(data.GodownTimeBoundList);
                angular.forEach($scope.TblGodownTimeBoundDetails, function (item) {
                    item.SocietyID = item.SocietyID ? item.SocietyID.toString() : '';
                    item.MTCapacityId = item.MTCapacityId ? item.MTCapacityId.toString() : '';
                    item.SchemeId = item.SchemeId ? item.SchemeId.toString() : '';
                    item.FinancialYearID = item.FinancialYearID ? item.FinancialYearID.toString() : '';
                });

                $scope.district_MemberAdd = $scope.godown.DISTRICT_ID + '';
                $scope.getblock();
                $scope.ddlblocki_MemberAdd = $scope.godown.BLOCK_ID + '';
                $scope.getGrampanchyat();
                $scope.ddlGP_MemberAdd = $scope.godown.GP_ID + '';
                $scope.getvillage();
                $scope.ddlVillage_MemberAdd = $scope.godown.VILLAGE_ID + '';
                $scope.godown.NoOfGodown = $scope.TblGodownTimeBoundDetails.length;
                $scope.GodownDocuments = JSON.parse(data.DocumentList);
                $scope.godown.Amt = $scope.godown.SanctionAmt;
                $scope.godown.CCBBranchId = $scope.godown.CCBBranchId.toString();
                $scope.godown.MTCapacityId = $scope.godown.MTCapacityId.toString();
                $scope.godown.SocietyAvailabilityId = $scope.godown.SocietyAvailabilityId.toString();
                $scope.GetMTCapacityList();
                $scope.GetGodownDocList();
                $scope.GetGodownLandList();
                $scope.GetCcbBranchList();
                $scope.godown.SocietyID = $scope.godown.SocietyId.toString();
            }).
            error(function (data, status) {
                $scope.GodownApplicationData = [];
            });
    }

    $scope.pageChangeHandler = function (num) {
    };

    $scope.GoBack = function () {
        window.location.href = '/godown/GodownFormationAction';
    };

    $scope.GetGodownId = function (id) {
        $scope.godownid = id;
    };

    $scope.ClearRoleofForm = function () {
        $scope.userrole = '';
        $scope.SingleChoise = false;
        $scope.userrole.LevelID = '0';
    };

    $scope.UpdateGodown = function () {

        var formData = new FormData();
        formData.append("GodownId", $scope.godownid);
        formData.append("Description", $scope.godown.Description);

        formData.append("Longitude", $scope.godown.Longitude);
        formData.append("Latitude", $scope.godown.Latitude);
        if (document.getElementById("godownPhoto").files.length > 0) {
            formData.append("file", document.getElementById("godownPhoto").files[0]);
        }
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/UpdateGodowndata", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (parseInt(data, 10) > 0) {
                    var Msg = (_language == 'en-us' ? 'Data Update Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    alert(Msg);
                    window.location.reload();
                }
                else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                    var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    alert(Msg);
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while auditor assignment insert");

            });
    };

    $scope.validatdata = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'Validate following:';

        if ($scope.godown.isOldGodown == undefined || $scope.godown.isOldGodown == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please select Godown ' : '\n <br />  Please select Godown |';
        }

        if ($('#HouseBuildingNo_MemberAdd').val() == '' || $('#HouseBuildingNo_MemberAdd').val() == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter Address' : '\n <br /> Please enter Address |';

        }
        if ($('#district_MemberAdd').val() == '' || $('#district_MemberAdd').val() == undefined || $scope.district_MemberAdd == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select District' : '\n <br /> Please Select District |';

        }
        if ($('#ddlConstituency').val() == '' || $('#ddlConstituency').val() == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Constituency' : '\n <br /> Please Select Constituency |';

        }
        if ($scope.godown.haveLandarea == undefined || $scope.godown.haveLandarea == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Does Society have Land or Not? ' : '\n <br /> Does Society have Land or Not?  |';

        }

        if ($scope.godown.isOldGodown == 0) {
            if ($scope.godown.CurrentStatus == '' || $scope.godown.CurrentStatus == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  Current Status Of Godown' : '\n <br />Please select  Current Status Of Godown |';

            }
            if ($scope.godown.Usegodown == '' || $scope.godown.Usegodown == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  Use of Godown' : '\n <br />Please select  Use of Godown|';

            }
            if ($scope.godown.ComplitionYear == '0' || $scope.godown.ComplitionYear == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  Complition Year' : '\n <br />Please select  Complition Year|';

            }
        }

        if ($scope.UralUrban == '' || $scope.UralUrban == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select Urban Rural' : '\n <br /> Please select Urban Rural |';

        }

        if ($scope.UralUrban == "1") {

            if ($('#ddlcity_MemberAdd').val() == '' || $('#ddlcity_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select City' : '\n <br />  Please Select City |';
            }

            if ($('#ddlward_MemberAdd').val() == '' || $('#ddlward_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Ward' : '\n <br />  Please Select Ward|';
            }
            if ($('#pincode_MemberAdd').val() == '' || $('#pincode_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Pin' : '\n <br />  Please Enter Pin |';
            }

        }
        if ($scope.UralUrban == "2") {

            if ($('#ddlblocki_MemberAdd').val() == '' || $('#ddlblocki_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Block' : '\n <br />  Please Select Block|';
            }


            if ($('#ddlGP_MemberAdd').val() == '' || $('#ddlGP_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram Panchayat' : '\n <br />  Please Select Gram Panchayat|';
            }

            if ($('#ddlVillage_MemberAdd').val() == '' || $('#ddlVillage_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Village ' : '\n <br />  Please Select Village|';
            }

            if ($('#ddlVillage_MemberAdd').val() == '' || $('#ddlVillage_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Village ' : '\n <br />  Please Select Village|';
            }
            if ($('#pincode_MemberAdd').val() == '' || $('#pincode_MemberAdd').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Pin' : '\n <br />  Please Enter Pin |';
            }

        }


        if ($scope.godown.haveLandarea == "1") {

            if ($scope.godown.Scheme == '' || $scope.godown.Scheme == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select Scheme' : '\n <br /> Please select Scheme |';

            }
            if ($scope.godown.Amt == '' || $scope.godown.Amt == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter  Amount' : '\n <br /> Please enter Amount |';

            }
            if ($('#idddate').val() == '' || $('#idddate').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select Sanction date' : '\n <br />Please select Sanction date |';

            }

            if ($scope.godown.Capacity == '' || $scope.godown.Capacity == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Capacity' : '\n <br /> Please Enter Capacity |';

            }



            //if ($scope.godown.CCB == '' || $scope.godown.CCB == undefined) {
            //    message += $scope._language == 'en-us' ? '\n <br /> Please select  CCB' : '\n <br /> Please select CCB |';

            //}

            if ($scope.godown.LandSize == '' || $scope.godown.LandSize == undefined || $scope.godown.LandSize == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter LandSize' : '\n <br /> Please Enter LandSize or Do not Enter 0 |';

            }


            if ($scope.godown.Description == '' || $scope.godown.Description == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Description' : '\n <br /> Please Enter Description |';
            }

            if ($scope.godown.LandType == '' || $scope.godown.LandType == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Land Type' : '\n <br /> Please Select LandType |';
            }

            if ($('#godownPhoto1')[0].files.length === 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Godown Photo' : '\n <br /> Please Upload Godown Photo |';
            }


            //if ($('#godownPhoto2')[0].files.length === 0) {
            //    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Sanction Document ' : '\n <br /> Please Upload Sanction Document  |';
            //}

            if ($scope.godown.isChcType == '' || $scope.godown.haveLandareaReamrk == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select CHC Type  ' : '\n <br /> Please select CHC Type  |';

            }

            if ($scope.godown.isChcType == '1') {

                if ($("#idddatetb").val() == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Establishment Date' : '\n <br /> Please Enter Establishment Date |';
                }

                if ($("#TotalCostidd").val() == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Cost' : '\n <br /> Please Enter Total Cost |';

                }
            }
        }


        if ($scope.godown.LandType == '0' || $scope.godown.LandType == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  LandType' : '\n <br /> Please select LandType |';

        }

        if ($scope.godown.isChcType == '0' || $scope.godown.isChcType == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select  Is CHC' : '\n <br /> Please select CHC |';

        }


        else if ($scope.godown.haveLandarea != undefined && $scope.godown.haveLandarea == 2) {
            if ($scope.godown.haveLandareaReamrk == '' || $scope.godown.haveLandareaReamrk == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please select  Remarks' : '\n <br /> Please select  Remarks |';

            }
        }

        if (message.length > 29) {
            ShowModelAlert("Godown", message);
            return false;
        }
        return true;

    };
    $scope.translation = [];
    $scope.getTranslation = function () {
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
        }

    };

    $scope.godown1 = [];
    $scope.GetGodownlistdetail = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/society/Godownlistsdetails/?Gid=" + id).
            success(function (data, status) {
                $scope.godown1 = data.Table;
                $scope.godown2 = data.Table1;
                $scope.godown3 = data.Table2;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }


                };
                $scope.showLoader = false;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching role list.");
                $scope.showLoader = false;
            });
    };
    $scope.GetCcbList = function () {
        $scope.BranchList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_LIST&MasterCode=0").
            success(function (data, status) {
                $scope.CCBdataset = data;
                if ($scope.hdnLevelId > 0 && $scope.hdnCcbId > 0) {
                    if ($scope.hdnLevelId == 7) {
                        $scope.BankList = data.filter(x => x.CCBId == $scope.CcbIds.CCBId);
                        $scope.search.MPacsBankId = "" + $scope.CcbIds.CCBId;
                        $scope.isBankDisabled = true;
                        $scope.GetCcbBranchList();
                    }
                    else if ($scope.hdnLevelId == 8 || $scope.hdnLevelId == 9) {
                        $scope.search.MPacsBankId = "" + $scope.CcbIds.CCBId;
                        $scope.GetCcbBranchList();
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankList = [];
            });
    };
    $scope.GetCcbList();

    $scope.GetCcbBranchList = function () {
        $(".se-pre-con").fadeIn("slow");

        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_BANK_LIST&MasterCode=" + ($scope.godown.CCB ?? 0)).
            success(function (data, status) {
                $scope.BranchList = data;
                if ($scope.hdnLevelId > 0 && $scope.hdnCcbId > 0) {
                    if ($scope.hdnLevelId == 8) {
                        $scope.BranchList = data.filter(x => x.Branch_ID == $scope.CcbIds.Branch_ID);
                        $scope.search.MPacsBranchId = "" + $scope.CcbIds.Branch_ID;
                        $scope.search.MPacsBankId = "" + $scope.CcbIds.CCBId;
                        $scope.isBranchDisabled = true;
                        $scope.isBankDisabled = true;
                    }
                    else {
                        $scope.BranchList = data;
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BranchList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.toggleAll = function () {
        var toggleStatus = $scope.isAllSelected;
        angular.forEach($scope.AllMenuList, function (itm) { itm.Checked = toggleStatus; });

    }

    $scope.optionToggled = function () {
        //$scope.isAllSelected = $scope.AllMenuList.every(function (itm) { return itm.Checked; })
    }

    $scope.ISNewGodown = function () {
        $scope.godown.haveLandarea = '0';
        $("#divDoesSociety").show();
        $("#divAddress").show();
        $("#divFooter").show();

    }

    $scope.ISOldGodown = function () {
        $scope.godown.haveLandarea = '1';
        $("#divDoesSociety").hide();
        $("#divAddress").show();
        $("#divFooter").show();
    }

    $scope.ClearAssignRightsForm = function () {
        window.location = '../User/UserRole';
    };
    $scope.ShowForm = false;
    $scope.ShowForm1 = true;
    $scope.AddRoleClick = function () {
        $scope.SingleChoise = false;
        $scope.userrole = '';
        $scope.IsVisible = false;
        $scope.userrole.LevelID = '0';
        $scope.ShowForm = true;
        $scope.ShowForm1 = false;

    };

    $scope.update = function (tableData) {
        $scope.userrole = tableData;
    };

    $scope.SetStatus = function (GodownId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/society/ChangeRoleStatus", { Id: GodownId, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.GetGodownlist(0);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.isKVSS = false;
    $scope.isGSS = false;
    $scope.getshowKvssGss = function (id) {
        if (id === 2) {
            $scope.isKVSS = true;
            $scope.isGSS = false;
        }
        else if (id === 1) {
            $scope.isGSS = true;
            $scope.isKVSS = false;
        }
    };
    $scope.isChcyes = false;
    $scope.isChcno = false;
    $scope.godown = { isGSSKVSS: "1" };
    $scope.type = "GSS";
    $scope.getshowChc = function (id) {

        if (id === 2) {
            $scope.isChcno = true;
            $scope.isChcyes = false;
        }
        else if (id === 1) {
            $scope.isChcyes = true;
            $scope.isChcno = false;
        }
    };
    $scope.isBenyes = false;
    $scope.isBenno = false;
    $scope.getshowben = function (id) {
        if (id === 2) {
            $scope.isBenno = true;
            $scope.isBenyes = false;
        }
        else if (id === 1) {
            $scope.isBenyes = true;
            $scope.isBenno = false;
        }

    };
    $scope.showGSSKvss = false;
    $scope.GetKVSSList = function (id) {
        if (id == 1) {
            //old
            $scope.type = "GSS"
            //new
            //$scope.type = ""
            $scope.showGSSKvss = true;
        }
        else if (id == 2) {
            $scope.type = "KVSS"
        }
        $scope.KvssGssList = [];
        $http.get(SiteUrl + "/Society/GetKVSSGSSData/?Type=" + $scope.type + "&DistrictId=" + $('#district_MemberAdd').val() + "&BlockId=" + ($scope.UralUrban == 1 ? $("#ddlblocki_MemberAdd").val() : 0) + "&CityId=" + ($scope.UralUrban == 1 ? 0 : $("#ddlcity_MemberAdd").val()) + "&GPId=" + ($scope.UralUrban == 1 ? $("#ddlGP_MemberAdd").val() : 0)).
            success(function (data, status) {
                $scope.KvssGssList = data;
                $scope.showGSSKvss = true;
            }).
            error(function (data, status) {
                $scope.KvssGssList = [];
                alert('Error while fetching KVSSList  data.');
            });
    };
    $scope.GetSocietyList = function () {
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Godown/GetSocietyData/?Type=" + $scope.type + "&DistrictId=" + $('#district_MemberAdd').val() + "&BlockId=" + ($scope.UralUrban == 1 ? $("#ddlblocki_MemberAdd").val() : 0) + "&CityId=" + ($scope.UralUrban == 1 ? 0 : $("#ddlcity_MemberAdd").val()) + "&GPId=" + ($scope.UralUrban == 1 ? $("#ddlGP_MemberAdd").val() : 0)).
            success(function (data, status) {
                $scope.SocietyList = data;

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching SocietyList  data.');
            });
    };
    $scope.ConstituencyList = [];
    $scope.GetConstituencyList = function () {
        if ($("#district_MemberAdd").val() != "" && $("#district_MemberAdd").val() != undefined) {

            $http.post(SiteUrl + "/FertilizerSupply/GetConstituencyListByDistrictId", { DistrictId: $('#district_MemberAdd').val() }).
                success(function (data, status) {
                    $scope.ConstituencyList = data;
                }).
                error(function (data, status) {
                    $scope.ConstituencyList = [];
                    alert('Error while fetching Constituency List  data.');
                });
        }
    };

    $scope.MTCapacityList = [];
    $scope.GetMTCapacityList = function () {
        $http.get(SiteUrl + "/Godown/GetNewLovValuesByListId/?ListCode=MetricTon")
            .success(function (data, status) {
                $scope.MTCapacityList = data;
            })
            .error(function () {
                $scope.MTCapacityList = [];
            });
    };

    $scope.SchemeList = [];
    $scope.GetSchemeList = function () {
        $http.get(SiteUrl + "/Godown/GetGodownPropsalSchemeByListId/?ListCode=GodownProposalTimeBound")
            .success(function (data, status) {
                $scope.SchemeList = data;
                console.log("$scope.SchemeList", $scope.SchemeList);
            })
            .error(function () {
                $scope.SchemeList = [];
            });
    };

    $scope.GodownDocList = [];
    $scope.GetGodownDocList = function () {
        $http.get(SiteUrl + "/Godown/GetNewLovValuesDocByListId/?ListCode=GodownProposalDocs")
            .success(function (data, status) {
                $scope.GodownDocList = data;
            })
            .error(function () {
                $scope.GodownDocList = [];
            });
    };

    $scope.GodownLandList = [];
    $scope.GetGodownLandList = function () {
        $http.get(SiteUrl + "/Godown/GetNewLovValuesDocByListId/?ListCode=ForGodownLandDetails")
            .success(function (data, status) {
                $scope.GodownLandList = data;
            })
            .error(function () {
                $scope.GodownLandList = [];
            });
    };

    $scope.IsFreeze = true;
    $scope.DashboardData = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Godown/CitizenDashboardDataWithDepartmentUser", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId),
            "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId),
            "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data.filter(x => x.IsObjection == false);
            $scope.SocietyDataList = $scope.TableData;
            $scope.SocietyList = $scope.SocietyDataList;
            $(".se-pre-con").fadeOut("slow");
            if ($scope.SocietyDataList.length == 1) {
                $scope.godown.SocietyID = $scope.SocietyDataList[0].SocietyID.toString();
                $scope.OnSocietyChange();
                $scope.IsSocietyDisabled = true;
            } else {
                $scope.godown.SocietyID = "";
                $scope.IsSocietyDisabled = false;
                $scope.godown.ApplicantName = "";
                $scope.district_MemberAdd = "";
                $scope.ddlblocki_MemberAdd = "";
                $scope.ddlGP_MemberAdd = "";
                $scope.ddlVillage_MemberAdd = "";
            }

        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };


    $scope.OnSocietyChange = function () {
        $scope.district_MemberAdd = "";
        $scope.ddlblocki_MemberAdd = "";
        $scope.ddlGP_MemberAdd = "";
        $scope.ddlVillage_MemberAdd = "";

        if (!$scope.SocietyDataList || $scope.SocietyDataList.length == 0) {
            console.warn("SocietyDataList is empty");
            return;
        }

        var row = $scope.SocietyDataList.find(function (x) {
            return x.SocietyID.toString() === $scope.godown.SocietyID.toString();
        });

        if (!row) {
            console.warn("No matching society found for ID:", $scope.godown.SocietyID);
            return;
        }

        var missingFields = [];

        if (!row.DISTRICT_ID)
            missingFields.push("District");

        if (!row.BLOCK_ID)
            missingFields.push("Block");

        if (!row.GP_ID)
            missingFields.push("Gram Panchayat");

        if (!row.VILLAGE_ID)
            missingFields.push("Village");

        if (!row.PresidentName || row.PresidentName.trim() === "")
            missingFields.push("President Name");

        if (missingFields.length > 0) {

            alert(
                _language == 'en-us' ? "Before filling the form, please update Society Profile.\n\nMissing: " + missingFields.join(", ") : "फॉर्म भरने से पहले कृपया सोसायटी प्रोफाइल अपडेट करें।\n\nअनुपलब्ध विवरण: " + missingFields.join(", ")
            );

            $scope.godown.SocietyID = "";

            return;
        }

        $scope.godown.SocietyID = row.SocietyID.toString();
        $scope.godown.ApplicantName = row.PresidentName;
        $scope.UralUrban = "2";
        $scope.district_MemberAdd = row.DISTRICT_ID.toString();

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + row.DISTRICT_ID)
            .success(function (blockData) {
                $scope.BlockDataList = blockData;
                $scope.ddlblocki_MemberAdd = row.BLOCK_ID + '';
                if ($scope.district_MemberAdd != "" && $scope.district_MemberAdd != 0) {
                    $scope.getAssemblyByDistrict($scope.district_MemberAdd);
                }
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + row.BLOCK_ID)
                    .success(function (gpData) {
                        $scope.GramPancyatList = gpData;
                        $scope.ddlGP_MemberAdd = row.GP_ID.toString();
                        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + row.GP_ID)
                            .success(function (villageData) {
                                $scope.VillageList = villageData;
                                $scope.ddlVillage_MemberAdd = row.VILLAGE_ID.toString();
                            })
                            .error(function () {
                                $scope.VillageList = [];
                            });
                    })
                    .error(function () {
                        $scope.GramPancyatList = [];
                    });
            })
            .error(function () {
                $scope.BlockDataList = [];
            });
    };

    //PWD Document Upload
    $scope.SelectPWDReceiptFile = function (files) {

        if (files && files.length > 0) {

            if (!$scope.godown) {
                $scope.godown = {};
            }

            if (!$scope.godown.PWDCertificateFilePath) {
                $scope.godown.PWDCertificateFilePath = {};
            }

            $scope.godown.PWDCertificateFilePath = files[0].name;

            $scope.UploadPWDCertificateDocument(files);
        }
    };

    $scope.UploadPWDCertificateDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("PWDCertificateFilePath", file);

        $http.post(SiteUrl + "/Godown/UploadPWDDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.godown.PWDCertificateFilePath = r.fileName;
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

    // Related to Godown Document Upload
    $scope.SelectRelatedGodownReceiptFile = function (files) {
        if (files && files.length > 0) {

            if (!$scope.godown) {
                $scope.godown = {};
            }

            if (!$scope.godown.RelatedGodownDocumentFilePath) {
                $scope.godown.RelatedGodownDocumentFilePath = {};
            }

            $scope.godown.RelatedGodownDocumentFilePath = files[0].name;

            $scope.UploadRelatedGodownDocument(files);
        }
    };

    $scope.UploadRelatedGodownDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("RelatedGodownDocumentFilePath", file);

        $http.post(SiteUrl + "/Godown/UploadRelatedGodownDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.godown.RelatedGodownDocumentFilePath = r.fileName;
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
    $scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Godown/CitizenDashboardDataWithDepartmentUser", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId),
            "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId),
            "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data.filter(x => x.IsObjection == false);
            $scope.SocietyDataList = $scope.TableData;
            $scope.SocietyList = $scope.SocietyDataList;
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");

        const urlParams = new URLSearchParams(window.location.search);
        $scope.GodownApplicationEncId = urlParams.get('eId');

        $scope.mode = urlParams.get('mode');
        $scope.isViewMode = ($scope.mode === 'v');

        if ($scope.GodownApplicationEncId && $scope.GodownApplicationEncId !== "") {
            $scope.GetGodownFormEditData($scope.GodownApplicationEncId);
        }
        $scope.IsEditable = !$scope.isViewMode;

        if ($scope.isViewMode) {
            setTimeout(function () {
                $('input, select, textarea ,.datepicker, .ui-datepicker').prop('disabled', true);
                $('input[type=submit], button[type=submit]').hide();
            }, 600);
        }

        $scope.GetMTCapacityList();
        $scope.GetGodownDocList();
        $scope.GetGodownLandList();
        $scope.GetSchemeList();
        $scope.FinancialYearList();
        $scope.GetSocietyBySSO();
    };

    $scope.getTranslation();

    $scope.OnPageLoad();

    $scope.SelectReceiptFile = function (files) {

        if (files && files.length > 0) {

            if (!$scope.godown) {
                $scope.godown = {};
            }

            if (!$scope.godown.DocumentPath) {
                $scope.godown.DocumentPath = {};
            }

            $scope.godown.DocumentPath = files[0].name;

            $scope.UploadLoanDocument(files);
        }
    };

    $scope.UploadLoanDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("DocumentPath", file);

        $http.post(SiteUrl + "/Godown/UploadGodownDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.godown.DocumentPath = r.fileName;
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

    $scope.GodownDocuments = [];

    $scope.GodownUploadDocument = function (files, docId) {
        debugger;
        if (!files || files.length == 0) return;

        let file = files[0];

        var extension = file.name.split('.').pop().toLowerCase();

        if (extension !== "pdf") {
            alert("Only PDF files are allowed.");
            input.value = "";
            return;
        }

        if (file.type && file.type !== "application/pdf") {
            alert("Only PDF files are allowed.");
            input.value = "";
            return;
        }

        let fd = new FormData();
        fd.append("DocumentPath", file);
        fd.append("DocId", docId);

        $http.post(SiteUrl + "/Godown/UploadGodownDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            if (response.data.success) {
                let existing = $scope.GodownDocuments.find(x => x.DocId == docId);
                if (existing) {
                    existing.FilePath = response.data.fileName;
                } else {
                    $scope.GodownDocuments.push({ DocId: parseInt(docId), FilePath: response.data.fileName });
                }
            } else {
                alert(response.data.message);
            }
        });
    };

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

    $scope.getUrban = function (id) {
        $(".Rurals_MemberAdd").show();
        $(".cityward_MemberAdd").hide();
        $(".pincode_MemberAdd").show();
        $scope.getblock();
    }

    $scope.getRural = function (id) {
        $(".Rurals_MemberAdd").hide();
        $(".cityward_MemberAdd").show();
        $(".pincode_MemberAdd").show();
        $scope.loadcity()
    }

    $scope.getward = function () {
        $scope.WardList = [];
        if ($("#ddlcity_MemberAdd").val() != "" && $("#ddlcity_MemberAdd").val() != undefined) {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $("#ddlcity_MemberAdd").val()).
                success(function (data, status) {
                    $scope.WardList = data;

                }).
                error(function (data, status) {
                    WardList = [];
                });
        }
    }

    $scope.getblock = function () {
        if ($scope.district_MemberAdd) {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.district_MemberAdd)
                .success(function (data, status) {
                    $scope.BlockDataList = data;
                })
                .error(function () {
                    $scope.BlockDataList = [];
                });
        }
    };

    $scope.getGrampanchyat = function (blockId) {
        var id = blockId || $scope.ddlblocki_MemberAdd;
        if (id) {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + id)
                .success(function (data, status) {
                    $scope.GramPancyatList = data;
                })
                .error(function () {
                    $scope.GramPancyatList = [];
                });
        }
    };

    $scope.getvillage = function (gpId) {
        var id = gpId || $scope.ddlGP_MemberAdd;
        if (id) {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + id)
                .success(function (data, status) {
                    $scope.VillageList = data;
                })
                .error(function () {
                    $scope.VillageList = [];
                });
        }
    };


    $scope.getAssemblyByDistrict = function (districtId) {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ASSEMBLY&MasterCode=" + districtId)
            .success(function (data) {
                $scope.assemblyDataList = data;
            })
            .error(function () {
                $scope.assemblyDataList = [];
            });
    };

});

Role.directive('onlyNumbers', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            function validate(value) {
                if (value == null) {
                    return value;
                }
                var cleanValue = value.toString().replace(/[^0-9]/g, '');
                if (value !== cleanValue) {
                    ngModelCtrl.$setViewValue(cleanValue);
                    ngModelCtrl.$render();
                }

                return cleanValue;
            }

            ngModelCtrl.$parsers.push(validate);
            element.on('keypress', function (event) {
                var charCode = event.which || event.keyCode;

                if (charCode < 48 || charCode > 57) {
                    event.preventDefault();
                }
            });
        }
    };
});
Role.directive('ifscCode', function () {
    return {
        restrict: 'A',
        require: 'ngModel',

        link: function (scope, element, attrs, ngModelCtrl) {

            element.on('input', function () {

                var value = element.val()
                    .toUpperCase()
                    .replace(/[^A-Z0-9]/g, '');

                value = value.substring(0, 11);

                if (value.length > 0) {
                    var firstFour = value.substring(0, 4)
                        .replace(/[^A-Z]/g, '');

                    value = firstFour + value.substring(4);
                }

                if (value.length >= 5) {
                    value = value.substring(0, 4) + '0' + value.substring(5);
                }

                scope.$applyAsync(function () {
                    ngModelCtrl.$setViewValue(value);
                    ngModelCtrl.$render();
                });
            });
        }
    };
});