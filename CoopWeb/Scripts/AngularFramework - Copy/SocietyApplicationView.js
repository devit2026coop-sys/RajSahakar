var CitizenModule = angular.module("CitizenMod", ['angularUtils.directives.dirPagination']);
var ImageDataList = [];
function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
    this.FileName = '';
}
var TableData10 = [];

var SportImageDataList = [];
CitizenModule.controller("CitizenCnt", function ($scope, $http) {
    // Global Setting
    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $scope.DivSms = true;
    $scope.divTabulation = true;
    $scope.divSocialProfiling = false;
    $scope.divSocietyProfilingShow = false;
    $scope.divAddBtn = true;
    $scope.Message = '';
    $scope.SocietyIdForAdministrativeDetails = 0;
    $scope.SocietyRegOfficeDetailsShow = 0;
    $scope.SocietyDirectory = '';
    $scope.ActId = 0;
    $scope.IsUpdate = 0;
    var ImageTempList = [];
    $scope.IsAdministrative = true;
    $scope.lstSocietyWorkScope = [];
    $scope.IsFirst = 1;
    $scope.WorkscopeDetailsData = [];
    $scope.UrbanForSportSociety = false;
    $scope.RuralForSportSociety = false;
    $scope.AddMoreForSportShow = false;
    $scope.PrimaryBodyDetails = false;
    $scope.PrimaryBodyName = false;
    $scope.ShowRegDateAndNoForRSSCAffiliationYes = false;
    $scope.AffiliatedByRSSC = false;

    $scope.SocietyIdEnc = '';
    $scope.SportsAttechments = false;
    $scope.IsNewRegistration = '';
    $scope.IsEditMode = false;
    $scope.PartialSave = 0;
    $scope.ShowPaymentSave = false;

    //start same address
    $scope.ElUploadAdministrative = function (file, row) {
        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = file[0];
        FileDataWithRowObj.RowNo = row;
        ImageDataList.push(FileDataWithRowObj);
    }

    $scope.SportElUploadAdministrative = function (file) {
        SportImageDataList.push(file[0]);
    }

    $scope.updateaddress = function () {

        $scope.SocietyRegisteredOffice.IsRuralcopy = $scope.SocietyRegisteredOffice.IsRural;
        $scope.RuralUrbanBlockCopy();
        if ($scope.SocietyRegisteredOffice.IsSameCEOAddress == false || $scope.SocietyRegisteredOffice.IsSameCEOAddress == 'False') {
            $scope.allDisable = false;
            $scope.SocietyRegisteredOffice.HouseBuildingNocopy = null;
            $scope.SocietyRegisteredOffice.LaneStreetNamecopy = null;
            $scope.SocietyRegisteredOffice.Localitycopy = null;
            $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
            $scope.SocietyRegisteredOffice.GP_ID_copy = '0';
            $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = '0';
            $scope.SocietyRegisteredOffice.CITY_IDcopy = '0';
            $scope.SocietyRegisteredOffice.WARD_IDcopy = '0';
            $scope.SocietyRegisteredOffice.PinCodecopy = null;
            $scope.SocietyRegisteredOffice.AreaOfOperationcopy = null;
            $scope.SocietyRegisteredOffice.CityTypes_copy = '0';
            $scope.SocietyRegisteredOffice.DIVISION_ID_copy = '0';
            $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = '0';
            $scope.SocietyRegisteredOffice.BLOCK_ID_copy = '0';
        }
        else if ($scope.SocietyRegisteredOffice.IsSameCEOAddress == true || $scope.SocietyRegisteredOffice.IsSameCEOAddress == 'True') {
            $scope.allDisable = true;
            $scope.SocietyRegisteredOffice.HouseBuildingNocopy = $scope.SocietyRegisteredOffice.HouseBuildingNo;
            $scope.SocietyRegisteredOffice.LaneStreetNamecopy = $scope.SocietyRegisteredOffice.LaneStreetName;
            $scope.SocietyRegisteredOffice.Localitycopy = $scope.SocietyRegisteredOffice.Locality;
            $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
            $scope.SocietyRegisteredOffice.SocietyIdcopy = $scope.SocietyRegisteredOffice.SocietyId;
            $scope.GramPanchayatscopy = $scope.GramPanchayats;
            $scope.SocietyRegisteredOffice.GP_IDcopy = $scope.SocietyRegisteredOffice.GP_ID;
            $scope.Villagescopy = $scope.Villages;
            $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = $scope.SocietyRegisteredOffice.VILLAGE_ID;
            $scope.CityListdatacopy = $scope.CityListdata;
            $scope.SocietyRegisteredOffice.CITY_IDcopy = $scope.SocietyRegisteredOffice.CITY_ID;
            $scope.Wardcopy = $scope.Ward;
            $scope.SocietyRegisteredOffice.WARD_IDcopy = $scope.SocietyRegisteredOffice.WARD_ID;
            $scope.SocietyRegisteredOffice.PinCodecopy = $scope.SocietyRegisteredOffice.PinCode;
            $scope.SocietyRegisteredOffice.AreaOfOperationcopy = $scope.SocietyRegisteredOffice.AreaOfOperation;
            $scope.SocietyRegisteredOffice.CityTypes_copy = $scope.SocietyRegisteredOffice.CityTypes;
            $scope.Divisionscopy = $scope.Divisions;
            $scope.SocietyRegisteredOffice.DIVISION_ID_copy = $scope.SocietyRegisteredOffice.DIVISION_ID;
            $scope.DistrictListcopy = $scope.DistrictList;
            $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = $scope.SocietyRegisteredOffice.DISTRICT_ID;
            $scope.Blocksdatacopy = $scope.Blocksdata;
            $scope.SocietyRegisteredOffice.BLOCK_ID_copy = $scope.SocietyRegisteredOffice.BLOCK_ID;
            if ($scope.SocietyRegisteredOffice.IsRuralAdd == 'true') {
                $scope.SocietyRegisteredOffice.IsRuralAddcopy = 'true';
                $scope.RuralUrbanBlock();
                $scope.RuralUrbanBlockCopy();
            }
            if ($scope.SocietyRegisteredOffice.IsRuralAdd == 'false') {
                $scope.SocietyRegisteredOffice.IsRuralAddcopy = 'false';
                $scope.RuralUrbanBlock();
                $scope.RuralUrbanBlockCopy();
            }
        }

    };

    $scope.Otpsend = function () {
        if ($('#SSOAuth_chkConsent').prop("checked")) {
            $scope.messagebox = false;
            $scope.FillOtp = true
            //SEND OTP
            $scope.GenrateOTP();
        }
        else {
            $scope.messagebox = true;
            $scope.FillOtp = false;
        }
    };
    $scope.ClosePopUp = function () {
        $scope.messagebox = false;

    };
    $scope.CloseOtpBox = function () {
        $scope.messagebox = false;
        $scope.OtpDilogBox = false;
        $scope.FillOtp = false;
        $('#ssoauthloading').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
    };

    //End Sports address
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
    // END : Code for Language label setting


    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetCurrentSocietyIDEnc();
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
        }
        $(".se-pre-con").fadeOut("slow");
    }



    // START Show Society Details
    $scope.ShowDetails = function (SID, IsFirst) {

        $scope.SocietyIdEnc = SID;
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = true;
        $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
            //$http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $scope.SocietyDetailsData = [];
                if (data.length > 0) {

                    debugger;
                    var BasicDetails = data[0];

                    var AdministrativeDetails = [];
                    var OfficeDeatils = data;
                    if (BasicDetails.IsNewRegistration == 'False' && BasicDetails.ActId == 3) {
                        $scope.TwoThousandOneProfilling = true;
                    }
                    else {
                        $scope.TwoThousandOneProfilling = false;
                    }
                    angular.forEach(data, function (item) {
                        AdministrativeDetails.push({
                            "Name": item.Name, "ContactNo": item.ContactNo, "Type": item.UserType, "SocietyId": item.SocietyId, "DOB": item.ContactNo, "DOB": item.ContactNo, "Photo": item.Photo, "AADHAARNo": item.AadharNo
                        });
                    });
                    if ($scope.EMitraTransactionId == 'undefined' && BasicDetails.IsPermanentSave == 0) {
                        //IsPermanentSave
                        $scope.ShowPaymentSave = true;

                    }
                    // alert(BasicDetails.MaxAppliFlowStatusID );
                    if (BasicDetails.MaxAppliFlowStatusID == 1 || BasicDetails.MaxAppliFlowStatusID == 5 || BasicDetails.MaxAppliFlowStatusID == 10 || BasicDetails.MaxAppliFlowStatusID == 20 || BasicDetails.MaxAppliFlowStatusID == 27) {
                        $scope.StatusAction = 1;
                    }
                    else if (BasicDetails.MaxAppliFlowStatusID == undefined || BasicDetails.MaxAppliFlowStatusID == 'undefined') {
                        $scope.StatusAction = 1;
                    }
                    else {
                        $scope.StatusAction = 0;
                    }

                    alert(BasicDetails.ResponseString);
                    $scope.SocietyDetailsData.BasicDetails = [];
                    if (BasicDetails.ResponseString != null && BasicDetails.ResponseString.length > 0 && BasicDetails.ResponseString != 'null' && BasicDetails.ResponseString != '') {
                        $scope.DivPayment = true;
                        //alert('{"' + BasicDetails.ResponseString.replace(/\~/g, '":"').replace(/\^/g , '","')+'"}');
                        $scope.SocietyDetailsData.PaymentDetails = JSON.parse(BasicDetails.ResponseString); //JSON.parse('{"' + BasicDetails.ResponseString.replace(/\~/g, '":"').replace(/\^/g, '","') + '"}');
                        var yyyy = $scope.SocietyDetailsData.PaymentDetails.EMITRATIMESTAMP.substring(0, 4);
                        var MM = $scope.SocietyDetailsData.PaymentDetails.EMITRATIMESTAMP.substring(4, 6);
                        var DD = $scope.SocietyDetailsData.PaymentDetails.EMITRATIMESTAMP.substring(6, 8);
                        var HH = $scope.SocietyDetailsData.PaymentDetails.EMITRATIMESTAMP.substring(8, 10);
                        var Mint = $scope.SocietyDetailsData.PaymentDetails.EMITRATIMESTAMP.substring(10, 12);
                        //alert(DD + '/' + MM + '/' + yyyy + '  ' + HH + ':' + Mint);
                        $scope.TransDataTime = (DD + '/' + MM + '/' + yyyy + '  ' + HH + ':' + Mint);
                    }
                    else {
                        $scope.DivPayment = false;
                        $scope.TransDataTime = '';
                        $scope.SocietyDetailsData.PaymentDetails = [];
                    }
                    // alert($scope.SocietyDetailsData.PaymentDetails.TRANSACTIONSTATUSCODE + '---' + $scope.SocietyDetailsData.PaymentDetails.RECEIPTNO + '--' + $scope.SocietyDetailsData.PaymentDetails.TRANSACTIONID);

                    $scope.SocietyDetailsData.BasicDetails = BasicDetails;
                    if ($scope.SocietyDetailsData.BasicDetails.BankPassBookFile != null && $scope.SocietyDetailsData.BasicDetails.BankPassBookFile != 'null' && $scope.SocietyDetailsData.BasicDetails.BankPassBookFile.length > 0 && $scope.SocietyDetailsData.BasicDetails.BankPassBookFile != '') {
                        $scope.showBankstatement = true;
                    }
                    else {
                        $scope.showBankstatement = false;
                    }
                    $scope.SocietyDetailsData.OfficeDeatils = OfficeDeatils;
                    if ($scope.SocietyDetailsData.BasicDetails.ActId != 7) {

                        ELECTEDMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        if (ELECTEDMEMBERS.length==0)
                        {
                            ELECTEDMEMBERS = data.filter(x => (x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        }
                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));

                        if (ALLMEMBERS.length == 0) {
                            ALLMEMBERS = data.filter(x => (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        }

                        if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                            NOMINATEDMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'NOMINATEDMEMBERS'));
                            if (NOMINATEDMEMBERS.length == 0)
                            {
                                NOMINATEDMEMBERS = data.filter(x => (x.MEMBERTYPE == 'NOMINATEDMEMBERS'));
                            }
                        }


                    }
                    else if ($scope.SocietyDetailsData.BasicDetails.ActId == 7) {

                        ELECTEDMEMBERS = data.filter(x => (x.AddressFor == '0') && (x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        if (ELECTEDMEMBERS.length == 0)
                        {
                            ELECTEDMEMBERS = data.filter(x => (x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        }

                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        if (ALLMEMBERS.length == 0)
                        {
                            ALLMEMBERS = data.filter(x => (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        }
                    }




                    var TableData10 = [];
                    var TableData10All = [];
                    var TableNominatedMembers = [];
                    if ($scope.SocietyDetailsData.BasicDetails.ActId != 7) {
                        if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                            var X = (NOMINATEDMEMBERS.length)
                            if (X > 0) {
                                for (var x = 0; x < X; x++) {
                                    alert(NOMINATEDMEMBERS[x].NominatedBy)
                                    TableNominatedMembers.push(
                                        {
                                            "Id": x,
                                            "Name": NOMINATEDMEMBERS[x].Name,
                                            "FatherName": NOMINATEDMEMBERS[x].FatherName,
                                            "POSTNAME": NOMINATEDMEMBERS[x].POSTNAME,
                                            "Aadh": NOMINATEDMEMBERS[x].AadharNo,
                                            "Gender": NOMINATEDMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                            "DOB": NOMINATEDMEMBERS[x].DOB,
                                            "ContactNo": NOMINATEDMEMBERS[x].ContactNo,
                                            "Photo": NOMINATEDMEMBERS[x].Photo,
                                            "CategoryName": NOMINATEDMEMBERS[x].Category,
                                            "NominatedBy": NOMINATEDMEMBERS[x].NominatedBy == '2' ? 'Nabard' : (NOMINATEDMEMBERS[x].NominatedBy == '1' ? 'Government' : (NOMINATEDMEMBERS[x].NominatedBy == '3' ? 'RBI' : 'Technical Director')),
                                            "MEMBERTYPE": NOMINATEDMEMBERS[x].MEMBERTYPE,
                                            "MembershipNo": NOMINATEDMEMBERS[x].MembershipNo,
                                            "ShareAllotted": NOMINATEDMEMBERS[x].ShareAllotted,
                                            "ShareCapitalAmount": NOMINATEDMEMBERS[x].ShareCapitalAmount,
                                            "loaneeType": ($scope._language == 'en-us' ? ALLMEMBERS[x].loaneeTypeEn : ALLMEMBERS[x].loaneeTypeHi)
                                        });
                                }
                                $scope.TableNominatedMembers = TableNominatedMembers;
                                $scope.DivNominated = true;
                            }
                            else
                                $scope.DivNominated = false;
                        }



                        var X = (ELECTEDMEMBERS.length)
                        for (var x = 0; x < X; x++) {
                            TableData10.push(
                                {
                                    "Id": x,
                                    "Name": ELECTEDMEMBERS[x].Name,
                                    "FatherName": ELECTEDMEMBERS[x].FatherName,
                                    "POSTNAME": ELECTEDMEMBERS[x].POSTNAME,
                                    "Aadh": ELECTEDMEMBERS[x].AadharNo,
                                    "Gender": ELECTEDMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                    "DOB": ELECTEDMEMBERS[x].DOB,
                                    "ContactNo": ELECTEDMEMBERS[x].ContactNo,
                                    "Photo": ELECTEDMEMBERS[x].Photo,
                                    "CategoryName": ELECTEDMEMBERS[x].Category,
                                    "Occupation": ELECTEDMEMBERS[x].Occupation,
                                    "MEMBERTYPE": ELECTEDMEMBERS[x].MEMBERTYPE,
                                    "MembershipNo": ELECTEDMEMBERS[x].MembershipNo,
                                    "ShareAllotted": ELECTEDMEMBERS[x].ShareAllotted,
                                    "ShareCapitalAmount": ELECTEDMEMBERS[x].ShareCapitalAmount,
                                    "loaneeType": ($scope._language == 'en-us' ? ELECTEDMEMBERS[x].loaneeTypeEn : ELECTEDMEMBERS[x].loaneeTypeHi)
                                });
                        }

                        $scope.TableData10 = TableData10;

                        var X = (ALLMEMBERS.length)
                        for (var x = 0; x < X; x++) {
                            TableData10All.push(
                                {
                                    "Id": x,
                                    "Name": ALLMEMBERS[x].Name,
                                    "FatherName": ALLMEMBERS[x].FatherName,
                                    "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                    "Aadh": ALLMEMBERS[x].AadharNo,
                                    "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                    "DOB": ALLMEMBERS[x].DOB,
                                    "ContactNo": ALLMEMBERS[x].ContactNo,
                                    "Photo": ALLMEMBERS[x].Photo,
                                    "CategoryName": ALLMEMBERS[x].Category,
                                    "Occupation": ALLMEMBERS[x].Occupation,
                                    "MEMBERTYPE": 'GENERALMEMBERS',
                                    "MembershipNo": ALLMEMBERS[x].MembershipNo,
                                    "ShareAllotted": ALLMEMBERS[x].ShareAllotted,
                                    "ShareCapitalAmount": ALLMEMBERS[x].ShareCapitalAmount,
                                    "loaneeType": ($scope._language == 'en-us' ? ALLMEMBERS[x].loaneeTypeEn : ALLMEMBERS[x].loaneeTypeHi)
                                });
                        }
                        $scope.TableData10All = TableData10All;
                    }

                    //SPORTS
                    if ($scope.SocietyDetailsData.BasicDetails.ActId == 7) {
                        var X = (ELECTEDMEMBERS.length);
                        for (var x = 0; x < X; x++) {
                            TableData10.push(
                                {
                                    "Id": x,
                                    "Name": ELECTEDMEMBERS[x].Name,
                                    "POSTNAME": ELECTEDMEMBERS[x].POSTNAME,
                                    "Aadh": ELECTEDMEMBERS[x].AadharNo,
                                    "Gender": ELECTEDMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                    "DOB": ELECTEDMEMBERS[x].DOB,
                                    "ContactNo": ELECTEDMEMBERS[x].ContactNo,
                                    "Photo": ELECTEDMEMBERS[x].Photo,
                                    "CategoryName": ELECTEDMEMBERS[x].Category,
                                    "Occupation": ELECTEDMEMBERS[x].Occupation,
                                    "MEMBERTYPE": ELECTEDMEMBERS[x].MEMBERTYPE
                                });
                        }
                        $scope.TableData10 = TableData10;

                        var X = (ALLMEMBERS.length)
                        for (var x = 0; x < X; x++) {
                            TableData10All.push(
                                {
                                    "Id": x,
                                    "Name": ALLMEMBERS[x].Name,
                                    "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                    "Aadh": ALLMEMBERS[x].AadharNo,
                                    "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                    "DOB": ALLMEMBERS[x].DOB,
                                    "ContactNo": ALLMEMBERS[x].ContactNo,
                                    "Photo": ALLMEMBERS[x].Photo,
                                    "CategoryName": ALLMEMBERS[x].Category,
                                    "Occupation": ALLMEMBERS[x].Occupation,
                                    "MEMBERTYPE": 'GENERALMEMBERS'
                                });
                        }
                        $scope.TableData10All = TableData10All;
                    }



                    if ($scope.SocietyDetailsData.BasicDetails.ActId == 1) {
                        $scope.Showforactidfirst = false;

                    }
                    else if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                        $scope.Showforactidfirst = true;
                    }

                    $scope.ActId = $scope.SocietyDetailsData.BasicDetails.ActId;
                    var newdab = data.filter(x => x.IsSameCEOAddress == 'True' && x.AddressFor == 0)

                    $scope.SocietyDetailsData.OfficeDeatils.dtab = newdab.length > 0 ? newdab[0] : {};



                    var newdab = data.filter(x => x.AddressFor == 9 && x.IsSameCEOAddress == 'False')

                    $scope.SocietyDetailsData.OfficeDeatils.dtabnew = newdab.length > 0 ? newdab[0] : {};

                    var sachiv = data.filter(x => x.AddressFor == 11)
                    $scope.SocietyDetailsData.OfficeDeatils.Newscetrat = sachiv.length > 0 ? sachiv[0] : {};

                    var Treasurer = data.filter(x => x.AddressFor == 12)
                    $scope.SocietyDetailsData.OfficeDeatils.newTreasurer = Treasurer.length > 0 ? Treasurer[0] : {};


                    var CEODetails = data.filter(x => x.UserType == 1);//CEO
                    $scope.SocietyDetailsData.CEO = CEODetails.length > 0 ? CEODetails[0] : [];
                    if ($scope.SocietyDetailsData.CEO.Name != 'undefined' || $scope.EMitraTransactionId > 0) {
                        $scope.BtnAdminDetailShowhide = true;
                    }
                    else {
                        $scope.BtnAdminDetailShowhide = false;
                    }

                    var ChairmanDetails = data.filter(x => x.UserType == 2);//Chairman
                    $scope.SocietyDetailsData.Chairman = ChairmanDetails.length > 0 ? ChairmanDetails[0] : [];

                    var AdministratorDetails = data.filter(x => x.UserType == 3);//Appointed Administrator
                    $scope.SocietyDetailsData.Administrator = AdministratorDetails.length > 0 ? AdministratorDetails[0] : [];

                    $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed = AdministratorDetails.length > 0 ? $scope.translation.Yes : $scope.translation.No;
                    $scope.IsAdministratorAppointed = AdministratorDetails.length > 0 ? true : false;

                    var MemberDetails = [];


                    //MemberDetails = data.filter(x => (x.UserType == 4 || x.UserType == 5) && x.IsSameCEOAddress == 'False');//Elected /Nomineted

                    //if (MemberDetails.length == 0) {
                    //    MemberDetails = data.filter(x => (x.UserType == 4 || x.UserType == 5));//Elected /Nomineted
                    //}

                    MemberDetails = data.filter(x => x.MEMBERTYPE == 'GENERALMEMBERS' && x.IsSameCEOAddress == 'True');
                    $scope.SocietyDetailsData.MemberDetails = MemberDetails;
                    if (MemberDetails.length > 0) {
                        $scope.hidemenber = true;
                    }
                    else {
                        $scope.hidemenber = false;
                    }
                    if (MemberDetails.length == 0) {
                        MemberDetails = data.filter(x => x.MEMBERTYPE == 'GENERALMEMBERS');//Elected /Nomineted
                    }
                    if ($scope.SocietyDetailsData.Administrator.IsAdministratorAppointed == 'हाँ' || $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed == 'Yes') {
                        $scope.IsAppointed = true;
                    }

                    //var MemberDetails = data.filter(x => (x.UserType == 4 || x.UserType == 5) && x.IsSameCEOAddress == 'False');//Elected /Nominete
                    if (MemberDetails.length == 0) {
                        MemberDetails = data.filter(x => (x.UserType == 4 || x.UserType == 5));//Elected /Nomineted
                    }

                    $scope.SocietyDetailsData.EMD = data.filter(x => (x.MEMBERTYPE == 'ELECTEDMEMBERS') && x.IsSameCEOAddress == 'False');

                    //start sport View
                    //var sportChairmanDetails = data.filter(x => x.UserType == 1 && x.ActId == 7);//CEO
                    //$scope.SocietyDetailsData.ChairmanSport = sportChairmanDetails.length > 0 ? sportChairmanDetails[0] : [];

                    //var SportsectrateDetails = data.filter(x => x.UserType == 2 && x.ActId == 7);//sport sec
                    //$scope.SocietyDetailsData.sectratesport = SportsectrateDetails.length > 0 ? SportsectrateDetails[0] : [];

                    //var SporttrasDetails = data.filter(x => x.UserType == 3 && x.ActId == 7);//sport sec
                    //$scope.SocietyDetailsData.SportTrasurrdtsDetails = SporttrasDetails.length > 0 ? SporttrasDetails[0] : [];

                    //var sportMemberDetails = data.filter(x => x.UserType == 4 && x.ActId == 7 && x.AddressFor == 0);//Elected /Nomineted
                    //$scope.SocietyDetailsData.sportMemberDetails = sportMemberDetails;

                    var sporttrasurerDetails = data.filter(x => x.UserType == 5 && x.ActId == 7 && x.AddressFor == 0);//Elected /Nomineted
                    $scope.SocietyDetailsData.sporttrasurerDetails = sporttrasurerDetails;

                    //End sport view
                    var liqDetails = data.filter(x => x.UserType == 6);//LIQvideter 
                    $scope.SocietyDetailsData.liq = liqDetails.length > 0 ? liqDetails[0] : [];
                    
                    if (liqDetails.length > 0) {
                        $scope.ShowLiquidator = true;
                        $scope.SubOtherDivShow = true;

                    } //LIQvideter 
                    else {
                        $scope.ShowLiquidator = false;
                        $scope.SubOtherDivShow = true;
                    }
                    //Bordmenber Hide
                    if (MemberDetails.length > 0) {
                        $scope.Hidemenbers = true;
                    }
                    else {
                        $scope.Hidemenbers = true;
                    }



                    var liqDetails = data.filter(x => x.UserType == 6);
                    $scope.SocietyDetailsData.liq = liqDetails.length > 0 ? liqDetails[0] : [];
                    if ($scope.SocietyDetailsData.BasicDetails.ISRDural == "Urban" || $scope.SocietyDetailsData.BasicDetails.IsRural == "0") {
                        $scope.divCity = true;
                        $scope.divBlock = false;
                        $scope.divWard = true;
                        $scope.divGP = false;
                        $scope.divVillage = false;
                        $scope.Showviewdat = false;
                        $scope.ShowviewdatU = true;
                    }
                    else if ($scope.SocietyDetailsData.BasicDetails.ISRDural == "Rural" || $scope.SocietyDetailsData.BasicDetails.IsRural == "1") {
                        $scope.divCity = false;
                        $scope.divBlock = true;
                        $scope.divWard = false;
                        $scope.divGP = true;
                        $scope.divVillage = true;
                        $scope.Showviewdat = true;
                        $scope.ShowviewdatU = false;
                    }

                }
                else {
                    $scope.SocietyDetailsData["BasicDetails"] = [];
                    $scope.SocietyDetailsData["AdministrativeDetails"] = [];
                }

                if ($scope.SocietyDetailsData.BasicDetails.ActId != "7") {
                    $scope.DivOther = true;
                    $scope.DivSport = false;
                }
                else {
                    $scope.DivOther = false;
                    $scope.DivSport = true;
                }



                if ($scope.SocietyDetailsData.BasicDetails.IsAffiliatedByDLSC == "Yes") {
                    $scope.IsRelationSportDLSC = true;
                }
                else {
                    $scope.IsRelationSportDLSC = false;
                }
                if ($scope.SocietyDetailsData.BasicDetails.IsAffiliatedBySOC == "Yes") {
                    $scope.IsRelationSportSOC = true;
                }
                else {
                    $scope.IsRelationSportSOC = false;

                }
                if ($scope.SocietyDetailsData.BasicDetails.IsAffiliatedByRSSC == "Yes") {
                    $scope.IsRelationSportRSSC = true;
                }
                else {
                    $scope.IsRelationSportRSSC = false;

                }


                if ($scope.SocietyDetailsData.BasicDetails.IsAffiliationBySLSA == "Yes") {
                    $scope.IsAffiliationBySLSA = true;
                }
                else {
                    $scope.IsAffiliationBySLSA = false;

                }

                if ($scope.SocietyDetailsData.BasicDetails.ISRDSSC == 'State Level') {

                    $scope.IsStatelevelF = false;
                    $scope.Caseofstate = true;
                    $scope.Caseofstatenot = false;

                }
                else {

                    $scope.IsStatelevelF = true;
                    $scope.Caseofstate = false;
                    $scope.Caseofstatenot = true;

                }

                if ($scope.SocietyDetailsData.BasicDetails.IsRural == "Urban" || $scope.SocietyDetailsData.BasicDetails.ISRDSSC == "District Level") {

                    $scope.IsStatelevelRural = true;
                    $scope.IsStatelevelRuralNO = false;
                    $scope.IsStatelevelF = false;
                }
                else if ($scope.SocietyDetailsData.BasicDetails.IsRural == "Rural" || $scope.SocietyDetailsData.BasicDetails.ISRDSSC == "District Level") {
                    $scope.IsStatelevelRuralNO = true;
                    $scope.IsStatelevelRural = false;
                    $scope.IsStatelevelF = false;

                }
                if ($scope.SocietyDetailsData.BasicDetails.IsAffiliatedByDLSC == "Yes" && $scope.SocietyDetailsData.BasicDetails.ISRDSSC == "District Level") {
                    $scope.caseState = true;

                }
                else {

                    $scope.caseState = false;
                }



                var select = 0;
                if (select == 1) {
                    $scope.newhide = true;
                }
                else {
                    $scope.newhide = false;
                }

                if (TableData10.length == 0)
                    $scope.DivOther22 = false;
                else
                    $scope.DivOther22 = true;

                if (TableData10All.length == 0)
                    $scope.DivMemberList = false;
                else
                    $scope.DivMemberList = true;

                $scope.IsPermanentSave = (data[0].IsPermanentSave);

                if ($scope.SocietyDetailsData.BasicDetails.ActId != "7") {
                    $scope.DivOther = true;
                    $scope.DivSport = false;
                    $scope.DivOther22 = true;
                    $scope.DivMemberList = true;
                    
                    
                }
                else {
                    $scope.DivOther = false;
                    $scope.DivSport = true;
                    $scope.DivOther22 = false;
                    $scope.DivMemberList = false;
                }

            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
            });

    }
    // END Show Society Details
    $scope.SubmitPermanentData = function () {
        $http.post(SiteUrl + "/Society/SavePermanentSocietyData", { "SocietyIDEnc": $scope.SocietyIDEnc }).
            success(function (data, status) {
                if (data > 0) {
                    alert("Data Save Sucessfully.");
                    location.reload();
                }

            }).
            error(function (data, status) {
                alert("Error While Permanent Data Save.");
            });
    }
    // END Show Society Details
    // START Show Workscope Details
    $scope.ShowDetailsWorkscope = function (SID, ValidationFlag) {
        $http.post(SiteUrl + "/Society/GetWorkscopeBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
            success(function (data, status) {
                $scope.WorkscopeDetailsData = [];
                if (data.length > 0) {
                    $scope.lstSocietyWorkScope = data;
                    $scope.singlearray = data[0];
                   
                    if ($scope.lstSocietyWorkScope.length > 0)                    {
                        
                        $scope.SocietyDetailsData.BasicDetails.DISTRICT_ENG = $scope.lstSocietyWorkScope[0].DISTRICT_ENG;
                    }
                    if ($scope.singlearray.IsRSSC == "True") {
                        $scope.TrueDivForPrimarybody = false;
                        $scope.hidefilediv = false;
                    }
                    else {
                        $scope.TrueDivForPrimarybody = true;
                        $scope.hidefilediv = true;
                    }
                    if ($scope.singlearray.IsStatLevel != 1 && ($scope.singlearray.IsRuralUrban == "0" || $scope.singlearray.IsRuralUrban == "False")) {

                        $scope.IsStatelevelRural = true;
                        $scope.IsStatelevelRuralNO = false;
                        $scope.IsStatelevelF = false;
                        $scope.IsRuralUworkscope = true;
                        $scope.IsUrbanRworkscope = false;
                    }
                    else if ($scope.singlearray.IsRuralUrban == "1" || $scope.singlearray.IsRuralUrban == "True") {
                        $scope.IsStatelevelRuralNO = false;
                        $scope.IsStatelevelRural = false;
                        $scope.IsStatelevelF = true;
                        $scope.IsRuralUworkscope = false;
                        $scope.IsUrbanRworkscope = true;
                    }

                    if ($scope.singlearray.IsStatLevel == '0') {
                        //$scope.SocietyRegistration.IsStatLevel = 'No';
                        $scope.singlearray.IsStatLevel = 'No';
                        //$scope.Addmorebtn(ValidationFlag);
                        $scope.deletebtnhide = false;
                        $scope.RuralurbanDiv = true;
                        $scope.grid = false;

                        if ($scope.singlearray.IsRuralUrban == '1') {
                            $scope.singlearray.Rural = 'true';
                            if ($scope.ActId == 7) {
                                $scope.IsStatelevelF = true;
                                $scope.IsStatelevelRural = false;
                            }
                            else {
                                $scope.IsStatelevelF = false;;
                            }
                        }
                        else {
                            $scope.SocietyRegistration.Rural = 'false';
                        }
                        $scope.grid = true;
                        $scope.InCaseofupdate = true;
                        //$scope.gridtwo = false;
                        // $scope.Addmorebtn();

                    }
                    else if ($scope.singlearray.IsStatLevel == '1' && $scope.SocietyRegistration != undefined) {
                        $scope.SocietyRegistration.IsStatLevel = 'Yes';
                    }



                    if ($scope.lstSocietyWorkScope.length > 0) {
                        $scope.DataYes = true;
                    }
                    else {
                        $scope.DataYes = false;
                    }
                }

                else {
                    $scope.SocietyDetailsData["WorkscopeDetailsData"] = [];

                }

            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");

            });

    }
    // END Show Workscope Details


    // START Back To Page
    $scope.BackToPage = function () {
        $scope.divTabulation = true;
        $scope.divAddBtn = true;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = false;

    }
    // START Back To Page
    //START




    function compare(a, b) {
        if (a.RowNo < b.RowNo)
            return -1;
        if (a.RowNo > b.RowNo)
            return 1;
        return 0;
    }

    $scope.ClearSocietyAdministrativeDetails = function () {
        $('input').val('');

        $scope.ImageDataList = [];
        $scope.SportImageDataList = [];
        $scope.SocietyRegistration.CategoryId1 = '0';

    }


    // START : Add New Registration
    $scope.AddNewSociety = function () {
        $scope.showrulesfile = false;
        $scope.ShowSubmitBtn = false;
        $scope.divOtherSocietys = false;
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = true;
        $scope.divSocietyProfilingShow = false;
        $scope.btnBasicDetails = true;
        $scope.btnAdministrativeDetails = true;
        $scope.btnSocietyOfficeDetails = true;
        $scope.lstSocietyWorkScope = [];
        $('#Iddstateyes')[0].checked = false;
        $('#Iddstateno')[0].checked = false;
        $('#ISrurl')[0].checked = false;
        $('#ISUrban')[0].checked = false;
        $scope.grid = false;
    }
    // END : Add New Registration
    //START



    $scope.showHideDiv = function () {
        $scope.SocietyRegistration.BRN = '';
        $scope.SportsAttechments = false;
        $scope.showrulesfile = false;
        $scope.ShowSubmitBtn = false;
        $scope.divOtherSocietys = false;
        $scope.nepster = true;
        $scope.CheckBRN = false;
        $scope.divOtherSocietys = false;
        $scope.GetSocietyTypeList();

        if ($scope.SocietyRegistration.ActId != 7) {
            TableData10 = [];
            var X = $scope.SocietyRegistration.ActId == 1 ? 8 : 11;
            for (var x = 1; x < X; x++) {
                TableData10.push(
                    {
                        "Id": x
                    });
            }
            $scope.TableData10 = TableData10;
            $scope.Act2001 = true;
            $scope.divOtherSocietys = true;
            $scope.divBrn = true;
            $scope.nepster = true;
            $scope.CheckBRN = false;
            $scope.divOtherSocietys = false;
            $scope.SportsAttechments = false;

            //by n saini
            $scope.gridtwo = false;
            $scope.AddMoreForSportShow = false;
            $scope.divSportsSocietys = false;
            $scope.AffiliatedByRSSC = false;
            $scope.AffiliationByDLSC = false;

            $scope.SportsAttechments = false;
            ///
            if ($scope.SocietyRegistration.ActId == 3) {
                $scope.Act2001 = true;
                $scope.showhideBylaws = true;
                $scope.showhideOther = false;
                $scope.showrulesfile = false;
                $scope.SportsAttechments = false;
                $scope.Act2001 = true;
                $scope.Act1958 = false;
                $scope.Leveldatachange(1);
                $scope.CheckBRN = false;
                $scope.SportsAttechments = false;
                $scope.SocietyRegistration.IsStatLevel = 'Yes';
                $('#Iddstateyes').attr("disabled", "disabled");
                $('#Iddstateno').attr("disabled", "disabled");
                $scope.Act1958 = false;

            }
            else if ($scope.SocietyRegistration.ActId == 1) {
                $scope.Act1958 = true;
                $scope.Act2001 = false;
                $scope.SportsAttechments = false;
                $scope.showrulesfile = false;
                $scope.showhideOther = false;
                $scope.Leveldatachange(2);
                $('#Iddstateyes').attr("disabled", "disabled");
                $('#Iddstateno').attr("disabled", "disabled");
                $('#Iddstateyes')[0].checked = false;
                $scope.CheckBRN = false;
                $scope.SportsAttechments = false;
                $('#Iddstateno')[0].checked = true;
                $scope.SocietyRegistration.IsStatLevel = 'No';
                //$('#Iddstateyes').removeAttr("disabled");
                //$('#Iddstateno').removeAttr("disabled");
            }
            else {
                $scope.Act2001 = false;
                $scope.showhideBylaws = false;
                $scope.showhideOther = true;
                $scope.showrulesfile = true;
                $scope.showrulesfile = false;
                $scope.CheckBRN = false;
                $scope.SportsAttechments = false;
                $('#Iddstateyes')[0].checked = false;
                $('#Iddstateno')[0].checked = false;
                $scope.SportsAttechments = false;
            }
        }
        else {
            TableData10 = [];
            var X = 1003;
            for (var x = 1001; x < X; x++) {
                TableData10.push(
                    {
                        "Id": x
                    });
            }
            $scope.TableSportsData2 = TableData10;
            $scope.Act2001 = false;
            $scope.nepster = true;
            $scope.CheckBRN = false;
            $scope.divOtherSocietys = true;
            $scope.gridtwo = false;
            $scope.AddMoreForSportShow = false;
            $scope.divBrn = false;
            $scope.divSportsSocietys = true;
            $scope.divOtherSocietys = false;
            $scope.showhideBylaws = false;
            $scope.showhideOther = false;
            $scope.divSocietyProfilingShow = false;
            $scope.UrbanForSportSocietyNew = false;
            $scope.showrulesfile = false;
            $scope.SportsAttechments = true;
            $scope.SportsAttechments = false;
            $scope.SocietyRegistration.IsStatLevel = undefined;
            $scope.SportsAttechments = false;
            $scope.LevelState = false
            $scope.IsAffiliatedBySOC = false;
            $scope.FileDLSC = false;
            $scope.AffiliationBySLSA = false;
        }
        //$('#Iddstateyes').removeAttr("disabled");
        //$('#Iddstateno').removeAttr("disabled");
        //$('#Iddstateyes')[0].checked = false;
        //$('#Iddstateno')[0].checked = false;
    }
    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural;
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };

    $scope.RuralUrbanBlockCopy = function () {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralcopy;

        if (SelectedVal == 'true' || SelectedVal == 'True') {
            $scope.UrbanDivCopy = true;
            $scope.RuralDivcopy = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = true;
        }
        else {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = false;
        }
    };

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlockForSport = function () {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralAddSports;
        if (SelectedVal == 'U') {
            $scope.UrbanDivSport = true;
            $scope.RuralDivSport = false;
        }
        else if (SelectedVal == 'R') {
            $scope.UrbanDivSport = false;
            $scope.RuralDivSport = true;
        }
        else {
            $scope.UrbanDivSport = false;
            $scope.RuralDivSport = false;

        }
    };

    $scope.RuralUrbanBlockForSportcopy = function () {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralSecretary;
        if (SelectedVal == 'true' || SelectedVal == 'True') {
            $scope.UrbanDivSportCopy = true;
            $scope.RuralDivSportcopy = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDivSportCopy = false;
            $scope.RuralDivSportcopy = true;
        }
        else {
            $scope.UrbanDivSportCopy = false;
            $scope.RuralDivSportcopy = false;
        }
    };
    $scope.IsTreasurer = function () {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralTreasurer;

        if (SelectedVal == 'true' || SelectedVal == 'True') {
            $scope.UrbanDivTreasurer = true;
            $scope.RuralDivTreasurer = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDivTreasurer = false;
            $scope.RuralDivTreasurer = true;
        }
        else {


            $scope.UrbanDivTreasurer = false;
            $scope.RuralDivTreasurer = false;

        }
    }

    //start Level of office data
    $scope.Levelofficedata = function () {
        $scope.showtabbar = true;
        $('#ddlDistrict').val(0);
        $('#ddlblocki').val(0);
        $('#ddlGP').val(0);
        $('#ddlVillage').val(0);
        $('#ddlDivision').val(0);
        if ($scope.SocietyRegistration.SocietyTypeId == 9) {
            $scope.SocietyRegistration.WorkScopeLevel_ID = 1;
            $scope.SocietyRegistration.IsStatLevel = 'Yes';
            $('#Iddstateyes').attr("disabled", "disabled");
            $('#Iddstateno').attr("disabled", "disabled");

        }

        var SelectedVal = $scope.SocietyRegistration.WorkScopeLevel_ID;
        $scope.GetSocietyClass();
        if (SelectedVal == '1') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.GPDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = false;
            $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = 0;
            $scope.SocietyRegistration.VILLAGE_ID = 0;
            $scope.SocietyRegistration.BLOCK_ID = 0;
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.District = [];
            $scope.SocietyRegistration.IsStatLevel == 'Yes'
            $('#Iddstateyes')[0].checked = true;
            //$scope.Addmorebtn();
        }
        else if (SelectedVal == '2') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = true;
            $scope.GPDiv = false;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = false;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $('#Iddstateyes')[0].checked = false;

            //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
            //    success(function (data, status) {
            //        $scope.Blocks = data;
            //    }).
            //    error(function (data, status) {
            //        $scope.Blocks = [];
            //    });
        }
        else if (SelectedVal == '3') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            //$scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = true;
            $scope.GPDiv = false;
            $scope.Districtlevel3 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $('#Iddstateyes')[0].checked = false;
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else if (SelectedVal == '4') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.BlockruralDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $('#Iddstateyes')[0].checked = false;
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else if (SelectedVal == '5') {
            $scope.showtabbar = true;
            $scope.BlockruralDiv = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            //$scope.StateDiv = true;
            $scope.Addmore = true;
            //$scope.ruralurbanb = false;
            //$scope.Districtlevel3 = true;
            //$scope.Districtlevel1 = false;
            //$scope.Districtlevel2 = false;
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $('#Iddstateyes')[0].checked = false;
            $scope.Blocks = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else if (SelectedVal == '6') {
            $scope.showtabbar = true;
            $scope.VillageDiv = true;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.BlockruralDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;

            $scope.Blocks = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else {
            $scope.StateDiv = false;
            $scope.DivisionDiv = false;
            $scope.DistrictDiv = false;
            $scope.TehsilDiv = false;
            $scope.PsDiv = false;
            $scope.VillageDiv = false;
            $scope.Addmore = false;
            $scope.ruralurbanb = false;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.CitytypDiv = false;
        }

        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.SocietyCategoryId = '0';

        //if ($scope.SocietyRegistration.SocietyTypeId == '9') {
        //    $scope.SocietyRegistration.IsStatLevel == 'Yes'
        //    $scope.Leveldatachange(1);
        //    $scope.Addmorebtn();
        //    $('#Iddstateyes').attr("disabled", "disabled");
        //    $('#Iddstateno').attr("disabled", "disabled");
        //    $('#Iddstateyes')[0].checked = true;
        //}
        //else {           
        //    $('#Iddstateyes')[0].checked = false;
        //    $scope.SocietyRegistration.IsStatLevel == 'No';
        //}
    };
    //End Level of office data

    $scope.Leveldatachange = function (item) {
        $scope.lstSocietyWorkScope = [];
        $scope.SocietyRegistration.Rural = null;
        $scope.SocietyRegistration.urban = null;

        if (item == '1') {
            $scope.RuralurbanDiv = false;
            $scope.RuralDivRadio = false;
            $scope.UrbanDivRadio = false;
            $scope.grid = false;
            $scope.Addmore = false;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
        }
        if (item == '2') {
            $scope.RuralurbanDiv = true;
            $scope.grid = false;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
        }

    }
    //end
    $scope.LeveldataonUrban = function (item) {

        $scope.lstSocietyWorkScope = [];
        if (item == '2') {

            $scope.UrbanDivRadio = true;
            $scope.RuralDivRadio = false;
            $scope.Addmore = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
            $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
            $scope.SocietyRegistration.DIVISION_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
        }
        else if (item == '1') {
            $scope.UrbanDivRadio = false;
            $scope.RuralDivRadio = true;
            $scope.Addmore = true;
            $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
            $scope.SocietyRegistration.CityTypes = '0';
            $scope.SocietyRegistration.CITY_ID = '0';
            $scope.SocietyRegistration.WARD_ID = '0';
        }

    }
    //start
    $scope.GetWorkscopedata = function (SocietyIdEnc) {
        $http.get(SiteUrl + "/Society/Getallworkscopedata/?SocietyId = " + SocietyIdEnc).
            success(function (data, status) {
                $scope.workscopedatashow = data;

                if (data != null && data.length == 0) {
                }
                else {
                }
            }).
            error(function (data, status) {
                alert('Error while auditor data list fetch.');
            });

    }
    //end
    $scope.GotoDairy = function () {
        if ($('#ChkMilkId').prop('checked')) {
            window.location = ' /Citizen/MilkProductionCooperativeSociety';
            $('#BtnClose').click();
        }
        else {
            $('#BtnClose').click();
            $scope.AddNewSociety();
        }

    }


    $scope.getfilter = function () {

        $http.post(SiteUrl + "/Citizen/CitizenDashboard", $scope.SocietyRegistration).

            success(function (data, status) {

                $scope.Socitydata = data;

            }).
            error(function (data, status) {
                $scope.Socitydata = [];
            });

    };

    $scope.getSocietyfilterBySocietyType = function () {

        $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);

    };

    $scope.getSocietyfilter = function () {

        $scope.GetSocietyTypeList();
        $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);

    };

    $scope.CategoryGenderFormValidation = function () {
        var GenderCount = 0;
        var STCount = 0;
        var SCCount = 0;
        var Message = '';
        var Returnval = false;
        if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
            $('#TblElectedMember tr').each(function () {
                GenderCount = GenderCount + ($(this).find('td:eq(5)').find('label').html() == "FEMALE" ? 1 : 0);
                SCCount = SCCount + (parseInt($(this).find('td:eq(6)').find('select').val()) == 2 ? 1 : 0)
                STCount = STCount + (parseInt($(this).find('td:eq(6)').find('select').val()) == 1 ? 1 : 0)
            });
        }
        if (GenderCount > 1 && SCCount > 0 && STCount > 0) {
            Returnval = true;
        }
        else if (GenderCount < 2 || SCCount < 1 || STCount < 1) {
            Message = $scope._language == 'en-us' ? 'नियमानुसार दो महिला और एक अनुसूचित जनजाति व एक अनुसूचित जाति के सदस्य होना अनिवर्य है क्या आप फिर भी डाटा सेव करना चाहते है!' : 'According to the rules, it is imperative to be two women and a scheduled tribe and a Scheduled Caste member. Do you want to save the data?'
            var r = confirm(Message);
            if (r == true) {
                Returnval = true;
            } else {
                Returnval = false;
            }
        }


        return Returnval;
    }

    //START : Get Sports Associations List
    $scope.GetSportsAssociationsList = function () {
        $http.get(SiteUrl + "/SportAssociation/SportAssociationList/?WhereClause=" + "").
            //$http.get(SiteUrl + "/Society/Getallworkscopedata/?SocietyId = " + SocietyIdEnc). 
            success(function (data, status) {
                $scope.SportAssociationList = data;
            }).
            error(function (data, status) {
                $scope.SportAssociationList = [];
            });
    };
    $scope.LevelOfSportsAcademy = function () {
        $scope.showtabbar = true;
        $scope.AddMoreForSportShow = false;
        $scope.SocietyRegistration.AssociationId = '0';
        $scope.SocietyRegistration.SocietyName = '';
        $scope.LevelLitle = '';
        $scope.SocietyRegistration.DISTRICT_ID = '0';
        $scope.SocietyRegistration.AssociationId = '0';
        $scope.SocietyRegistration.BLOCK_ID = '0';
        $scope.SocietyRegistration.GP_ID = '0';
        $scope.SocietyRegistration.VILLAGE_ID = '0';
        $scope.LevelDistricshow = false;
        $scope.primarydiv = true;
        $scope.lstSocietyWorkScope = [];
        $scope.SocietyRegistration.PrimaryBodyCEOName = '';
        $scope.SocietyRegistration.PrimaryBodyCEOMobileNo = '';
        $scope.SocietyRegistration.PrimaryBodyName = '';
        $scope.SportsAttechments = true;
        $scope.gridtwo = false;
        $scope.AddMoreForSportShow = false;
        $scope.FileSOC = true;
        $scope.SocietyRegistration.Ruraltwo = '';
        if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {
            $scope.AffiliationBySLSC = true;
            $scope.LevelDistricshow = true;
            $scope.primarydiv = false;
            $scope.LevelDistricsHide = false;
            $scope.SportSocietyCityType = false;
            $scope.UrbanForSportSocietyNew = false;
            $scope.UrbanForSportSociety = false;
            $scope.RuralurbanDiv = false;
            $scope.DistrictDiv = true;
            $scope.District = false;
            $scope.Districtt = true;
            $scope.RuralForSportSociety = false;
            $scope.PrimaryBodyDetails = true;
            $scope.PrimaryBodyName = false;
            $scope.GetDistrictWithoutDivisionList();
            $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the District Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् से अवगत कराये|');
            $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of State Olympic Council Affiliation ?' : 'राज्य ओलंपिक परिषद से संबद्धता की स्थिति चुने?');
            $scope.AffiliatedByRSSC = true;
            $scope.divSportSocietyName = true;
            $scope.AffiliationByDLSC = false;
            $scope.FileDLSC = false;
            $scope.FileRSSC = true;
            $scope.FileDOC = false;
            $scope.IsOlympicCouncilApproved = true;// $scope.SocietyRegistration.AssociationId.split('#')[1] > 0 ? true : false
            $scope.AffiliationBySLSA = false;

        }
        else {

            $scope.RuralurbanDiv = true;
            $scope.DistrictDiv = true;
            $scope.District = true;
            $scope.Districtt = false;
            $scope.GetDistrictWithoutDivisionList();
            $scope.UrbanForSportSociety = false;
            $scope.LevelDistricshow = false;
            $scope.LevelDistricsHide = true;
            $scope.PrimaryBodyName = true;
            $scope.primarydiv = true;
            //   $scope.lstSocietyWorkScope
            $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the Primary Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली प्राथमिक क्रीड़ा निकायों से अवगत कराये|');
            $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of District Olympic Council Affiliation ?' : 'जिला ओलंपिक परिषद से संबद्धता की स्थिति चुने?');


            $scope.divSportSocietyName = true;

            $scope.FileRSSC = true;
            //$scope.FileDLSC = true;

            $scope.FileDOC = true;

            $scope.FileDOCNo = 5;
            $scope.FileSLSANo = 5;
            $scope.FileDOCANo = 3;
            $scope.FileDLSCNo = 4;
            $scope.SportsAttechments = true;

            $scope.AffiliationByDLSC = true;
            $scope.AffiliatedByRSSC = false;
            $scope.AffiliationBySLSC = false;
            $scope.IsOlympicCouncilApproved = true;//$scope.SocietyRegistration.AssociationId.split('#')[1] > 0 ? true : false;
            $scope.AffiliationBySLSA = true;

        }
        $scope.GetSportsAssociationsList();
        $scope.ShowSubmitBtn = true;
    }
    //START : 
    $scope.ShowDivSportSocietyName = function () {
        var DNAME = '';
        if ($scope.SocietyRegistration.IsStatLevel == 'No') {
            DNAME = $scope.GetSportAssociationName();

        }
        else {

        }
        $scope.IsOlympicCouncilApproved = ($scope.SocietyRegistration.AssociationId.split('#')[1] > 0 ? true : false);
        $scope.IsAffiliatedBySOC = ($scope.SocietyRegistration.AssociationId.split('#')[1] > 0 ? true : false);
        $scope.SocietyRegistration.SLSAFileName = '';
    };
    //END
    $scope.GetSportAssociationName = function () {
        var DistrictName = '';
        angular.forEach($scope.DistrictList, function (item) {
            if ($scope.SocietyRegistration.DISTRICT_ID == item.DISTRICT_ID) {
                DistrictName = $scope._language == 'en-us' ? item.DISTRICT_ENG : item.DISTRICT_MANGAL;
            }
        });
        return DistrictName;
    };
    $scope.SportsRural = function () {
        //$scope.gridtwo = null;
        //$scope.lstSocietyWorkScope = [];
        $scope.AddMoreForSportShow = true;
        $scope.PrimaryBodyDetails = true;
        $scope.PrimaryBodyName = true;
        $scope.SocietyRegistration.PrimaryBodyCEOName = '';
        $scope.SocietyRegistration.PrimaryBodyCEOMobileNo = '';
        $scope.SocietyRegistration.PrimaryBodyName = '';
        // $scope.gridtwo = false;
        $scope.AddMoreForSportShow = true;
        if ($scope.SocietyRegistration.Ruraltwo == 'true') {
            $scope.RuralForSportSociety = true;
            $scope.UrbanForSportSocietyNew = false;
            $scope.RuralurbanDiv = true;
            $scope.SportSocietyCityType = false;
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';


        }
        else {
            $scope.SportSocietyCityType = true;
            $scope.RuralForSportSociety = false;
            $scope.UrbanForSportSocietyNew = true;
            $scope.SocietyRegistration.CityTypes = '0';
            $scope.SocietyRegistration.CITY_ID = '0';
            $scope.SocietyRegistration.WARD_ID = '0';
            //$scope.SportSocietyCityType = true;
        }
    }
    $scope.SopertGridelevel = function () {
        if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {

            $scope.LevelDistricshow = true;
        }
        if ($scope.SocietyRegistration.IsStatLevel == 'No') {

            $scope.LevelDistricshow = false;
        }
        else
            $scope.LevelDistricshow = false;
    }
    //START : 
    $scope.ShowRegDateAndNoForRSSCAffiliation = function (val) {
        if (val != 1) {
            $scope.SocietyRegistration.RSSCRegNo = '';
            $scope.SocietyRegistration.RSSCAffiliatedDate = '';
            $scope.RSSCFileName = '';
        }
        if ($scope.SocietyRegistration.IsAffiliatedByRSSC == "Yes") {
            $scope.ShowRegDateAndNoForRSSCAffiliationYes = true;
            $scope.FileRSSC = true;
        }
        else {
            $scope.ShowRegDateAndNoForRSSCAffiliationYes = false;
            $scope.FileRSSC = false;
        }
    };
    //END
    //START : 
    $scope.ShowRegDateAndNoForSOCAffiliation = function (val) {
        if (val != 1) {
            $scope.SocietyRegistration.SOCRegNo = '';
            $scope.SocietyRegistration.SOCAffiliatedDate = '';
            $scope.SOCFileName = '';
        }

        if ($scope.SocietyRegistration.IsAffiliatedBySOC == "Yes") {
            $scope.ShowRegDateAndNoForAffiliationBySOCYes = true;
            $scope.IsAffiliatedBySOC = true;
            if ($scope.SocietyRegistration.IsAffiliatedByDLSC == "Yes")
                $scope.FileDOCANo = 4;
            else
                $scope.FileDOCANo = 3;
        }
        else {
            $scope.ShowRegDateAndNoForAffiliationBySOCYes = false;
            $scope.IsAffiliatedBySOC = false;
            $scope.FileDOCANo = 3;
            $scope.FileSLSANo = 4;
        }

        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'No')
            $scope.FileSLSANo = 4;
        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes')
            $scope.FileSLSANo = 5;
        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'No' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'No')
            $scope.FileSLSANo = 3;
        if ($scope.SocietyRegistration.IsAffiliationBySLSA == 'No' && $scope.SocietyRegistration.IsAffiliationBySLSA == 'No')
            $scope.FileSLSANo = 6;
    };
    //END
    //START : 
    $scope.ShowRegDateAndNoForSLSAffiliation = function (val) {
        if (val != 1) {
            $scope.SocietyRegistration.SLSARegNo = '';
            $scope.SocietyRegistration.SLSAAffiliatedDate = '';
            $scope.SLSAFileName = '';
        }

        if ($scope.SocietyRegistration.IsAffiliationBySLSA == "Yes") {
            $scope.ShowRegDateAndNoForSLSAAffiliationYes = true;
            $scope.FileSLSA = true;
        }
        else {
            $scope.ShowRegDateAndNoForSLSAAffiliationYes = false;
            $scope.FileSLSA = false;
        }

        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes')
            $scope.FileSLSANo = 5;
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'No' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'No')
            $scope.FileSLSANo = 3;
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == undefined)
            $scope.FileSLSANo = 4;
        if ($scope.SocietyRegistration.IsAffiliationBySLSA == 'No' && $scope.SocietyRegistration.IsAffiliationBySLSA == 'No')
            $scope.FileSLSANo = 6;

    };
    //END
    //START :
    $scope.ShowRegDateAndNoForDLSCffiliation = function () {
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == "Yes") {
            $scope.AffiliationByDLSCYes = true;
            $scope.FileDLSC = true;
            $scope.FileDLSCNo = 3;
            $scopeFileDOCANo = 4;
        }
        else {
            $scope.AffiliationByDLSCYes = false;
            $scope.FileDLSC = false;
        }
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'No' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'No')
            $scope.FileSLSANo = 3;
        else if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes')
            $scope.FileSLSANo = 5;



    };
    //END
    //START :
    $scope.ShowRegDateAndNoForSLSCffiliation = function () {
        if ($scope.SocietyRegistration.IsAffiliatedBySLSC == "Yes") {
            $scope.AffiliationBySLSCYes = true;
            $scope.LevelState = true;
            $scope.FileSLSCNo = 3;
            $scope.FileDOCANo = 4;
        }
        else {
            $scope.AffiliationBySLSCYes = false;
            $scope.LevelState = false;
            $scope.FileDOCANo = 3;
        }
    };
    //END

    //START
    $scope.RemoveRowFromList = function (e) {
        var index = $scope.lstSocietyWorkScope.indexOf(e);
        $scope.lstSocietyWorkScope.splice(index, 1);
    }





    $scope.Profiling = function () {
        if ($scope.SocietyRegistration.IsSportsNewRegistration == 'true') {
            $scope.IsProfiling = false;
        }
        else {
            $scope.IsProfiling = true;
        }
    }
    $scope.GetCurrentSocietyIDEnc = function () {
        $http.get(SiteUrl + "/Society/GetCurrentSocietyIDEnc").
            success(function (data, status) {
                var res = data.split("~");
                if (res.length > 2) {
                    $scope.SocietyIDEnc = res[0];
                    $scope.BackUrl = res[1];
                    $scope.EMitraTransactionId = res[2];
                    $scope.ShowDetails($scope.SocietyIDEnc, res[2]);
                    $scope.ShowDetailsWorkscope($scope.SocietyIDEnc, res[2]);
                   

                }
                else if (res.length > 1) {
                    $scope.SocietyIDEnc = res[0];
                    $scope.BackUrl = res[1];
                    if ($scope.BackUrl.indexOf('Error^') > -1) {
                        var ErrorMsg = $scope.BackUrl.split("^");
                        if (ErrorMsg.length > 1) {
                            alert("Error :- " + ErrorMsg[1]);
                        }
                    }
                    if ($scope.BackUrl.indexOf("Success^") > -1) {
                        var ErrorMsg = $scope.BackUrl.split("^");
                        if (ErrorMsg.length > 1) {
                            alert(ErrorMsg[1]);
                        }
                    }
                    $scope.ShowDetails($scope.SocietyIDEnc);
                    $scope.ShowDetailsWorkscope($scope.SocietyIDEnc);
                }
            }).
            error(function (data, status) {
                $scope.SocietyIDEnc = '';
                $scope.BackUrl = '';
            });
    }


    $scope.ClickUserVarification = function () {
        $scope.ShowSubmitBtn = true;
        $scope.divOtherSocietys = true;
        $scope.IsNewRegistration = $scope.SocietyRegistration.IsNewRegistration;
        if ($scope.SocietyRegistration.IsNewRegistration == 'true') {
            $scope.IsProfiling = false;
            $scope.showtabbar = true;
            $scope.astrt = true;
            $scope.showrulesfile = true;

        }

        else {
            $scope.IsProfiling = true;
            $scope.showtabbar = true;
            $scope.astrt = true;
            $scope.showrulesfile = true;
        }
    }

    
    $scope.BackClipk = function () {   
        if ($scope.BackUrl =='_Citizen_SocietyProfile')       
            window.location = '../Citizen/SocietyProfile';
    }


    $scope.FinalSubmit = function (SocietyIdEnc) {
        //Message = $scope._language == 'en-us' ? 'आप इस प्रविष्टि के बाद अपने डेटा में परिवर्तन नहीं कर सकते| क्या आप फिर भी डाटा सेव करना चाहते है!' : 'You cannot edit your data after this submission. Do you want to save the data?'
        //if (confirm(Message)) {
        //    window.location = ' /Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_Citizen_SocietyProfile~' + $scope.EMitraTransactionId;
        //}
        
        window.location = ' /Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_Citizen_SocietyProfile~' + $scope.EMitraTransactionId;
    }
    $scope.FeePayMent = function (EMitraAppRequestID) {
        $http.post(SiteUrl + "/Emitra/PayServices", { AppRequestID: EMitraAppRequestID, ServiceID: "3343" }).
            success(function (data, status) {
                if (data > 0) {
                    alert('Final Submition has been done sucessfully');
                    window.location = "../Citizen/SocietyProfile";
                }
                else {
                    ShowModelAlert("Society/CEO Address Details", 'An Error Occurred Please Try Again Later');
                }
                //window.location = "/Citizen/CitizenDashboard";

            }).
            error(function (data, status) {
                $scope.SocietyRegisteredOffice = [];
            });

    }
    $scope.PaymentInitiatByCityzen = function (Registration) {
        $http.post(SiteUrl + "/Citizen/FeePaymentByCitizen", { "RegistrationNo": Registration }).
            success(function (data, status) {

            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }

    $scope.NotNullBRN = function () {
        if ($scope.SocietyRegistration.BRN != 0) {
            $scope.ShowValidateBtn = false;
        }
        else {
            $scope.ShowValidateBtn = true;
        }
    }



    $scope.BrnDetails = function () {

        $("input[name='SocietyRegistration']")[0].checked = false;
        $("input[name='SocietyRegistration']")[1].checked = false;
        var formData = new FormData();
        formData.append("BRN", $scope.SocietyRegistration.BRN);
        $http.post(SiteUrl + "/Society/GetBrnDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.result == 'success') {
                    $scope.CheckBRN = true;
                    $scope.SocietyRegistration.SocietyName = data.BRN;
                    $scope.nepster = true;
                    $scope.showtabbar = true;
                }
                else if (data.result == 'failed') {
                    ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
                }
            }).
            error(function (data, status) {
                $scope.CheckBRN = false;
                $scope.nepster = true;
                $scope.showtabbar = true;
                ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
            });
    };


    $scope.ShowRegFormContent = function () {
        $scope.showtabbar = false;

    }

    $scope.BankStmentBtn = function (SID) {
        window.location = ' /Society/BankStatement?SocietyIdENC=' + SID;
    }


    $scope.HidesportCouncil = function () {

        $scope.sportCouncil = false;

    }

    $scope.OnPageLoad();

});



