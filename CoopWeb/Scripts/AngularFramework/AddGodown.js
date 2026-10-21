var Role = angular.module("UserRole", ['angularUtils.directives.dirPagination']);

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

Role.controller("Role", function ($scope, $http, $window) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    var Mdata = [];
    $scope.TblCHCDetails = [];
    $scope.TblDetails = [];
    $scope.godown = { isGSSKVSS: "1" };
    //Insert user role details in db
    $scope.UploadFile = function (file) {
        $scope.File = file[0];
    }


    $scope.TblCHCDetails.push({ EstablishmentDate: 0, Cost: 0, Instrument: 0, Price: 0, Manufacture: 0, RegistrationNo: 0 });


    // Update by vishnu swami Add More Row
    $scope.AddMoreRow = function () {
        $scope.TblCHCDetails.push({ EstablishmentDate: 0, Cost: 0, Instrument: 0, Price: 0, Manufacture: 0, RegistrationNo: 0 });
    };
    // Update by vishnu swami  Remove Row
    $scope.RemoveRow = function (index) {
        $scope.TblCHCDetails.splice(index, 1);
    };
    // Update by vishnu swami Add More Row
    $scope.TblDetails.push({ Year: 0, Number: 0 });
    $scope.AddRow = function () {
        $scope.TblDetails.push({ Year: 0, Number: 0 });
    };
    // Update by vishnu swami  Remove Row
    $scope.RemoveARow = function (index) {
        $scope.TblDetails.splice(index, 1);
    };

    var addressdataset1 = {
        Housebuilding: "",
        State: "6",
        District: " " + $("#hdnDistrictId").val() + " ",
        Lanestreet: "",// + ' ' + addressdata1[1],
        Locality: "",//+ ' ' + addressdata1[11],
        Pincode: ""
    }

    //GET Financial Year
    $scope.FinancialYearList = [];
    $scope.GetFinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //debugger;
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };

    $scope.GetFinancialYearList();


    $scope.InsertSave = function () {
        console.log($scope.TblGenDetails);
        if (!$scope.validatdata()) {
            return false;
        }
        else {
            //$scope.MemberAddress = GetAddressinfo('MemberAdd');
            var formData = new FormData();
            $scope.HouseNo = $("#HouseBuildingNo_MemberAdd").val();
            $scope.LaneStreetName = "";//$("#pincode_MemberAdd").val();
            $scope.Locality = ""; $("#pincode_MemberAdd").val();
            $scope.STATE_ID = "6";// $("#pincode_MemberAdd").val();
            $scope.DISTRICT_ID = $("#district_MemberAdd").val();
            $scope.CITY_ID = $("#ddlcity_MemberAdd").val();
            $scope.VILLAGE_ID = $("#ddlVillage_MemberAdd").val();
            $scope.BLOCK_ID = $("#ddlblocki_MemberAdd").val();
            $scope.GP_ID = $("#ddlGP_MemberAdd").val();
            $scope.WARD_ID = $("#ddlward_MemberAdd").val();
            $scope.PinCode = $("#pincode_MemberAdd").val();
            //AddKVssGss
            // $scope.KVSSGSSId = $scope.MemberAddress.KVSSGSSId;

            /*Added by Akshay Gupta on 29 July 2021 For Godown set Add*/

            formData.append("HouseNo", $scope.HouseNo);
            formData.append("LaneStreetName", $scope.LaneStreetName);
            formData.append("Locality", $scope.Locality);
            formData.append("STATE_ID", $scope.STATE_ID);
            formData.append("DISTRICT_ID", $scope.DISTRICT_ID);
            formData.append("CITY_ID", $scope.CITY_ID);
            formData.append("DIVISION_ID", $scope.DIVISION_ID);


            formData.append("VILLAGE_ID", $scope.VILLAGE_ID);
            formData.append("GP_ID", $scope.GP_ID);
            formData.append("BLOCK_ID", $scope.BLOCK_ID);


            formData.append("WARD_ID", $scope.WARD_ID);
            formData.append("PinCode", $scope.PinCode);


            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;
            if ($('#idddate').val() == '') {

                $('#idddate').val(dateTime);
            }
            if ($('#idddatetb').val() == '') {
                $('#idddatetb').val(dateTime);
            }

            /*Added by Akshay Gupta on 29 July 2021 For Godown set Add*/
            formData.append("Address", $scope.godown.Address);
            formData.append("SanctionAmt", $scope.godown.Amt);
            formData.append("SanctionDate", $('#idddate').val() == '' ? dateTime : $('#idddate').val());
            //formData.append("SanctionDate", $('#idddate').val());
            formData.append("SanctionDoc", $scope.File);
            formData.append("GodownCapacity", $scope.godown.Capacity);
            formData.append("LandType", $scope.godown.LandType);
            formData.append("LandSize", $scope.godown.LandSize);
            formData.append("Description", $scope.godown.Description);
            formData.append("ConstituencyId", $scope.ConstituencyId);
            formData.append("KVSSGSSId", $scope.godown.KVSSGSS);
            formData.append("SchemeName", $scope.godown.Scheme);
            formData.append("CCB", $scope.godown.CCB);
            formData.append("CurrentStatus", $scope.godown.CurrentStatus);
            formData.append("Usegodown", $scope.godown.Usegodown);
            formData.append("IsOldGodown", $scope.godown.isOldGodown == 'undefined' ? '0' : $scope.godown.isOldGodown);
            formData.append("ComplitionYear", $scope.godown.ComplitionYear == 'undefined' ? '0' : $scope.godown.ComplitionYear);



            $scope.TblDetails.forEach(function (i, j) {

                formData.append("TblDetails[" + j + "].Number", i.Number);
                formData.append("TblDetails[" + j + "].Year", i.Year);
            }


            );
            formData.append("TblCHCDetails[0].EstablishmentDate", $('#idddatetb').val() == '' ? dateTime : $('#idddatetb').val());
            formData.append("TblCHCDetails[0].Cost", $('#TotalCostidd').val());
            $scope.TblCHCDetails.forEach(function (r, v) {
                formData.append("TblCHCDetails[" + v + "].Price", r.Price);
                formData.append("TblCHCDetails[" + v + "].Manufacture", r.Manufacture);
                formData.append("TblCHCDetails[" + v + "].Instrument", r.Instrument);
                formData.append("TblCHCDetails[" + v + "].RegistrationNo", r.RegistrationNo);
            }
            );


            if ($scope.godown.haveLandarea == "1") {
                if (document.getElementById("godownPhoto1").files.length > 0) {
                    formData.append("file", document.getElementById("godownPhoto1").files[0]);
                }



            }
            if ($scope.godown.haveLandarea == "2") {

                formData.append("SanctionDate", "");

            }



            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveGodown", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (parseInt(data, 10) > 0) {
                        var Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                        alert(Msg);
                        window.location.reload();
                    }
                    else if (parseInt(data, 10) == -2) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
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
        }


    };

    $scope.pageChangeHandler = function (num) {
    };



    //start updation for godown list
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
            debugger;
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
    // START : Code for Language label setting
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


    $scope.GetStatusList = function () {
        $http.get(SiteUrl + "/society/GetGodownStatus").
            success(function (data, status) {
                $scope.StatusList = JSON.parse(data);
            }).
            error(function (data, status) {
                $scope.Level = [];
            });
    };


    $scope.GetStatusList();
    $scope.GetGodownlist = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/society/Godownlists/?Status=" + $scope.godown.status).
            success(function (data, status) {
                $scope.godown = data;

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

                    return '';
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


    $scope.GodownCCBlistsdetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "Master/GetMasterData/?MasterType=CCB_LIST&MasterCode=0").
            success(function (data, status) {
                $scope.CCBdataset = data;
                $(".se-pre-con").fadeOut("slow");

            }).

            error(function (data, status) {

                alert("Error while fetching CCB list.");
                $scope.showLoader = false;
            });
    };



    $scope.toggleAll = function () {
        var toggleStatus = $scope.isAllSelected;
        angular.forEach($scope.AllMenuList, function (itm) { itm.Checked = toggleStatus; });

    }

    $scope.optionToggled = function () {
        //$scope.isAllSelected = $scope.AllMenuList.every(function (itm) { return itm.Checked; })
    }


    //Clear user Role
    $scope.ClearRoleForm = function () {
        debugger;
        $scope.userrole = '';
        $scope.SingleChoise = false;
        $scope.userrole.LevelID = '0';


        $scope.HouseBuildingNo_MemberAdd = '';
        $scope.district_MemberAdd = '';
        $scope.ConstituencyId = '';
        $scope.ddlGP_MemberAdd = '';
        $scope.ddlVillage_MemberAdd = '';
        $scope.ddlcity_MemberAdd = '';

        $scope.godown.haveLandarea = '';
        $scope.godown.haveLandareaReamrk = '';
        $scope.godown.isGSSKVSS = '';
        $scope.godown.CCB = '0';
        $scope.godown.Amt = '';
        $scope.godown.Date = '';
        $scope.godown.Capacity = '';
        $scope.godown.LandSize = '';
        $scope.godown.Photo1 = '';
        $scope.godown.Description = '';
        $scope.godown.LandType = '';
        $scope.godown.Document = '';
        $scope.godown.isChcType = '';
        //  $scope.item.EstablishmentDate	='';	
        //$scope.item.Cost = '';

        $("#HouseBuildingNo_MemberAdd").val('');
        $("#district_memberadd").val('');
        $(" #ddlconstituency      ").val('');
        $(" #ddlgp_memberadd      ").val('');
        $(" #ddlvillage_memberadd ").val('');
        $(" #ddlcity_memberadd    ").val('');
        $("#ddlward_memberadd    ").val('');
        $("#pincode_memberadd    ").val('');
        $(" #totalcostidd         ").val('');
        $(" #idddatetb            ").val('');
        $(" #godownphoto1         ").val('');
        $(" #idddate              ").val('');
        $(" #ddlccb               ").val('');
        $(" #ddlgss               ").val('');
        $(" #ddldd                ").val('');



    };

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
        //SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
        //if ($("#hdnDistrictId").val() != "" && $("#hdnDistrictId").val() != "0") {
        //    $("#ddlDivision_MemberAdd").parent().hide();
        //    $("#district_MemberAdd").parent().hide();
        //    $("#ddlstate_MemberAdd").parent().hide();
        //    $("#district_MemberAdd").val($("#hdnDistrictId").val());

        //}
    };
    // START : Get Data for Update
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



    //umesh work changes
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
    //Vishnu work changes
    $scope.isChcyes = false;
    $scope.isChcno = false;
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
    //Vishnu work changes
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

    //$scope.GSSList = [];
    //$scope.GetGSSList = function () {
    //    debugger;
    //    // $(".blockUI").fadeIn("slow");
    //    $http.post(SiteUrl + "/FertilizerSupply/GetGSSORKVSSBySSOId", { TypeName: "GSS" }).
    //        success(function (data, status) {
    //            // $(".blockUI").fadeOut("slow");
    //            //$scope.GSSList = [];
    //            $scope.GSSList = data;

    //        }).
    //        error(function (data, status) {
    //            $scope.GSSList = [];
    //            //$(".blockUI").fadeOut("slow");
    //            alert('Error while fetching GSSList  data.');
    //        });
    //};

    $scope.showGSSKvss = false;
    $scope.GetKVSSList = function (id) {
        if (id == 1) {
            $scope.type = "GSS"
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

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");


    };
    //on page load end
    //Get list of all users on page list
    $scope.getTranslation();
    $scope.IsVisible = false;
    // $scope.RoleList('and 1=1');
    $scope.OnPageLoad();
    //$scope.GodownCCBlistsdetails();
    //$scope.LevelList();
    //$scope.MenuList();
    //$scope.SingleChoise = false;
    $scope.GetGodownlist(0);
    // $scope.GetGSSList();
    //$scope.GetKVSSList();
    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "") {
                    $("#ddrDistrictId").attr("disabled", "disabled");
                    $scope.DistrictId = $("#hdnDistrictId").val();
                }
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

    //$scope.DistrictChange = function () {
    //    $scope.GetConstituencyList();
    //    //$scope.GetKVSSList($scope.godown.isGSSKVSS);
    //    if ($scope.UralUrban == 1) {
    //        $scope.ddlblocki_MemberAdd = ""
    //       $scope.getblock(); 
    //    } else {
    //        $scope.ddlcity_MemberAdd = "";
    //        $scope.loadcity();
    //    }
    //}

    $scope.loadcity = function () {
        $scope.Citylist = [];
        if ($("#district_MemberAdd").val() != "" && $("#district_MemberAdd").val() != undefined) {
            //$scope.GetKVSSList($scope.godown.isGSSKVSS);
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $("#district_MemberAdd").val()).
                success(function (data, status) {
                    $scope.Citylist = data;
                    //$('#ddlcity_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');
                    //for (var i = 0; i < Citylist.length; i++) {

                    //    $('#ddlcity_MemberAdd').append('<option value="' + Citylist[i].CITY_ID + '">' + Citylist[i].CITY_ENG + '</option>');
                    //}
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    $scope.Citylist = [];
                });
        }
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



    $scope.getblock = function (disid) {

        $scope.BlockDataList = [];
        if ($("#district_MemberAdd").val() != "" && $("#district_MemberAdd").val() != undefined) {
            //$scope.GetKVSSList($scope.godown.isGSSKVSS);
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $("#district_MemberAdd").val()).
                success(function (data, status) {
                    $scope.BlockDataList = data;
                    //$('#ddlblocki_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');
                    //for (var i = 0; i < BlockDataList.length; i++) {

                    //    $('#ddlblocki_MemberAdd').append('<option value="' + BlockDataList[i].BLOCK_ID + '">' + BlockDataList[i].BLOCK_ENG + '</option>');
                    //    //BLOCK_MANGAL
                    //}
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    BlockDataList = [];
                });
        }
    }

    $scope.getGrampanchyat = function (divisionid) {
        $scope.GramPancyatList = [];
        if ($("#ddlblocki_MemberAdd").val() != "" && $("#ddlblocki_MemberAdd").val() != undefined) {
            //$scope.GetKVSSList($scope.godown.isGSSKVSS);
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $("#ddlblocki_MemberAdd").val()).
                success(function (data, status) {

                    $scope.GramPancyatList = data;
                    //$('#ddlGP_MemberAdd').append('<option value=""  class="ng-binding">--- Select ---</option>');

                    //for (var i = 0; i < GramPancyatList.length; i++) {

                    //    $('#ddlGP_MemberAdd').append('<option value="' + GramPancyatList[i].GP_ID + '">' + GramPancyatList[i].GP_ENG + '</option>');
                    //}//GP_MANGAL
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    GramPancyatList = [];

                });
        }
    }

    $scope.getvillage = function (pnchid) {
        //VillageList = [];
        $scope.VillageList = [];
        if ($("#ddlGP_MemberAdd").val() != "" && $("#ddlGP_MemberAdd").val() != undefined) {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $("#ddlGP_MemberAdd").val()).
                success(function (data, status) {
                    $scope.VillageList = data;
                    //$('#ddlVillage_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');

                    //for (var i = 0; i < VillageList.length; i++) {

                    //    $('#ddlVillage_MemberAdd').append('<option value="' + VillageList[i].VILLAGE_ID + '">' + VillageList[i].VILLAGE_ENG + '</option>');
                    //}//VILLAGE_MANGAL
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }
    }





});