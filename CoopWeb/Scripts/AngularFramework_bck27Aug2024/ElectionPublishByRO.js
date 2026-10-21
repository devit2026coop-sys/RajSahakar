

var ArrSocietyElectionOfficer = new Array();

function SocietyElectionOfficer() {
    this.ElectionMasterId = 0;
    this.SocietyId = 0;
    this.OfficerId = 0;
}
var MasterMod = angular.module('PublishByROMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PublishByROCONT", function ($scope, $http, $sce) {


    var ArrSelectSociety = new Array();
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.OrderId = '';
    $scope.StepInElection = 0
    //$scope.SelectSociety = [];
    $scope.OTP = '';
    $scope.TXN = '';
    // on page load
    $scope.OnPageLoad = function () {
        //var url = window.location.search;
        //$scope.OrderId = (url.split('Id')[1]);
        //$scope.OrderId = $scope.OrderId.substring(1, $scope.OrderId.length);
        $(".se-pre-con").fadeIn("slow");
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
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + '').
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = data;
            });
    };
    //END    


    $scope.GetSocietyListByElectionOffice = function () {
        return $http.post("../Election/ElectionOfficerForSociety/?OredrId=" + $scope.OrderId).
            success(function (data, status) {
                $scope.SocietyListElectionOfficer = data;

                //$scope.SocietyList = $scope.SocietyListdataview;
                //var newdab = data.filter(x => x.StratDate == '12/31/9999' ? '' : StratDate)
                //alert(newdab);
            }).
            error(function (data, status) {
                $scope.SocietyListElectionOfficer = [];
                alert('Error while fetching society list data.');
            });
    };



    $scope.GetSocietyList = function () {
        return $http.post("../Election/GetSocietyListByOrderID/?OredrId=" + $scope.OrderId).
            success(function (data, status) {
                $scope.SocietyListdataview = data;

                // $scope.MyDefaultValue = "1002";
                //$scope.SocietyList = $scope.SocietyListdataview;
                //var newdab = data.filter(x => x.StratDate == '12/31/9999' ? '' : StratDate)
                //alert(newdab);
            }).
            error(function (data, status) {
                $scope.SocietyListdataview = [];
                alert('Error while fetching society list data.');
            });
    };



    $scope.GetElectionQuestionList = function () {
        return $http.post("../Election/GetElectionQuestion/?OredrId=" + $scope.OrderId).
            success(function (data, status) {
                $scope.ElectionQuestionData = data;
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching society list data.');
            });
    };

    $scope.SendOTP = function () {

        var formData = new FormData();
        DataArray = [];
        var RemainingElectionOfficer = 0;

        if ($scope.SocietyListdataview.filter(function (x) { return x.OfficerId === "0" || x.StepId === "0" }).length > 0) {
            RemainingElectionOfficer = 1;
        } else {
            RemainingElectionOfficer = 0
        }
        //alert($scope.SocietyListdataview.filter(function (x) { return x.OfficerId === "0" || x.StepId === "0" }).length);
        //$scope.SocietyListdataview.forEach(function (item, index) {
        //    if (item.OfficerId != '0') {
        //        var objSocietyElectionOfficerList = new SocietyElectionOfficer();
        //        objSocietyElectionOfficerList.ElectionMasterIdEnc = $scope.OrderId;
        //        objSocietyElectionOfficerList.SocietyId = $(this).find('td:eq(1)').find('input').val();
        //        objSocietyElectionOfficerList.StepId = $(this).find('td:eq(2)').find('select').val();
        //        objSocietyElectionOfficerList.OfficerId = $(this).find('td:eq(3)').find('select').val();
        //    }

        //});



        //$('#tblPublish tr').each(function () {
        //    if ($(this).find('td:eq(2)').find('select').val() > 0) {
        //        var objSocietyElectionOfficerList = new SocietyElectionOfficer();
        //        objSocietyElectionOfficerList.ElectionMasterIdEnc = $scope.OrderId;
        //        objSocietyElectionOfficerList.SocietyId = $(this).find('td:eq(1)').find('input').val();
        //        objSocietyElectionOfficerList.StepId = $(this).find('td:eq(2)').find('select').val();
        //        objSocietyElectionOfficerList.OfficerId = $(this).find('td:eq(3)').find('select').val();
        //        //ArrSocietyElectionOfficer.push(objSocietyElectionOfficerList);
        //    }
        //    else {
        //        RemainingElectionOfficer = RemainingElectionOfficer + 1;
        //    }

        //    if ($(this).find('td:eq(3)').find('select').val() > 0) {
        //    }
        //    else {
        //        RemainingElectionOfficer = RemainingElectionOfficer + 1;
        //    }
        //});

        if (RemainingElectionOfficer == 0) {

            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
                if (SessionStatus.Status == false) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
                } else {
                    $.ajax({
                        url: '../Election/GenerateAadhaarOTPForEsing',
                        //data: JSON.stringify({
                        //    "objElectionSocietyData": ArrDates,
                        //    "ElectionMasterIdEnc": $('#btnautoclick').val(),
                        //}),
                        type: 'GET',
                        contentType: 'application/json',
                        // dataType: 'json',
                        beforeSend: function () {
                        },
                        success: function (data) {

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
                }
            });
        }

        else {
            $(".se-pre-con").fadeOut("slow");
            ShowOkMessage('', "Please select election officer and step for every society in order.", function () { });
        }
    };

    // END :Get List of Societies
    $scope.SavePublishData = function () {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        DataArray = [];
        var RemainingElectionOfficer = 0;
        ArrSocietyElectionOfficer = new Array();
        
    
        if ($scope.SocietyListdataview.filter(function (x) { return x.OfficerId === "0" || x.StepId === "0" }).length > 0) {
            RemainingElectionOfficer = 1;
        } else {
            RemainingElectionOfficer = 0
            $scope.SocietyListdataview.forEach(function (item, index) {
            if (item.OfficerId != '0') {
                var objSocietyElectionOfficerList = new SocietyElectionOfficer();
                objSocietyElectionOfficerList.ElectionMasterIdEnc = $scope.OrderId;
                objSocietyElectionOfficerList.SocietyId = item.SocietyID;
                objSocietyElectionOfficerList.StepId = item.StepId
                objSocietyElectionOfficerList.OfficerId = item.OfficerId
                ArrSocietyElectionOfficer.push(objSocietyElectionOfficerList);
            }

        });

        }




        //$('#tblPublish tr').each(function () {
        //    if ($(this).find('td:eq(2)').find('select').val() > 0) {
        //        var objSocietyElectionOfficerList = new SocietyElectionOfficer();
        //        objSocietyElectionOfficerList.ElectionMasterIdEnc = $scope.OrderId;
        //        objSocietyElectionOfficerList.SocietyId = $(this).find('td:eq(1)').find('input').val();
        //        objSocietyElectionOfficerList.StepId = $(this).find('td:eq(2)').find('select').val();
        //        objSocietyElectionOfficerList.OfficerId = $(this).find('td:eq(3)').find('select').val();
        //        ArrSocietyElectionOfficer.push(objSocietyElectionOfficerList);
        //    }
        //    else {
        //        RemainingElectionOfficer = RemainingElectionOfficer + 1;
        //    }

        //    if ($(this).find('td:eq(3)').find('select').val() > 0) {
        //    }
        //    else {
        //        RemainingElectionOfficer = RemainingElectionOfficer + 1;
        //    }
        //});
        //debugger        
        //$.ajax({
        //    url: '../../Election/InsertSocietyElectionOfficer',
        //    data: JSON.stringify({
        //        "ObjListSocietyElecOffi": {
        //            "DataArray": ArrSocietyElectionOfficer,
        //        }
        //    }),
        //    type: 'POST',
        //    contentType: 'application/json',
        //    dataType: 'json',
        //    beforeSend: function () {
        //    },
        //    success: function (data) {              
        //    },
        //})
        if (RemainingElectionOfficer == 0) {
            //$.ajax({
            //    url: '../Election/InsertSocietyElectionOfficerByReturningOfficer',
            //    data: JSON.stringify({
            //        "ObjListSocietyElecOffi": {
            //            "DataArray": ArrSocietyElectionOfficer,
            //        }
            //    }),
            //    type: 'POST',
            //    contentType: 'application/json',
            //    dataType: 'json',
            //    beforeSend: function () {
            //    },


            $http.post(SiteUrl + "/Election/InsertSocietyElectionOfficerByReturningOfficer", {
                "OTP": $scope.OTP, "TXN": $('#lbltrn').html(), "ElectionMasterIdEnc": $scope.OrderId, "ObjListSocietyElecOffi": {
                    "DataArray": ArrSocietyElectionOfficer,
                }
            }).

                success(function (data, status) {
                    

                    if (data == "Success") {
                        ShowOkMessage("", _language == 'en-us' ? 'Order successfully generated' : "आदेश  सफलतापूर्वक जारी हुआ", function () {
                            $('#myOTPModal').modal("hide");
                            window.location = '../Election/PublishByRO';
                        });
                        //window.location = '../Election/ElectionSocietyList';
                    } else
                    if (data == "PDF Not Generate") {
                        ShowOkMessage("", _language == 'en-us' ? "Order could not be approved. Please try after some time." : "आदेश अनुमोदित नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                    } else if (data == "LOGOUT") {
                        ShowOkMessage("", 'Session out.\n', function () {
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        });
                    } else if (data == "Invalid OTP!") {
                        ShowOkMessage("", data, function () {
                        });
                    } else if (data != "LOGOUT") {
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

                    // window.location = '../Election/ElectionSocietyList';
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    ShowOkMessage('', message, function () { });
                    $(".se-pre-con").fadeOut("slow");
                });

        }
        else {
            $(".se-pre-con").fadeOut("slow");
            ShowOkMessage('', "Please select election officer and step for every society in order.", function () { });
        }


    };

    $scope.VerifyOTP = function () {

        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Election/VerifyAadhaarOTPForOrderApproved", { "OTP": $scope.OTP, "TXN": $('#lbltrn').html(), "ElectionMasterIdEnc": $scope.OrderId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data != "Success") {
                        ShowOkMessage("", data, function () { });
                    }
                    else if (data == "Success") {
                        ShowOkMessage("", 'Society election approved successfully', function () {
                            window.location = '../Election/PublishByRO';
                        });

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



    $scope.Chkdsociety = function (SocietyIdENC) {
        var _SelectedSocietyForElection = new SocietySelected();
        _SelectedSocietyForElection.SocietyIdEnc = SocietyIdENC;
        ArrSelectSociety.push(_SelectedSocietyForElection);
    }

    $scope.allChkdsociety = function () {
        angular.forEach($scope.SocietyListdataview, function (value, index) {
            var _SelectedSocietyForElection = new SocietySelected();
            _SelectedSocietyForElection.SocietyIdEnc = value.SocietyIdENC;
            if ($('#select-all').prop('checked') == true) {
                ArrSelectSociety.push(_SelectedSocietyForElection);
            }
            else {
                ArrSelectSociety = [];
            }
        })
    }
    $scope.GetElectionOrderList = function () {
        // alert(2344);
        // alert(JSON.stringify( $scope.ElectionSociety));
        $http.post("../Election/GetElectionOrderListForRetirningOfficer").
            success(function (data, status) {
                //   alert(JSON.stringify(data));
                $scope.ElectionOrderData = data;
                $scope.ElectionOrderData.forEach(function (item, i) {

                    item.SocietiesName = $sce.trustAsHtml(item.SocietiesName);

                })
            }).
            error(function (data, status) {
                $scope.ElectionOrderData = [];
                alert('Error while fetching society list data.');
            });
    };
    $scope.ShowOfficerDetailDiv = function (id, StepInElection) {
        $scope.OrderId = id;
        $scope.StepInElection = StepInElection;
        $scope.ShowOfficerDetail = true;
        $(".se-pre-con").fadeIn("slow");
        $scope.GetSocietyListByElectionOffice().then(function () {
            $scope.GetSocietyList().then(function () {
                $scope.GetElectionQuestionList().then(function () {
                    $(".se-pre-con").fadeOut("slow");
                });
            });
        });

       

        $scope.ShowSavedData = true;
    };


    $scope.Backtable = function () {
        $scope.ShowSavedData = false;
        $scope.ShowOfficerDetail = false;
    }
    $scope.OnPageLoad();
    $scope.GetElectionOrderList();
    // $scope.GetSocietyListByElectionOffice();

    //$scope.SaveTrainning = function () {
    //    $.ajax({
    //        url: '../Trainning/SaveTrainning',
    //        data: JSON.stringify({
    //            "_Model": {
    //                "lstTrainningUsers": ArrSelectTraningUser,
    //            }
    //        }),
    //        type: 'POST',
    //        contentType: 'application/json',
    //        dataType: 'json',
    //        beforeSend: function () {
    //        },
    //        success: function (data) {
    //            if ((data.length) > 0) {
    //                alert("Save Sucessfully");
    //                window.location = '/Trainning/Trainning';
    //            }
    //            else {
    //                alert("Error While Data Insert");
    //                window.location = '/Trainning/Trainning';
    //            }
    //        },
    //    })
    //};


    //$scope.ChkdtraningUsr = function (UserId) {
    //    var _SelectedChkdtraningUsr = new TraningUserSelected();
    //    _SelectedChkdtraningUsr.UserId = UserId;
    //    ArrSelectTraningUser.push(_SelectedChkdtraningUsr);
    //}

});






