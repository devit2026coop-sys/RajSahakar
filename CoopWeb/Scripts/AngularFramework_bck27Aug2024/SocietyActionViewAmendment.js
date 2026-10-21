var CitizenModule = angular.module("SocietyActionViewMod", ['angularUtils.directives.dirPagination']);
var ImageDataList = [];
function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
}


var SportImageDataList = [];
CitizenModule.controller("SocietyActionViewCnt", function ($scope, $http) {
    // Global Setting

    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $scope.SocietyIDEnc = '';
    $scope.BackUrl = '';
    $scope.EMitraTransactionId = '';
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.ShowOTPDiv = false;
    $scope.showSocietyReportFile = false;
    $scope.ObjectionFlagBasic = false;
    $scope.ObjectionFlagAdmin = false;
    $scope.ObjectionFlagOffice = false;
    $scope.RoleId = 0;
    $scope.CurrentStatusId = 0;

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "../Translations/Translation_HI.json").
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
        $scope.divOtherSocietys = true;
        $scope.divBrn = true;
        $scope.GetCurrentSocietyIDEnc();
        $scope.GetCurrentUserId();





        $scope._language = _language; // Set LAnguiage
        if (_language == 'en-us') {
            $http.get(SiteUrl + "Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $(".se-pre-con").fadeOut("slow");


    };
    // END : Call default functions on page 
    // START Show Society Details
    $scope.ShowDetails = function (SID, IsFirst) {

        $scope.SocietyIdEnc = SID;
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = true;
        $http.post(SiteUrl + "/ApplicationFlow/GetSocietyDetailsBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
            //$http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $scope.SocietyDetailsData = [];
                if (data.length > 0) {

                    var BasicDetails = data[0];
                    $scope.IsSocietyAmendment = data[0].IsAmendment;
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


                    $scope.SocietyDetailsData.BasicDetails = [];
                    if (BasicDetails.ResponseString != null && BasicDetails.ResponseString.length > 0 && BasicDetails.ResponseString != 'null' && BasicDetails.ResponseString != '') {
                        $scope.DivPayment = true;
                        //alert('{"' + BasicDetails.ResponseString.replace(/\~/g, '":"').replace(/\^/g , '","')+'"}');
                        $scope.SocietyDetailsData.PaymentDetails = JSON.parse('{"' + BasicDetails.ResponseString.replace(/\~/g, '":"').replace(/\^/g, '","') + '"}');
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
                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                            NOMINATEDMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'NOMINATEDMEMBERS'));
                        }


                    }
                    else if ($scope.SocietyDetailsData.BasicDetails.ActId == 7) {

                        ELECTEDMEMBERS = data.filter(x => (x.AddressFor == '0') && (x.MEMBERTYPE == 'ELECTEDMEMBERS'));

                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                    }




                    var TableData10 = [];
                    var TableData10All = [];
                    var TableNominatedMembers = [];
                    if ($scope.SocietyDetailsData.BasicDetails.ActId != 7) {
                        if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                            var X = (NOMINATEDMEMBERS.length)
                            if (X > 0) {
                                for (var x = 0; x < X; x++) {
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
                                            "loaneeType": ($scope._language == 'en-us' ? ALLMEMBERS[x].loaneeTypeEn : ALLMEMBERS[x].loaneeTypeHi),
                                            "Address": NOMINATEDMEMBERS[x].Address
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
                                    "loaneeType": ($scope._language == 'en-us' ? ELECTEDMEMBERS[x].loaneeTypeEn : ELECTEDMEMBERS[x].loaneeTypeHi),
                                    "Address": ELECTEDMEMBERS[x].Address
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
                                    "loaneeType": ($scope._language == 'en-us' ? ALLMEMBERS[x].loaneeTypeEn : ALLMEMBERS[x].loaneeTypeHi),
                                    "Address": ALLMEMBERS[x].Address
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

            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
            });

    };
    // END Show Society Details

    // START Show Workscope Details
    $scope.ShowDetailsWorkscope = function (SID, ValidationFlag) {
        $http.post(SiteUrl + "/Society/GetWorkscopeBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
            success(function (data, status) {
                $scope.WorkscopeDetailsData = [];
                if (data.length > 0) {
                    $scope.lstSocietyWorkScope = data;
                    $scope.singlearray = data[0];

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
                        $scope.SocietyRegistration.IsStatLevel = 'No';
                        //$scope.Addmorebtn(ValidationFlag);
                        $scope.deletebtnhide = false;
                        $scope.RuralurbanDiv = true;
                        $scope.grid = false;

                        if ($scope.singlearray.IsRuralUrban == '1') {
                            $scope.SocietyRegistration.Rural = 'true';
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

    };
    // END Show Workscope Details

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
                    $scope.update(res[0]);

                }
                else if (res.length > 1) {
                    $scope.SocietyIDEnc = res[0];
                    $scope.BackUrl = res[1];
                    $scope.ShowDetails($scope.SocietyIDEnc);
                    $scope.ShowDetailsWorkscope($scope.SocietyIDEnc);
                    $scope.update(res[0]);
                }
            }).
            error(function (data, status) {
                $scope.SocietyIDEnc = '';
                $scope.BackUrl = '';
            });
    }




    $scope.SaveSocietyApplicationFlowStatus = function () {        
        if ($scope.ApplicationFlowMaster.StatusId != 0 && $scope.ApplicationFlowMaster.Remark != "") {
            if ($scope.ApplicationFlowMaster.StatusId != 17 && ($scope.ApplicationFlowMaster.StatusId != 14)) {
                if ($scope.ApplicationFlowMaster.StatusId == 5 || $scope.ApplicationFlowMaster.StatusId == 10 || $scope.ApplicationFlowMaster.StatusId == 20) {
                    if ($scope.ObjectionFlagBasic == false && $scope.ObjectionFlagAdmin == false && $scope.ObjectionFlagOffice == false) {
                        alert(_language == 'en-us' ? 'Please Select Objection Belong To.\n' : 'कृप्या आपत्ति संबंधित है चुनें|\n');
                    }
                    else {
                        $(".se-pre-con").fadeIn("slow");
                        $http.post("../ApplicationFlow/SaveSocietyObjectionBelong", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "ObjectionFlagBasic": $scope.ObjectionFlagBasic, "ObjectionFlagAdmin": $scope.ObjectionFlagAdmin, "ObjectionFlagOffice": $scope.ObjectionFlagOffice }).
                            success(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                $scope.saveDataInDB();
                            }).
                            error(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                            });
                    }
                }
                else {
                    $scope.saveDataInDB();
                }
            }
            else {
                // $scope.saveDataInDB();
                // $http.get("../ApplicationFlow/GenerateAadhaarOTP").
                $(".se-pre-con").fadeIn("slow");
                $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data.indexOf('NO#') >= 0) {
                            alert("Due to some issue OTP not send");
                           // location.reload();
                            //window.location = '../ApplicationFlow/ApplicationFlow';
                        } else {
                            if (data != "LOGOUT") {
                                //alert(data);
                                $scope.ApplicationFlowMaster.TXN = data;
                                alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                                $scope.ShowOTPDiv = true;
                            }
                            else {
                                window.location = 'https://sso.rajasthan.gov.in/signin';
                            }
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                    });
            }
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Status and Remark\n' : 'कृपया स्थिति और टिप्पणी जोड़ें|\n');
        }
    };

    $scope.ClearAppFlowForm = function () {
        $scope.ApplicationFlowMaster.Remark = '';
        $scope.ApplicationFlowMaster.StatusId = '0';
        $scope.ApplicationFlowMaster.OfficeId = '0';
        $scope.ReportSocietyFile = '';
        $scope.ShowOTPDiv = false;      
        window.location = '/ApplicationFlow/ApplicationFlow';
    }
    $scope.UploadReportFile = function (file) {
        $scope.ReportSocietyFile = file[0];
    }
    $scope.UploadeReportFileInDB = function () {
        var formData = new FormData();
        formData.append("SocietyReportFile", $scope.ReportSocietyFile);
        formData.append("SocietyIDEnc", $scope.ApplicationFlowMaster.SocietyIDEnc);
        $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
            }).
            error(function (data, status) {
            });
    }
    $scope.saveDataInDB = function () {
        $scope.showLoader = true;
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            if ($scope.ApplicationFlowMaster.OfficeId == '0') {
                alert("Please Select Officer.")
                return;
            }
            else {
                $scope.ApplicationFlowMaster.UserId = $scope.ApplicationFlowMaster.OfficeId;
            }
        }
        if ($scope.ReportSocietyFile == undefined && $scope.ApplicationFlowMaster.StatusId == 36) {
            alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');
            return;
        }
        else if ($scope.ReportSocietyFile != undefined && $scope.ApplicationFlowMaster.StatusId == 36) {
            $scope.UploadeReportFileInDB();
        }
       
        if ($scope.ApplicationFlowMaster.StatusId == 13 || $scope.ApplicationFlowMaster.StatusId == 21 || $scope.ApplicationFlowMaster.StatusId == 6) {       
            ShowAlertMessage("Warning", 'en-us' ? '\n  Are you sure you want to reject this society application ?' : '\n  क्या आप वाकई इस सोसायटी एप्लिकेशन को अस्वीकार करना चाहते हैं?', function () {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == 1) {
                            if ($scope.ApplicationFlowMaster.StatusId != 17) {
                                ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                            }
                            else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                                $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                            }
                            else {
                                ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                            }
                        }
                        else if (data == -2) {
                            ShowOkMessage("", "SECRETARY dose not exist in member list", function () { });
                        } else {
                            $(".se-pre-con").fadeOut("slow");
                            if ($scope.ApplicationFlowMaster.StatusId == 14 && data.toString() == "Invalid OTP!") {
                                ShowOkMessage("", data.toString(), function () { });
                            } else {
                                ShowOkMessage("", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n', function () { });
                            }
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert("Error while insert");
                    });

            });          
        } else {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == 1) {
                        if ($scope.ApplicationFlowMaster.StatusId != 17) {
                            ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                        }
                        else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                            $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                        }
                        else {
                            ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                        }
                    }
                    else if (data == -2) {
                        ShowOkMessage("", "SECRETARY dose not exist in member list", function () { });
                    } else {
                        $(".se-pre-con").fadeOut("slow");
                        if ($scope.ApplicationFlowMaster.StatusId == 14 && data.toString() == "Invalid OTP!") {
                            ShowOkMessage("", data.toString(), function () { });
                        } else {
                            ShowOkMessage("", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n', function () { });
                        }
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while insert");
                });
        }
        $scope.showLoader = false;
    };

    //$scope.saveDataInDB = function () {
    //    $scope.showLoader = true;
    //    $http.post("../ApplicationFlow/SaveSocietyApplicationFlowStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
    //        success(function (data, status) {
    //            if (data == 1) {

    //                if ($scope.ApplicationFlowMaster.StatusId == 14) {
    //                    $scope.BankOpeningCertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
    //                }
    //                else if ($scope.ApplicationFlowMaster.StatusId != 17) {
    //                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
    //                    location.reload();
    //                    //window.location = '../ApplicationFlow/ApplicationFlow';
    //                }
    //                else if ($scope.ApplicationFlowMaster.StatusId == 17) {
    //                    $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
    //                }
    //                else {
    //                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
    //                    location.reload();
    //                    //window.location = '../ApplicationFlow/ApplicationFlow';
    //                }
    //                // else if ($scope.ApplicationFlowMaster.ActId == '3') {
    //                //    $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
    //                //}
    //                //else if ($scope.ApplicationFlowMaster.ActId == '7') {
    //                //    $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
    //                //}
    //                //else {
    //                //    window.location = '/ApplicationFlow/ApplicationFlow';
    //                //}
    //            }
    //            else {
    //                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
    //                location.reload();
    //                    //window.location = '/ApplicationFlow/ApplicationFlow';
    //            }
    //        }).
    //        error(function (data, status) {
    //            alert("Error while insert");
    //        });
    //    $scope.showLoader = false;
    //}

    $scope.CertificateGenerate = function (SocietyIDEnc) {

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/ApplicationFlow/CertificateGenerate/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                location.reload();
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                location.reload();
            });
    }
    $scope.BankOpeningCertificateGenerate = function (SocietyIDEnc) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/ApplicationFlow/BankOpeningCertificateGenerate/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                location.reload();
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                location.reload();
            });
    }
    $scope.VerifyOTP = function () {
        if ($scope.ApplicationFlowMaster.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            if ($scope.ApplicationFlowMaster.StatusId == 14) {
                $scope.saveDataInDB();
            } else {
                $http.post(SiteUrl + "/ApplicationFlow/VerifyAadhaarOTPForEsing", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "StatusId": $scope.ApplicationFlowMaster.StatusId, "Remark": $scope.ApplicationFlowMaster.Remark, "IsAmendment": $scope.IsSocietyAmendment}).
                    success(function (data, status) {

                        $(".se-pre-con").fadeOut("slow");
                        if (data == "Fail,-2") {
                            ShowOkMessage("", "SECRETARY dose not exist in member list", function () {
                                // location.reload();
                            });
                        } else
                            if (data == "Fail" || data == "0" || data == 0) {
                                ShowOkMessage("Error", "Certificate not Generate. There is some problem.Please try after some time or contact on support.", function () { });
                            } else
                                if (data != "Success") {
                                    alert(data);
                                }
                                else if (data == "Success") {
                                    alert('Application for Society Registration Approved');
                                    location.reload();
                                }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                        location.reload();
                    });
            }
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
    };

    // on page load

    $scope.GetCurrentUserId = function () {
        $http.get("../ApplicationFlow/GetCurrentUserId").
            success(function (data, status) {
                $scope.BaseUserId = data;
            }).
            error(function (data, status) {
                $scope.BaseUserId = '';
            });
    }

    $scope.SocietyProceesStatusList = function (StatusId, ActId, SocietyIDEnc) {
        $scope.SocietyProceesStatus = [];
        //  $http.get("/ApplicationFlow/SocietyProceesStatusList/?WhereClause= and IsActive=1 and SocietyProceesStatusIdParent=" + StatusId + " AND WorkOnActId=" + ActId).
        $http.post("../ApplicationFlow/SocietyProceesStatusList", { "StatusId": StatusId, "WorkOnActId": ActId, "SocietyIDEnc": SocietyIDEnc }).

            success(function (data, status) {
                $scope.SocietyProceesStatus = data;
            }).
            error(function (data, status) {
                $scope.SocietyProceesStatus = [];
            });
    };


    $scope.GetAppliFlowList = function () {
        $scope.showLoader = true;
        $http.get("../ApplicationFlow/GetSocietyAppliFlowList").
            success(function (data, status) {
                var radioValue = $("input[name='gender']:checked").val();
                if (radioValue == 'R') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 6 || x.StatusId == 13 || x.StatusId == 21 || x.StatusId == 16 || x.StatusId == 18 || x.StatusId == 34 || x.StatusId == 29); // Rejected(6,13,21) AutoClose(16,18,34,29)
                }
                else if (radioValue == 'A') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 17 || x.StatusId == 26); // Approved(17,26)
                }
                else if (radioValue == 'O') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 20 || x.StatusId == 27); // Objection(5,10,20,27)
                }
                else if (radioValue == 'PA') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId != 5 && x.StatusId != 10 && x.StatusId != 20 && x.StatusId != 27 && x.StatusId != 6 && x.StatusId != 13 && x.StatusId != 21 && x.StatusId != 16 && x.StatusId != 18 && x.StatusId != 34 && x.StatusId != 29 && x.StatusId != 17 && x.StatusId != 26); // Pending
                }
                else {
                    $scope.ApplicationFlowList = data.filter(x => x.UserId == $scope.BaseUserId); // Panding Self
                }
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

            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.GetApplicationFlowSociety = function (SocietyIDEnc) {
        $scope.showLoader = true;
        $http.get("../ApplicationFlow/GetApplicationFlowSociety/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $scope.ApplicationFlowSociety = data;
                // $scope.BaseUserId = data.filter(x => x.StatusId == 3)[0].UserId;
                //  alert(($scope.BaseUserId));
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

            });
        $scope.showLoader = true;
    }


    $scope.update = function (SocietyIDEnc) {
        $scope.showLoader = true;

        // $http.post("../AppicationView/GetSocietyDetailBySocietyIDEnc", { "SocietyIDEnc": SocietyIDEnc }).
        $http.post(SiteUrl + "../ApplicationFlow/GetSocietyDetailBySocietyIDEnc", { "SocietyIDEnc": SocietyIDEnc }).
            success(function (data, status) {
                $scope.GetApplicationFlowSociety(SocietyIDEnc);
                $scope.SocietyProceesStatusList(data[0].StatusId, data[0].ActId, $scope.SocietyIDEnc);
                $scope.CurrentStatusId = data[0].StatusId;
                $scope.ApplicationFlowMaster.SocietyIDEnc = SocietyIDEnc;
                $scope.ApplicationFlowMaster.ActId = data[0].ActId;
                $scope.ApplicationFlowMaster.SocietyName = data[0].SocietyName;
                $scope.ApplicationFlowMaster.RegistrationNo = data[0].RegistrationNo;
                $scope.ApplicationFlowMaster.UserId = data[0].UserId;

            }).
            error(function (data, status) {
                $scope.SocietyIDEnc = '';
                $scope.BackUrl = '';
            });
        $scope.showLoader = false;
    };

    $scope.GetRoleList = function () {
        $scope.RoleList = [];
        $scope.RoleId = '0';
        $scope.OfficerData = [];
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            $http.post(SiteUrl + "/ApplicationFlow/GetRoleListForApplicationStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "NextStatusId": $scope.ApplicationFlowMaster.StatusId, "CurrentStatusId": $scope.CurrentStatusId }).
                success(function (data, status) {
                    $scope.RoleList = data;
                    if (data.length > 0) {
                        $("#ddlRole").attr("disabled", 'disabled');
                        $scope.RoleId = data[0].RoleID;
                        $scope.GetOfficer();
                    }
                }).
                error(function (data, status) {
                    $scope.RoleList = [];
                });
        }
    };

    $scope.GetOfficer = function () {
        $scope.OfficerData = [];
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            $http.post(SiteUrl + "/ApplicationFlow/GetOfficerBySocietyIDEnc", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "NextStatusId": $scope.ApplicationFlowMaster.StatusId, "CurrentStatusId": $scope.CurrentStatusId, "RoleId": $scope.RoleId }).
                success(function (data, status) {
                    $scope.ApplicationFlowMaster.OfficeId = '0';
                    $scope.OfficerData = data;
                    if (data.length > 0) {
                        $("#ddlOffice").attr("disabled", 'disabled');
                        $scope.ApplicationFlowMaster.OfficeId = data[0].userid;
                    }
                }).
                error(function (data, status) {
                    $scope.OfficerData = [];
                });
        }
    };

    $scope.PaymentProcess = function () {
        $http.get(SiteUrl + "/Emitra/PayServices/?AppRequestID=" + $scope.EMitraTransactionId).
            success(function (data, status) {
                if (data > 0) {
                    alert('Payment Successful Received');
                    window.location = "../Citizen/SocietyProfile";
                }
                else {
                    ShowModelAlert("Society Administrative Details", 'Issue in Payment Receiveing Please Try after Some time');
                }


            }).
            error(function (data, status) {
                $scope.SocietyRegisteredOffice = [];
            });
    }

    $scope.BackToPage = function () {
        var RightBankUrl = $scope.BackUrl.replaceAll("_", "/");
        window.location = RightBankUrl;
    }

    String.prototype.replaceAll = function (searchStr, replaceStr) {
        var str = this;
        // no match exists in string?
        if (str.indexOf(searchStr) === -1) {
            // return string
            return str;
        }
        // replace and remove first match, and do another recursirve search/replace
        return (str.replace(searchStr, replaceStr)).replaceAll(searchStr, replaceStr);
    }


    $scope.OnPageLoad();

});