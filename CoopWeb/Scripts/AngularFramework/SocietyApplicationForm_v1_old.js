var TableData10 = [];
var CitizenModule = angular.module("CitizenMod", ['angularUtils.directives.dirPagination']);
var ImageDataList = [];
var BIOdataList = [];
var SocietyDataForEdit = [];
var Nomi_ALLMEMBERS = [];

function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
    this.FileName = '';
}

function BIOdata() {

    this.Column2 = '';//NAME  
    this.Column3 = '';//GEN
    this.Column4 = '';//DBO
    this.Column5 = '';//IMG NAME
    this.Column8 = '';//FNAME    
    this.Column10 = '';//ADDRESS
}





var SportImageDataList = [];
CitizenModule.controller("CitizenCnt", function ($scope, $http) {
    // Global Setting
    $scope.OtpDilogBox = false;
    var MemberDetails = [];
    var NOMINATEDMEMBERS = [];
    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $scope.DivSms = true;
    $scope.divTabulation = true;
    $scope.divSocialProfiling = false;
    $scope.divSocietyProfilingShow = false;
    $scope.divAddBtn = true;
    $scope.Blocks = [];
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
    $scope.IsNewRegistration = false;
    $scope.IsEditMode = false;
    $scope.IsAddressEditAble = true;

    $scope.PartialSave = 0;

    $scope.ADLSCFileName = '';
    $scope.ASOCFileName = '';
    $scope.ARegistrationCertificateFile = '';
    $scope.ABylawsFile = '';
    $scope.ARulesAndRegulationFile = '';
    $scope.ARSSCFileName = '';
    $scope.AMemberListFile = '';
    $scope.ABankOpeningCertificate = '';
    $scope.PaidUpShareCapital = '';
    $scope.NoOfBranches = '';
    $scope.Tot_ELECTEDMEMBERS = '';
    $scope.Tot_GENERALMEMBERS = '';
    $scope.Tot_MEMBERS = '';
    $scope.TableDataForFilter = '';
    $scope.SubmitStatus = 0;
    //start same address
    $scope.IsPermanentSave = 0;
    $scope.Auth = 'otp';
    $scope.MsgAadhar = 'Please allow system for your AADHAAR based Authentication/ eKYC.'
    $scope.DivAddressDetails = false;
    $scope.SocietyRegistration = { IsNewRegistration: true };

    $scope.ElectedMemberModel = {
        "AADHAARNo": "",
        "Name": "",
        "FatherName": "",
        "Address": "",
        "ContactNo": "",
        "DOB": "",
        "Gender": "",//text
        "CategoryId": "0",
        "PostId": '0',
        "PostName": "",
        "Photo": "",
        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
        "IsActive": true,
        "UserType": "4",//Elected board Menbers
        "AdministratorDesignationId": "-1",
        "Occupation": "",
        "MembershipNo": "",
        "ShareAllotted": "",
        "ShareCapitalAmount": "",
        "loaneeType": "",
        "MEMBERTYPE": "",//($(this).find('td:eq(15)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
        "SubmitStatus": $scope.SubmitStatus
    }


    $scope.ElUploadAdministrative = function (file, row) {
        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = file[0];
        FileDataWithRowObj.RowNo = row;
        ImageDataList.push(FileDataWithRowObj);
    }

    $scope.GetSocietyDownloadList = function (SocietyIdENC) {
        $scope.ADLSCFileName = '';
        $scope.ASOCFileName = '';
        $scope.ARegistrationCertificateFile = '';
        $scope.ABylawsFile = '';
        $scope.ARulesAndRegulationFile = '';
        $scope.ARSSCFileName = '';
        $scope.AMemberListFile = '';
        $scope.ABankOpeningCertificate = '';
        $http.get(SiteUrl + "/Citizen/GetSocietyDownloadList/?SocietyIdENC=" + SocietyIdENC).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.ADLSCFileName = data[0].DLSCFileName;
                    $scope.ASOCFileName = data[0].SOCFileName;
                    $scope.ARegistrationCertificateFile = data[0].RegistrationCertificateFile;
                    $scope.ABylawsFile = data[0].BylawsFile;
                    $scope.ARulesAndRegulationFile = data[0].RulesAndRegulationFile;
                    $scope.ARSSCFileName = data[0].RSSCFileName;
                    $scope.AMemberListFile = data[0].MemberListFile;
                    $scope.ABankOpeningCertificate = data[0].BankOpeningCertificate;
                }
            }).
            error(function (data1, status) {
            });
    }

    $scope.SportElUploadAdministrative = function (file) {
        SportImageDataList.push(file[0]);
    }

    $scope.updateaddress = function () {

        $scope.SocietyRegisteredOffice.IsRuralcopy = $scope.SocietyRegisteredOffice.IsRural;
        $scope.RuralUrbanBlockCopy();
        if ($scope.SocietyRegisteredOffice.IsSameCEOAddress == false || $scope.SocietyRegisteredOffice.IsSameCEOAddress == false) {
            $scope.allDisable = false;
            $scope.SocietyRegisteredOffice.HouseBuildingNocopy = null;
            $scope.SocietyRegisteredOffice.LaneStreetNamecopy = null;
            $scope.SocietyRegisteredOffice.Localitycopy = null;
            $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
            $scope.SocietyRegisteredOffice.GP_ID_copy = '0';
            $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = '0';
            $scope.SocietyRegisteredOffice.CITY_IDcopy = '0';
            $scope.SocietyRegisteredOffice.WARD_IDcopy = '0';
            $scope.SocietyRegisteredOffice.PinCodecopy = '0';
            $scope.SocietyRegisteredOffice.AreaOfOperationcopy = null;
            $scope.SocietyRegisteredOffice.CityTypes_copy = '0';
            $scope.SocietyRegisteredOffice.DIVISION_ID_copy = '0';
            $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = '0';
            $scope.SocietyRegisteredOffice.BLOCK_ID_copy = '0';
        }
        else if ($scope.SocietyRegisteredOffice.IsSameCEOAddress == true || $scope.SocietyRegisteredOffice.IsSameCEOAddress == true) {
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
            $scope.SocietyRegisteredOffice.CityTypes_copy = '0';
            $scope.Divisionscopy = $scope.Divisions;
            $scope.SocietyRegisteredOffice.DIVISION_ID_copy = $scope.SocietyRegisteredOffice.DIVISION_ID;
            $scope.DistrictListcopy = $scope.DistrictList;
            $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = $scope.SocietyRegisteredOffice.DISTRICT_ID;
            $scope.Blocksdatacopy = $scope.Blocksdata;
            $scope.SocietyRegisteredOffice.BLOCK_ID_copy = $scope.SocietyRegisteredOffice.BLOCK_ID;
            if ($scope.SocietyRegisteredOffice.IsRuralAdd == true) {
                $scope.SocietyRegisteredOffice.IsRuralAddcopy = true;
                $scope.RuralUrbanBlock();
                $scope.RuralUrbanBlockCopy();
            }
            if ($scope.SocietyRegisteredOffice.IsRuralAdd == false) {
                $scope.SocietyRegisteredOffice.IsRuralAddcopy = false;
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

        $('#SSOAuth_OtpText').val('');
        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
        $('#btnSSOAuth_ReSendOTP').hide();


        $('.se-pre-con').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
    };
    $scope.OpenOtpPopUp = function (AADHAARNo, FOR, TRID) {
        if (AADHAARNo.length < 12 || AADHAARNo.length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        var arrylist = AADHAARNo.split('');
        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = AADHAARNo;
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.FOR = FOR;
        $scope.CurrentRow = TRID;
        if ($scope.FOR == 'CEO') {
            $('#txtCEOAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'CHAIRMAN') {
            $('#txtChairmanAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'APPOINTED') {
            $('#txtAppointedAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'lIQUIDATOR') {
            $('#txtliquidatorAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'SPCHAIRMAN') {
            $('#txtSPCEOAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'SPSECRETARY') {
            $('#txtSecretaryAadhar').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'TREASURER') {
            $('#txtTreasurerAadhar').attr("disabled", "disabled");
        }

    };




    $scope.OpenOtpPopUpOther = function (AADHAARNo, FOR, TRID, index) {
        TRID = String(TRID);
        if (TRID.indexOf('A') !== 0) {
            TRID = 'A' + TRID;
        }
        var rowid = 0;
        if (TRID.startsWith('A')) {
            rowid = TRID.substring(1);
        }


        //if ($scope.TableData10.filter(function (x) { return x.Aadh == $scope.TableData10[index].Aadh && x.Id != TRID }).length > 0) {
        if ($scope.TableData10.filter(function (x) { return x.Aadh == $scope.TableData10[index].Aadh && x.Id != TRID.substring(1) }).length > 0) {
            ShowOkMessage("", _language === 'en-us' ? "The Aadhaar number you have entered has already been verified. So enter another Aadhaar number." : "आपने जो आधार नंबर दर्ज किया हैं वह पहले से सत्यापित किया जा चूका हैं। अतः अन्य आधार नंबर दर्ज करें।", function () { });
        } else {
            AadhaarAuth($scope.TableData10[index].Aadh, $http, "OTP-BIO", false, function (data) {

                $scope.FillOtp = false;
                $scope.FOR = FOR;
                $scope.CurrentRow = TRID;
                $scope.AADHAARNo = '';
                BIOdataList = [];
                $scope.TableData10[index].Isvalidate = true;
                $scope.TableData10[index].Address = data.Address;

                var BIOdataObj = new BIOdata();


                BIOdataObj.Column2 = data.Name;
                BIOdataObj.Column8 = data.FName;
                BIOdataObj.Column4 = data.DOB;
                BIOdataObj.Column3 = data.Gender;
                BIOdataObj.Column5 = data.Photo;
                BIOdataObj.Column10 = data.Address;
                BIOdataList.push(BIOdataObj);
                if ($scope.FOR == 'CEO') {
                    $scope.SocietyRegistration.CEOName = data.Name;
                    $scope.SocietyRegistration.CEOGender = data.Gender;
                    $scope.SocietyRegistration.CEODOB = data.DOB;
                    $scope.CEOSocietyFile = data.Photo;
                    $scope.SocietyRegistration.CEOSocietyFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.CEOProfile = true;
                    $scope.CEORemove = true;
                    $scope.GETCEO = false;
                }
                else {
                    $scope.CurrentRowDataFill(BIOdataList[0]);
                }
            });
        }
    };

    $scope.OpenOtpPopUpOtherFun = function () {

        var TRID = $("#hdRowId").val();
        TRID = String(TRID);
        if (TRID.indexOf('A') !== 0) {
            TRID = 'A' + TRID;
        }


        if ($('#' + TRID).find('td:eq(3)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(3)').find('input').val().length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        AadhaarAuth($('#' + TRID).find('td:eq(3)').find('input').val(), $http, "OTP-BIO", false, function (data) {
            $scope.FillOtp = false;
            $scope.CurrentRow = TRID;
            $scope.AADHAARNo = '';
            BIOdataList = [];
            var BIOdataObj = new BIOdata();
        
            BIOdataObj.Column2 = data.Name;
            BIOdataObj.Column8 = data.FName;
            BIOdataObj.Column4 = data.DOB;
            BIOdataObj.Column3 = data.Gender;
            BIOdataObj.Column5 = data.Photo;
            BIOdataObj.Column10 = data.Address;
            BIOdataList.push(BIOdataObj);
            $scope.CurrentRowDataFill(BIOdataList[0]);

        });
    };

    //$scope.GenrateOTP = function () {
    //    $('.se-pre-con').show();
    //    $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + $scope.AADHAARNo).
    //        success(function (data, status) {
    //            if (data[0].txnid.length > 0) {
    //                $scope.OTPStatus = true;
    //                if (data[0].txnid.split('#')[0] == "NO") {
    //                    alert(data[0].txnid.split('#')[1]);
    //                    $('.se-pre-con').hide();
    //                }
    //                else if (data[0].txnid.indexOf("DOITRJGOVAUTH") > -1) {
    //                    $('#btnSSOAuth_SendOTP').hide();
    //                    $('#btnSSOAuth_ValidateOTP').show();
    //                    $('#btnSSOAuth_ReSendOTP').show();
    //                    $scope.txn = data[0].txnid;//.split('#')[1];
    //                    $('.se-pre-con').hide();
    //                }
    //                else {
    //                    $('.se-pre-con').hide();
    //                    $('#btnSSOAuth_SendOTP').hide();
    //                    $('#btnSSOAuth_ValidateOTP').show();
    //                    $('#btnSSOAuth_ReSendOTP').show();
    //                    //$scope.txn = data[0].txnid.split('#')[1];
    //                    $scope.txn = data[0].txnid;
    //                    //  $scope.Showgride = false;                       
    //                }
    //            }
    //            else {
    //                alert('Some Error Occured Please Try Again');
    //                $('.se-pre-con').hide();
    //            }
    //        }).
    //        error(function (data, status) {
    //            $scope.SocietyRegdata = [];
    //        });
    //}
    //umesh new jan otp send by jan adhr 17062024 
 
    $scope.GenrateOTP1 = function (AADHAARNo, FOR, TRID, index) {

       

        TRID = String(TRID);
        if (TRID.indexOf('A') !== 0) {
            TRID = 'A' + TRID;
        }
        var rowid = 0;
        if (TRID.startsWith('A')) {
            rowid = TRID.substring(1);
        }
        $scope.CurrentRow = TRID;

        if (($scope.TableData10[index].Aadh??'') == '') {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
            return;
        }
        if ($scope.TableData10.filter(function (x) { return x.Aadh == $scope.TableData10[index].Aadh && x.Id != TRID.substring(1) }).length > 0) {
            ShowOkMessage("", _language === 'en-us' ? "The Aadhaar number you have entered has already been verified. So enter another Aadhaar number." : "आपने जो आधार नंबर दर्ज किया हैं वह पहले से सत्यापित किया जा चूका हैं। अतः अन्य आधार नंबर दर्ज करें।", function () { });
            $('#SendOTP').modal('hide', {
                backdrop: 'static',
                keyboard: false
            });

        }
        else {
            GetMemberDetails($scope.TableData10[index].Aadh);
        }

        if ($scope.IsJanAadharDataFetched == true) {
            $("#btnVerifyOTP").removeAttr("disabled");
            var mid = $scope.BhamashahMemberDetail[0].JANAADHAARMEMID;
            var aadhar = AADHAARNo;

            var ENR_id = $scope.BhamashahMemberDetail[0].ENR_ID;//$('#txtAadhaarno').val()
            var JanAadhar = $scope.BhamashahMemberDetail[0].JAN_AADHAR;;
            var newString = SiteUrl.replace("../", "/");
            /*if (aadhar.trim() != '') {*/
            $http.post(newString + "citizen/SendRecheckOTP", { "AadharNo": aadhar, "JanAadharId": JanAadhar, "MemberId": mid, "ENR_Id": ENR_id }).
                success(function (data1, status) {
                 
                    var message = '';
                    if (data1 != "") {
                        message = "Otp Sent Successfully";
                        $(document.getElementById("txtOTP")).focus();
                        $('#SendOTP').modal('show', {
                            backdrop: 'static',
                            keyboard: false
                        });

                    }
                    else {
                        message = _language == 'en-us' ? 'Some error occured during sending OTP!!' : 'ओटीपी भेजते समय कुछ त्रुटि है !!';
                        alert(message);
                    }
                });
        }
    }
    $scope.GenrateOTPFun = function () {


        var TRID = $("#hdRowId").val();
        TRID = String(TRID);
        if (TRID.indexOf('A') !== 0) {
            TRID = 'A' + TRID;
        }
        var rowid = 0;
        if (TRID.startsWith('A')) {
            rowid = TRID.substring(1);
        }
        $scope.CurrentRow = TRID;


        if ($('#' + TRID).find('td:eq(3)').find('input').val().length == 0) {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
            return;
        }
        else {
            if ($('#' + TRID).find('td:eq(3)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(3)').find('input').val().length > 12) {
                alert('Invalid Aadhaar Number');
                return;
            }
        }
        //if ($scope.TableData10.filter(function (x) { return x.Aadh == $scope.TableData10[rowid].Aadh && x.Id != rowid }).length > 0) {
        //    ShowOkMessage("", _language === 'en-us' ? "The Aadhaar number you have entered has already been verified. So enter another Aadhaar number." : "आपने जो आधार नंबर दर्ज किया हैं वह पहले से सत्यापित किया जा चूका हैं। अतः अन्य आधार नंबर दर्ज करें।", function () { });
        //    $('#SendOTP').modal('hide', {
        //        backdrop: 'static',
        //        keyboard: false
        //    });

        //}
        //else {
        GetMemberDetails($('#' + TRID).find('td:eq(3)').find('input').val());
       // }

        if ($scope.IsJanAadharDataFetched == true) {
            $("#btnVerifyOTP").removeAttr("disabled");
            var mid = $scope.BhamashahMemberDetail[0].JANAADHAARMEMID;
            var aadhar = $('#' + TRID).find('td:eq(3)').find('input').val();

            var ENR_id = $scope.BhamashahMemberDetail[0].ENR_ID;//$('#txtAadhaarno').val()
            var JanAadhar = $scope.BhamashahMemberDetail[0].JAN_AADHAR;;
            var newString = SiteUrl.replace("../", "/");
            /*if (aadhar.trim() != '') {*/
            $http.post(newString + "citizen/SendRecheckOTP", { "AadharNo": aadhar, "JanAadharId": JanAadhar, "MemberId": mid, "ENR_Id": ENR_id }).
                success(function (data1, status) {
           
                    var message = '';
                    if (data1 != "") {
                        message = "Otp Sent Successfully";
                        $(document.getElementById("txtOTP")).focus();
                        $('#SendOTP').modal('show', {
                            backdrop: 'static',
                            keyboard: false
                        });

                    }
                    else {
                        message = _language == 'en-us' ? 'Some error occured during sending OTP!!' : 'ओटीपी भेजते समय कुछ त्रुटि है !!';
                        alert(message);
                    }
                });
        }
    }
    function GetMemberDetails(JanaadhaarNo) {
        $.ajax({
            url: "../citizen/GetJanAadhaarDetails",
            method: "POST",
            data: {
                JanaadhaarNo: JanaadhaarNo
            },
            dataType: "json",
            async: false,
            success: function (data1) {              
                    if (Object.keys(data1).length == 0) {
                        var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                        alert(message);
                        $scope.BhamashahMemberDetail = [];
                    }
                    else {
                        if (data1[0].msg === "Unexpected JSON token when reading DataTable. Expected StartArray, got StartObject. Path '', line 1, position 1.") {
                            var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                            alert(message);
                            $scope.BhamashahMemberDetail = [];
                            $("#GetMemberDetailsPanel").hide();
                        }
                        else {
                            $scope.BhamashahMemberDetail = data1;
                         //   console.log($scope.BhamashahMemberDetail);
                            $scope.IsJanAadharDataFetched = true;
                            $("#GetMemberDetailsPanel").show();
                            $("#MemberId").html(data1[data1.length - 1]["MaskedMid"]);
                        }
                    }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                window.location = "~/Error/General";
            }
        });
    }

    $scope.VerifyOTP = function () {


        var message1 = '';
        $(".se-pre-con").fadeIn("slow");
        //$("#btnResendOTP").prop("disabled", true);
        $("#btnVerifyOTP").attr("disabled", "disabled");
        if ($scope.OTP != null && $scope.OTP.length == 6) {
            LoaderStart();
            var newString = SiteUrl.replace("../", "/");
            $http.post(newString + "citizen/VerifyRecheckOTP", { "OTP": $scope.OTP }).
                success(function (data, status, message) {
                    $(".se-pre-con").fadeOut("slow");
                    var k = data[0].Column1;
                    var message1 = '';
                    if (k == "Success") {                       
                           if ($scope.FOR == 'CEO') {
                               // 
                                $scope.SocietyRegistration.CEOName = data[0].Column2;
                                $scope.SocietyRegistration.CEOGender = data[0].Column3;
                                $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.CEOSocietyFile = data[0].Column5;
                                $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.CEOProfile = true;
                                $scope.CEORemove = true;
                                $scope.GETCEO = false;
                            }
                            else if ($scope.FOR == 'CHAIRMAN') {
                             //   
                                $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                                $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                                $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.ChairmanFile = data[0].Column5;
                                $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.ChairmanProfile = true;
                                $scope.CHAIRMANRemove = true;
                                $scope.GETCHAIRMAN = false;
                            }
                            else if ($scope.FOR == 'APPOINTED') {
                               // 
                                $scope.SocietyRegistration.AppointedName = data[0].Column2;
                                $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                                $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.AppointedFile = data[0].Column5;
                                $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.AdministratorProfile = true;
                                $scope.APPOINTEDRemove = true;
                                $scope.GETAPPOINTED = false;
                            }
                            else if ($scope.FOR == 'lIQUIDATOR') {
                               // 
                                $scope.SocietyRegistration.liquidatorName = data[0].Column2;
                                $scope.SocietyRegistration.liquidatorGender = data[0].Column3;
                                $scope.SocietyRegistration.liquidatorDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.liquidatorFile = data[0].Column5;
                                $scope.SocietyRegistration.liquidatorFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.Isliquidator = true;
                                $scope.lIQUIDATORRemove = true;
                                $scope.GETlIQUIDATOR = false;
                            }
                            else if ($scope.FOR == 'SPCHAIRMAN') {
                                
                                $scope.SocietyRegistration.CEOName = data[0].Column2;
                                $scope.SocietyRegistration.CEOGender = data[0].Column3;
                                $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.CEOSocietyFile = data[0].Column5;
                                $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.CEOSportsProfile = true;
                                $scope.SPCHAIRMANRemove = true;
                                $scope.GETSPCHAIRMAN = false;
                            }
                            else if ($scope.FOR == 'SPSECRETARY') {
                                
                                $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                                $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                                $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.ChairmanFile = data[0].Column5;
                                $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.ChairmanSportsProfile = true;
                                $scope.SPSECRETARYRemove = true;
                                $scope.GETSPSECRETARY = false;
                            }
                            else if ($scope.FOR == 'TREASURER') {
                                
                                $scope.SocietyRegistration.AppointedName = data[0].Column2;
                                $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                                $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                                $scope.ChairmanFile = data[0].Column5;
                                $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                                $scope.AppointedFile = data[0].Column5;
                                $scope.CloseOtpBox();
                                $scope.TreasurerProfile = true;
                                $scope.TREASURERRemove = true;
                                $scope.GETTREASURER = false;
                            }
                            else {
                               $scope.CurrentRowDataFill(data[0])
                               $('#SendOTP').modal('hide');
                               alert('successfully verified OTP!');
                        }
                        $('#SendOTP').modal('hide');


                            
                

                    }
                    else {
                        message1 = _language == 'en-us' ? 'Invalid OTP \n' : 'अमान्य ओ.टी.पी. \n';
                        $("#btnResendOTP").removeAttr("disabled");
                        $("#btnVerifyOTP").removeAttr("disabled");
                        alert(message1);
                        LoaderStop();
                        //$('#adharDetailsView').hide();
                    }
                    $scope.OTP = "";

                }).
                error(function (data, status) {
                    $("#btnResendOTP").removeAttr("disabled");
                    $("#btnVerifyOTP").removeAttr("disabled");
                    $(".se-pre-con").fadeOut("slow");
                    LoaderStop();
                    $('#adharDetailsView').hide();
                    alert("Error while verification");
                });
        }
        else {
            $("#btnResendOTP").removeAttr("disabled");
            $("#btnVerifyOTP").removeAttr("disabled");
            alert("Otp must be 6 digit ");
            $(".se-pre-con").fadeOut("slow");
            // $('#adharDetailsView').hide();
        }
    };
    //end by umesh send by jan adhr 17062024 


    $scope.ValidateOTP = function () {
        $('.se-pre-con').show();
       
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + $scope.AADHAARNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + $scope.txn).
            success(function (data, status) {

                $('.se-pre-con').hide();
                if (data[0].Column1.length > 0) {
                    if (data[0].Column1 == "Failed") {
                        ShowOkMessage("Error", data[0].Column2, function () { });

                    }
                    else if (data[0].Column1 == "Success") {
                        $('.se-pre-con').hide();
                        $scope.txn = '';
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                        $('#btnSSOAuth_ReSendOTP').hide();
                        if ($scope.FOR == 'CEO') {
                          
                            $scope.SocietyRegistration.CEOName = data[0].Column2;
                            $scope.SocietyRegistration.CEOGender = data[0].Column3;
                            $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = data[0].Column5;
                            $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.CEOProfile = true;
                            $scope.CEORemove = true;
                            $scope.GETCEO = false;
                        }
                        else if ($scope.FOR == 'CHAIRMAN') {
                          
                            $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                            $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                            $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.ChairmanProfile = true;
                            $scope.CHAIRMANRemove = true;
                            $scope.GETCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'APPOINTED') {
                          
                            $scope.SocietyRegistration.AppointedName = data[0].Column2;
                            $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                            $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.AppointedFile = data[0].Column5;
                            $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.AdministratorProfile = true;
                            $scope.APPOINTEDRemove = true;
                            $scope.GETAPPOINTED = false;
                        }
                        else if ($scope.FOR == 'lIQUIDATOR') {
                          
                            $scope.SocietyRegistration.liquidatorName = data[0].Column2;
                            $scope.SocietyRegistration.liquidatorGender = data[0].Column3;
                            $scope.SocietyRegistration.liquidatorDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.liquidatorFile = data[0].Column5;
                            $scope.SocietyRegistration.liquidatorFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.Isliquidator = true;
                            $scope.lIQUIDATORRemove = true;
                            $scope.GETlIQUIDATOR = false;
                        }
                        else if ($scope.FOR == 'SPCHAIRMAN') {
                           
                            $scope.SocietyRegistration.CEOName = data[0].Column2;
                            $scope.SocietyRegistration.CEOGender = data[0].Column3;
                            $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = data[0].Column5;
                            $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.CEOSportsProfile = true;
                            $scope.SPCHAIRMANRemove = true;
                            $scope.GETSPCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'SPSECRETARY') {
                         
                            $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                            $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                            $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.ChairmanSportsProfile = true;
                            $scope.SPSECRETARYRemove = true;
                            $scope.GETSPSECRETARY = false;
                        }
                        else if ($scope.FOR == 'TREASURER') {
                          
                            $scope.SocietyRegistration.AppointedName = data[0].Column2;
                            $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                            $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                            $scope.AppointedFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.TreasurerProfile = true;
                            $scope.TREASURERRemove = true;
                            $scope.GETTREASURER = false;
                        }
                        else {
                            $scope.CurrentRowDataFill(data[0])
                        }

                    }
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }



    $scope.VerifyOTPByBIO_backup = function (isSubmit) {


        //var selectedRoles = vm.roles.filter(function (x) { return x.id === role.id; });

        if ($('#SSOAuth_chkConsent').prop("checked")) {
            $scope.messagebox = false
        }
        else {
            $scope.messagebox = true;
            $scope.MsgAadhar = 'Please allow system for your AADHAAR based Authentication/ eKYC.'
            return;

        }

        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.AADHAARNo != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Citizen/VerifyBIOKYC?AadharNo=" + $scope.AADHAARNo).
                success(function (rdata, status) {
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (rdata.Status == "1") {
                        $scope.UserDetails = rdata.UserDetails;
                        //ShowOkMessage("", _language == 'en-us' ? 'Successfully Varified' : "Successfully Varified", function () {                        
                        alert("Successfully Varified");

                        BIOdataList = [];
                        var BIOdataObj = new BIOdata();
                      
                        BIOdataObj.Column2 = rdata.UserDetails.Name;
                        BIOdataObj.Column8 = rdata.UserDetails.FName;
                        BIOdataObj.Column4 = rdata.UserDetails.DOB;
                        BIOdataObj.Column3 = rdata.UserDetails.Gender;
                        BIOdataObj.Column5 = rdata.UserDetails.Photo;
                        BIOdataObj.Column10 = rdata.UserDetails.Address;
                        BIOdataList.push(BIOdataObj);


                        $('.se-pre-con').hide();
                        $scope.txn = '';
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                        $('#btnSSOAuth_ReSendOTP').hide();
                        if ($scope.FOR == 'CEO') {
                            $scope.SocietyRegistration.CEOName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.CEOGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.CEODOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.CEOProfile = true;
                            $scope.CEORemove = true;
                            $scope.GETCEO = false;
                        }
                        else if ($scope.FOR == 'CHAIRMAN') {
                            $scope.SocietyRegistration.ChairmanName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.ChairmanGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.ChairmanDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.ChairmanProfile = true;
                            $scope.CHAIRMANRemove = true;
                            $scope.GETCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'APPOINTED') {
                            $scope.SocietyRegistration.AppointedName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.AppointedGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.AppointedDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.AppointedFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.AppointedFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.AdministratorProfile = true;
                            $scope.APPOINTEDRemove = true;
                            $scope.GETAPPOINTED = false;
                        }
                        else if ($scope.FOR == 'lIQUIDATOR') {
                            $scope.SocietyRegistration.liquidatorName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.liquidatorGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.liquidatorDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.liquidatorFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.liquidatorFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.Isliquidator = true;
                            $scope.lIQUIDATORRemove = true;
                            $scope.GETlIQUIDATOR = false;
                        }
                        else if ($scope.FOR == 'SPCHAIRMAN') {
                            $scope.SocietyRegistration.CEOName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.CEOGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.CEODOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.CEOSportsProfile = true;
                            $scope.SPCHAIRMANRemove = true;
                            $scope.GETSPCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'SPSECRETARY') {
                            $scope.SocietyRegistration.ChairmanName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.ChairmanGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.ChairmanDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.ChairmanSportsProfile = true;
                            $scope.SPSECRETARYRemove = true;
                            $scope.GETSPSECRETARY = false;
                        }
                        else if ($scope.FOR == 'TREASURER') {
                            $scope.SocietyRegistration.AppointedName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.AppointedGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.AppointedDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.AppointedFile = rdata.UserDetails.Photo;
                            $scope.AppointedFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.TreasurerProfile = true;
                            $scope.TREASURERRemove = true;
                            $scope.GETTREASURER = false;
                        }
                        else {
                            $scope.CurrentRowDataFill(BIOdataList[0])
                        }
                        //$('.se-pre-con').hide();
                        //$scope.CloseOtpBox();
                        //$scope.MsgAadhar = _language == 'en-us' ? 'Successfully Varified' : "सफलतापूर्वक सत्यापित किया गया";
                        //$scope.messagebox = true;
                        //return;

                        //});
                        $scope.showDetails = true;
                        //window.location = '../Election/ElectionSocietyList';
                    } else {
                        ShowOkMessage("", _language == 'en-us' ? "Please try after some time." : "Please try after some time.", function () { });
                        $scope.showDetails = false;
                    }

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again." : "Invalid Data. Please try again.", function () { }); //return false;
                });
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
        }
        $scope.showLoader = false;
    };


    $scope.ClearAadharData = function (Clear) {
        if (Clear == 'CEO') {
            $scope.SocietyRegistration.CEOName = '';
            $scope.SocietyRegistration.CEOGender = '';
            $scope.SocietyRegistration.CEODOB = '';
            $scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.CEOSocietyFile = '';
            $scope.CEOProfile = false;
            $scope.CEORemove = false;
            $scope.GETCEO = true;
        }
        if (Clear == 'CHAIRMAN') {
            $scope.SocietyRegistration.ChairmanName = '';
            $scope.SocietyRegistration.ChairmanGender = '';
            $scope.SocietyRegistration.ChairmanDOB = '';
            $scope.ChairmanFile = '';
            $scope.SocietyRegistration.ChairmanFile = '';
            $scope.ChairmanProfile = false;
            $scope.CHAIRMANRemove = false;
            $scope.GETCHAIRMAN = true;
        }
        if (Clear == 'APPOINTED') {
            $scope.SocietyRegistration.AppointedName = '';
            $scope.SocietyRegistration.AppointedDOB = '';
            $scope.SocietyRegistration.AppointedGender = '';
            $scope.AppointedFile = '';
            $scope.SocietyRegistration.AppointedFile = '';
            $scope.AdministratorProfile = false;
            $scope.APPOINTEDRemove = false;
            $scope.GETAPPOINTED = true;
        }
        if (Clear == 'lIQUIDATOR') {
            $scope.SocietyRegistration.liquidatorName = '';
            $scope.SocietyRegistration.liquidatorDOB = '';
            $scope.SocietyRegistration.liquidatorGender = '';
            $scope.liquidatorFile = '';
            $scope.SocietyRegistration.liquidatorFile = '';
            $scope.Isliquidator = false;
            $scope.lIQUIDATORRemove = false;
            $scope.GETlIQUIDATOR = true;
        }
        if (Clear == 'SPCHAIRMAN') {
            $('#txtSPCEOAADHAARNo').removeAttr("disabled");
            $('#txtSPORTCEOContactNo').removeAttr("disabled");
            $('#ddlCEOSPORTCategory').removeAttr("disabled");
            $scope.SocietyRegistration.CEOAADHAARNo = '';
            $scope.SocietyRegistration.CEOName = '';
            $scope.SocietyRegistration.CEOGender = '';
            $scope.SocietyRegistration.CEODOB = '';
            $scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.CEOContactNo = '';
            $scope.SocietyRegistration.CEOSocietyFile = '';
            $scope.CEOSportsProfile = false;
            $scope.SPCHAIRMANRemove = false;
            $scope.GETSPCHAIRMAN = true;
            $scope.SocietyRegistration.CEOCategoryId = '0';
        }
        if (Clear == 'TREASURER') {
            $scope.SocietyRegistration.AppointedAADHAARNo = '';
            $scope.SocietyRegistration.AppointedContactNo = '';
            $scope.SocietyRegistration.AppointedCategoryId = '0'
            $('#txtTreasurerAadhar').removeAttr("disabled");
            $('#txtAppointedContactNo').removeAttr("disabled");
            $('#ddlAppointedSPORTCategory').removeAttr("disabled");
            $scope.SocietyRegistration.AppointedGender = '';
            $scope.SocietyRegistration.AppointedDOB = '';
            //$scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.AppointedFile = '';
            $scope.TreasurerProfile = false;
            $scope.TREASURERRemove = false;
            $scope.GETTREASURER = true;

        }
        if (Clear == 'SPSECRETARY') {
            $('#txtSecretaryAadhar').removeAttr("disabled");
            $('#txtSPChairmanContactNo').removeAttr("disabled");
            $('#ddlChairmanSPORTCategory').removeAttr("disabled");
            $scope.ChairmanSportsProfile = false;
            $scope.SocietyRegistration.ChairmanAADHAARNo = '';
            $scope.SocietyRegistration.ChairmanName = '';
            $scope.SocietyRegistration.ChairmanGender = '';
            $scope.SocietyRegistration.ChairmanDOB = '';
            $scope.ChairmanFile = '';
            $scope.SocietyRegistration.ChairmanFile = '';
            $scope.ChairmanSportsProfile = false;
            $scope.SPSECRETARYRemove = false;
            $scope.GETSPSECRETARY = true;
            $scope.SocietyRegistration.ChairmanContactNo = '';
            $scope.SocietyRegistration.ChairmanCategoryId = 0;
        }
    }
    $scope.ClearAadharDataFromTable = function (TR) {

        if (TR == undefined)
            TR = $("#hdRowId").val();
        TR = String(TR);
        if (TR.indexOf('A') !== 0) {
            TR = 'A' + TR;
        }
        var rowValue = TR.substring(1);

        $('#' + TR).find('td:eq(1)').find('input').val('');
        $('#' + TR).find('td:eq(1)').find('input').val('');//AADHAR
        $('#' + TR).find('td:eq(2)').find('input').val('');//NAME      
        $('#' + TR).find('td:eq(3)').find('input').val('');//FATHERNAME
        //      $('#' + TR).find('td:eq(4)').find('input').removeAttr("disabled");//MOBILE NO
        $('#' + TR).find('td:eq(4)').find('input').val('');//MOBILE NO
        $('#' + TR).find('td:eq(5)').find('input').val('');//DOB
        $('#' + TR).find('td:eq(6)').find('input').val('');
        $('#' + TR).find('td:eq(7)').find('select').val('0');
        $('#' + TR).find('td:eq(7)').find('select').removeAttr("disabled");
        $('#' + TR).find('td:eq(8)').find('input').val('');//POST
        $('#' + TR).find('td:eq(8)').find('input').removeAttr("disabled");
        //  $('#' + TR).find('td:eq(9)').find('input').val('');//DOB 
        $('#' + TR).find('td:eq(10)').find('input').val('');//DOB 

        $('#' + TR).find('td:eq(11)').find('input').val('');//DOB 

        if (rowValue > 1000 && rowValue < 2000) {
            $('#' + TR).find('td:eq(8)').find('textarea').val('');
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $($('#' + TR).find('td:eq(10)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(10)').find('i')[0]).show();
        }
        if (rowValue > 499 && rowValue < 1000) {
            $('#' + TR).find('td:eq(8)').find('select').val(0);
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $($('#' + TR).find('td:eq(10)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(10)').find('i')[0]).show();
        }
        else {
            $('#' + TR).find('td:eq(9)').find('input').val('');
            // ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(10)').find('input').val('');//DOB

            $('#' + TR).find('td:eq(12)').find('input').val('');//DOB
            $('#' + TR).find('td:eq(13)').find('input').val('');//DOB
            $($('#' + TR).find('td:eq(16)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(16)').find('i')[0]).show();
        }

    }
    $scope.CurrentRowDataFill = function (data) {
        var TR = $scope.CurrentRow;

        // $('#' + TR).find('td:eq(1)').find('input').val('');
        TR = String(TR);
        if (TR.indexOf('A') !== 0) {
            TR = 'A' + TR;
        }
        var rowValue = TR.substring(1);

        $('#' + TR).find('td:eq(4)').find('input').val(data.Column2);//NAME  
        $('#' + TR).find('td:eq(5)').find('input').val(data.Column8);//FNAME         
        $('#' + TR).find('td:eq(7)').find('input').val(((data.Column4 == undefined ? "" : data.Column4).replace('-', '/')).replace('-', '/'));
        $('#' + TR).find('td:eq(8)').find('label').text(data.Column3 == 'M' ? 'MALE' : 'FEMALE');
        if (rowValue > 1000 && rowValue < 2000) {
            ($('#' + TR).find('td:eq(11)').find('img'))[0].src = data.Column5 == undefined ? "" : data.Column5;
            $('#' + TR).find('td:eq(9)').find('textarea').val(data.Column10 == undefined ? "" : data.Column10);//ADDRESS
            $($('#' + TR).find('td:eq(12)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(10)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
            var jElement = $('#' + TR).find('td:eq(9)').find('textarea');
            jElement.css('border', 'rgb(222, 221, 221)');
            $($('#' + TR).find('td:eq(11)').find('i')[1]).show();//img show
        }
        if (rowValue > 2000 && rowValue < 3000) {
            ($('#' + TR).find('td:eq(11)').find('img'))[0].src = data.Column5 == undefined ? "" : data.Column5;
            $('#' + TR).find('td:eq(9)').find('textarea').val(data.Column10 == undefined ? "" : data.Column10);//ADDRESS
            $($('#' + TR).find('td:eq(12)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(10)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
            var jElement = $('#' + TR).find('td:eq(9)').find('textarea');
            jElement.css('border', 'rgb(222, 221, 221)');
            $($('#' + TR).find('td:eq(11)').find('i')[1]).show();//img show

        }
        else if (rowValue > 499 && rowValue < 1000) {
            $('#' + TR).find('td:eq(3)').find('label').text(data.Column10 == undefined ? "" : data.Column10);//FNAME
            ($('#' + TR).find('td:eq(11)').find('img'))[0].src = (data.Column5 ?? "");
            $($('#' + TR).find('td:eq(12)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(10)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
            $($('#' + TR).find('td:eq(11)').find('i')[1]).show();//img show
        }
        else if (rowValue >= 0 && rowValue < 500) {
            $('#' + TR).find('td:eq(18)').find('label').text(data.Column10 == undefined ? "" : data.Column10);//FNAME
            ($('#' + TR).find('td:eq(11)').find('img'))[0].src = (data.Column5 ?? ""); 
            $($('#' + TR).find('td:eq(12)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(11)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
            $($('#' + TR).find('td:eq(11)').find('i')[1]).show();//img show
        }


        var jElement = $('#' + TR).find('td:eq(3)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(4)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(5)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(6)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(8)').find('label');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(9)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(10)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        $scope.CloseOtpBox();
        //307691961366
    }
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
    //Add Row Calender Css Add
    $scope.AddCalender = function () {
        $('.AddDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D',
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
            yearRange: "-100:-18"

        });
    }
    //END

    // GET Society Class
    $scope.GetSocietyClass = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=" + $scope.SocietyRegistration.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });

    }
    //END
    // GET LIABILITY
    $scope.GetLiability = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LIABILITY&MasterCode=0").
            success(function (data, status) {
                $scope.LiabilityTypeList = data;

            }).
            error(function (data, status) {
                $scope.LiabilityTypeList = [];
            });
    }
    //END
    // GET MajorActivity
    $scope.GetMajorActivity = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MAJORACTIVITY&MasterCode=0").
            success(function (data, status) {
                $scope.MajorActivityList = data;
            }).
            error(function (data, status) {
                $scope.MajorActivityList = [];
            });
    }
    //END
    // GET Activity
    $scope.GetActivity = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACTIVITY&MasterCode=" + $scope.SocietyRegistration.MajorActivityId).
            success(function (data, status) {
                $scope.ActivityList = data;
            }).
            error(function (data, status) {
                $scope.ActivityList = [];
            });
        $scope.SocietyRegistration.BusinessId = '0';
    }
    //END
    // GET Business
    $scope.GetBusiness = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=BUSINESS&MasterCode=" + $scope.SocietyRegistration.ActivityId).
            success(function (data, status) {
                $scope.BusinessList = data;
            }).
            error(function (data, status) {
                $scope.BusinessList = [];
            });
    }
    //END


    //GET Load Table data
    $scope.DashboardData = function (ActId, SocietyTypeId, ClassId) {
        $http.post(SiteUrl + "/Citizen/CitizenDashboardDataWithDepartmentUser", { "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId) }).
            success(function (data, status) {
                $scope.TableData = [];
                $scope.TableData = data;
                if ($scope.TableData.length > 0) {
                    setTimeout(function () {
                        $($($('#SocietyListDeshBoard tbody tr:first')[0]).find('td:eq(0)')[0]).find('input').prop('checked', true);
                    }, 3000);
                    $scope.TableDataForFilter = $scope.TableData;
                    $scope.GetSocietiesForDeshBoard($scope.TableData[0].SocietyIdENC);
                    $scope.GetSocietyDownloadList($scope.TableData[0].SocietyIdENC);
                    $scope.PaidUpShareCapital = ($scope.TableData[0]).PaidUpShareCapital;
                    $scope.NoOfBranches = ($scope.TableData[0]).NoOfBranches;
                    $scope.Tot_ELECTEDMEMBERS = ($scope.TableData[0]).Tot_ELECTEDMEMBERS;
                    $scope.Tot_GENERALMEMBERS = ($scope.TableData[0]).Tot_GENERALMEMBERS;
                    $scope.Tot_MEMBERS = parseInt(($scope.TableData[0]).Tot_GENERALMEMBERS) + parseInt(($scope.TableData[0]).Tot_ELECTEDMEMBERS);
                    $scope.IsCCB = $scope.TableData[0].IsCCB;
                    //$scope.IsCCB == true ? $('#22279').show() : $('#22279').hide();  //LOCal
                    $scope.IsCCB == true ? $('#21427').show() : $('#21427').hide();  //production 
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
            error(function (data, status) {
                $scope.TableData = [];
            });

    }
    //END
    $scope.SWCSData = {};
    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.divOtherSocietys = false;
        $scope.nepster = true;
        if ($("#hdnIsFromSWC").val() != "Yes") {
            $scope.DashboardData();
        }
        $scope.showtabbar = true;
        $scope.IsCityzen = $('#EUserType').val() == 'C' ? true : false;
        $scope.KUser = $('#EUserType').val() == 'K' ? true : false;
        $('#txtRegistrationDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtSportRegistrationDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtliquidatorAppointedDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtElectionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtSportElectionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtSportAgmDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        // by n saini
        $('.clsDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        $('#txtDateRSSC').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        $('#txtAGMDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        $('#txtDateSOC').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });


        $('#txtDateSLSA').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        //$('#txt_DOB').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    maxDate: '0D',
        //    defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
        //    yearRange: "-100:-18"
        //});

        $('.VoterDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D',
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
            yearRange: "-100:-18"

        });

        $("input").focusout(function () {
            var jElement = $(this);
            if (jElement.val() != '') {
                if (jElement.attr('maxlength') == jElement.val().length || jElement.attr('maxlength') == undefined) {
                    jElement.css('border', '1px solid #ccc');
                }
                else {
                    jElement.css('border', 'solid 1px red');
                }
            }
        });
        $("input[type='radio']").click(function () {
            var jElement = $(this);
            if (jElement.prop('checked') == true) {
                validate = true;
                jElement.closest('div').css('border', '1px solid #fff');
            }
            else {
                validate = false;
                jElement.closest('div').css('border', 'solid 1px red');
            }

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
        }



        // Get List of all ACT
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
                if ($('#EUserType').val() == 'K' && ($('#EServiceid').val() == '4209' || $('#EServiceid').val() == '4172') && $('#IsUpdateYes').val() == "0") {

                    if ($('#EServiceid').val() == '4209')//PROFILE
                    {
                        $scope.AddNewSociety();
                        $scope.CheckRegOrProfileing(2);
                        $scope.SocietyRegistration.IsNewRegistration = false;
                        $("input[name='RegistrationProfile']")[0].checked = false;
                        $("input[name='RegistrationProfile']")[1].checked = true;
                        $scope.CheckBRN = true;
                        $scope.divBrnbtn = true;
                        $scope.nepster = true;
                        $scope.divBrn = true;
                    }
                    else if ($('#EServiceid').val() == '4172')//REG
                    {
                        $scope.AddNewSociety();
                        $scope.CheckRegOrProfileing(1);
                        $scope.SocietyRegistration.IsNewRegistration = true;
                        $("input[name='RegistrationProfile']")[0].checked = true;
                        $("input[name='RegistrationProfile']")[1].checked = false;
                        $scope.CheckBRN = true;
                        $scope.divBrnbtn = true;
                        $scope.nepster = true;
                        $scope.divBrn = true;

                        //$scope.divBrnYN = false; 



                    }


                    $scope.all = true;
                    $scope.SocietyAct = false;

                }
                else {
                    $scope.divTabulation = true;
                    $scope.divSocialProfiling = false;
                    $scope.divSocietyProfilingShow = false;
                }


            }).
            error(function (data, status) {


            });

        //START : Get State List
        $scope.Statelist = [];
        $scope.Statelist.push(
            {
                "STATE_ID": 6,
                "STATE_ENG": 'Rajasthan',
                "STATE_MANGAL": 'राजस्थान'

            });

        //END : Get State List

        //START : Get Division List
        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
        //END



        //Get Chairman Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        $scope.GetLiability();
        $scope.GetMajorActivity();

        $(".se-pre-con").fadeOut("slow");

        var Temp = window.location.href.split('?');
        if (Temp.length > 1) {
            var Temp2 = (Temp[1].split('='));
            if (Temp2.length > 1) {
                if (Temp2[1].toString() == true) {
                    $scope.AddNewSociety();
                }
            }
        }
        //// alert($("#hdnIsFromSWC").val());
        // if ($("#hdnIsFromSWC").val() == "Yes") {
        //     $scope.SWCSData = {};
        //     $http.post(SiteUrl + "/Citizen/BGetSWCSData").
        //         success(function (data, status) {
        //             $scope.SWCSData = JSON.parse(data);
        //             $scope.SocietyRegistration.ActId = $scope.SWCSData.ActID;
        //             showHideDiv()
        //             $scope.AddNewSociety();
        //             $scope.SocietyRegistration.IsNewRegistration = true;
        //             $scope.CheckRegOrProfileing();
        //             $scope.ShowValidateBtn = false;
        //             $scope.BrnDetails()

        //             $scope.SocietyRegistration.BRN = $scope.SWCSData.BRN;
        //         }).
        //         error(function (data, status) {
        //             //$scope.DistrictList = [];
        //         });
        // }
        // END : Post List for dropdown

    };
    // END : Call default functions on page load

    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.GetDistrictList = function (id) {
        if ($scope.SocietyRegisteredOffice.DIVISION_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID).
                success(function (data, status) {
                    $scope.DistrictList = data;

                }).
                error(function (data, status) {
                    $scope.DistrictList = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_ID_copy != '0' && id == 2) {

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID_copy).
                success(function (data, status) {
                    $scope.DistrictListcopy = data;

                }).
                error(function (data, status) {
                    $scope.DistrictListcopy = [];
                });

        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_IDSecretary).
                success(function (data, status) {
                    $scope.DistrictListSecretary = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_IDTreasurer).
                success(function (data, status) {
                    $scope.DistrictListTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListTreasurer = [];
                });
        }

    };

    $scope.GetSocietyDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegistration.DIVISION_ID).
            success(function (data, status) {
                $scope.AreaDistrictList = data;
            }).
            error(function (data, status) {
                $scope.AreaDistrictList = [];
            });
    };



    //END : Get District List

    //Start Get Tehsil
    $scope.GetTehsil = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=TEHSIL&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
            success(function (data, status) {
                $scope.TehsilList = data;
            }).
            error(function (data, status) {
                $scope.TehsilList = [];
            });
        $scope.Levelofficedata();
    };
    //End Tehsil list

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
    //END   

    //START : 
    $scope.AppointedShowHide = function () {
        if ($scope.SocietyRegistration.IsAdministratorAppointed == false) {
            $scope.AppointedYN = false;
        }
        else if ($scope.SocietyRegistration.IsAdministratorAppointed == true) {
            $scope.AppointedYN = true;
            //$scope.DesignationList()
        }
        else
            $scope.AppointedYN = false;
    };
    //END
    //START : 
    $scope.AppointedShowHidetable = function () {


        if ($scope.SocietyDetailsData.Administrator.IsAdministratorAppointed == "Yes" || "हाँ") {
            $scope.showtbl = true;
        }
        else {
            $scope.showtbl = false;
            //$scope.DesignationList()
        }
    };
    //END
    //START : 
    $scope.AdministrativeBoard = function () {
        $scope.ActId = $scope.SocietyRegistration.ActId;
        if ($scope.SocietyRegistration.IsAdministrative == "Administrative") {
            $scope.IsAdministrativeY = true;//true
            $scope.IsAdministrativeN = false;
            $scope.AppointedYN = true;
            //$scope.CEO = true;
            $scope.Isliquidator = false;
            TableData10 = [];
            $scope.translation.NominatedMember = 'Appointed Member'
        }
        else if ($scope.SocietyRegistration.IsAdministrative == "Board") {
            $scope.translation.NominatedMember = 'Nominated Member'
            $scope.SocietyRegistration.AppointedName = '';
            $('#txtAppointedDOB').val('');
            $scope.SocietyRegistration.AppointedDOB = '';
            $scope.SocietyRegistration.AppointedPostName = '';
            $scope.SocietyRegistration.liquidatorName = '';
            $scope.SocietyRegistration.liquidatorPostName = '';
            $scope.SocietyRegistration.liquidatorContactNo = '';
            $('#txtliquidatorAppointedDate').val('');
            $scope.SocietyRegistration.liquidatorAppointedDate = '';
            $scope.IsAdministrativeY = false;
            $scope.IsAdministrativeN = true;
            $scope.CEO = false;
            $scope.Isliquidator = false;
            $scope.AppointedYN = false;
            var temp = 0;

            TableData10 = [];
            var temp = 0;
            if ($scope.ActId != 10) {
                var X = $scope.ActId == 1 ? 8 : 16;
                X = $scope.ActId == 7 ? 6 : X;
                for (var x = 1; x < X; x++) {
                    TableData10.push(
                        {
                            "Id": x,
                            "POSTNAME": '',
                            "PostId": '0',
                            "Photo": '../Content/Images/Balnk.png',
                            "CatId": "0",
                            "CategoryId": "0"
                        });
                }
                angular.forEach(TableData10, function (item) {
                    if ($scope.ActId == '1') {
                        if (temp == 0) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT');
                        }
                        else if (temp == 1) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष');
                        }
                        else if (temp == 2) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY' : 'सचिव');
                        }
                        else if (temp == 3) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'TREASURER' : 'कोषाध्यक्ष');
                        }
                        else if (temp == 4) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'DIRECTOR MEMBER' : 'निदेशक सदस्य');
                        }
                    }
                    else if ($scope.ActId == '3') {
                        if (temp == 0) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT')
                        }
                        else if (temp == 1) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                        }
                        else if (temp == 2) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                        }
                        else if (temp == 3) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'DIRECTOR' : 'निदेशक')
                        }
                    }
                    else if ($scope.ActId == '7') {
                        if (temp == 0) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT');
                            item.PostId = '1016';
                        }
                        else if (temp == 1) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                            item.PostId = '1015';
                        }
                        else if (temp == 2) {
                            item.POSTNAME = ($scope._language == 'en-us' ? 'Treasurer' : 'कोषाध्यक्ष')
                            item.PostId = '1011';
                        }
                    }
                    temp = temp + 1;
                });
                $scope.TableData10 = TableData10;
            }


            $('#TblElectedMember tr').each(function () {
                if ($scope.ActId == '3') {
                    if (temp == 0) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT')
                    }
                    else if (temp == 1) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                    }
                    else if (temp == 2) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                    }
                    else if (temp == 3) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'DIRECTOR' : 'निदेशक')
                    }
                }
                else if ($scope.ActId == '1') {
                    if (temp == 0) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT')
                    }
                    else if (temp == 1) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                    }
                    else if (temp == 2) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'SECRETARY' : 'सचिव')
                    }
                    else if (temp == 3) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'TREASURER' : 'कोषाध्यक्ष')
                    }
                    else if (temp == 4) {
                        $(this).find('td:eq(8)').find('input').val($scope._language == 'en-us' ? 'DIRECTOR MEMBER' : 'निदेशक सदस्य')
                    }
                }
                else if ($scope.ActId == '7') {
                    if (temp == 0) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT');
                        item.PostId = '1016';
                    }
                    else if (temp == 1) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                        item.PostId = '1015';
                    }
                    else if (temp == 2) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'Treasurer' : 'कोषाध्यक्ष')
                        item.PostId = '1011';
                    }
                }

                temp = temp + 1;
            });

            TableData10NomiAll = [];
            if ($scope.ActId != 1) {
                var X = 1;
                for (var x = 0; x < X; x++) {
                    TableData10NomiAll.push(
                        {
                            "Id": x + 500,
                            "POSTNAME": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CatId": "0",
                            "CategoryId": "0"
                        });
                }
            }
            $scope.TableData10NomiAll = TableData10NomiAll;

        }
        else if ($scope.SocietyRegistration.IsAdministrative == "liquidator") {
            $scope.IsAdministrativeY = false;
            $scope.IsAdministrativeN = false;
            $scope.CEO = false;
            $scope.AppointedYN = false;
            $scope.SocietyRegistration.IsAdministratorAppointed = false;
            $scope.Isliquidator = true;
        }
        //$("input[type=text]").val("");
        //$("select").prop("selectedIndex", 0);

    };
    //END

    //START : Get City List
    $scope.GetCityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });


    };

    $scope.GetCityListByCityTypes = function (id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }

        if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function (data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListcopy = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function (data, status) {
                    $scope.CityListdataSecretary = data;
                }).
                error(function (data, status) {
                    $scope.CityListdataSecretary = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function (data, status) {
                    $scope.CityListdataTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.CityListdataTreasurer = [];
                });
        }

        // $scope.RuralUrbanBlock();
    };
    //END : Get City List

    // START : Get Ward List
    $scope.GetWardList = function (id) {
        if ($scope.SocietyRegisteredOffice.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_ID).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDcopy != '0' && id == 2 && $scope.SocietyRegisteredOffice.CITY_IDcopy != undefined) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDcopy).
                success(function (data, status) {
                    $scope.Wardcopy = data;
                }).
                error(function (data, status) {
                    $scope.Wardcopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDSecretary != '0' && $scope.SocietyRegisteredOffice.CITY_IDSecretary != undefined && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDSecretary).
                success(function (data, status) {
                    $scope.WardSecretary = data;
                }).
                error(function (data, status) {
                    $scope.WardSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.CITY_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDTreasurer).
                success(function (data, status) {
                    $scope.WardTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.WardTreasurer = [];
                });
        }

    };

    // END : Get Ward List

    // START : Get Ward List
    $scope.GetWardListbySociety = function () {
        $scope.Ward = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.SocietyRegistration.CITY_ID).
            success(function (data, status) {
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
    };
    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {

        if ($scope.SocietyRegisteredOffice.BLOCK_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_ID).
                success(function (data, status) {

                    $scope.GramPanchayats = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_ID_copy).
                success(function (data, status) {

                    $scope.GramPanchayatscopy = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatscopy = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDSecretary).
                success(function (data, status) {

                    $scope.GramPanchayatsSecretary = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatsSecretary = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer).
                success(function (data, status) {

                    $scope.GramPanchayatsTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatsTreasurer = [];

                });
        }


    };
    // END : Get List of GramPanchayat
    //Start
    $scope.GramPanchayatListForBasicD = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegistration.BLOCK_ID).
            success(function (data, status) {
                $scope.AreaGramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.AreaGramPanchayats = [];

            });
    };
    //End
    // START : Get List of Villages
    $scope.VillageList = function (id) {
        if ($scope.SocietyRegisteredOffice.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_ID).
                success(function (data, status) {
                    $scope.Villages = data;
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDcopy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDcopy).
                success(function (data, status) {
                    $scope.Villagescopy = data;

                }).
                error(function (data, status) {
                    $scope.Villagescopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDSecretary).
                success(function (data, status) {
                    $scope.VillagesSecretary = data;

                }).
                error(function (data, status) {
                    $scope.VillagesSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.GP_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDTreasurer).
                success(function (data, status) {
                    $scope.VillagesTreasurer = data;

                }).
                error(function (data, status) {
                    $scope.VillagesTreasurer = [];
                });
        }

    };
    // END : Get List of Villages
    // START : Get List of Villages for hide
    $scope.GetVillageForBasic = function () {
        $scope.AreaVillages = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegistration.GP_ID).
            success(function (data, status) {

                $scope.AreaVillages = data;
            }).
            error(function (data, status) {
                $scope.AreaVillages = [];
            });
    };
    // END : Get List of Villages
    // Get List of Block
    $scope.BlockList = function () {
        if ($scope.SocietyRegistration.WorkScopeLevel_ID != '7' && $scope.SocietyRegistration.WorkScopeLevel_ID != '8') {
            $scope.Blocks = [];
            $scope.AddMoreForSportShow = true;
            if ($scope.SocietyRegistration.DISTRICT_ID != '0') {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                    success(function (data, status) {
                        $scope.Blocks = data;
                    }).
                    error(function (data, status) {
                        $scope.Blocks = [];
                    });
            }
        }
        else {
            $scope.GetCityList();
        }

    };
    //end block
    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $scope.Blocksdata = [];
            $scope.GramPanchayats = [];
            $scope.Villages = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocksdata = data;
                }).
                error(function (data, status) {
                    $scope.Blocksdata = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function (data, status) {
                    $scope.Blocksdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.Blocksdatacopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function (data, status) {
                    $scope.BlocksdataSecretary = data;
                }).
                error(function (data, status) {
                    $scope.BlocksdataSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function (data, status) {

                    $scope.BlocksdataTreasurer = data;

                }).
                error(function (data, status) {
                    $scope.BlocksdataTreasurer = [];
                });
        }

    };
    //end

    $scope.UploadRegistrationCertificateFile = function (file) {
        $scope.RegistrationCertificateFile = file[0];
    }
    $scope.UploadBylawsFile = function (file) {
        //if (!checkFileLengthPage((file[0].size), 5242880)) {    // 5242880 =5Mb
        //    alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 5 MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 5 MB से कम करें|");
        //    $(row).val('');
        //    $(row).parent().find('.filesize').text('');
        //    return;
        //}
        $scope.ByeLawsSocietyFile = file[0];
    }

    $scope.UploadOldExecutiveCommitteeFile = function (file) {
        //if (!checkFileLengthPage((file[0].size), 5242880)) {    // 5242880 =5Mb
        //    alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 5 MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 5 MB से कम करें|");
        //    $(row).val('');
        //    $(row).parent().find('.filesize').text('');
        //    return;
        //}
        $scope.OldExecutiveCommittee = file[0];
    }

    $scope.UploadCooperativesListFile = function (file) {
        $scope.CooperativesListFile = file[0];
    }
    $scope.UploadRulesAndRegulationFile = function (file) {
        $scope.RulesAndRegulationFile = file[0];
    }
    //SPORT SOCIETY
    $scope.UploadRSSCFileName = function (file) {
        $scope.RSSCFileName = file[0];
    }

    $scope.UploadDLSCFileName = function (file) {
        $scope.DLSCFileName = file[0];
    }

    $scope.UploadSOCFileName = function (file) {
        $scope.SOCFileName = file[0];
    }

    $scope.UploadObjectsFile = function (file) {
        $scope.ObjectsFileName = file[0];
    }
    $scope.UploadSLSAFileName = function (file) {
        $scope.SLSAFileName = file[0];
    }


    $scope.BackClipk = function () {
        if ($('#EUserType').val() == "D")
            window.location = "../Citizen/SocietyProfile";
        else
            window.location = "../Citizen/Index";
    }

    $scope.UploadDOCAffiliationCertificatesFile = function (file) {
        $scope.DOCAffiliationCertificatesFile = file[0];
    }

    //$scope.UploadMemberListFile = function (file) {
    //    $scope.MemberListFile = file[0];
    //}
    //NEW
    $scope.UploadMembersListExcelFile = function (file) {
        $scope.MembersListExcelFile = file[0];
    }

    $scope.UploadWorkPlanEconomicViabilityFile = function (file) {
        $scope.WorkPlanEconomicViabilityFile = file[0];
    }
    $scope.UploadGeneralMeetingProcedureFile = function (file) {
        $scope.GeneralMeetingProcedureFile = file[0];
    }

    $scope.UploadBalanceSheetExcelFile = function (file) {
        $scope.BalanceSheetExcelFile = file[0];
    }



    // Start : Partial Submit
    ////$scope.SavePartialRegisteredOfficeDetails = function () {
    ////    $scope.PartialSave = 1;
    ////    $scope.SaveRegisteredOfficeDetails();
    ////}

    // End : Partial Submint

    // START : Save Registered Office Details
    $scope.SaveRegisteredOfficeDetails = function (SubmitStatus) {
        $scope.SocietyRegisteredOffice.SocietyIdEnc = $scope.SocietyIdEnc;
        $scope.SocietyRegisteredOffice.ActId = $scope.ActId;
        $scope.SocietyRegistration.ActId = $scope.ActId
        $scope.SocietyRegisteredOffice.SubmitStatus = SubmitStatus;
        $scope.SocietyRegisteredOffice.EMitraTransactionId = $scope.EMitraTransactionId;
        if ($scope.SocietyOfficeFormValidation() && $scope.SocietyOfficeFormValidationPartial()) {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveSocietyRegisteredOfficeDetails", $scope.SocietyRegisteredOffice).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if ((data.data.split('@')).length > 1) {
                        $scope.EMitraTransactionId = (data.data.split('@')).length > 1 ? (data.data.split('@'))[1] : '0';
                        $scope.SocietyRegistration.SocietyIdEnc = data.data.split('@')[0];
                        ShowModelAlert("Society Office Detail", 'Registered Office Detail Save Sucessfully');

                        $scope.FinalSubmit($scope.SocietyRegistration.SocietyIdEnc);
                    }
                    else if (data.data == -1) {
                        ShowModelAlert("Society/CEO Office Details", 'Registered Office Detail Already Exists');
                    }
                    else {
                        ShowModelAlert("Society Administrative Details", 'An Error Occurred Please Try Again Later');
                    }

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.SocietyRegisteredOffice = [];
                });

        }

    }

    // END : Save Registered Office Details

    // START : Get Sub Class
    $scope.GetSocietyCategory = function () {
        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + $scope.SocietyRegistration.ClassId + "&SubClassId=" + $scope.SocietyRegistration.SubClassId).
            success(function (data, status) {
                if (data.length > 0)
                    $scope.SocietyCategoryList = data;
            }).
            error(function (data, status) {
                $scope.SocietyCategoryList = [];
            });
    };
    // END : Get Sub Class

    // START : Get Society Category List
    $scope.GetSubClass = function () {
        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyRegistration.ClassId + "&SocietyTypeId=").// + $scope.SocietyRegistration.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });
        $scope.SocietyRegistration.SocietyCategoryId = '0';
    };
    // END 

    // START : Add More Member Details
    $scope.AddMoreMemberDetails = function () {
        if ($scope.TableValiDate(3)) {
            $(".clsShowHide").hide();
            var rows = $('#myTable2 tr').length + 1;
            if (_language == 'hi-in') {
                $('#myTable2').append('<tr>' +
                    '<td>' + rows + '</td>' +
                    '<td><input type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.DirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyRegistration.ContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या" ng-model="SocietyRegistration.AADHAARNo' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    '<td><input id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.DOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
                    '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="ElGender' + rows + '" value="F" ng-model="SocietyRegistration.Gender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="ElGender' + rows + '" ng-model="SocietyRegistration.Gender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
                    '<td><div style="width:100%"><select id="Category' + rows + '" ng- model="SocietyRegistration.CategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    '<td><div style="width:100%"><select id="Post' + rows + '" ng-model="SocietyRegistration.PostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                    '<td><input type="file" id="File_' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
                    '<td><i class="fa fa-trash clsShowHide" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    '</tr>');
            }
            else {
                $('#myTable2').append('<tr>' +
                    '<td>' + rows + '</td>' +
                    '<td><input type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" ng- model="SocietyRegistration.DirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" ng-model="SocietyRegistration.ContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" ng-model="SocietyRegistration.AADHAARNo' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    '<td><input id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.DOB' + rows + '" placeholder="DOB" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
                    '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="ElGender' + rows + '" value="F" ng-model="SocietyRegistration.Gender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);"> FeMale </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="ElGender' + rows + '" ng-model="SocietyRegistration.Gender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">Male</label></td ></div >' +
                    '<td><div style="width:100%"><select id="Category' + rows + '" ng- model="SocietyRegistration.CategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    '<td><div style="width:100%"><select id="Post' + rows + '" ng-model="SocietyRegistration.PostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                    '<td><input type="file" id="File_' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
                    '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    '</tr>');
            }
            $scope.AddCalender();

            //FIll Category DropDown 

            var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.CategoryList, function (item) {
                if (_language == 'en-us') {
                    ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                }
            });
            $("#Category" + rows).html(ddHTML);
            //END
            //FIll Post DropDown
            var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.ChairmanPosts, function (item) {
                if (_language == 'en- us') {
                    ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
                }
            });
            $("#Post" + rows).html(ddHTML);
            //END
        }

    };
    // END : Add More Member Details

    // START : Add More Member Details
    $scope.AddMoreMemberDetailsNew = function (Flag, Isvalidate) {

 
        if (Flag == 0) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = $('#TblNominatedMember tr').length + 500;
                if (_language == 'hi-in') {
                    $('#TblNominatedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                        '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input disabled="disabled" type= "text" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ><label style="display:none">{{item.Address}}</label></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद नाम"></div></td >' +
                        '<td><div style="width:100%"><select class="form-control"><option value="0" disabled>--- चुने ---</option><option value="1" class="ng-binding">सरकार द्वारा</option><option value="2" class="ng-binding">नाबार्ड द्वारा</option><option value="3" class="ng-binding">आरबीआई द्वारा</option><option value="4" class="ng-binding">तकनीकी निदेशक द्वारा</option></select ></div ></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    if ($scope.SocietyRegistration.ActId != 7) {
                        $('#TblNominatedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                            '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type= "text" disabled="disabled" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Father/ Husband Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" class="form-control" placeholder="Post Name"></div></td >' +
                            '<td><div style="width:100%"><select class="form-control"><option value="0" disabled>--- Select ---</option><option value="1" class="ng-binding">By Government</option><option value="2" class="ng-binding">By Nabard</option><option value="3" class="ng-binding">By RBI</option><option value="4" class="ng-binding">By Technical Director</option></select ></div ></td >' +
                            '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                            '</tr>');
                    }
                    else {
                        $('#TblNominatedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                            '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type= "text" disabled="disabled" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Father/ Husband Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" class="form-control" placeholder="Post Name"></div></td >' +
                            '<td style="display:none"><div style="width:100%"><select class="form-control"><option value="0" disabled>--- Select ---</option><option value="1" class="ng-binding">By Government</option><option value="2" class="ng-binding">By Nabard</option><option value="3" class="ng-binding">By RBI</option><option value="4" class="ng-binding">By Technical Director</option></select ></div ></td >' +
                            '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td ><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                            '</tr>');
                    }
                }
            }
            else {
                // alert(false);
            }
        }
        if (Flag == 1) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = ($('#TblSportElectedMember tr').length) + 1001;
                if (_language == 'hi-in') {
                    $('#TblSportElectedMember').append('<tr id="A' + rows + '"" dat-val="0">' +
                        '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                        '<td ng-show="hideshowhide" style="display:none" ><input type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                        '<td ng-show="hideshowhide"  style="display:none"><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" placeholder= "निदेशक का नाम" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse"   autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input disabled="disabled" type= "text" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                        '<td><textarea type="text" name="Address" class="form-control" placeholder="Address" autocomplete="off"></textarea></td >' +
                        '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-show="SHOWREMOVE" onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ></td >' +
                        '<td><i class="fa fa-trash clsShowHide" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblSportElectedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                        '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                        '<td ng-show="hideshowhide" style="display:none"><input  type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                        '<td ng-show="hideshowhide"  style="display:none"><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type= "text" disabled="disabled" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Father/ Husband Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="POST"></div></td >' +
                        '<td><textarea type="text" name="Address" class="form-control" placeholder="पता" autocomplete="off"></textarea></td >' +
                        '<td"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ></td >' +
                        '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
            }
        }
        if (Flag == 2) {
            if ($scope.ValidateNominatedMember(Flag)) {
                var rows = $('#TblSportNominatedMember tr').length + 1;
                if (_language == 'hi-in') {
                    $('#TblSportNominatedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                        '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                        '<td ng-show="hideshowhide" style="display:none" ><input type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                        '<td ng-show="hideshowhide"  style="display:none"><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ><label style="display:none">{{item.Address}}</label></td >' +
                        '<td><input disabled="disabled" type= "text" name= "FatherHusbandName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                        '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblSportNominatedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                        '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                        '<td ng-show="hideshowhide" style="display:none"><input  type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                        '<td ng-show="hideshowhide"  style="display:none" ><input type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ><label style="display:none">{{item.Address}}</label></td >' +
                        '<td><input type= "text" disabled="disabled" name= "FatherHusbandName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Father/ Husband Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
            }
        }
        if (Flag == 3) {
            if ($scope.AdministrativeFormValidation(Flag, Isvalidate)) {
                var rows = $('#TblElectedMember tr').length + 1;
                if (_language == 'hi-in') {

                    if (($scope.SocietyRegistration.IsNewRegistration == false) && $scope.Act2001) {
                        $('#TblElectedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td ng-show="hideshowhide" style="display:none"><input  type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                            '<td ng-show="hideshowhide"  style="display:none"><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                            '<td><input type="text" name="AdhareNo" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName" placeholder="पिता / पति नाम"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"  placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>-- चुने --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                            '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text" class="form-control" placeholder="व्यवसाय"></td>' +
                            '<td><input type="text" class="form-control" placeholder="सदस्यता संख्या"></td>' +
                            '<td><input type="text" class="form-control" placeholder="आवंटित शेयर की संख्या"></td>' +
                            '<td><input type="text" class="form-control" placeholder="शेयर पूंजी राशि"></td>' +
                            '<td><select class="form-control" id="loaneeNonloane' + rows + '" ><option value="0" disabled>-- चुने --</option><option value="1" >ऋणी</option><option value= "2" >अऋणी</option> </select ></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><label style="display:none"></label></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowFromTable(' + rows + ')"></i></td >' +
                            '</tr>');

                    }
                    else {
                        $('#TblElectedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td ng-show="hideshowhide" style="display:none"><input  type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                            '<td ng-show="hideshowhide"  style="display:none"><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                            '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName"  placeholder="पिता / पति नाम"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"   placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>-- चुने --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                            '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text" class="form-control" placeholder="व्यवसाय"></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><label style="display:none"></label></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowFromTable(' + rows + ')"></i></td >' +
                            '</tr>');
                    }
                }
                else {
                    if (($scope.SocietyRegistration.IsNewRegistration == false) && $scope.Act2001) {
                        $('#TblElectedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td ng-show="hideshowhide" style="display:none"><input  type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                            '<td ng-show="hideshowhide"  style="display:none"><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                            '<td><input type="text" name="AdhareNo"  placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName" ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName" placeholder="Father/ Husband Name"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"  placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select id="Category' + rows + '"><option value="0" disabled>-- Select --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" placeholder="Post"></div></td >' +
                            '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text"  placeholder="Occupation"></td>' +
                            '<td><input type="text"  placeholder="Membership No."></td>' +
                            '<td><input type="text"  placeholder="No. of Share Allotted"></td>' +
                            '<td><input type="text"  placeholder="Share Capital Amount"></td>' +
                            '<td><select class="form-control" id="loaneeNonloane' + rows + '" ><option value="0" disabled>-- Select --</option><option value="1" >loanee</option><option value= "2" >Non loane</option> </select ></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><label style="display:none"></label></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowFromTable(' + rows + ')"></i></td >' +
                            '</tr>');

                    }
                    if ($scope.SocietyRegistration.SocietyTypeId == 9 && $scope.Act2001 && ($scope.SocietyRegistration.IsNewRegistration == true)) {
                        $('#TblElectedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td ><input  type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                            '<td  ><input  type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                            '<td><input type="text" name="AdhareNo"  placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName"  placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" disabled="disabled"  name="FatherName"  placeholder="Father/ Husband Name"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"   placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select  id="Category' + rows + '"><option value="0" disabled>-- Select --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" placeholder="Post"></div></td >' +
                            '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text"  placeholder="Occupation"></td>' +
                            '<td style="display:none"><input type="text"  placeholder="Membership No."></td>' +
                            '<td style="display:none"><input type="text"  placeholder="No. of Share Allotted"></td>' +
                            '<td style="display:none"><input type="text"  placeholder="Share Capital Amount"></td>' +
                            '<td style="display:none"><select class="form-control" id="loaneeNonloane' + rows + '" ><option value="0" disabled>-- Select --</option><option value="1" >loanee</option><option value= "2" >Non loane</option> </select ></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><label></label></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowFromTable(' + rows + ')"></i></td >' +
                            '</tr>');


                    }

                    else {
                        $('#TblElectedMember').append('<tr id="A' + rows + '" dat-val="0">' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td ng-show="hideshowhide"  style="display:none"><input type="text" name ="societyName" placeholder="Name of society" autocomplete="off"></td>' +
                            '<td ng-show="hideshowhide"  style="display:none" ><input type="text" name ="societyPost" placeholder="Post in Society" autocomplete="off"></td>' +
                            '<td><input type="text" name="AdhareNo"  placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName"  placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" disabled="disabled"  name="FatherName"  placeholder="Father/ Husband Name"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"   placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select  id="Category' + rows + '"><option value="0" disabled>-- Select --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" placeholder="Post"></div></td >' +
                            '<td ><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text"  placeholder="Occupation"></td>' +
                            '<td style="display:none"><input type="text"  placeholder="Membership No."></td>' +
                            '<td style="display:none"><input type="text"  placeholder="No. of Share Allotted"></td>' +
                            '<td style="display:none"><input type="text"  placeholder="Share Capital Amount"></td>' +
                            '<td style="display:none"><select class="form-control" id="loaneeNonloane' + rows + '" ><option value="0" disabled>-- Select --</option><option value="1" >loanee</option><option value= "2" >Non loane</option> </select ></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><label></label></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowFromTable(' + rows + ')"></i></td >' +
                            '</tr>');

                    }
                }
            }
            else {


            }
        }

        //FIll Category DropDown 

        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language == 'en-us') {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#Category" + rows).html(ddHTML);
        //END
        //FIll Post DropDown
        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language == 'en- us') {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#Post" + rows).html(ddHTML);
        //END


    };
    // END : Add More Member Details

    // START : Add More Member Details For Edit FIlling
    $scope.AddMoreMemberDetailsWithoutValidation = function () {
        $(".clsShowHide").hide();
        var rows = $('#myTable2 tr').length + 1;
        $('#myTable2').append('<tr dat-val="0">' +
            '<td>' + rows + '</td>' +
            '<td><input type= "text" id="ElDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.ElDirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input type="text" id="ElContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyRegistration.ElContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
            '<td><input type="text" id="ElAADHAARNo' + rows + '" name="AdhareNo" class="form-control" placeholder="आधार संख्या" ng-model="SocietyRegistration.ElAADHAARNo1' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
            '<td><input id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.ElDOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
            '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="ElGender' + rows + '" value="F" ng-model="SocietyRegistration.ElGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="ElGender' + rows + '" ng-model="SocietyRegistration.ElGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
            '<td><div style="width:100%"><select id="ElCategoryId' + rows + '" ng- model="SocietyRegistration.ElCategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
            '<td><div style="width:100%"><select id="ElPostId' + rows + '" ng-model="SocietyRegistration.ElPostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
            '<td><label style="display:none" id="lblElAdministrativeFile' + rows + '"></label><input type="file" id="ElFile' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
            '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
            '</tr>');
        $scope.AddCalender();

        //FIll Category DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#ElCategoryId" + rows).html(ddHTML);
        //END
        //FIll Post DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#ElPostId" + rows).html(ddHTML);
        //END


    };
    // END : Add More Member Details

    // START : Add More Member Details DOWN
    $scope.AddMoreSportsMemberDetails = function () {
        if ($scope.TableValiDate(2)) {
            $(".clsShowHide").hide();
            var rows = $('#SportTableAdd tr').length + 1;
            $('#SportTableAdd').append('<tr dat-val="0">' +
                '<td>' + rows + '</td>' +
                '<td><input type= "text" id="SPElDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.DirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                '<td><input type="text"  id="SPElContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyRegistration.ContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                '<td><input type="text"  id="SPElAADHAARNo' + rows + '" name="AdhareNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="आधार संख्या" ng-model="SocietyRegistration.AADHAARNo' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                '<td><input id="txt_MDOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.DOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
                '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="ElGender' + rows + '" value="F" ng-model="SocietyRegistration.SportMemberGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="Gender1' + rows + '" ng-model="SocietyRegistration.SportMemberGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
                '<td><div style="width:100%"><select id="SportCategory' + rows + '" ng- model="SocietyRegistration.CategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                '<td><div style="width:100%"><select id="SportPost' + rows + '" ng-model="SocietyRegistration.PostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                '<td><label style="display:none" id="lblSportAdministrativeFile' + rows + '"></label><input type="file" id="File_' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
                '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveSportsData(this,' + rows + ')"></i></td >' +
                '</tr>');
            $scope.AddCalender();
            //FIll Category DropDown
            var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.CategoryList, function (item) {
                if (_language) {
                    ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                }
            });
            $("#SportCategory" + rows).html(ddHTML);
            //END
            //FIll Post DropDown
            var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.ChairmanPosts, function (item) {
                if (_language) {
                    ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
                }
            });
            $("#SportPost" + rows).html(ddHTML);
            //END
        }

    };

    $scope.AddMoreSportsMemberDetailsWithOutValidation = function () {
        $(".clsShowHide").hide();
        var rows = $('#SportTableAdd tr').length + 1;
        $('#SportTableAdd').append('<tr dat-val="0">' +
            '<td>' + rows + '</td>' +
            '<td><input type= "text" id="SPElDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.DirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input type="text"  id="SPElContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyRegistration.ContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
            '<td><input type="text"  id="SPElAADHAARNo' + rows + '" name="AdhareNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="आधार संख्या" ng-model="SocietyRegistration.AADHAARNo' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
            '<td><input id="txt_MDOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.DOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
            '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="ElGender' + rows + '" value="F" ng-model="SocietyRegistration.SportMemberGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="ElGender' + rows + '" ng-model="SocietyRegistration.SportMemberGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
            '<td><div style="width:100%"><select id="SportCategory' + rows + '" ng- model="SocietyRegistration.CategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
            '<td><div style="width:100%"><select id="SportPost' + rows + '" ng-model="SocietyRegistration.PostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
            '<td><label style="display:none" id="lblSportAdministrativeFile' + rows + '"></label><input type="file" id="File_' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
            '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveSportsData(this,' + rows + ')"></i></td >' +
            '</tr>');
        $scope.AddCalender();
        //FIll Category DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#SportCategory" + rows).html(ddHTML);
        //END
        //FIll Post DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#SportPost" + rows).html(ddHTML);
        //END


    };
    // END : Add More Member Details
    // START : Add More Member Details UP
    $scope.AddMoreExecutiveSportsMemberDetails = function () {
        if ($scope.TableValiDate(1)) {
            $(".clsShowHide").hide();
            var rows = $('#SportExecutiveTable tr').length + 1;
            if (rows < 19) {
                $('#SportExecutiveTable').append('<tr dat-val="0">' +
                    '<td>' + rows + '</td>' +
                    '<td><input type= "text" id="SPDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.DirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    '<td><input type="text" id="SPContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  ng-model="SocietyRegistration.ContactNo' + rows + '" maxlength="10"  onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    '<td><input type="text" id="SPAADHAARNo' + rows + '" name="AdhareNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="आधार संख्या" ng-model="SocietyRegistration.AADHAARNo' + rows + '" maxlength="12"  onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    '<td><input id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.DOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
                    '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="SPGender' + rows + '" value="F" ng-model="SocietyRegistration.ExecutiveGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="SPGender' + rows + '" ng-model="SocietyRegistration.ExecutiveGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
                    '<td><div style="width:100%"><select id="SPCategory' + rows + '" ng- model="SocietyRegistration.CategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    '<td><textarea type="text" id="SPtextarea' + rows + '" name="Address" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="पता" ng-model="SocietyRegistration.Address' + rows + '"  autocomplete="off"></textarea></td >' +
                    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                    '<td><label style="display:none" id="lblSportExecutiveFile' + rows + '"></label><input type="file" id="File_' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().UploadAdministrative' + rows + '(this.files)"></td >' +
                    //'<td><i class="fa fa-trash" id="Remove_' + rows + '" onclick="RemoveExecutiveSportsData(this)"></i></td >' +
                    '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="angular.element(this).scope().RemoveSportElUploadAdministrative' + rows + '(this.files);RemoveExecutiveSportsData(this)"></i></td >' +
                    '</tr>');
                $scope.AddCalender();
                //FIll Category DropDown
                var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
                angular.forEach($scope.CategoryList, function (item) {
                    if (_language) {
                        ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                    }
                    else {
                        ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                    }
                });
                $("#SPCategory" + rows).html(ddHTML);
                //END
                //FIll Post DropDown
                var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
                angular.forEach($scope.ChairmanPosts, function (item) {
                    if (_language) {
                        ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
                    }
                    else {
                        ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
                    }
                });
                $("#SPPost" + rows).html(ddHTML);
                //END
            }
            else {
                $('#btnAddMoreExecutiveSports').hide();
                alert("You Can Not Add rows more then 21");

            }
        }
        //FIll Category DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#Category" + rows).html(ddHTML);
        //END
        //FIll Post DropDown

        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#Post" + rows).html(ddHTML);
        //END


    };
    // END : Add More Member Details

    // START : Add More Member Details UP
    $scope.AddMoreExecutiveSportsMemberDetailsWithoutValidation = function () {
        var rows = $('#SportExecutiveTable tr').length + 1;
        $(".clsShowHide").hide();
        if (rows < 19) {
            $('#SportExecutiveTable').append('<tr dat-val="0">' +
                '<td>' + rows + '</td>' +
                '<td><input type= "text" id="SPDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.DirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                '<td><input type="text" id="SPContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  ng-model="SocietyRegistration.ContactNo' + rows + '" maxlength="10"  onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                '<td><input type="text" id="SPAADHAARNo' + rows + '" name="AdhareNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="आधार संख्या" ng-model="SocietyRegistration.AADHAARNo' + rows + '" maxlength="12"  onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                '<td><input id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.DOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
                '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="SPGender' + rows + '" value="F" ng-model="SocietyRegistration.ExecutiveGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="SPGender' + rows + '" ng-model="SocietyRegistration.ExecutiveGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
                '<td><div style="width:100%"><select id="SPCategory' + rows + '" ng- model="SocietyRegistration.CategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                '<td><textarea type="text" id="SPtextarea' + rows + '" name="Address" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="पता" ng-model="SocietyRegistration.Address' + rows + '"  autocomplete="off"></textarea></td >' +
                '<td><div style="width:100%"><select id="SPPost' + rows + '" ng-model="SocietyRegistration.PostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                '<td><label style="display:none" id="lblSportExecutiveFile' + rows + '"></label><input type="file" id="File_' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().UploadAdministrative' + rows + '(this.files)"></td >' +
                //'<td><i class="fa fa-trash" id="Remove_' + rows + '" onclick="RemoveExecutiveSportsData(this)"></i></td >' +
                '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="angular.element(this).scope().RemoveSportElUploadAdministrative' + rows + '(this.files);RemoveExecutiveSportsData(this)"></i></td >' +
                '</tr>');
            $scope.AddCalender();
            //FIll Category DropDown
            var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.CategoryList, function (item) {
                if (_language) {
                    ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
                }
            });
            $("#SPCategory" + rows).html(ddHTML);
            //END
            //FIll Post DropDown
            var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.ChairmanPosts, function (item) {
                if (_language) {
                    ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
                }
            });
            $("#SPPost" + rows).html(ddHTML);
            //END
        }
        else {
            $('#btnAddMoreExecutiveSports').hide();
            alert("You Can Not Add rows more then 21");

        }

        //FIll Category DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#Category" + rows).html(ddHTML);
        //END
        //FIll Post DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language) {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#Post" + rows).html(ddHTML);
        //END
    };
    // END : Add More Member Details umesh2022
    $scope.getdataForAdhar = function () {
        $http.get(SiteUrl + "/society/AAdharOne/?AAdharENC=").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });

    }
    // START : Save Society Profiling
    $scope.SaveSocietyProfiling = function (SubmitStatus) {
 

        $scope.SubmitStatus = SubmitStatus;
        $(".se-pre-con").fadeIn("slow");
        $(".SaveSocietyInfo").hide();
        if ($scope.FormValidation() || $scope.FormValidationPartial()) {
            //ADMINISTRATIVE VALIDATION
            if ($scope.SocietyRegistration.ActId > 0) {
               
                if ($scope.AdministrativeFormValidation(SubmitStatus) == false) {
                    $(".se-pre-con").fadeOut("slow");
                    $(".SaveSocietyInfo").show();
                    return;
                }
                else {
                    if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                        if (!$scope.CategoryGenderFormValidation()) {
                            $(".se-pre-con").fadeOut("slow");
                            $(".SaveSocietyInfo").show();
                            return;
                        }
                    }
                }
            }
            else {
                var tempchaek = SubmitStatus == 0 ? false : true;
                if ($scope.ValidateNominatedMember(1, tempchaek) == false) {
                    $(".se-pre-con").fadeOut("slow");
                    $(".SaveSocietyInfo").show();
                    return;
                }
            }
            //END
            //ADDRESS DETAILS

            if ($scope.SocietyOfficeFormValidation() == false || $scope.SocietyOfficeFormValidationPartial() == false) {

                $(".se-pre-con").fadeOut("slow");
                $(".SaveSocietyInfo").show();
                return;
            }


            //if (!SubmitStatus)
            /// {

            // }
            var postData;
            $scope.RegistrationCertificateFile = $scope.GetFileUploaded("flRegistration_Certificate")
            $scope.ByeLawsSocietyFile = $scope.GetFileUploaded("flBy_laws");
            $scope.RulesAndRegulationFile = $scope.GetFileUploaded("flRulesAnd_Regulation");
            $scope.RSSCFileName = $scope.GetFileUploaded("flSLSCAffiliation_Certificates");

            $scope.SOCFileName = $scope.GetFileUploaded("flSOCAffiliation_Certificate");
            $scope.SLSAFileName = $scope.GetFileUploaded("flSLSAffiliation_Certificates");
            $scope.OldExecutiveCommittee = $scope.GetFileUploaded("flUploadOldExecutive_Committee");
            $scope.MembersListExcelFile = $scope.GetFileUploaded("flUploadMembers_List");
            $scope.WorkPlanEconomicViabilityFile = $scope.GetFileUploaded("flSocietyWorkPlanEconomic_Viability");
            $scope.GeneralMeetingProcedureFile = $scope.GetFileUploaded("flGeneralMeeting_Procedure");
            $scope.BalanceSheetExcelFile = $scope.GetFileUploaded("flBalanceSheet_Upload");
            $scope.ObjectsFileName = $scope.GetFileUploaded("flUpload_Objects");
            if ($scope.SocietyRegistration.ActId == 7) {
                $scope.DLSCFileName = $scope.GetFileUploaded("flDLSCAffiliation_Certificates");
            } else {
                $scope.DLSCFileName = $scope.GetFileUploaded("flCooperatives_List");
            }


            if ($scope.SocietyRegistration.ActId != "7") {
                if ($scope.lstSocietyWorkScope.length <= 0) {
                    $scope.Addmorebtn(true);
                }
                var SocietyRegistrationData = [];
                if ($scope.SocietyRegistration.IsAdministrative == 'Administrative') {
                    SocietyRegistrationData.push(
                        {
                            "AdministrativeId": "0",
                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "Name": $scope.SocietyRegistration.AppointedName,
                            "IsActive": true,
                            "ContactNo": '',
                            "AADHAARNo": '121212121212',
                            "DOB": $('#txtAppointedDOB').val().split("/").reverse().join("-"),
                            "AppointedDate": $('#txtAppointedDOB').val().split("/").reverse().join("-"),
                            "Gender": $scope.SocietyRegistration.AppointedGender,
                            "UserType": "3",
                            "AdministratorDesignationId": "-1",
                            "CategoryId": 0,
                            "PostId": 0,
                            "PostName": $scope.SocietyRegistration.AppointedPostName,
                            "Photo": '../Content/Images/Balnk.png',
                            "MEMBERTYPE": "ADMINISTRATIVE",
                            "SubmitStatus": $scope.SubmitStatus
                        });
                    $scope.SocietyRegisteredOffice.PinCodecopy = '0';
                }
                if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
                    SocietyRegistrationData.push(
                        {
                            "AdministrativeId": "0",
                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "Name": $scope.SocietyRegistration.liquidatorName,
                            "IsActive": true,
                            "ContactNo": $scope.SocietyRegistration.liquidatorContactNo,
                            "AADHAARNo": '',
                            "DOB": '',
                            "AppointedDate": $('#txtliquidatorAppointedDate').val().split("/").reverse().join("-"),
                            "Gender": '',
                            "UserType": "6",
                            "AdministratorDesignationId": "-1",
                            "CategoryId": 0,
                            "PostId": 0,
                            "PostName": $scope.SocietyRegistration.liquidatorPostName,
                            "Photo": '../Content/Images/Balnk.png',
                            "MEMBERTYPE": "LIQUIDATOR",
                            "SubmitStatus": $scope.SubmitStatus
                        });
                }
                if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                    //Ramtest-------------
                    var Count = 0;
                    $('#TblElectedMember tr').each(function (index) {
                        if ($(this).find('td:eq(10)').find('input').val() != '' && $(this).find('td:eq(9)').find('select').val() > 0 && $(this).find('td:eq(3)').find('input').val() != '') {
                            var AdministrativeId = $(this).attr("dat-val") == undefined && $(this).attr("dat-val") == "" ? 0 : $(this).attr("dat-val");
                            if (($scope.SocietyRegistration.IsNewRegistration == false) && $scope.Act2001) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": AdministrativeId,
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(18)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": $(this).find('td:eq(13)').find('input').val(),
                                        "ShareAllotted": $(this).find('td:eq(14)').find('input').val(),
                                        "ShareCapitalAmount": $(this).find('td:eq(15)').find('input').val(),
                                        "loaneeType": $(this).find('td:eq(16)').find('select').val(),
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                            else if (($scope.SocietyRegistration.IsNewRegistration == true) && $scope.Act2001 && $scope.SocietyRegistration.SocietyTypeId == 9) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": AdministrativeId,
                                        "NameOfSociety": $(this).find('td:eq(1)').find('input').val(),
                                        "PostinSociety": $(this).find('td:eq(2)').find('input').val(),
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(18)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": '',
                                        "ShareAllotted": 0,
                                        "ShareCapitalAmount": 0,
                                        "loaneeType": 0,
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }

                            else {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": AdministrativeId,

                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(18)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": '',
                                        "ShareAllotted": 0,
                                        "ShareCapitalAmount": 0,
                                        "loaneeType": 0,
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                        }

                    });

                    $('#TblNominatedMember tr').each(function () {
                        var AdministrativeId = $(this).attr("dat-val") == undefined && $(this).attr("dat-val") == "" ? 0 : $(this).attr("dat-val");
                        if ($(this).find('td:eq(1)').find('input').val() != '' && $(this).find('td:eq(7)').find('select').val() > 0 && $(this).find('td:eq(9)').find('input').val() != '') {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": AdministrativeId,
                                    "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                    "Name": $(this).find('td:eq(2)').find('input').val(),
                                    "FatherName": $(this).find('td:eq(3)').find('input').val(),
                                    "Address": $(this).find('td:eq(3)').find('label').html(),
                                    "ContactNo": $(this).find('td:eq(4)').find('input').val(),
                                    "DOB": $(this).find('td:eq(5)').find('input').val().split("/").reverse().join("-"),
                                    "Gender": ($(this).find('td:eq(6)').find('label').html()).charAt(0),//text
                                    "CategoryId": $(this).find('td:eq(7)').find('select').val(),
                                    "PostId": '0',
                                    "PostName": $(this).find('td:eq(8)').find('input').val(),
                                    "NominatedBy": $(this).find('td:eq(9)').find('select').val(),
                                    "Photo": ($(this).find('td:eq(10)').find('img'))[0].src,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "5",//Nominated board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": "NOMINATEDMEMBERS",
                                    "SubmitStatus": $scope.SubmitStatus
                                });
                        }
                    });
                }

                postData = {
                    BRN: $scope.SocietyRegistration.BRN,
                    SocietyIDEnc: $scope.SocietyRegistration.SocietyIdEnc,
                    ActId: $scope.SocietyRegistration.ActId,
                    OldRegistrationNo: $scope.OldRegistrationNo,
                    RegistrationNo: $scope.SocietyRegistration.RegistrationNo,
                    SocietyName: $scope.SocietyRegistration.SocietyName,
                    SocietyTypeId: $scope.SocietyRegistration.SocietyTypeId,
                    ClassId: $scope.SocietyRegistration.ClassId,
                    SubClassId: $scope.SocietyRegistration.SubClassId,
                    SocietyCategoryId: $scope.SocietyRegistration.SocietyCategoryId,
                    ByeLawsSocietyFile: $scope.ByeLawsSocietyFile,
                    OldExecutiveCommittee: $scope.OldExecutiveCommittee,
                    RegistrationDate: $('#txtRegistrationDate').val().split("/").reverse().join("-"),
                    ElectionDate: $('#txtElectionDate').val().split("/").reverse().join("-"),
                    NoOfBranches: $scope.SocietyRegistration.NoOfBranches,
                    LOSocietyOfficeDetails: $scope.SocietyRegistration.WorkScopeLevel,
                    STATE_ID: $scope.SocietyRegistration.STATE_ID,
                    DIVISION_ID: $scope.SocietyRegistration.DIVISION_ID,
                    DISTRICT_ID: $scope.SocietyRegistration.DISTRICT_ID,
                    TEHSIL_ID: $scope.SocietyRegistration.TEHSIL_ID,
                    BLOCK_ID: $scope.SocietyRegistration.BLOCK_ID,
                    VILLAGE_ID: $scope.SocietyRegistration.VILLAGE_ID,
                    RegistrationCertificateFile: $scope.RegistrationCertificateFile,
                    RulesAndRegulationFile: $scope.RulesAndRegulationFile,
                    lstSocietyWorkScope: $scope.lstSocietyWorkScope,
                    IsNewRegistration: $scope.SocietyRegistration.IsNewRegistration,
                    BusinessId: $scope.SocietyRegistration.BusinessId,
                    LiablityId: $scope.SocietyRegistration.LiablityId,
                    MemberLiablity: $scope.SocietyRegistration.MemberLiablity,
                    TotalMembersApplyingForSocociety: $scope.SocietyRegistration.TotalNumberOfMembers,
                    AgmDate: $('#txtAGMDate').val().split("/").reverse().join("-"),
                    TotalNumberOfMembersInAGM: $scope.SocietyRegistration.NoOfMemberInAGM,
                    EntranceFeesCollected: $('#txtEntranceFees').val(),
                    SecurityDeposit: $scope.SocietyRegistration.SecurityDeposit,
                    WorkingCapital: $scope.SocietyRegistration.WorkingCapital,
                    LoanAmt: $scope.SocietyRegistration.LoanAmt,

                    SingleShareValue: $scope.SocietyRegistration.SingleShareValue,
                    PaidUpShareCapital: $scope.SocietyRegistration.PaidUpShareCapital,
                    MemberListExcelFile: $scope.MembersListExcelFile,
                    WorkPlanShowingEconomicViabilityFile: $scope.WorkPlanEconomicViabilityFile,
                    GeneralMeetingProceedingFile: $scope.GeneralMeetingProcedureFile,
                    BalanceSheetFile: $scope.BalanceSheetExcelFile,
                    ObjectsFile: $scope.ObjectsFileName,
                    ObjectsFileName: $scope.ObjectsFileName,
                    SaveStatus: $scope.SubmitStatus,
                    AreaOfOperation: $scope.SocietyRegistration.AreaOfOperation,
                    CooperativesListFile: $scope.CooperativesListFile,
                    DLSCFileName: $scope.DLSCFileName,
                    ClassA: $scope.SocietyRegistration.TotalMembersClassA,
                    ClassB: $scope.SocietyRegistration.TotalMembersClassB,
                    ClassC: $scope.SocietyRegistration.TotalMembersClassC,
                    SocietyDirectory: $scope.SocietyDirectory,
                    BoardType: $scope.SocietyRegistration.IsAdministrative,
                    AdministrativeDataList: SocietyRegistrationData,
                    AddressDetails: $scope.SocietyRegisteredOffice
                }
            } else {

                if ($scope.lstSocietyWorkScope.length <= 0) {
                    $scope.AddMoreForSportSociety();
                }

                var SocietyRegistrationData = [];
                $scope.SocietyIdForAdministrativeDetails = 0;
                if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                    var Count = 0;
                    //Ram
                    $('#TblElectedMember tr').each(function (index) {
                        if ($(this).find('td:eq(10)').find('input').val() != '' && $(this).find('td:eq(9)').find('select').val() > 0 && $(this).find('td:eq(3)').find('input').val() != '') {
                            var AdministrativeId = $(this).attr("dat-val") == undefined && $(this).attr("dat-val") == "" ? 0 : $(this).attr("dat-val");
                            if (($scope.SocietyRegistration.IsNewRegistration == false) && $scope.Act2001) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": AdministrativeId,
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(18)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": $(this).find('td:eq(13)').find('input').val(),
                                        "ShareAllotted": $(this).find('td:eq(14)').find('input').val(),
                                        "ShareCapitalAmount": $(this).find('td:eq(15)').find('input').val(),
                                        "loaneeType": $(this).find('td:eq(16)').find('select').val(),
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                            else if (($scope.SocietyRegistration.IsNewRegistration == true) && $scope.Act2001 && $scope.SocietyRegistration.SocietyTypeId == 9) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": AdministrativeId,
                                        "NameOfSociety": $(this).find('td:eq(1)').find('input').val(),
                                        "PostinSociety": $(this).find('td:eq(2)').find('input').val(),
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(18)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": '',
                                        "ShareAllotted": 0,
                                        "ShareCapitalAmount": 0,
                                        "loaneeType": 0,
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }

                            else {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": AdministrativeId,
                                        //"NameOfSociety": $(this).find('td:eq(1)').find('input').val(),
                                        //"PostinSociety": $(this).find('td:eq(2)').find('input').val(),
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(18)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": '',
                                        "ShareAllotted": 0,
                                        "ShareCapitalAmount": 0,
                                        "loaneeType": 0,
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                        }

                    });
                    //RAM

                    $('#TblNominatedMember tr').each(function (index) {
                        var AdministrativeId = $(this).attr("dat-val") == undefined && $(this).attr("dat-val") == "" ? 0 : $(this).attr("dat-val");
                        if ($(this).find('td:eq(1)').find('input').val() != '' && $(this).find('td:eq(7)').find('select').val() > 0 && ($(this).find('td:eq(9)').find('input').val() > 0 || $scope.SocietyRegistration.ActId == "7")) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": AdministrativeId,
                                    "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                    "Name": $(this).find('td:eq(2)').find('input').val(),
                                    "FatherName": $(this).find('td:eq(3)').find('input').val(),
                                    "Address": $(this).find('td:eq(3)').find('label').html(),
                                    "ContactNo": $(this).find('td:eq(4)').find('input').val(),
                                    "DOB": $(this).find('td:eq(5)').find('input').val().split("/").reverse().join("-"),
                                    "Gender": ($(this).find('td:eq(6)').find('label').html()).charAt(0),//text
                                    "CategoryId": $(this).find('td:eq(7)').find('select').val(),
                                    "PostId": '0',
                                    "PostName": $(this).find('td:eq(8)').find('input').val(),
                                    "NominatedBy": $(this).find('td:eq(9)').find('select').val(),
                                    "Photo": ($(this).find('td:eq(10)').find('img'))[0].src,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "5",//Nominated board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": "NOMINATEDMEMBERS",
                                    "SubmitStatus": $scope.SubmitStatus
                                });
                        }
                    });
                }
                postData = {
                    SocietyIDEnc: $scope.SocietyRegisteredOffice.SocietyIdEnc,
                    ActId: $scope.SocietyRegistration.ActId,
                    IsNewRegistration: $scope.SocietyRegistration.IsNewRegistration,
                    OldRegistrationNo: $scope.OldRegistrationNo,
                    RegistrationNo: $scope.SocietyRegistration.RegistrationNo,
                    RegistrationDate: $('#txtSportRegistrationDate').val().split("/").reverse().join("-"),
                    SocietyName: $scope.SocietyRegistration.SocietyName,
                    DISTRICT_ID: $scope.SocietyRegistration.DISTRICT_ID,
                    IsStatLevel: $scope.SocietyRegistration.IsStatLevel == 'No' ? false : true,
                    AssociationId: $scope.SocietyRegistration.AssociationId.split('#')[0],
                    ElectionDate: $('#txtSportElectionDate').val().split("/").reverse().join("-"),
                    AGMDate: $('#txtSportAgmDate').val().split("/").reverse().join("-"),
                    RSSCFileName: $scope.RSSCFileName,
                    DLSCFileName: $scope.DLSCFileName,
                    SOCFileName: $scope.SOCFileName,
                    SLSAFileName: $scope.SLSAFileName,
                    lstSocietyWorkScope: $scope.lstSocietyWorkScope,
                    IsAffiliatedByRSSC: $scope.SocietyRegistration.IsAffiliatedByRSSC == 'Yes' ? true : false,
                    RSSCRegNo: $scope.SocietyRegistration.RSSCRegNo,
                    RSSCAffiliatedDate: $('#txtDateRSSC').val().split("/").reverse().join("-"),
                    IsAffiliatedBySOC: $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' ? true : false,
                    SOCRegNo: $scope.SocietyRegistration.SOCRegNo,
                    SOCAffiliatedDate: $('#txtDateSOC').val().split("/").reverse().join("-"),
                    IsAffiliationBySLSA: $scope.SocietyRegistration.IsAffiliationBySLSA == 'Yes' ? true : false,
                    SLSARegNo: $scope.SocietyRegistration.SLSARegNo,
                    SLSAAffiliatedDate: $('#txtDateSLSA').val().split("/").reverse().join("-"),
                    IsAffiliatedByDLSC: $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' ? true : false,
                    DLSCAffiliatedDate: $('#txtDateDLSCDistrict').val().split("/").reverse().join("-"),
                    DLSCRegNo: $scope.SocietyRegistration.DLSCRegNo,
                    RegistrationCertificateFile: $scope.RegistrationCertificateFile,
                    ByeLawsSocietyFile: $scope.ByeLawsSocietyFile,
                    MemberListExcelFile: $scope.MembersListExcelFile,
                    EMitraTransactionId: $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId,
                    ClassA: 0,
                    ClassB: 0,
                    ClassC: 0,
                    SocietyDirectory: $scope.SocietyDirectory,
                    SaveStatus: $scope.SubmitStatus,
                    AddressDetails: $scope.SocietyRegisteredOffice,
                    AdministrativeDataList: SocietyRegistrationData
                }
            }


            var postData = JSON.stringify({ _Entity: postData });
            //  var postData = '{"_Entity":{"BRN":"8005630018000010","SocietyIDEnc":"0x010000002517A11F41174A5CE48B2DCAE0FA7CF188FDBE5D8CEF3F9E94B6411230BD3300","ActId":"1","OldRegistrationNo":"COOP/2021/PALI/200502","RegistrationNo":"COOP/2021/PALI/200502","SocietyName":"RAJASTHAN VETERINARY RELIEF SOCIETY SADRI","SocietyTypeId":"13","ClassId":"0","SubClassId":"0","SocietyCategoryId":"0","ByeLawsSocietyFile":"","OldExecutiveCommittee":"","RegistrationDate":"","ElectionDate":"","NoOfBranches":0,"STATE_ID":"6","DIVISION_ID":"5","DISTRICT_ID":"20","BLOCK_ID":"0","VILLAGE_ID":"0","RegistrationCertificateFile":"","RulesAndRegulationFile":"../Content/SocietyProfiling/COOP2021PALI200502/AC3FEB1BB7054BFB8E5A7D147D60BD18.pdf","lstSocietyWorkScope":[{"STATE_Name":"Rajasthan","STATE_MANGAL":"????????","WorkScopeLevel_ID":"7","STATE_ID":"6","DISTRICT_ID":"20","CityTypes":"0","CITY_ID":"124","WARD_ID":"0","AreaOfOperation":"Hospital Area","DISTRICT_ENG":"Pali","DISTRICT_Name":"Pali","DISTRICT_MANGAL":"????","CityTypes1":"N/A","CITY_Name":"Sadri","CITY_MANGAL":"?????","WARD_Name":"N/A","WARD_MANGAL":"-","IsRSSC":"False","SportsAssociationName":"","SportNameEn":"","SportNameHi":"","DirectorName":"","MobileNo":"","IsRuralUrban":"0","IsStatLevel":"0","$$hashKey":"object:1043"}],"IsNewRegistration":true,"BusinessId":"187","LiablityId":"1","MemberLiablity":"0","TotalMembersApplyingForSocociety":7,"AgmDate":"2021-06-18","TotalNumberOfMembersInAGM":7,"EntranceFeesCollected":"77","SingleShareValue":0,"PaidUpShareCapital":0,"MemberListExcelFile":"../Content/SocietyProfiling/COOP2021PALI200502/412B1405F2A94BE1AC824F52310A2F40.xlsx","WorkPlanShowingEconomicViabilityFile":"","GeneralMeetingProceedingFile":"","BalanceSheetFile":"","ObjectsFile":"../Content/SocietyProfiling/COOP2021PALI200502/96C3EF8D40694EB79687E8FB6B60F910.pdf","ObjectsFileName":"../Content/SocietyProfiling/COOP2021PALI200502/96C3EF8D40694EB79687E8FB6B60F910.pdf","SaveStatus":1,"AreaOfOperation":"Hospital Area","CooperativesListFile":"","DLSCFileName":"","ClassA":null,"ClassB":null,"ClassC":null,"SocietyDirectory":"","BoardType":"Board","AdministrativeDataList":[{"AADHAARNo":"905740160002","Name":"Dr.Sanjay Rote","FatherName":"S/O Ramakant Rote","Address":"House : , LandMark : Plot No.-17,Pawan Vally,, Street : , District: Pali, PIN: 306401, State: Rajasthan","ContactNo":"9414325819","DOB":"1972-06-08","Gender":"M","CategoryId":"3","PostId":"0","PostName":"VICE CHAIRMAN","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_277eab9e52ab407ea64e755de23664ca.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"govt employee","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1},{"AADHAARNo":"630473769660","Name":"Parikshit Purohit","FatherName":"C/O Himatsinh","Address":"House : govt. veterinary hospital, LandMark : , Street : Sadri, District: Pali, PIN: 306702, State: Rajasthan","ContactNo":"9829262795","DOB":"1971-04-09","Gender":"M","CategoryId":"3","PostId":"0","PostName":"EX OFFICIO MEMBER SECRETARY","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_8e99acebc034456a98f78806d2f931dd.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"GOVT EMPLOYEE","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1},{"AADHAARNo":"498628302201","Name":"Natharam Choudhry","FatherName":"C/O Fuaram Choudhary","Address":"House : -, LandMark : , Street : bera dholi kakar hari om sagar bandh ke paas, District: Pali, PIN: 306703, State: Rajasthan","ContactNo":"9413064127","DOB":"1968-07-01","Gender":"M","CategoryId":"4","PostId":"0","PostName":"MEMBER","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_ce3da63601de417ea0c980408dbf1ab4.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"GOVT EMPLOYEE","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1},{"AADHAARNo":"837729280321","Name":"Dinesh Kumar Solanki","FatherName":"S/O Hari Ram","Address":"House : , LandMark : , Street : , District: Pali, PIN: 306702, State: Rajasthan","ContactNo":"9829374441","DOB":"1966-07-05","Gender":"M","CategoryId":"2","PostId":"0","PostName":"MEMBER","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_8fc88b097265476584f152711ebd2267.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"GOVT EMPLOYEE","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1},{"AADHAARNo":"249633201941","Name":"Neemitkumar Narendra Punamiya","FatherName":"NARENDRA ","Address":"House : Room No. 51, 5th Floor, LandMark : Opp Inorbit Mall,, Street : Kalpataru Pinnacle Chs Ltd ,Goregaon Mulund Link Road, District: Mumbai, PIN: 400104, State: Maharashtra","ContactNo":"9820119075","DOB":"1972-04-04","Gender":"M","CategoryId":"3","PostId":"0","PostName":"MEMBER","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_9aaeee20d372461380df098d9b7c2453.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"BUSINESS","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1},{"AADHAARNo":"671840686548","Name":"Vikram Singh Rathore","FatherName":"S/O: Kan Singh","Address":"House : , LandMark : 112 sadul gunj bikaner ,infront of shree ram hotal, Street : , District: Bikaner, PIN: 334001, State: Rajasthan","ContactNo":"8107916823","DOB":"1964-12-18","Gender":"M","CategoryId":"3","PostId":"0","PostName":"Chairman","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_b49b8ac106c848de91cb8b2a6e78cee2.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"Govt Employee","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1},{"AADHAARNo":"978726938520","Name":"Valaram","FatherName":"S/O: Hirraram","Address":"House : , LandMark : h.no-110 phatak se mada road, Street : , District: Pali, PIN: 306702, State: Rajasthan","ContactNo":"9960389556","DOB":"1955-01-01","Gender":"M","CategoryId":"4","PostId":"0","PostName":"MEMBER","Photo":"https://rajsahakarapp.rajasthan.gov.in/Content/SocietyProfiling/COOP2021PALI200502/member_01314e5e908743bda2e2b40913c8b4a6.jpg","SocietyIdEnc":0,"IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","Occupation":"PASHUPALAK","MembershipNo":"","ShareAllotted":0,"ShareCapitalAmount":0,"loaneeType":0,"MEMBERTYPE":"ELECTEDMEMBERS","SubmitStatus":1}],"AddressDetails":{"STATE_ID":"6","DIVISION_ID":"5","DISTRICT_ID":"20","BLOCK_ID":"0","GP_ID":"0","VILLAGE_ID":"0","CITY_ID":"124","WARD_ID":"3604","STATE_IDcopy":"6","DIVISION_ID_copy":"5","DISTRICT_ID_copy":"20","CITY_IDcopy":"122","WARD_IDcopy":"3551","BLOCK_ID_copy":"0","GP_IDcopy":"0","VILLAGE_IDcopy":"0","DIVISION_IDSecretary":"0","DISTRICT_IDSecretary":"0","CITY_IDSecretary":"0","WARD_IDSecretary":"0","BLOCK_IDSecretary":"0","GP_IDSecretary":"0","VILLAGE_IDSecretary":"0","DIVISION_IDTreasurer":"0","DISTRICT_IDTreasurer":"0","CITY_IDTreasurer":"0","WARD_IDTreasurer":"0","BLOCK_IDTreasurer":"0","GP_IDTreasurer":"0","VILLAGE_IDTreasurer":"0","SocietyIdEnc":"0x010000002517A11F41174A5CE48B2DCAE0FA7CF188FDBE5D8CEF3F9E94B6411230BD3300","IsRural":"true","HouseBuildingNo":"First Grade Veterinary Hospital","LaneStreetName":"DESURI ROAD","Locality":"SADRI TEH DESURI","PinCodecopy":306401,"PinCode":306702,"IsRuralcopy":"true","HouseBuildingNocopy":"VETERINARY POLICLINIC","LaneStreetNamecopy":"old bus stand","Localitycopy":"OLD BUS STAND PALI MARWAR","CityTypes_copy":"0","STATE_IDSecretary":"6","STATE_IDTreasurer":"6","herebyAccept":true}}}';
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveSocietyProfilingNew_v1", postData).
                success(function (data, status) {
                    $("#InnerModelPopup").addClass("alert alert-danger fade in");
                    if (data.data.indexOf("NotValidName") > -1) {
                        $(".SaveSocietyInfo").show();
                        $(".se-pre-con").fadeOut("slow");
                        var ValidNameCheck = "आप जिस सोसाइटी का रजिस्ट्रेशन कर रहे हैं उस सोसाइटी के नाम में कुछ शब्द है जो मान्य नहीं हैं | कृपया सोसाइटी का नाम सही करके फिर से आवेदन करें | अमान्य शब्द " + data.data.replace("NotValidName", "");
                        ShowOkMessage("Warning", ValidNameCheck, function () { return; });
                    } else
                        if (data.data.indexOf("ErrorInMemberdata") > -1) {
                            ShowModelAlert("Error", "<b>Error In Uploaded Members Excel. All Column values is mandatory.Please fill correct values and re-upload. </b><br/><br/>" + data.data.split('@')[1]);
                            $(".SaveSocietyInfo").show();
                            $(".se-pre-con").fadeOut("slow");
                            return;
                        } else if (data.data == '-5') {
                            if ($scope.SocietyRegistration.ActId != '7')
                                ShowModelAlert("Error", "A Society already exist with this Name or Registration or BRN");
                            else
                                ShowModelAlert("Error", "A Society already exist with this Name or Registration");
                            $(".SaveSocietyInfo").show();
                        }
                        else if (data.data == '-10') {
                            $(".SaveSocietyInfo").show();
                            ShowModelAlert("Error", "YOU ALREADY SUBMITTED 2 SOCIETY WITH THE SAME SSOID");
                        }
                        else
                            if (data.data == '-1' || data.data == '0') {
                                $(".SaveSocietyInfo").show();
                                ShowModelAlert("Error", "Some error occurred please try after some time ");
                            }
                            else if ((data.data.split('@')).length > 1) {
                                var message1 = "";
                                if (SubmitStatus == 0) {
                                    message1 = $scope.IsEditMode == false ? "The details of your society has been partially saved." : "The details of your society has been partially updated";
                                } else {
                                    message1 = "The details of your society have been submitted for approval. When your society approved from department then you can view or download related documents from dashboard";
                                }
                                ShowOkMessage("Success", message1, function () {
                                    window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                                });
                            }
                            else if (data.data.length > 10) {
                                ShowOkMessage("Success", "Basic Details Saved Sucessfully", function () {
                                    window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                                });
                            }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                    $('#btnAdministrativedetails').trigger('click');
                });


        }
        else {
            $(".se-pre-con").fadeOut("slow");

            $(".SaveSocietyInfo").show();
        }
    };
    $scope.ChangeSocietySteps = function () {
        if ($('#DivSocietySteps').attr('class') == 'SocietyStep_1') {
            $("#DivSocietySteps").removeClass("SocietyStep_1")
            $("#DivSocietySteps").addClass("SocietyStep_2");
        }
        else if ($('#DivSocietySteps').attr('class') == 'SocietyStep_2') {
            $("#DivSocietySteps").removeClass("SocietyStep_2")
            $("#DivSocietySteps").addClass("SocietyStep_3");
        }
    }
    $scope.ClearForm = function () {

        $scope.ActId = $scope.SocietyRegistration.ActId;
        $scope.SocietyRegistration.ActId = $scope.ActId;
        $scope.SocietyRegistration.CEOCategoryId = '0';
        $scope.SocietyRegistration.ChairmanCategoryId = '0';
        $scope.SocietyRegistration.AppointedCategoryId = '0';
        $scope.SocietyRegistration.CategoryId1 = '0';
        $scope.SocietyRegistration.CategoryId2 = '0';
        $scope.SocietyRegistration.PostId1 = '0';
        $scope.SocietyRegistration.PostId2 = '0';
        $scope.SocietyRegistration.ElCategoryId1 = '0';
        $scope.SocietyRegistration.ElPostId1 = '0';
        $scope.SocietyRegistration.CEOCategoryId = '0';
        $scope.SocietyRegistration.ChairmanCategoryId = '0';
        $scope.SocietyRegistration.AppointedCategoryId = '0';
        $scope.SocietyRegistration.CategoryId1 = '0';
        $scope.SocietyRegistration.CategoryId2 = '0';
        $scope.SocietyRegistration.PostId1 = '0';
        $scope.SocietyRegistration.PostId2 = '0';
        $scope.SocietyRegistration.ElCategoryId1 = '0';
        $scope.SocietyRegistration.ElPostId1 = '0';
        $scope.SocietyRegistration.BRN = '';
        $scope.SocietyRegistration.RegistrationNo = '';
        $scope.SocietyRegistration.SocietyName = '';
        $scope.SocietyRegistration.RegistrationDate = '';
        $scope.SocietyRegistration.SocietyTypeId = '0';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietySubClassList = [];
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.Rural = undefined;
        $scope.SocietyRegistration.IsStatLevel = undefined;
        $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
        $scope.SocietyRegistration.DIVISION_ID = '0';
        $scope.SocietyRegistration.DISTRICT_ID = '0';
        $scope.SocietyRegistration.BLOCK_ID = '0';
        $scope.SocietyRegistration.GP_ID = '0';
        $scope.SocietyRegistration.VILLAGE_ID = '0';
        $scope.SocietyRegistration.CITY_ID = '0';
        $scope.SocietyRegistration.CityTypes = '0';
        $scope.SocietyRegistration.WARD_ID = '0';
        $("#flRegistrationCertificateFile").val('');
        $("#flRulesAndRegulation").val('');
        $("#flBylawsFile").val('');
        $("#flBylawsFile").parent().find('.filesize').text('');
        $("#flRulesAndRegulation").parent().find('.filesize').text('');
        $("#flRegistrationCertificateFile").parent().find('.filesize').text('');
        $scope.SocietyRegistration.BusinessId = '0';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.SocietyCategoryId = '0';

        $("#flObjects").val('');
        $("#flObjects").parent().find('.filesize').text('');

        $("#flMembersListExcelFile").val('');
        $("#flMembersListExcelFile").parent().find('.filesize').text('');



    };
    //END : Delete User
    // START : Clear Society Profile Section
    $scope.ClearSocietyDetailsForm = function () {

        $scope.SocietyRegistration.CEOCategoryId = '0';
        $scope.SocietyRegistration.ChairmanCategoryId = '0';
        $scope.SocietyRegistration.AppointedCategoryId = '0';
        $scope.SocietyRegistration.CategoryId1 = '0';
        $scope.SocietyRegistration.CategoryId2 = '0';
        $scope.SocietyRegistration.PostId1 = '0';
        $scope.SocietyRegistration.PostId2 = '0';
        $scope.SocietyRegistration.ElCategoryId1 = '0';
        $scope.SocietyRegistration.ElPostId1 = '0';
        $scope.SocietyRegistration.ActId = '0';
        $scope.SocietyRegistration.BRN = '';
        $scope.SocietyRegistration.RegistrationNo = '';
        $scope.SocietyRegistration.SocietyName = '';
        $scope.SocietyRegistration.RegistrationDate = '';
        $scope.SocietyRegistration.SocietyTypeId = '0';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietySubClassList = [];
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.lstSocietyWorkScope = null;
        $scope.SocietyRegistration.Rural = undefined;
        $scope.SocietyRegistration.IsStatLevel = undefined;
        $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
        $scope.SocietyRegistration.DIVISION_ID = '0';
        $scope.SocietyRegistration.DISTRICT_ID = '0';
        $scope.SocietyRegistration.BLOCK_ID = '0';
        $scope.SocietyRegistration.GP_ID = '0';
        $scope.SocietyRegistration.VILLAGE_ID = '0';
        $scope.SocietyRegistration.CITY_ID = '0';
        $scope.SocietyRegistration.CityTypes = '0';
        $scope.SocietyRegistration.WARD_ID = '0';

        $scope.RuralDivRadio = false;
        $scope.UrbanDivRadio = false;
        $scope.grid = false;
        $scope.Addmore = false;
        $scope.VillageDiv = false;
        $scope.PsDiv = false;
        $scope.BlockruralDiv = false;
        $scope.SocietyRegistration.SocietyCategoryId = '0';
        $scope.SocietyRegistration.ElectionDate = null;
        $scope.SocietyRegistration.NoOfBranches = '';
        $("#flRegistrationCertificateFile").val('');
        $("#flRulesAndRegulation").val('');
        $("#flBylawsFile").val('');
        $("#flBylawsFile").parent().find('.filesize').text('');
        $("#flRulesAndRegulation").parent().find('.filesize').text('');
        $("#flRegistrationCertificateFile").parent().find('.filesize').text('');

    };
    // END : Clear Society Profile Section

    // START : Save Society Audit Report Details
    $scope.SaveSocietyAuditDetails = function () {

        var Msg = '';

        if ($scope.SocietyAuditReport.IsApprovedByRegistrar == undefined) {
            Msg += (_language == 'en-us' ? 'Is Audit Report Approved By Registrar (Latest Available) ? \n' : 'क्या लेखा परीक्षा रिपोर्ट रजिस्ट्रार द्वारा स्वीकृत है\n');
        }

        if ($scope.SocietyAuditReport.IsSocietyWorking == undefined) {
            Msg += (_language == 'en-us' ? 'Is Society Working ? \n' : 'क्या सोसाइटी कार्यशील है ?\n');
        }

        if ($scope.SocietyAuditReport.FinancialYear == undefined || $scope.SocietyAuditReport.FinancialYear == '0') {
            Msg += (_language == 'en-us' ? 'Financial Year For Which Last Audit Report is Available \n' : 'वित्तीय वर्ष जिसके लिए अंतिम लेखा परीक्षा रिपोर्ट उपलब्ध है\n');
        }

        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + Msg;
            alert(Msg);
            return;
        }

        var formData = new FormData();
        formData.append("SocietyId", $scope.hdfSocietyId);
        formData.append("IsApprovedByRegistrar", $scope.SocietyAuditReport.IsApprovedByRegistrar);
        formData.append("IsSocietyWorking", $scope.SocietyAuditReport.IsSocietyWorking);
        formData.append("FinancialYear", $scope.SocietyAuditReport.FinancialYear);
        formData.append("AuditReport", $scope.SocietyAuditFile);

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
                    alert(AltMsg);
                }

                $scope.ClearSocietyAuditReportForm();
            }).
            error(function (data, status) {
                ShowModelAlert("Society Audit Report", "Error While Society Audit Report Insert");
            });

    };
    // END :  Save Society Audit Report Details

    // START : Clear Society Audit Report Details
    $scope.ClearSocietyAuditReportForm = function () {
        $scope.SocietyAuditReport.IsApprovedByRegistrar = false;
        $scope.SocietyAuditReport.IsSocietyWorking = false;
        $scope.SocietyAuditReport.FinancialYear = '0';
    };
    // END : Clear Society Audit Report Details
    $scope.AddAdministrativeDetails = function (SID, ShowTab, SocietyName, FilledProfileLevel, RegistrationNo, ActId) {

        $scope.SocietyRegistration.ActId = ActId;
        $scope.SocietyIdForAdministrativeDetails = SID;

        if (ActId != "7") {
            $scope.DivSport = false;
            $scope.DivOther = true;
        }
        else {
            $scope.DivSport = true;
            $scope.DivOther = false;
        }
        if (FilledProfileLevel == 1) {
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;

            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
            $scope.SocietyIdForAdministrativeDetails = SID;
            $scope.SocietyDirectory = RegistrationNo;
            $scope.ActId = ActId;

            $("#SocietyStep_1").removeClass("active stepactive1");
            $("#SocietyStep_1").addClass("disabled stepactive1");
            $("#SocietyStep_2").addClass("active stepactive2");

            $("#step1").removeClass("tab-pane active");
            $("#step1").addClass("tab-pane ng-hide");
            $("#step2").removeClass("tab-pane ng-hide");
            $("#step2").addClass("tab-pane active");

        }
        else if (FilledProfileLevel == 2) {
            $scope.SocietyIdForAdministrativeDetails = SID;
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
            $("#SocietyStep_1").removeClass("active stepactive1");
            $("#SocietyStep_1").addClass("disabled stepactive1");

            $("#SocietyStep_2").removeClass("active stepactive1");
            $("#SocietyStep_2").addClass("disabled stepactive1");
            $("#SocietyStep_3").addClass("active stepactive3");
            $("#step2").removeClass("tab-pane active");
            $("#step2").addClass("tab-pane ng-hide");
            $("#step3").removeClass("tab-pane ng-hide");
            $("#step3").addClass("tab-pane active");




            $scope.SocietyRegisteredOffice.STATE_ID = '6';
            $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
            $scope.SocietyRegisteredOffice.STATE_IDSecretary = '6';
            $scope.SocietyRegisteredOffice.STATE_IDTreasurer = '6';

        }

    }

    $scope.UpdateLayout = function (FilledProfileLevel) {

        if (FilledProfileLevel == 1) {
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
        }
        else if (FilledProfileLevel == 2) {
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
        }
        else if (FilledProfileLevel == 3) {
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
        }
    }

    // START Show Society Details
    // START Show Society Details
    $scope.ApplicationView = function (SID) {
        if (SID != '') {
            window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + SID + "~_Citizen_SocietyProfile~undefined";
        }
    }

    $scope.SocietyAmendment = { IsAmendment: false, ChangeByeLaws: false, ChangeName: false, ChaneAddress: false, ChaneExecutiveMember: false };

    $scope.GetSocietyAmendmentDocuments = function (SID, AmendmentId) {
        $http.post(SiteUrl + "/Society/GetSocietyAmendmentDocuments", { SocietyIdEnc: SID, AmendmentId: AmendmentId }).
            success(function (data, status) {
                $scope.FileCollection = [];
                var documents = JSON.parse(data);
                documents.forEach(function (item, index) {
                    $scope.FileCollection.push({ 'Id': item.DocumentType, 'FileType': '', 'File': item.DocumentURL, 'Title': '', FileName: item.DocumentURL, FileId: item.DocumentType, IsFileUpload: true });
                });
            }).
            error(function (data, status) {
                $scope.FileCollection = [];
            });
    }

    $scope.ShowDetails = function (SID, IsFirst) {
        $scope.SocietyIdEnc = SID;
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = true;
        $(".blockUI").fadeIn("slow");
        //$http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
        $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId_v1", { "SocietyId": SID }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyDetailsData = [];
                if (data.length > 0) {
                    SocietyDataForEdit = data;
                    var BasicDetails = data[0];
                    var AdministrativeDetails = [];
                    var OfficeDeatils = data;
                    var IsBRNYes = '';

                   // console.log(BasicDetails);
                    if (BasicDetails.IsNewRegistration == true && BasicDetails.ActId == 3 && BasicDetails.SocietyTypeId == 9) {
                        $scope.showApex = true;
                    }
                    angular.forEach(data, function (item) {

                        AdministrativeDetails.push({
                            "Name": item.Name, "ContactNo": item.ContactNo, "Type": item.UserType, "SocietyId": item.SocietyId, "DOB": item.ContactNo, "DOB": item.ContactNo, "Photo": item.Photo, "AADHAARNo": item.AadharNo
                        });
                    });
                    $scope.SocietyAmendment.IsAmendment = false;
                    if (BasicDetails.IsAmendmentAtProfiling == true) {
                        if (BasicDetails.AmendmentType != undefined && BasicDetails.AmendmentType != "") {
                            $scope.SocietyAmendment.ChangeByeLaws = BasicDetails.AmendmentType.indexOf('1') > -1;
                            $scope.SocietyAmendment.ChangeName = BasicDetails.AmendmentType.indexOf('2') > -1;
                            $scope.SocietyAmendment.ChaneAddress = BasicDetails.AmendmentType.indexOf('3') > -1;
                            $scope.SocietyAmendment.ChaneExecutiveMember = BasicDetails.AmendmentType.indexOf('4') > -1;
                        }
                        $scope.SocietyAmendment.IsAmendment = true;
                        //$scope.GetSocietyAmendmentDocuments(SID, 0);
                    }
                    $scope.SocietyDetailsData.BasicDetails = [];
                    $scope.SocietyDetailsData.BasicDetails = BasicDetails;
                    $scope.SocietyDetailsData.OfficeDeatils = OfficeDeatils;
                    $scope.BasciDetailsIsPermanentSave = BasicDetails.BasciDetailsIsPermanentSave;
                    $scope.AdminDetailsIsPermanentSave = BasicDetails.AdminDetailsIsPermanentSave;
                    $scope.AddressDetailsIsPermanentSave = BasicDetails.AddressDetailsIsPermanentSave;




                    $scope.GetUplodedOthersDocuments($scope.SocietyDetailsData.BasicDetails.SocietyID, $scope.SocietyDetailsData.BasicDetails.SocietyDirectory);





                    TableData10 = [];
                    var temp = 0;
                    $scope.SocietyDetailsData.BasicDetails.BRN = $scope.SocietyDetailsData.BasicDetails.BRN == null ? "" : $scope.SocietyDetailsData.BasicDetails.BRN;
                    if ($scope.SocietyDetailsData.BasicDetails.ActId != 7) {
                        if ($scope.SocietyDetailsData.BasicDetails.BRN.length > 0 && ($scope.SocietyDetailsData.BasicDetails.BRN != undefined || $scope.SocietyDetailsData.BasicDetails.BRN != "")) {
                            //$scope.divBrnYN = true;
                            $scope.IsBRNYes = true;
                        }
                        else {
                            // $scope.divBrnYN = false;
                            $scope.IsBRNYes = false;
                        }

                        var X = $scope.SocietyDetailsData.BasicDetails.ActId == 1 ? 8 : 11;
                        for (var x = 1; x < X; x++) {
                            TableData10.push(
                                {
                                    "Id": x,
                                    "POSTNAME": '',
                                    "PostId": '0',
                                    "Photo": '../Content/Images/Balnk.png',
                                    "CatId": "0",
                                    "CategoryId": "0"
                                });
                        }

                        angular.forEach(TableData10, function (item) {
                            if ($scope.SocietyDetailsData.BasicDetails.ActId == '1') {
                                if (temp == 0) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'अध्यक्ष/सभापति');
                                }
                                else if (temp == 1) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष');
                                }
                                else if (temp == 2) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY' : 'सचिव');
                                }
                                else if (temp == 3) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'TREASURER' : 'कोषाध्यक्ष');
                                }
                                else if (temp == 4) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'DIRECTOR MEMBER' : 'निदेशक सदस्य');
                                }
                            }
                            else if ($scope.SocietyDetailsData.BasicDetails.ActId == '3') {
                                if (temp == 0) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN' : 'अध्यक्ष')
                                }
                                else if (temp == 1) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                                }
                                else if (temp == 2) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                                }
                                else if (temp == 3) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'DIRECTOR' : 'निदेशक')
                                }
                            }

                            temp = temp + 1;
                        });
                        $scope.TableData10 = TableData10;
                    }
                    else {
                        var X = 1003;
                        for (var x = 1001; x < X; x++) {
                            TableData10.push(
                                {
                                    "Id": x,
                                    "POSTNAME": '',
                                    "PostId": '0',
                                    "Photo": '../Content/Images/Balnk.png',
                                    "CatId": "0",
                                    "CategoryId": "0"
                                });
                        }
                        angular.forEach(TableData10, function (item) {
                            if ($scope.SocietyDetailsData.BasicDetails.ActId == '7') {
                                if (temp == 0) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'Chairman' : 'अध्यक्ष');
                                    item.PostId = '1016';
                                }
                                else if (temp == 1) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                                    item.PostId = '1015';
                                }
                                else if (temp == 2) {
                                    item.POSTNAME = ($scope._language == 'en-us' ? 'Treasurer' : 'कोषाध्यक्ष')
                                    item.PostId = '1011';
                                }
                            }
                        });
                        $scope.TableSportsData2 = TableData10;
                    }

                    if ($scope.SocietyDetailsData.BasicDetails.ActId == 1) {
                        $scope.Showforactidfirst = false;
                    }
                    else if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                        $scope.Showforactidfirst = true;
                        $scope.SocietyDetailsData.BasicDetails.RegistrationDate = $scope.SocietyDetailsData.BasicDetails.RegistrationDate == null ? "" : $scope.SocietyDetailsData.BasicDetails.RegistrationDate;
                        $scope.SocietyDetailsData.BasicDetails.ElectionDate = $scope.SocietyDetailsData.BasicDetails.ElectionDate == null ? "" : $scope.SocietyDetailsData.BasicDetails.ElectionDate;
                        $scope.SocietyDetailsData.BasicDetails.AGMDate = $scope.SocietyDetailsData.BasicDetails.AGMDate == null ? "" : $scope.SocietyDetailsData.BasicDetails.AGMDate
                        $scope.SocietyDetailsData.BasicDetails.RegistrationDate = $scope.SocietyDetailsData.BasicDetails.RegistrationDate.indexOf('9999') > -1 ? '' : $scope.SocietyDetailsData.BasicDetails.RegistrationDate;
                        $scope.SocietyDetailsData.BasicDetails.ElectionDate = $scope.SocietyDetailsData.BasicDetails.ElectionDate.indexOf('9999') > -1 ? '' : $scope.SocietyDetailsData.BasicDetails.ElectionDate;
                        $scope.SocietyDetailsData.BasicDetails.AGMDate = $scope.SocietyDetailsData.BasicDetails.AGMDate.indexOf('9999') > -1 ? '' : $scope.SocietyDetailsData.BasicDetails.AGMDate;
                    }

                    $scope.ActId = $scope.SocietyDetailsData.BasicDetails.ActId;
                    var newdab = data.filter(function (x) { return x.IsSameCEOAddress == true && x.AddressFor == 0 })

                    $scope.SocietyDetailsData.OfficeDeatils.dtab = newdab.length > 0 ? newdab[0] : 'N/A';

                    var newdab = data.filter(function (x) { return x.AddressFor == 9 && x.IsSameCEOAddress == false })

                    $scope.SocietyDetailsData.OfficeDeatils.dtabnew = newdab.length > 0 ? newdab[0] : 'N/A';

                    var sachiv = data.filter(function (x) { return x.AddressFor == 11 })
                    $scope.SocietyDetailsData.OfficeDeatils.Newscetrat = sachiv.length > 0 ? sachiv[0] : 'N/A';

                    var Treasurer = data.filter(function (x) { return x.AddressFor == 12 })
                    $scope.SocietyDetailsData.OfficeDeatils.newTreasurer = Treasurer.length > 0 ? Treasurer[0] : 'N/A';

                    var AdministratorDetails = data.filter(function (x) { return x.UserType == 3 });//Appointed Administrator
                    $scope.SocietyDetailsData.Administrator = AdministratorDetails.length > 0 ? AdministratorDetails[0] : [];

                    $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed = AdministratorDetails.length > 0 ? $scope.translation.Yes : $scope.translation.No;
                    $scope.IsAdministratorAppointed = AdministratorDetails.length > 0 ? true : false;

                    $scope.BtnAdminDetailShowhide = true;

                    MemberDetails = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') && (x.IsSameCEOAddress == true || x.IsSameCEOAddress == true) });
                    if (MemberDetails.length == 0) {
                        MemberDetails = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                    }

                    $scope.SocietyDetailsData.MemberDetails = MemberDetails;

                    if (MemberDetails.length > 0) {
                        $scope.hidemenber = true;
                    }
                    else {
                        $scope.hidemenber = false;
                    }

                    if ($scope.SocietyDetailsData.Administrator.IsAdministratorAppointed == 'हाँ' || $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed == 'Yes') {
                        $scope.IsAppointed = true;
                    }

                    if ($scope.SocietyDetailsData.BasicDetails.ActId == 3) {
                        NOMINATEDMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'NOMINATEDMEMBERS') && x.IsSameCEOAddress == true });
                        if (NOMINATEDMEMBERS.length == 0) {
                            NOMINATEDMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'NOMINATEDMEMBERS') });
                        }
                        if (NOMINATEDMEMBERS.length == 0) {
                            NOMINATEDMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'NOMINATEDMEMBERS') });
                        }
                        if (NOMINATEDMEMBERS.length > 0) {
                            $scope.IsNOMINATEDMEMBERS = true;
                            $scope.NOMINATEDMEMBERS = NOMINATEDMEMBERS;
                        }
                    }

                    $scope.SocietyDetailsData.EMD = data.filter(function (x) { return (x.MEMBERTYPE == 'ELECTEDMEMBERS') && x.IsSameCEOAddress == false });

                    if ($scope.SocietyDetailsData.EMD.length == 0) {
                        $scope.SocietyDetailsData.EMD = data.filter(function (x) { return (x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                    }

                    if ($scope.SocietyDetailsData.EMD.length == 0)
                        $scope.TableEMD = 0;
                    else
                        $scope.TableEMD = 1;

                    //start sport View
                    var sportChairmanDetails = data.filter(function (x) { return x.UserType == 1 && x.ActId == 7 });//CEO
                    $scope.SocietyDetailsData.ChairmanSport = sportChairmanDetails.length > 0 ? sportChairmanDetails[0] : [];

                    var SportsectrateDetails = data.filter(function (x) { return x.UserType == 2 && x.ActId == 7 });//sport sec
                    $scope.SocietyDetailsData.sectratesport = SportsectrateDetails.length > 0 ? SportsectrateDetails[0] : [];

                    var SporttrasDetails = data.filter(function (x) { return x.UserType == 3 && x.ActId == 7 });//sport sec
                    $scope.SocietyDetailsData.SportTrasurrdtsDetails = SporttrasDetails.length > 0 ? SporttrasDetails[0] : [];

                    var sportMemberDetails = data.filter(function (x) { return x.MEMBERTYPE == 'ELECTEDMEMBERS' && x.ActId == 7 && x.AddressFor == 0 });//Elected /Nomineted
                    if (sportMemberDetails.length == 0) {
                        sportMemberDetails = data.filter(function (x) { return x.MEMBERTYPE == 'ELECTEDMEMBERS' && x.ActId == 7 });//Elected /Nomineted
                    }

                    $scope.SocietyDetailsData.sportMemberDetails = sportMemberDetails;

                    var sporttrasurerDetails = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') && x.ActId == 7 && x.AddressFor == 0 });//Elected /Nomineted
                    if (sporttrasurerDetails.length == 0) {
                        sporttrasurerDetails = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') && x.ActId == 7 });//Elected /Nomineted
                    }

                    $scope.SocietyDetailsData.sporttrasurerDetails = sporttrasurerDetails;

                    $scope.SocietyDetailsData.SportTrasurrdtsDetails.SocietyIdEnc = SID;
                    //End sport view
                    var liqDetails = data.filter(function (x) { return x.UserType == 6 });//LIQvideter 
                    $scope.SocietyDetailsData.liq = liqDetails.length > 0 ? liqDetails[0] : [];

                    if ($scope.SocietyDetailsData.BasicDetails.UserType == 6) {
                        $scope.ShowLiquidator = true;
                        $scope.SubOtherDivShow = false;

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
                        $scope.Hidemenbers = false;
                    }


                    var liqDetails = data.filter(function (x) { return x.UserType == 6 });
                    $scope.SocietyDetailsData.liq = liqDetails.length > 0 ? liqDetails[0] : [];
                    if ($scope.SocietyDetailsData.BasicDetails.IsRural == "Urban" || $scope.SocietyDetailsData.BasicDetails.IsRuralUrban == "0") {
                        $scope.divCity = true;
                        $scope.divBlock = false;
                        $scope.divWard = true;
                        $scope.divGP = false;
                        $scope.divVillage = false;
                        $scope.Showviewdat = false;
                        $scope.ShowviewdatU = true;
                    }
                    else if ($scope.SocietyDetailsData.BasicDetails.IsRural == "Rural" || $scope.SocietyDetailsData.BasicDetails.IsRuralUrban == "1") {
                        $scope.divCity = false;
                        $scope.divBlock = true;
                        $scope.divWard = false;
                        $scope.divGP = true;
                        $scope.divVillage = true;
                        $scope.Showviewdat = true;
                        $scope.ShowviewdatU = false;
                    }


                    if (($scope.SocietyDetailsData.BasicDetails.IsNewRegistration == false || $scope.SocietyDetailsData.BasicDetails.IsNewRegistration == false) && $scope.ActId == 3) {
                        $scope.ViewTwoThousandOneProfilling = true;
                    }
                    else {
                        $scope.ViewTwoThousandOneProfilling = false;
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

                if ($scope.SocietyDetailsData.BasicDetails.IsAffiliationBySLSA == "Yes") {
                    $scope.IsAffiliatedBySLSA = true;
                }
                else {
                    $scope.IsAffiliatedBySLSA = false;
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
                //setTimeout(function () { $scope.SetUplodedDocuments(BasicDetails); }, 5000);

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
            });

    }

    $scope.SetUplodedDocuments = function (BasicDetails) {
        $scope.FileCollection = [];
        $scope.FileCollection.push({ 'Id': 'flRegistration_Certificate', 'FileType': '', 'File': BasicDetails.RegistrationCertificateFile, 'Title': '', FileName: BasicDetails.RegistrationCertificateFile, FileId: 'flRegistration_Certificate', IsFileUpload: $scope.IsProfiling ? BasicDetails.RegistrationCertificateFile != '' && BasicDetails.RegistrationCertificateFile != undefined && BasicDetails.RegistrationCertificateFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flBy_laws', 'FileType': '', 'File': BasicDetails.BylawsFile, 'Title': '', FileName: BasicDetails.BylawsFile, FileId: 'flBy_laws', IsFileUpload: $scope.Act2001 || $scope.SocietyRegistration.ActId == 7 ? BasicDetails.BylawsFile != undefined && BasicDetails.BylawsFile != '' && BasicDetails.BylawsFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flRulesAnd_Regulation', 'FileType': '', 'File': BasicDetails.RulesAndRegulationFile, 'Title': '', FileName: BasicDetails.RulesAndRegulationFile, FileId: 'flRulesAnd_Regulation', IsFileUpload: $scope.Act1958 ? BasicDetails.RulesAndRegulationFile != undefined && BasicDetails.RulesAndRegulationFile != '' && BasicDetails.RulesAndRegulationFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flSLSCAffiliation_Certificates', 'FileType': '', 'File': BasicDetails.RSSCFileName, 'Title': '', FileName: BasicDetails.RSSCFileName, FileId: 'flSLSCAffiliation_Certificates', IsFileUpload: $scope.SocietyRegistration.ActId == 7 && $scope.LevelState ? BasicDetails.RSSCFileName != undefined && BasicDetails.RSSCFileName != '' && BasicDetails.RSSCFileName != null : false });
        $scope.FileCollection.push({ 'Id': 'flDLSCAffiliation_Certificates', 'FileType': '', 'File': BasicDetails.DLSCFileName, 'Title': '', FileName: BasicDetails.DLSCFileName, FileId: 'flDLSCAffiliation_Certificates', IsFileUpload: $scope.SocietyRegistration.ActId == 7 && $scope.FileDLSC ? BasicDetails.DLSCFileName != undefined && BasicDetails.DLSCFileName != '' && BasicDetails.DLSCFileName != null : false });
        $scope.FileCollection.push({ 'Id': 'flSOCAffiliation_Certificate', 'FileType': '', 'File': BasicDetails.SOCFileName, 'Title': '', FileName: BasicDetails.SOCFileName, FileId: 'flSOCAffiliation_Certificate', IsFileUpload: $scope.SocietyRegistration.ActId == 7 && $scope.IsAffiliatedBySOC ? BasicDetails.SOCFileName != undefined && BasicDetails.SOCFileName != '' && BasicDetails.SOCFileName != null : false });
        $scope.FileCollection.push({ 'Id': 'flSLSAffiliation_Certificates', 'FileType': '', 'File': BasicDetails.SLSAFileName, 'Title': '', FileName: BasicDetails.SLSAFileName, FileId: 'flSLSAffiliation_Certificates', IsFileUpload: $scope.SocietyRegistration.ActId == 7 && $scope.FileSLSA ? BasicDetails.SLSAFileName != undefined && BasicDetails.SLSAFileName != '' && BasicDetails.SLSAFileName != null : false });
        $scope.FileCollection.push({ 'Id': 'flUploadOldExecutive_Committee', 'FileType': '', 'File': BasicDetails.OldExecutiveCommittee, 'Title': '', FileName: BasicDetails.OldExecutiveCommittee, FileId: 'flUploadOldExecutive_Committee', IsFileUpload: $scope.Act1958 && !$scope.SocietyRegistration.IsNewRegistration ? BasicDetails.OldExecutiveCommittee != undefined && BasicDetails.OldExecutiveCommittee != '' && BasicDetails.OldExecutiveCommittee != null : false });
        $scope.FileCollection.push({ 'Id': 'flUploadMembers_List', 'FileType': '', 'File': BasicDetails.MemberListFile, 'Title': '', FileName: BasicDetails.MemberListFile, FileId: 'flUploadMembers_List', IsFileUpload: BasicDetails.MemberListFile != undefined && BasicDetails.MemberListFile != '' && BasicDetails.MemberListFile != null });
        $scope.FileCollection.push({ 'Id': 'flSocietyWorkPlanEconomic_Viability', 'FileType': '', 'File': BasicDetails.WorkPlanEcoViabilityFile, 'Title': '', FileName: BasicDetails.WorkPlanEcoViabilityFile, FileId: 'flSocietyWorkPlanEconomic_Viability', IsFileUpload: $scope.Act2001 ? BasicDetails.WorkPlanEcoViabilityFile != undefined && BasicDetails.WorkPlanEcoViabilityFile != '' && BasicDetails.WorkPlanEcoViabilityFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flGeneralMeeting_Procedure', 'FileType': '', 'File': BasicDetails.GeneralMeetingProcedureFile, 'Title': '', FileName: BasicDetails.GeneralMeetingProcedureFile, FileId: 'flGeneralMeeting_Procedure', IsFileUpload: $scope.Act2001 ? BasicDetails.GeneralMeetingProcedureFile != undefined && BasicDetails.GeneralMeetingProcedureFile != '' && BasicDetails.GeneralMeetingProcedureFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flBalanceSheet_Upload', 'FileType': '', 'File': BasicDetails.BalanceSheetExcelFile, 'Title': '', FileName: BasicDetails.BalanceSheetExcelFile, FileId: 'flBalanceSheet_Upload', IsFileUpload: $scope.Act2001 ? BasicDetails.BalanceSheetExcelFile != undefined && BasicDetails.BalanceSheetExcelFile != '' && BasicDetails.BalanceSheetExcelFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flUpload_Objects', 'FileType': '', 'File': BasicDetails.ObjectsFile, 'Title': '', FileName: BasicDetails.ObjectsFile, FileId: 'flUpload_Objects', IsFileUpload: $scope.Act1958 ? BasicDetails.ObjectsFile != undefined && BasicDetails.ObjectsFile != '' && BasicDetails.ObjectsFile != null : false });
        $scope.FileCollection.push({ 'Id': 'flCooperatives_List', 'FileType': '', 'File': BasicDetails.DLSCFileName, 'Title': '', FileName: BasicDetails.DLSCFileName, FileId: 'flCooperatives_List', IsFileUpload: $scope.Act2001 ? BasicDetails.DLSCFileName != undefined && BasicDetails.DLSCFileName != '' && BasicDetails.DLSCFileName != null : false });
    }


    $scope.GetUplodedOthersDocuments = function (SocietyId, SocietyDirectory) {
        $(".blockUI").fadeIn("slow");
        $scope.SocietyOthersDocuments = [];
        $http.post(SiteUrl + "/Society/GetSocietyOthersDocuments", { "SocietyId": SocietyId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                data.DataList.forEach(function (item, index) {
                    $scope.SocietyOthersDocuments.push({ Id: index, Name: "/SocietyProfiling/" + SocietyDirectory + "/" + item.DocumentURL, FileId: item.Id, IsFileUpload: true, FileName: item.DocumentName });
                });
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
            });
    }

    $scope.DeleteOtherDocument = function (index, id) {
        ShowAlertMessage("Alert", "Are you sure you want to delete this document", function () {
            $scope.SocietyOthersDocuments.splice(index, 1);
            $http.post(SiteUrl + "/Society/DeleteOtherDocument", { "Id": id }).
                success(function (data, status) {
                    //$(".blockUI").fadeOut("slow");                   
                }).
                error(function (data, status) {
                    //$(".blockUI").fadeOut("slow");                  
                });

        });

    }


    // END Show Society Details

    // END Show Society Details
    // START Show Workscope Details
    $scope.ShowDetailsWorkscope = function (SID, ValidationFlag) {
        if ($scope.lstSocietyWorkScope.length == 0) {
            $(".blockUI").fadeIn("slow");
            $http.post(SiteUrl + "/Society/GetWorkscopeBySocietyId_v1", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.WorkscopeDetailsData = [];
                    if (data.length > 0) {
                        $scope.lstSocietyWorkScope = data;
                        if ($scope.lstSocietyWorkScope.length > 0) {
                            $scope.SocietyRegistration.AreaOfOperation = $scope.lstSocietyWorkScope[0].AreaOfOperation;
                            $scope.SocietyRegistration.WorkScopeLevel_ID = $scope.lstSocietyWorkScope[0].WorkScopeLevel_ID;
                        }
                        else {
                            $scope.grid = false;
                        }

                        //alert(JSON.stringify($scope.lstSocietyWorkScope));
                        $scope.singlearray = data[0];

                        if ($scope.singlearray.IsRSSC == true) {
                            $scope.TrueDivForPrimarybody = false;
                            $scope.hidefilediv = false;
                        }
                        else {
                            $scope.TrueDivForPrimarybody = true;
                            $scope.hidefilediv = true;
                        }
                        if ($scope.singlearray.IsRuralUrban == "0" || $scope.singlearray.IsRuralUrban == false) {

                            $scope.IsStatelevelRural = true;
                            $scope.IsStatelevelRuralNO = false;
                            $scope.IsStatelevelF = false;
                            $scope.IsRuralUworkscope = true;
                            $scope.IsUrbanRworkscope = false;
                            $scope.RuralDivRadio = true;
                        }
                        else if ($scope.singlearray.IsRuralUrban == "1" || $scope.singlearray.IsRuralUrban == true) {
                            $scope.IsStatelevelRuralNO = false;
                            $scope.IsStatelevelRural = false;
                            $scope.IsStatelevelF = true;
                            $scope.IsRuralUworkscope = false;
                            $scope.IsUrbanRworkscope = true;
                            $scope.RuralDivRadio = false;
                        }

                        if ($scope.singlearray.IsStatLevel == '0' && $scope.SocietyRegistration != undefined) {
                            $scope.SocietyRegistration.IsStatLevel = 'No';
                            //$scope.Addmorebtn(ValidationFlag);
                            $scope.deletebtnhide = false;

                            $scope.grid = false;


                            if ($scope.singlearray.IsRuralUrban == '1') {
                                $scope.SocietyRegistration.Rural = true;
                                $scope.RuralDivRadio = true;
                                if ($scope.ActId == 7) {
                                    $scope.IsStatelevelF = true;
                                    $scope.IsStatelevelRural = false;
                                }
                                else {
                                    $scope.IsStatelevelF = false;;
                                }
                            }
                            else {
                                $scope.SocietyRegistration.Rural = false;
                                $scope.RuralDivRadio = false;
                            }
                            $scope.grid = true;
                            $scope.InCaseofupdate = true;
                            //$scope.gridtwo = false;
                            // $scope.Addmorebtn();

                        }
                        else if ($scope.singlearray.IsStatLevel == '1') {
                            $scope.SocietyRegistration.IsStatLevel = 'Yes';
                            $scope.RuralDivRadio = false;
                        }
                        if ($scope.lstSocietyWorkScope.length > 0) {
                            $scope.DataYes = true;
                            if ($scope.ActId == 7 || $scope.ActId == '7') {
                                $scope.SocietyRegistration.DISTRICT_ID = $scope.lstSocietyWorkScope[0].DISTRICT_ID;
                                $scope.SocietyDetailsData.BasicDetails.DISTRICT_ENG = $scope.lstSocietyWorkScope[0].DISTRICT_Name;
                                $scope.SocietyDetailsData.BasicDetails.DISTRICT_MANGAL = $scope.lstSocietyWorkScope[0].DISTRICT_MANGAL;
                                $scope.BlockList();
                                if ($scope.singlearray.IsStatLevel == '0') {
                                    $scope.grid = false;
                                }
                            }
                        }
                        else {
                            $scope.DataYes = false;
                        }
                    }
                    else {
                        //$scope.SocietyDetailsData.WorkscopeDetailsData = [];
                        $scope.grid = false;
                    }


                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    alert("Error while fetching Society Details.");
                });
        }
    }
    // END Show Workscope Details


    // START Back To Page
    $scope.IsBackToSearchList = false;
    $scope.BackToPage = function () {

        if ($scope.IsBackToSearchList) {
            $scope.IsShowSearchSociety = true;
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


            $('#ISrurl')[0].checked = false;
            $('#ISUrban')[0].checked = false;
            $scope.grid = false;


        } else {
            $scope.divTabulation = true;
            $scope.divAddBtn = true;
            $scope.divSocialProfiling = false;
            $scope.divSocietyProfilingShow = false;
            $scope.IsBackToSearchList = true;
        }
        $scope.IsBackToSearchList = false;



    }
    // START Back To Page
    //START



    // START : Save Society Administrative Details
    $scope.SaveSocietyAdministrativeDetails = function (SubmitStatus) {
        $scope.SubmitStatus = SubmitStatus;
        if ($scope.SocietyRegistration.ActId == undefined && $scope.ActId > 0) {
            $scope.SocietyRegistration.ActId = $scope.ActId;
        }
        $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
        $scope.SocietyRegistration.ActId = $scope.ActId;
        if ($scope.SocietyRegistration.ActId != "7") {
            if ($scope.AdministrativeFormValidation(SubmitStatus)) {


                if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                    
                    if (!$scope.CategoryGenderFormValidation()) {
                        $(".se-pre-con").fadeOut("slow");
                        return;
                    }
                }
                var SocietyRegistrationData = [];
                var formData = new FormData();
                $(".se-pre-con").fadeIn("slow");
                formData.append("ActId", $scope.SocietyRegistration.ActId);
                formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);
                formData.append("ClassA", $scope.SocietyRegistration.TotalMembersClassA);
                formData.append("ClassB", $scope.SocietyRegistration.TotalMembersClassB);
                formData.append("ClassC", $scope.SocietyRegistration.TotalMembersClassC);
                formData.append("SocietyDirectory", $scope.SocietyDirectory);
                formData.append("SubmitStatus", $scope.SubmitStatus);
                $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;


                if ($scope.SocietyRegistration.IsAdministrative == 'Administrative') {
                    SocietyRegistrationData.push(
                        {
                            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "Name": $scope.SocietyRegistration.AppointedName,
                            "IsActive": true,
                            "ContactNo": '',
                            "AADHAARNo": '121212121212',
                            "DOB": $('#txtAppointedDOB').val().split("/").reverse().join("-"),
                            "AppointedDate": $('#txtAppointedDOB').val().split("/").reverse().join("-"),
                            "Gender": $scope.SocietyRegistration.AppointedGender,
                            "UserType": "3",
                            "AdministratorDesignationId": "-1",
                            "CategoryId": 0,
                            "PostId": 0,
                            "PostName": $scope.SocietyRegistration.AppointedPostName,
                            "Photo": '../Content/Images/Balnk.png',
                            "MEMBERTYPE": "ADMINISTRATIVE",
                            "SubmitStatus": $scope.SubmitStatus
                        });
                }

                if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
                    SocietyRegistrationData.push(
                        {
                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "Name": $scope.SocietyRegistration.liquidatorName,
                            "IsActive": true,
                            "ContactNo": $scope.SocietyRegistration.liquidatorContactNo,
                            "AADHAARNo": '',
                            "DOB": '',
                            "AppointedDate": $('#txtliquidatorAppointedDate').val().split("/").reverse().join("-"),
                            "Gender": '',
                            "UserType": "6",
                            "AdministratorDesignationId": "-1",
                            "CategoryId": 0,
                            "PostId": 0,
                            "PostName": $scope.SocietyRegistration.liquidatorPostName,
                            "Photo": '../Content/Images/Balnk.png',
                            "MEMBERTYPE": "LIQUIDATOR",
                            "SubmitStatus": $scope.SubmitStatus
                        });
                }
                if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                    var Count = 0;
                    $('#TblElectedMember tr').each(function () {
                        if ($(this).find('td:eq(10)').find('input').val() != '' && $(this).find('td:eq(9)').find('select').val() > 0 && $(this).find('td:eq(3)').find('input').val() != '') {

                            if (($scope.SocietyRegistration.IsNewRegistration == false) && $scope.Act2001) {
                                SocietyRegistrationData.push(
                                    {
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(3)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": $(this).find('td:eq(13)').find('input').val(),
                                        "ShareAllotted": $(this).find('td:eq(14)').find('input').val(),
                                        "ShareCapitalAmount": $(this).find('td:eq(15)').find('input').val(),
                                        "loaneeType": $(this).find('td:eq(16)').find('select').val(),
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                            else if (($scope.SocietyRegistration.IsNewRegistration == true) && $scope.Act2001 && $scope.SocietyRegistration.SocietyTypeId == 9) {
                                SocietyRegistrationData.push({
                                    //RamTest
                                    "NameOfSociety": $(this).find('td:eq(1)').find('input').val(),
                                    "PostinSociety": $(this).find('td:eq(2)').find('input').val(),


                                    "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                    "Name": $(this).find('td:eq(4)').find('input').val(),
                                    "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                    "Address": $(this).find('td:eq(3)').find('label').html(),
                                    "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                    "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                    "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                    "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                    "PostId": '0',
                                    "PostName": $(this).find('td:eq(10)').find('input').val(),
                                    "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                    "MembershipNo": '',
                                    "ShareAllotted": 0,
                                    "ShareCapitalAmount": 0,
                                    "loaneeType": 0,
                                    "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                    "SubmitStatus": $scope.SubmitStatus
                                });

                            }
                            else {
                                SocietyRegistrationData.push(

                                    {
                                        "NameOfSociety": $(this).find('td:eq(1)').find('input').val(),
                                        "PostinSociety": $(this).find('td:eq(2)').find('input').val(),
                                        "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                        "Name": $(this).find('td:eq(4)').find('input').val(),
                                        "FatherName": $(this).find('td:eq(5)').find('input').val(),
                                        "Address": $(this).find('td:eq(3)').find('label').html(),
                                        "ContactNo": $(this).find('td:eq(6)').find('input').val(),
                                        "DOB": $(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-"),
                                        "Gender": ($(this).find('td:eq(8)').find('label').html()).charAt(0),//text
                                        "CategoryId": $(this).find('td:eq(9)').find('select').val(),
                                        "PostId": '0',
                                        "PostName": $(this).find('td:eq(10)').find('input').val(),
                                        "Photo": ($(this).find('td:eq(11)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(12)').find('input').val(),
                                        "MembershipNo": '',
                                        "ShareAllotted": 0,
                                        "ShareCapitalAmount": 0,
                                        "loaneeType": 0,
                                        "MEMBERTYPE": ($(this).find('td:eq(17)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                        }

                    });

                    $('#TblNominatedMember tr').each(function () {
                        if ($(this).find('td:eq(1)').find('input').val() != '' && $(this).find('td:eq(7)').find('select').val() > 0 && $(this).find('td:eq(9)').find('input').val() != '') {
                            SocietyRegistrationData.push(
                                {
                                    "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                    "Name": $(this).find('td:eq(2)').find('input').val(),
                                    "FatherName": $(this).find('td:eq(3)').find('input').val(),
                                    "Address": $(this).find('td:eq(3)').find('label').html(),
                                    "ContactNo": $(this).find('td:eq(4)').find('input').val(),
                                    "DOB": $(this).find('td:eq(5)').find('input').val().split("/").reverse().join("-"),
                                    "Gender": ($(this).find('td:eq(6)').find('label').html()).charAt(0),//text
                                    "CategoryId": $(this).find('td:eq(7)').find('select').val(),
                                    "PostId": '0',
                                    "PostName": $(this).find('td:eq(8)').find('input').val(),
                                    "NominatedBy": $(this).find('td:eq(9)').find('select').val(),
                                    "Photo": ($(this).find('td:eq(10)').find('img'))[0].src,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "5",//Nominated board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": "NOMINATEDMEMBERS",
                                    "SubmitStatus": $scope.SubmitStatus

                                });
                        }

                    });
                }
                formData.append("BoardType", $scope.SocietyRegistration.IsAdministrative);
                formData.append("DataArray", JSON.stringify(SocietyRegistrationData));

                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetails", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == '-1') {
                            alert('Some Error Occured')
                        }
                        else {
                            if ((data.split('@')).length > 1) {
                                $scope.EMitraTransactionId = (data.split('@')).length > 1 ? (data.split('@'))[1] : '0';
                                $scope.SocietyRegistration.SocietyIdEnc = data.split('@')[0];
                                $scope.SocietyIdEnc = data.split('@')[0];
                                ShowModelAlert("Society Profiling", "Society Administrative Details Update Sucessfully");
                                var $active = $('.wizard .nav-tabs li.active');
                                prevTab($active);
                                $active.next().removeClass('disabled');
                                var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                                if ($scope.IsEditMode == false) {
                                    //$scope.ActId = $scope.SocietyRegistration.ActId;
                                    $scope.SocietyDirectory = data.societydirectory;
                                    $scope.SocietyIdForAdministrativeDetails = data.data;
                                    ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                                    $scope.divSocietyProfilingShow = false;
                                    $scope.divAddBtn = false;
                                    $scope.lstSocietyWorkScope = [];

                                    $("#SocietyStep_1").addClass("disabled stepactive1");
                                    $("#SocietyStep_2").removeClass("active stepactive1");
                                    $("#SocietyStep_2").addClass("disabled stepactive1");
                                    $("#SocietyStep_3").addClass("active stepactive2");


                                    $("#step1").addClass("tab-pane ng-hide");
                                    $("#step2").removeClass("tab-pane active");
                                    $("#step2").addClass("tab-pane ng-hide");
                                    $("#step3").addClass("tab-pane active");

                                    if ($scope.ActId != "7") {
                                        $scope.DivOther = true;
                                        $scope.DivSport = false;
                                    }
                                    else {
                                        $scope.DivOther = false;
                                        $scope.DivSport = true;
                                    }
                                }
                                else {
                                    $scope.ClearSocietyAdministrativeDetails();
                                    $scope.divSocietyProfilingShow = true;

                                    var $active = $('.wizard .nav-tabs li.active');
                                    prevTab($active);
                                    $active.next().removeClass('disabled');
                                    var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                                    $scope.divSocialProfiling = false;


                                    $("#SocietyStep_22").removeClass("active stepactive1");
                                    $("#SocietyStep_22").addClass("disabled stepactive1");
                                    $("#SocietyStep_33").addClass("active stepactive3");

                                    $("#step22").removeClass("tab-pane active");
                                    $("#step22").addClass("tab-pane ng-hide");
                                    $("#step33").removeClass("tab-pane ng-hide");
                                    $("#step33").addClass("tab-pane active");


                                    $('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });

                                }
                            }
                            else {
                                $scope.ClearSocietyAdministrativeDetails();
                                ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                                if ($scope.IsEditMode == false) {
                                    $scope.SocietyDirectory = data.societydirectory;
                                    $scope.SocietyIdForAdministrativeDetails = data.data;
                                    ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                                    $scope.divSocietyProfilingShow = false;
                                    $scope.divAddBtn = false;
                                    $scope.lstSocietyWorkScope = [];

                                    $("#SocietyStep_1").addClass("disabled stepactive1");
                                    $("#SocietyStep_2").removeClass("active stepactive1");
                                    $("#SocietyStep_2").addClass("disabled stepactive1");
                                    $("#SocietyStep_3").addClass("active stepactive2");


                                    $("#step1").addClass("tab-pane ng-hide");
                                    $("#step2").removeClass("tab-pane active");
                                    $("#step2").addClass("tab-pane ng-hide");
                                    $("#step3").addClass("tab-pane active");

                                    if ($scope.ActId != "7") {
                                        $scope.DivOther = true;
                                        $scope.DivSport = false;
                                    }
                                    else {
                                        $scope.DivOther = false;
                                        $scope.DivSport = true;
                                    }
                                }

                            }
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        $scope.DistrictList = [];
                    });

            }
        }
        else {
            //&& $scope.ValidateNominatedMember(2, true)
            var tempchaek = SubmitStatus == 0 ? false : true;
            if ($scope.ValidateNominatedMember(1, tempchaek)) {
                var SocietyRegistrationData = [];
                var formData = new FormData();
                $(".se-pre-con").fadeIn("slow");
                formData.append("ActId", $scope.SocietyRegistration.ActId);
                formData.append("CEOSocietyFile", $scope.CEOSocietyFile);
                formData.append("ChairmanFile", $scope.ChairmanFile);
                formData.append("AppointedFile", $scope.AppointedFile);
                formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);
                formData.append("SubmitStatus", $scope.SubmitStatus);

                var formDataLocal = new FormData();

                formData.append("ClassA", 0);
                formData.append("ClassB", 0);
                formData.append("ClassC", 0);
                formData.append("SocietyDirectory", $scope.SocietyDirectory);
                //अध्यक्ष
                $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdForAdministrativeDetails.split('@')[0];
                $('#TblSportElectedMember tr').each(function () {
                    if ($(this).find('td:eq(1)').find('input').val() != '' && $(this).find('td:eq(7)').find('select').val() > 0) {
                        SocietyRegistrationData.push(
                            {
                                "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                "Name": $(this).find('td:eq(2)').find('input').val(),
                                "FatherName": $(this).find('td:eq(3)').find('input').val(),
                                "ContactNo": $(this).find('td:eq(4)').find('input').val(),
                                "DOB": $(this).find('td:eq(5)').find('input').val().split("/").reverse().join("-"),
                                "Gender": ($(this).find('td:eq(6)').find('label').html()).charAt(0),//text
                                "CategoryId": $(this).find('td:eq(7)').find('select').val(),
                                "PostId": '0',
                                "PostName": $(this).find('td:eq(8)').find('input').val(),
                                "Address": $(this).find('td:eq(9)').find('textarea').val(),
                                "Photo": ($(this).find('td:eq(10)').find('img'))[0].src,
                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "IsActive": true,
                                "UserType": "4",//Elected board Menbers
                                "AdministratorDesignationId": "-1",
                                "MEMBERTYPE": ($(this).find('td:eq(11)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                "SubmitStatus": $scope.SubmitStatus
                            });
                    }
                });
                $('#TblSportNominatedMember tr').each(function () {
                    SocietyRegistrationData.push(
                        {

                            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                            "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                            "Name": $(this).find('td:eq(2)').find('input').val(),
                            "FatherName": $(this).find('td:eq(3)').find('input').val(),
                            "ContactNo": $(this).find('td:eq(4)').find('input').val(),
                            "DOB": $(this).find('td:eq(5)').find('input').val().split("/").reverse().join("-"),
                            "Gender": ($(this).find('td:eq(6)').find('label').html()).charAt(0),//text
                            "CategoryId": $(this).find('td:eq(7)').find('select').val(),
                            "PostId": '0',
                            "PostName": $(this).find('td:eq(8)').find('input').val(),
                            "Address": $(this).find('td:eq(9)').find('textarea').val(),
                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "IsActive": true,
                            "UserType": "5",//Nominated member
                            "AdministratorDesignationId": "-1",
                            "Photo": ($(this).find('td:eq(10)').find('img'))[0].src,
                            "MEMBERTYPE": ($(this).find('td:eq(11)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                            "SubmitStatus": $scope.SubmitStatus

                        });
                    //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
                });

                $(".se-pre-con").fadeIn("slow");
                formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
                $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetails", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if ((data.split('@')).length > 1) {
                            $scope.EMitraTransactionId = (data.split('@')).length > 1 ? (data.split('@'))[1] : '0';
                            $scope.SocietyIdEnc = data.split('@')[0];
                            ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                        }
                        $scope.ClearSocietyAdministrativeDetails();
                        $scope.divSocietyProfilingShow = true;
                        if ($scope.IsEditMode == false) {
                            //$scope.ActId = $scope.SocietyRegistration.ActId;
                            $scope.SocietyDirectory = data.societydirectory;
                            $scope.SocietyIdForAdministrativeDetails = data.data;
                            ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                            $scope.divSocietyProfilingShow = false;
                            $scope.divAddBtn = false;
                            $scope.lstSocietyWorkScope = [];

                            $("#SocietyStep_1").addClass("disabled stepactive1");
                            $("#SocietyStep_2").removeClass("active stepactive1");
                            $("#SocietyStep_2").addClass("disabled stepactive1");
                            $("#SocietyStep_3").addClass("active stepactive2");


                            $("#step1").addClass("tab-pane ng-hide");
                            $("#step2").removeClass("tab-pane active");
                            $("#step2").addClass("tab-pane ng-hide");
                            $("#step3").addClass("tab-pane active");

                            if ($scope.ActId != "7") {
                                $scope.DivOther = true;
                                $scope.DivSport = false;
                            }
                            else {
                                $scope.DivOther = false;
                                $scope.DivSport = true;
                            }
                        }
                        else {


                            var $active = $('.wizard .nav-tabs li.active');
                            prevTab($active);
                            $active.next().removeClass('disabled');
                            var $active = $('.wizardShow .nav-tabs li.active').attr('id');

                            $scope.divSocialProfiling = false;
                            $("#SocietyStep_33").removeClass("disabled stepactive1");
                            $("#SocietyStep_33").addClass("active stepactive1");
                            $("#SocietyStep_22").removeClass("active stepactive3");
                            $("#SocietyStep_22").addClass("disabled stepactive3");

                            $("#step33").removeClass("tab-pane ng-hide");
                            $("#step33").addClass("tab-pane active");
                            $("#step22").removeClass("tab-pane active");
                            $("#step22").addClass("tab-pane ng-hide");


                            $('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });

                            $scope.SocietyDirectory = data.societydirectory;
                            $scope.SocietyIdForAdministrativeDetails = data.data;
                            ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                            $scope.divSocietyProfilingShow = true;
                            $scope.divAddBtn = false;
                            $scope.lstSocietyWorkScope = [];

                            $("#SocietyStep_1").addClass("disabled stepactive1");
                            $("#SocietyStep_2").removeClass("active stepactive1");
                            $("#SocietyStep_2").addClass("disabled stepactive1");
                            $("#SocietyStep_3").addClass("active stepactive2");


                            $("#step1").addClass("tab-pane ng-hide");
                            $("#step2").removeClass("tab-pane active");
                            $("#step2").addClass("tab-pane ng-hide");
                            $("#step3").addClass("tab-pane active");

                            if ($scope.ActId != "7") {
                                $scope.DivOther = true;
                                $scope.DivSport = false;
                            }
                            else {
                                $scope.DivOther = false;
                                $scope.DivSport = true;
                            }
                        }


                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        $scope.DistrictList = [];
                    });

            }
        }
        $scope.SocietyRegisteredOffice.STATE_ID = '6';
        $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
        $scope.SocietyRegisteredOffice.STATE_IDSecretary = '6';
        $scope.SocietyRegisteredOffice.STATE_IDTreasurer = '6';
    };
    // END : Save Society Administrative Details
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


        $('#ISrurl')[0].checked = false;
        $('#ISUrban')[0].checked = false;
        $scope.grid = false;
    }
    // END : Add New Registration
    //START
    $scope.ManageMemberTableCount = function () {



    }


    $scope.Addmorebtn = function (ValidationFlag) {
        if (ValidationFlag) {
            if (!$scope.FormValidationOthersocity(ValidationFlag)) {
                return false;
            }
        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '1') {
            $scope.grid = false;
            $scope.lstSocietyWorkScope = [];
            $scope.lstSocietyWorkScope.push(
                {
                    "WorkScopeId": 0,
                    "WorkScopeLevel_ID": 1,
                    "IsStatLevel": 2,
                    "STATE_ID": 6,
                    "DIVISION_ID": 0,
                    "DISTRICT_ID": 0,
                    "BLOCK_ID": 0,
                    "VILLAGE_ID": 0,
                    "CITY_ID": 0,
                    "WARD_ID": 0,
                    "GP_ID": 0,
                    "CityTypes": 0
                }
            );
        }
        else if ($scope.SocietyRegistration.WorkScopeLevel_ID > 1 && ValidationFlag) {

            if ($scope.lstSocietyWorkScope.length != 0 && (ValidationFlag == undefined || ValidationFlag == true)) {
                if (parseInt(($scope.lstSocietyWorkScope[0]).WorkScopeLevel_ID, 10) != (parseInt($scope.SocietyRegistration.WorkScopeLevel_ID, 10))) {
                    alert('data is not at same level hence can not be added.');
                    return;
                }
                if ($scope.SocietyRegistration.DIVISION_ID != 0 && $scope.SocietyRegistration.CITY_ID == 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function (x) {
                        return x.WorkScopeLevel_ID == parseInt($('#static').val())
                            && x.DIVISION_ID == parseInt($('#ddlDivision').val())
                            && x.DISTRICT_ID == parseInt($('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val())
                            && x.BLOCK_ID == parseInt($('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val())
                            && x.GP_ID == parseInt($('#ddlGP').val() == null ? 0 : $('#ddlGP').val())
                            && x.VILLAGE_ID == parseInt($('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val())
                    });
                }
                else if ($scope.SocietyRegistration.CITY_ID != 0 && $scope.SocietyRegistration.WARD_ID == 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function (x) {
                        return
                        x.CITY_ID == parseInt($('#ddlCity').val())
                    }
                    );
                }
                else if ($scope.SocietyRegistration.CITY_ID != 0 && $scope.SocietyRegistration.WARD_ID != 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function (x) {
                        return
                        x.CITY_ID == parseInt($('#ddlCity').val())
                            && x.WARD_ID == parseInt($('#ddlward').val())
                    }
                    );
                }
                if (_data.length > 0) {
                    alert(_language == 'en-us' ? 'record already exist\n' : 'रिकॉर्ड पहले से मौजूद है \n');
                    return;
                }

            }

            if (($scope.SocietyRegistration.IsStatLevel == 'No' || $scope.SocietyRegistration.DIVISION_ID > '0') && ($scope.SocietyRegistration.WorkScopeLevel_ID != '0')) {
                $scope.grid = true;
                //IsSportsRural
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeId": 0,
                        "WorkScopeLevel_ID": parseInt($('#static option:selected').val()),
                        "STATE_ID": 6,
                        "IsStatLevel": 0,
                        "DIVISION_ID": $('#ddlDivision').val() == null ? 0 : $('#ddlDivision').val(),
                        "DISTRICT_ID": $('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val(),
                        "BLOCK_ID": $('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val(),
                        "VILLAGE_ID": $('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val(),
                        "CITY_ID": $('#ddlCity').val() == null ? 0 : $('#ddlCity').val(),
                        "WARD_ID": $('#ddlward').val() == null ? 0 : $('#ddlward').val(),
                        "GP_ID": $('#ddlGP').val() == null ? 0 : $('#ddlGP').val(),
                        "IsRuralUrban": $('#ISrurl').prop('checked') == true ? 1 : 0,


                        "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                        "STATE_Name": 'Rajasthan' ? 'Rajasthan' : 'N/A',
                        "DIVISION_Name": parseInt($('#ddlDivision option:selected').val()) > 0 ? $('#ddlDivision option:selected').text() : 'N/A',
                        "DISTRICT_ENG": parseInt($('#ddlDistrict option:selected').val()) > 0 ? $('#ddlDistrict option:selected').text() : 'N/A',
                        "BLOCK_Name": parseInt($('#ddlblocki option:selected').val()) > 0 ? $('#ddlblocki option:selected').text() : 'N/A',
                        "VILLAGE_Name": parseInt($('#ddlVillage option:selected').val()) > 0 ? $('#ddlVillage option:selected').text() : 'N/A',
                        "CityTypes_Name": parseInt($('#staticall option:selected').val()) > 0 ? $('#staticall option:selected').text() : 'N/A',
                        "CITY_Name": parseInt($('#ddlCity option:selected').val()) > 0 ? $('#ddlCity option:selected').text() : 'N/A',
                        "WARD_Name": parseInt($('#ddlward option:selected').val()) > 0 ? $('#ddlward option:selected').text() : 'N/A',
                        "GP_ID_Name": parseInt($('#ddlGP option:selected').val()) > 0 ? $('#ddlGP option:selected').text() : 'N/A',
                    }
                );
            }
            else if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {
                $scope.grid = false;
                $scope.lstSocietyWorkScope = [];
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeId": 0,
                        "WorkScopeLevel_ID": 1,
                        "IsStatLevel": 1,
                        "STATE_ID": 6,
                        "DIVISION_ID": 0,
                        "DISTRICT_ID": 0,
                        "BLOCK_ID": 0,
                        "VILLAGE_ID": 0,
                        "CITY_ID": 0,
                        "WARD_ID": 0,
                        "GP_ID": 0,
                        "CityTypes": 0
                    }
                );
            }
            else if ($scope.SocietyRegistration.IsStatLevel == 'No' && ValidationFlag == undefined) {
                // alert("Please Choose level of work scope");
            }


            $scope.SocietyRegistration.WorkScopeLevel = '0';
            $scope.SocietyRegistration.STATE_ID = '0';
            $scope.SocietyRegistration.DIVISION_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.SocietyRegistration.TEHSIL_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            //$scope.SocietyRegistration.Rural = null;
            //$scope.SocietyRegistration.urban = null;
            $scope.SocietyRegistration.CityTypes = '0';
            $scope.SocietyRegistration.CITY_ID = '0';
            $scope.SocietyRegistration.WARD_ID = '0';

            //$scope.SocietyRegistration.No = null;
            //$scope.SocietyRegistration.Yes = null;

            $scope.SocietyRegistration.GP_ID = '0';
            //$scope.SocietyRegistration.WorkScopeLevel_ID = '0';
        }

    }

    $scope.AddMoreForSportSociety = function () {

        if (!$scope.FormValidationSportsAssociation()) {
            return false;
        }

        if ($scope.SocietyRegistration.DISTRICT_ID != 0 || $scope.SocietyRegistration.BLOCK_ID != 0) {
            var _data = $scope.lstSocietyWorkScope.filter(function (x) { return x.DISTRICT_ID == $('#ddlDistricttwo option:selected').val() }

                //&& x.GP_ID == $('#ddlGP option:selected').val()
                //&& x.VILLAGE_ID == $('#ddlGP option:selected').val()
                //&& x.WorkScopeLevel_ID == $('#static option:selected').val()
            );
        }
        if ($scope.SocietyRegistration.BLOCK_ID != 0) {
            //var _data = $scope.lstSocietyWorkScope.filter(function (x) { return x.BLOCK_ID == $('#ddlblockitwo option:selected').val() }

            //    //&& x.GP_ID == $('#ddlGP option:selected').val()
            //    //&& x.VILLAGE_ID == $('#ddlGP option:selected').val()
            //    //&& x.WorkScopeLevel_ID == $('#static option:selected').val()
            //);
        }
        else if ($scope.SocietyRegistration.CITY_ID != 0) {
            var _data = $scope.lstSocietyWorkScope.filter(function (x) {
                return
                x.CITY_ID == $('#ddlCity').val()
            }
                //&& x.CITY_ID == $('#ddlCity option:selected').val()
                //&& x.WARD_ID == $('#ddlward option:selected').val()                
            );
        }
        if (_data.length > 0) {
            alert(_language == 'en-us' ? 'record already exist\n' : 'रिकॉर्ड पहले से मौजूद है \n');
            return;
        }


        if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {
            $scope.gridtwo = true;
            $scope.lstSocietyWorkScope.push(
                {
                    "WorkScopeId": 0,
                    "WorkScopeLevelID": parseInt($('#static option:selected').val()),
                    "STATE_ID": 6,
                    "IsStatLevel": $scope.SocietyRegistration.IsStatLevel == 'Yes' ? 1 : 0,
                    "DISTRICT_ID": parseInt($('#ddlDistricttwo option:selected').val()),
                    "AssociationId": parseInt($('#ddlgame option:selected').val().split('#')[0]),

                    "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                    "STATE_Name": 'Rajasthan' ? 'राजस्थान' : 'N/A',
                    "DISTRICT_ENG": parseInt($('#ddlDistricttwo option:selected').val()) > 0 ? $('#ddlDistricttwo option:selected').text() : 'N/A',
                    "SportsAssociationName": $scope.SocietyRegistration.IsStatLevel == 'Yes' ? (parseInt($('#ddlgame option:selected').val()) > 0 ? $('#ddlgame option:selected').text() : 'N/A') : ($scope.SocietyRegistration.PrimaryBodyName),
                    "DirectorName": $scope.SocietyRegistration.PrimaryBodyCEOName,
                    "MobileNo": $scope.SocietyRegistration.PrimaryBodyCEOMobileNo,
                }
            );

        }
        else if ($scope.SocietyRegistration.IsStatLevel == 'No') {
            $scope.gridtwo = true;
            $scope.lstSocietyWorkScope.push(
                {
                    "WorkScopeId": 0,
                    "WorkScopeLevelID": parseInt($('#static option:selected').val()),
                    "STATE_ID": 6,
                    "IsStatLevel": 0,
                    "DISTRICT_ID": parseInt($('#ddlDistricttwo option:selected').val()) > 0 ? $('#ddlDistricttwo option:selected').val() : '0',
                    "AssociationId": parseInt($('#ddlgame option:selected').val().split('#')[0]) > 0 ? $('#ddlgame option:selected').val().split('#')[0] : '0',
                    "BLOCK_ID": parseInt($('#ddlblockitwo option:selected').val()) > 0 ? $('#ddlblockitwo option:selected').val() : '0',
                    "VILLAGE_ID": parseInt($('#ddlVillage option:selected').val()) > 0 ? $('#ddlVillage option:selected').val() : '0',
                    "CITY_ID": parseInt($('#ddlCity option:selected').val()) > 0 ? $('#ddlCity option:selected').val() : '0',
                    "WARD_ID": parseInt($('#ddlward option:selected').val()) > 0 ? $('#ddlward option:selected').val() : '0',
                    "GP_ID": parseInt($('#ddlGP option:selected').val()) > 0 ? $('#ddlGP option:selected').val() : '0',
                    "CityTypes": parseInt($('#staticalltwo option:selected').val()) > 0 ? $('#staticalltwo option:selected').val() : '0',

                    "IsRuralUrban": $('#IsSportsRural').prop('checked') == true ? 1 : 0,
                    "SportsAssociationName": $scope.SocietyRegistration.IsStatLevel == 'Yes' ? (parseInt($('#ddlgame option:selected').val()) > 0 ? $('#ddlgame option:selected').text() : 'N/A') : ($scope.SocietyRegistration.PrimaryBodyName),
                    "DirectorName": $scope.SocietyRegistration.PrimaryBodyCEOName,
                    "MobileNo": $scope.SocietyRegistration.PrimaryBodyCEOMobileNo,



                    "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                    "STATE_Name": 'Rajasthan' ? 'राजस्थान' : 'N/A',
                    "DISTRICT_ENG": parseInt($('#ddlDistricttwo option:selected').val()) > 0 ? $('#ddlDistricttwo option:selected').text() : 'N/A',
                    "BLOCK_Name": parseInt($('#ddlblockitwo option:selected').val()) > 0 ? $('#ddlblockitwo option:selected').text() : 'N/A',
                    "VILLAGE_Name": parseInt($('#ddlVillage option:selected').val()) > 0 ? $('#ddlVillage option:selected').text() : 'N/A',
                    "CityTypes_Name": parseInt($('#staticalltwo option:selected').val()) > 0 ? $('#staticalltwo option:selected').text() : 'N/A',
                    "CITY_Name": parseInt($('#ddlCity option:selected').val()) > 0 ? $('#ddlCity option:selected').text() : 'N/A',
                    "WARD_Name": parseInt($('#ddlward option:selected').val()) > 0 ? $('#ddlward option:selected').text() : 'N/A',
                    "GP_ID_Name": parseInt($('#ddlGP option:selected').val()) > 0 ? $('#ddlGP option:selected').text() : 'N/A',
                }
            );
        }

        $scope.SocietyRegistration.PrimaryBodyCEOName = '';
        $scope.SocietyRegistration.PrimaryBodyCEOMobileNo = '';
        $scope.SocietyRegistration.WorkScopeLevel = '0';
        $scope.SocietyRegistration.STATE_ID = '6';
        $scope.SocietyRegistration.DIVISION_ID = '0';
        // $scope.SocietyRegistration.DISTRICT_ID = '0';
        $scope.SocietyRegistration.TEHSIL_ID = '0';
        $scope.SocietyRegistration.BLOCK_ID = '0';
        $scope.SocietyRegistration.VILLAGE_ID = '0';
        //$scope.SocietyRegistration.Rural = null;
        //$scope.SocietyRegistration.urban = null;
        $scope.SocietyRegistration.CityTypes = '0';
        $scope.SocietyRegistration.CITY_ID = '0';
        $scope.SocietyRegistration.WARD_ID = '0';

        //$scope.SocietyRegistration.No = null;
        //$scope.SocietyRegistration.Yes = null;

        $scope.SocietyRegistration.GP_ID = '0';
        $scope.SocietyRegistration.WorkScopeLevel_ID = '0';


    }
    // START : Save Society Administrative Details
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
    $scope.FormValidationPartial = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.SocietyRegistration.ActId == "7") {

            if ($scope.SocietyRegistration.IsSportsNewRegistrationSelect == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Application Type' : '\n <br /> कृपया आवेदन के प्रकार का चयन करें |';
            }

            if ($scope.SocietyRegistration.SocietyName == undefined || $scope.SocietyRegistration.SocietyName == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Sports Association Name' : '\n <br /> कृपया क्रीड़ा संगम का नाम दर्ज करें |';
            }

            if ($scope.SocietyRegistration.IsStatLevel == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Sports Association level' : '\n <br /> कृपया क्रीड़ा संगम का स्तर का चयन करे |';
            }
            if ($scope.SocietyRegistration.AssociationId.split('#')[1] = 0 || $scope.SocietyRegistration.AssociationId == undefined || $scope.SocietyRegistration.AssociationId == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Sport' : '\n <br /> कृपया क्रीड़ा का चयन करे |';
            }

            if ($scope.SocietyRegistration.DISTRICT_ID == 0 && $scope.SocietyRegistration.IsStatLevel == 'No') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select District Council' : '\n <br /> कृपया जिला परिषद का चयन करे |';

            }
            if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
            }
            if ($('#txtSportRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId == "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख दर्ज करे |';

            }
        }
        else {
            if ($scope.SocietyRegistration.ActId == undefined || $scope.SocietyRegistration.ActId == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Act' : '\n <br /> कृपया अधिनियम का चयन करे |';
            }
            if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
            }
            if (($scope.SocietyRegistration.SocietyName == undefined || $scope.SocietyRegistration.SocietyName == '') && $scope.SocietyRegistration.ActId != "7") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Name' : '\n <br /> कृपया सोसाइटी का नाम दर्ज करें |';
            }
            if ($scope.SocietyRegistration.SocietyTypeId == 0 && $scope.SocietyRegistration.ActId != "7") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Type' : '\n <br /> कृपया सोसाइटी का प्रकार चुनें |';
            }
            if (($scope.SocietyRegistration.BRN == undefined || $scope.SocietyRegistration.BRN == '') && $scope.SocietyRegistration.ActId != "7" && $('#EUserType').val() != "D") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter BRN No.' : '\n <br /> कृपया बीआरएन नंबर दर्ज करें |';
            }
            if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख का चयन करे |';
            }
        }

        if (message.length > 29) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }
        if ($scope.SubmitStatus == 1)
            return false;
        else
            return true;
    }
    //validateAddForm START
    $scope.FormValidation = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyRegistration.ActId == "7") {
            if ($scope.SocietyRegistration.IsStatLevel == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Sports Association level' : '\n <br /> कृपया क्रीड़ा संगम का स्तर का चयन करे |';

            }
            else {
                if ($scope.SocietyRegistration.AssociationId == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Sport' : '\n <br /> कृपया क्रीड़ा का चयन करे |';
                }
                if ($scope.SocietyRegistration.DISTRICT_ID == 0 && $scope.SocietyRegistration.IsStatLevel == 'No') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select District Council' : '\n <br /> कृपया जिला परिषद का चयन करे |';

                }
                if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.IsNewRegistration == false) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
                }

                //DISTRICT
                if (($scope.GetFileUploaded("flDLSCAffiliation_Certificates") == '') && $scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To The District Sports Council(PDF).' : '\n <br />कृपया जिला खेल परिषद से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ)|';
                }
                if (($scope.GetFileUploaded("flSOCAffiliation_Certificate") == '') && $scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To The District Olympic Association(PDF).' : '\n <br />कृपया जिला ओलंपिक परिषद से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ) |';
                }
                if (($scope.GetFileUploaded("flSLSAffiliation_Certificates") == '') && $scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.IsAffiliationBySLSA == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificates Of Affiliation To The State Level Sports Council(PDF).' : '\n <br />कृपया राज्य स्तरीय खेल संघ से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ)|';
                }
                //DISTRICT END
                //STATE
                if (($scope.GetFileUploaded("flSLSCAffiliation_Certificates") == '') && $scope.SocietyRegistration.IsStatLevel == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySLSC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To The State Level Sports Council(PDF).' : '\n <br /> कृपया राज्य स्तरीय खेल परिषद से संबद्धता का प्रमाणपत्र अपलोड करें (पीडीएफ)|';
                }
                if (($scope.GetFileUploaded("flSOCAffiliation_Certificate") == '') && $scope.SocietyRegistration.IsStatLevel == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To State Olympic Association(PDF).' : '\n <br /> कृपया राज्य ओलंपिक परिषद से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ)|';
                }
                //STATE END





                var rowCount = $('#SportsAssoCEOtbl tbody tr').length;
                if ($scope.SocietyRegistration.IsStatLevel == 'No') {
                    if (rowCount < 3) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please enter the primary sports body representing the minimum 3' : '\n <br /> कृपया न्यूनतम 3 प्रतिनिधित्व करने वाली प्राथमिक क्रीड़ा निकाय दर्ज करें |';

                    }
                    if ($scope.SocietyRegistration.Ruraltwo == undefined && rowCount == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Rural/Urban' : '\n <br /> कृपया शहरी/ग्रामीण का चयन करे|';

                    }

                    if ($scope.SocietyRegistration.IsAffiliatedBySOC == undefined && $scope.SocietyRegistration.AssociationId.split('#')[1] > 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select District Olympic Association Affiliation' : '\n <br /> कृपया जिला ओलंपिक परिषद से संबद्धता की स्थिति चुने|';
                    }
                    else if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                        if ($scope.SocietyRegistration.SOCRegNo == undefined || $scope.SocietyRegistration.SOCRegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(District Olympic Association)' : '\n <br /> कृपया संबद्धता क्रमांक(जिला ओलंपिक परिषद) दर्ज करें |';
                        }
                        if ($('#txtDateSOC').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(District Olympic Association)' : '\n <br /> कृपया संबद्धता तिथि(जिला ओलंपिक परिषद) दर्ज करें |';
                        }
                    }
                    if ($scope.SocietyRegistration.IsAffiliatedByDLSC == undefined) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of District Level Sports Council Affiliation' : '\n <br /> कृपया जिला स्तरीय क्रीड़ा परिषद् से संबद्धता की स्थिति चुने|';
                    }
                    else if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes') {
                        if ($scope.SocietyRegistration.DLSCRegNo == undefined || $scope.SocietyRegistration.DLSCRegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(District Level Sports Council)' : '\n <br /> कृपया संबद्धता क्रमांक(जिला स्तरीय क्रीड़ा परिषद्) दर्ज करें |';
                        }
                        if ($('#txtDateDLSCDistrict').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(District Level Sports Council)' : '\n <br /> कृपया संबद्धता तिथि(जिला स्तरीय क्रीड़ा परिषद्) दर्ज करें |';
                        }
                    }

                    if ($scope.SocietyRegistration.IsAffiliationBySLSA == undefined) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of State Level Sports Association' : '\n <br /> कृपया राज्य स्तरीय क्रीड़ा संघ की स्थिति चुनें|';
                    }
                    else if ($scope.SocietyRegistration.IsAffiliationBySLSA == 'Yes') {
                        if ($scope.SocietyRegistration.SLSARegNo == undefined || $scope.SocietyRegistration.SLSARegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(State Level Sports Association)' : '\n <br /> कृपया संबद्धता क्रमांक(राज्य स्तरीय खेल संघ) दर्ज करें |';
                        }
                        if ($('#txtDateSLSA').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(State Level Sports Association)' : '\n <br /> कृपया संबद्धता तिथि(राज्य स्तरीय खेल संघ) दर्ज करें |';
                        }
                    }

                }
                else if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {
                    if (rowCount < 6) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please enter the District Sports Council representing the minimum 6 ' : '\n <br /> कृपया न्यूनतम 6 प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् दर्ज करें |';

                    }
                    if ($scope.SocietyRegistration.IsAffiliatedBySOC == undefined) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of State Olympic Association Affiliation' : '\n <br /> कृपया राज्य ओलंपिक परिषद से संबद्धता की स्थिति चुने|';

                    }
                    else if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                        if ($scope.SocietyRegistration.SOCRegNo == undefined || $scope.SocietyRegistration.SOCRegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(State Olympic Association)' : '\n <br /> कृपया संबद्धता क्रमांक(राज्य ओलंपिक परिषद) दर्ज करें |';
                        }
                        if ($('#txtDateSOC').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(State Olympic Association)' : '\n <br /> कृपया संबद्धता तिथि(राज्य ओलंपिक परिषद) दर्ज करें |';
                        }
                    }

                    //if ($scope.SocietyRegistration.IsAffiliatedBySLSC == undefined) {
                    //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of District Level Sports Council Affiliation' : '\n <br /> कृपया जिला स्तरीय क्रीड़ा परिषद् से संबद्धता की स्थिति चुने|';
                    //}
                    //else if ($scope.SocietyRegistration.IsAffiliatedBySLSC == 'Yes') {
                    //    if ($scope.SocietyRegistration.DLSCRegNo == undefined || $scope.SocietyRegistration.DLSCRegNo == '') {
                    //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(District Level Sports Council)' : '\n <br /> कृपया संबद्धता क्रमांक(जिला स्तरीय क्रीड़ा परिषद्) दर्ज करें |';
                    //    }
                    //    if ($('#txtDateSLSCState').val().length == 0) {
                    //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(District Level Sports Council)' : '\n <br /> कृपया संबद्धता तिथि(जिला स्तरीय क्रीड़ा परिषद्) दर्ज करें |';
                    //    }
                    //}
                }
                if ($scope.SocietyRegistration.SocietyName == undefined || $scope.SocietyRegistration.SocietyName == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Sports Association Name' : '\n <br /> कृपया क्रीड़ा संगम का नाम दर्ज करें |';
                }
                if ($('#txtSportElectionDate').val().length == 0 && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == false)) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Election Date' : '\n <br /> कृपया अंतिम चुनाव तिथि का चयन करे |';
                }
                if ($('#txtSportAgmDate').val().length == 0 && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == false)) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Annual General Meeting Date' : '\n <br /> कृपया वार्षिक आम बैठक की तारीख चुनें |';
                }
                // by n saini
                //if ($scope.SocietyRegistration.IsAffiliatedByRSSC == undefined && $scope.SocietyRegistration.IsStatLevel == 'Yes') {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of Rajasthan State Sports Council Affiliation' : '\n <br /> कृपया राजस्थान राज्य क्रीड़ा परिषद से संबद्धता की स्थिति चुने|';
                //}
                else if ($scope.SocietyRegistration.IsAffiliatedByRSSC == 'Yes') {
                    if ($scope.SocietyRegistration.RSSCRegNo == undefined || $scope.SocietyRegistration.RSSCRegNo == '') {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(Rajasthan State Sports Council)' : '\n <br /> कृपया संबद्धता क्रमांक(राजस्थान राज्य क्रीड़ा परिषद) दर्ज करें |';
                    }
                    if ($('#txtDateRSSC').val().length == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(Rajasthan State Sports Council)' : '\n <br /> कृपया संबद्धता तिथि(राजस्थान राज्य क्रीड़ा परिषद) दर्ज करें |';
                    }
                }
            }
        }

        if ($scope.SocietyRegistration.ActId == undefined || $scope.SocietyRegistration.ActId == "0") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Act' : '\n <br /> कृपया अधिनियम का चयन करे |';
        }
        if (($scope.SocietyRegistration.BRN == undefined || $scope.SocietyRegistration.BRN == '') && $scope.SocietyRegistration.ActId != "7" && $('#EUserType').val() != "D") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter BRN No.' : '\n <br /> कृपया बीआरएन नंबर दर्ज करें |';
        }
        if (($scope.SocietyRegistration.AreaOfOperation == undefined || $scope.SocietyRegistration.AreaOfOperation == '' || $scope.SocietyRegistration.AreaOfOperation == 'undefined') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Area of Operation as per by Laws' : '\n <br /> कृपया by Laws के अनुसार कार्य क्षेत्र का विवरण दर्ज करें |';
        }
        if ($scope.SocietyRegistration.IsNewRegistrationSelect == undefined && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }
        if ($scope.SocietyRegistration.IsSportsNewRegistrationSelect == undefined && $scope.SocietyRegistration.ActId == "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }
        if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false)) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
        }
        if ($scope.SocietyRegistration.SocietyName == undefined && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Name' : '\n <br /> कृपया सोसाइटी का नाम दर्ज करें |';
        }
        if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false)) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख का चयन करे |';
        }
        if ($('#txtElectionDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false)) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Election Date' : '\n <br /> कृपया चुनाव तिथि का चयन करें |';
        }
        if ($scope.SocietyRegistration.SocietyTypeId == 0 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Type' : '\n <br /> कृपया सोसाइटी का प्रकार चुनें |';
        }
        if ($scope.SocietyRegistration.ClassId == 0 && $scope.SocietyRegistration.ActId == 3 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Class' : '\n <br /> कृपया सोसाइटी क्लास का चयन करे  |';
        }
        if ($scope.SocietyRegistration.SubClassId == 0 && $scope.SocietyRegistration.ActId == 3) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Sub Society Class' : '\n <br /> कृपया उप सोसाइटी क्लास का चयन करे |';
        }
        if ($scope.SocietyRegistration.SocietyClassId == 0 && $scope.SocietyCategoryList.Count > 0 && $scope.SocietyRegistration.ActId == 3) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Category' : '\n <br /> कृपया सोसायटी श्रेणी का चयन करें |';
        }

        if ($scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.WorkScopeLevel_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया सोसाइटी कार्य स्कोप स्तर चुनें |';
            }
            if ($scope.lstSocietyWorkScope.length == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया सोसाइटी कार्य स्कोप स्तर चुनें |';
            }
        }

        if (($scope.SocietyRegistration.NoOfBranches == null || $scope.SocietyRegistration.NoOfBranches == undefined || $scope.SocietyRegistration.NoOfBranches == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter No of Branches.' : '\n <br /> शाखाओं में से कोई भी दर्ज करें |';
        }

        if ($scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.ActId != "7") {
            if ($scope.SocietyRegistration.Rural == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Choose the Rural/Urban.' : '\n <br /> कृपया ग्रामीण / शहरी चुनें|';
            }
        }

        //if ($scope.SocietyRegistration.IsNationalLevel == undefined && $scope.SocietyRegistration.IsStatLevel == undefined && $scope.SocietyRegistration.ActId == "1") {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Choose Area of operations ?' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें?';

        //}
        if (($scope.SocietyRegistration.WorkScopeLevel_ID == undefined || $scope.SocietyRegistration.WorkScopeLevel_ID == '0') && $scope.SocietyRegistration.ActId != "7" && $scope.lstSocietyWorkScope.length < 1) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Choose Area of operations ?' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें?';
        }

        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '7' && $scope.lstSocietyWorkScope.length < 1) {
            if ($scope.SocietyRegistration.CITY_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the City .' : '\n <br /> कृपया शहर का चयन करें |';
            }
            if ($scope.SocietyRegistration.WARD_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the ward .' : '\n <br /> कृपया वार्ड का चयन करें |';
            }
        }

        //if ($scope.SocietyRegistration.Rural == true && $scope.lstSocietyWorkScope.length < 1) {
        //    if ($scope.SocietyRegistration.WorkScopeLevel_ID == '0') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Choose Level of work scope.' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें |';
        //    }
        //}

        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '2' && $scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division Level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';
        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '3' && $scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';

            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';
        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '4' && $scope.lstSocietyWorkScope.length == '0') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';


            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें|';

        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '5' && $scope.lstSocietyWorkScope.length == '0') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';



            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें |';


            if ($scope.SocietyRegistration.GP_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Gram panchayat level.' : '\n <br /> कृपया ग्राम पंचायत स्तर का चयन करें |';

        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '6' && $scope.lstSocietyWorkScope.length == '0') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';


            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें |';


            if ($scope.SocietyRegistration.GP_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Gram panchayat level.' : '\n <br /> कृपया ग्राम पंचायत स्तर का चयन करें |';


            if ($scope.SocietyRegistration.VILLAGE_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Village level.' : '\n <br /> कृपया गांव स्तर का चयन करें |';

        }
        if (($scope.SocietyRegistration.SocietyCategoryId == undefined || $scope.SocietyRegistration.SocietyCategoryId == 0) && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Category' : '\n <br /> कृपया सोसाइटी श्रेणी का चयन करें |';
        }

        if (($scope.SocietyRegistration.MajorActivityId == 0 || $scope.SocietyRegistration.MajorActivityId == undefined) && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Major Activity' : '\n <br /> कृपया प्रमुख गतिविधि का चयन करें |';
        }

        if (($scope.SocietyRegistration.ActivityId == 0 || $scope.SocietyRegistration.ActivityId == undefined) && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Activity' : '\n <br /> कृपया सोसाइटी गतिविधि का चयन करें |';
        }

        if (($scope.SocietyRegistration.BusinessId == 0 || $scope.SocietyRegistration.BusinessId == undefined) && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Business' : '\n <br /> कृपया सोसाइटी व्यापार का चयन करें |';
        }

        if (($scope.SocietyRegistration.LiablityId == 0 || $scope.SocietyRegistration.LiablityId == undefined) && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Liability Type' : '\n <br /> कृपया सोसाइटी दायित्व के प्रकार का चयन करें |';
        }

        if ($scope.SocietyRegistration.LiablityId == 1 && ($scope.SocietyRegistration.MemberLiablity == undefined || $scope.SocietyRegistration.MemberLiablity == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Liability of a Member LTD Society' : '\n <br /> कृपया लिमिटेड सोसायटी के सदस्य दायित्व की संख्या करें |';
        }

        if (($scope.SocietyRegistration.TotalNumberOfMembers == undefined || $scope.SocietyRegistration.TotalNumberOfMembers == '') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Number of Members Applying for the Society' : '\n <br /> कृपया सोसाइटी के लिए आवेदन करने वाले सदस्यों की कुल संख्या दर्ज करें |';
        }

        if ($('#txtEntranceFees').val() == "" && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Entrance Fees Collected' : '\n <br /> कृपया आवेदन करने वाले सदस्यों की प्रवेश शुल्क दर्ज करें|';
        }

        if (($scope.SocietyRegistration.SecurityDeposit == undefined || $scope.SocietyRegistration.SecurityDeposit == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Security Deposit' : '\n <br /> कृपया अमानत राशि दर्ज करें|';
        }

        if (($scope.SocietyRegistration.WorkingCapital == undefined || $scope.SocietyRegistration.WorkingCapital == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Working Capital' : '\n <br /> कृपया कार्यशील पूंजी दर्ज करें|';
        }

        if (($scope.SocietyRegistration.LoanAmt == undefined || $scope.SocietyRegistration.LoanAmt == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Loan Amount' : '\n <br /> कृपया ऋण की राशि दर्ज करें|';
        }

        if (($scope.SocietyRegistration.SingleShareValue == undefined || $scope.SocietyRegistration.SingleShareValue == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Value of One Share' : '\n <br /> कृपया कृपया एक शेयर का मूल्य दर्ज करें|';
        }

        if (($scope.SocietyRegistration.PaidUpShareCapital == undefined || $scope.SocietyRegistration.PaidUpShareCapital == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Paid Up Share Capital' : '\n <br /> कृपया शेयर पूंजी का भुगतान दर्ज करें|';
        }

        if ($('#txtAGMDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select AGM Date' : '\n <br /> कृपया एजीएम की तारीख का चयन करे |';
        }

        if (($scope.SocietyRegistration.NoOfMemberInAGM == undefined || $scope.SocietyRegistration.NoOfMemberInAGM == '') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter No of Members who Participated in AGM' : '\n <br /> कृपया एजीएम में भाग लेने वाले सदस्यों की संख्या दर्ज करें|';
        }

        if ($scope.SocietyRegistration.ActId != "7") {
            if ($scope.GetFileUploaded("flRegistration_Certificate") == '' && ($scope.SocietyRegistration.IsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Registration Certificate(PDF)' : '\n <br /> कृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';
            }
        }
        else if ($scope.SocietyRegistration.ActId == "7") {
            if ($scope.GetFileUploaded("flRegistration_Certificate") == '' && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Registration Certificate(PDF)' : '\n <br /> कृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';
            }
        }

        if ($scope.GetFileUploaded("flRulesAnd_Regulation") == '' && $scope.SocietyRegistration.ActId == "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Rules And Regulation(PDF)' : '\n <br /> कृपया नियम और अधिनियम अपलोड करें (पीडीएफ) |';
        }

        if ($scope.GetFileUploaded("flBy_laws") == '' && $scope.SocietyRegistration.ActId != "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Bye Laws(PDF)' : '\n <br /> कृपया बाई लॉज़ अपलोड करें(पीडीएफ) |';
        }

        if ($scope.GetFileUploaded("flUploadOldExecutive_Committee") == '' && $scope.SocietyRegistration.ActId == "1" && $scope.SocietyRegistration.IsNewRegistration == false) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Old Executive Committee' : '\n <br /> कृपया पुरानी कार्यकारी समिति को अपलोड करें |';
        }

        if ($scope.GetFileUploaded("flUploadMembers_List") == '') {   //by n saini

            if ($scope.SocietyRegistration.ActId == "3" && $scope.GetFileUploaded("flCooperatives_List") == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Members List(Excel) OR Upload the Details Of Your Member Cooperatives (Excel)' : '\n <br /> कृपया सदस्य सूची(एक्सेल) अपलोड करें या अपनी सदस्य सहकारी समितियों की डिटेल अपलोड करें |';
            }
            else if ($scope.SocietyRegistration.ActId != "3") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Members List(Excel)' : '\n <br /> कृपया सदस्य सूची(एक्सेल) अपलोड करें |';
            }

        }

        if ($scope.GetFileUploaded("flSocietyWorkPlanEconomic_Viability") == '' && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Work Plan Showing Economic Viability of the Society(PDF)' : '\n <br /> कृपया आर्थिक कार्य योजना अपलोड करें (पीडीएफ)|';
        }

        if ($scope.GetFileUploaded("flGeneralMeeting_Procedure") == '' && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload General Meeting Procedure File(PDF)' : '\n <br /> कृपया अपलोड जनरल मीटिंग प्रोसीजर फाइल अपलोड करें(पीडीऍफ़) |';
        }

        if ($scope.GetFileUploaded("flBalanceSheet_Upload") == '' && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Balance Sheet(Excel)' : '\n <br /> कृपया बैलेंस शीट (एक्सेल) अपलोड करें |';
        }

        if ($scope.GetFileUploaded("flUpload_Objects") == '' && $scope.SocietyRegistration.ActId == "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Objects(PDF)' : '\n <br /> कृपया उद्देश्य अपलोड करें (पीडीएफ) |';
            message += '<br />';
        }


        if (message.length > 29 && $scope.SubmitStatus == 1) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }
        if ($scope.SubmitStatus == 0) {

            return false;
        }
        return true;
    }
    //END

    $scope.SocietyOfficeFormValidationPartial = function () {

        var Isvalidate = true;

        var message = $scope._language == 'en-us' ? '<b>Please validate the following to enter the address of the Society:</b> ' : '<b>कृपया सोसाइटी का पता दर्ज करने के लिए निम्नलिखित मान्य करें :</b>';
        if ($scope.SocietyRegisteredOffice.HouseBuildingNo == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNo.length < 1) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society office House / Building No.' : '\n <br /> सोसायटी कार्यालय का हाउस / बिल्डिंग नंबर दर्ज करें |';
            Isvalidate = false;
        }
        if ($scope.SocietyRegisteredOffice.LaneStreetName == undefined || $scope.SocietyRegisteredOffice.LaneStreetName.length < 1) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter office Lane/Street Name' : '\n <br /> कृपया  कार्यालय का लेन / सड़क नाम दर्ज करें |';
            Isvalidate = false;
        }
        if ($scope.SocietyRegisteredOffice.Locality == undefined || $scope.SocietyRegisteredOffice.Locality.length < 1) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select office Locality' : '\n <br /> कृपया कार्यालय का मोहल्ला का चयन करें|';
            Isvalidate = false;
        }
        if ($scope.SocietyRegisteredOffice.STATE_ID == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select office State.' : '\n <br /> कृपया कार्यालय का राज्य का चयन करें |';
            Isvalidate = false;
        }
        if ($scope.SocietyRegisteredOffice.IsRural == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please choose office Is Rural or urban  ?' : '\n <br /> कृपया कार्यालय का ग्रामीण या शहरी चुनें ?';
            Isvalidate = false;
        }
        if ($scope.SocietyRegisteredOffice.IsRural == true || $scope.SocietyRegisteredOffice.IsRural == "true") {

            if ($scope.SocietyRegisteredOffice.CITY_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office City ' : '\n <br /> कृपया कार्यालय का शहर का चयन करें |';
                Isvalidate = false;
            }
            if ($scope.SocietyRegisteredOffice.WARD_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office Ward' : '\n <br /> कृपया कार्यालय का वार्ड का चयन करें |';
                Isvalidate = false;
            }
        }
        if ($scope.SocietyRegisteredOffice.IsRural == false || $scope.SocietyRegisteredOffice.IsRural == "false") {

            if ($scope.SocietyRegisteredOffice.BLOCK_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office Block' : '\n <br /> कृपया कार्यालय का ब्लॉक का चयन करें|';
                Isvalidate = false;
            }
            if ($scope.SocietyRegisteredOffice.GP_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office Gram Panchayat' : '\n <br /> कृपया कार्यालय का ग्राम पंचायत का चयन करें |';
                Isvalidate = false;
            }
            if ($scope.SocietyRegisteredOffice.VILLAGE_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office Village' : '\n <br /> कृपया कार्यालय का गांव का चयन करें |';
                Isvalidate = false;
            }

        }
        if ($scope.SocietyRegisteredOffice.PinCode == 0 || $scope.SocietyRegisteredOffice.PinCode == undefined || $scope.SocietyRegisteredOffice.PinCode == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter office PinCode' : '\n <br /> कृपया पिनकोड दर्ज करें  |';
            Isvalidate = false;
        }

        if (Isvalidate == false && $scope.SubmitStatus == 0) {
            var _Temp = (_language == 'en-us' ? 'Society Office Details' : 'सोसायटी कार्यालय विवरण');
            ShowModelAlert(_Temp, message);
            return false;
        } else if ($scope.SocietyRegisteredOffice.herebyAccept == undefined) {
            var _Temp = (_language == 'en-us' ? 'Society Office Details' : 'सोसायटी कार्यालय विवरण');
            message = $scope._language == 'en-us' ? '\n <br /><b> Please aceept terms and condition.</b>' : '\n <br /><b> कृपया नियम और शर्त स्वीकार करें।</b>';
            ShowModelAlert(_Temp, message);
            return false;
        }
        return true;
    }


    $scope.SocietyOfficeFormValidation = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.SocietyRegistration.ActId == "7") {
            if ($scope.SocietyRegisteredOffice.HouseBuildingNoSecretary == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNoSecretary.length < 1 || $scope.SocietyRegisteredOffice.HouseBuildingNoSecretary == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Secretary House / Building No.' : '\n <br /> सोसायटी सचिव का हाउस / बिल्डिंग नंबर दर्ज करें|';

            }
            if ($scope.SocietyRegisteredOffice.LaneStreetNameSecretary == undefined || $scope.SocietyRegisteredOffice.LaneStreetNameSecretary.length < 1 || $scope.SocietyRegisteredOffice.LaneStreetNameSecretary == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Secretary Lane/Street Name' : '\n <br /> कृपया सचिव का लेन / सड़क का नाम दर्ज करें|';

            }
            if ($scope.SocietyRegisteredOffice.LocalitySecretary == undefined || $scope.SocietyRegisteredOffice.LocalitySecretary.length < 1 || $scope.SocietyRegisteredOffice.LocalitySecretary == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Locality' : '\n <br /> कृपया सचिव का लोकैलिटी का चयन करें|';

            }
            if ($scope.SocietyRegisteredOffice.STATE_IDSecretary == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary State.' : '\n <br /> कृपया सचिव का राज्य का चयन करें |';

            }
            if ($scope.SocietyRegisteredOffice.IsRuralSecretary == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please choose Secretary Is Rural or urban  ?' : '\n <br /> कृपया सचिव का  ग्रामीण या शहरी चुनें ?';

            }


            if ($scope.SocietyRegisteredOffice.IsRuralSecretary == true || $scope.SocietyRegisteredOffice.IsRuralSecretary == "true") {
                //if ($scope.SocietyRegisteredOffice.CityTypesSecretary == 0) {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary CityType ' : '\n <br /> कृपया सचिव का  शहर का प्रकार चुनें |';

                //}
                if ($scope.SocietyRegisteredOffice.CITY_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary City ' : '\n <br /> कृपया सचिव का  शहर का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.WARD_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Ward' : '\n <br /> कृपया सचिव का  वार्ड का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.IsRuralSecretary == false || $scope.SocietyRegisteredOffice.IsRuralSecretary == "false") {
                if ($scope.SocietyRegisteredOffice.BLOCK_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Block' : '\n <br /> कृपया ब्लॉक का चयन करें|';

                }
                if ($scope.SocietyRegisteredOffice.GP_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Gram Panchayat' : '\n <br /> कृपया सचिव का  ग्राम पंचायत का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.VILLAGE_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Village' : '\n <br /> कृपया सचिव का गांव का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.PinCodeSecretary == '' || $scope.SocietyRegisteredOffice.PinCodeSecretary == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Secretary Enter PinCode' : '\n <br /> कृपया सचिव का पिनकोड दर्ज करें  |';

            }

            if ($scope.SocietyRegisteredOffice.HouseBuildingNoTreasurer == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNoTreasurer.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Treasurer House / Building No.' : '\n <br /> सोसायटी कोषाध्यक्ष का हाउस / बिल्डिंग नंबर दर्ज करें |';

            }
            if ($scope.SocietyRegisteredOffice.LaneStreetNameTreasurer == undefined || $scope.SocietyRegisteredOffice.LaneStreetNameTreasurer.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Treasurer Lane/Street Name' : '\n <br /> कृपया कोषाध्यक्ष का लेन / सड़क नाम दर्ज करें |';

            }
            if ($scope.SocietyRegisteredOffice.LocalityTreasurer == undefined || $scope.SocietyRegisteredOffice.LocalityTreasurer.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer Locality' : '\n <br /> कृपया कोषाध्यक्ष का मोहल्ला का चयन करें|';

            }
            if ($scope.SocietyRegisteredOffice.STATE_IDTreasurer == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer State.' : '\n <br /> कृपया कोषाध्यक्ष का राज्य का चयन करें |';

            }
            if ($scope.SocietyRegisteredOffice.IsRuralTreasurer == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please choose Treasurer Is Rural or urban  ?' : '\n <br /> कृपया कोषाध्यक्ष का ग्रामीण या शहरी चुनें ?';

            }
            if ($scope.SocietyRegisteredOffice.IsRuralTreasurer == true || $scope.SocietyRegisteredOffice.IsRuralTreasurer == "true") {
                if ($scope.SocietyRegisteredOffice.CITY_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer City ' : '\n <br /> कृपया कोषाध्यक्ष का शहर का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.WARD_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer Ward' : '\n <br /> कृपया कोषाध्यक्ष का वार्ड का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.IsRuralTreasurer == false || $scope.SocietyRegisteredOffice.IsRuralTreasurer == "false") {

                if ($scope.SocietyRegisteredOffice.BLOCK_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Block' : '\n <br /> कृपया कोषाध्यक्ष का ब्लॉक का चयन करें|';

                }
                if ($scope.SocietyRegisteredOffice.GP_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram Panchayat' : '\n <br /> कृपया कोषाध्यक्ष का ग्राम पंचायत का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.VILLAGE_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Village' : '\n <br /> कृपया कोषाध्यक्ष का गांव का चयन करें |';

                }

            }
            if ($scope.SocietyRegisteredOffice.PinCodeTreasurer == 0 || $scope.SocietyRegisteredOffice.PinCodeTreasurer == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Treasurer PinCode' : '\n <br /> कृपया कोषाध्यक्ष का पिनकोड दर्ज करें  |';

            }

            //if ($scope.SocietyRegisteredOffice.herebyAccept == undefined) {
            //    message += $scope._language == 'en-us' ? '\n <br /> Please aceept terms and condition.' : '\n <br /> कृपया नियम और शर्त स्वीकार करें।|';
            //}
        }
        if ($scope.SocietyRegistration.ActId != "7") {
            if ($scope.SocietyRegisteredOffice.HouseBuildingNo == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNo.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society office House / Building No.' : '\n <br /> सोसायटी कार्यालय का हाउस / बिल्डिंग नंबर दर्ज करें |';

            }
            if ($scope.SocietyRegisteredOffice.LaneStreetName == undefined || $scope.SocietyRegisteredOffice.LaneStreetName.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter office Lane/Street Name' : '\n <br /> कृपया  कार्यालय का लेन / सड़क नाम दर्ज करें |';

            }
            if ($scope.SocietyRegisteredOffice.Locality == undefined || $scope.SocietyRegisteredOffice.Locality.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office Locality' : '\n <br /> कृपया कार्यालय का मोहल्ला का चयन करें|';

            }
            if ($scope.SocietyRegisteredOffice.STATE_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office State.' : '\n <br /> कृपया कार्यालय का राज्य का चयन करें |';

            }
            if ($scope.SocietyRegisteredOffice.IsRural == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please choose office Is Rural or urban  ?' : '\n <br /> कृपया कार्यालय का ग्रामीण या शहरी चुनें ?';

            }
            if ($scope.SocietyRegisteredOffice.IsRural == true || $scope.SocietyRegisteredOffice.IsRural == "true") {

                if ($scope.SocietyRegisteredOffice.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office City ' : '\n <br /> कृपया कार्यालय का शहर का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.WARD_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office Ward' : '\n <br /> कृपया कार्यालय का वार्ड का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.IsRural == false || $scope.SocietyRegisteredOffice.IsRural == "false") {

                if ($scope.SocietyRegisteredOffice.BLOCK_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office Block' : '\n <br /> कृपया कार्यालय का ब्लॉक का चयन करें|';

                }
                if ($scope.SocietyRegisteredOffice.GP_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office Gram Panchayat' : '\n <br /> कृपया कार्यालय का ग्राम पंचायत का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.VILLAGE_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office Village' : '\n <br /> कृपया कार्यालय का गांव का चयन करें |';

                }

            }
            if ($scope.SocietyRegisteredOffice.PinCode == 0 || $scope.SocietyRegisteredOffice.PinCode == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter office PinCode' : '\n <br /> कृपया पिनकोड दर्ज करें  |';
            }
            //($scope.SocietyRegisteredOffice.IsSameCEOAddress == undefined || $scope.SocietyRegisteredOffice.IsSameCEOAddress == false) &&
            if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                if ($scope.SocietyRegisteredOffice.HouseBuildingNocopy == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNocopy.length < 1) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman  HouseCopy / Building No.' : '\n <br /> सोसायटी अध्यक्ष का हाउस / बिल्डिंग नंबर दर्ज करें |';

                }
                if ($scope.SocietyRegisteredOffice.LaneStreetNamecopy == undefined || $scope.SocietyRegisteredOffice.LaneStreetNamecopy.length < 1) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman Lane/Street Name' : '\n <br /> कृपया अध्यक्ष का लेन / सड़क नाम दर्ज करें |';

                }

                if ($scope.SocietyRegisteredOffice.Localitycopy == undefined || $scope.SocietyRegisteredOffice.Localitycopy.length < 1) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Locality' : '\n <br /> कृपया अध्यक्ष का मोहल्ला का चयन करें|';

                }
                if ($scope.SocietyRegisteredOffice.IsRuralcopy == undefined) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please choose Chairman Is Rural or urban  ?' : '\n <br /> कृपया अध्यक्ष का का ग्रामीण या शहरी चुनें ?';

                }

                if ($scope.SocietyRegisteredOffice.STATE_IDcopy == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman State.' : '\n <br /> कृपया अध्यक्ष का राज्य का चयन करें |';

                }

                if ($scope.SocietyRegisteredOffice.DISTRICT_IDcopy == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman District' : '\n <br /> कृपया अध्यक्ष का जिला चुनें |';

                }
                if ($scope.SocietyRegisteredOffice.IsRuralcopy == true || $scope.SocietyRegisteredOffice.IsRuralcopy == "true") {
                    if ($scope.SocietyRegisteredOffice.CITY_IDcopy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman City ' : '\n <br /> कृपया अध्यक्ष का शहर का चयन करें |';

                    }
                    if ($scope.SocietyRegisteredOffice.WARD_IDcopy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Ward ' : '\n <br /> कृपया अध्यक्ष का वार्ड का चयन करें |';

                    }
                }
                if ($scope.SocietyRegisteredOffice.IsRuralcopy == false || $scope.SocietyRegisteredOffice.IsRuralcopy == "false") {

                    if ($scope.SocietyRegisteredOffice.BLOCK_ID_copy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Block' : '\n <br /> कृपया अध्यक्ष का ब्लॉक का चयन करें|';

                    }
                    if ($scope.SocietyRegisteredOffice.GP_IDcopy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Gram Panchayat' : '\n <br /> कृपया अध्यक्ष का ग्राम पंचायत का चयन करें |';

                    }
                    if ($scope.SocietyRegisteredOffice.VILLAGE_IDcopy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Village' : '\n <br /> कृपया अध्यक्ष का गांव का चयन करें |';

                    }
                }
                if ($scope.SocietyRegisteredOffice.PinCodecopy == 0 || $scope.SocietyRegisteredOffice.PinCodecopy == undefined) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman PinCodeCopy' : '\n <br /> कृपया अध्यक्ष का पिनकोड दर्ज करें  |';

                }

                //if ($scope.SocietyRegisteredOffice.AreaOfOperationcopy == undefined || $scope.SocietyRegisteredOffice.AreaOfOperationcopy.length == 0) {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Area Of OperationCopy' : '\n <br /> सोसाइटी का कार्यक्षेत्र दर्ज करें |';

                //}

            }

        }

        if ($scope.SocietyRegisteredOffice.herebyAccept == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please aceept terms and condition.' : '\n <br /> कृपया नियम और शर्त स्वीकार करें।|';
        }

        if (message.length > 29 && $scope.SubmitStatus == 1) {
            var _Temp = (_language == 'en-us' ? 'Society Office Details' : 'सोसायटी कार्यालय विवरण');
            ShowModelAlert(_Temp, message);
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends

    $scope.showHideDiv = function () {
        
        $("#txtBrn").removeAttr("disabled");
        $scope.txtBrndisabled = false;
        if ($('#EUserType').val() == 'K' && $scope.SocietyRegistration.ActId == '7') {
            if ($('#EServiceid').val() == '4209')//PROFILE
            {
                $scope.SocietyRegistration.IsNewRegistration = false;
                $scope.Profiling(2);
                $("input[name='RegistrationProfile']")[0].checked = false;
                $("input[name='RegistrationProfile']")[1].checked = true;
            }
            else if ($('#EServiceid').val() == '4172')//REG
            {
                $scope.SocietyRegistration.IsNewRegistration = true;
                $("input[name='RegistrationProfile']")[0].checked = true;
                $("input[name='RegistrationProfile']")[1].checked = false;
                $scope.Profiling(1);
            }
            $('input[name="RegistrationProfile"]').attr('disabled', 'disabled');

        }
        else if ($scope.SocietyRegistration.ActId != '7') {
            if ($('#EUserType').val() == 'K') {
                $scope.CheckBRN = true;
                //$scope.divBrn = false;
                //$scope.divBrnYN = true;

                $($("input[name='SocietyRegistrationBRN']")[0]).checked = false;
                $($("input[name='SocietyRegistrationBRN']")[1]).checked = false;
            }
            else {
                $scope.CheckBRN = true;
                $scope.divBrnbtn = true;
            }

        }

        $scope.DivAdministrative = true;
        $scope.SocietyRegistration.BRN = '';
        $scope.SportsAttechments = false;
        $scope.showrulesfile = false;
        $scope.ShowSubmitBtn = false;
        $scope.divOtherSocietys = false;
        $scope.nepster = true;

        $scope.GetSocietyTypeList();
        var temp = 0;
        if ($scope.SocietyRegistration.ActId != 7) {
            $scope.IsOlympicCouncilApproved = false;  //by n saini
            $scope.AffiliationBySLSC = false;  //by n saini
            $scope.AffiliationBySLSA = false;  //by n saini
            TableData10 = [];
            var X = $scope.SocietyRegistration.ActId == 1 ? 8 : 16;
            for (var x = 1; x < X; x++) {
                TableData10.push(
                    {
                        "Id": x,
                        "POSTNAME": '',
                        "Photo": '../Content/Images/Balnk.png',
                        "CatId": "0",
                        "CategoryId": "0"
                    });
            }
            angular.forEach(TableData10, function (item) {
                if ($scope.SocietyRegistration.ActId == '1') {
                    if (temp == 0) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'अध्यक्ष/सभापति');
                    }
                    else if (temp == 1) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष');
                    }
                    else if (temp == 2) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY' : 'सचिव');
                    }
                    else if (temp == 3) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'TREASURER' : 'कोषाध्यक्ष');
                    }
                    else if (temp == 4) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'DIRECTOR MEMBER' : 'निदेशक सदस्य');
                    }
                }
                else if ($scope.SocietyRegistration.ActId == '3') {
                    if (temp == 0) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN' : 'अध्यक्ष')
                    }
                    else if (temp == 1) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                    }
                    else if (temp == 2) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                    }
                    else if (temp == 3) {
                        item.POSTNAME = ($scope._language == 'en-us' ? 'DIRECTOR' : 'निदेशक')
                    }
                }
                temp = temp + 1;
            });
            $scope.TableData10 = TableData10;
            TableData10NomiAll = [];
            if ($scope.SocietyRegistration.ActId == 3) {
                var X = 2;
                for (var x = 1; x < X; x++) {
                    TableData10NomiAll.push(
                        {
                            "Id": x + 500,
                            "POSTNAME": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CatId": "0",
                            "CategoryId": "0"
                        });
                }
            }
            $scope.TableData10NomiAll = TableData10NomiAll;
            $scope.Act2001 = true;
            $scope.divOtherSocietys = true;
            //$scope.divBrn = true;
            $scope.nepster = true;
            //$scope.CheckBRN = false;
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
                //$scope.Leveldatachange(1);
                //$scope.CheckBRN = false;
                $scope.SportsAttechments = false;
                $scope.Act1958 = false;
            }
            else if ($scope.SocietyRegistration.ActId == 1) {
                $scope.Act1958 = true;
                $scope.Act2001 = false;
                $scope.showrulesfile = false;
                $scope.SportsAttechments = false;
                $scope.showhideOther = false;
                $scope.showhideBylaws = false;  // add by N saini
                // $scope.Leveldatachange(2);
                //$('#Iddstateyes').attr("disabled", "disabled");
                //$('#Iddstateno').attr("disabled", "disabled");

                //$scope.CheckBRN = false;
                $scope.SportsAttechments = false;
                if ($scope.IsEditMode == true) {
                    $scope.divOtherSocietys = true;
                    $scope.showrulesfile = true;
                    $scope.CheckBRN = true;
                    $scope.ShowSubmitBtn = true;
                }
                //$('#Iddstateno')[0].checked = true;
                //$scope.SocietyRegistration.IsStatLevel = 'No';
                //$('#Iddstateyes').removeAttr("disabled");
                //$('#Iddstateno').removeAttr("disabled");
            }
            else {
                $scope.Act2001 = false;
                $scope.showhideBylaws = false;
                $scope.showhideOther = true;
                $scope.showrulesfile = true;
                $scope.showrulesfile = false;
                //$scope.CheckBRN = false;
                $scope.SportsAttechments = false;

                $scope.SportsAttechments = false;
            }
        }
        else {
            TableData10 = [];
            var X = 1006;
            for (var x = 1001; x < X; x++) {
                TableData10.push(
                    {
                        "Id": x,
                        "POSTNAME": '',
                        "Photo": '../Content/Images/Balnk.png',
                        "CatId": "0",
                        "CategoryId": "0"
                    });
            }
            angular.forEach(TableData10, function (item) {
                if (temp == 0) {
                    item.POSTNAME = ($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'अध्यक्ष/सभापति');
                }
                else if (temp == 1) {
                    item.POSTNAME = ($scope._language == 'en-us' ? 'SECRETARY' : 'सचिव');
                }
                else if (temp == 2) {
                    item.POSTNAME = ($scope._language == 'en-us' ? 'TREASURER' : 'कोषाध्यक्ष');
                }
                temp = temp + 1;
            });


            $scope.TableSportsData2 = TableData10;
            $scope.Act2001 = false;
            $scope.Act2005 = true;
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
        $scope.ActId = $scope.SocietyRegistration.ActId;
        if ($scope.ActId == "1") {

            $scope.SocietyRegistration.IsAdministrative = "Board"
            $scope.AdministrativeBoard();
            $scope.Act2001 = false;
            $scope.IsAdministrativeN = true;
        }
        else if ($scope.ActId == "3") {
            $scope.Act2001 = true;
        }
        if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined && $('#EUserType').val() == 'D')
            $scope.CheckRegOrProfileing($scope.SocietyRegistration.IsNewRegistration ? 1 : 2);
        else if ($('#EUserType').val() == 'C') {
            $scope.DivAdministrative = false;
            $scope.DivAddressDetails = false;
        }

        $scope.SetSocietyDocuents();
    }
    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function (V) {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural == 'false' ? false : true;
        if (V != 1) {
            $scope.SocietyRegisteredOffice.CityTypes = "0";
            $scope.SocietyRegisteredOffice.CITY_ID = "0";
            $scope.SocietyRegisteredOffice.WARD_ID = "0";
            $scope.SocietyRegisteredOffice.BLOCK_ID = "0";
            $scope.SocietyRegisteredOffice.GP_ID = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_ID = "0";
        }
        if (SelectedVal == true || SelectedVal == true) {
            $scope.SocietyRegisteredOffice.BLOCK_ID = "0";
            $scope.SocietyRegisteredOffice.GP_ID = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_ID = "0";
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };

    $scope.RuralUrbanBlockCopy = function () {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralcopy == 'false' ? false : true;
        $scope.SocietyRegisteredOffice.CityTypes_copy = "0";
        $scope.SocietyRegisteredOffice.CITY_IDcopy = "0";
        $scope.SocietyRegisteredOffice.WARD_IDcopy = "0";
        $scope.SocietyRegisteredOffice.BLOCK_ID_copy = "0";
        $scope.SocietyRegisteredOffice.GP_IDcopy = "0";
        $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = "0";
        if (SelectedVal == true || SelectedVal == true) {
            $scope.UrbanDivCopy = true;
            $scope.RuralDivcopy = false;
            //$scope.GetCityListByCityTypes(2);
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = true;
            //$scope.BlockListByOffAdd(2)
        }
        else {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = false;
        }
    };

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlockForSport = function () {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralAddSports == 'false' ? false : true;
        $scope.SocietyRegisteredOffice.CityTypes_copy = "0";
        $scope.SocietyRegisteredOffice.CITY_IDcopy = "0";
        $scope.SocietyRegisteredOffice.WARD_IDcopy = "0";
        $scope.SocietyRegisteredOffice.BLOCK_ID_copy = "0";
        $scope.SocietyRegisteredOffice.GP_IDcopy = "0";
        $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = "0";
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

    $scope.RuralUrbanBlockForSportcopy = function (IsEdit) {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralSecretary == 'false' ? false : true;
        if (IsEdit != '1') {
            $scope.SocietyRegisteredOffice.CityTypesSecretary = "0";
            $scope.SocietyRegisteredOffice.CITY_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.WARD_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.BLOCK_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.GP_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_IDSecretary = "0";
        }
        if (SelectedVal == true || SelectedVal == true) {
            $scope.UrbanDivSportCopy = true;
            $scope.RuralDivSportcopy = false;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDivSportCopy = false;
            $scope.RuralDivSportcopy = true;
        }
        else {
            $scope.UrbanDivSportCopy = false;
            $scope.RuralDivSportcopy = false;
        }
    };
    $scope.IsTreasurer = function (IsEdit) {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralTreasurer == 'false' ? false : true;
        if (IsEdit != '1') {
            $scope.SocietyRegisteredOffice.CityTypesTreasurer = "0";
            $scope.SocietyRegisteredOffice.CITY_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.WARD_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.GP_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_IDTreasurer = "0";
        }

        if (SelectedVal == true || SelectedVal == true) {
            $scope.UrbanDivTreasurer = true;
            $scope.RuralDivTreasurer = false;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDivTreasurer = false;
            $scope.RuralDivTreasurer = true;
        }
        else {


            $scope.UrbanDivTreasurer = false;
            $scope.RuralDivTreasurer = false;

        }
    }

    //

    $scope.ResetDataForAddress = function (val, Level) {
        if (val == 1) {
            if (Level < 3) {
                $scope.SocietyRegisteredOffice.IsRural = undefined;
                $scope.CityListdata = [];
                $scope.Blocksdata = [];
                $scope.GramPanchayats = [];
                $scope.Villages = [];
                $scope.Ward = [];
            }
        }
        else if (val == 2) {
            if (Level < 3) {
                $scope.SocietyRegisteredOffice.IsRuralcopy = undefined;
                $scope.CityListdatacopy = [];
                $scope.Blocksdatacopy = [];
                $scope.GramPanchayatscopy = [];
                $scope.Villagescopy = [];
                $scope.Wardcopy = [];
            }
        }
    }
    //


    //start Level of office data
    $scope.Levelofficedata = function (Cflag) {

        $scope.showtabbar = true;
        $('#ddlDistrict').val(0);
        $('#ddlblocki').val(0);
        $('#ddlGP').val(0);
        $('#ddlVillage').val(0);
        $('#ddlDivision').val(0);
        $('#ddlCity').val(0);
        if ($scope.SocietyRegistration.SocietyTypeId == 9) {

            // $scope.SocietyRegistration.WorkScopeLevel_ID = 1;
            $scope.SocietyRegistration.IsStatLevel = 'Yes';
            $('#Iddstateyes').attr("disabled", "disabled");
            $('#Iddstateno').attr("disabled", "disabled");
            $scope.lstSocietyWorkScope = [];
            $scope.Addmore = false;
            $scope.hideshowhide = true;

        }
        else {

            $scope.hideshowhide = false;
            if ($scope.IsEditMode == true) {

            }
            else {
                if (Cflag == 1)
                    $scope.SocietyRegistration.IsStatLevel = undefined;
            }
            $scope.lstSocietyWorkScope = [];
            $scope.Addmore = true;

        }
        var SelectedVal = $scope.SocietyRegistration.WorkScopeLevel_ID;
        $scope.GetSocietyClass();
        if (SelectedVal == '3' || SelectedVal == '4' || SelectedVal == '5' || SelectedVal == '6') {
            if ($scope.Blocks.length == 0) {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                    success(function (data, status) {
                        $scope.Blocks = data;
                    }).
                    error(function (data, status) {
                        $scope.Blocks = [];
                    });
            }
        }
        if (SelectedVal == '7' || SelectedVal == '8') {

        }
        if (SelectedVal == '1') {
            $scope.UrbanDivRadio = false;
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.GPDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = false;
            $scope.StateDiv = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.District = [];
            $scope.SocietyRegistration.IsStatLevel == 'Yes'
            $scope.Addmore = false;
            $scope.Addmorebtn(true);
            $scope.SocietyRegistration.AreaOfOperation = "All RAJASTHAN";

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
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;
        }
        else if (SelectedVal == '3') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = true;
            $scope.GPDiv = false;
            $scope.Districtlevel3 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;

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
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;

        }
        else if (SelectedVal == '5') {
            $scope.showtabbar = true;
            $scope.BlockruralDiv = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            $scope.Addmore = true;
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;

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
            $scope.UrbanDivRadio = false;
            $scope.SocietyRegistration.AreaOfOperation = "";
        }
        else if (SelectedVal == '7' || SelectedVal == '8') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.GPDiv = false;
            $scope.BlockruralDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.UrbanDivRadio = true;
            $scope.SocietyRegistration.AreaOfOperation = "";
            if (SelectedVal == '8')
                $scope.DivRadioWard = true;
            else
                $scope.DivRadioWard = false;

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
        $scope.SetSocietyDocuents();




    };
    //End Level of office data

    $scope.Leveldatachange = function (item) {
        $scope.lstSocietyWorkScope = [];
        $scope.SocietyRegistration.Rural = null;
        $scope.SocietyRegistration.urban = null;

        if (item == '1' || item == '0') {

            $scope.RuralDivRadio = false;
            $scope.UrbanDivRadio = false;
            $scope.grid = false;
            $scope.Addmore = false;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
        }
        if (item == '2') {

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
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;

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
    //Edit Form Basic Deatils
    $scope.OldRegistrationNo = "";
    $scope.EditBasicDetailsForm = function (BasicDetails, SID, IsFirst, id, ValidationFlag) {
        $scope.RuralurbanDiv = false;
        $scope.CheckBRN = true;
        $scope.nepster = true;
        $scope.showtabbar = true;
        $scope.txtBrndisabled = false;
        $scope.OldRegistrationNo = "";
        $scope.txtRegistrationNo = true;
        BasicDetails.IsNewRegistration = BasicDetails.IsNewRegistration == null ? false : BasicDetails.IsNewRegistration;
        if (BasicDetails.IsNewRegistration == false) {
            $scope.txtRegistrationNo = false;
            $scope.OldRegistrationNo = BasicDetails.RegistrationNo;
        }
        $scope.txtSocietyName = true;
        $scope.BRNYes = true;
        $scope.SocietyAct = true;

        $scope.ClickUserVarification();
        if (IsFirst > 0) {
            $scope.UpdateLayout(IsFirst);
        }
        else {
            $scope.divTabulation = false;
            $scope.divAddBtn = false;
            $scope.divSocialProfiling = true;
            $scope.divSocietyProfilingShow = false;
        }

        $scope.all = true;
        $scope.IsEditMode = true;

        $scope.SocietyRegisteredOffice.SocietyIdEnc = SID;

        $scope.SocietyIdEnc = SID;
        $(".blockUI").fadeIn("slow");
        if (SocietyDataForEdit.length == 0) {
            $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId_v1", { "SocietyId": SID }).
                success(function (data, status) {
                    $scope.SocietyDetails(data);
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    alert("Error while fetching Society Details.");

                });
        }
        else {
            $scope.SocietyDetails(SocietyDataForEdit);
        }

    }


    $scope.SocietyDetails = function (data) {

        $scope.SocietyRegistration = [];
        if (data.length > 0) {

            var BasicDetails = data[0];
            var New = data;
            //basicdetails            
            $scope.SocietyAmendment.IsAmendment = false;
            if (BasicDetails.IsAmendmentAtProfiling == true) {

                if (BasicDetails.AmendmentType != undefined && BasicDetails.AmendmentType != "") {
                    $scope.SocietyAmendment.ChangeByeLaws = BasicDetails.AmendmentType.indexOf('1') > -1;
                    $scope.SocietyAmendment.ChangeName = BasicDetails.AmendmentType.indexOf('2') > -1;
                    $scope.SocietyAmendment.ChaneAddress = BasicDetails.AmendmentType.indexOf('3') > -1;
                    $scope.SocietyAmendment.ChaneExecutiveMember = BasicDetails.AmendmentType.indexOf('4') > -1;
                }

                $scope.SocietyAmendment.IsAmendment = true;
                // $scope.GetSocietyAmendmentDocuments(BasicDetails.SocietyIdEnc, 0);
            }
            if (BasicDetails.ActId == 7) {
                $scope.GetSportsAssociationsList();
            }
            $scope.OldRegistrationNo = BasicDetails.RegistrationNo;
            $scope.grid = true;
            $scope.SocietyRegistration = BasicDetails;
            $scope.ShowValidateBtn = false;
            var CEODetails = data.filter(function (x) { return x.UserType == 1 });
            $scope.SocietyRegistration.CEO = CEODetails.length > 0 ? CEODetails[0] : [];
            $scope.SocietyRegistration.BasicDetails = [];
            $scope.SocietyRegistration.BasicDetails = BasicDetails;
            $scope.SocietyRegistration = BasicDetails;
            $scope.SocietyDetailsData = BasicDetails;
            $scope.SocietyRegistration.ActId = BasicDetails.ActId.toString();
            $scope.ActId = BasicDetails.ActId.toString();
            var BRN = BasicDetails.BRN;
            BasicDetails.ClassId = BasicDetails.ClassId == null || BasicDetails.ClassId == undefined ? "0" : BasicDetails.ClassId.toString();
            BasicDetails.SubClassId = BasicDetails.SubClassId == null || BasicDetails.SubClassId == undefined ? "0" : BasicDetails.SubClassId.toString();
            BasicDetails.SocietyTypeId = BasicDetails.SocietyTypeId == null || BasicDetails.SocietyTypeId == undefined ? "0" : BasicDetails.SocietyTypeId.toString();
            BasicDetails.MajorActivityId = BasicDetails.MajorActivityId == null || BasicDetails.MajorActivityId == undefined ? "0" : BasicDetails.MajorActivityId.toString();
            BasicDetails.ActivityId = BasicDetails.ActivityId == null || BasicDetails.ActivityId == undefined ? "0" : BasicDetails.ActivityId.toString();
            BasicDetails.BusinessId = BasicDetails.BusinessId == null || BasicDetails.BusinessId == undefined ? "0" : BasicDetails.BusinessId.toString();

            $scope.SocietyRegistration.LiablityId = $scope.SocietyRegistration.LiablityId == null || $scope.SocietyRegistration.LiablityId == undefined ? "0" : $scope.SocietyRegistration.LiablityId.toString();

            BasicDetails.SocietyCategoryId = BasicDetails.SocietyCategoryId == null || BasicDetails.SocietyCategoryId == undefined ? "0" : BasicDetails.SocietyCategoryId.toString();
            //SocietyRegistration.SocietyTypeId

            var ClassId = BasicDetails.ClassId.toString();
            var SubClassId = BasicDetails.SubClassId == null || BasicDetails.SubClassId == undefined ? "0" : BasicDetails.SubClassId.toString();
            var SocietyCategoryId = BasicDetails.SocietyCategoryId.toString();
            $scope.showHideDiv();
            $scope.SocietyRegistration.BRN = BRN;
            $scope.divBrnbtn = true;
            if ($scope.SocietyDetailsData.BRN.length > 0 && ($scope.SocietyDetailsData.BRN != undefined || $scope.SocietyDetailsData.BRN != "")) {
                //$scope.divBrnYN = true;
                $scope.SocietyDetailsData.IsBRNYes = true;
                $scope.divBrn = true;
            }
            else {
                //$scope.divBrnYN = false;
                if ($('#EUserType').val() != 'C') {
                    $scope.SocietyDetailsData.IsBRNYes = false;
                    $scope.divBrn = false;
                }
                else {
                    $scope.SocietyDetailsData.IsBRNYes = true;
                    $scope.divBrn = true;
                }
            }

            $scope.GetSocietyTypeList();
            $scope.GetSocietyClass();
            $scope.SocietyRegistration.ClassId = BasicDetails.ClassId.toString();

            $scope.Levelofficedata();
            $scope.SocietyRegistration.ClassId = ClassId;
            $scope.GetSubClass();
            $scope.SocietyRegistration.SubClassId = SubClassId;
            $scope.GetSocietyCategory();
            $scope.SocietyRegistration.BalanceSheetExcelUpload = BasicDetails.BalanceSheetExcelFile;

            $scope.SocietyRegistration.GeneralMeetingProcedure = BasicDetails.GeneralMeetingProcedureFile;

            $scope.RegistrationCertificateFile = BasicDetails.RegistrationCertificateFile.length > 3 ? BasicDetails.RegistrationCertificateFile : '';
            $scope.RulesAndRegulationFile = BasicDetails.RulesAndRegulationFile.length > 3 ? BasicDetails.RulesAndRegulationFile : '';
            $scope.SocietyRegistration.SocietyCategoryId = SocietyCategoryId;

            $scope.BalanceSheetExcelFile = BasicDetails.BalanceSheetExcelFile.length > 3 ? BasicDetails.BalanceSheetExcelFile : '';
            $scope.GeneralMeetingProcedureFile = BasicDetails.GeneralMeetingProcedureFile.length > 3 ? BasicDetails.GeneralMeetingProcedureFile : '';
            $scope.WorkPlanEconomicViabilityFile = BasicDetails.WorkPlanEcoViabilityFile.length > 3 ? BasicDetails.WorkPlanEcoViabilityFile : '';
            $scope.MembersListExcelFile = BasicDetails.MemberListFile.length > 3 ? BasicDetails.MemberListFile : '';
            $scope.ByeLawsSocietyFile = BasicDetails.BylawsFile.length > 3 ? BasicDetails.BylawsFile : '';

            $scope.OldExecutiveCommittee = BasicDetails.OldExecutiveCommittee != undefined && BasicDetails.OldExecutiveCommittee.length > 3 ? BasicDetails.OldExecutiveCommittee : '';

            $scope.CooperativesListFile = BasicDetails.DLSCFileName.length > 3 ? BasicDetails.DLSCFileName : '';
            $scope.DLSCFileName = BasicDetails.DLSCFileName.length > 3 ? BasicDetails.DLSCFileName : '';

            //alert(BasicDetails.AGMDate);
            BasicDetails.IsNewRegistration = BasicDetails.IsNewRegistration == null ? false : BasicDetails.IsNewRegistration;
            $scope.SocietyRegistration.IsNewRegistration = BasicDetails.IsNewRegistration == null ? false : BasicDetails.IsNewRegistration;// == false ? false : true;
            $scope.IsNewRegistration = BasicDetails.IsNewRegistration;

            $scope.SocietyRegistration.IsNewRegistrationSelect = BasicDetails.IsNewRegistration ? 'true' : 'false';

            if ($scope.IsNewRegistration);
            {
                $scope.translation['MembersApplyingForTheSociety'] = 'Total Number of Member of Society';
            }
            $('#txtLiablityofMember').val(BasicDetails.MemberLiablity);
            if ($scope.ActId == 1) {
                $scope.divOtherSocietys = true;
                $scope.showrulesfile = true;
                $scope.CheckBRN = true;
                $scope.ShowSubmitBtn = true;
            }
            if (BasicDetails.ActId != 7) {
                $scope.divOtherSocietys = true;
                $scope.showrulesfile = true;
                $scope.CheckBRN = true;
                $scope.ShowSubmitBtn = true;
                $scope.SocietyRegistration.MajorActivityId = BasicDetails.MajorActivityId;
                var BusinessId = BasicDetails.BusinessId;
                $scope.GetActivity();
                $scope.SocietyRegistration.ActivityId = BasicDetails.ActivityId;
                $scope.GetBusiness();
                $scope.SocietyRegistration.BusinessId = BusinessId;
                $scope.ObjectsFileName = BasicDetails.ObjectsFile;

            }
            if (BasicDetails.ActId == 7) {

                $('#ddlgame').attr("disabled", "disabled");
                $('#txtSportSocietyName').attr("disabled", "disabled");
                $('input[name="RegistrationProfile"]').attr('disabled', 'disabled');
                $('input[name="StatLevel"]').attr('disabled', 'disabled');
                //$scope.IsSportsDistrictdisabled = true;

                $scope.divOtherSocietys = false;
                $scope.divBrn = false;
                $scope.CheckBRN = false;

                $scope.grid = false;
                $scope.gridtwo = true;
                $scope.SportsAttechments = true;

                $scope.ByeLawsSocietyFile = $scope.SocietyRegistration.BylawsFile;
                $scope.OldExecutiveCommittee = $scope.SocietyRegistration.OldExecutiveCommittee;

                $scope.SOCFileName = $scope.SocietyRegistration.SOCFileName;
                $scope.RSSCFileName = $scope.SocietyRegistration.RSSCFileName;
                $scope.MemberListFile = $scope.SocietyRegistration.MemberListFile;
                $scope.DLSCFileName = $scope.SocietyRegistration.DLSCFileName;

                $scope.SocietyRegistration.IsStatLevel = BasicDetails.ISRDSSC == 'State Level' ? 'Yes' : 'No';
                $scope.AddMoreForSportShow = true;
                $scope.FileSOC = true;

                if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {

                    $scope.LevelDistricshow = true;
                    $scope.primarydiv = false;
                    $scope.LevelDistricsHide = false;
                    $scope.SportSocietyCityType = false;
                    $scope.UrbanForSportSocietyNew = false;
                    $scope.UrbanForSportSociety = false;

                    $scope.DistrictDiv = true;
                    $scope.District = false;
                    $scope.Districtt = true;
                    $scope.RuralForSportSociety = false;
                    $scope.PrimaryBodyDetails = true;
                    $scope.PrimaryBodyName = false;
                    $scope.GetDistrictWithoutDivisionList();
                    $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the District Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् से अवगत कराये|');
                    $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the District Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् से अवगत कराये|');
                    $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Rajasthan State Olympic Association Affiliation ?' : 'राज्य ओलंपिक परिषद से संबद्धता की स्थिति चुने?');
                    $scope.AffiliatedByRSSC = true;
                    $scope.divSportSocietyName = true;
                    $scope.AffiliationByDLSC = false;
                    $scope.FileDLSC = false;
                    $scope.FileRSSC = true;
                    $scope.FileDOC = false;
                }
                else {

                    $scope.RuralurbanDiv = true;
                    //$scope.BlockList();
                    $scope.DistrictDiv = true;
                    $scope.District = true;
                    $scope.Districtt = false;
                    $scope.GetDistrictWithoutDivisionList();
                    $scope.UrbanForSportSociety = false;
                    $scope.LevelDistricshow = false;
                    $scope.LevelDistricsHide = true;
                    $scope.PrimaryBodyName = true;
                    $scope.PrimaryBodyDetails = true;

                    $scope.primarydiv = true;
                    $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the Primary Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली प्राथमिक क्रीड़ा निकायों से अवगत कराये|');
                    $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of District Olympic Association Affiliation ?' : 'जिला ओलंपिक परिषद से संबद्धता की स्थिति चुने?');
                    $scope.AffiliatedByRSSC = true;
                    $scope.AffiliationByDLSC = true;
                    $scope.divSportSocietyName = true;
                    $scope.FileRSSC = true;
                    $scope.FileDLSC = true;
                    $scope.FileDOC = true;
                    $scope.SportsAttechments = true;
                    $scope.FileSLSA = true;
                    $scope.ShowRegDateAndNoForSLSAAffiliationYes = true;
                }

                $scope.SocietyRegistration.SocietyName = BasicDetails.SocietyName;
                $scope.SocietyRegistration.ElectionDate = BasicDetails.ElectionDate;
                $('#txtSportAgmDate').val(BasicDetails.AGMDate.indexOf('9999') > -1 ? '' : BasicDetails.AGMDate);
                if ($scope.SocietyRegistration.RegistrationNo.length > 0)
                    $scope.SocietyRegistration.ClickYes = true;
                else
                    $scope.SocietyRegistration.ClickYes = false;


                $http.get(SiteUrl + "/SportAssociation/SportAssociationList/?WhereClause=" + "").
                    success(function (data, status) {
                        $scope.SportAssociationList = data;
                        angular.forEach($scope.SportAssociationList, function (item) {
                            if (item.AssociationId == $scope.SocietyRegistration.SportsAssociationId) {
                                $scope.SocietyRegistration.AssociationId = item.AssociationIdStr;
                            }
                        });

                        $scope.SocietyRegistration.IsAffiliatedBySOC = BasicDetails.IsAffiliatedBySOC == 'NO' ? 'No' : 'Yes';
                        $scope.ShowRegDateAndNoForSOCAffiliation(1);
                        var TempFlag = $scope.IsAffiliatedBySOC;
                        $scope.SocietyRegistration.IsAffiliatedByDLSC = BasicDetails.IsAffiliatedByDLSC == 'NO' ? 'No' : 'Yes';
                        var TempName = $scope.SocietyRegistration.SLSAFileName;
                        $scope.ShowDivSportSocietyName();
                        $scope.SocietyRegistration.SLSAFileName = TempName;
                        $scope.IsAffiliatedBySOC = TempFlag;
                        $scope.ShowRegDateAndNoForDLSCffiliation();

                        $scope.SocietyRegistration.IsAffiliationBySLSA = BasicDetails.IsAffiliationBySLSA == 'NO' ? 'No' : 'Yes';
                        $scope.ShowRegDateAndNoForSLSAffiliation(1);
                        $scope.SocietyRegistration.SLSARegNo = BasicDetails.SLSARegNo;
                        $scope.SocietyRegistration.SLSAAffiliatedDate = BasicDetails.SLSAAffiliatedDate;


                        $scope.MembersListExcelFile = BasicDetails.MemberListFile;
                        $scope.ByeLawsSocietyFile = BasicDetails.BylawsFile;
                        $scope.OldExecutiveCommittee = BasicDetails.OldExecutiveCommittee;
                        $scope.DLSCFileName = BasicDetails.DLSCFileName;
                        $scope.SOCFileName = BasicDetails.SOCFileName;
                        $scope.SLSAFileName = BasicDetails.SLSAFileName;
                        $scope.RSSCFileName = BasicDetails.RSSCFileName;

                        $scope.showrulesfile = false;
                        $scope.grid = false;
                        $scope.ShowSubmitBtn = true;
                        $scope.SocietyRegistration["IsSportsNewRegistrationSelect"] = '';
                        $scope.SocietyRegistration.IsSportsNewRegistrationSelect = BasicDetails.IsNewRegistration ? 'true' : 'false';
                        // by n saini
                        $scope.IsProfiling = true;
                        if ((BasicDetails.IsNewRegistration == true || BasicDetails.IsNewRegistration == true)) {
                            $scope.IsProfiling = false;
                        }
                        $scope.SetSocietyDocuents();
                        $scope.SetUplodedDocuments(BasicDetails);
                        $scope.GetUplodedOthersDocuments(BasicDetails.SocietyID, BasicDetails.SocietyDirectory);
                    }).
                    error(function (data, status) {
                        $scope.SportAssociationList = [];
                    });
            }

            $scope.SocietyRegistration.RegistrationDate = $scope.SocietyRegistration.RegistrationDate == null ? "" : $scope.SocietyRegistration.RegistrationDate;
            $scope.SocietyRegistration.ElectionDate = $scope.SocietyRegistration.ElectionDate == null ? "" : $scope.SocietyRegistration.ElectionDate;
            $scope.SocietyRegistration.AGMDate = $scope.SocietyRegistration.AGMDate == null ? "" : $scope.SocietyRegistration.AGMDate;

            $scope.SocietyRegistration.RegistrationDate = $scope.SocietyRegistration.RegistrationDate.indexOf('9999') > -1 ? '' : $scope.SocietyRegistration.RegistrationDate;
            $scope.SocietyRegistration.ElectionDate = $scope.SocietyRegistration.ElectionDate.indexOf('9999') > -1 ? '' : $scope.SocietyRegistration.ElectionDate;
            $scope.SocietyRegistration.AGMDate = $scope.SocietyRegistration.AGMDate.indexOf('9999') > -1 ? '' : $scope.SocietyRegistration.AGMDate;
            $('#txtAGMDate').val(BasicDetails.AGMDate.indexOf('9999') > -1 ? '' : BasicDetails.AGMDate);
            $scope.IsProfiling = true;
            if (($scope.SocietyRegistration.IsNewRegistration == true)) {
                $scope.IsProfiling = false;
            }
            $scope.SetSocietyDocuents();
            //ADMIN DETAILS

            $scope.DivAdministrative = true;
            var ActId = BasicDetails.ActId;
            $scope.SocietyRegistration.TotalMembersClassA = BasicDetails.TotalMembersClassA;
            $scope.SocietyRegistration.TotalMembersClassB = BasicDetails.TotalMembersClassB;
            $scope.SocietyRegistration.TotalMembersClassC = BasicDetails.TotalMembersClassC;
            var AdministratorDetails = data.filter(function (x) { return x.UserType == 3 });//Appointed Administrator
            //LIQvideter 
            var liqDetails = [];
            liqDetails = data.filter(function (x) { return x.UserType == 6 });
            //END

            if (AdministratorDetails.length > 0) {
                $scope.SocietyDetailsData.IsAdministratorAppointed = $scope.translation.Yes;
                $scope.SocietyRegistration.IsAdministrative = 'Administrative';
                $scope.IsAdministrativeN = false;
                $scope.IsAdministrativeY = true;
                Isliquidator = false;
                $scope.SocietyRegistration.AppointedName = AdministratorDetails[0].Name;
                if (AdministratorDetails[0].AppointedDate.indexOf("9999") > 0)
                    $('#txtAppointedDOB').val(AdministratorDetails[0].DOB);
                else
                    $('#txtAppointedDOB').val(AdministratorDetails[0].AppointedDate);

                $scope.SocietyRegistration.AppointedPostName = AdministratorDetails[0].POSTNAME;
            }
            else if (liqDetails.length > 0) {
                $scope.SocietyRegistration.IsAdministrative = 'liquidator';
                Isliquidator = true;
                $scope.IsAdministrativeN = false;
                $scope.IsAdministrativeY = false;
                $scope.SocietyRegistration.liquidatorName = liqDetails[0].Name;
                $('#txtliquidatorAppointedDate').val(liqDetails[0].AppointedDate);
                $scope.SocietyRegistration.liquidatorPostName = liqDetails[0].POSTNAME;
                $scope.SocietyRegistration.liquidatorContactNo = liqDetails[0].ContactNo;
            }
            else {
                $scope.SocietyRegistration.IsAdministratorAppointed = $scope.translation.No;
                $scope.IsAdministrativeN = true;
                $scope.IsAdministrativeY = false;
                Isliquidator = false;
                $scope.SocietyRegistration.IsAdministrative = 'Board';
            }

            ////Elected /Nomineted

            var ALLMEMBERS = [];
            var NOMominatedMember = [];

            if (ActId != 7) {
                ALLMEMBERS = data.filter(function (x) { return (x.IsSameCEOAddress == true) && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                if (ALLMEMBERS.length == 0) {
                    ALLMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                }
            }
            else if (ActId == 7) {
                ALLMEMBERS = data.filter(function (x) { return (x.IsSameCEOAddress == true) && (x.AddressFor == '0') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                if (ALLMEMBERS.length == 0) {
                    ALLMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                }
                //Nomi_ALLMEMBERS = [];
                //Nomi_ALLMEMBERS = data.filter(function (x) { return x.IsSameCEOAddress == true && x.MEMBERTYPE == 'NOMINATEDMEMBERS' });
                Nomi_ALLMEMBERS = data.filter(function (x) { return x.IsSameCEOAddress == true && (x.AddressFor == '0') && x.MEMBERTYPE == 'NOMINATEDMEMBERS' });

                //$scope.translation.NominatedMember = 'Nominated Member';
            }

            if (ActId == 3) {
                $scope.Act2001 = true;
                Nomi_ALLMEMBERS = data.filter(function (x) { return x.IsSameCEOAddress == true && x.MEMBERTYPE == 'NOMINATEDMEMBERS' });
                //$scope.IsNewRegistration = false;
                var Administrative = data.filter(function (x) { return x.IsSameCEOAddress == true && x.BoardType == 'Administrative' });//Appointed Administrator           

                if (Administrative.length > 0) {
                    $scope.translation.NominatedMember = 'Appointed Member';
                }
                else {
                    $scope.translation.NominatedMember = 'Nominated Member';
                }
            }

            if (ALLMEMBERS.length > 0) {
                $scope.SocietyRegistration.IsAdministrative = 'Board';

                if ((ALLMEMBERS[0].IsNewRegistration == false) && ALLMEMBERS[0].ActId == 3) {
                    $scope.TwoThousandOneProfilling = true;
                    $scope.SocietyRegistration.IsNewRegistration = false;
                }
                else {
                    $scope.TwoThousandOneProfilling = false;
                    $scope.SocietyRegistration.IsNewRegistration = ALLMEMBERS[0].IsNewRegistration;
                }
            }

            $scope.SetSocietyDocuents();

            var TableData10NomiAll = [];
            var TableData10All = [];

            var X = 7;
            if (ActId > 0) {

                var TableData10 = [];
                if (ActId == 1)
                    X = (ALLMEMBERS.length > 6 ? ALLMEMBERS.length : 7)
                else if (ActId == 7)
                    X = (ALLMEMBERS.length > 4 ? ALLMEMBERS.length : 5)
                else {
                    var Administrative = data.filter(function (x) { return x.IsSameCEOAddress == true && x.BoardType == 'Administrative' });
                    var liquidator = data.filter(function (x) { return x.IsSameCEOAddress == true && x.MEMBERTYPE == 'LIQUIDATOR' }); //Appointed LIQUIDATOR 
                    if (Administrative.length > 0) {
                        X = 2;
                    }
                    else if (liquidator.length > 0) {
                        X = 0;
                    }
                    else {
                        X = (ALLMEMBERS.length > 14 ? ALLMEMBERS.length : 15)
                    }
                }

                for (var x = 0; x < ALLMEMBERS.length; x++) {
                    ALLMEMBERS[x].CategoryId = ALLMEMBERS[x].CategoryId == null ? "" : ALLMEMBERS[x].CategoryId.toString();
                    ALLMEMBERS[x].Occupation = ALLMEMBERS[x].Occupation == null ? "" : ALLMEMBERS[x].Occupation.toString();
                    ALLMEMBERS[x].loaneeType = ALLMEMBERS[x].loaneeType == null ? "" : ALLMEMBERS[x].loaneeType.toString();

                    TableData10.push(
                        {
                            "AdministrativeId": ALLMEMBERS[x].AdministrativeId,
                            "Id": x + 1,
                            "Name": ALLMEMBERS[x].Name,
                            "NameOfSociety": ALLMEMBERS[x].NameOfSociety,
                            "PostinSociety": ALLMEMBERS[x].PostinSociety,
                            "POSTNAME": ALLMEMBERS[x].POSTNAME,
                            "FatherName": ALLMEMBERS[x].FatherName,
                            "Address": ALLMEMBERS[x].Address,
                            "Aadh": ALLMEMBERS[x].AadharNo2,//AadharNo
                            "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                            "DOB": ALLMEMBERS[x].DOB,
                            "ContactNo": ALLMEMBERS[x].ContactNo,
                            "Photo": ALLMEMBERS[x].Photo,
                            "CategoryId": ALLMEMBERS[x].CategoryId.toString(),
                            "Occupation": ALLMEMBERS[x].Occupation.toString(),
                            "CatId": ALLMEMBERS[x].CategoryId,
                            "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false,
                            "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                            "MembershipNo": ALLMEMBERS[x].MembershipNo,
                            "ShareAllotted": ALLMEMBERS[x].ShareAllotted,
                            "ShareCapitalAmount": ALLMEMBERS[x].ShareCapitalAmount,
                            "IsEdit": true,
                            "loaneeType": ALLMEMBERS[x].loaneeType != undefined && ALLMEMBERS[x].loaneeType != null ? ALLMEMBERS[x].loaneeType.toString() : "0",
                            "loaneeType1": ALLMEMBERS[x].loaneeType != undefined && ALLMEMBERS[x].loaneeType != null ? ALLMEMBERS[x].loaneeType.toString() : "0",
                            "Isvalidate": true
                        });
                }

                for (var x = ALLMEMBERS.length; x < X; x++) {
                    TableData10.push(
                        {
                            "AdministrativeId": "0",
                            "Id": x + 1,
                            "Name": '',
                            "FatherName": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "CategoryName": '',
                            "Occupation": '',
                            "CatId": "0",
                            "Status": false,
                            "MEMBERTYPE": '',
                            "Address": '',
                            "IsEdit": false,
                            "CategoryId": "0",
                            "Isvalidate": false

                        });
                }
                $scope.TableData10 = TableData10;
            }
            else {
                X = (ALLMEMBERS.length > 14 ? ALLMEMBERS.length : 15)
                for (var x = 0; x < ALLMEMBERS.length; x++) {

                    ALLMEMBERS[x].CategoryId = ALLMEMBERS[x].CategoryId == null ? "" : ALLMEMBERS[x].CategoryId.toString();
                    TableData10All.push(
                        {
                            "AdministrativeId": ALLMEMBERS[x].AdministrativeId,
                            "Id": x + 1,
                            "Name": ALLMEMBERS[x].Name,
                            "NameOfSociety": ALLMEMBERS[x].NameOfSociety,
                            "PostinSociety": ALLMEMBERS[x].PostinSociety,
                            "FatherName": ALLMEMBERS[x].FatherName,
                            "POSTNAME": ALLMEMBERS[x].POSTNAME,
                            "Aadh": ALLMEMBERS[x].AadharNo2,//AadharNo,
                            "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                            "DOB": ALLMEMBERS[x].DOB,
                            "ContactNo": ALLMEMBERS[x].ContactNo,
                            "Photo": ALLMEMBERS[x].Photo,
                            "CategoryName": ALLMEMBERS[x].Category,
                            "Occupation": ALLMEMBERS[x].Occupation,
                            "CategoryId": ALLMEMBERS[x].CategoryId.toString(),
                            "CatId": ALLMEMBERS[x].CategoryId.toString(),
                            "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false,
                            "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                            "IsEdit": true,
                            "Address": ALLMEMBERS[x].Address,
                            "Isvalidate": true
                        });
                }

                for (var x = ALLMEMBERS.length; x < X; x++) {
                    TableData10All.push(
                        {
                            "AdministrativeId": "0",
                            "Id": x + 1,
                            "Name": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '',
                            "CategoryName": '',
                            "Occupation": '',
                            "CatId": "0",
                            "Status": false,
                            "MEMBERTYPE": '',
                            "FatherName": '',
                            "Address": '',
                            "IsEdit": false,
                            "CategoryId": "0",
                            "Isvalidate": false
                        });
                }
                $scope.TableData10All = TableData10All;
            }

            $("#TblElectedMember").find("input,textarea,select").attr("disabled", "disabled");

            //Nominated Member

            if (ActId != 1) {
                for (var x = 0; x < Nomi_ALLMEMBERS.length; x++) {
                    Nomi_ALLMEMBERS[x].CategoryId = Nomi_ALLMEMBERS[x].CategoryId == null ? "" : Nomi_ALLMEMBERS[x].CategoryId.toString();
                    TableData10NomiAll.push(
                        {
                            "AdministrativeId": Nomi_ALLMEMBERS[x].AdministrativeId,
                            "Id": x + 500,
                            "Name": Nomi_ALLMEMBERS[x].Name,
                            "NameOfSociety": Nomi_ALLMEMBERS[x].NameOfSociety,
                            "PostinSociety": Nomi_ALLMEMBERS[x].PostinSociety,
                            "FatherName": Nomi_ALLMEMBERS[x].FatherName,
                            "POSTNAME": Nomi_ALLMEMBERS[x].POSTNAME,
                            "Aadh": Nomi_ALLMEMBERS[x].AadharNo2,//AadharNo,
                            "Gender": Nomi_ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                            "DOB": Nomi_ALLMEMBERS[x].DOB,
                            "ContactNo": Nomi_ALLMEMBERS[x].ContactNo,
                            "Photo": Nomi_ALLMEMBERS[x].Photo,
                            "CategoryName": Nomi_ALLMEMBERS[x].Category,
                            "Occupation": Nomi_ALLMEMBERS[x].Occupation,
                            "CategoryId": Nomi_ALLMEMBERS[x].CategoryId.toString(),
                            "CatId": Nomi_ALLMEMBERS[x].CategoryId.toString(),
                            "Status": Nomi_ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false,
                            "MEMBERTYPE": Nomi_ALLMEMBERS[x].MEMBERTYPE,
                            "MembershipNo": Nomi_ALLMEMBERS[x].MembershipNo,
                            "ShareAllotted": Nomi_ALLMEMBERS[x].ShareAllotted,
                            "ShareCapitalAmount": Nomi_ALLMEMBERS[x].ShareCapitalAmount,
                            "IsEdit": true,
                            "loaneeType": ($scope._language == 'en-us' ? Nomi_ALLMEMBERS[x].loaneeTypeEn : Nomi_ALLMEMBERS[x].loaneeTypeHi),
                            "Address": Nomi_ALLMEMBERS[x].Address,
                            "Isvalidate": true
                        });
                }
                for (var x = Nomi_ALLMEMBERS.length; x < 1; x++) {
                    TableData10NomiAll.push(
                        {
                            "AdministrativeId": "0",
                            "Id": x + 500,
                            "Name": '',
                            "FatherName": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '',
                            "CategoryName": '',
                            "Occupation": '',
                            "CatId": "0",
                            "Status": false,
                            "MEMBERTYPE": '',
                            "Address": '',
                            "IsEdit": false,
                            "CategoryId": "0",
                            "Isvalidate": false
                        });
                }
                $scope.TableData10NomiAll = TableData10NomiAll;
            }
            //END

            if (liqDetails.length == 0 && BasicDetails.ActId != 7) {
                if (ALLMEMBERS.length > 0) {
                    $scope.SocietyRegistration.IsAdministrative = 'Board';
                    $scope.CEO = true;
                    $scope.IsAdministrativeY = false;
                    $scope.Isliquidator = false;
                    $('#txtCEOAADHAARNo').attr("disabled", "disabled");
                    $('#txtChairmanAADHAARNo').attr("disabled", "disabled");
                    $('#txtliquidatorAADHAARNo').removeAttr("disabled");
                }
                else if (AdministratorDetails.length > 0) {
                    $('#txtCEOAADHAARNo').attr("disabled", "disabled");
                    $('#txtChairmanAADHAARNo').attr("disabled", "disabled");
                    $('#txtliquidatorAADHAARNo').removeAttr("disabled");
                    $scope.SocietyRegistration.IsAdministrative = 'Administrative';
                    $scope.CEO = true;
                    $scope.AppointedYN = true;
                    $scope.SocietyRegistration.IsAdministratorAppointed = true;
                    $scope.IsAdministrativeY = true;
                    $scope.IsAdministrativeN = false;
                    $scope.Isliquidator = false;
                }
                else if (BasicDetails.ActId > 1 && MemberDetails.length == 0 && liqDetails.length == 0 && (AdministratorDetails.length > 0)) {
                    //$scope.SocietyRegistration.IsAdministrative = 'Administrative';
                    //$scope.AppointedYN = AdministratorDetails.length > 0 ? true : false;
                    //$scope.IsAdministrativeY = true;
                    //$scope.IsAdministrativeN = false;
                    //$scope.CEO = true;


                    //if (AdministratorDetails.length == 0) {
                    //    $('input:radio[name=AdministrativeBoard]')[1].checked = true;
                    //    $('#txtAppointedAADHAARNo').removeAttr("disabled", "disabled");                        
                    //}
                    //else {
                    //    $('input:radio[name=AdministrativeBoard]')[0].checked = true;
                    //    $('#txtAppointedAADHAARNo').attr("disabled", "disabled");
                    //}

                    $scope.AppointedShowHide();
                }
                else if (BasicDetails.ActId == 1 && MemberDetails.length == 0 && liqDetails.length == 0) {
                    if (BasicDetails.ActId == 1) {
                        $scope.SocietyRegistration.IsAdministrative = 'Board';
                        $scope.AdministrativeBoard();
                    }
                }

                $scope.SocietyDetailsData.liq = liqDetails.length > 0 ? liqDetails[0] : [];
            }
            else {

            }
            //START Appointed
            if (AdministratorDetails.length > 0 && AdministratorDetails[0].PostID != undefined) {
                $scope.TreasurerProfile = true;
                $('#txtTreasurerAadhar').attr("disabled", "disabled");
                $('#txtSPChairmanName').attr("disabled", "disabled");
                $scope.GETTREASURER = false;
                $scope.TREASURERRemove = true;
                $('#txtAppointedContactNo').attr("disabled", "disabled");
                $('#ddlAppointedSPORTCategory').attr("disabled", "disabled");

                $scope.SocietyRegistration.IsAdministratorAppointed = true;
                $scope.SocietyRegistration.AppointedName = AdministratorDetails[0].Name;
                $scope.SocietyRegistration.AppointedContactNo = AdministratorDetails[0].ContactNo;
                $scope.SocietyRegistration.AppointedAADHAARNo = AdministratorDetails[0].AadharNo2;
                $scope.SocietyRegistration.AppointedDOB = AdministratorDetails[0].DOB;
                $scope.SocietyRegistration.AppointedGender = AdministratorDetails[0].Gender;
                $scope.AppointedFile = AdministratorDetails[0].Photo;
                $scope.SocietyRegistration.AppointedFile = AdministratorDetails[0].Photo;
                $scope.SocietyRegistration.AppointedCategoryId = AdministratorDetails[0].CategoryId;
                $scope.SocietyRegistration.AppointedPostId = AdministratorDetails[0].PostID;
                $scope.AppointedShowHide();
            }
            else {
                $scope.SocietyRegistration.IsAdministratorAppointed = false;
                $scope.AppointedShowHide();
            }
            //END
            if ($scope.SocietyRegistration.ActId != 7) {


            }

            //liquidator DATA BIND

            if (liqDetails.length > 0 && liqDetails[0].ActId != undefined) {
                $scope.SocietyIdForAdministrativeDetails = liqDetails[0].SocietyIdEnc;
                $scope.SocietyRegistration.ActId = liqDetails[0].ActId;
                $scope.SocietyDirectory = liqDetails[0].SocietyDirectory;
                $scope.SocietyRegistration.liquidatorName = liqDetails[0].Name;
                $scope.SocietyRegistration.liquidatorContactNo = liqDetails[0].ContactNo;
                $scope.SocietyRegistration.liquidatorAADHAARNo = liqDetails[0].AadharNo2;
                $scope.SocietyRegistration.liquidatorDOB = liqDetails[0].DOB;
                $scope.SocietyRegistration.liquidatorGender = liqDetails[0].Gender;
                $scope.SocietyRegistration.liquidatorAppointedDate = liqDetails[0].AppointedDate;
                // $scope.ChairmanFile = liqDetails.Photo;
                $scope.SocietyRegistration.liquidatorFile = liqDetails[0].Photo;
                $scope.SocietyRegistration.liquidatorCategoryId = liqDetails[0].CategoryId;
                $scope.SocietyRegistration.liquidatorPostId = liqDetails[0].PostID;
                $scope.SocietyRegistration.IsAdministrative = 'liquidator';
                $scope.Isliquidator = true;
                $scope.IsAdministrativeN = false;
                $scope.IsAdministrativeY = false;
                $scope.AppointedYN = false;
            }
            //END 

            //END

            //ADDRESS START
            $scope.DivAddressDetails = true;
            if (BasicDetails.RegistrationNo == null || BasicDetails.RegistrationNo == "" || BasicDetails.RegistrationNo == 'undefined') {
                //$scope.IsAddressEditAble = true;
            }
            else if (BasicDetails.RegistrationNo != null && BasicDetails.RegistrationNo != 'undefined' && BasicDetails.RegistrationNo != "" && BasicDetails.RegistrationNo.length > 3) {
                if (BasicDetails.IsNewRegistration == false) {
                    //$scope.IsAddressEditAble = true;
                }
                else if (BasicDetails.IsNewRegistration == true) {
                    {
                        // $scope.IsAddressEditAble = false;
                    }
                }
            }
            var AdministratorDetailsOff = data.filter(function (x) { return x.UserType == 3 });//Appointed Administrator
            var liqDetailsOff = [];
            liqDetailsOff = data.filter(function (x) { return x.UserType == 6 });
            if (AdministratorDetailsOff.length > 0) {
                $scope.SocietyRegistration.IsAdministrative = 'Administrative';
            }
            else if (liqDetailsOff.length > 0) {
                $scope.SocietyRegistration.IsAdministrative = 'liquidator';
            }
            else {
                $scope.SocietyRegistration.IsAdministrative = 'Board';
            }

            var BasicDetailsOffice = New.filter(function (x) { return x.AddressFor == 0 });
            var BasicDetailsCEO = New.filter(function (x) { return x.AddressFor == 9 });

            //$scope.SocietyRegisteredOffice = BasicDetailsOffice[0];
            //$scope.SocietyRegisteredOffice = [];
            //OFFICE                        
            if (BasicDetailsOffice.length > 0) {

                BasicDetailsOffice[0].CITY_ID = BasicDetailsOffice[0].CITY_ID == null ? "" : BasicDetailsOffice[0].CITY_ID.toString();
                BasicDetailsOffice[0].STATE_ID = BasicDetailsOffice[0].STATE_ID == null ? "" : BasicDetailsOffice[0].STATE_ID.toString();
                BasicDetailsOffice[0].WARD_ID = BasicDetailsOffice[0].WARD_ID == null ? "" : BasicDetailsOffice[0].WARD_ID.toString();
                BasicDetailsOffice[0].DIVISION_ID = BasicDetailsOffice[0].DIVISION_ID == null ? "" : BasicDetailsOffice[0].DIVISION_ID.toString();
                BasicDetailsOffice[0].DISTRICT_ID = BasicDetailsOffice[0].DISTRICT_ID == null ? "" : BasicDetailsOffice[0].DISTRICT_ID.toString();
                BasicDetailsOffice[0].BLOCK_ID = BasicDetailsOffice[0].BLOCK_ID == null ? "" : BasicDetailsOffice[0].BLOCK_ID.toString();
                BasicDetailsOffice[0].GP_ID = BasicDetailsOffice[0].GP_ID == null ? "" : BasicDetailsOffice[0].GP_ID.toString();
                BasicDetailsOffice[0].VILLAGE_ID = BasicDetailsOffice[0].VILLAGE_ID == null ? "" : BasicDetailsOffice[0].VILLAGE_ID.toString();



                $scope.SocietyRegisteredOffice.IsRural = BasicDetailsOffice[0].IsRural ? 'true' : 'false';
                $scope.SocietyRegisteredOffice.HouseBuildingNo = BasicDetailsOffice[0].HouseBuildingNo;
                $scope.SocietyRegisteredOffice.LaneStreetName = BasicDetailsOffice[0].LaneStreetName;
                $scope.SocietyRegisteredOffice.Locality = BasicDetailsOffice[0].Locality;
                $scope.SocietyRegisteredOffice.PinCodecopy = BasicDetailsOffice[0].PinCode == '' ? '0' : BasicDetailsOffice[0].PinCode;
                $scope.SocietyRegisteredOffice.DIVISION_ID = BasicDetailsOffice[0].DIVISION_ID.toString();
                $scope.GetDistrictList(1);
                $scope.SocietyRegisteredOffice.DISTRICT_ID = BasicDetailsOffice[0].DISTRICT_ID.toString();

                $scope.SocietyRegisteredOffice.PinCode = BasicDetailsOffice[0].PinCode;
                if (BasicDetailsOffice[0].IsRural == false || BasicDetailsOffice[0].IsRural == false) {
                    $scope.RuralUrbanBlock(1);
                    //$scope.BlockListByOffAdd(1);
                    $scope.SocietyRegisteredOffice.BLOCK_ID = BasicDetailsOffice[0].BLOCK_ID.toString();
                    $scope.GramPanchayatList(1);
                    $scope.SocietyRegisteredOffice.GP_ID = BasicDetailsOffice[0].GP_ID.toString();
                    $scope.VillageList(1);
                    $scope.SocietyRegisteredOffice.VILLAGE_ID = BasicDetailsOffice[0].VILLAGE_ID.toString();
                    $scope.SocietyRegisteredOffice.STATE_ID = BasicDetailsOffice[0].STATE_ID.toString();

                }
                else {
                    $scope.SocietyRegisteredOffice.IsRural = BasicDetailsOffice[0].IsRural ? 'true' : 'false';


                    $scope.RuralUrbanBlock(1);
                    $scope.SocietyRegisteredOffice.CITY_ID = BasicDetailsOffice[0].CITY_ID.toString();
                    $scope.GetWardList(1);
                    $scope.SocietyRegisteredOffice.WARD_ID = BasicDetailsOffice[0].WARD_ID.toString();
                }

            }
            //OFFICE END

            //CEO START
            if (BasicDetailsCEO.length > 0) {
                BasicDetailsCEO[0].CITY_ID = BasicDetailsCEO[0].CITY_ID == null ? "" : BasicDetailsCEO[0].CITY_ID.toString();
                BasicDetailsCEO[0].STATE_ID = BasicDetailsCEO[0].STATE_ID == null ? "" : BasicDetailsCEO[0].STATE_ID.toString();
                BasicDetailsCEO[0].WARD_ID = BasicDetailsCEO[0].WARD_ID == null ? "" : BasicDetailsCEO[0].WARD_ID.toString();
                BasicDetailsCEO[0].DIVISION_ID = BasicDetailsCEO[0].DIVISION_ID == null ? "" : BasicDetailsCEO[0].DIVISION_ID.toString();
                BasicDetailsCEO[0].DISTRICT_ID = BasicDetailsCEO[0].DISTRICT_ID == null ? "" : BasicDetailsCEO[0].DISTRICT_ID.toString();
                BasicDetailsCEO[0].BLOCK_ID = BasicDetailsCEO[0].BLOCK_ID == null ? "" : BasicDetailsCEO[0].BLOCK_ID.toString();
                BasicDetailsCEO[0].GP_ID = BasicDetailsCEO[0].GP_ID == null ? "" : BasicDetailsCEO[0].GP_ID.toString();
                BasicDetailsCEO[0].VILLAGE_ID = BasicDetailsCEO[0].VILLAGE_ID == null ? "" : BasicDetailsCEO[0].VILLAGE_ID.toString();


                if (BasicDetailsCEO[0].IsRural == false || BasicDetailsCEO[0].IsRural == false) {
                    $scope.SocietyRegisteredOffice.IsRuralcopy = BasicDetailsCEO[0].IsRural ? 'true' : 'false';
                    //$scope.SocietyRegisteredOffice.CEONotCopy = BasicDetailsCEO.length > 0 ? BasicDetailsCEO[0] : 'N/A';
                    $scope.SocietyRegisteredOffice.HouseBuildingNocopy = BasicDetailsCEO[0].HouseBuildingNo;
                    $scope.SocietyRegisteredOffice.LaneStreetNamecopy = BasicDetailsCEO[0].LaneStreetName;
                    $scope.SocietyRegisteredOffice.Localitycopy = BasicDetailsCEO[0].Locality;

                    $scope.RuralUrbanBlockCopy(2);
                    $scope.SocietyRegisteredOffice.DIVISION_ID_copy = BasicDetailsCEO[0].DIVISION_ID.toString();
                    $scope.GetDistrictList(2);
                    $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = BasicDetailsCEO[0].DISTRICT_ID.toString();
                    $scope.BlockListByOffAdd(2);
                    $scope.SocietyRegisteredOffice.BLOCK_ID_copy = BasicDetailsCEO[0].BLOCK_ID.toString();
                    $scope.GramPanchayatList(2);
                    $scope.SocietyRegisteredOffice.GP_IDcopy = BasicDetailsCEO[0].GP_ID.toString();
                    $scope.VillageList(2);
                    $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = BasicDetailsCEO[0].VILLAGE_ID.toString();
                    $scope.SocietyRegisteredOffice.PinCodecopy = BasicDetailsCEO[0].PinCode;
                    $scope.SocietyRegisteredOffice.STATE_IDcopy = BasicDetailsCEO[0].STATE_ID.toString();
                }
                else {
                    $scope.SocietyRegisteredOffice.IsRuralcopy = BasicDetailsCEO[0].IsRural ? 'true' : 'false';

                    $scope.SocietyRegisteredOffice.HouseBuildingNocopy = BasicDetailsCEO[0].HouseBuildingNo;
                    $scope.SocietyRegisteredOffice.LaneStreetNamecopy = BasicDetailsCEO[0].LaneStreetName;
                    $scope.SocietyRegisteredOffice.Localitycopy = BasicDetailsCEO[0].Locality;
                    //$scope.SocietyRegisteredOffice.CEONotCopy = BasicDetailsCEO.length > 0 ? BasicDetailsCEO[0] : 'N/A';

                    $scope.SocietyRegisteredOffice.DIVISION_ID_copy = BasicDetailsCEO[0].DIVISION_ID.toString();
                    $scope.GetDistrictList(2);
                    $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = BasicDetailsCEO[0].DISTRICT_ID.toString();
                    $scope.RuralUrbanBlockCopy(2);


                    $scope.SocietyRegisteredOffice.CITY_IDcopy = BasicDetailsCEO[0].CITY_ID.toString();
                    $scope.GetWardList(2);
                    $scope.SocietyRegisteredOffice.WARD_IDcopy = BasicDetailsCEO[0].WARD_ID.toString();
                    $scope.SocietyRegisteredOffice.PinCodecopy = BasicDetailsCEO[0].PinCode;
                    $scope.SocietyRegisteredOffice.STATE_IDcopy = BasicDetailsCEO[0].STATE_ID.toString();
                }
            }



            //CEO END
            if (BasicDetailsOffice.length > 0) {
                $scope.ActId = BasicDetailsOffice[0].ActId;
                $scope.SocietyRegisteredOffice.SocietyIdEnc = BasicDetailsOffice[0].SocietyIdEnc;
            }
            //Umesh Work on Sport show
            if ($scope.ActId == 7) {
                //For Secatery                            
                $scope.RuralUrbanBlockForSportcopy();
                var Secterydata = New.filter(function (x) { return x.AddressFor == 11 })
                //$scope.SocietyRegisteredOffice.Secretary = Secterydata.length > 0 ? Secterydata[0] : 'N/A';
                if (Secterydata.length > 0) {
                    Secterydata[0].CITY_ID = Secterydata[0].CITY_ID == null ? "" : Secterydata[0].CITY_ID.toString();
                    Secterydata[0].STATE_ID = Secterydata[0].STATE_ID == null ? "" : Secterydata[0].STATE_ID.toString();
                    Secterydata[0].WARD_ID = Secterydata[0].WARD_ID == null ? "" : Secterydata[0].WARD_ID.toString();
                    Secterydata[0].DIVISION_ID = Secterydata[0].DIVISION_ID == null ? "" : Secterydata[0].DIVISION_ID.toString();
                    Secterydata[0].DISTRICT_ID = Secterydata[0].DISTRICT_ID == null ? "" : Secterydata[0].DISTRICT_ID.toString();
                    Secterydata[0].BLOCK_ID = Secterydata[0].BLOCK_ID == null ? "" : Secterydata[0].BLOCK_ID.toString();
                    Secterydata[0].GP_ID = Secterydata[0].GP_ID == null ? "" : Secterydata[0].GP_ID.toString();
                    Secterydata[0].VILLAGE_ID = Secterydata[0].VILLAGE_ID == null ? "" : Secterydata[0].VILLAGE_ID.toString();

                    $scope.SocietyRegisteredOffice.HouseBuildingNoSecretary = Secterydata[0].HouseBuildingNo;
                    $scope.SocietyRegisteredOffice.LaneStreetNameSecretary = Secterydata[0].LaneStreetName;
                    $scope.SocietyRegisteredOffice.LocalitySecretary = Secterydata[0].Locality;
                    $scope.SocietyRegisteredOffice.STATE_IDSecretary = Secterydata[0].STATE_ID.toString();
                    $scope.SocietyRegisteredOffice.IsRuralSecretary = Secterydata[0].IsRural ? 'true' : 'false';
                    $scope.RuralUrbanBlockForSportcopy(1);
                    $scope.SocietyRegisteredOffice.PinCodeSecretary = Secterydata[0].PinCode;
                    $scope.SocietyRegisteredOffice.CityTypesSecretary = 0;
                    $scope.SocietyRegisteredOffice.CITY_IDSecretary = Secterydata[0].CITY_ID.toString();
                    $scope.GetWardList(3);
                    $scope.SocietyRegisteredOffice.WARD_IDSecretary = Secterydata[0].WARD_ID.toString();
                    $scope.SocietyRegisteredOffice.DIVISION_IDSecretary = Secterydata[0].DIVISION_ID.toString();
                    $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary = Secterydata[0].DISTRICT_ID.toString();
                    $scope.SocietyRegisteredOffice.BLOCK_IDSecretary = Secterydata[0].BLOCK_ID.toString();
                    $scope.SocietyRegisteredOffice.GP_IDSecretary = Secterydata[0].GP_ID.toString();
                    $scope.SocietyRegisteredOffice.VILLAGE_IDSecretary = Secterydata[0].VILLAGE_ID.toString();
                    $scope.GetCityListByCityTypes(3);
                    $scope.GetDistrictList(3);
                    //$scope.DivisionList();
                    $scope.BlockListByOffAdd(3);
                    $scope.GramPanchayatList(3);
                    $scope.VillageList(3);
                }
                //For trasurer data
                $scope.IsTreasurer();
                var Trasererdata = New.filter(function (x) { return x.AddressFor == 12 })
                //$scope.SocietyRegisteredOffice.Traserer = Trasererdata.length > 0 ? Trasererdata[0] : 'N/A';
                if (Trasererdata.length > 0) {

                    Trasererdata[0].CITY_ID = Trasererdata[0].CITY_ID == null ? "" : Trasererdata[0].CITY_ID.toString();
                    Trasererdata[0].STATE_ID = Trasererdata[0].STATE_ID == null ? "" : Trasererdata[0].STATE_ID.toString();
                    Trasererdata[0].WARD_ID = Trasererdata[0].WARD_ID == null ? "" : Trasererdata[0].WARD_ID.toString();
                    Trasererdata[0].DIVISION_ID = Trasererdata[0].DIVISION_ID == null ? "" : Trasererdata[0].DIVISION_ID.toString();
                    Trasererdata[0].DISTRICT_ID = Trasererdata[0].DISTRICT_ID == null ? "" : Trasererdata[0].DISTRICT_ID.toString();
                    Trasererdata[0].BLOCK_ID = Trasererdata[0].BLOCK_ID == null ? "" : Trasererdata[0].BLOCK_ID.toString();
                    Trasererdata[0].GP_ID = Trasererdata[0].GP_ID == null ? "" : Trasererdata[0].GP_ID.toString();
                    Trasererdata[0].VILLAGE_ID = Trasererdata[0].VILLAGE_ID == null ? "" : Trasererdata[0].VILLAGE_ID.toString();

                    $scope.SocietyRegisteredOffice.HouseBuildingNoTreasurer = Trasererdata[0].HouseBuildingNo;
                    $scope.SocietyRegisteredOffice.LaneStreetNameTreasurer = Trasererdata[0].LaneStreetName;
                    $scope.SocietyRegisteredOffice.LocalityTreasurer = Trasererdata[0].Locality;
                    $scope.SocietyRegisteredOffice.STATE_IDTreasurer = Trasererdata[0].STATE_ID.toString();
                    $scope.SocietyRegisteredOffice.IsRuralTreasurer = Trasererdata[0].IsRural ? 'true' : 'false';
                    $scope.IsTreasurer(1);
                    $scope.SocietyRegisteredOffice.PinCodeTreasurer = Trasererdata[0].PinCode;
                    $scope.SocietyRegisteredOffice.CityTypesTreasurer = 0;
                    $scope.SocietyRegisteredOffice.CITY_IDTreasurer = Trasererdata[0].CITY_ID.toString();
                    $scope.GetWardList(4);
                    $scope.SocietyRegisteredOffice.WARD_IDTreasurer = Trasererdata[0].WARD_ID.toString();
                    $scope.SocietyRegisteredOffice.DIVISION_IDTreasurer = Trasererdata[0].DIVISION_ID.toString();
                    $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer = Trasererdata[0].DISTRICT_ID.toString();
                    $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer = Trasererdata[0].BLOCK_ID.toString();
                    $scope.SocietyRegisteredOffice.GP_IDTreasurer = Trasererdata[0].GP_ID.toString();
                    $scope.SocietyRegisteredOffice.VILLAGE_IDTreasurer = Trasererdata[0].VILLAGE_ID.toString();
                    $scope.GetCityListByCityTypes(4);
                    $scope.GetDistrictList(4);
                    //$scope.DivisionList();
                    $scope.BlockListByOffAdd(4);
                    $scope.GramPanchayatList(4);
                    $scope.VillageList(4);
                }
            }
            //$scope.GetCityListByCityTypes(1);
            //$scope.GetCityListByCityTypes(2);
            //$scope.GetCityListByCityTypes(3);
            $scope.SocietyRegisteredOffice.STATE_ID = '6';
            $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
            $scope.SocietyRegisteredOffice.STATE_IDSecretary = '6';
            $scope.SocietyRegisteredOffice.STATE_IDTreasurer = '6';
            //ADDRESS AND


            var AdministrativeDetails = [];
            angular.forEach(data, function (item) {
                AdministrativeDetails.push({
                    "Name": item.Name, "ContactNo": item.ContactNo, "Type": item.UserType, "SocietyId": item.SocietyId, "DOB": item.ContactNo, "DOB": item.ContactNo, "Photo": item.Photo, "AADHAARNo": item.AadharNo
                });
            });
            $scope.SetUplodedDocuments(BasicDetails);
            $scope.GetUplodedOthersDocuments(BasicDetails.SocietyID, BasicDetails.SocietyDirectory);
            //$scope.SocietyDetailsData.BasicDetails = [];DLSCFileName
        }
        else {
            $scope.SocietyDetailsData["BasicDetails"] = [];
            $scope.SocietyDetailsData["AdministrativeDetails"] = [];
        }
        $(".blockUI").fadeOut("slow");
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
                GenderCount = GenderCount + ($(this).find('td:eq(8)').find('label').html() == "FEMALE" ? 1 : 0);
                SCCount = SCCount + (parseInt($(this).find('td:eq(9)').find('select').val()) == 2 ? 1 : 0)
                STCount = STCount + (parseInt($(this).find('td:eq(9)').find('select').val()) == 1 ? 1 : 0)
            });
        }
        if (GenderCount > 1 && SCCount > 0 && STCount > 0) {
            Returnval = true;
        }
        else if ((GenderCount < 2 || SCCount < 1 || STCount < 1) && $scope.SocietyRegistration.ActId == "3") {
            Message = $scope._language == 'en-us' ? 'नियमानुसार दो महिला और एक अनुसूचित जनजाति व एक अनुसूचित जाति के सदस्य होना अनिवर्य है क्या आप फिर भी डाटा सेव करना चाहते है!' : 'According to the rules, it is imperative to be two women and a scheduled tribe and a Scheduled Caste member. Do you want to save the data?'
            var r = confirm(Message);
            if (r == true) {
                Returnval = true;
            } else {
                Returnval = false;
            }
        }
        else {
            Returnval = true;
        }


        return Returnval;
    }

    $scope.SkipSocietyAdministrativeDetails = function () {
        $scope.SubmitStatus = 0;
        var $active = $('.wizard .nav-tabs li.active');
        prevTab($active);
        $active.next().removeClass('disabled');
        var $active = $('.wizardShow .nav-tabs li.active').attr('id');
        $scope.SocietyDirectory = data.societydirectory;
        $scope.SocietyIdForAdministrativeDetails = data.data;

        $scope.divSocietyProfilingShow = false;
        $scope.divAddBtn = false;
        $scope.lstSocietyWorkScope = [];

        $("#SocietyStep_1").addClass("disabled stepactive1");
        $("#SocietyStep_2").removeClass("active stepactive1");
        $("#SocietyStep_2").addClass("disabled stepactive1");
        $("#SocietyStep_3").addClass("active stepactive2");


        $("#step1").addClass("tab-pane ng-hide");
        $("#step2").removeClass("tab-pane active");
        $("#step2").addClass("tab-pane ng-hide");
        $("#step3").addClass("tab-pane active");

        if ($scope.ActId != "7") {
            $scope.DivOther = true;
            $scope.DivSport = false;
        }
        else {
            $scope.DivOther = false;
            $scope.DivSport = true;
        }
    }

    $scope.SkipSocietyAdministrativeDetails = function () {
        return true;
    }

    //START AdministrativeFormValidation  
    $scope.AdministrativeFormValidation = function (SubmitStatus) {

        // return true;
        var validate = true;
        var Count = 0;

        var message = $scope._language == 'en-us' ? ' Validate  following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.SocietyRegistration.ActId > 0) {
            if ($scope.SocietyRegistration.IsAdministrative == undefined && $scope.SocietyRegistration.ActId != "1" && SubmitStatus == 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrative Type' : '\n <br /> प्रशासनिक प्रकार का चयन करे |';
            }

            //liquidator
            if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
                if ($scope.SocietyRegistration.liquidatorName == undefined || $scope.SocietyRegistration.liquidatorName == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Name' : '\n <br /> परिसमापक का नाम दर्ज करें |';

                }

                if ($('#txtliquidatorAppointedDate').val() == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Appointment' : '\n <br /> कृपया परिसमापक की नियुक्ति तिथि दर्ज करे |';

                }

                if ($scope.SocietyRegistration.liquidatorPostName == "" || $scope.SocietyRegistration.liquidatorPostName == undefined) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Post.' : '\n <br /> कृपया परिसमापक का पद दर्ज करे |';

                }
                if ($scope.SocietyRegistration.liquidatorContactNo == "" || $scope.SocietyRegistration.liquidatorContactNo == undefined) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Mobile No.' : '\n <br /> कृपया परिसमापक का मोबाइल नंबर दर्ज करे |';

                }
            }
            //END
            if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                var Count = 0;
                //$scope.TableData10.forEach(function (item, index) {

                //});                

                $('#TblElectedMember  tr').each(function (index) {
                    if ($scope.SocietyRegistration.ActId == '1' && ($scope.SocietyRegistration.IsNewRegistration == false)) {
                        //if (index == 0 && SubmitStatus == 1) {
                        //    if ($scope.TableData10[0].Aadh == "" || $scope.TableData10[0].Aadh == undefined || $scope.TableData10[0].Aadh.length < 12) {
                        //        $("#adh_0").addClass("Redborder");
                        //        message += '\n <br /> Please Enter Aadhar no of ' + ($scope.TableData10[0].POSTNAME == undefined || $scope.TableData10[0].POSTNAME == "" ? "CHAIRMAN/PRESIDENT" : $scope.TableData10[0].POSTNAME)
                        //        validate = false;
                        //    }

                        //    if ($scope.TableData10[0].ContactNo == "" || $scope.TableData10[0].ContactNo == undefined) {
                        //        $("#ContactNo_0").addClass("Redborder");
                        //        message += '\n <br /> Please Enter ContactNo no of ' + ($scope.TableData10[0].POSTNAME == undefined || $scope.TableData10[0].POSTNAME == "" ? "CHAIRMAN/PRESIDENT" : $scope.TableData10[0].POSTNAME)
                        //        validate = false;
                        //    }

                        //    if ($scope.TableData10[0].CategoryId == "" || $scope.TableData10[0].CategoryId == undefined || $scope.TableData10[0].CategoryId == "0") {
                        //        $("#CategoryId_0").addClass("Redborder");
                        //        message += '\n <br /> Please Select Category of ' + ($scope.TableData10[0].POSTNAME == undefined || $scope.TableData10[0].POSTNAME == "" ? "CHAIRMAN/PRESIDENT" : $scope.TableData10[0].POSTNAME)
                        //        validate = false;
                        //    }

                        //    if ($scope.TableData10[0].POSTNAME == "" || $scope.TableData10[0].POSTNAME == undefined) {
                        //        $("#POSTNAME_0").addClass("Redborder");
                        //        message += '\n <br /> Please Enter Post of Member';
                        //        validate = false;
                        //    }

                        //    if ($scope.TableData10[0].Occupation == "" || $scope.TableData10[0].Occupation == undefined) {
                        //        $("#Occupation_0").addClass("Redborder");
                        //        message += '\n <br /> Please Enter Occupation of ' + ($scope.TableData10[0].POSTNAME == undefined || $scope.TableData10[0].POSTNAME == "" ? "CHAIRMAN/PRESIDENT" : $scope.TableData10[0].POSTNAME);
                        //        validate = false;
                        //    }

                        //    if ($scope.TableData10[0].Isvalidate == undefined || $scope.TableData10[0].Isvalidate == false) {
                        //        $("#SSOID_0").addClass("Redborder");
                        //        message += '\n <br /> Please Validate Aadhar no of ' + ($scope.TableData10[0].POSTNAME == undefined || $scope.TableData10[0].POSTNAME == "" ? "CHAIRMAN/PRESIDENT" : $scope.TableData10[0].POSTNAME);
                        //        validate = false;
                        //    }

                        //    if (message.length > 24 || validate == false) {
                        //        if (message.length > 24) {
                        //            ShowModelAlert("Administrative Form Validation", message);
                        //        }
                        //        else {
                        //            ShowModelAlert("Administrative Form Validation", $scope._language == 'en-us' ? 'Validate the value of red border Area' : 'लाल सीमा के मूल्य को मान्य करें');
                        //        }
                        //        $(".se-pre-con").fadeOut("slow");
                        //        return false;
                        //    }
                        //}
                    } else {
                        //if (index >= 5 && $scope.SocietyRegistration.ActId == "1") {
                        //} else {
                        if (SubmitStatus == 1 || ($(this).find('td:eq(3)').find('input').val() != '' && $(this).find('td:eq(3)').find('input').val() != 'undefined')) {
                            //Ram

                            //if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                            //    var jElement = $(this).find('td:eq(3)').find('input');
                            //    jElement.css('border', 'solid 1px red');
                            //    validate = false;
                            //}
                            //else {
                            //    var jElement = $(this).find('td:eq(3)').find('input');
                            //    jElement.css('border', 'rgb(222, 221, 221)');
                            //}
                            //Ram



                            //AADHAR
                            if ($(this).find('td:eq(5)').find('input').val() == undefined || $(this).find('td:eq(5)').find('input').val() == '') {
                                var jElement = $(this).find('td:eq(5)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(5)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            //NAME
                            if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                                var jElement = $(this).find('td:eq(4)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(4)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            ////FATHER NAME
                            //if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                            //    var jElement = $(this).find('td:eq(3)').find('input');
                            //    jElement.css('border', 'solid 1px red');
                            //    validate = false;
                            //    $(this).find('td:eq(3)').find('input').removeAttr("disabled");
                            //}
                            //else {
                            //    var jElement = $(this).find('td:eq(3)').find('input');
                            //    jElement.css('border', 'rgb(222, 221, 221)');
                            //}
                            //PHONE
                            if ($(this).find('td:eq(6)').find('input').val() == undefined || $(this).find('td:eq(6)').find('input').val() == '') {
                                var jElement = $(this).find('td:eq(6)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                                $(this).find('td:eq(6)').find('input').removeAttr("disabled");
                            }
                            else {
                                var jElement = $(this).find('td:eq(6)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            //DATE
                            if ($(this).find('td:eq(7)').find('input').val() == undefined || $(this).find('td:eq(7)').find('input').val() == '') {
                                var jElement = $(this).find('td:eq(7)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(7)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            //GENDER
                            if ($(this).find('td:eq(8)').find('label').html() == '') {
                                var jElement = $(this).find('td:eq(8)').find('label');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(8)').find('label');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            //CATEGORY
                            if ($(this).find('td:eq(9)').find('select').val().indexOf('und') > -1 || $(this).find('td:eq(9)').find('select').val() == null || $(this).find('td:eq(9)').find('select').val() == "0") {
                                var jElement = $(this).find('td:eq(9)').find('select');
                                jElement.css('border', 'solid 1px red');
                                $(this).find('td:eq(9)').find('select').removeAttr("disabled");
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(9)').find('select');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            //POST
                            if ($(this).find('td:eq(10)').find('input').val() == '' || $(this).find('td:eq(10)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(10)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(10)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                            //IMAGE
                            //if ($(this).find('td:eq(9)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                            //    var jElement = $(this).find('td:eq(9)').find('img');
                            //    jElement.css('border', 'solid 1px red');
                            //    validate = false;
                            //}
                            //else {
                            //    var jElement = $(this).find('td:eq(9)').find('img');
                            //    jElement.css('border', 'rgb(222, 221, 221)');
                            //}

                            //Occupation
                            if ($(this).find('td:eq(12)').find('input').val() == '') {
                                var jElement = $(this).find('td:eq(12)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(12)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            if (($scope.SocietyRegistration.IsNewRegistration == false) && $scope.Act2001) {
                                //MembershipNo
                                if ($(this).find('td:eq(13)').find('input').val() == '' || $(this).find('td:eq(13)').find('input').val() == undefined) {
                                    var jElement = $(this).find('td:eq(13)').find('input');
                                    jElement.css('border', 'solid 1px red');
                                    validate = false;
                                }
                                else {
                                    var jElement = $(this).find('td:eq(13)').find('input');
                                    jElement.css('border', 'rgb(222, 221, 221)');
                                }

                                //ShareAllotted
                                if ($(this).find('td:eq(14)').find('input').val() == '' || $(this).find('td:eq(14)').find('input').val() == undefined) {
                                    var jElement = $(this).find('td:eq(14)').find('input');
                                    jElement.css('border', 'solid 1px red');
                                    validate = false;
                                }
                                else {
                                    var jElement = $(this).find('td:eq(14)').find('input');
                                    jElement.css('border', 'rgb(222, 221, 221)');
                                }

                                //ShareCapitalAmount
                                if ($(this).find('td:eq(15)').find('input').val() == '' || $(this).find('td:eq(15)').find('input').val() == undefined) {
                                    var jElement = $(this).find('td:eq(15)').find('input');
                                    jElement.css('border', 'solid 1px red');
                                    validate = false;
                                }
                                else {
                                    var jElement = $(this).find('td:eq(15)').find('input');
                                    jElement.css('border', 'rgb(222, 221, 221)');
                                }

                                //loanee/non loanee
                                if ($(this).find('td:eq(16)').find('select').val() == '' || $(this).find('td:eq(16)').find('select').val() == null || $(this).find('td:eq(16)').find('select').val() == "0") {
                                    var jElement = $(this).find('td:eq(16)').find('select');
                                    jElement.css('border', 'solid 1px red');
                                    $(this).find('td:eq(16)').find('select').removeAttr("disabled");
                                    validate = false;
                                }
                                else {
                                    var jElement = $(this).find('td:eq(16)').find('select');
                                    jElement.css('border', 'rgb(222, 221, 221)');
                                }

                                if ($(this).find('td:eq(17)').find('input').prop('checked') == true) {
                                    Count = Count + 1;
                                }
                            }
                            else {
                                if ($(this).find('td:eq(17)').find('input').prop('checked') == true) {
                                    Count = Count + 1;
                                }

                            }
                        }
                    }
                });
                //validate = $scope.ValidateNominatedMember(0, true);
                //validate = $scope.ValidateElectedMember(3, true);
                if ($scope.SocietyRegistration.ActId == "3" && Count < 10 && $scope.SocietyRegistration.IsAdministrative == 'Board' && SubmitStatus == 1) {
                    //var r = confirm($scope._language == 'en-us' ? 'Minimum 12 Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 12 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    //if (r == false) {
                    //return false;
                    //}
                    alert($scope._language == 'en-us' ? 'Minimum 10 Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 10 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    return false;

                }
                else if ($scope.SocietyRegistration.ActId == "1" && Count < 5 && SubmitStatus == 1 && ($scope.SocietyRegistration.IsNewRegistration)) {
                    //var r = confirm($scope._language == 'en-us' ? 'Minimum 5 Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 5 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    //if (r == false) {
                    //   return false;
                    //}					
                    alert($scope._language == 'en-us' ? 'Minimum 5 Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 5 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    return false;
                }
                else if ($scope.SocietyRegistration.ActId == "7" && Count < 5 && SubmitStatus == 1) {
                    alert($scope._language == 'en-us' ? 'Minimum 5  Elected Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 5 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    return false;

                }
            }

            if ($scope.SocietyRegistration.IsAdministrative == 'Administrative') {

                if ($scope.SocietyRegistration.AppointedName == undefined || $scope.SocietyRegistration.AppointedName == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Administrator Name' : '\n <br /> प्रशासक का नाम दर्ज करें |';
                }

                if ($scope.SocietyRegistration.AppointedPostName == undefined || $scope.SocietyRegistration.AppointedPostName == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Administrative Post' : '\n <br /> कृपया प्रशासनिक पोस्ट दर्ज करें|';
                }

                if ($('#txtAppointedDOB').val() == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Date of Order' : '\n <br /> कृपया आदेश की तारीख दर्ज करे |';
                }
            }

            if ($scope.SocietyRegistration.IsAdministrative == 'Board' && $scope.SocietyRegistration.ActId == "3" && ($scope.IsNewRegistration == false) && SubmitStatus == 1) {
                if ($scope.SocietyRegistration.TotalMembersClassA == undefined || $scope.SocietyRegistration.TotalMembersClassA == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class A.' : '\n <br /> कृपया कक्षा ए के कुल सदस्य दर्ज करें |';
                }
                if ($scope.SocietyRegistration.TotalMembersClassB == undefined || $scope.SocietyRegistration.TotalMembersClassB == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class B.' : '\n <br /> कृपया कक्षा बी के कुल सदस्य दर्ज करें |';
                }
                if ($scope.SocietyRegistration.TotalMembersClassC == undefined || $scope.SocietyRegistration.TotalMembersClassC == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class C.' : '\n <br /> कृपया कक्षा सी के कुल सदस्य दर्ज करें |';
                }
            }

        }
        else {



        }
        //End Sport Association

        if (message.length > 22 && validate == false) {
            if (message.length > 22) {
                ShowModelAlert("Administrative Form Validation", message);
            }
            else {
                ShowModelAlert("Administrative Form Validation", $scope._language == 'en-us' ? 'Validate the value of red border Area' : 'लाल सीमा के मूल्य को मान्य करें');
            }
            $(".se-pre-con").fadeOut("slow");
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends
    //START : Get Sports Associations List
    $scope.GetSportsAssociationsList = function () {
        $http.get(SiteUrl + "/SportAssociation/SportAssociationList/?WhereClause=" + "").
            success(function (data, status) {
                $scope.SportAssociationList = data;

            }).
            error(function (data, status) {
                $scope.SportAssociationList = [];
            });
    };

    $scope.GetSocietiesForDeshBoard = function (SocietyIdEnc) {
        //$http.post(SiteUrl + "/Society/GetSocietiesForDeshBoardBySocietyId/?SocietyId=" + SocietyIdEnc).
        //    success(function (data, status) {

        //        $scope.NextElectionDate = data.filter(function (x) { return x.DataType == '1' }).length > 0 ? data.filter(function (x) { return x.DataType == '1' })[0].DataDate : '';
        //        $scope.InspectionDueDate = data.filter(function (x) { return x.DataType == '2' }).length > 0 ? data.filter(function (x) { return x.DataType == '2' })[0].DataDate : '';
        //        $scope.NextAuditDate = data.filter(function (x) { return x.DataType == '3' }).length > 0 ? data.filter(function (x) { return x.DataType == '3' })[0].DataDate : '';
        //        $scope.NextAGMDate = data.filter(function (x) { return x.DataType == '4' }).length > 0 ? data.filter(function (x) { return x.DataType == '4' })[0].DataDate : '';

        //        $scope.PaidUpShareCapital = $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc }).length > 0 ? $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc })[0].PaidUpShareCapital : '0';
        //        $scope.NoOfBranches = $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc }).length > 0 ? $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc })[0].NoOfBranches : '0';
        //        //$scope.Tot_ELECTEDMEMBERS = $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc).length > 0 ? $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc)[0].Tot_ELECTEDMEMBERS : '0';
        //        //$scope.Tot_GENERALMEMBERS = $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc).length > 0 ? $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc)[0].Tot_GENERALMEMBERS : '0';
        //        //$scope.Tot_MEMBERS = parseInt($scope.Tot_ELECTEDMEMBERS) + parseInt($scope.Tot_GENERALMEMBERS);

        //    }).
        //    error(function (data, status) {
        //        $scope.SportAssociationList = [];
        //    });
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

            $scope.DistrictDiv = true;
            $scope.District = false;
            $scope.Districtt = true;
            $scope.RuralForSportSociety = false;
            $scope.PrimaryBodyDetails = true;
            $scope.PrimaryBodyName = false;
            $scope.GetDistrictWithoutDivisionList();
            $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the District Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् से अवगत कराये|');
            $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of State Olympic Association Affiliation ?' : 'राज्य ओलंपिक परिषद से संबद्धता की स्थिति चुने?');
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
            //$scope.PrimaryBodyName = true;
            $scope.primarydiv = true;
            $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the Primary Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली प्राथमिक क्रीड़ा निकायों से अवगत कराये|');
            $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of District Olympic Association Affiliation ?' : 'जिला ओलंपिक परिषद से संबद्धता की स्थिति चुने?');


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
        $scope.DivAdministrative = true;
        $scope.divSportsSocietys = true;
        $scope.divOtherSocietys = false;
        $scope.DivAddressDetails = true;

        //alert($scope.RuralurbanDiv);
        $scope.SetSocietyDocuents();
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
        if ($scope.SocietyRegistration.Ruraltwo == true) {
            $scope.RuralForSportSociety = true;
            $scope.UrbanForSportSocietyNew = false;

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
            $scope.SOCFileName = '';
        }

        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'No')
            $scope.FileSLSANo = 4;
        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes')
            $scope.FileSLSANo = 5;
        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'No' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'No')
            $scope.FileSLSANo = 3;


        $scope.SetSocietyDocuents();
        // $scope.SetUplodedDocuments();
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
            $scope.AffiliationBySLSA = true;
        }
        else {
            $scope.ShowRegDateAndNoForSLSAAffiliationYes = false;
            $scope.FileSLSA = false;
            $scope.AffiliationBySLSA = true;
            $scope.SLSAFileName = '';
        }

        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes')
            $scope.FileSLSANo = 5;
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'No' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'No')
            $scope.FileSLSANo = 3;
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == undefined)
            $scope.FileSLSANo = 4;

        $scope.SetSocietyDocuents();
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
            $scope.DLSCFileName = '';
        }
        if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'No' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'No')
            $scope.FileSLSANo = 3;
        else if ($scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes')
            $scope.FileSLSANo = 5;


        $scope.SetSocietyDocuents();
        //$scope.SetUplodedDocuments();
    };
    //END
    //START :
    $scope.ShowRegDateAndNoForSLSCffiliation = function () {

        if ($scope.SocietyRegistration.IsAffiliatedBySLSC == "Yes") {
            $scope.AffiliationBySLSCYes = true;
            $scope.LevelState = true;
            $scope.FileSLSCNo = 3;
            $scope.FileDOCANo = 4;
            $scope.SocietyRegistration.IsAffiliatedByDLSC = 'No';
        }
        else {
            $scope.AffiliationBySLSCYes = false;
            $scope.LevelState = false;
            $scope.FileDOCANo = 3;
        }
        $scope.SetSocietyDocuents();
    };
    //END
    //
    $scope.RemoveRowFromTable = function (e) {
        $('#A' + e).remove();
    }
    //
    //START
    $scope.RemoveRowFromList = function (e) {
        var index = $scope.lstSocietyWorkScope.indexOf(e);
        $scope.lstSocietyWorkScope.splice(index, 1);
    }
    //START
    $scope.FormValidationSportsAssociation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyRegistration.IsStatLevel == 'No') {
            if ($scope.SocietyRegistration.Ruraltwo == undefined && $scope.lstSocietyWorkScope.length == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Rural/Urban' : '\n <br /> कृपया शहरी/ग्रामीण का चयन करे करे |';

            }
            //urban=false, rural=true
            if ($scope.SocietyRegistration.Ruraltwo == true) {
                if ($scope.SocietyRegistration.BLOCK_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Block' : '\n <br /> कृपया ब्लाक का चयन करे |';

                }
                if ($scope.SocietyRegistration.GP_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram Panchayat' : '\n <br /> कृपया ग्रामपंचायत का चयन करे |';

                }
                if ($scope.SocietyRegistration.VILLAGE_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram' : '\n <br /> कृपया ग्राम का चयन करे |';

                }
            }
            else if ($scope.SocietyRegistration.Ruraltwo == false) {
                if ($scope.SocietyRegistration.CityTypes == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select City type' : '\n <br /> कृपया सिटी टाइप का चयन करे |';

                }
                if ($scope.SocietyRegistration.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select City' : '\n <br /> कृपया शहर का चयन करे |';

                }
                if ($scope.SocietyRegistration.WARD_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Ward' : '\n <br /> कृपया वार्ड का चयन करे |';

                }
            }
            if ($scope.SocietyRegistration.PrimaryBodyName == undefined || $scope.SocietyRegistration.PrimaryBodyName == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Primary Body Name' : '\n <br /> कृपया प्राथमिक निकाय का नाम दर्ज करें |';

            }
        }

        if ($scope.SocietyRegistration.PrimaryBodyCEOName == undefined || $scope.SocietyRegistration.PrimaryBodyCEOName == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter CEO Name' : '\n <br /> कृपया मुख्य संचालक का नाम दर्ज करें |';

        }
        if ($scope.SocietyRegistration.PrimaryBodyCEOMobileNo == undefined || $scope.SocietyRegistration.PrimaryBodyCEOMobileNo == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Name' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';

        }
        if ($scope.SocietyRegistration.DISTRICT_ID == 0 && $scope.SocietyRegistration.Ruraltwo != false) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select District Council' : '\n <br /> कृपया जिला परिषद का चयन करे |';

        }
        if ($scope.SocietyRegistration.AssociationId == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Sport' : '\n <br /> कृपया क्रीड़ा का चयन करे |';

        }

        if (message.length > 29) {
            ShowModelAlert("Society Sports Association Profiling", message);
            return false;
        }
        return true;
    }


    //END

    //Star Otheassocities grid validation
    $scope.FormValidationOthersocity = function (ValidationFlag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyRegistration.IsStatLevel == 'undefined' && (ValidationFlag == true || ValidationFlag == undefined)) {

            if ($scope.SocietyRegistration.Rural == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Rural/Urban' : '\n <br /> कृपया शहरी/ग्रामीण का चयन करे |';

            }
            //urban=false, rural=true
            if ($scope.SocietyRegistration.Rural == true) {

                if ($scope.SocietyRegistration.WorkScopeLevel_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Work Scope ' : '\n <br /> वर्कस्कोप का चयन करें|';

                }
                if ($scope.SocietyRegistration.DIVISION_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 1) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Division Council' : '\n <br /> कृपया संभाग का चयन करे |';

                }
                if ($scope.SocietyRegistration.DISTRICT_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 3) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select District Council' : '\n <br /> कृपया जिला परिषद का चयन करे |';

                }
                if ($scope.SocietyRegistration.BLOCK_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 4) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Block' : '\n <br /> कृपया ब्लाक का चयन करे |';

                }
                if ($scope.SocietyRegistration.GP_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 5) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram Panchayat' : '\n <br /> कृपया ग्रामपंचायत का चयन करे |';

                }
                if ($scope.SocietyRegistration.VILLAGE_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 6) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram' : '\n <br /> कृपया ग्राम का चयन करे |';

                }
            }
            else if ($scope.SocietyRegistration.Rural == false) {
                if ($scope.SocietyRegistration.CityTypes == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select City type' : '\n <br /> कृपया सिटी टाइप का चयन करे |';

                }
                if ($scope.SocietyRegistration.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select City' : '\n <br /> कृपया शहर का चयन करे |';

                }
                if ($scope.SocietyRegistration.WARD_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Ward' : '\n <br /> कृपया वार्ड का चयन करे |';

                }
            }
        }
        if (message.length > 29) {
            ShowModelAlert("Society other Association Profiling", message);
            return false;
        }
        return true;
    }
    //End Otheassocities grid validation
    $scope.RemoveDeleteButton = function () {
        var TCount = $('#SportExecutiveTable tr').length;
        $('#SportExecutiveTable tr td:eq(10)').each(function () {
            var jElement = $(this).find('td:eq(10)').find('input');
        });
    }

    $scope.TableValiDate = function (tableflag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var Table_Name = "";
        if (tableflag == 1)
            Table_Name = '#SportExecutiveTable tr';
        else if (tableflag == 2)
            Table_Name = '#SportTableAdd tr';
        else if (tableflag == 3) {
            Table_Name = '#myTable2 tr';
        }

        // var Table_Name = tableflag == 1 ? '#SportExecutiveTable tr' : '#SportTableAdd tr'
        $(Table_Name).each(function (item) {
            if ($(this).find('td:eq(1)').find('input').val() == '' || $(this).find('td:eq(1)').find('input').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Director Name' : '\n <br /> कृपया निदेशक का नाम दर्ज करें |';

                var jElement = $(this).find('td:eq(1)').find('input');
                jElement.css('border', 'solid 1px red');
            }

            if ($(this).find('td:eq(2)').find('input').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';

                var jElement = $(this).find('td:eq(2)').find('input');
                jElement.css('border', 'solid 1px red');
            }

            if ($(this).find('td:eq(2)').find('input').val() != '') {
                if ($(this).find('td:eq(2)').find('input').val().length != 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Mobile Number' : '\n <br /> कृपया सही मोबाइल नंबर दर्ज करें |';

                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'solid 1px red');
                }
                else {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'solid 1px #ccc');
                }
            }


            if ($(this).find('td:eq(3)').find('input').val() != '') {
                if ($(this).find('td:eq(3)').find('input').val().length != 12) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Aadhaar Number' : '\n <br /> कृपया सही आधार संख्या दर्ज करें |';

                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'solid 1px red');
                }
            }
            if (tableflag == 1) {
                if ($(this).find('td:eq(7)').find('textarea').val() == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्जा करे |';

                    var jElement = $(this).find('td:eq(7)').find('textarea');
                    jElement.css('border', 'solid 1px red');
                }
                else {
                    var jElement = $(this).find('td:eq(7)').find('textarea');
                    jElement.css('border', 'solid 1px #ccc');
                }


                var Post = $(this).find('td:eq(8)');
                var PostId = Post.find('option:selected').val();

                if (PostId == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Post' : '\n <br /> कृपया पद का चयन करें |';

                    var jElement = $(this).find('td:eq(8)').find('select');
                    jElement.css('border', 'solid 1px red');
                }
                else {
                    var jElement = $(this).find('td:eq(8)').find('select');
                    jElement.css('border', '1px solid #ccc');
                }
            }
            else {
                var Post = $(this).find('td:eq(7)');
                var PostId = Post.find('option:selected').val();

                if (PostId == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Post' : '\n <br /> कृपया पद का चयन करें |';

                    var jElement = $(this).find('td:eq(7)').find('select');
                    jElement.css('border', 'solid 1px red');
                }
                else {
                    var jElement = $(this).find('td:eq(7)').find('select');
                    jElement.css('border', '1px solid #ccc');
                }
            }

            if ($(this).find('td:eq(5)').find('input:checked').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Gender Type.' : '\n <br /> कृपया लिंग का चयन करे |';

                //var jElement = $(this).find('td:eq(5)').find('input');
            }
            else {
                var jElement = $(this).find('td:eq(5)');
                //jElement.css('border', 'solid 1px red');
            }

            var Cast = $(this).find('td:eq(6)');
            var CastId = Cast.find('option:selected').val();

            if (CastId == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Category' : '\n <br /> कृपया वर्ग का चयन करें |';

                var jElement = $(this).find('td:eq(6)').find('select');
                jElement.css('border', 'solid 1px red');
            }
            else {
                var jElement = $(this).find('td:eq(6)').find('select');
                jElement.css('border', '1px solid #ccc');
            }

        });

        if (message.length > 29) {
            ShowModelAlert("Society Sports Association Profiling", message);
            return false;
        }
        return true;
    }

    $scope.Profiling = function (type) {
        if (type == 1) {
            $scope.SocietyRegistration.IsNewRegistration = true;
        } else if (type == 2) {
            $scope.SocietyRegistration.IsNewRegistration = false;
        }
        if ($scope.SocietyRegistration.IsNewRegistration == true || $scope.SocietyRegistration.IsNewRegistration == true) {
            $scope.IsProfiling = false;
            $scope.IsNewRegistration = true;
        }
        else {
            $scope.IsProfiling = true;
            $scope.IsNewRegistration = false;
        }
        $scope.SetSocietyDocuents();
    }

    $scope.CheckRegOrProfileing = function (Type) {
        if (Type == 1) {
            $scope.SocietyRegistration.IsNewRegistration = true
        } else {
            $scope.SocietyRegistration.IsNewRegistration = false
        }
        if ($scope.SocietyRegistration.IsNewRegistration == true) {
            $scope.divBrn = true;
            $scope.IsShowSearchSociety = false;
        }
        else {
            $scope.divBrn = true;
            $scope.GetDistrictWithoutDivisionList();
            $scope.IsShowSearchSociety = true;
        }
        if ($('#EUserType').val() == 'D') {
            $scope.divBrn = false;
            $scope.CheckBRN = true;
            $scope.nepster = true;
            $scope.showtabbar = true;
            $scope.ClickUserVarification();
            $scope.divOtherSocietys = true;
            $scope.IsSocietyNamedisabled = false;
        }
        else {

            $scope.divBrn = true;
            $scope.CheckBRN = false;
            $scope.nepster = false;
            $scope.divOtherSocietys = false;
            $scope.IsSocietyNamedisabled = true;
        }
        if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined && $('#EUserType').val() == 'D') {
            $scope.DivAdministrative = true;
            $scope.DivAddressDetails = true;
        }
        $scope.SetSocietyDocuents();
    }

    $scope.ClickBrnYesOrNot = function () {

        if ($scope.SocietyRegistration.IsBRNYes == false || $scope.SocietyRegistration.IsBRNYes == false) {
            $scope.ClickUserVarification();
            $scope.IsSocietyNamedisabled = false;
            $scope.showrulesfile = true;
            $scope.divOtherSocietys = true;
            $scope.ShowSubmitBtn = true;
            //$scope.divBrn = false;           
        }
        else if ($scope.SocietyRegistration.IsBRNYes == true || $scope.SocietyRegistration.IsBRNYes == true) {
            $scope.ClickUserVarification();
            //$scope.divBrn = true;
            $scope.divOtherSocietys = false;
            $scope.showrulesfile = false;
            $scope.ShowSubmitBtn = false;
            $scope.divBrnbtn = true;
        }
        $scope.SocietyRegistration.BRN = '';
    }

    $scope.ClickUserVarification = function () {
        $scope.ShowSubmitBtn = true;
        $scope.IsNewRegistration = ($scope.SocietyRegistration.IsNewRegistration);
        if ($scope.ActId == "1")
            $scope.IsNewRegistration = true;

        if ($scope.SocietyRegistration.IsNewRegistration == true) {
            $scope.IsProfiling = false;
            $scope.showtabbar = true;
            $scope.astrt = true;
            $scope.showrulesfile = true;
            $scope.divBrnbtn = true;
        }
        else {
            $scope.IsProfiling = true;
            $scope.showtabbar = true;
            $scope.astrt = true;
            $scope.showrulesfile = true;
            $scope.divBrnbtn = true;
        }
    }

    $scope.ValidateNominatedMember = function (FindTablebyId, Isvalidate) {
        //return true;
        var validate = true;
        var table = ''
        if (FindTablebyId == 0) {
            table = '#TblNominatedMember tr';
        }
        else if (FindTablebyId == 1) {
            table = '#TblSportElectedMember tr';
        }
        else if (FindTablebyId == 2) {
            table = '#TblSportNominatedMember tr';
        }
        var Count = 0;
        $(table).each(function () {
            //AADHAAR Number
            if ($(this).find('td:eq(1)').find('input').val() != '' || Count == 0 && $scope.SubmitStatus == 0 && validate == false) {
                if ($(this).find('td:eq(1)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(1)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(1)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //Director Name
                if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //Father/ Husband Name
                if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                //Contact No.
                if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(4)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                    $(this).find('td:eq(4)').find('input').removeAttr("disabled");
                }
                else {
                    var jElement = $(this).find('td:eq(4)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //DOB
                if ($(this).find('td:eq(5)').find('input').val() == undefined || $(this).find('td:eq(5)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(5)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(5)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //GEN
                if ($(this).find('td:eq(6)').find('label').html() == '') {
                    var jElement = $(this).find('td:eq(6)').find('label');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(6)').find('label');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
                //CATEGORY
                if ($(this).find('td:eq(7)').find('select').val() == null || $(this).find('td:eq(7)').find('select').val() == "0") {
                    var jElement = $(this).find('td:eq(7)').find('select');
                    jElement.css('border', 'solid 1px red');
                    $(this).find('td:eq(7)').find('select').removeAttr("disabled");
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(7)').find('select');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
                //POST
                if ($(this).find('td:eq(8)').find('input').val() == null || $(this).find('td:eq(8)').find('input').val() == "") {
                    var jElement = $(this).find('td:eq(8)').find('input');
                    jElement.css('border', 'solid 1px red');
                    $(this).find('td:eq(8)').find('input').removeAttr("disabled");
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(8)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
                //Nominated By Whom	
                if (FindTablebyId == 0) {
                    if ($(this).find('td:eq(9)').find('select').val() == null || $(this).find('td:eq(9)').find('select').val() == "0") {
                        var jElement = $(this).find('td:eq(9)').find('select');
                        jElement.css('border', 'solid 1px red');
                        $(this).find('td:eq(9)').find('select').removeAttr("disabled");
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(9)').find('select');
                        jElement.css('border', 'rgb(222, 221, 221)');

                    }
                }

                if (FindTablebyId == 1) {
                    if ($(this).find('td:eq(8)').find('textarea').val() == '') {
                        var jElement = $(this).find('td:eq(8)').find('textarea');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(8)').find('textarea');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                }

                if ($(this).find('td:eq(10)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                    var jElement = $(this).find('td:eq(10)').find('img');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(10)').find('img');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }

                Count = Count + 1;
            }
            else if ($scope.SubmitStatus == 1) {
                if ($(this).find('td:eq(1)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(1)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(1)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //Director Name
                if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //Father/ Husband Name
                if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                //Contact No.
                if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(4)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                    $(this).find('td:eq(4)').find('input').removeAttr("disabled");
                }
                else {
                    var jElement = $(this).find('td:eq(4)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //DOB
                if ($(this).find('td:eq(5)').find('input').val() == undefined || $(this).find('td:eq(5)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(5)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(5)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                //GEN
                if ($(this).find('td:eq(6)').find('label').html() == '') {
                    var jElement = $(this).find('td:eq(6)').find('label');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(6)').find('label');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
                //CATEGORY
                if ($(this).find('td:eq(7)').find('select').val() == null || $(this).find('td:eq(7)').find('select').val() == "0") {
                    var jElement = $(this).find('td:eq(7)').find('select');
                    jElement.css('border', 'solid 1px red');
                    $(this).find('td:eq(7)').find('select').removeAttr("disabled");
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(7)').find('select');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
                //POST
                if ($(this).find('td:eq(8)').find('input').val() == null || $(this).find('td:eq(8)').find('input').val() == "") {
                    var jElement = $(this).find('td:eq(8)').find('input');
                    jElement.css('border', 'solid 1px red');
                    $(this).find('td:eq(8)').find('input').removeAttr("disabled");
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(8)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
                //Nominated By Whom	
                if (FindTablebyId == 0) {
                    if ($(this).find('td:eq(9)').find('select').val() == null || $(this).find('td:eq(9)').find('select').val() == "0") {
                        var jElement = $(this).find('td:eq(9)').find('select');
                        jElement.css('border', 'solid 1px red');
                        $(this).find('td:eq(9)').find('select').removeAttr("disabled");
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(9)').find('select');
                        jElement.css('border', 'rgb(222, 221, 221)');

                    }
                }

                if (FindTablebyId == 1) {
                    if ($(this).find('td:eq(8)').find('textarea').val() == '') {
                        var jElement = $(this).find('td:eq(8)').find('textarea');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(8)').find('textarea');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                }

                if ($(this).find('td:eq(10)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                    var jElement = $(this).find('td:eq(10)').find('img');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(10)').find('img');
                    jElement.css('border', 'rgb(222, 221, 221)');

                }
            }
        });
        return validate;

    }

    $scope.ValidateElectedMember = function (FindTablebyId, Isvalidate) {
        
        if (Isvalidate == false)
            return true;
        else {
            var validate = true;
            var table = ''
            if (FindTablebyId == 3) {
                table = '#TblElectedMember tr';
            }
            $(table).each(function () {
                if ($(this).find('td:eq(3)').find('input').val() != '' || FindTablebyId != 0) {
                    if ($(this).find('td:eq(1)').find('input').val() == undefined || $(this).find('td:eq(1)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(1)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(1)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //NAME
                    if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(2)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(2)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //PHONE
                    if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(3)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                        $(this).find('td:eq(3)').find('input').removeAttr("disabled");
                    }
                    else {
                        var jElement = $(this).find('td:eq(3)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //DATE
                    if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(4)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(4)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //GENDER
                    if ($(this).find('td:eq(5)').find('label').html() == '') {
                        var jElement = $(this).find('td:eq(5)').find('label');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(5)').find('label');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //CATEGORY
                    if ($(this).find('td:eq(6)').find('select').val() == null || $(this).find('td:eq(6)').find('select').val() == "0") {
                        var jElement = $(this).find('td:eq(6)').find('select');
                        jElement.css('border', 'solid 1px red');
                        $(this).find('td:eq(6)').find('select').removeAttr("disabled");
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(6)').find('select');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //POST
                    if ($(this).find('td:eq(7)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(7)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(7)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }

                    if ($(this).find('td:eq(8)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                        var jElement = $(this).find('td:eq(8)').find('img');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(8)').find('img');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //Occupation
                    if ($(this).find('td:eq(9)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(9)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(9)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }

                    if ($(this).find('td:eq(15)').find('input').prop('checked') == true) {
                        Count = Count + 1;
                    }
                }
            });
            return validate;
        }
    }

    $scope.FinalSubmit = function (SocietyIdEnc) {
        //Message = $scope._language == 'en-us' ? 'आप इस प्रविष्टि के बाद अपने डेटा में परिवर्तन नहीं कर सकते| क्या आप फिर भी डाटा सेव करना चाहते है!' : 'You cannot edit your data after this submission. Do you want to save the data?'
        //if (confirm(Message)) {
        window.location = '../Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_Citizen_SocietyProfile~' + $scope.EMitraTransactionId;
        //$http.post(SiteUrl + "/Society/SaveSocietyFinalSubmition", { SocietyIdEncr: SocietyIdEnc }).
        //    success(function (data, status) {
        //        if (data > 0) {
        //            $scope.FeePayMent(data);
        //        }
        //        else {
        //            ShowModelAlert("Society Administrative Details", 'An Error Occurred Please Try Again Later');
        //        }

        //    }).
        //    error(function (data, status) {
        //        $scope.SocietyRegisteredOffice = [];
        //    });
        // }
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
        if (!($scope.SocietyRegistration.BRN == 0 || $scope.SocietyRegistration.ActId == 0)) {
            $scope.ShowValidateBtn = false;
        }
        else {
            $scope.ShowValidateBtn = true;
        }
    }



    $scope.BrnDetails = function () {


        $scope.CheckBRN = true;
        $scope.nepster = true;
        $scope.showtabbar = true;
        $scope.ClickUserVarification();
        $scope.IsSocietyNamedisabled = true;
        $scope.divOtherSocietys = true;
        if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined) {
            $scope.DivAdministrative = true;
            $scope.DivAddressDetails = true;
        }

        $(".se-pre-con").fadeIn("slow");
        if ($("input[name='SocietyRegistration']")[1].checked == true && $scope.SocietyRegistration.ActId == '3')
            $('#lblFeeCalculate').html('0');

        var formData = new FormData();
        formData.append("BRN", $scope.SocietyRegistration.BRN);
        formData.append("SocietyIdENC", $scope.SocietyIdEnc);
        formData.append("IsNewReg", $scope.SocietyRegistration.IsNewRegistration);
        $scope.SetSocietyDocuents();
        $http.post(SiteUrl + "/Society/GetBrnDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.result == 'success') {
                    $("#txtBrn").attr("disabled", "disabled");
                    //$("#txtBrn").removeAttr("disabled");
                    $("#lbltemp").html(data.BRN);
                    $scope.CheckBRN = true;
                    $scope.SocietyRegistration.SocietyName = data.BO_Name;
                    $scope.nepster = true;
                    $scope.showtabbar = true;
                    $scope.ClickUserVarification();
                    $scope.IsSocietyNamedisabled = true;
                    $scope.divOtherSocietys = true;
                    if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined) {
                        $scope.DivAdministrative = true;
                        $scope.DivAddressDetails = true;

                    }
                }
                else if (data.result == 'failed') {
                    $scope.showrulesfile = false;
                    $scope.ShowSubmitBtn = false;
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", data.BRN);
                    $scope.DivAdministrative = false;
                    $scope.DivAddressDetails = false;
                }
                else if (data.result == 'invalidname') {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Invalid Society Name: " + data.BO_Name);
                    $scope.CheckBRN = false;
                    $scope.DivAdministrative = false;
                    $scope.DivAddressDetails = false;
                }

            }).
            error(function (data, status) {
                $scope.CheckBRN = false;
                $scope.nepster = true;
                $scope.showtabbar = true;
                $(".se-pre-con").fadeOut("slow");
                $scope.DivAdministrative = false;
                $scope.DivAddressDetails = false;
                ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
            });

        if ($('#EUserType').val() == 'K' && $scope.SocietyRegistration.ActId != '7') {
            if ($('#EServiceid').val() == '4209')//PROFILE
            {
                $scope.SocietyRegistration.IsNewRegistration = false;
                $scope.ClickUserVarification();
                $("input[name='SocietyRegistration']")[0].checked = false;
                $("input[name='SocietyRegistration']")[1].checked = true;
            }
            else if ($('#EServiceid').val() == '4172')//REG
            {
                $scope.SocietyRegistration.IsNewRegistration = true;
                $("input[name='SocietyRegistration']")[0].checked = true;
                $("input[name='SocietyRegistration']")[1].checked = false;
                $scope.ClickUserVarification();
            }
            $('input[name="SocietyRegistration"]').attr('disabled', 'disabled');
        }
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

    //START : Check File Size
    function checkFileLengthPage(fileSized, MaxSize) {
        alert(fileSized);
        alert(MaxSize);
        alert(fileSized < MaxSize);
        if (fileSized < MaxSize) {
            //153600 - 150 KB 512000 -- 200KB    //     2048000 =2000kb
            return true;
        }
        return false;
    }
    //END: 




    $scope.VerifyOTPByBIO = function (isSubmit) {


        //var selectedRoles = vm.roles.filter(function (x) { return x.id === role.id; });

        if ($('#SSOAuth_chkConsent').prop("checked")) {
            $scope.messagebox = false
        }
        else {
            $scope.messagebox = true;
            $scope.MsgAadhar = 'Please allow system for your AADHAAR based Authentication/ eKYC.'
            return;

        }

        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.AADHAARNo != '') {

            $scope.CaptureFingerPrint();
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
        }
        $scope.showLoader = false;
    };
    $scope.CaptureFingerPrint = function () {
        var error_msg = "";
        if (_language == 'en-us') {
            error_msg = "Use following possible steps to resolve the issue:<br>1. Register your device from vendor or using Register Your Device link.<br>2. Download and install the latest registered device drivers and services.<br>3. Start/ restart the registered device service. <br/>4. Remove & plug in device again. <br/>5. Allow device Access from Your System Setting 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        } else {
            error_msg = "समस्या को हल करने के लिए संभव चरणों का उपयोग करें: <br>1. अपने डिवाइस को वेंडर से पंजीकृत करें या रजिस्टर योर डिवाइस लिंक का उपयोग करें। <br>2. नवीनतम पंजीकृत डिवाइस ड्राइवर तथा सर्विसेस  डाउनलोड और स्थापित करें। <br>3. पंजीकृत डिवाइस सेवा शुरू / पुनः आरंभ करें। <br/>4. डिवाइस निकालें और  फिर से प्लगइन  करें।  <br/>5. अपने सिस्टम सेटिंग से इंटरनेट एक्सेस की अनुमति दें 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        }
        $(".se-pre-con_bio").fadeIn("slow");

        var ssoauth_ver = $.now();
        var data = { p: 'https', type: 'EKYC', device: 'bio', isHttpsService: false };

        startCaptureRD({ authType: data.type, fpDevice: data.device, env: "P", isHttpsService: data.isHttpsService },
            function (responce) {
                $(".se-pre-con_bio").fadeOut("slow");
                if (responce.status == 'success') {
                    $scope.VerifyBioData(responce.data);
                } else {
                    ShowOkMessage("Error", "" + error_msg, function () { });
                }
            });


        //$.getScript("../Scripts/ssoauth_capture.min.js?v=" + ssoauth_ver).done(function (script, textStatus) {
        //    if (textStatus == "success") {
        //        startCaptureRD({ authType: data.type, fpDevice: data.device, env: "P", isHttpsService: data.isHttpsService },
        //            function (responce) {
        //                $(".se-pre-con_bio").fadeOut("slow");
        //                if (responce.status == 'success') {
        //                    $scope.VerifyBioData(responce.data);
        //                } else {
        //                    ShowOkMessage("Error", "" + error_msg, function () { });
        //                }
        //            });
        //    }
        //});
    }
    $scope.VerifyBioData = function (XMLData) {
        //var selectedRoles = vm.roles.filter(function (x) { return x.id === role.id; });
        if ($('#SSOAuth_chkConsent').prop("checked")) {
            $scope.messagebox = false
        }
        else {
            $scope.messagebox = true;
            $scope.MsgAadhar = 'Please allow system for your AADHAAR based Authentication/ eKYC.'
            return;

        }

        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.AADHAARNo != '') {
            var data = { AadharNo: $scope.AADHAARNo, xml: XMLData };
            $(".se-pre-con_bio_submit").fadeIn("slow");
            $http.post(SiteUrl + "/Citizen/ValidateBIOKYC", JSON.stringify({ Details: data })).
                success(function (rdata, status) {
                    $(".se-pre-con_bio_submit").fadeOut("slow");
                 //   console.log(rdata);
                    //$('#myOTPModal').modal("hide");
                    if (rdata.Status == "300") {
                        ShowOkMessage("Warning", _language == 'en-us' ? "Biometric data did not match for the given Aadhaar number." : "दिए गए आधार नंबर के लिए बायोमेट्रिक डेटा मैच नहीं हुआ", function () { });
                    } else if (rdata.Status == "K-100") {
                        ShowOkMessage("Warning", _language == 'en-us' ? "Invalid Aadhar No." : "अमान्य आधार संख्या", function () { });
                    } else if (rdata.Status == "811") {
                        ShowOkMessage("Warning", _language == 'en-us' ? "Missing biometric data in CIDR for the given Aadhaar number." : "दिए गए आधार संख्या के लिए CIDR में बॉयोमीट्रिक डेटा गुम", function () { });
                    } else if (rdata.Status == "336" || rdata.Status == "337") {
                        ShowOkMessage("Warning", _language == 'en-us' ? "UIDAI server not responding, Try again after some time." : "UIDAI सर्वर प्रतिक्रिया नहीं दे रहा है, कुछ समय बाद पुन: प्रयास करें।", function () { });
                    } else if (rdata.Status == "5") {
                        //$scope.CaptureMorpho();
                    } else if (rdata.Status == "1") {
                        $scope.UserDetails = rdata.UserDetails;
                        //ShowOkMessage("", _language == 'en-us' ? 'Successfully Varified' : "Successfully Varified", function () {                        
                        // alert("Successfully Varified");
                        ShowOkMessage("Success", _language == 'en-us' ? "Successfully Verified." : "सफलतापूर्वक सत्यापित", function () { });
                        BIOdataList = [];
                        var BIOdataObj = new BIOdata();
                        
                        BIOdataObj.Column2 = rdata.UserDetails.Name;
                        BIOdataObj.Column8 = rdata.UserDetails.FName;
                        BIOdataObj.Column4 = rdata.UserDetails.DOB;
                        BIOdataObj.Column3 = rdata.UserDetails.Gender;
                        BIOdataObj.Column5 = rdata.UserDetails.Photo;
                        BIOdataObj.Column10 = rdata.UserDetails.Address;
                        BIOdataList.push(BIOdataObj);
                        $scope.txn = '';
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                        $('#btnSSOAuth_ReSendOTP').hide();
                        if ($scope.FOR == 'CEO') {
                            $scope.SocietyRegistration.CEOName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.CEOGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.CEODOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.CEOProfile = true;
                            $scope.CEORemove = true;
                            $scope.GETCEO = false;
                        }
                        else if ($scope.FOR == 'CHAIRMAN') {
                            $scope.SocietyRegistration.ChairmanName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.ChairmanGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.ChairmanDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.ChairmanProfile = true;
                            $scope.CHAIRMANRemove = true;
                            $scope.GETCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'APPOINTED') {
                            $scope.SocietyRegistration.AppointedName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.AppointedGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.AppointedDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.AppointedFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.AppointedFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.AdministratorProfile = true;
                            $scope.APPOINTEDRemove = true;
                            $scope.GETAPPOINTED = false;
                        }
                        else if ($scope.FOR == 'lIQUIDATOR') {
                            $scope.SocietyRegistration.liquidatorName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.liquidatorGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.liquidatorDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.liquidatorFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.liquidatorFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.Isliquidator = true;
                            $scope.lIQUIDATORRemove = true;
                            $scope.GETlIQUIDATOR = false;
                        }
                        else if ($scope.FOR == 'SPCHAIRMAN') {
                            $scope.SocietyRegistration.CEOName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.CEOGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.CEODOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.CEOSocietyFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.CEOSportsProfile = true;
                            $scope.SPCHAIRMANRemove = true;
                            $scope.GETSPCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'SPSECRETARY') {
                            $scope.SocietyRegistration.ChairmanName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.ChairmanGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.ChairmanDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.ChairmanSportsProfile = true;
                            $scope.SPSECRETARYRemove = true;
                            $scope.GETSPSECRETARY = false;
                        }
                        else if ($scope.FOR == 'TREASURER') {
                            $scope.SocietyRegistration.AppointedName = rdata.UserDetails.Name;
                            $scope.SocietyRegistration.AppointedGender = rdata.UserDetails.Gender;
                            $scope.SocietyRegistration.AppointedDOB = ((rdata.UserDetails.DOB).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = rdata.UserDetails.Photo;
                            $scope.SocietyRegistration.AppointedFile = rdata.UserDetails.Photo;
                            $scope.AppointedFile = rdata.UserDetails.Photo;
                            $scope.CloseOtpBox();
                            $scope.TreasurerProfile = true;
                            $scope.TREASURERRemove = true;
                            $scope.GETTREASURER = false;
                        }
                        else {
                            $scope.CurrentRowDataFill(BIOdataList[0])
                        }
                        //$('#ssoauthloading').hide();
                        //$scope.CloseOtpBox();
                        //$scope.MsgAadhar = _language == 'en-us' ? 'Successfully Varified' : "सफलतापूर्वक सत्यापित किया गया";
                        //$scope.messagebox = true;
                        //return;

                        //});
                        $scope.showDetails = true;
                        //window.location = '../Election/ElectionSocietyList';
                    } else {
                        ShowOkMessage("Warning", _language == 'en-us' ? "UIDAI server not responding, Try again after some time." : "UIDAI सर्वर प्रतिक्रिया नहीं दे रहा है, कुछ समय बाद पुन: प्रयास करें।", function () { });
                        //ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again" : "Invalid Data. Please try again", function () { });
                        $scope.showDetails = false;
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con_bio_submit").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again." : "Invalid Data. Please try again.", function () { }); //return false;
                });
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
        }

    };



    function getHttpError(jqXHR) {
        var err = "Unhandled Exception";
        if (jqXHR.status === 0) {
            err = 'Service Unavailable';
        } else if (jqXHR.status == 404) {
            err = 'Requested page not found';
        } else if (jqXHR.status == 500) {
            err = 'Internal Server Error';
        } else if (thrownError === 'parsererror') {
            err = 'Requested JSON parse failed';
        } else if (thrownError === 'timeout') {
            err = 'Time out error';
        } else if (thrownError === 'abort') {
            err = 'Ajax request aborted';
        } else {
            err = 'Unhandled Error';
        }
        return err;
    }

    $scope.OnPageLoad();
    $scope.getdataForAdhar();
    $scope.ShowValidateBtn = true;
    $scope.OtpDilogBox = false;
    $scope.messagebox = false;
    $scope.GETCEO = true;
    $scope.GETCHAIRMAN = true;
    $scope.GETAPPOINTED = true;
    $scope.GETlIQUIDATOR = true;
    $scope.GETSPCHAIRMAN = true;


    // GET Business
    $scope.SWCSData = [];
    $scope.GetSWCSData = function () {

        $http.post(SiteUrl + "/citizen/BGetSWCSData").
            success(function (data, status) {
                $scope.SWCSData = data;
            }).
            error(function (data, status) {
                $scope.SWCSData = [];
            });
    }

    //  $scope.GetSWCSData();
    $scope.SocietyDocuments = [];
    $scope.SetSocietyDocuents = function () {
        /*$scope.Levelofficedata();*/
        $scope.SocietyDocuments = [];
        $scope.SocietyDocuments.push({ Id: 1, Name: $scope.translation.RegistrationCertificate, FileId: "flRegistration_Certificate", IsShow: $scope.IsProfiling, Size: "5120", FileType: "application/pdf", Format: "" });
        $scope.SocietyDocuments.push({ Id: 2, Name: $scope.translation.Bylaws, FileId: "flBy_laws", IsShow: $scope.Act2001 || $scope.SocietyRegistration.ActId == 7, Size: "5120", FileType: "application/pdf", Format: $scope.Act2001 ? "" : "/Content/SocietyProfiling/rulesandregulation.pdf" });
        $scope.SocietyDocuments.push({ Id: 3, Name: $scope.translation.CooperativesList, FileId: "flCooperatives_List", IsShow: $scope.Act2001, Size: "5120", FileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel", Format: "/Content/SocietyExcel/UploadBalanceSheet.xlsx", Format: "/Content/SocietyExcel/Unit.xlsx" });
        $scope.SocietyDocuments.push({ Id: 4, Name: $scope.translation.SocietyWorkPlanEconomicViability, FileId: "flSocietyWorkPlanEconomic_Viability", IsShow: $scope.Act2001, Size: "5120", FileType: "application/pdf", Format: "" });
        $scope.SocietyDocuments.push({ Id: 5, Name: $scope.translation.GeneralMeetingProcedure, FileId: "flGeneralMeeting_Procedure", IsShow: $scope.Act2001, Size: "5120", FileType: "application/pdf", Format: "" });
        $scope.SocietyDocuments.push({ Id: 6, Name: $scope.translation.BalanceSheetUpload, FileId: "flBalanceSheet_Upload", IsShow: $scope.Act2001, Size: "5120", FileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel", Format: "/Content/SocietyExcel/UploadBalanceSheet.xlsx" });
        $scope.SocietyDocuments.push({ Id: 7, Name: $scope.translation.UploadObjects, FileId: "flUpload_Objects", IsShow: $scope.Act1958, Size: "5120", FileType: "application/pdf", Format: "/Content/SocietyProfiling/Objects.pdf" });
        $scope.SocietyDocuments.push({ Id: 8, Name: $scope.translation.RulesAndRegulation, FileId: "flRulesAnd_Regulation", IsShow: $scope.Act1958, Size: "5120", FileType: "application/pdf", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
        //Ram        
        if ($scope.SocietyRegistration.SocietyTypeId == 9) {
            $scope.SocietyDocuments.push({ Id: 9, Name: $scope.translation.UploadMembersList, FileId: "flUploadMembers_List", IsShow: $scope.Act1958 || $scope.Act2001 || $scope.SocietyRegistration.SocietyTypeId == 7, Size: "10240", FileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel", Format: "/Content/SocietyExcel/UpdatedMemberList.xlsx" });
        }
        else {
            $scope.SocietyDocuments.push({ Id: 9, Name: $scope.translation.UploadMembersList, FileId: "flUploadMembers_List", IsShow: $scope.Act1958 || $scope.Act2001 || $scope.SocietyRegistration.ActId == 7, Size: "10240", FileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel", Format: "/Content/SocietyExcel/MemberList.xlsx" });
        }
        //Ram
        $scope.SocietyDocuments.push({ Id: 10, Name: $scope.translation.UploadOldExecutiveCommittee, FileId: "flUploadOldExecutive_Committee", IsShow: $scope.Act1958 && !$scope.SocietyRegistration.IsNewRegistration, Size: "5120", FileType: "application/pdf", Format: "" });
        $scope.SocietyDocuments.push({ Id: 11, Name: $scope.translation.DLSCAffiliationCertificates, FileId: "flDLSCAffiliation_Certificates", IsShow: $scope.SocietyRegistration.ActId == 7 && $scope.FileDLSC, Size: "5120", FileType: "application/pdf", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
        $scope.SocietyDocuments.push({ Id: 12, Name: ($scope.SocietyRegistration.IsStatLevel === 'Yes' ? $scope.translation.SOCAffiliationCertificate : $scope.translation.DOCAffiliationCertificates), FileId: "flSOCAffiliation_Certificate", IsShow: $scope.SocietyRegistration.ActId == 7 && $scope.IsAffiliatedBySOC, Size: "5120", FileType: "application/pdf", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
        $scope.SocietyDocuments.push({ Id: 13, Name: $scope.translation.SLSAffiliationCertificates, FileId: "flSLSAffiliation_Certificates", IsShow: $scope.SocietyRegistration.ActId == 7 && $scope.FileSLSA, Size: "5120", FileType: "application/pdf", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
        $scope.SocietyDocuments.push({ Id: 14, Name: $scope.translation.SLSCAffiliationCertificates, FileId: "flSLSCAffiliation_Certificates", IsShow: $scope.SocietyRegistration.ActId == 7 && $scope.LevelState, Size: "5120", FileType: "application/pdf", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
        $scope.SocietyDocuments.sort((a, b) => a.Id > b.Id);
    }

    $scope.SocietyOthersDocuments = [];
    //$scope.SocietyOthersDocuments.push({ Id: $scope.SocietyOthersDocuments.length + 1, Name: "", FileId: "" });
    $scope.AddOthersDocuments = function () {
        $scope.SocietyOthersDocuments.push({ Id: $scope.SocietyOthersDocuments.length + 1, Name: "", FileName: "", FileId: "", IsFileUpload: false });
    }


    //END
    $scope.SocietyDirectoryFile = "";
    $scope.UploadFile = function (id, type, index, model, UploadType, fileext, FileName) {
        try {

            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;


                var formData = new FormData();
                var filetype = fileext == 'flUploadMembers_List' || fileext == 'flBalanceSheet_Upload' ? "excel" : "";
                formData.append(fileext, files[0]);

                $(".blockUI").show();

                var RegistrationNo = $scope.SocietyRegistration.RegistrationNo == undefined ? "" : $scope.SocietyRegistration.RegistrationNo.replace(/ /g, '').replace(/\\/g, "").replace(/-/g, "").replace(/\//g, "");
                $scope.SocietyDirectoryFile = RegistrationNo;
                var OldRegistrationNo = $scope.OldRegistrationNo == undefined ? "" : $scope.OldRegistrationNo.replace(/ /g, '').replace(/\\/g, "").replace(/-/g, "").replace(/\//g, "");
                var SocietyIDEnc = $scope.SocietyRegistration.SocietyIdEnc == undefined ? "" : $scope.SocietyRegistration.SocietyIdEnc;

                formData.append("OldRegistrationNo", $scope.OldRegistrationNo);
                $scope.xhrProfile(SiteUrl + "/Society/UploadProfilingFiles_v1?IsNewRegistration=" + ($scope.SocietyRegistration.IsNewRegistration == null || $scope.SocietyRegistration.IsNewRegistration == undefined || $scope.SocietyRegistration.IsNewRegistration == '' ? false : $scope.SocietyRegistration.IsNewRegistration) + "&RegistrationNo=" + RegistrationNo + "&OldRegistrationNo=" + OldRegistrationNo + "&SocietyIDEnc=" + SocietyIDEnc + "&filetype=" + fileext + "&ActId=" + $scope.SocietyRegistration.ActId + "&FileName=" + FileName, formData, type, index, model);


            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }
    $scope.xhrProfile = function (url, data, type, index, model, UploadType) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");

                    if (request.responseText.indexOf('@') > -1) {
                        ShowOkMessage("Error", request.responseText.split('@')[1], function () { });
                    } else
                        if (request.responseText == "") {
                            ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                        } else {
                            $scope.FileCollection.forEach(function (itm, indx) {
                                if (itm.FileId == model) {
                                    itm.FileName = ($scope.SocietyDirectoryFile != undefined && $scope.SocietyDirectoryFile != '' ? "SocietyProfiling" : "") + '/' + request.responseText;
                                    itm.File = request.responseText;
                                    $("#" + model).parent().parent().css('border', 'none');
                                }
                            });
                            //$scope["" + model + ""]  = request.responseText;                        
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

                //if (($(".addprofilePhoto").length >= uploadIndex + 1) || ($(".addFamilyPhoto").length >= uploadIndex + 1)) {
                //    $(".blockUI").hide();
                //    LoadView('_PhotosVideos', 'tb-photosvideos');
                //}
                //++uploadIndex;
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }
    $scope.filesModal = { 'Id': 0, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: "", IsFileUpload: false }
    $scope.FileCollection = [];
    $scope.ChangeFile = function (id) {
        var FileCollection = $scope.FileCollection;
        $scope.FileCollection = [];
        FileCollection.forEach(function (item, index) {
            if (item.FileId != id) {
                $scope.FileCollection.push(item);
            }
        });
        $("#" + id).val("");
        $("#" + id).parent().parent().css('border', 'none');
    }
    $scope.IsFileUpload = function (id) {
        return $scope.FileCollection.filter(function (x) { return x.FileId == id && x.IsFileUpload == true }).length > 0;
    }
    $scope.GetFileUploaded = function (id) {
        try {
            return $scope.FileCollection.filter(function (x) { return x.FileId == id && x.IsFileUpload == true })[0].FileName;
        } catch (ex) {
            return "";
        }
    }


    $scope.ChangeOtherFile = function (id) {
        var SocietyOthersDocuments = $scope.SocietyOthersDocuments;
        $scope.SocietyOthersDocuments = [];
        SocietyOthersDocuments.forEach(function (item, index) {
            if (item.Id != id) {
                $scope.SocietyOthersDocuments.push(item);
            }
        });
        $("#" + id).val("");
        $("#" + id).parent().parent().css('border', 'none');
    }
    $scope.IsOtherFileUpload = function (id) {
        return $scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id && x.IsFileUpload == true }).length > 0;
    }
    $scope.GetOtherFileUploaded = function (id) {
        try {
            return $scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id && x.IsFileUpload == true })[0].Name;
        } catch (ex) {
            return "";
        }
    }

    $scope.UploadOtherFile = function (id, index) {
        try {
            $("#txtOtherDocuemnt_" + index).removeClass("Redborder");
            if ($scope.SocietyOthersDocuments[index].FileName == undefined || $scope.SocietyOthersDocuments[index].FileName == "") {
                $("#txtOtherDocuemnt_" + index).addClass("Redborder");
                ShowOkMessage("", "Please enter other docuemnt name", function () {

                });
            } else {

                if (window.FormData !== undefined) {
                    var fileUpload = $("#" + id).get(0);
                    var files = fileUpload.files;
                    var formData = new FormData();
                    formData.append("file", files[0]);
                    $(".blockUI").show();
                    var RegistrationNo = $scope.SocietyRegistration.RegistrationNo == undefined ? "" : $scope.SocietyRegistration.RegistrationNo;//.replace(/ /g, '').replace(/\\/g, "").replace(/-/g, "").replace(/\//g, "");
                    $scope.SocietyDirectoryFile = RegistrationNo;
                    var OldRegistrationNo = $scope.OldRegistrationNo == undefined ? "" : $scope.OldRegistrationNo;///.replace(/ /g, '').replace(/\\/g, "").replace(/-/g, "").replace(/\//g, "");
                    var SocietyIDEnc = $scope.SocietyRegistration.SocietyIdEnc == undefined ? "" : $scope.SocietyRegistration.SocietyIdEnc;

                    formData.append("OldRegistrationNo", $scope.OldRegistrationNo);
                    $scope.xhrOtherFile(SiteUrl + "/Society/UploadSocietyOtherfiles?RegistrationNo=" + RegistrationNo + "&OldRegistrationNo=" + OldRegistrationNo + "&SocietyIDEnc=" + SocietyIDEnc + "&SocietyId=" + $scope.SocietyRegistration.SocietyID + "&FileName=" + $("#txtOtherDocuemnt_" + index).val(), formData, id);


                } else {
                    alert("FormData is not supported.");
                }
            }
        }
        catch (ex) { alert(ex) }
    }
    $scope.xhrOtherFile = function (url, data, model) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");

                    if (request.responseText.indexOf('@') > -1) {
                        ShowOkMessage("Error", request.responseText.split('@')[1], function () { });
                    } else
                        if (request.responseText == "") {
                            ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                        } else {
                            $scope.SocietyOthersDocuments.forEach(function (itm, indx) {
                                if (itm.Id == model) {
                                    itm.Name = ($scope.SocietyDirectoryFile != undefined && $scope.SocietyDirectoryFile != '' ? "SocietyProfiling" : "") + '/' + request.responseText;
                                    itm.File = request.responseText;
                                    $("#" + model).parent().parent().css('border', 'none');
                                }
                            });
                            //$scope["" + model + ""]  = request.responseText;                        
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

                //if (($(".addprofilePhoto").length >= uploadIndex + 1) || ($(".addFamilyPhoto").length >= uploadIndex + 1)) {
                //    $(".blockUI").hide();
                //    LoadView('_PhotosVideos', 'tb-photosvideos');
                //}
                //++uploadIndex;
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    //$scope.SetOldExecutiveType = function (type) {
    //    $scope.TableData10.forEach(function (item, index) {
    //        item.IsEdit = true;
    //    });
    //}

    $scope.ddlDistrct = { DISTRICT_ID: "" }
    $scope.GetSocityDataForOtp = function () {
        if ($scope.ddlDistrct.DISTRICT_ID == undefined || $scope.ddlDistrct.DISTRICT_ID == "") {
            ShowOkMessage("", "Please select district of your society", function () { });
        } else {
            $(".se-pre-con").fadeIn("slow");
            $scope.textchange = true;
            $scope.Showgride = false;
            $scope.showotpblock = false;
            $http.post(SiteUrl + "/Citizen/GetSocityDataForOtp2", { "SocietyRegistrationNo": $scope.SocietyRegistration.SocietyRegistrationNo, "DistrictId": $scope.ddlDistrct.DISTRICT_ID, "ActId": $scope.SocietyRegistration.ActId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                   // console.log(data);

                    var resultData = data.data;//.filter(function (x) { return x.ActId == 1 });

                    $scope.showSoceitygride = true;
                    var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';

                    if (data.Result === false) {
                        $scope.Showgride = false;
                        $scope.textchange = false;
                        $scope.showSoceitygride = false;
                        var msg1 = 'आपकी सोसाइटी की जानकारी राज-सहकार पोर्टल पर उपलब्ध नहीं  है। <br/><span style="font-size:15px;color: red;"> Note:- आप द्वारा प्रोफाइलिंग के दौरान दर्ज की गयी जानकारी का विभाग द्वारा सत्यापन किया जायेगा उसके बाद आप संशोधन हेतु आवेदन कर पाएंगे। </span> '
                        ShowAlertMessage_v1("", "प्रोफाइलिंग करें", "रद्द करें", msg1, function () {
                            $scope.IsSearchResult = false;
                            $scope.IsShowProfiling = true;
                            $scope.IsShowAmendment = false;
                            $scope.divOtherSocietys == true;
                            $scope.GetSocietyDetails = false;
                            $scope.IsProfiling = true;
                            var addressdataset1 = {
                                Housebuilding: " ",
                                State: "6",
                                District: " ",
                                Lanestreet: "",
                                Locality: "",
                                Pincode: "",
                                CITY_ID: "",
                                BLOCK_ID: "",
                                GP_ID: "",
                                WARD_Id: "",
                                VILLAGE_ID: ""
                            }
                            //$scope.BrnDetails();
                            SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function () {

                            });

                            $scope.$apply();
                        });

                    }
                    else {
                        $scope.SocietyRegdata = [];
                        $scope.SocietyRegdata = resultData;
                        $scope.IsSearchResult = true;
                        $scope.IsShowProfiling = false;
                        $scope.IsShowAmendment = false;
                    }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.SocietyRegdata = [];
                    $(".se-pre-con").fadeOut("slow");
                });
            //$(".se-pre-con").fadeOut("slow");
        }
    }

    $scope.SelectSocietyForVerify = function (item, index) {
        //alert(SocietyID);
        $scope.SocietyRegdata.forEach(function (item1, index) {
            item1.ischecked = false;
        });
        item.ischecked = true;
        var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
        if (item.IsSsoFound === true && item.IsSameSSO === false) {
            var msg3 = 'यह सोसाइटी किसी और की SSO ID से मैप है। क्या आप इसे अपनी SSO ID से मैप करना चाहते है ?';
            $scope.SocietyRegdataSelected = [];
            ShowAlertMessage("", msg3, function () {
                window.location.href = "../Citizen/UserSocietyMappingValid";
            });
        } else if (item.IsSsoFound === true && item.IsSameSSO === true && (item.ApprovalStatusId == 1 || item.ApprovalStatusId == 5 || item.ApprovalStatusId == 10 || item.ApprovalStatusId == 20 || item.ApprovalStatusId == 27)) {
            $scope.IsShowSearchSociety = false;
            $scope.IsBackToSearchList = true;
            $scope.ShowDetails(item.SocietyIDEnc, 'ShowDetails', item.MaxAppliFlowStatusID);
            $scope.ShowDetailsWorkscope(item.SocietyIDEnc, 'flase');
        }
        else if (item.IsSsoFound === true && item.IsSameSSO === true && (item.ApprovalStatusId == 17 || item.ApprovalStatusId == 26)) {
            $scope.IsShowSearchSociety = false;
            $scope.IsBackToSearchList = true;
            $scope.ApplicationView(item.SocietyIDEnc);
        } else if (item.IsSsoFound === true && item.IsSameSSO === true && item.IsAmendment == true) {
            $scope.IsShowSearchSociety = false;
            $scope.IsBackToSearchList = true;
            $scope.ApplicationView(item.SocietyIDEnc);
        }
        else if (item.IsSsoFound === true && item.IsSameSSO === true) {
            $scope.IsShowSearchSociety = false;
            $scope.IsBackToSearchList = true;
            $scope.ShowDetails(item.SocietyIDEnc, 'ShowDetails', item.MaxAppliFlowStatusID);
            $scope.ShowDetailsWorkscope(item.SocietyIDEnc, 'flase');
        } else {
            $scope.IsBackToSearchList = true;
            $scope.IsShowSearchSociety = false;
            $scope.ShowDetails(item.SocietyIDEnc, 'ShowDetails', item.MaxAppliFlowStatusID);
            $scope.ShowDetailsWorkscope(item.SocietyIDEnc, 'flase');

        }
    }



});


CitizenModule.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");
                        var maxLength = scope.SocietyDocuments.filter(function (x) { return x.FileId == id })[0].Size;
                        // The size of the file. 
                        if (file >= parseInt(maxLength)) {
                            ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than " + parseInt(maxLength / 1022) + " MB" : "फ़ाइल बहुत बड़ी है, कृपया " + maxLength / 1022 + " MB से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {

                            if (scope.FileCollection.filter(function (x) { return x.FileId === id }).length <= 0) {
                                scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                            } else {
                                scope.FileCollection.forEach(function (itm, indx) {
                                    if (itm.FileId == id) {
                                        itm.IsFileUpload = true;
                                    }
                                });
                            }

                            //scope.AuditorAssign["IsUpload" + id + ""] = true;
                            //scope.AuditorAssign["" + id + ""] = "";

                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});
CitizenModule.directive('validotherfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");
                        var maxLength = 2048;
                        //var maxLength = scope.SocietyDocuments.filter(function (x) { return x.FileId == id })[0].Size;
                        // The size of the file. 
                        if (file >= parseInt(maxLength)) {
                            ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than " + parseInt(maxLength / 1022) + " MB" : "फ़ाइल बहुत बड़ी है, कृपया " + maxLength / 1022 + " MB से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {

                            if (scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id }).length <= 0) {
                                scope.SocietyOthersDocuments.push({ Id: id, Name: "", FileId: id, IsFileUpload: true });
                            } else {
                                scope.SocietyOthersDocuments.forEach(function (itm, indx) {
                                    if (itm.Id == id) {
                                        itm.IsFileUpload = true;
                                    }
                                });
                            }

                            //scope.AuditorAssign["IsUpload" + id + ""] = true;
                            //scope.AuditorAssign["" + id + ""] = "";

                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});