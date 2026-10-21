var ArrDates = new Array();

function ElectionDates() {
    this.StartDate = '';
    this.EndDate = '';
    this.StartTime = '';
    this.EndTime = '';
    this.ElectionMasterId = '0';
    this.El_QuestionId = '';
    this.OtherTime = '';
}
var MasterMod = angular.module('ElectionModule', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ElectionCont", function ($scope, $http, $location) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.OTP = '';
    $scope.TXN = '';
    $scope.SocietyRegistration = [];

    // on page load
    $scope.OnPageLoad = function () {
        $scope.GetElectionDates();
        $(".se-pre-con").fadeIn("slow");
        $('#txtAccidentTime1').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime2').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime3').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime4').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime5').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime6').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime7').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime8').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime9').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime22').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime33').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime44').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime55').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime66').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime77').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime777').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime17').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime99').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime10').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        $('#txtAccidentTime11').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
            showPeriod: true,
            showLeadingZero: true
        });
        //$('#txtRegistrationDate1').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate2').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate3').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate4').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate5').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate6').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate7').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate8').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        //$('#txtRegistrationDate22').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    minDate: '0'
        //});
        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });

        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $(".se-pre-con").fadeOut("slow");

        $scope.ClearUnitForm = function () {
            //$scope.UnitMaster = null;
            $scope.UnitMaster.DepartmentId = 0;
            $scope.UnitMaster.UnitNameEn = '';
            $scope.UnitMaster.UnitNameHi = '';
            $scope.UnitMaster.IsActive = false;
            $scope.UnitMaster.UnitId = 0;
        };
    };

    // START :Get List of Societies

    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + '').
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = data;
            });
    };
    //END    

    $scope.GetSocietyElectionDetail = function () {
        $http.post("/Election/GetSocietyElectionDetail", { "ElectionMasterIdEnc": $('#btnautoclick').val() }).
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.SocietyList = data[0];
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while Society Election Detail data.');
            });
    };
    $scope.ishowSubmitdates = true;
    $scope.GetElectionDates = function () {
        $http.post("/Election/GetElectionDates", { "OredrId": $('#btnautoclick').val() }).
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.SocietyRegistration = data;
                if ($scope.SocietyRegistration.length > 0 && $scope.SocietyRegistration[0].IsElectionOrderPDFSign) {
                    $scope.SocietyRegistration[0].IsElectionOrderPDFSign = $scope.SocietyRegistration[0].IsElectionOrderPDFSign == 'False' ? false : true;
                    if ($scope.SocietyRegistration[0].IsElectionOrderPDFSign) {
                        $scope.ishowSubmitdates = false;
                    }
                }

                setTimeout(function () {
                    $("input[name='RegistrationDate']").each(function () {
                        debugger;
                        if ($(this).val() != '') {
                            if (!$(this).hasClass("SetCalender")) {
                                $(this).addClass("SetCalender");
                                $('.SetCalender').datepicker({
                                    showOn: "both",
                                    changeMonth: true,
                                    buttonImage: "../images/calender.png",
                                    changeYear: true,
                                    dateFormat: "dd/mm/yy",
                                    minDate: '0'
                                });
                            }
                        }
                    });
                }, 1000);
                
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while Society Election Detail data.');
            });
    };


    $scope.SaveElectionDates = function () {
        debugger;
        $(".se-pre-con").fadeIn("slow");
        var ValidationChaeck = true;


        $('#tblDates input:visible').each(function () {
            if ($(this).val().length == 0) {
                $(this).css('border', 'solid 1px red');
                ValidationChaeck = false;
            }
            else {
                $(this).css('border', 'solid 1px');
            }
        });
       
        if (ValidationChaeck == false) {
            $(".se-pre-con").fadeOut("slow");
            return;
        } else {
            $(".se-pre-con").fadeIn("slow");}

        var i = 0;
        ArrDates = new Array();
        $('#tblDates > tbody > tr').each(function (i, j) {
            var _obj = new ElectionDates();
            _obj.El_QuestionId = $(this).find('td:eq(1)').find('input').val();
            _obj.ElectionMasterId = $location.absUrl().split('?')[1].replace('ElectionMasterId=', '');
            _obj.Stap1Date = '';
            _obj.Stap2Date = '';
            _obj.Stap3Date = '';
            _obj.Stap4Date = '';
            _obj.Stap5Date = '';
            _obj.StartTime = '';
            _obj.EndTime = '';
            _obj.OtherTime = '';

            for (var p = 0; p < $scope.SocietyList.StepInElection; p++) {
                if (!(i == 8 || i == 9 || i == 10 || i == 11)) {

                    if (p == 0) {
                        _obj.Stap1Date = $(this).find('td:eq(2)').find('input').val().split("/").reverse().join("-");
                    }
                    else if (p == 1) {
                        _obj.Stap2Date = $(this).find('td:eq(3)').find('input').val().split("/").reverse().join("-");
                    }
                    else if (p == 2) {
                        _obj.Stap3Date = $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-");
                    }
                    else if (p == 3) {
                        _obj.Stap4Date = $(this).find('td:eq(5)').find('input').val().split("/").reverse().join("-");
                    }
                    else if (p == 4) {
                        _obj.Stap5Date = $(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                    }
                }

                if (i == 1) {
                    _obj.EndTime = $('#txtAccidentTime1').val();
                }
                else if (i == 2) {
                    _obj.StartTime = $('#txtAccidentTime2').val();
                    _obj.EndTime = $('#txtAccidentTime22').val();
                }
                else if (i == 3) {
                    _obj.StartTime = $('#txtAccidentTime3').val();
                    _obj.EndTime = $('#txtAccidentTime33').val();
                }
                else if (i == 4) {
                    _obj.StartTime = $('#txtAccidentTime4').val();
                    _obj.EndTime = $('#txtAccidentTime44').val();
                }
                else if (i == 5) {
                    _obj.StartTime = $('#txtAccidentTime5').val();
                    _obj.EndTime = $('#txtAccidentTime55').val();
                }
                else if (i == 8) {
                    _obj.StartTime = $('#txtAccidentTime6').val();
                    _obj.EndTime = $('#txtAccidentTime66').val();
                }
                else if (i == 9) {
                    _obj.StartTime = $('#txtAccidentTime7').val();
                    _obj.EndTime = $('#txtAccidentTime77').val();
                    _obj.OtherTime = $('#txtAccidentTime777').val();
                }
                else if (i == 10) {
                    _obj.EndTime = $('#txtAccidentTime8').val();
                }
                else if (i == 11) {
                    _obj.StartTime = $('#txtAccidentTime9').val();
                    _obj.EndTime = $('#txtAccidentTime99').val();
                }
            }
            ArrDates.push(_obj);
        });

        $.ajax({
            url: '../Election/SaveElectionDatesdata',
            data: JSON.stringify({
                "objElectionSocietyData": ArrDates,
                "ElectionMasterIdEnc": $('#btnautoclick').val(),
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                debugger;
                if (data == "PDF Not Generate") {
                    ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." :"आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                } else
                    if (data.indexOf('NO#') >= 0) {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time.":"आदेश जनरेट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });                   
                   // window.location = '../Election/ElectionSocietyList';
                } else {
                    if (data != "LOGOUT") {
                        // alert(data);
                        $('#lbltrn').html(data);
                        $('#ShowDateDiv').hide();
                        $('#ShowOTPDiv').show();
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                        ShowOkMessage("",_language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });
                    }
                    else {
                        window.location = 'https://sso.rajasthan.gov.in/signin';
                    }
                }
               // window.location = '../Election/ElectionSocietyList';
                $(".se-pre-con").fadeOut("slow");
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
                $(".se-pre-con").fadeOut("slow");
            },
            complete: function () {
                $(".se-pre-con").fadeOut("slow");
            }
        })
    };
    $scope.VerifyOTP = function () {
        debugger;
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/VerifyAadhaarOTPForOrderEsing", { "OTP": $scope.OTP, "TXN": $('#lbltrn').html(), "ElectionMasterIdEnc": $('#btnautoclick').val() }).
                success(function (data, status) {
                    debugger;        $(".se-pre-con").fadeOut("slow");
                    if (data != "Success") {
                
                        alert(data);
                    }
                    else if (data == "Success") {
          
                        alert('Society election order successfully generated');
                        window.location = '../Election/ElectionSocietyList';
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
        $scope.showLoader = false;
    };


    // END :OnPageLoad
    $scope.OnPageLoad();
    $scope.GetSocietyElectionDetail();
    //  $scope.GetSocietyList();
    //  $scope.GetSocietyTypeList();

});






