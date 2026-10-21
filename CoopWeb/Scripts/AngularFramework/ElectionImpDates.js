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
    $scope.SocietyRegistration1 = 0;

    $scope.GetElectionDates = function () {

        $http.post("/Election/GetElectionDates", { "OredrId": $('#btnautoclick').val() }).
            success(function (data, status) {
                
                $scope.SocietyRegistration = data;
                if ($scope.SocietyRegistration.length > 0 && $scope.SocietyRegistration[0].IsElectionOrderPDFSign) {
                  //  $scope.SocietyRegistration[0].IsElectionOrderPDFSign = $scope.SocietyRegistration[0].IsElectionOrderPDFSign == 'False' ? false : true;
                    if ($scope.SocietyRegistration[0].IsElectionOrderPDFSign) {
                        $scope.ishowSubmitdates = false;
                    }
                }

           
                if ($scope.SocietyRegistration[0].Column1 == "-1") {
                    $scope.SocietyRegistration1 = 0;
                    $scope.Ispostpone1 = false;
                }
                else {
                    $scope.SocietyRegistration1 = $scope.SocietyRegistration.length;
                }

                setTimeout(function () {
                    $("input[name='RegistrationDate']").each(function () {
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



    $scope.customheaderstatus = false;

    $scope.Chkcustomheader = function (value) {

        if (document.getElementById('CustomHeader').checked && value == 1) {
            
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/GET_DEFAULT_TEXT", {
                "objElectionSocietyData": ArrDates,
                "ElectionMasterIdEnc": $('#btnautoclick').val(),
                "HeaderContantDetails": $("#HtmlContent6").val() != "" || $("#HtmlContent6").val() != null ? $("#HtmlContent6").val() : " ",
               // "Snumber": $("#htmlcontentsnumber").val(),
                "CustomHeaderValue": 1
            }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.customheaderstatus = true;
                    $scope.checkstatus = true;
                   
                    $("#HtmlContent6").summernote("code", data);
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                   alert('Error while data load.');
                });
        }
        else {
            $scope.customheaderstatus = false;
            $scope.checkstatus = true;
        }
       
    }


    $scope.SendOTP = function () {
       
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
            $(".se-pre-con").fadeIn("slow");
        }

        var i = 0;

        var id = 1;
        var id1 = 2;
        var id2 = 3;
        var id3 = 4;
        var id4 = 5;
        var id5 = 6;
        var id6 = 7;
        var id7 = 8;
        var id8 = 9;
      
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
            _obj.Stap6Date = '';
            _obj.Stap7Date = '';
            _obj.Stap8Date = '';
            _obj.Stap9Date = '';
            _obj.Stap10Date = '';

            _obj.StartTime = '';
            _obj.EndTime = '';
            _obj.OtherTime = '';
            for (var p = 0; p < $scope.SocietyList.StepInElection; p++) {

                    if (!(i == 8 || i == 9 || i == 10 || i == 11)) {
                        if (i == 0)
                        {
                            if (p == 0) {
                               _obj.Stap1Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                                id = id + 10;
                            }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                _obj.Stap3Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                  _obj.Stap4Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id++;
                        }
                        else if (i == 1) {
                            if (p == 0) {
                                _obj.Stap1Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                                id1 = id1 + 19;
                            }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                               
                                _obj.Stap3Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                 _obj.Stap4Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id1++;
                        }
                        else if (i == 2) {
                            if (p == 0) {
                            
                                 _obj.Stap1Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                                  id2 = id2 + 28;
                            }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                  _obj.Stap3Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                  _obj.Stap4Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id2++;
                        }
                        else if (i == 3) {
                            if (p == 0) {
                            
                                 _obj.Stap1Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                                 id3 = id3 + 37;
                             }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                 _obj.Stap3Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                 _obj.Stap4Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                            
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id3++;
                        }
                        else if (i == 4) {
                            if (p == 0) {
                              
                                    _obj.Stap1Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                                    id4 = id4 + 46;
                              }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                   _obj.Stap3Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                _obj.Stap4Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                           
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id4++;
                        }
                        else if (i == 5) {
                            if (p == 0) {
                              
                               _obj.Stap1Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                                id5 = id5 + 55;
                             }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                 _obj.Stap3Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                  _obj.Stap4Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                           
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id5++;
                        }
                        else if (i == 6) {
                            if (p == 0) {
                               
                                 _obj.Stap1Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                                id6 = id6 + 64;
                            }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                _obj.Stap3Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                _obj.Stap4Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                           
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id6++;
                        }
                        else if (i == 7) {
                            if (p == 0) {
                               
                                    _obj.Stap1Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                                    id7 = id7 + 73;
                              
                            }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                  _obj.Stap3Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                _obj.Stap4Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                           
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id7++;
                        }
                      
                        else if (i == 12) {
                            if (p == 0) {
                               
                               _obj.Stap1Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                                id8 = id8 + 82;

                            }
                            else if (p == 1) {
                                _obj.Stap2Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                            }
                            else if (p == 2) {
                                 _obj.Stap3Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                            }
                            else if (p == 3) {
                                 _obj.Stap4Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                            }
                            else if (p == 4) {
                                _obj.Stap5Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                            }
                           
                            else if (p == 5) {
                                _obj.Stap6Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 6) {
                                _obj.Stap7Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 7) {
                                _obj.Stap8Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 8) {
                                _obj.Stap9Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                            }
                            else if (p == 9) {
                                _obj.Stap10Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                            }
                            id8++;
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

        if (document.getElementById('CustomHeader').checked) {
            var message = "";
            if ($("#HtmlContent6").val() == "" || $("#HtmlContent6").val() == null || $("#HtmlContent6").val() == undefined) {
                message += $scope._language == 'en-us' ? 'Please Enter custom header Contant।' : '<br />कृपया हैडर कंटेंट दर्ज करें।';
                if (message.length > 29) {
                    $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("Contant Header", message, function () { });
                        return false;
                    }
                    return true;
            }
            else {
                $(".se-pre-con").fadeIn("slow");
                $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                    if (SessionStatus.Status == false) {
                        ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                    }
                    else {
                        $http.post(SiteUrl + "/Election/SaveElectionDatesdata", {
                            "objElectionSocietyData": ArrDates,
                            "ElectionMasterIdEnc": $('#btnautoclick').val(),
                            "HeaderContantDetails": $("#HtmlContent6").val() != "" || $("#HtmlContent6").val() != null ? $("#HtmlContent6").val() : " "
                          
                        }).
                            success(function (data, status) {

                                if (data.indexOf('NO#') >= 0) {
                                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                                } else {
                                    if (data != "LOGOUT") {
                                        // alert(data);
                                        $('#lbltrn').html(data);
                                        $('#ShowDateDiv').hide();
                                        $('#ShowOTPDiv').show();
                                        $("html, body").animate({ scrollTop: 0 }, "slow");
                                        ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });
                                    }
                                    else {
                                        window.location = 'https://sso.rajasthan.gov.in/signin';
                                    }
                                }
                                $(".se-pre-con").fadeOut("slow");
                            }).
                            error(function (data, status) {
                                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                                alert(message);
                                $(".se-pre-con").fadeOut("slow");
                            })
                    }
                });
            }
        }
        else {

            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                if (SessionStatus.Status == false) {
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                } else {
                    $http.post(SiteUrl + "/Election/SaveElectionDatesdata", {
                        "objElectionSocietyData": ArrDates,
                        "ElectionMasterIdEnc": $('#btnautoclick').val(),
                        "HeaderContantDetails": $("#HtmlContent6").val() != "" || $("#HtmlContent6").val() != null ? $("#HtmlContent6").val() : " "
                       // "CustomHeaderValue": 0
                    }).
                        success(function (data, status) {

                            if (data.indexOf('NO#') >= 0) {
                                ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                            } else {
                                if (data != "LOGOUT") {
                                    // alert(data);
                                    $('#lbltrn').html(data);
                                    $('#ShowDateDiv').hide();
                                    $('#ShowOTPDiv').show();
                                    $("html, body").animate({ scrollTop: 0 }, "slow");
                                    ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent for the e-sign to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });
                                }
                                else {
                                    window.location = 'https://sso.rajasthan.gov.in/signin';
                                }
                            }
                            $(".se-pre-con").fadeOut("slow");
                        }).
                        error(function (data, status) {
                            message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                            alert(message);
                            $(".se-pre-con").fadeOut("slow");
                        })
                }
            });
        }




       
    };

    $scope.SaveElectionDates = function () {
        
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
            $(".se-pre-con").fadeIn("slow");
        }

        var i = 0;

        var id = 1;
        var id1 = 2;
        var id2 = 3;
        var id3 = 4;
        var id4 = 5;
        var id5 = 6;
        var id6 = 7;
        var id7 = 8;
        var id8 = 9;

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
            _obj.Stap6Date = '';
            _obj.Stap7Date = '';
            _obj.Stap8Date = '';
            _obj.Stap9Date = '';
            _obj.Stap10Date = '';

            _obj.StartTime = '';
            _obj.EndTime = '';
            _obj.OtherTime = '';
            for (var p = 0; p < $scope.SocietyList.StepInElection; p++) {

                if (!(i == 8 || i == 9 || i == 10 || i == 11)) {
                    if (i == 0) {
                        if (p == 0) {
                            _obj.Stap1Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                            id = id + 10;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id++;
                    }
                    else if (i == 1) {
                        if (p == 0) {
                            _obj.Stap1Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                            id1 = id1 + 19;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {

                            _obj.Stap3Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id1).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id1++;
                    }
                    else if (i == 2) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                            id2 = id2 + 28;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id2).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id2++;
                    }
                    else if (i == 3) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                            id3 = id3 + 37;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }

                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id3).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id3++;
                    }
                    else if (i == 4) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                            id4 = id4 + 46;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }

                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id4).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id4++;
                    }
                    else if (i == 5) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                            id5 = id5 + 55;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }

                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id5).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id5++;
                    }
                    else if (i == 6) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                            id6 = id6 + 64;
                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }

                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id6).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id6++;
                    }
                    else if (i == 7) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                            id7 = id7 + 73;

                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }

                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id7).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id7++;
                    }

                    else if (i == 12) {
                        if (p == 0) {

                            _obj.Stap1Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                            id8 = id8 + 82;

                        }
                        else if (p == 1) {
                            _obj.Stap2Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                        }
                        else if (p == 2) {
                            _obj.Stap3Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                        }
                        else if (p == 3) {
                            _obj.Stap4Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");
                        }
                        else if (p == 4) {
                            _obj.Stap5Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-"); //$(this).find('td:eq(6)').find('input').val().split("/").reverse().join("-");
                        }

                        else if (p == 5) {
                            _obj.Stap6Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(7)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 6) {
                            _obj.Stap7Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(8)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 7) {
                            _obj.Stap8Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(9)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 8) {
                            _obj.Stap9Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(10)').find('input').val().split("/").reverse().join("-");
                        }
                        else if (p == 9) {
                            _obj.Stap10Date = $('#txtRegistrationDate' + id8).val().split("/").reverse().join("-");//$(this).find('td:eq(11)').find('input').val().split("/").reverse().join("-");
                        }
                        id8++;
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

        ;
        if (document.getElementById('CustomHeader').checked) {
            var message = "";
            ;
            if ($("#HtmlContent6").val() == "" || $("#HtmlContent6").val() == null || $("#HtmlContent6").val() == undefined) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Header Contant' : 'कृपया हैडर कंटेंट दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Header Contant।' : '<br />कृपया हैडर कंटेंट दर्ज करें।';
                }

                if (message.length > 29) {
                    ShowOkMessage("Add Member", message, function () { });
                    return false;
                }
                return true;
            }
            else {
                $(".se-pre-con").fadeIn("slow");
                $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                    if (SessionStatus.Status == false) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                    } else {
                        $http.post(SiteUrl + "/Election/GenerateElectionInitiatPDF", {
                            "objElectionSocietyData": ArrDates,
                            "ElectionMasterIdEnc": $('#btnautoclick').val(),
                            "OTP": $scope.OTP,
                            "TXN": $('#lbltrn').html(),
                            "HeaderContantDetails": $("#HtmlContent6").val() != "" || $("#HtmlContent6").val() != null ? $("#HtmlContent6").val() : " ",
                            "Snumber": $("#htmlcontentsnumber").val(),
                            "CustomHeaderValue": 1
                        }).
                            success(function (data, status) {
                                if (data == "Success") {
                                    ShowOkMessage("", _language == 'en-us' ? 'Order successfully generated' : "आदेश  सफलतापूर्वक जारी हुआ", function () {
                                        $('#myOTPModal').modal("hide");
                                        window.location = '../Election/ElectionSocietyList';
                                    });
                                    //window.location = '../Election/ElectionSocietyList';
                                } else if (data == "0") {
                                    ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                                } else {

                                    if (data == "LOGOUT") {
                                        ShowOkMessage("", 'Session out.\n', function () {
                                            window.location = 'https://sso.rajasthan.gov.in/signin';
                                        });
                                    } else if (data == "Invalid OTP!") {
                                        ShowOkMessage("", data, function () {

                                        });
                                    } else

                                        if (data != "LOGOUT") {
                                            // alert(data);
                                            $('#lbltrn').html(data);
                                            $('#ShowDateDiv').hide();
                                            $('#ShowOTPDiv').show();
                                            $("html, body").animate({ scrollTop: 0 }, "slow");
                                            ShowOkMessage("", data, function () { });
                                        }
                                        else {
                                            window.location = 'https://sso.rajasthan.gov.in/signin';
                                        }
                                }
                                // window.location = '../Election/ElectionSocietyList';
                                $(".se-pre-con").fadeOut("slow");
                            }).
                            error(function (data, status) {
                                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                                alert(message);
                                $(".se-pre-con").fadeOut("slow");
                            });


                    }
                });
            }
        }
        else {
           
            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                if (SessionStatus.Status == false) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                } else {
                    $http.post(SiteUrl + "/Election/GenerateElectionInitiatPDF", {
                        "objElectionSocietyData": ArrDates,
                        "ElectionMasterIdEnc": $('#btnautoclick').val(),
                        "OTP": $scope.OTP,
                        "TXN": $('#lbltrn').html(),
                        "HeaderContantDetails": $("#HtmlContent6").val() != "" || $("#HtmlContent6").val() != null ? $("#HtmlContent6").val() : " ",
                        "CustomHeaderValue": 0
                    }).
                        success(function (data, status) {
                            if (data == "Success") {
                                ShowOkMessage("", _language == 'en-us' ? 'Order successfully generated' : "आदेश  सफलतापूर्वक जारी हुआ", function () {
                                    $('#myOTPModal').modal("hide");
                                    window.location = '../Election/ElectionSocietyList';
                                });
                                //window.location = '../Election/ElectionSocietyList';
                            } else if (data == "0") {
                                ShowOkMessage("", _language == 'en-us' ? "Order could not be generated. Please try after some time." : "आदेश जनरेट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                            } else {

                                if (data == "LOGOUT") {
                                    ShowOkMessage("", 'Session out.\n', function () {
                                        window.location = 'https://sso.rajasthan.gov.in/signin';
                                    });
                                } else if (data == "Invalid OTP!") {
                                    ShowOkMessage("", data, function () {

                                    });
                                } else

                                    if (data != "LOGOUT") {
                                        // alert(data);
                                        $('#lbltrn').html(data);
                                        $('#ShowDateDiv').hide();
                                        $('#ShowOTPDiv').show();
                                        $("html, body").animate({ scrollTop: 0 }, "slow");
                                        ShowOkMessage("", data, function () { });
                                    }
                                    else {
                                        window.location = 'https://sso.rajasthan.gov.in/signin';
                                    }
                            }
                            // window.location = '../Election/ElectionSocietyList';
                            $(".se-pre-con").fadeOut("slow");
                        }).
                        error(function (data, status) {
                            message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                            alert(message);
                            $(".se-pre-con").fadeOut("slow");
                        });


                }
            });
        }


       
    };

    $scope.VerifyOTP = function () {

        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                if (SessionStatus.Status == false) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                } else {
                    $http.post(SiteUrl + "/Election/VerifyAadhaarOTPForOrderEsing", { "OTP": $scope.OTP, "TXN": $('#lbltrn').html(), "ElectionMasterIdEnc": $('#btnautoclick').val() }).
                        success(function (data, status) {
                            $(".se-pre-con").fadeOut("slow");
                            if (data != "Success") {
                                ShowOkMessage("", data, function () { });
                                //alert(data);
                            }
                            else if (data == "Success") {
                                ShowOkMessage("", "Society election order successfully generated", function () { window.location = '../Election/ElectionSocietyList'; });
                            }
                        }).
                        error(function (data, status) {
                            $(".se-pre-con").fadeOut("slow");
                        });
                }
            });
        }
        else {
            ShowOkMessage("Warning", _language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n', function () { });
            //alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
        $scope.showLoader = false;
    };

    // END :OnPageLoad
    $scope.OnPageLoad();
    $scope.GetSocietyElectionDetail();
    //  $scope.GetSocietyList();
    //  $scope.GetSocietyTypeList();

});



