var TableData10 = [];
var CitizenModule = angular.module("CitizenMod", ['angularUtils.directives.dirPagination']);
var ImageDataList = [];
var BIOdataList = [];
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
    var MemberDetails = [];
    var NOMINATEDMEMBERS = [];
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

    $scope.OpenOtpPopUpOther = function (AADHAARNo, FOR, TRID) {
        if ($('#' + TRID).find('td:eq(1)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(1)').find('input').val().length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('.se-pre-con').hide();
        var arrylist = $('#' + TRID).find('td:eq(1)').find('input').val().split('');

        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = $('#' + TRID).find('td:eq(1)').find('input').val();
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.FOR = FOR;
        $scope.CurrentRow = TRID;
    };

    $scope.OpenOtpPopUpOtherFun = function () {
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('.se-pre-con').hide();
        var TRID = $("#hdRowId").val();
        if ($('#' + TRID).find('td:eq(1)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(1)').find('input').val().length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        var arrylist = $('#' + TRID).find('td:eq(1)').find('input').val().split('');
        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = $('#' + TRID).find('td:eq(1)').find('input').val();
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.CurrentRow = TRID;
    };

    $scope.GenrateOTP = function () {
        $('.se-pre-con').show();
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + $scope.AADHAARNo).
            success(function (data, status) {
                if (data[0].txnid.length > 0) {
                    $scope.OTPStatus = true;
                    if (data[0].txnid.split('#')[0] == "NO") {
                        alert(data[0].txnid.split('#')[1]);
                        $('.se-pre-con').hide();
                    }
                    else if (data[0].txnid.indexOf("DOITRJGOVAUTH") > -1) {
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $('#btnSSOAuth_ReSendOTP').show();
                        $scope.txn = data[0].txnid;//.split('#')[1];
                        $('.se-pre-con').hide();
                    }
                    else {
                        $('.se-pre-con').hide();
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $('#btnSSOAuth_ReSendOTP').show();
                        //$scope.txn = data[0].txnid.split('#')[1];
                        $scope.txn = data[0].txnid;
                        //  $scope.Showgride = false;                       
                    }
                }
                else {
                    alert('Some Error Occured Please Try Again');
                    $('.se-pre-con').hide();
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }

    $scope.ValidateOTP = function () {
        $('.se-pre-con').show();
        alert($scope.SocietyDirectory);
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + $scope.AADHAARNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + $scope.txn + "&_SocietyDirectory=" + $scope.SocietyDirectory).
            success(function (data, status) {

                $('.se-pre-con').hide();
                if (data[0].Column1.length > 0) {
                    if (data[0].Column1 == "Failed") {
                        //$scope.AADHAARNo = '';
                        //$('#SSOAuth_OtpText').val('');                       
                        //alert(data[0].Column2);
                        ShowOkMessage("Error", data[0].Column2, function () { });
                        //$('#btnSSOAuth_SendOTP').show();
                        //$('#btnSSOAuth_ValidateOTP').hide();
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
            //var url = "http://127.0.0.1:11100/capture";
            //var PIDOPTS = "<PidOptions ver=\"1.0\"><Opts fCount=\"1\" fType=\"0\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"10000\" otp=\"\" wadh=\"RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw=\" posh=\"\"/></PidOptions>";
            /*
            format=\"0\"     --> XML
            format=\"1\"     --> Protobuf
            */
            // $scope.CaptureMorpho();
            $scope.CaptureMantra();


            //////try {

            //////    var error_msg = "";
            //////    if (_language == 'en-us') {
            //////        error_msg = "Use following possible steps to resolve the issue:<br>1. Use Internet Explorer(IE) for BIO KYC <br/> 2. Register your device from vendor(https://rdserviceonline.com/) or using Register Your Device link.<br>3. Download and install the latest registered device drivers.<br>4. Start/ restart the registered device service. <br/> 4. Allow device Access from Your System Setting 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
            //////    } else {
            //////        error_msg = "समस्या को हल करने के लिए संभव चरणों का उपयोग करें: <br>1. BIO KYC के लिए इंटरनेट एक्सप्लोरर (IE) का उपयोग करें।  <br/> 2. अपने डिवाइस को वेंडर (https://rdserviceonline.com/) से पंजीकृत करें या रजिस्टर योर डिवाइस लिंक का उपयोग करें। <br>3. नवीनतम पंजीकृत डिवाइस ड्राइवर डाउनलोड और स्थापित करें। <br>4. पंजीकृत डिवाइस सेवा शुरू / पुनः आरंभ करें। <br/>5. अपने सिस्टम सेटिंग से इंटरनेट एक्सेस की अनुमति दें 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
            //////    }

            //////    var xhr;
            //////    var ua = window.navigator.userAgent;
            //////    var msie = ua.indexOf("MSIE ");
            //////    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return version number
            //////    {
            //////        //IE browser
            //////        xhr = new ActiveXObject("Microsoft.XMLHTTP");
            //////    } else {
            //////        //other browser
            //////        xhr = new XMLHttpRequest();
            //////    }
            //////    xhr.open('CAPTURE', url, true);
            //////    xhr.setRequestHeader("Content-Type", "text/xml");
            //////    xhr.setRequestHeader("Accept", "text/xml");
            //////    xhr.onreadystatechange = function () {
            //////        if (xhr.readyState == 4) {
            //////            var status = xhr.status;
            //////            if (status == 200) {
            //////                if (xhr.responseText.indexOf("Device not ready") > -1)
            //////                {
            //////                    $scope.CaptureMantra();
            //////                } else {
            //////                    $scope.VerifyBioData(xhr.responseText);
            //////                }
            //////                //alert(xhr.responseText);

            //////            } else {
            //////                ShowOkMessage("Error", "15" + error_msg, function () { });
            //////            }
            //////        }
            //////    };

            //////    //xhr.onload = function () {
            //////    //    if (xhr.status == 200) { $scope.VerifyBioData(xhr.responseText); }
            //////    //    else {
            //////    //        ShowOkMessage("Error", "15" + error_msg, function () { }); 
            //////    //    }
            //////    //};

            //////    xhr.send(PIDOPTS);
            //////    xhr.onerror = function () {
            //////        //callback(xhr.status);
            //////        //console.log(xhr.status);
            //////        $scope.CaptureMantra();
            //////        //ShowOkMessage("Error", "11" + error_msg, function () { });
            //////    };
            //////    //xhr.ontimeout = function () {
            //////    //    //callback(xhr.status);
            //////    //    ShowOkMessage("Error", "12" +error_msg, function () { });
            //////    //};
            //////    //xhr.abort = function () {
            //////    //   // callback(xhr.status);
            //////    //    ShowOkMessage("Error", "13" +error_msg, function () { });
            //////    //};

            //////} catch (ex) {
            //////    //ShowOkMessage("Error", "14" +error_msg , function () { });
            //////}
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
        }
        $scope.showLoader = false;
    };

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


            // var data1111 = "<?xml version='1.0'?>            <PidData>                <Resp errCode='0' errInfo='Success' fCount='1' fType='0' nmPoints='45' qScore='77' />          <DeviceInfo dpId='MANTRA.MSIPL' rdsId='MANTRA.WIN.001' rdsVer='1.0.0' mi='MFS100' mc='' dc='e6c82d8d-219d-4184-8f67-39bae404caf3'>                  <additional_info>                        <Param name='srno' value='1185537' />                        <Param name='sysid' value='6EAFEBF2CFF0C1FFBFF0' />                        <Param name='ts' value='2019-12-11T12:20:35+05:30' />                    </additional_info>                </DeviceInfo>                <Skey ci='20191230'></Skey>                <Hmac>hIkK416l1buzNexuoAt3K1BIECNcGdEBdhG+1HQL2ut+Zt1sC3CgxwQ5uDu3BSlA</Hmac>                <Data type='X'></Data>            </PidData>";

            var data = { AadharNo: $scope.AADHAARNo, xml: XMLData };

            $(".se-pre-con_bio").fadeIn("slow");
            $http.post(SiteUrl + "/Citizen/ValidateBIOKYC", JSON.stringify({ Details: data })).
                success(function (rdata, status) {
                    $(".se-pre-con_bio").fadeOut("slow");
                    console.log(rdata);
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
                        $scope.CaptureMorpho();
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
                    $(".se-pre-con_bio").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again." : "Invalid Data. Please try again.", function () { }); //return false;
                });
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
        }

    };

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
        $('#' + TR).find('td:eq(1)').find('input').removeAttr("disabled");
        $('#' + TR).find('td:eq(1)').find('input').val('');//AADHAR
        $('#' + TR).find('td:eq(2)').find('input').val('');//NAME      
        $('#' + TR).find('td:eq(3)').find('input').val('');//FATHERNAME
        $('#' + TR).find('td:eq(4)').find('input').removeAttr("disabled");//MOBILE NO
        $('#' + TR).find('td:eq(4)').find('input').val('');//MOBILE NO
        $('#' + TR).find('td:eq(5)').find('input').val('');//DOB
        $('#' + TR).find('td:eq(6)').find('label').text('');
        $('#' + TR).find('td:eq(7)').find('select').val('0');
        $('#' + TR).find('td:eq(7)').find('select').removeAttr("disabled");
        $('#' + TR).find('td:eq(8)').find('input').val('');//POST
        $('#' + TR).find('td:eq(8)').find('input').removeAttr("disabled");
        if (TR > 1000 && TR < 2000) {
            $('#' + TR).find('td:eq(8)').find('textarea').val('');
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $($('#' + TR).find('td:eq(10)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(10)').find('i')[0]).show();
        }
        if (TR > 499 && TR < 1000) {
            $('#' + TR).find('td:eq(8)').find('select').val(0);
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $($('#' + TR).find('td:eq(10)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(10)').find('i')[0]).show();
        }
        else {
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(10)').find('input').val('');//DOB
            $('#' + TR).find('td:eq(11)').find('input').val('');//DOB
            $('#' + TR).find('td:eq(12)').find('input').val('');//DOB
            $('#' + TR).find('td:eq(13)').find('input').val('');//DOB
            $($('#' + TR).find('td:eq(16)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(16)').find('i')[0]).show();
        }
    }
    $scope.CurrentRowDataFill = function (data) {
        var TR = $scope.CurrentRow;

        $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
        $('#' + TR).find('td:eq(2)').find('input').val(data.Column2);//NAME  
        $('#' + TR).find('td:eq(3)').find('input').val(data.Column8);//FNAME         
        $('#' + TR).find('td:eq(5)').find('input').val(((data.Column4).replace('-', '/')).replace('-', '/'));
        $('#' + TR).find('td:eq(6)').find('label').text(data.Column3 == 'M' ? 'MALE' : 'FEMALE');
        if (TR > 1000 && TR < 2000) {
            ($('#' + TR).find('td:eq(10)').find('img'))[0].src = data.Column5;
            $('#' + TR).find('td:eq(9)').find('textarea').val(data.Column10);//ADDRESS  
            $($('#' + TR).find('td:eq(12)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(10)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
            var jElement = $('#' + TR).find('td:eq(9)').find('textarea');
            jElement.css('border', 'rgb(222, 221, 221)');
        }
        else if (TR > 499 && TR < 1000) {
            $('#' + TR).find('td:eq(3)').find('label').text(data.Column10);//FNAME  
            ($('#' + TR).find('td:eq(10)').find('img'))[0].src = data.Column5;
            $($('#' + TR).find('td:eq(11)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(11)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(10)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
        }
        else if (TR >= 0 && TR < 500) {
            $('#' + TR).find('td:eq(3)').find('label').text(data.Column10);//FNAME  
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = data.Column5;
            $($('#' + TR).find('td:eq(16)').find('i')[0]).hide();
            $($('#' + TR).find('td:eq(16)').find('i')[1]).show();
            var jElement = $('#' + TR).find('td:eq(9)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');
        }


        var jElement = $('#' + TR).find('td:eq(1)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(2)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(3)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(4)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(6)').find('label');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(7)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(8)').find('select');
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

    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.divOtherSocietys = false;
        $scope.nepster = true;
        $scope.DashboardData();
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

                if ($('#EUserType').val() == 'K' && ($('#EServiceid').val() == '4209' || $('#EServiceid').val() == '4172')) {

                    if ($('#EServiceid').val() == '4209')//PROFILE
                    {
                        $scope.AddNewSociety();
                        $scope.CheckRegOrProfileing();
                        $scope.SocietyRegistration.IsNewRegistration = 'false';
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
                        $scope.CheckRegOrProfileing();
                        $scope.SocietyRegistration.IsNewRegistration = 'true';
                        $("input[name='RegistrationProfile']")[0].checked = true;
                        $("input[name='RegistrationProfile']")[1].checked = false;
                        //$scope.divBrn = true; 
                        $scope.CheckBRN = true;
                        //$scope.divBrnYN = false; 
                        $scope.divBrnbtn = true;
                        $scope.nepster = true;

                    }


                    $scope.all = true;
                    $scope.SocietyAct = false;

                }


            }).
            error(function (data, status) {


            });

        //START : Get State List        
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=STATE&MasterCode=-1").
            success(function (data, status) {
                $scope.Statelist = data;
                $scope.readonlyfun = true;
            }).
            error(function (data, status) {
                $scope.Statelist = [];
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

        //Get CEO Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=9").
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {


            });
        // END : Post List for dropdown

        //Get Chairman Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.ChairmanPosts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown

        //Get Board Member Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-2").
            success(function (data, status) {
                $scope.BoardPosts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown

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
                if (Temp2[1].toString() == 'true') {
                    $scope.AddNewSociety();
                }
            }
        }
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
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
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
        if ($scope.SocietyRegistration.IsAdministratorAppointed == "false") {
            $scope.AppointedYN = false;
        }
        else if ($scope.SocietyRegistration.IsAdministratorAppointed == "true") {
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
            $scope.IsAdministrativeY = false;
            $scope.IsAdministrativeN = true;
            $scope.CEO = false;
            $scope.Isliquidator = false;
            $scope.AppointedYN = false;
            var temp = 0;
            if ($('#TblElectedMember tr').length) {
                TableData10 = [];
                var temp = 0;
                if ($scope.ActId != 7) {
                    var X = $scope.ActId == 1 ? 8 : 16;
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
                        if ($scope.ActId == '7') {
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
                    });
                    $scope.TableSportsData2 = TableData10;
                }
            }



            $('#TblElectedMember tr').each(function () {
                if ($scope.ActId == '3') {
                    if (temp == 0) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT')
                    }
                    else if (temp == 1) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                    }
                    else if (temp == 2) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'SECRETARY/CEO' : 'सचिव / मुख्य कार्यकारी अधिकारी')
                    }
                    else if (temp == 3) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'DIRECTOR' : 'निदेशक')
                    }
                }
                else {
                    if (temp == 0) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'CHAIRMAN/PRESIDENT' : 'CHAIRMAN/PRESIDENT')
                    }
                    else if (temp == 1) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'VICE CHAIRMAN' : 'उपाध्यक्ष')
                    }
                    else if (temp == 2) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'SECRETARY' : 'सचिव')
                    }
                    else if (temp == 3) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'TREASURER' : 'कोषाध्यक्ष')
                    }
                    else if (temp == 4) {
                        $(this).find('td:eq(7)').find('input').val($scope._language == 'en-us' ? 'DIRECTOR MEMBER' : 'निदेशक सदस्य')
                    }
                }

                temp = temp + 1;
            });

            TableData10NomiAll = [];
            if ($scope.ActId == 3) {
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
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegistration.CityTypes).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });


    };

    $scope.GetCityListByCityTypes = function (id) {

        if ($scope.SocietyRegisteredOffice.CityTypes != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypes).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.CityTypes_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypes_copy).
                success(function (data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.CityTypesSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypesSecretary).
                success(function (data, status) {
                    $scope.CityListdataSecretary = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.CityTypesTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypesTreasurer).
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
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDSecretary).
                success(function (data, status) {

                    $scope.GramPanchayatsSecretary = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer).
                success(function (data, status) {

                    $scope.GramPanchayatsTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }


    };
    // END : Get List of GramPanchayat
    //Start
    $scope.GramPanchayatListForBasicD = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegistration.BLOCK_ID).
            success(function (data, status) {

                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];

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
        $scope.Villages = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegistration.GP_ID).
            success(function (data, status) {

                $scope.Villages = data;
            }).
            error(function (data, status) {
                $scope.Villages = [];
            });
    };
    // END : Get List of Villages
    // Get List of Block
    $scope.BlockList = function () {
        $scope.Blocks = [];
        $scope.AddMoreForSportShow = true;
        if ($scope.SocietyRegistration.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                success(function (data, status) {

                    $scope.Blocks = data;
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
    //end block
    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        $scope.Blocks = [];
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
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
        else if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function (data, status) {

                    $scope.Blocksdatacopy = data;
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
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function (data, status) {

                    $scope.BlocksdataSecretary = data;
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
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function (data, status) {

                    $scope.BlocksdataTreasurer = data;
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
                    $scope.Blocksdata = [];
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
            $http.post(SiteUrl + "/Society/SaveSocietyRegisteredOfficeDetails", $scope.SocietyRegisteredOffice).
                success(function (data, status) {
                    if ((data.data.split('@')).length > 1) {
                        $scope.EMitraTransactionId = (data.data.split('@')).length > 1 ? (data.data.split('@'))[1] : '0';
                        $scope.SocietyRegistration.SocietyIdEnc = data.data.split('@')[0];
                        ShowModelAlert("Society Office Detail", 'Registered Office Detail Save Sucessfully');
                        //SHOW VIEW DATA
                        //$scope.ShowDetails($scope.SocietyRegisteredOffice.SocietyIdEnc, 'ShowDetails');
                        // $scope.ShowDetailsWorkscope($scope.SocietyRegisteredOffice.SocietyIdEnc);
                        //END                        
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
                    $scope.SocietyRegisteredOffice = [];
                });

        }

    }

    // END : Save Registered Office Details

    // START : Get Sub Class
    $scope.GetSocietyCategory = function () {
        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + $scope.SocietyRegistration.ClassId + "&SubClassId=" + $scope.SocietyRegistration.SubClassId).
            success(function (data, status) {
                $scope.SocietyCategoryList = data;
            }).
            error(function (data, status) {
                $scope.SocietyCategoryList = [];
            });
    };
    // END : Get Sub Class

    // START : Get Society Category List
    $scope.GetSubClass = function () {
        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyRegistration.ClassId + "&SocietyTypeId=" + $scope.SocietyRegistration.SocietyTypeId).
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
                    $('#TblNominatedMember').append('<tr id=' + rows + '>' +
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
                        '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblNominatedMember').append('<tr id=' + rows + '>' +
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
                        '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
            }
            else {
                // alert('false');
            }
        }
        if (Flag == 1) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = ($('#TblSportElectedMember tr').length) + 1001;
                if (_language == 'hi-in') {
                    $('#TblSportElectedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" placeholder= "निदेशक का नाम" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse"   autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input disabled="disabled" type= "text" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                        '<td><textarea type="text" name="Address" class="form-control" placeholder="Address" autocomplete="off"></textarea></td >' +
                        '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-show="SHOWREMOVE" onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ></td >' +
                        '<td><i class="fa fa-trash clsShowHide" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblSportElectedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type= "text" disabled="disabled" name= "FName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Father/ Husband Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="POST"></div></td >' +
                        '<td><textarea type="text" name="Address" class="form-control" placeholder="पता" autocomplete="off"></textarea></td >' +
                        '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
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
                    $('#TblSportNominatedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ><label style="display:none">{{item.Address}}</label></td >' +
                        '<td><input disabled="disabled" type= "text" name= "FatherHusbandName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                        '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblSportNominatedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
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
            if ($scope.AdministrativeFormValidation(Isvalidate)) {
                var rows = $('#TblElectedMember tr').length + 1;
                if (_language == 'hi-in') {
                    if (($scope.SocietyRegistration.IsNewRegistration == 'false' || $scope.SocietyRegistration.IsNewRegistration == 'False') && $scope.Act2001) {
                        $('#TblElectedMember').append('<tr id=' + rows + '>' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td><input type="text" name="AdhareNo" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName" placeholder="पिता / पति नाम"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"  placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>-- चुने --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                            '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text" class="form-control" placeholder="व्यवसाय"></td>' +
                            '<td><input type="text" class="form-control" placeholder="सदस्यता संख्या"></td>' +
                            '<td><input type="text" class="form-control" placeholder="आवंटित शेयर की संख्या"></td>' +
                            '<td><input type="text" class="form-control" placeholder="शेयर पूंजी राशि"></td>' +
                            '<td><select class="form-control" id="loaneeNonloane' + rows + '" ><option value="0" disabled>-- चुने --</option><option value="1" >ऋणी</option><option value= "2" >अऋणी</option> </select ></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                            '</tr>');
                    }
                    else {
                        $('#TblElectedMember').append('<tr id=' + rows + '>' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName"  placeholder="पिता / पति नाम"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"   placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>-- चुने --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                            '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text" class="form-control" placeholder="व्यवसाय"></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                            '</tr>');
                    }
                }
                else {
                    if (($scope.SocietyRegistration.IsNewRegistration == 'false' || $scope.SocietyRegistration.IsNewRegistration == 'False') && $scope.Act2001) {
                        $('#TblElectedMember').append('<tr id=' + rows + '>' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td><input type="text" name="AdhareNo"  placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName" ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName" placeholder="Father/ Husband Name"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"  placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select id="Category' + rows + '"><option value="0" disabled>-- Select --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" placeholder="Post"></div></td >' +
                            '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text"  placeholder="Occupation"></td>' +
                            '<td><input type="text"  placeholder="Membership No."></td>' +
                            '<td><input type="text"  placeholder="No. of Share Allotted"></td>' +
                            '<td><input type="text"  placeholder="Share Capital Amount"></td>' +
                            '<td><select class="form-control" id="loaneeNonloane' + rows + '" ><option value="0" disabled>-- Select --</option><option value="1" >loanee</option><option value= "2" >Non loane</option> </select ></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                            '</tr>');
                    }
                    else {
                        $('#TblElectedMember').append('<tr id=' + rows + '>' +
                            '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                            '<td><input type="text" name="AdhareNo"  placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                            '<td><input type= "text" disabled="disabled" name= "DirectorsName"  placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                            '<td><input type="text" name="FatherName"  placeholder="Father/ Husband Name"  autocomplete="off"><label style="display:none">{{item.Address}}</label></td >' +
                            '<td><input type="text" name="ContactNo"   placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                            '<td style="display:none"><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                            '<td style="display:none"><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                            '<td><div style="width:100%"><select  id="Category' + rows + '"><option value="0" disabled>-- Select --</option></select ></div ></td >' +
                            '<td><div style="width:100%"><input type="text" placeholder="Post"></div></td >' +
                            '<td style="display:none"><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                            '<td><input type="text"  placeholder="Occupation"></td>' +
                            '<td><input type="checkbox" class="checkbox-input"></td>' +
                            '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-trash-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
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
        $('#myTable2').append('<tr>' +
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
            $('#SportTableAdd').append('<tr>' +
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
        $('#SportTableAdd').append('<tr>' +
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
                $('#SportExecutiveTable').append('<tr>' +
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
            $('#SportExecutiveTable').append('<tr>' +
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
    // END : Add More Member Details

    // START : Save Society Profiling
    $scope.SaveSocietyProfiling = function (SubmitStatus) {

        $scope.SubmitStatus = SubmitStatus;
        if ($scope.FormValidation() || $scope.FormValidationPartial()) {
            var formData = new FormData();
            $(".se-pre-con").fadeIn("slow");
            if ($scope.SocietyRegistration.ActId != "7") {
                formData.append("BRN", $scope.SocietyRegistration.BRN);
                formData.append("SocietyIDEnc", $scope.SocietyRegisteredOffice.SocietyIdEnc);
                formData.append("ActId", $scope.SocietyRegistration.ActId);
                formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
                formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
                formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);
                formData.append("ClassId", $scope.SocietyRegistration.ClassId);
                formData.append("SubClassId", $scope.SocietyRegistration.SubClassId);
                formData.append("SocietyCategoryId", $scope.SocietyRegistration.SocietyCategoryId);
                formData.append("ByeLawsSocietyFile", $scope.ByeLawsSocietyFile);
                formData.append("RegistrationDate", $('#txtRegistrationDate').val().split("/").reverse().join("-"));
                formData.append("ElectionDate", $('#txtElectionDate').val().split("/").reverse().join("-"));
                formData.append("NoOfBranches", $scope.SocietyRegistration.NoOfBranches);
                //formData.append("IsStateLevel", $scope.SocietyRegistration.Yes ? '1' : '2');
                formData.append("LOSocietyOfficeDetails", $scope.SocietyRegistration.WorkScopeLevel);
                formData.append("STATE_ID", $scope.SocietyRegisteredOffice.STATE_ID);
                formData.append("DIVISION_ID", $scope.SocietyRegistration.DIVISION_ID);
                formData.append("DISTRICT_ID", $scope.SocietyRegistration.DISTRICT_ID);
                formData.append("TEHSIL_ID", $scope.SocietyRegistration.TEHSIL_ID);
                formData.append("BLOCK_ID", $scope.SocietyRegistration.BLOCK_ID);
                formData.append("VILLAGE_ID", $scope.SocietyRegistration.VILLAGE_ID);
                formData.append("RegistrationCertificateFile", $scope.RegistrationCertificateFile);
                formData.append("RulesAndRegulationFile", $scope.RulesAndRegulationFile);
                formData.append("lstSocietyWorkScope", JSON.stringify($scope.lstSocietyWorkScope));
                formData.append("IsNewRegistration", $scope.IsNewRegistration);

                //NEW 05/12/2018
                formData.append("BusinessId", $scope.SocietyRegistration.BusinessId);
                formData.append("LiablityId", $scope.SocietyRegistration.LiablityId);
                formData.append("MemberLiablity", $scope.SocietyRegistration.MemberLiablity);
                formData.append("TotalMembersApplyingForSocociety", $scope.SocietyRegistration.TotalNumberOfMembers);
                formData.append("AgmDate", $('#txtAGMDate').val().split("/").reverse().join("-"));
                formData.append("TotalNumberOfMembersInAGM", $scope.SocietyRegistration.NoOfMemberInAGM);
                formData.append("EntranceFeesCollected", $('#txtEntranceFees').val());
                formData.append("SingleShareValue", $scope.SocietyRegistration.SingleShareValue);
                formData.append("PaidUpShareCapital", $scope.SocietyRegistration.PaidUpShareCapital);
                formData.append("MemberListExcelFile", $scope.MembersListExcelFile);
                formData.append("WorkPlanShowingEconomicViabilityFile", $scope.WorkPlanEconomicViabilityFile);
                formData.append("GeneralMeetingProceedingFile", $scope.GeneralMeetingProcedureFile);
                formData.append("BalanceSheetFile", $scope.BalanceSheetExcelFile);
                formData.append("ObjectsFile", $scope.ObjectsFileName);
                formData.append("ObjectsFileName", $scope.ObjectsFileName);
                formData.append("IsNewRegistration", $scope.IsNewRegistration);
                formData.append("SaveStatus", $scope.SubmitStatus);


            }
            else {

                formData.append("SocietyIDEnc", $scope.SocietyRegisteredOffice.SocietyIdEnc);
                formData.append("ActId", $scope.SocietyRegistration.ActId);
                // by n saini
                if ($scope.SocietyRegistration.IsSportsNewRegistration == 'true' || $scope.SocietyRegistration.IsSportsNewRegistration == true) {
                    formData.append("IsNewRegistration", true);
                }
                else {
                    formData.append("IsNewRegistration", false);
                }
                formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
                formData.append("RegistrationDate", $('#txtSportRegistrationDate').val().split("/").reverse().join("-"));
                formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
                formData.append("DISTRICT_ID", $scope.SocietyRegistration.DISTRICT_ID);
                formData.append("IsStatLevel", $scope.SocietyRegistration.IsStatLevel == 'No' ? false : true);
                formData.append("AssociationId", $scope.SocietyRegistration.AssociationId.split('#')[0]);
                formData.append("ElectionDate", $('#txtSportElectionDate').val().split("/").reverse().join("-"));
                formData.append("AGMDate", $('#txtSportAgmDate').val().split("/").reverse().join("-"));
                formData.append("RSSCFileName", $scope.RSSCFileName);
                formData.append("DLSCFileName", $scope.DLSCFileName);
                formData.append("SOCFileName", $scope.SOCFileName);

                formData.append("lstSocietyWorkScope", JSON.stringify($scope.lstSocietyWorkScope));

                //राजस्थान राज्य क्रीड़ा परिषद
                formData.append("IsAffiliatedByRSSC", $scope.SocietyRegistration.IsAffiliatedByRSSC == 'Yes' ? true : false);
                formData.append("RSSCRegNo", $scope.SocietyRegistration.RSSCRegNo);
                formData.append("RSSCAffiliatedDate", $('#txtDateRSSC').val().split("/").reverse().join("-"));

                //जिला ओलंपिक परिषद//राज्य ओलंपिक परिषद
                formData.append("IsAffiliatedBySOC", $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' ? true : false);
                formData.append("SOCRegNo", $scope.SocietyRegistration.SOCRegNo);
                formData.append("SOCAffiliatedDate", $('#txtDateSOC').val().split("/").reverse().join("-"));

                //जिला स्तरीय क्रीड़ा परिषद्

                //by n saini
                if ($scope.SocietyRegistration.IsAffiliatedByDLSC != undefined && $scope.SocietyRegistration.IsStatLevel == 'No') {
                    formData.append("IsAffiliationBySLSA", $scope.SocietyRegistration.IsAffiliationBySLSA == 'Yes' ? true : false);
                    formData.append("SLSARegNo", $scope.SocietyRegistration.SLSARegNo);
                    formData.append("SLSAAffiliatedDate", $('#txtDateSLSA').val().split("/").reverse().join("-"));
                    formData.append("SLSAFileName", $scope.SLSAFileName);
                    formData.append("IsAffiliatedByDLSC", $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' ? true : false);
                    formData.append("DLSCAffiliatedDate", $('#txtDateDLSCDistrict').val().split("/").reverse().join("-"));
                    formData.append("DLSCRegNo", $scope.SocietyRegistration.DLSCRegNo);
                }
                else if ($scope.SocietyRegistration.IsAffiliatedBySLSC != undefined && $scope.SocietyRegistration.IsStatLevel == 'Yes') {
                    formData.append("IsAffiliationBySLSA", $scope.SocietyRegistration.IsAffiliatedBySLSC == 'Yes' ? true : false);
                    formData.append("SLSAAffiliatedDate", $('#txtDateSLSCState').val().split("/").reverse().join("-"));
                    formData.append("SLSARegNo", $scope.SocietyRegistration.DLSCRegNo);
                    formData.append("SLSAFileName", $scope.RSSCFileName);
                }
                else {
                    formData.append("IsAffiliatedByDLSC", $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes' ? true : false);
                    if ($('#txtDateDLSCState').val() != "" && $('#txtDateDLSCState').val() != undefined && $('#txtDateDLSCState').val() != "") {
                        formData.append("DLSCAffiliatedDate", $('#txtDateDLSCState').val().split("/").reverse().join("-"));
                    }
                }
                formData.append("RegistrationCertificateFile", $scope.RegistrationCertificateFile);
                formData.append("ByeLawsSocietyFile", $scope.ByeLawsSocietyFile);
                formData.append("MemberListExcelFile", $scope.MembersListExcelFile);
                formData.append("SaveStatus", $scope.SubmitStatus);
                //AFTER 25122018
                //राज्य स्तरीय खेल संघ                

            }
            $http.post(SiteUrl + "/Society/SaveSocietyProfiling", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $("#InnerModelPopup").addClass("alert alert-danger fade in");

                    if (data.data.indexOf("ErrorInMemberdata") > -1) {
                        ShowModelAlert("Error", "<b>Error In Uploaded Members Excel. All Column values is mandatory.Please fill correct values and re-upload. </b><br/><br/>" + data.data.split('@')[1]);
                        return;
                    } else if (data.data == '-5') {
                        if ($scope.SocietyRegistration.ActId != '7')
                            ShowModelAlert("Error", "A Society already exist with this Name or Registration or BRN");
                        else
                            ShowModelAlert("Error", "A Society already exist with this Name or Registration");

                    } else

                        if (data.data == '-1') {
                            ShowModelAlert("Error", "Some error occurred please try after some time ");
                        }
                        else if ((data.data.split('@')).length > 1) {
                            ShowModelAlert("Society Profiling", "Basic Details Update Sucessfully");
                            $scope.EMitraTransactionId = (data.data.split('@')).length > 1 ? (data.data.split('@'))[1] : '0';
                            //$scope.SocietyRegistration.SocietyIdEnc = data.data.split('@')[0];
                            $scope.SocietyIdEnc = data.data.split('@')[0];

                            if ($scope.IsEditMode == false) {
                                $scope.ActId = $scope.SocietyRegistration.ActId;
                                $scope.SocietyDirectory = data.societydirectory;
                                $scope.SocietyIdForAdministrativeDetails = data.data;
                                ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                                $scope.divSocietyProfilingShow = false;
                                $scope.divAddBtn = false;
                                $scope.lstSocietyWorkScope = [];
                                $("#SocietyStep_1").removeClass("active stepactive1");
                                $("#SocietyStep_1").addClass("disabled stepactive1");
                                $("#SocietyStep_2").addClass("active stepactive2");

                                $("#step1").removeClass("tab-pane active");
                                $("#step1").addClass("tab-pane ng-hide");
                                $("#step2").removeClass("tab-pane ng-hide");
                                $("#step2").addClass("tab-pane active");
                                $scope.IsFirst = 2;
                                $scope.IsNewRegistration = ($scope.SocietyRegistration.IsNewRegistration == 'false' ? false : true);
                                if ($scope.ActId == "1")
                                    $scope.IsNewRegistration = true;

                                if ($scope.ActId != "7") {
                                    $scope.DivOther = true;
                                    $scope.DivSport = false;
                                    if ($scope.ActId == "1") {
                                        $scope.SocietyRegistration.IsAdministrative = "Board"
                                        $scope.AdministrativeBoard();
                                        $scope.Act2001 = false;
                                        $scope.IsAdministrativeN = true;
                                    }
                                    else {
                                        $scope.Act2001 = true;
                                    }
                                }
                                else {
                                    $scope.DivOther = false;
                                    $scope.DivSport = true;
                                    $scope.GETSPCHAIRMAN = true;
                                    $scope.GETSPSECRETARY = true;
                                    $scope.GETTREASURER = true;
                                }
                            }
                            else {

                                if ($scope.ActId == "1") {
                                    $scope.SocietyRegistration.IsAdministrative = "Board";
                                    $scope.Act1958 = true;
                                    $scope.Act2001 = false;
                                }
                                else {
                                    $scope.Act2001 = true;
                                    $scope.Act1958 = false;
                                    $scope.SocietyRegistration.IsAdministrative = undefined;
                                }

                                $scope.ShowDetails($scope.SocietyRegistration.SocietyIdEnc, 'ShowDetails');
                                $scope.ShowDetailsWorkscope($scope.SocietyRegistration.SocietyIdEnc);
                                $scope.SocietyIdEnc = $scope.SocietyRegistration.SocietyIdEnc;
                                var $active = $('.wizard .nav-tabs li.active');
                                prevTab($active);
                                $active.next().removeClass('disabled');
                                var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                                $scope.divSocietyProfilingShow = true;
                                $scope.IsFirst = 2;
                                $scope.divSocialProfiling = false;
                                $scope.IsNewRegistration = ($scope.SocietyRegistration.IsNewRegistration == 'false' ? false : true);
                                if ($scope.ActId == "1")
                                    $scope.IsNewRegistration = true;

                                $("#SocietyStep_22").removeClass("active stepactive1");
                                $("#SocietyStep_22").addClass("disabled stepactive1");
                                $("#SocietyStep_11").removeClass("active stepactive3");

                                $("#step22").removeClass("tab-pane active");
                                $("#step22").addClass("tab-pane ng-hide");
                                $("#step11").removeClass("tab-pane ng-hide");
                                $("#step11").addClass("tab-pane active");

                                $("#SocietyStep_22").removeClass("disabled stepactive1");
                                $("#SocietyStep_22").addClass("active stepactive1");
                                $("#SocietyStep_11").addClass("disabled stepactive3");

                                $("#step22").removeClass("tab-pane ng-hide");
                                $("#step22").addClass("tab-pane active");
                                $("#step11").removeClass("tab-pane active");
                                $("#step11").addClass("tab-pane ng-hide");
                                $('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });

                            }
                            //$active.next().removeClass('disabled');
                            //var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                            //if ($active == "SocietyStep_22") {
                            //    $("#SocietyStep_22").removeClass("active");
                            //    $("#SocietyStep_11").addClass("disabled stepactive2");
                            //    $("#SocietyStep_11").addClass("active stepactive1");

                            //    $("#step22").removeClass("tab-pane active");
                            //    $("#step22").addClass("tab-pane ng-hide");
                            //    $("#step11").removeClass("tab-pane ng-hide");
                            //    $("#step11").addClass("tab-pane active");
                            //}
                            //if ($active == "SocietyStep_33") {

                            //    $("#SocietyStep_33").removeClass("active stepactive1");
                            //    $("#SocietyStep_33").addClass("disabled stepactive1");
                            //    $("#SocietyStep_22").addClass("active stepactive3");

                            //    $("#step33").removeClass("tab-pane active");
                            //    $("#step33").addClass("tab-pane ng-hide");
                            //    $("#step22").removeClass("tab-pane ng-hide");
                            //    $("#step22").addClass("tab-pane active");

                            //}
                            //$('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });
                        }
                        else if (data.data.length > 10) {
                            $scope.ActId = $scope.SocietyRegistration.ActId;
                            $scope.SocietyDirectory = data.societydirectory;
                            $scope.SocietyIdForAdministrativeDetails = data.data;
                            ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                            $scope.divSocietyProfilingShow = false;
                            $scope.divAddBtn = false;
                            $scope.lstSocietyWorkScope = [];
                            $("#SocietyStep_1").removeClass("active stepactive1");
                            $("#SocietyStep_1").addClass("disabled stepactive1");
                            $("#SocietyStep_2").addClass("active stepactive2");

                            $("#step1").removeClass("tab-pane active");
                            $("#step1").addClass("tab-pane ng-hide");
                            $("#step2").removeClass("tab-pane ng-hide");
                            $("#step2").addClass("tab-pane active");
                            $scope.IsFirst = 2;
                            $scope.IsNewRegistration = ($scope.SocietyRegistration.IsNewRegistration == 'false' ? false : true);
                            if ($scope.ActId == "1")
                                $scope.IsNewRegistration = true;

                            if ($scope.ActId != "7") {
                                $scope.DivOther = true;
                                $scope.DivSport = false;
                                if ($scope.ActId == "1") {
                                    $scope.SocietyRegistration.IsAdministrative = "Board"
                                    $scope.AdministrativeBoard();
                                    $scope.Act2001 = false;
                                    $scope.IsAdministrativeN = true;
                                }
                                else {
                                    $scope.Act2001 = true;
                                }
                            }
                            else {
                                $scope.DivOther = false;
                                $scope.DivSport = true;
                            }
                        }
                    $scope.ClearForm();


                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                    $('#btnAdministrativedetails').trigger('click');
                });


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
        $scope.RuralurbanDiv = false;
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
            $scope.IsFirst = 2;

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

            $scope.IsFirst = 3;


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
            $("#SocietyStep_2").removeClass("active stepactive1");
            $("#SocietyStep_2").addClass("disabled stepactive2");

            $("#SocietyStep_3").removeClass("active stepactive3");
            $("#SocietyStep_3").addClass("disabled stepactive3");

            $("#SocietyStep_1").removeClass("disabled stepactive1");
            $("#SocietyStep_1").addClass("active stepactive1");

            $("#step1").removeClass("tab-pane ng-hide");
            $("#step1").addClass("tab-pane active");
            $("#step2").removeClass("tab-pane active");
            $("#step2").addClass("tab-pane ng-hide");
            $("#step3").removeClass("tab-pane active");
            $("#step3").addClass("tab-pane ng-hide");
            $scope.IsFirst = 1;

        }
        else if (FilledProfileLevel == 2) {
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
            $("#SocietyStep_1").removeClass("active stepactive1");
            $("#SocietyStep_1").addClass("disabled stepactive1");
            $("#SocietyStep_2").addClass("active stepactive2");
            $("#step1").removeClass("tab-pane active");
            $("#step1").addClass("tab-pane ng-hide");
            $("#step2").removeClass("tab-pane ng-hide");
            $("#step2").addClass("tab-pane active");
            $scope.IsFirst = 2;

        }
        else if (FilledProfileLevel == 3) {

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
            $scope.IsFirst = 3;

        }
    }

    // START Show Society Details
    // START Show Society Details
    $scope.ApplicationView = function (SID) {
        if (SID != '') {
            window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + SID + "~_Citizen_SocietyProfile~undefined";
        }
    }


    $scope.ShowDetails = function (SID, IsFirst) {

        $scope.SocietyIdEnc = SID;
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = true;
        $(".blockUI").fadeIn("slow");
        //$http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
        $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyDetailsData = [];
                if (data.length > 0) {
                    var BasicDetails = data[0];
                    var AdministrativeDetails = [];
                    var OfficeDeatils = data;
                    var IsBRNYes = '';
                    angular.forEach(data, function (item) {
                        AdministrativeDetails.push({
                            "Name": item.Name, "ContactNo": item.ContactNo, "Type": item.UserType, "SocietyId": item.SocietyId, "DOB": item.ContactNo, "DOB": item.ContactNo, "Photo": item.Photo, "AADHAARNo": item.AadharNo
                        });
                    });
                    $scope.SocietyDetailsData.BasicDetails = [];
                    $scope.SocietyDetailsData.BasicDetails = BasicDetails;
                    $scope.SocietyDetailsData.OfficeDeatils = OfficeDeatils;
                    $scope.BasciDetailsIsPermanentSave = BasicDetails.BasciDetailsIsPermanentSave;
                    $scope.AdminDetailsIsPermanentSave = BasicDetails.AdminDetailsIsPermanentSave;
                    $scope.AddressDetailsIsPermanentSave = BasicDetails.AddressDetailsIsPermanentSave;

                    TableData10 = [];
                    var temp = 0;
                    if ($scope.SocietyDetailsData.BasicDetails.ActId != 7) {
                        if ($scope.SocietyDetailsData.BasicDetails.BRN.length > 0 && ($scope.SocietyDetailsData.BasicDetails.BRN != undefined || $scope.SocietyDetailsData.BasicDetails.BRN != "")) {
                            //$scope.divBrnYN = true;
                            $scope.IsBRNYes = 'true';
                        }
                        else {
                            // $scope.divBrnYN = false;
                            $scope.IsBRNYes = 'false';
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
                        $scope.SocietyDetailsData.BasicDetails.RegistrationDate = $scope.SocietyDetailsData.BasicDetails.RegistrationDate.indexOf('9999') > -1 ? '' : $scope.SocietyDetailsData.BasicDetails.RegistrationDate;
                        $scope.SocietyDetailsData.BasicDetails.ElectionDate = $scope.SocietyDetailsData.BasicDetails.ElectionDate.indexOf('9999') > -1 ? '' : $scope.SocietyDetailsData.BasicDetails.ElectionDate;
                        $scope.SocietyDetailsData.BasicDetails.AGMDate = $scope.SocietyDetailsData.BasicDetails.AGMDate.indexOf('9999') > -1 ? '' : $scope.SocietyDetailsData.BasicDetails.AGMDate;
                    }
                    $scope.ActId = $scope.SocietyDetailsData.BasicDetails.ActId;
                    var newdab = data.filter(function (x) { return x.IsSameCEOAddress == 'True' && x.AddressFor == 0 })

                    $scope.SocietyDetailsData.OfficeDeatils.dtab = newdab.length > 0 ? newdab[0] : 'N/A';

                    var newdab = data.filter(function (x) { return x.AddressFor == 9 && x.IsSameCEOAddress == 'False' })

                    $scope.SocietyDetailsData.OfficeDeatils.dtabnew = newdab.length > 0 ? newdab[0] : 'N/A';

                    var sachiv = data.filter(function (x) { return x.AddressFor == 11 })
                    $scope.SocietyDetailsData.OfficeDeatils.Newscetrat = sachiv.length > 0 ? sachiv[0] : 'N/A';

                    var Treasurer = data.filter(function (x) { return x.AddressFor == 12 })
                    $scope.SocietyDetailsData.OfficeDeatils.newTreasurer = Treasurer.length > 0 ? Treasurer[0] : 'N/A';


                    //var CEODetails = data.filter(x => x.UserType == 1);//CEO
                    //$scope.SocietyDetailsData.CEO = CEODetails.length > 0 ? CEODetails[0] : [];
                    //if ($scope.SocietyDetailsData.CEO.Name != 'undefined' || $scope.EMitraTransactionId > 0) {
                    //    $scope.BtnAdminDetailShowhide = true;
                    //}
                    //else {
                    //    $scope.BtnAdminDetailShowhide = false;
                    //}

                    //var ChairmanDetails = data.filter(x => x.UserType == 2);//Chairman
                    //$scope.SocietyDetailsData.Chairman = ChairmanDetails.length > 0 ? ChairmanDetails[0] : [];

                    var AdministratorDetails = data.filter(function (x) { return x.UserType == 3 });//Appointed Administrator
                    $scope.SocietyDetailsData.Administrator = AdministratorDetails.length > 0 ? AdministratorDetails[0] : [];

                    $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed = AdministratorDetails.length > 0 ? $scope.translation.Yes : $scope.translation.No;
                    $scope.IsAdministratorAppointed = AdministratorDetails.length > 0 ? true : false;
                    //$scope.BtnAdminDetailShowhide = AdministratorDetails.length > 0 ? true : false;

                    $scope.BtnAdminDetailShowhide = true;

                    MemberDetails = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') && (x.IsSameCEOAddress == 'True' || x.IsSameCEOAddress == true) });
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
                        NOMINATEDMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'NOMINATEDMEMBERS') && x.IsSameCEOAddress == 'True' });
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

                    //var MemberDetails = data.filter(x => (x.UserType == 4 || x.UserType == 5) && x.IsSameCEOAddress == 'False');//Elected /Nominete


                    $scope.SocietyDetailsData.EMD = data.filter(function (x) { return (x.MEMBERTYPE == 'ELECTEDMEMBERS') && x.IsSameCEOAddress == 'False' });
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


                    if (($scope.SocietyDetailsData.BasicDetails.IsNewRegistration == 'false' || $scope.SocietyDetailsData.BasicDetails.IsNewRegistration == 'False') && $scope.ActId == 3) {
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
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
            });

    }
    // END Show Society Details

    // END Show Society Details
    // START Show Workscope Details
    $scope.ShowDetailsWorkscope = function (SID, ValidationFlag) {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetWorkscopeBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.WorkscopeDetailsData = [];
                if (data.length > 0) {
                    $scope.lstSocietyWorkScope = data;

                    //alert(JSON.stringify($scope.lstSocietyWorkScope));
                    $scope.singlearray = data[0];

                    if ($scope.singlearray.IsRSSC == "True") {
                        $scope.TrueDivForPrimarybody = false;
                        $scope.hidefilediv = false;
                    }
                    else {
                        $scope.TrueDivForPrimarybody = true;
                        $scope.hidefilediv = true;
                    }
                    if ($scope.singlearray.IsRuralUrban == "0" || $scope.singlearray.IsRuralUrban == "False") {

                        $scope.IsStatelevelRural = true;
                        $scope.IsStatelevelRuralNO = false;
                        $scope.IsStatelevelF = false;
                        $scope.IsRuralUworkscope = true;
                        $scope.IsUrbanRworkscope = false;
                        $scope.RuralDivRadio = true;
                    }
                    else if ($scope.singlearray.IsRuralUrban == "1" || $scope.singlearray.IsRuralUrban == "True") {
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
                        $scope.RuralurbanDiv = true;
                        $scope.grid = false;


                        if ($scope.singlearray.IsRuralUrban == '1') {
                            $scope.SocietyRegistration.Rural = 'true';
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
                            $scope.SocietyRegistration.Rural = 'false';
                            $scope.RuralDivRadio = false;
                        }
                        $scope.grid = true;
                        $scope.InCaseofupdate = true;
                        //$scope.gridtwo = false;
                        // $scope.Addmorebtn();

                    }
                    else if ($scope.singlearray.IsStatLevel == '1') {
                        $scope.SocietyRegistration.IsStatLevel = 'Yes';
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
                    $scope.SocietyDetailsData["WorkscopeDetailsData"] = [];

                }

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
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
                        return;
                    }
                }
                var SocietyRegistrationData = [];
                var formData = new FormData();
                formData.append("ActId", $scope.SocietyRegistration.ActId);
                formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);
                formData.append("ClassA", $scope.SocietyRegistration.TotalMembersClassA);
                formData.append("ClassB", $scope.SocietyRegistration.TotalMembersClassB);
                formData.append("ClassC", $scope.SocietyRegistration.TotalMembersClassC);
                formData.append("SocietyDirectory", $scope.SocietyDirectory);
                formData.append("SubmitStatus", $scope.SubmitStatus);
                $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;


                //if ($scope.SocietyRegistration.IsAdministrative == 'Administrative' || $scope.SocietyRegistration.IsAdministrative == 'Board') {
                //    SocietyRegistrationData.push(
                //        {
                //            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                //            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                //            "Name": $scope.SocietyRegistration.CEOName,
                //            "IsActive": true,
                //            "ContactNo": $scope.SocietyRegistration.CEOContactNo,
                //            "AADHAARNo": $scope.SocietyRegistration.CEOAADHAARNo,
                //            "DOB": $('#txtCEODOB').val().split("/").reverse().join("-"),
                //            "Gender": $scope.SocietyRegistration.CEOGender,
                //            "UserType": "1",
                //            "AdministratorDesignationId": "-1",
                //            "CategoryId": $scope.SocietyRegistration.CEOCategoryId,
                //            "PostId": $scope.SocietyRegistration.CEOPostId,
                //            "Photo": $scope.SocietyRegistration.CEOSocietyFile,
                //            "MEMBERTYPE": "ELECTEDMEMBERS"
                //        });
                //}
                //if ($scope.SocietyRegistration.IsAdministrative == 'Administrative') {
                //    SocietyRegistrationData.push(
                //        {                            
                //            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                //            "Name": $scope.SocietyRegistration.ChairmanName,
                //            "IsActive": true,
                //            "ContactNo": $scope.SocietyRegistration.ChairmanContactNo,
                //            "AADHAARNo": $scope.SocietyRegistration.ChairmanAADHAARNo,
                //            "DOB": $('#txtChairmanDOB').val().split("/").reverse().join("-"),
                //            "Gender": $scope.SocietyRegistration.ChairmanGender,
                //            "UserType": "2",
                //            "AdministratorDesignationId": "-1",
                //            "CategoryId": $scope.SocietyRegistration.ChairmanCategoryId,
                //            "PostId": $scope.SocietyRegistration.ChairmanPostId,
                //            "Photo": $scope.SocietyRegistration.ChairmanFile,
                //            "MEMBERTYPE": "ADMINISTRATIVE"
                //        });
                //}

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
                    //var LiqPh = $scope.SocietyRegistration.liquidatorFile.split('/');
                    //if (LiqPh.length > 3) {
                    //    $scope.SocietyRegistration.liquidatorFile = LiqPh[3];
                    //}
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
                        if ($(this).find('td:eq(8)').find('input').val() != '' && $(this).find('td:eq(7)').find('select').val() > 0 && $(this).find('td:eq(1)').find('input').val() != '') {

                            if (($scope.SocietyRegistration.IsNewRegistration == 'false' || $scope.SocietyRegistration.IsNewRegistration == 'False') && $scope.Act2001) {
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
                                        "Photo": ($(this).find('td:eq(9)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(10)').find('input').val(),
                                        "MembershipNo": $(this).find('td:eq(11)').find('input').val(),
                                        "ShareAllotted": $(this).find('td:eq(12)').find('input').val(),
                                        "ShareCapitalAmount": $(this).find('td:eq(13)').find('input').val(),
                                        "loaneeType": $(this).find('td:eq(14)').find('select').val(),
                                        "MEMBERTYPE": ($(this).find('td:eq(15)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                            else {
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
                                        "Photo": ($(this).find('td:eq(9)').find('img'))[0].src,
                                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                        "IsActive": true,
                                        "UserType": "4",//Elected board Menbers
                                        "AdministratorDesignationId": "-1",
                                        "Occupation": $(this).find('td:eq(10)').find('input').val(),
                                        //"MembershipNo": $(this).find('td:eq(11)').find('input').val(),
                                        //"ShareAllotted": $(this).find('td:eq(12)').find('input').val(),
                                        //"ShareCapitalAmount": $(this).find('td:eq(13)').find('input').val(),
                                        //"loaneeType": $(this).find('td:eq(14)').find('select').val(),

                                        "MembershipNo": '',
                                        "ShareAllotted": 0,
                                        "ShareCapitalAmount": 0,
                                        "loaneeType": 0,
                                        "MEMBERTYPE": ($(this).find('td:eq(15)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS'),
                                        "SubmitStatus": $scope.SubmitStatus
                                    });
                            }
                        }
                        //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
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
                $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetails", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
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
                                    $scope.IsFirst = 3;
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
                                    $scope.IsFirst = 3;
                                    var $active = $('.wizard .nav-tabs li.active');
                                    prevTab($active);
                                    $active.next().removeClass('disabled');
                                    var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                                    $scope.divSocialProfiling = false;
                                    //$("#SocietyStep_33").removeClass("disabled stepactive1");
                                    //$("#SocietyStep_33").addClass("active stepactive1");
                                    //$("#SocietyStep_22").removeClass("disabled stepactive3");


                                    //$("#step33").removeClass("tab-pane ng-hide");
                                    //$("#step33").addClass("tab-pane active");
                                    //$("#step22").removeClass("tab-pane active");
                                    //$("#step22").addClass("tab-pane ng-hide");


                                    $("#SocietyStep_22").removeClass("active stepactive1");
                                    $("#SocietyStep_22").addClass("disabled stepactive1");
                                    $("#SocietyStep_33").addClass("active stepactive3");

                                    $("#step22").removeClass("tab-pane active");
                                    $("#step22").addClass("tab-pane ng-hide");
                                    $("#step33").removeClass("tab-pane ng-hide");
                                    $("#step33").addClass("tab-pane active");


                                    $('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });

                                    //$scope.SocietyDirectory = data.societydirectory;
                                    //$scope.SocietyIdForAdministrativeDetails = data.data;
                                    //ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                                    //$scope.divSocietyProfilingShow = false;
                                    //$scope.divAddBtn = false;
                                    //$scope.lstSocietyWorkScope = [];

                                    //$("#SocietyStep_1").addClass("disabled stepactive1");
                                    //$("#SocietyStep_2").removeClass("active stepactive1");
                                    //$("#SocietyStep_2").addClass("disabled stepactive1");
                                    //$("#SocietyStep_3").addClass("active stepactive2");


                                    //$("#step1").addClass("tab-pane ng-hide");
                                    //$("#step2").removeClass("tab-pane active");
                                    //$("#step2").addClass("tab-pane ng-hide");
                                    //$("#step3").addClass("tab-pane active");
                                    //$scope.IsFirst = 3;
                                    //if ($scope.ActId != "7") {
                                    //    $scope.DivOther = true;
                                    //    $scope.DivSport = false;
                                    //}
                                    //else {
                                    //    $scope.DivOther = false;
                                    //    $scope.DivSport = true;
                                    //}
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
                                    $scope.IsFirst = 3;
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
                formData.append("ActId", $scope.SocietyRegistration.ActId);
                formData.append("CEOSocietyFile", $scope.CEOSocietyFile);
                formData.append("ChairmanFile", $scope.ChairmanFile);
                formData.append("AppointedFile", $scope.AppointedFile);
                formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);

                //var _data = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });

                //var Count = 20;
                //var tempRowNo = 1;
                //for (var Y = 0; Y < ImageDataList.length; Y++) {
                //    for (; tempRowNo < ImageDataList[Y].RowNo; tempRowNo++) {
                //        formData.append("TempOtherAdministrativeFile" + (tempRowNo + Count), ImageDataList[Y].File);
                //    }
                //    formData.append("OtherAdministrativeFile" + (ImageDataList[Y].RowNo + Count), ImageDataList[Y].File);
                //    tempRowNo++;
                //}

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
                                "MEMBERTYPE": ($(this).find('td:eq(11)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS')
                            });
                    }
                });
                $('#SportTableAdd tr').each(function () {
                    SocietyRegistrationData.push(
                        {

                            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                            "Name": $(this).find('td:eq(1)').find('input').val(),
                            "IsActive": true,
                            "ContactNo": $(this).find('td:eq(2)').find('input').val(),
                            "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                            "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                            "Gender": ($(this).find('td:eq(5)').find('label').html()).charAt(0),//text
                            "CategoryId": $(this).find('td:eq(6)').find('select').val() > 0 ? $(this).find('td:eq(6)').find('select').val() : 0,
                            "PostId": $(this).find('td:eq(7)').find('select').val() > 0 ? $(this).find('td:eq(7)').find('select').val() : 0,
                            "Address": '',
                            "UserType": "5",//Nominated member
                            "AdministratorDesignationId": "-1",
                            "Photo": ($(this).find('td:eq(8)').find('img'))[0].src,

                        });
                    //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
                });



                formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
                $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetails", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
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
                            $scope.IsFirst = 3;
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

                            $scope.IsFirst = 3;
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
                            $scope.IsFirst = 3;
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
                            $scope.IsFirst = 3;
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
        $('#Iddstateyes')[0].checked = false;
        $('#Iddstateno')[0].checked = false;
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
        if ($scope.SocietyRegistration.IsNationalLevel == 'Yes') {
            $scope.grid = false;
            $scope.lstSocietyWorkScope = [];
            $scope.lstSocietyWorkScope.push(
                {
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
        else if (($scope.SocietyRegistration.CityTypes > 0 || $scope.SocietyRegistration.DIVISION_ID > 0 || $scope.SocietyRegistration.IsStatLevel) && ValidationFlag) {

            if ($scope.lstSocietyWorkScope.length != 0 && (ValidationFlag == undefined || ValidationFlag == true)) {
                if (parseInt(($scope.lstSocietyWorkScope[0]).WorkScopeLevel_ID, 10) != (parseInt($scope.SocietyRegistration.WorkScopeLevel_ID, 10))) {
                    alert('data is not at same level hence can not be added.');
                    return;
                }
                if ($scope.SocietyRegistration.DIVISION_ID != 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function (x) {
                        return x.WorkScopeLevel_ID == parseInt($('#static').val())
                            && x.DIVISION_ID == parseInt($('#ddlDivision').val())
                            && x.DISTRICT_ID == parseInt($('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val())
                            && x.BLOCK_ID == parseInt($('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val())
                            && x.GP_ID == parseInt($('#ddlGP').val() == null ? 0 : $('#ddlGP').val())
                            && x.VILLAGE_ID == parseInt($('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val())
                        //&& x.WorkScopeLevel_ID == $('#static option:selected').val()
                    });
                }
                else if ($scope.SocietyRegistration.CityTypes != 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function (x) {
                        return
                        x.CityTypes == $('#staticall').val()
                            && x.CITY_ID == $('#ddlCity').val()
                            && x.WARD_ID == $('#ddlward').val()
                    }
                    );
                }
                if (_data.length > 0) {
                    alert(_language == 'en-us' ? 'record already exist\n' : 'रिकॉर्ड पहले से मौजूद है \n');
                    return;
                }

            }

            if (($scope.SocietyRegistration.IsStatLevel == 'No' || $scope.SocietyRegistration.CityTypes > '0' || $scope.SocietyRegistration.DIVISION_ID > '0') && ($scope.SocietyRegistration.WorkScopeLevel_ID != '0' || $scope.SocietyRegistration.CityTypes != '0')) {
                $scope.grid = true;
                //IsSportsRural
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeLevel_ID": parseInt($('#static option:selected').val()),
                        "STATE_ID": 6,
                        "IsStatLevel": 0,
                        "DIVISION_ID": $('#ddlDivision').val() == null ? 0 : $('#ddlDivision').val(),
                        "DISTRICT_ID": $('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val(),
                        "BLOCK_ID": $('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val(),
                        "VILLAGE_ID": $('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val(),
                        "CITY_ID": $('#ISrurl').prop('checked') ? 0 : $('#ddlCity').val(),
                        "WARD_ID": $('#ISrurl').prop('checked') ? 0 : $('#ddlward').val(),
                        "GP_ID": $('#ddlGP').val() == null ? 0 : $('#ddlGP').val(),
                        "CityTypes": $('#ISrurl').prop('checked') ? 0 : $('#staticall').val(),
                        "IsRuralUrban": $('#ISrurl').prop('checked') == true ? 1 : 0,


                        "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                        "STATE_Name": 'Rajasthan' ? 'राजस्थान' : 'N/A',
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
            var _data = $scope.lstSocietyWorkScope.filter(function (x) { return x.BLOCK_ID == $('#ddlblockitwo option:selected').val() }

                //&& x.GP_ID == $('#ddlGP option:selected').val()
                //&& x.VILLAGE_ID == $('#ddlGP option:selected').val()
                //&& x.WorkScopeLevel_ID == $('#static option:selected').val()
            );
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
                    "WorkScopeLevelID": parseInt($('#static option:selected').val()),
                    "STATE_ID": 6,
                    "IsStatLevel": $scope.SocietyRegistration.IsStatLevel == 'Yes' ? 1 : 0,
                    "DISTRICT_ID": parseInt($('#ddlDistricttwo option:selected').val()),
                    "AssociationId": parseInt($('#ddlgame option:selected').val().split('#')[0]),

                    "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                    "STATE_Name": 'Rajasthan' ? 'राजस्थान' : 'N/A',
                    "DISTRICT_ENG": parseInt($('#ddlDistricttwo option:selected').val()) > 0 ? $('#ddlDistrict option:selected').text() : 'N/A',
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

            if ($scope.SocietyRegistration.IsSportsNewRegistration == undefined) {
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
            if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && ($scope.SocietyRegistration.IsSportsNewRegistration == false || $scope.SocietyRegistration.IsSportsNewRegistration == 'false')) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
            }
            if ($('#txtSportRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId == "7" && ($scope.SocietyRegistration.IsSportsNewRegistration == false || $scope.SocietyRegistration.IsSportsNewRegistration == 'false')) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter  Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख दर्ज करे |';

            }
        }
        else {

            if ($scope.SocietyRegistration.ActId == undefined || $scope.SocietyRegistration.ActId == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Act' : '\n <br /> कृपया अधिनियम का चयन करे |';
            }
            if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
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
            if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
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
        //alert($scope.SocietyRegistration.IsAffiliatedBySLSC);       S 
        //alert($scope.SocietyRegistration.IsAffiliationBySLSA);       D
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
                if (($scope.DLSCFileName == undefined || $scope.DLSCFileName == '') && $scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To The District Sports Council(PDF).' : '\n <br />कृपया जिला खेल परिषद से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ)|';
                }
                if (($scope.SOCFileName == undefined || $scope.SOCFileName == '') && $scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To The District Olympic Council(PDF).' : '\n <br />कृपया जिला ओलंपिक परिषद से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ) |';
                }
                if (($scope.SLSAFileName == undefined || $scope.SLSAFileName == '') && $scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.IsAffiliationBySLSA == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificates Of Affiliation To The State Level Sports Association(PDF).' : '\n <br />कृपया राज्य स्तरीय खेल संघ से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ)|';
                }
                //DISTRICT END
                //STATE
                if (($scope.RSSCFileName == undefined || $scope.RSSCFileName == '') && $scope.SocietyRegistration.IsStatLevel == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySLSC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To The State Level Sports Council(PDF).' : '\n <br /> कृपया राज्य स्तरीय खेल परिषद से संबद्धता का प्रमाणपत्र अपलोड करें (पीडीएफ)|';
                }
                if (($scope.SOCFileName == undefined || $scope.SOCFileName == '') && $scope.SocietyRegistration.IsStatLevel == 'Yes' && $scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload The Certificate Of Affiliation To State Olympic Council(PDF).' : '\n <br /> कृपया राज्य ओलंपिक परिषद से संबद्धता का प्रमाण पत्र अपलोड करें (पीडीएफ)|';
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
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of District Olympic Council Affiliation' : '\n <br /> कृपया जिला ओलंपिक परिषद से संबद्धता की स्थिति चुने|';
                    }
                    else if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                        if ($scope.SocietyRegistration.SOCRegNo == undefined || $scope.SocietyRegistration.SOCRegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(District Olympic Council)' : '\n <br /> कृपया संबद्धता क्रमांक(जिला ओलंपिक परिषद) दर्ज करें |';
                        }
                        if ($('#txtDateSOC').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(District Olympic Council)' : '\n <br /> कृपया संबद्धता तिथि(जिला ओलंपिक परिषद) दर्ज करें |';
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
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of State Olympic Council Affiliation' : '\n <br /> कृपया राज्य ओलंपिक परिषद से संबद्धता की स्थिति चुने|';

                    }
                    else if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes') {
                        if ($scope.SocietyRegistration.SOCRegNo == undefined || $scope.SocietyRegistration.SOCRegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(State Olympic Council)' : '\n <br /> कृपया संबद्धता क्रमांक(राज्य ओलंपिक परिषद) दर्ज करें |';
                        }
                        if ($('#txtDateSOC').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(State Olympic Council)' : '\n <br /> कृपया संबद्धता तिथि(राज्य ओलंपिक परिषद) दर्ज करें |';
                        }
                    }

                    if ($scope.SocietyRegistration.IsAffiliatedBySLSC == undefined) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Status of District Level Sports Council Affiliation' : '\n <br /> कृपया जिला स्तरीय क्रीड़ा परिषद् से संबद्धता की स्थिति चुने|';
                    }
                    else if ($scope.SocietyRegistration.IsAffiliatedBySLSC == 'Yes') {
                        if ($scope.SocietyRegistration.DLSCRegNo == undefined || $scope.SocietyRegistration.DLSCRegNo == '') {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Number(District Level Sports Council)' : '\n <br /> कृपया संबद्धता क्रमांक(जिला स्तरीय क्रीड़ा परिषद्) दर्ज करें |';
                        }
                        if ($('#txtDateSLSCState').val().length == 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Affiliation Date(District Level Sports Council)' : '\n <br /> कृपया संबद्धता तिथि(जिला स्तरीय क्रीड़ा परिषद्) दर्ज करें |';
                        }
                    }
                }
                if ($scope.SocietyRegistration.SocietyName == undefined || $scope.SocietyRegistration.SocietyName == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Sports Association Name' : '\n <br /> कृपया क्रीड़ा संगम का नाम दर्ज करें |';
                }
                if ($('#txtSportElectionDate').val().length == 0 && ($scope.SocietyRegistration.IsSportsNewRegistration == false || $scope.SocietyRegistration.IsSportsNewRegistration == 'false')) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Election Date' : '\n <br /> कृपया अंतिम चुनाव तिथि का चयन करे |';
                }
                if ($('#txtSportAgmDate').val().length == 0 && ($scope.SocietyRegistration.IsSportsNewRegistration == false || $scope.SocietyRegistration.IsSportsNewRegistration == 'false')) {
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
        if ($scope.SocietyRegistration.IsNewRegistration == undefined && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }

        if ($scope.SocietyRegistration.IsSportsNewRegistration == undefined && $scope.SocietyRegistration.ActId == "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }

        if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';

        }
        if ($scope.SocietyRegistration.SocietyName == undefined && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Name' : '\n <br /> कृपया सोसाइटी का नाम दर्ज करें |';

        }

        if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख का चयन करे |';

        }
        if ($('#txtElectionDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
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


        if ($scope.SocietyRegistration.IsNationalLevel == undefined && $scope.SocietyRegistration.IsStatLevel == undefined && $scope.SocietyRegistration.ActId == "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Choose Area of operations ?' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें?';

        }
        if ($scope.SocietyRegistration.IsStatLevel == undefined && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Choose Area of operations ?' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें?';

        }

        if ($scope.SocietyRegistration.Rural == 'false' && $scope.lstSocietyWorkScope.length < 1) {

            if ($scope.SocietyRegistration.CityTypes == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the City Type.' : '\n <br /> कृपया शहर का प्रकार चुनें?';

            }
            if ($scope.SocietyRegistration.CITY_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the City .' : '\n <br /> कृपया शहर का चयन करें |';

            }
            if ($scope.SocietyRegistration.WARD_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the ward .' : '\n <br /> कृपया वार्ड का चयन करें |';

            }
        }



        if ($scope.SocietyRegistration.Rural == 'true' && $scope.lstSocietyWorkScope.length < 1) {
            if ($scope.SocietyRegistration.WorkScopeLevel_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Choose Level of work scope.' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें |';

            }
        }


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

        if (($scope.SocietyRegistration.SingleShareValue == undefined || $scope.SocietyRegistration.SingleShareValue == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Entrance Fees Collected' : '\n <br /> कृपया आवेदन करने वाले सदस्यों की प्रवेश शुल्क दर्ज करें|';
        }

        if (($scope.SocietyRegistration.PaidUpShareCapital == undefined || $scope.SocietyRegistration.PaidUpShareCapital == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Paid Up Share Capital' : '\n <br /> कृपया शेयर पूंजी का भुगतान दर्ज करें|';
        }
        // && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')
        if ($('#txtAGMDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select AGM Date' : '\n <br /> कृपया एजीएम की तारीख का चयन करे |';
        }

        if (($scope.SocietyRegistration.NoOfMemberInAGM == undefined || $scope.SocietyRegistration.NoOfMemberInAGM == '') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Entrance Fees Collected' : '\n <br /> कृपया आवेदन करने वाले सदस्यों की प्रवेश शुल्क दर्ज करें|';
        }

        if ($scope.SocietyRegistration.ActId != "7") {
            if (($scope.RegistrationCertificateFile == undefined || $scope.RegistrationCertificateFile == '') && ($scope.SocietyRegistration.IsNewRegistration == 'false' || $scope.SocietyRegistration.IsSportsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Registration Certificate(PDF)' : '\n <br /> कृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';
            }
        }
        else if ($scope.SocietyRegistration.ActId == "7") {
            if (($scope.RegistrationCertificateFile == undefined || $scope.RegistrationCertificateFile == '') && ($scope.SocietyRegistration.IsSportsNewRegistration == 'false' || $scope.SocietyRegistration.IsSportsNewRegistration == false)) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Registration Certificate(PDF)' : '\n <br /> कृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';
            }
        }


        if (($scope.RulesAndRegulationFile == undefined || $scope.RulesAndRegulationFile == undefined) && $scope.SocietyRegistration.ActId == "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Rules And Regulation(PDF)' : '\n <br /> कृपया नियम और अधिनियम अपलोड करें (पीडीएफ) |';
        }

        if (($scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') && $scope.SocietyRegistration.ActId != "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Bye Laws(PDF)' : '\n <br /> कृपया बाई लॉज़ अपलोड करें(पीडीएफ) |';
        }
        //&& $scope.SocietyRegistration.ActId != "7"
        if (($scope.MembersListExcelFile == undefined || $scope.MembersListExcelFile == '')) {   //by n saini
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Members List(Excel)' : '\n <br /> कृपया सदस्य सूची(एक्सेल) अपलोड करें |';
        }


        if (($scope.WorkPlanEconomicViabilityFile == undefined || $scope.WorkPlanEconomicViabilityFile == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Work Plan Showing Economic Viability of the Society(PDF)' : '\n <br /> कृपया आर्थिक कार्य योजना अपलोड करें (पीडीएफ)|';
        }

        if (($scope.GeneralMeetingProcedureFile == undefined || $scope.GeneralMeetingProcedureFile == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload General Meeting Procedure File(PDF)' : '\n <br /> कृपया अपलोड जनरल मीटिंग प्रोसीजर फाइल अपलोड करें(पीडीऍफ़) |';
        }

        if (($scope.BalanceSheetExcelFile == undefined || $scope.BalanceSheetExcelFile == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Balance Sheet(Excel)' : '\n <br /> कृपया बैलेंस शीट (एक्सेल) अपलोड करें |';
        }

        if (($scope.ObjectsFileName == undefined || $scope.ObjectsFileName == '') && $scope.SocietyRegistration.ActId == "1") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Objects(PDF)' : '\n <br /> कृपया उद्देश्य अपलोड करें (पीडीएफ) |';
            message += '<br />';
        }




        if (message.length > 29 && $scope.SubmitStatus == 1) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }
        if ($scope.SubmitStatus == 0) {
            1780


            return false;
        }
        return true;
    }
    //END

    $scope.SocietyOfficeFormValidationPartial = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
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
        if ($scope.SocietyRegisteredOffice.IsRural == 'true' || $scope.SocietyRegisteredOffice.IsRural == 'True') {
            if ($scope.SocietyRegisteredOffice.CityTypes == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office CityType ' : '\n <br /> कृपया कार्यालय का शहर का प्रकार चुनें |';

            }
            if ($scope.SocietyRegisteredOffice.CITY_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office City ' : '\n <br /> कृपया कार्यालय का शहर का चयन करें |';

            }
            if ($scope.SocietyRegisteredOffice.WARD_ID == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select office Ward' : '\n <br /> कृपया कार्यालय का वार्ड का चयन करें |';

            }
        }
        if ($scope.SocietyRegisteredOffice.IsRural == 'false' || $scope.SocietyRegisteredOffice.IsRural == 'False') {

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
        if ($scope.SocietyRegisteredOffice.PinCode == 0 || $scope.SocietyRegisteredOffice.PinCode == undefined || $scope.SocietyRegisteredOffice.PinCode == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter office PinCode' : '\n <br /> कृपया पिनकोड दर्ज करें  |';
        }
        if ($scope.SocietyRegisteredOffice.herebyAccept == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please aceept terms and condition.' : '\n <br /> कृपया नियम और शर्त स्वीकार करें।|';
        }
        if (message.length > 29 && $scope.SocietyRegisteredOffice.SubmitStatus == 0) {
            var _Temp = (_language == 'en-us' ? 'Society Office Details' : 'सोसायटी कार्यालय विवरण');
            ShowModelAlert(_Temp, message);
            return false;
        }
        return true;
    }


    $scope.SocietyOfficeFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.SocietyRegistration.ActId == "7") {
            if ($scope.SocietyRegisteredOffice.HouseBuildingNoSecretary == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNoSecretary.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Secretary House / Building No.' : '\n <br /> सोसायटी सचिव का हाउस / बिल्डिंग नंबर दर्ज करें|';

            }
            if ($scope.SocietyRegisteredOffice.LaneStreetNameSecretary == undefined || $scope.SocietyRegisteredOffice.LaneStreetNameSecretary.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Secretary Lane/Street Name' : '\n <br /> कृपया सचिव का लेन / सड़क का नाम दर्ज करें|';

            }
            if ($scope.SocietyRegisteredOffice.LocalitySecretary == undefined || $scope.SocietyRegisteredOffice.LocalitySecretary.length < 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Locality' : '\n <br /> कृपया सचिव का लोकैलिटी का चयन करें|';

            }
            if ($scope.SocietyRegisteredOffice.STATE_IDSecretary == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary State.' : '\n <br /> कृपया सचिव का राज्य का चयन करें |';

            }
            if ($scope.SocietyRegisteredOffice.IsRuralSecretary == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please choose Secretary Is Rural or urban  ?' : '\n <br /> कृपया सचिव का  ग्रामीण या शहरी चुनें ?';

            }


            if ($scope.SocietyRegisteredOffice.IsRuralSecretary == 'true' || $scope.SocietyRegisteredOffice.IsRuralSecretary == 'True') {
                if ($scope.SocietyRegisteredOffice.CityTypesSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary CityType ' : '\n <br /> कृपया सचिव का  शहर का प्रकार चुनें |';

                }
                if ($scope.SocietyRegisteredOffice.CITY_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary City ' : '\n <br /> कृपया सचिव का  शहर का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.WARD_IDSecretary == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Ward' : '\n <br /> कृपया सचिव का  वार्ड का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.IsRuralSecretary == 'false' || $scope.SocietyRegisteredOffice.IsRuralSecretary == 'False') {
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
            if ($scope.SocietyRegisteredOffice.IsRuralTreasurer == 'true' || $scope.SocietyRegisteredOffice.IsRuralTreasurer == 'True') {
                if ($scope.SocietyRegisteredOffice.CityTypesTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer CityType ' : '\n <br /> कृपया कोषाध्यक्ष का शहर का प्रकार चुनें |';

                }
                if ($scope.SocietyRegisteredOffice.CITY_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer City ' : '\n <br /> कृपया कोषाध्यक्ष का शहर का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.WARD_IDTreasurer == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Treasurer Ward' : '\n <br /> कृपया कोषाध्यक्ष का वार्ड का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.IsRuralTreasurer == 'false' || $scope.SocietyRegisteredOffice.IsRuralTreasurer == 'False') {

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
            if ($scope.SocietyRegisteredOffice.IsRural == 'true' || $scope.SocietyRegisteredOffice.IsRural == 'True') {
                if ($scope.SocietyRegisteredOffice.CityTypes == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office CityType ' : '\n <br /> कृपया कार्यालय का शहर का प्रकार चुनें |';

                }
                if ($scope.SocietyRegisteredOffice.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office City ' : '\n <br /> कृपया कार्यालय का शहर का चयन करें |';

                }
                if ($scope.SocietyRegisteredOffice.WARD_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select office Ward' : '\n <br /> कृपया कार्यालय का वार्ड का चयन करें |';

                }
            }
            if ($scope.SocietyRegisteredOffice.IsRural == 'false' || $scope.SocietyRegisteredOffice.IsRural == 'False') {

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
                if ($scope.SocietyRegisteredOffice.IsRuralcopy == 'true' || $scope.SocietyRegisteredOffice.IsRuralcopy == 'True') {
                    if ($scope.SocietyRegisteredOffice.CITY_IDcopy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman City ' : '\n <br /> कृपया अध्यक्ष का शहर का चयन करें |';

                    }
                    if ($scope.SocietyRegisteredOffice.WARD_IDcopy == 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Ward ' : '\n <br /> कृपया अध्यक्ष का वार्ड का चयन करें |';

                    }
                }
                if ($scope.SocietyRegisteredOffice.IsRuralcopy == 'false' || $scope.SocietyRegisteredOffice.IsRuralcopy == 'False') {

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

        if (message.length > 29 && $scope.SocietyRegisteredOffice.SubmitStatus == 1) {
            var _Temp = (_language == 'en-us' ? 'Society Office Details' : 'सोसायटी कार्यालय विवरण');
            ShowModelAlert(_Temp, message);
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends

    $scope.showHideDiv = function () {

        if ($('#EUserType').val() == 'K' && $scope.SocietyRegistration.ActId == '7') {
            if ($('#EServiceid').val() == '4209')//PROFILE
            {
                $scope.SocietyRegistration.IsNewRegistration = 'false';
                $scope.Profiling();
                $("input[name='RegistrationProfile']")[0].checked = false;
                $("input[name='RegistrationProfile']")[1].checked = true;
            }
            else if ($('#EServiceid').val() == '4172')//REG
            {
                $scope.SocietyRegistration.IsNewRegistration = 'true';
                $("input[name='RegistrationProfile']")[0].checked = true;
                $("input[name='RegistrationProfile']")[1].checked = false;
                $scope.Profiling();
            }
            $('input[name="RegistrationProfile"]').attr('disabled', 'disabled');

            //$scope.divBrnYN = false;
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
        else {
            //$scope.divBrnYN = false;

        }

        //$("#txtBrn").removeAttr("disabled");
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
                $('#Iddstateyes')[0].checked = false;
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
                $('#Iddstateyes')[0].checked = false;
                $('#Iddstateno')[0].checked = false;
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
    $scope.RuralUrbanBlock = function (V) {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural;
        if (V != 1) {
            $scope.SocietyRegisteredOffice.CityTypes = "0";
            $scope.SocietyRegisteredOffice.CITY_ID = "0";
            $scope.SocietyRegisteredOffice.WARD_ID = "0";
            $scope.SocietyRegisteredOffice.DIVISION_ID = "0";
            $scope.SocietyRegisteredOffice.DISTRICT_ID = "0";
            $scope.SocietyRegisteredOffice.BLOCK_ID = "0";
            $scope.SocietyRegisteredOffice.GP_ID = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_ID = "0";
        }
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
        $scope.SocietyRegisteredOffice.CityTypes_copy = "0";
        $scope.SocietyRegisteredOffice.CITY_IDcopy = "0";
        $scope.SocietyRegisteredOffice.WARD_IDcopy = "0";
        $scope.SocietyRegisteredOffice.DIVISION_ID_copy = "0";
        $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = "0";
        $scope.SocietyRegisteredOffice.BLOCK_ID_copy = "0";
        $scope.SocietyRegisteredOffice.GP_IDcopy = "0";
        $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = "0";

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
        $scope.SocietyRegisteredOffice.CityTypes_copy = "0";
        $scope.SocietyRegisteredOffice.CITY_IDcopy = "0";
        $scope.SocietyRegisteredOffice.WARD_IDcopy = "0";
        $scope.SocietyRegisteredOffice.DIVISION_ID_copy = "0";
        $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = "0";
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

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralSecretary;
        if (IsEdit != '1') {
            $scope.SocietyRegisteredOffice.CityTypesSecretary = "0";
            $scope.SocietyRegisteredOffice.CITY_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.WARD_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.DIVISION_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.BLOCK_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.GP_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_IDSecretary = "0";
        }
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
    $scope.IsTreasurer = function (IsEdit) {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralTreasurer;
        if (IsEdit != '1') {
            $scope.SocietyRegisteredOffice.CityTypesTreasurer = "0";
            $scope.SocietyRegisteredOffice.CITY_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.WARD_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.DIVISION_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.GP_IDTreasurer = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_IDTreasurer = "0";
        }

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
    $scope.Levelofficedata = function (Cflag) {
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
            $scope.lstSocietyWorkScope = [];
            $scope.Addmore = false;
        }
        else {
            if ($scope.IsEditMode == true) {
            }
            else {
                if (Cflag == 1)
                    $scope.SocietyRegistration.IsStatLevel = undefined;

                $('#Iddstateyes').removeAttr("disabled");
                $('#Iddstateno').removeAttr("disabled");
                //$('#Iddstateyes')[0].checked = false;
                //$('#Iddstateno')[0].checked = false;
                $scope.lstSocietyWorkScope = [];
            }
            $scope.Addmore = true;
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
            $('#Iddstateyes')[0].checked = true;
            $scope.Addmorebtn(true);
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
        //$scope.SocietyRegistration.ClassId = '0';
        //$scope.SocietyRegistration.SubClassId = '0';
        //$scope.SocietyRegistration.SocietyCategoryId = '0';

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

        if (item == '1' || item == '0') {
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
    //Edit Form Basic Deatils
    $scope.EditBasicDetailsForm = function (SID, IsFirst, id, ValidationFlag) {
        
        $scope.CheckBRN = true;
        $scope.nepster = true;
        $scope.showtabbar = true;
        $scope.txtBrndisabled = true;

        $scope.txtRegistrationNo = true;
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
        $scope.divBrnbtn = false;
        $scope.all = true;
        $scope.IsEditMode = true;
        $scope.SocietyRegisteredOffice.SocietyIdEnc = SID;
        $scope.SocietyIdEnc = SID;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyRegistration = [];
                if (data.length > 0) {
                    $scope.SocietyDirectory = data[0].SocietyDirectory;
                    var BasicDetails = data[0];
                    var New = data;
                    //basicdetails                   

                    if (id == 1) {
                        $scope.GetSportsAssociationsList();
                        $scope.grid = true;
                        $scope.SocietyRegistration = BasicDetails;
                        alert($scope.SocietyRegistration.RegistrationNo);

                        var CEODetails = data.filter(function (x) { return x.UserType == 1 });
                        $scope.SocietyRegistration.CEO = CEODetails.length > 0 ? CEODetails[0] : [];
                        $scope.SocietyRegistration.BasicDetails = [];
                        $scope.SocietyRegistration.BasicDetails = BasicDetails;
                        $scope.SocietyRegistration = BasicDetails;
                        $scope.SocietyDetailsData = BasicDetails;
                        $scope.SocietyRegistration.ActId = BasicDetails.ActId;
                        $scope.ActId = BasicDetails.ActId;
                        var BRN = BasicDetails.BRN;
                        var ClassId = BasicDetails.ClassId;
                        var SubClassId = BasicDetails.SubClassId;
                        var SocietyCategoryId = BasicDetails.SocietyCategoryId;
                        $scope.showHideDiv();
                        $scope.SocietyRegistration.BRN = BRN;
                        $scope.divBrnbtn = false;
                        if ($scope.SocietyDetailsData.BRN.length > 0 && ($scope.SocietyDetailsData.BRN != undefined || $scope.SocietyDetailsData.BRN != "")) {
                            //$scope.divBrnYN = true;
                            $scope.SocietyDetailsData.IsBRNYes = 'true';
                            $scope.divBrn = true;
                        }
                        else {
                            //$scope.divBrnYN = false;
                            $scope.SocietyDetailsData.IsBRNYes = 'false';
                            $scope.divBrn = false;
                        }

                        $scope.GetSocietyTypeList();
                        $scope.GetSocietyClass();
                        $scope.SocietyRegistration.ClassId = BasicDetails.ClassId;

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

                        //alert(BasicDetails.AGMDate);
                        $scope.SocietyRegistration.IsNewRegistration = BasicDetails.IsNewRegistration == "False" ? 'false' : 'true';
                        $scope.IsNewRegistration = BasicDetails.IsNewRegistration;
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
                                $scope.RuralurbanDiv = false;
                                $scope.DistrictDiv = true;
                                $scope.District = false;
                                $scope.Districtt = true;
                                $scope.RuralForSportSociety = false;
                                $scope.PrimaryBodyDetails = true;
                                $scope.PrimaryBodyName = false;
                                $scope.GetDistrictWithoutDivisionList();
                                $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the District Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् से अवगत कराये|');
                                $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the District Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली जिला क्रीड़ा परिषद् से अवगत कराये|');
                                $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of State Olympic Council Affiliation ?' : 'राज्य ओलंपिक परिषद से संबद्धता की स्थिति चुने?');
                                $scope.AffiliatedByRSSC = true;
                                $scope.divSportSocietyName = true;
                                $scope.AffiliationByDLSC = false;
                                $scope.FileDLSC = false;
                                $scope.FileRSSC = true;
                                $scope.FileDOC = false;
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
                                $scope.LevelLitle = ($scope._language == 'en-us' ? 'Provide the Primary Sporting Bodies Representing This Game.' : 'इस खेल का प्रतिनिधित्व करने वाली प्राथमिक क्रीड़ा निकायों से अवगत कराये|');
                                $scope.OlympicCouncilType = ($scope._language == 'en-us' ? 'Status of District Olympic Council Affiliation ?' : 'जिला ओलंपिक परिषद से संबद्धता की स्थिति चुने?');
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
                                $scope.SocietyRegistration.ClickYes = 'true';
                            else
                                $scope.SocietyRegistration.ClickYes = 'false';


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
                                    $scope.DLSCFileName = BasicDetails.DLSCFileName;
                                    $scope.SOCFileName = BasicDetails.SOCFileName;
                                    $scope.SLSAFileName = BasicDetails.SLSAFileName;
                                    $scope.RSSCFileName = BasicDetails.RSSCFileName;

                                    $scope.showrulesfile = false;
                                    $scope.grid = false;
                                    $scope.ShowSubmitBtn = true;

                                    $scope.SocietyRegistration.IsSportsNewRegistration = BasicDetails.IsNewRegistration;
                                    // by n saini
                                    if (($scope.SocietyRegistration.IsSportsNewRegistration == 'true' || $scope.SocietyRegistration.IsSportsNewRegistration == true)) {
                                        $scope.IsProfiling = false;
                                    }

                                }).
                                error(function (data, status) {
                                    $scope.SportAssociationList = [];
                                });
                        }

                        $scope.SocietyRegistration.RegistrationDate = $scope.SocietyRegistration.RegistrationDate.indexOf('9999') > -1 ? '' : $scope.SocietyRegistration.RegistrationDate;
                        $scope.SocietyRegistration.ElectionDate = $scope.SocietyRegistration.ElectionDate.indexOf('9999') > -1 ? '' : $scope.SocietyRegistration.ElectionDate;
                        $scope.SocietyRegistration.AGMDate = $scope.SocietyRegistration.AGMDate.indexOf('9999') > -1 ? '' : $scope.SocietyRegistration.AGMDate;
                        $('#txtAGMDate').val(BasicDetails.AGMDate.indexOf('9999') > -1 ? '' : BasicDetails.AGMDate);
                        if (($scope.SocietyRegistration.IsNewRegistration == 'true' || $scope.SocietyRegistration.IsNewRegistration == true)) {
                            $scope.IsProfiling = false;
                        }

                    }
                    //Admin
                    else if (id == 2) {
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
                            $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed = $scope.translation.Yes;
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
                        }
                        else {
                            $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed = $scope.translation.No;
                            $scope.IsAdministrativeN = true;
                            $scope.IsAdministrativeY = false;
                            Isliquidator = false;
                            $scope.SocietyRegistration.IsAdministrative = 'Board';
                        }





                        if (liqDetails.length > 0) {
                            $scope.SocietyRegistration.IsAdministrative = 'liquidator';
                            Isliquidator = true;
                            $scope.IsAdministrativeN = false;
                            $scope.IsAdministrativeY = false;
                            $scope.SocietyRegistration.liquidatorName = liqDetails[0].Name;
                            $('#txtliquidatorAppointedDate').val(liqDetails[0].AppointedDate);
                            $scope.SocietyRegistration.liquidatorPostName = liqDetails[0].POSTNAME;
                            $scope.SocietyRegistration.liquidatorContactNo = liqDetails[0].ContactNo;
                        }
                        ////Elected /Nomineted

                        var ALLMEMBERS = [];
                        var NOMominatedMember = [];

                        if (ActId != 7) {
                            ALLMEMBERS = data.filter(function (x) { return (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                            if (ALLMEMBERS.length == 0) {
                                ALLMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                            }
                        }
                        else if (ActId == 7) {
                            ALLMEMBERS = data.filter(function (x) { return (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                            if (ALLMEMBERS.length == 0) {
                                ALLMEMBERS = data.filter(function (x) { return (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS') });
                            }
                        }


                        if (ActId == 3) {
                            $scope.Act2001 = true;
                            Nomi_ALLMEMBERS = data.filter(function (x) { return x.IsSameCEOAddress == 'True' && x.MEMBERTYPE == 'NOMINATEDMEMBERS' });
                            $scope.IsNewRegistration = false;
                            var Administrative = data.filter(function (x) { return x.IsSameCEOAddress == 'True' && x.BoardType == 'Administrative' });//Appointed Administrator           

                            if (Administrative.length > 0) {
                                $scope.translation.NominatedMember = 'Appointed Member';
                            }
                            else {
                                $scope.translation.NominatedMember = 'Nominated Member';
                            }
                        }

                        if (ALLMEMBERS.length > 0) {
                            $scope.SocietyRegistration.IsAdministrative = 'Board';

                            if ((ALLMEMBERS[0].IsNewRegistration == 'false' || ALLMEMBERS[0].IsNewRegistration == 'False') && ALLMEMBERS[0].ActId == 3) {
                                $scope.TwoThousandOneProfilling = true;
                                $scope.SocietyRegistration.IsNewRegistration = 'False';
                            }
                            else {
                                $scope.TwoThousandOneProfilling = false;
                                $scope.SocietyRegistration.IsNewRegistration = ALLMEMBERS[0].IsNewRegistration;
                            }
                        }


                        var TableData10NomiAll = [];
                        var TableData10All = [];

                        var X = 7;
                        if (ActId != 7) {

                            var TableData10 = [];
                            if (ActId == 1)
                                X = (ALLMEMBERS.length > 6 ? ALLMEMBERS.length : 7)
                            else {
                                var Administrative = data.filter(function (x) { return x.IsSameCEOAddress == 'True' && x.BoardType == 'Administrative' });
                                var liquidator = data.filter(function (x) { return x.IsSameCEOAddress == 'True' && x.MEMBERTYPE == 'LIQUIDATOR' }); //Appointed LIQUIDATOR 
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
                                TableData10.push(
                                    {
                                        "Id": x + 1,
                                        "Name": ALLMEMBERS[x].Name,
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
                                        "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                        "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                                        "MembershipNo": ALLMEMBERS[x].MembershipNo,
                                        "ShareAllotted": ALLMEMBERS[x].ShareAllotted,
                                        "ShareCapitalAmount": ALLMEMBERS[x].ShareCapitalAmount,
                                        "loaneeType": ALLMEMBERS[x].loaneeType//($scope._language == 'en-us' ? ALLMEMBERS[x].loaneeTypeEn : ALLMEMBERS[x].loaneeTypeHi)
                                    });
                            }

                            for (var x = ALLMEMBERS.length; x < X; x++) {
                                TableData10.push(
                                    {
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
                                        "Status": 'false',
                                        "MEMBERTYPE": '',
                                        "Address": '',
                                        "CategoryId": "0"
                                    });
                            }
                            $scope.TableData10 = TableData10;
                        }
                        else if (ActId == 7) {

                            X = (ALLMEMBERS.length > 4 ? ALLMEMBERS.length : 5)
                            for (var x = 0; x < ALLMEMBERS.length; x++) {
                                TableData10All.push(
                                    {
                                        "Id": (1000) + x + 1,
                                        "Name": ALLMEMBERS[x].Name,
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
                                        "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                        "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                                        "Address": ALLMEMBERS[x].Address
                                    });
                            }

                            for (var x = ALLMEMBERS.length; x < X; x++) {
                                TableData10All.push(
                                    {
                                        "Id": (1000) + x + 1,
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
                                        "Status": 'false',
                                        "MEMBERTYPE": '',
                                        "Address": '',
                                        "CategoryId": "0"
                                    });
                            }
                            $scope.TableData10All = TableData10All;

                            $scope.TableSportsData2 = TableData10All;

                        }
                        else {
                            X = (ALLMEMBERS.length > 14 ? ALLMEMBERS.length : 15)
                            for (var x = 0; x < ALLMEMBERS.length; x++) {
                                TableData10All.push(
                                    {
                                        "Id": x + 1,
                                        "Name": ALLMEMBERS[x].Name,
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
                                        "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                        "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                                        "Address": ALLMEMBERS[x].Address

                                    });
                            }

                            for (var x = ALLMEMBERS.length; x < X; x++) {
                                TableData10All.push(
                                    {
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
                                        "Status": 'false',
                                        "MEMBERTYPE": '',
                                        "FatherName": '',
                                        "Address": '',
                                        "CategoryId": "0"
                                    });
                            }
                            $scope.TableData10All = TableData10All;
                        }

                        $("#TblElectedMember").find("input,textarea,select").attr("disabled", "disabled");

                        //Nominated Member
                        if (ActId == 3) {
                            for (var x = 0; x < Nomi_ALLMEMBERS.length; x++) {
                                TableData10NomiAll.push(
                                    {
                                        "Id": x + 500,
                                        "Name": Nomi_ALLMEMBERS[x].Name,
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
                                        "Status": Nomi_ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                        "MEMBERTYPE": Nomi_ALLMEMBERS[x].MEMBERTYPE,
                                        "MembershipNo": Nomi_ALLMEMBERS[x].MembershipNo,
                                        "ShareAllotted": Nomi_ALLMEMBERS[x].ShareAllotted,
                                        "ShareCapitalAmount": Nomi_ALLMEMBERS[x].ShareCapitalAmount,
                                        "loaneeType": ($scope._language == 'en-us' ? Nomi_ALLMEMBERS[x].loaneeTypeEn : Nomi_ALLMEMBERS[x].loaneeTypeHi),
                                        "Address": ALLMEMBERS[x].Address
                                    });
                            }
                            for (var x = Nomi_ALLMEMBERS.length; x < 1; x++) {
                                TableData10NomiAll.push(
                                    {
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
                                        "Status": 'false',
                                        "MEMBERTYPE": '',
                                        "Address": '',
                                        "CategoryId": "0"
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
                                $scope.AppointedYN = $scope.AdministratorDetails.length > 0 ? true : false;
                                $scope.SocietyRegistration.IsAdministratorAppointed = $scope.AdministratorDetails.length > 0 ? true : false;
                                $scope.IsAdministrativeY = true;
                                $scope.IsAdministrativeN = false;
                                $scope.Isliquidator = false;
                            }
                            else if (liqDetails.length > 0) {
                                $scope.SocietyRegistration.IsAdministrative = 'liquidator';
                                $scope.Isliquidator = true;
                                $scope.AppointedYN = false;
                                $scope.IsAdministrativeY = false;
                                $scope.IsAdministrativeN = false;

                            }
                            else if (BasicDetails.ActId > 1 && MemberDetails.length == 0 && liqDetails.length == 0 && (AdministratorDetails.length > 0 || AdministratorDetails.length == 0)) {
                                $scope.SocietyRegistration.IsAdministrative = 'Administrative';

                                $scope.AppointedYN = AdministratorDetails.length > 0 ? true : false;
                                $scope.IsAdministrativeY = true;
                                $scope.IsAdministrativeN = false;
                                $scope.CEO = true;
                                if (AdministratorDetails.length == 0) {
                                    $('input:radio[name=AdministratorAppointed]')[1].checked = true;
                                    $('#txtAppointedAADHAARNo').removeAttr("disabled", "disabled");
                                    $scope.GETAPPOINTED = false;
                                    $scope.APPOINTEDRemove = true;
                                    $scope.AdministratorProfile = true;
                                }
                                else {
                                    $('input:radio[name=AdministratorAppointed]')[0].checked = true;
                                    $('#txtAppointedAADHAARNo').attr("disabled", "disabled");
                                }

                                $scope.AppointedShowHide();
                            }
                            else if (BasicDetails.ActId == 1 && MemberDetails.length == 0 && liqDetails.length == 0 && (AdministratorDetails.length > 0 || AdministratorDetails.length == 0)) {
                                if (BasicDetails.ActId == 1) {
                                    $scope.SocietyRegistration.IsAdministrative = 'Board';
                                    $scope.AdministrativeBoard();
                                }
                            }


                            if (liqDetails.length > 0) {
                                $scope.GETlIQUIDATOR = false;
                                $scope.IQUIDATORRemove = true;
                                $scope.Isliquidator = true;
                            }
                            else {
                                $scope.GETCEO = false;
                                $scope.CEORemove = true;
                                $scope.CEOProfile = true;
                                //ChairmanProfile

                                $scope.GETCHAIRMAN = false;
                                $scope.CHAIRMANRemove = true;
                                $scope.ChairmanProfile = true;
                            }

                            //$scope.AdministrativeBoard();

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

                            $scope.SocietyRegistration.IsAdministratorAppointed = 'true';
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
                            $scope.SocietyRegistration.IsAdministratorAppointed = 'false';
                            $scope.AppointedShowHide();
                        }
                        //END



                        if ($scope.SocietyRegistration.ActId != 7) {


                        }
                        $scope.IsAdministrativeN = true;
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

                        }
                        //END  
                    }
                    //  Address details    //Umesh Work on Sport show

                    else if (id == 3) {
                        if (BasicDetails.RegistrationNo == null || BasicDetails.RegistrationNo == "" || BasicDetails.RegistrationNo == 'undefined') {
                            $scope.IsAddressEditAble = true;
                        }
                        else if (BasicDetails.RegistrationNo != null && BasicDetails.RegistrationNo != 'undefined' && BasicDetails.RegistrationNo != "" && BasicDetails.RegistrationNo.length > 3) {
                            if (BasicDetails.IsNewRegistration == 0) {
                                $scope.IsAddressEditAble = true;
                            }
                            else if (BasicDetails.IsNewRegistration == 1) {
                                {
                                    $scope.IsAddressEditAble = false;
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

                        $scope.SocietyRegisteredOffice = BasicDetailsOffice[0];
                        //OFFICE                        
                        if (BasicDetailsOffice.length > 0) {
                            if ($scope.SocietyRegisteredOffice.IsRural == 'False' || $scope.SocietyRegisteredOffice.IsRural == 'false') {
                                $scope.RuralUrbanBlock(1);
                                $scope.SocietyRegisteredOffice.DIVISION_ID = BasicDetailsOffice[0].DIVISION_ID;
                                $scope.GetDistrictList(1);
                                $scope.SocietyRegisteredOffice.DISTRICT_ID = BasicDetailsOffice[0].DISTRICT_ID;
                                $scope.BlockListByOffAdd(1);
                                $scope.SocietyRegisteredOffice.BLOCK_ID = BasicDetailsOffice[0].BLOCK_ID;
                                $scope.GramPanchayatList(1);
                                $scope.SocietyRegisteredOffice.GP_ID = BasicDetailsOffice[0].GP_ID;
                                $scope.VillageList(1);
                                $scope.SocietyRegisteredOffice.VILLAGE_ID = BasicDetailsOffice[0].VILLAGE_ID;
                                $scope.SocietyRegisteredOffice.STATE_ID = BasicDetailsOffice[0].STATE_ID;
                            }
                            else {
                                $scope.RuralUrbanBlock(1);
                                $scope.SocietyRegisteredOffice.CityTypes = BasicDetailsOffice[0].CityTypes;
                                $scope.GetCityListByCityTypes(1);
                                $scope.SocietyRegisteredOffice.CITY_ID = BasicDetailsOffice[0].CITY_ID;
                                $scope.GetWardList(1);
                                $scope.SocietyRegisteredOffice.WARD_ID = BasicDetailsOffice[0].WARD_ID;
                            }
                        }
                        //OFFICE END

                        //CEO START
                        if (BasicDetailsCEO.length > 0) {
                            if (BasicDetailsCEO[0].IsRural == 'False' || BasicDetailsCEO[0].IsRural == 'false') {
                                $scope.SocietyRegisteredOffice.IsRuralcopy = BasicDetailsCEO[0].IsRural;
                                $scope.SocietyRegisteredOffice.CEONotCopy = BasicDetailsCEO.length > 0 ? BasicDetailsCEO[0] : 'N/A';
                                $scope.SocietyRegisteredOffice.HouseBuildingNocopy = $scope.SocietyRegisteredOffice.CEONotCopy.HouseBuildingNo;
                                $scope.SocietyRegisteredOffice.LaneStreetNamecopy = $scope.SocietyRegisteredOffice.CEONotCopy.LaneStreetName;
                                $scope.SocietyRegisteredOffice.Localitycopy = $scope.SocietyRegisteredOffice.CEONotCopy.Locality;

                                $scope.RuralUrbanBlockCopy(2);
                                $scope.SocietyRegisteredOffice.DIVISION_ID_copy = $scope.SocietyRegisteredOffice.CEONotCopy.DIVISION_ID;
                                $scope.GetDistrictList(2);
                                $scope.SocietyRegisteredOffice.DISTRICT_ID_copy = $scope.SocietyRegisteredOffice.CEONotCopy.DISTRICT_ID;
                                $scope.BlockListByOffAdd(2);
                                $scope.SocietyRegisteredOffice.BLOCK_ID_copy = $scope.SocietyRegisteredOffice.CEONotCopy.BLOCK_ID;
                                $scope.GramPanchayatList(2);
                                $scope.SocietyRegisteredOffice.GP_IDcopy = $scope.SocietyRegisteredOffice.CEONotCopy.GP_ID;
                                $scope.VillageList(2);
                                $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = $scope.SocietyRegisteredOffice.CEONotCopy.VILLAGE_ID;
                                $scope.SocietyRegisteredOffice.PinCodecopy = $scope.SocietyRegisteredOffice.CEONotCopy.PinCode;
                                $scope.SocietyRegisteredOffice.STATE_IDcopy = BasicDetailsCEO[0].STATE_ID;
                            }
                            else {
                                $scope.SocietyRegisteredOffice.IsRuralcopy = BasicDetailsCEO[0].IsRural;
                                $scope.SocietyRegisteredOffice.HouseBuildingNocopy = BasicDetailsCEO[0].HouseBuildingNo;
                                $scope.SocietyRegisteredOffice.LaneStreetNamecopy = BasicDetailsCEO[0].LaneStreetName;
                                $scope.SocietyRegisteredOffice.Localitycopy = BasicDetailsCEO[0].Locality;
                                $scope.SocietyRegisteredOffice.CEONotCopy = BasicDetailsCEO.length > 0 ? BasicDetailsCEO[0] : 'N/A';
                                $scope.RuralUrbanBlockCopy(2);
                                $scope.SocietyRegisteredOffice.CityTypes_copy = $scope.SocietyRegisteredOffice.CEONotCopy.CityTypes;
                                $scope.GetCityListByCityTypes(2);
                                $scope.SocietyRegisteredOffice.CITY_IDcopy = $scope.SocietyRegisteredOffice.CEONotCopy.CITY_ID;
                                $scope.GetWardList(2);
                                $scope.SocietyRegisteredOffice.WARD_IDcopy = $scope.SocietyRegisteredOffice.CEONotCopy.WARD_ID;
                                $scope.SocietyRegisteredOffice.PinCodecopy = $scope.SocietyRegisteredOffice.CEONotCopy.PinCode;
                                $scope.SocietyRegisteredOffice.STATE_IDcopy = BasicDetailsCEO[0].STATE_ID;
                            }
                        }
                        //CEO END


                        $scope.ActId = $scope.SocietyRegisteredOffice.ActId;
                        $scope.SocietyRegisteredOffice.SocietyIdEnc = BasicDetailsOffice[0].SocietyIdEnc;



                        //Umesh Work on Sport show
                        if ($scope.ActId == 7) {
                            //For Secatery                            
                            $scope.RuralUrbanBlockForSportcopy();
                            var Secterydata = New.filter(function (x) { return x.AddressFor == 11 })
                            $scope.SocietyRegisteredOffice.Secretary = Secterydata.length > 0 ? Secterydata[0] : 'N/A';
                            //$scope.SocietyRegisteredOffice.CityTypes = CityTypeId;
                            //$scope.SocietyRegisteredOffice.CITY_ID = CITY_ID;
                            //$scope.SocietyRegisteredOffice.WARD_ID = WARD_ID;
                            //$scope.SocietyRegisteredOffice.CityTypes_copy = CityTypes_copy;
                            //$scope.SocietyRegisteredOffice.CITY_IDcopy = CITY_IDcopy;
                            //$scope.SocietyRegisteredOffice.WARD_IDcopy = WARD_IDcopy;

                            $scope.SocietyRegisteredOffice.HouseBuildingNoSecretary = $scope.SocietyRegisteredOffice.Secretary.HouseBuildingNo;
                            $scope.SocietyRegisteredOffice.LaneStreetNameSecretary = $scope.SocietyRegisteredOffice.Secretary.LaneStreetName;
                            $scope.SocietyRegisteredOffice.LocalitySecretary = $scope.SocietyRegisteredOffice.Secretary.Locality;
                            $scope.SocietyRegisteredOffice.STATE_IDSecretary = $scope.SocietyRegisteredOffice.STATE_ID;
                            $scope.SocietyRegisteredOffice.IsRuralSecretary = $scope.SocietyRegisteredOffice.Secretary.IsRural;
                            $scope.RuralUrbanBlockForSportcopy(1);
                            $scope.SocietyRegisteredOffice.PinCodeSecretary = $scope.SocietyRegisteredOffice.Secretary.PinCode;
                            $scope.SocietyRegisteredOffice.CityTypesSecretary = $scope.SocietyRegisteredOffice.Secretary.CityTypes;
                            $scope.SocietyRegisteredOffice.CITY_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.CITY_ID;
                            $scope.GetWardList(3);
                            $scope.SocietyRegisteredOffice.WARD_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.WARD_ID;
                            $scope.SocietyRegisteredOffice.DIVISION_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.DIVISION_ID;
                            $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.DISTRICT_ID;
                            $scope.SocietyRegisteredOffice.BLOCK_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.BLOCK_ID;
                            $scope.SocietyRegisteredOffice.GP_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.GP_ID;
                            $scope.SocietyRegisteredOffice.VILLAGE_IDSecretary = $scope.SocietyRegisteredOffice.Secretary.VILLAGE_ID;
                            $scope.GetCityListByCityTypes(3);
                            $scope.GetDistrictList(3);
                            //$scope.DivisionList();
                            $scope.BlockListByOffAdd(3);
                            $scope.GramPanchayatList(3);
                            $scope.VillageList(3);
                            //For trasurer data
                            $scope.IsTreasurer();
                            var Trasererdata = New.filter(function (x) { return x.AddressFor == 12 })
                            $scope.SocietyRegisteredOffice.Traserer = Trasererdata.length > 0 ? Trasererdata[0] : 'N/A';
                            $scope.SocietyRegisteredOffice.HouseBuildingNoTreasurer = $scope.SocietyRegisteredOffice.Traserer.HouseBuildingNo;
                            $scope.SocietyRegisteredOffice.LaneStreetNameTreasurer = $scope.SocietyRegisteredOffice.Traserer.LaneStreetName;
                            $scope.SocietyRegisteredOffice.LocalityTreasurer = $scope.SocietyRegisteredOffice.Traserer.Locality;
                            $scope.SocietyRegisteredOffice.STATE_IDTreasurer = $scope.SocietyRegisteredOffice.STATE_ID;
                            $scope.SocietyRegisteredOffice.IsRuralTreasurer = $scope.SocietyRegisteredOffice.Traserer.IsRural;
                            $scope.IsTreasurer(1);
                            $scope.SocietyRegisteredOffice.PinCodeTreasurer = $scope.SocietyRegisteredOffice.Traserer.PinCode;
                            $scope.SocietyRegisteredOffice.CityTypesTreasurer = $scope.SocietyRegisteredOffice.Traserer.CityTypes;
                            $scope.SocietyRegisteredOffice.CITY_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.CITY_ID;
                            $scope.GetWardList(4);
                            $scope.SocietyRegisteredOffice.WARD_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.WARD_ID;
                            $scope.SocietyRegisteredOffice.DIVISION_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.DIVISION_ID;
                            $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.DISTRICT_ID;
                            $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.BLOCK_ID;
                            $scope.SocietyRegisteredOffice.GP_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.GP_ID;
                            $scope.SocietyRegisteredOffice.VILLAGE_IDTreasurer = $scope.SocietyRegisteredOffice.Traserer.VILLAGE_ID;
                            $scope.GetCityListByCityTypes(4);
                            $scope.GetDistrictList(4);
                            //$scope.DivisionList();
                            $scope.BlockListByOffAdd(4);
                            $scope.GramPanchayatList(4);
                            $scope.VillageList(4);
                        }
                        $scope.GetCityListByCityTypes(1);
                        $scope.GetCityListByCityTypes(2);
                        $scope.GetCityListByCityTypes(3);
                        $scope.SocietyRegisteredOffice.STATE_ID = '6';
                        $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
                        $scope.SocietyRegisteredOffice.STATE_IDSecretary = '6';
                        $scope.SocietyRegisteredOffice.STATE_IDTreasurer = '6';
                    }

                    var AdministrativeDetails = [];
                    angular.forEach(data, function (item) {
                        AdministrativeDetails.push({
                            "Name": item.Name, "ContactNo": item.ContactNo, "Type": item.UserType, "SocietyId": item.SocietyId, "DOB": item.ContactNo, "DOB": item.ContactNo, "Photo": item.Photo, "AADHAARNo": item.AadharNo
                        });
                    });

                    //$scope.SocietyDetailsData.BasicDetails = [];

                }
                else {
                    $scope.SocietyDetailsData["BasicDetails"] = [];
                    $scope.SocietyDetailsData["AdministrativeDetails"] = [];
                }

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");

            });

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
        $scope.IsFirst = 3;
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
        var validate = true;
        var Count = 0;

        var message = $scope._language == 'en-us' ? ' Validate  following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.SocietyRegistration.ActId != "7") {
            if ($scope.SocietyRegistration.IsAdministrative == undefined && $scope.SocietyRegistration.ActId != "1") {

                message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrative Type' : '\n <br /> प्रशासनिक प्रकार का चयन करे |';
            }

            //liquidator
            if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
                if ($scope.SocietyRegistration.liquidatorName == undefined || $scope.SocietyRegistration.liquidatorName == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Name' : '\n <br /> परिसमापक का नाम दर्ज करें |';

                }

                //if ($('#txtliquidatorDOB').val() == '') {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Birth' : '\n <br /> कृपया परिसमापक की जन्म तिथि दर्ज करे |';

                //}

                if ($('#txtliquidatorAppointedDate').val() == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Appointment' : '\n <br /> कृपया परिसमापक की नियुक्ति तिथि दर्ज करे |';

                }

                //if ($scope.SocietyRegistration.liquidatorCategoryId == "0" || $scope.SocietyRegistration.liquidatorCategoryId == undefined) {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of liquidator.' : '\n <br /> कृपया परिसमापक के वर्ग का चयन करें |';

                //}

                //if ($scope.SocietyRegistration.liquidatorGender == undefined) {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Select liquidator Gender Type.' : '\n <br /> कृपया परिसमापक के लिंग का चयन करे |';

                //}
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
                $('#TblElectedMember tr').each(function () {
                    if (($(this).find('td:eq(1)').find('input').val() != undefined && $(this).find('td:eq(1)').find('input').val() != '' || Count == 0) && SubmitStatus == 0) {
                        if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(2)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(2)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //FATHER NAME
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
                        //PHONE
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
                        //DATE
                        if ($(this).find('td:eq(5)').find('input').val() == undefined || $(this).find('td:eq(5)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(5)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(5)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //GENDER
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
                        if ($(this).find('td:eq(7)').find('select').val().indexOf('und') > -1 || $(this).find('td:eq(7)').find('select').val() == null || $(this).find('td:eq(7)').find('select').val() == "0") {
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
                        if ($(this).find('td:eq(8)').find('input').val() == '' || $(this).find('td:eq(8)').find('input').val() == undefined) {
                            var jElement = $(this).find('td:eq(8)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(8)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //IMAGE
                        if ($(this).find('td:eq(9)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                            var jElement = $(this).find('td:eq(9)').find('img');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(9)').find('img');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }

                        //Occupation
                        if ($(this).find('td:eq(10)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(10)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(10)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }

                        if ($scope.SocietyRegistration.IsNewRegistration == 'false' && $scope.Act2001) {
                            //MembershipNo
                            if ($(this).find('td:eq(11)').find('input').val() == '' || $(this).find('td:eq(11)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(11)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(11)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            //ShareAllotted
                            if ($(this).find('td:eq(12)').find('input').val() == '' || $(this).find('td:eq(12)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(12)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(12)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            //ShareCapitalAmount
                            if ($(this).find('td:eq(13)').find('input').val() == '' || $(this).find('td:eq(13)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(13)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(13)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            //loanee/non loanee
                            if ($(this).find('td:eq(14)').find('select').val() == null || $(this).find('td:eq(14)').find('select').val() == "0") {
                                var jElement = $(this).find('td:eq(14)').find('select');
                                jElement.css('border', 'solid 1px red');
                                $(this).find('td:eq(14)').find('select').removeAttr("disabled");
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(14)').find('select');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            if ($(this).find('td:eq(15)').find('input').prop('checked') == true) {
                                Count = Count + 1;
                            }
                        }
                        else {
                            if ($(this).find('td:eq(10)').find('input').prop('checked') == true) {
                                Count = Count + 1;
                            }
                        }
                        Count = Count + 1;
                    }
                    else if (SubmitStatus == 1) {

                        //AADHAR
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
                        //FATHER NAME
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
                        //PHONE
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
                        //DATE
                        if ($(this).find('td:eq(5)').find('input').val() == undefined || $(this).find('td:eq(5)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(5)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(5)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //GENDER
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
                        if ($(this).find('td:eq(7)').find('select').val().indexOf('und') > -1 || $(this).find('td:eq(7)').find('select').val() == null || $(this).find('td:eq(7)').find('select').val() == "0") {
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
                        if ($(this).find('td:eq(8)').find('input').val() == '' || $(this).find('td:eq(8)').find('input').val() == undefined) {
                            var jElement = $(this).find('td:eq(8)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(8)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //IMAGE
                        if ($(this).find('td:eq(9)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                            var jElement = $(this).find('td:eq(9)').find('img');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(9)').find('img');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }

                        //Occupation
                        if ($(this).find('td:eq(10)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(10)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(10)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }

                        if ($scope.SocietyRegistration.IsNewRegistration == 'false' && $scope.Act2001) {
                            //MembershipNo
                            if ($(this).find('td:eq(11)').find('input').val() == '' || $(this).find('td:eq(11)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(11)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(11)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            //ShareAllotted
                            if ($(this).find('td:eq(12)').find('input').val() == '' || $(this).find('td:eq(12)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(12)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(12)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            //ShareCapitalAmount
                            if ($(this).find('td:eq(13)').find('input').val() == '' || $(this).find('td:eq(13)').find('input').val() == undefined) {
                                var jElement = $(this).find('td:eq(13)').find('input');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(13)').find('input');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            //loanee/non loanee
                            if ($(this).find('td:eq(14)').find('select').val() == null || $(this).find('td:eq(14)').find('select').val() == "0") {
                                var jElement = $(this).find('td:eq(14)').find('select');
                                jElement.css('border', 'solid 1px red');
                                $(this).find('td:eq(14)').find('select').removeAttr("disabled");
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(14)').find('select');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }

                            if ($(this).find('td:eq(15)').find('input').prop('checked') == true) {
                                Count = Count + 1;
                            }
                        }
                        else {
                            if ($(this).find('td:eq(10)').find('input').prop('checked') == true) {
                                Count = Count + 1;
                            }
                        }
                    }
                });


                //validate = $scope.ValidateNominatedMember(0, true);
                //validate = $scope.ValidateElectedMember(3, true);

                if ($scope.SocietyRegistration.ActId == "3" && Count < 12 && $scope.SocietyRegistration.IsAdministrative == 'Board' && SubmitStatus == 1) {
                    var r = confirm($scope._language == 'en-us' ? 'Minimum 12 officers should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 12 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    if (r == false) {
                        return false;
                    }
                }
                else if ($scope.SocietyRegistration.ActId == "1" && Count < 7 && SubmitStatus == 1) {
                    var r = confirm($scope._language == 'en-us' ? 'Minimum 7 officers should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 7 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
                    if (r == false) {
                        return false;
                    }
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

            if ($scope.SocietyRegistration.IsAdministrative == 'Board' && $scope.SocietyRegistration.ActId == "3" && ($scope.IsNewRegistration == false || $scope.IsNewRegistration == 'false') && SubmitStatus == 1) {
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

        if (message.length > 24 || validate == false) {
            if (message.length > 24) {
                ShowModelAlert("Administrative Form Validation", message);
            }
            else {
                ShowModelAlert("Administrative Form Validation", $scope._language == 'en-us' ? 'Validate the value of red border Area' : 'लाल सीमा के मूल्य को मान्य करें');
            }
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends
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

    $scope.GetSocietiesForDeshBoard = function (SocietyIdEnc) {
        $http.post(SiteUrl + "/Society/GetSocietiesForDeshBoardBySocietyId/?SocietyId=" + SocietyIdEnc).
            success(function (data, status) {

                $scope.NextElectionDate = data.filter(function (x) { return x.DataType == '1' }).length > 0 ? data.filter(function (x) { return x.DataType == '1' })[0].DataDate : '';
                $scope.InspectionDueDate = data.filter(function (x) { return x.DataType == '2' }).length > 0 ? data.filter(function (x) { return x.DataType == '2' })[0].DataDate : '';
                $scope.NextAuditDate = data.filter(function (x) { return x.DataType == '3' }).length > 0 ? data.filter(function (x) { return x.DataType == '3' })[0].DataDate : '';
                $scope.NextAGMDate = data.filter(function (x) { return x.DataType == '4' }).length > 0 ? data.filter(function (x) { return x.DataType == '4' })[0].DataDate : '';

                $scope.PaidUpShareCapital = $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc }).length > 0 ? $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc })[0].PaidUpShareCapital : '0';
                $scope.NoOfBranches = $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc }).length > 0 ? $scope.TableDataForFilter.filter(function (x) { return x.SocietyIdENC == SocietyIdEnc })[0].NoOfBranches : '0';
                //$scope.Tot_ELECTEDMEMBERS = $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc).length > 0 ? $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc)[0].Tot_ELECTEDMEMBERS : '0';
                //$scope.Tot_GENERALMEMBERS = $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc).length > 0 ? $scope.TableDataForFilter.filter(x => x.SocietyIdENC == SocietyIdEnc)[0].Tot_GENERALMEMBERS : '0';
                //$scope.Tot_MEMBERS = parseInt($scope.Tot_ELECTEDMEMBERS) + parseInt($scope.Tot_GENERALMEMBERS);

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
            $scope.SOCFileName = '';
        }

        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'No')
            $scope.FileSLSANo = 4;
        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'Yes' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'Yes')
            $scope.FileSLSANo = 5;
        if ($scope.SocietyRegistration.IsAffiliatedBySOC == 'No' && $scope.SocietyRegistration.IsAffiliatedByDLSC == 'No')
            $scope.FileSLSANo = 3;
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
    //START
    $scope.FormValidationSportsAssociation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyRegistration.IsStatLevel == 'No') {
            if ($scope.SocietyRegistration.Ruraltwo == undefined && $scope.lstSocietyWorkScope.length == 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Rural/Urban' : '\n <br /> कृपया शहरी/ग्रामीण का चयन करे करे |';

            }
            //urban=false, rural=true
            if ($scope.SocietyRegistration.Ruraltwo == 'true') {
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
            else if ($scope.SocietyRegistration.Ruraltwo == 'false') {
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
        if ($scope.SocietyRegistration.DISTRICT_ID == 0 && $scope.SocietyRegistration.Ruraltwo != 'false') {
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
            if ($scope.SocietyRegistration.Rural == 'true') {

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
            else if ($scope.SocietyRegistration.Rural == 'false') {
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

    $scope.Profiling = function () {
        if ($scope.SocietyRegistration.IsSportsNewRegistration == 'true' || $scope.SocietyRegistration.IsSportsNewRegistration == true) {
            $scope.IsProfiling = false;
        }
        else {
            $scope.IsProfiling = true;
        }
    }

    $scope.CheckRegOrProfileing = function () {

        if ($scope.SocietyRegistration.IsNewRegistration == 'true' || $scope.SocietyRegistration.IsNewRegistration == true) {
            //$scope.ClickUserVarification();
            $scope.divBrn = true;
            //$scope.divBrnYN = false;
        }
        else {
            //$scope.divBrnYN = true;
            $scope.divBrn = true;
            $scope.translation['MembersApplyingForTheSociety'] = 'Total Number of Member of Society';
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
            //$scope.showtabbar = false;
            $scope.divOtherSocietys = false;
            $scope.IsSocietyNamedisabled = true;
        }
    }

    $scope.ClickBrnYesOrNot = function () {

        if ($scope.SocietyRegistration.IsBRNYes == false || $scope.SocietyRegistration.IsBRNYes == 'false') {
            $scope.ClickUserVarification();
            $scope.IsSocietyNamedisabled = false;
            $scope.showrulesfile = true;
            $scope.divOtherSocietys = true;
            $scope.ShowSubmitBtn = true;
            //$scope.divBrn = false;           
        }
        else if ($scope.SocietyRegistration.IsBRNYes == true || $scope.SocietyRegistration.IsBRNYes == 'true') {
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
        $scope.IsNewRegistration = ($scope.SocietyRegistration.IsNewRegistration == 'false' ? false : true);
        if ($scope.ActId == "1")
            $scope.IsNewRegistration = true;

        if ($scope.SocietyRegistration.IsNewRegistration == 'true' || $scope.SocietyRegistration.IsNewRegistration == true) {
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
                if ($(this).find('td:eq(1)').find('input').val() != '' || FindTablebyId != 0) {
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

                    if ($(this).find('td:eq(10)').find('input').prop('checked') == true) {
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
        $(".se-pre-con").fadeIn("slow");
        if ($("input[name='SocietyRegistration']")[1].checked == true && $scope.SocietyRegistration.ActId == '3')
            $('#lblFeeCalculate').html('0');
        //$("input[name='SocietyRegistration']")[0].checked = false;
        //$("input[name='SocietyRegistration']")[1].checked = false;
        var formData = new FormData();
        formData.append("BRN", $scope.SocietyRegistration.BRN);
        formData.append("IsNewReg", $scope.SocietyRegistration.IsNewRegistration);
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
                }
                else if (data.result == 'failed') {
                    $scope.showrulesfile = false;
                    $scope.ShowSubmitBtn = false;
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", data.BRN);
                }
                else if (data.result == 'invalidname') {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Invalid Society Name: " + data.BO_Name);
                    $scope.CheckBRN = false;
                }

            }).
            error(function (data, status) {
                $scope.CheckBRN = false;
                $scope.nepster = true;
                $scope.showtabbar = true;
                $(".se-pre-con").fadeOut("slow");
                ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
            });

        if ($('#EUserType').val() == 'K' && $scope.SocietyRegistration.ActId != '7') {
            if ($('#EServiceid').val() == '4209')//PROFILE
            {
                $scope.SocietyRegistration.IsNewRegistration = 'false';
                $scope.ClickUserVarification();
                $("input[name='SocietyRegistration']")[0].checked = false;
                $("input[name='SocietyRegistration']")[1].checked = true;
            }
            else if ($('#EServiceid').val() == '4172')//REG
            {
                $scope.SocietyRegistration.IsNewRegistration = 'true';
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


    $scope.CaptureMantra12312 = function () {
        
        // New
        var GetCustomDomName = "127.0.0.1";
        //    
        var primaryUrl = "http://" + GetCustomDomName + ":";
        try {
            var protocol = window.location.href;
            if (protocol.indexOf("https") >= 0) {
                primaryUrl = "https://" + GetCustomDomName + ":";
            }
        } catch (e)
        { }
        //alert("Please wait while discovering port from 11100 to 11120.\nThis will take some time.");       
        for (var i = 11100; i <= 11105; i++) {
            if (primaryUrl == "https://" + GetCustomDomName + ":" && OldPort == true) {
                i = "8005";
            }
            var verb = "RDSERVICE";
            var err = "";
            var res;
            $.support.cors = true;
            $.ajax({
                type: "RDSERVICE",
                async: false,
                crossDomain: true,
                url: primaryUrl + i.toString(),
                contentType: "text/xml; charset=utf-8",
                processData: false,
                cache: false,
                crossDomain: true,
                success: function (data) {
                    finalUrl = primaryUrl + i.toString();
                    $scope.Mantra(finalUrl);
                    return;
                },
                error: function (jqXHR, ajaxOptions, thrownError) {
                    if (i == "8005" && OldPort == true) {
                        OldPort = false;
                        i = "11099";
                    }
                },

            });
        }
    }


    $scope.CaptureMantra = function (finalUrl) {
        
        var GetCustomDomName = '127.0.0.1:' + $("#MantraAVDNPort").val();
        var strWadh = "";
        var strOtp = "";
        // Demo();
        var XML = '<?xml version="1.0"?> <PidOptions ver="1.0"> <Opts fCount="1" fType="0" iCount="0" pCount="0" pgCount="2" format="0"   pidVer="2.0" timeout="10000" pTimeout="20000" posh="UNKNOWN" env="P"  wadh=\"RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw=\" /> <CustOpts><Param name="mantrakey" value="" /></CustOpts> </PidOptions>'
        finalUrl = "http://" + GetCustomDomName;//:" + $("#ddlAVDM").val();
        try {
            var protocol = window.location.href;
            if (protocol.indexOf("https") >= 0) {
                finalUrl = "https://" + GetCustomDomName;
            }
        } catch (e)
        { }
        var verb = "CAPTURE";
        var err = "";
        var res;
        $.support.cors = true;
        var httpStaus = false;
        var jsonstr = "";
        var error_msg = "";
        if (_language == 'en-us') {
            error_msg = "Use following possible steps to resolve the issue:<br>1. Register your device from vendor or using Register Your Device link.<br>2. Download and install the latest registered device drivers and services.<br>3. Start/ restart the registered device service. <br/> 4. Allow device Access from Your System Setting 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        } else {
            error_msg = "समस्या को हल करने के लिए संभव चरणों का उपयोग करें: <br>2. अपने डिवाइस को वेंडर से पंजीकृत करें या रजिस्टर योर डिवाइस लिंक का उपयोग करें। <br>2. नवीनतम पंजीकृत डिवाइस ड्राइवर तथा सर्विसेस  डाउनलोड और स्थापित करें। <br>3. पंजीकृत डिवाइस सेवा शुरू / पुनः आरंभ करें। <br/>4. अपने सिस्टम सेटिंग से इंटरनेट एक्सेस की अनुमति दें 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        }
        $.ajax({
            type: "CAPTURE",
            async: false,
            crossDomain: true,
            url: finalUrl,
            data: XML,
            contentType: "text/xml; charset=utf-8",
            processData: false,
            success: function (data) {
                //var parser = new DOMParser();
                //var xmlDoc = parser.parseFromString(data, "text/xml");
                //var respCode = xmlDoc.getElementsByTagName("Resp")[0];
                //var errorCode = respCode.getAttribute("errCode");
                //if (parseInt(errorCode) == 0) {
                $scope.VerifyBioData(data);
                //} else {

                //}
            },
            error: function (jqXHR, ajaxOptions, thrownError) {
                //$('#txtPidOptions').val(XML);
                var d = 12;
                //alert(thrownError);
                //ShowOkMessage("Error", "15" + error_msg, function () { });
                $scope.CaptureMorpho();

            },
        });
    }

    $scope.CaptureMorpho = function () {
        var error_msg = "";
        if (_language == 'en-us') {
            error_msg = "Use following possible steps to resolve the issue:<br>1. Register your device from vendor or using Register Your Device link.<br>2. Download and install the latest registered device drivers and services.<br>3. Start/ restart the registered device service. <br/> 4. Allow device Access from Your System Setting 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        } else {
            error_msg = "समस्या को हल करने के लिए संभव चरणों का उपयोग करें: <br>2. अपने डिवाइस को वेंडर से पंजीकृत करें या रजिस्टर योर डिवाइस लिंक का उपयोग करें। <br>2. नवीनतम पंजीकृत डिवाइस ड्राइवर तथा सर्विसेस  डाउनलोड और स्थापित करें। <br>3. पंजीकृत डिवाइस सेवा शुरू / पुनः आरंभ करें। <br/>4. अपने सिस्टम सेटिंग से इंटरनेट एक्सेस की अनुमति दें 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        }
        $(".se-pre-con_bio").fadeIn("slow");

        var ssoauth_ver = $.now();
        var data = { p: 'https', type: 'EKYC', device: 'bio', isHttpsService: false };
        $.getScript("../Scripts/ssoauth_capture.min.js?v=" + ssoauth_ver).done(function (script, textStatus) {
            if (textStatus == "success") {
                startCaptureRD({ authType: data.type, fpDevice: data.device, env: "P", isHttpsService: data.isHttpsService },
                    function (responce) {
                        if (responce.status == 'success') {
                            $scope.VerifyBioData(responce.data);
                        } else {
                            $(".se-pre-con_bio").fadeOut("slow");
                            ShowOkMessage("Error", "" + error_msg, function () { });
                        }
                    });
            }
        });



        //var url = "http://127.0.0.1:11101";

        //var xhr;
        //var ua = window.navigator.userAgent;
        //var msie = ua.indexOf("MSIE ");

        //if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return version number
        //{
        //    //IE browser
        //    xhr = new ActiveXObject("Microsoft.XMLHTTP");
        //} else {
        //    //other browser
        //    xhr = new XMLHttpRequest();
        //}

        //xhr.open('RDSERVICE', url, true);

        //xhr.onreadystatechange = function () {
        //    // if(xhr.readyState == 1 && count == 0){
        //    //	fakeCall();
        //    //}
        //    if (xhr.readyState == 4) {
        //        var status = xhr.status;

        //        if (status == 200) {

        //            // alert(xhr.responseText);
        //            var parser = new DOMParser();
        //            var xmlDoc = parser.parseFromString(xhr.responseText, "text/xml");
        //            var respCode = xmlDoc.getElementsByTagName("Interface")[0];
        //            var capturePath = respCode.getAttribute("path");
        //            $scope.Morpho(capturePath);            //Call Capture() here if FingerPrint Capture is required inside RDService() call
        //            //console.log(xhr.response);

        //        } else {
        //            ShowOkMessage("Error", "" + error_msg, function () { });
        //            console.log(xhr.response);

        //        }
        //    }

        //};

        ///*setTimeout(function(){
        //xhr.send();},1000);*/
        //xhr.send();
    }


    $scope.Morpho = function (url) {
        var error_msg = "";
        if (_language == 'en-us') {
            error_msg = "Use following possible steps to resolve the issue:<br>1. Register your device from vendor or using Register Your Device link.<br>2. Download and install the latest registered device drivers and services.<br>3. Start/ restart the registered device service. <br/> 4. Allow device Access from Your System Setting 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        } else {
            error_msg = "समस्या को हल करने के लिए संभव चरणों का उपयोग करें: <br>2. अपने डिवाइस को वेंडर से पंजीकृत करें या रजिस्टर योर डिवाइस लिंक का उपयोग करें। <br>2. नवीनतम पंजीकृत डिवाइस ड्राइवर तथा सर्विसेस  डाउनलोड और स्थापित करें। <br>3. पंजीकृत डिवाइस सेवा शुरू / पुनः आरंभ करें। <br/>4. अपने सिस्टम सेटिंग से इंटरनेट एक्सेस की अनुमति दें 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
        }
        // Demo();
        // var XML = '< PidOptions ver= "1.0" > <Opts fCount="1" fType="0" format="0" pidVer="2.0" timeout="20000" posh="UNKNOWN" env="P" wadh="RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw=" /></PidOptions>';
        //var url = "http://127.0.0.1:" + $("#MorphoAVDNPort").val() + "/capture";
        url = "http:/" + url;
        var PIDOPTS = '<PidOptions ver=\"1.0\">' + '<Opts fCount=\"1\" fType=\"0\" iCount=\"\" iType=\"\" pCount=\"\" pType=\"\" format=\"0\" pidVer=\"2.0\" timeout=\"10000\" otp=\"\" wadh=\"RZ+k4w9ySTzOibQdDHPzCFqrKScZ74b3EibKYy1WyGw=\" posh=\"\"/>' + '</PidOptions>';

        /*
        format=\"0\"     --> XML
        format=\"1\"     --> Protobuf
        */
        var xhr;
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) // If Internet Explorer, return version number
        {
            //IE browser
            xhr = new ActiveXObject("Microsoft.XMLHTTP");
        } else {
            //other browser
            xhr = new XMLHttpRequest();
        }

        xhr.open('CAPTURE', url, false);
        xhr.setRequestHeader("Content-Type", "text/xml");
        xhr.setRequestHeader("Accept", "text/xml");

        xhr.onreadystatechange = function () {
            //if(xhr.readyState == 1 && count == 0){
            //	fakeCall();
            //}
            if (xhr.readyState == 4) {
                var status = xhr.status;
                if (status == 200) {
                    //alert(xhr.responseText);
                    $scope.VerifyBioData(xhr.responseText);
                } else {
                    ShowOkMessage("Error", "" + error_msg, function () { });
                    //console.log(xhr.response);
                }
            }

        };

        xhr.send(PIDOPTS);

    }


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

    $scope.ShowValidateBtn = true;
    $scope.OtpDilogBox = false;
    $scope.messagebox = false;
    $scope.GETCEO = true;
    $scope.GETCHAIRMAN = true;
    $scope.GETAPPOINTED = true;
    $scope.GETlIQUIDATOR = true;
    $scope.GETSPCHAIRMAN = true;



});


