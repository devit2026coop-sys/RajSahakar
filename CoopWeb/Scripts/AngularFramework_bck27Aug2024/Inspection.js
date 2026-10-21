function AddNewrow(Id, count) {
    var Rows = '<td><label style="display: none;">' + ($('#hdfUserType').val() == 'C' ? '0' : '1') + '</label><input type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >'
    for (var i = 1; i < count; i++) {
        Rows += '<td><label style="display: none;">' + ($('#hdfUserType').val() == 'C' ? '0' : '1') + '</label><input type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >'
    }
    Rows += '<td><input type="button" class="btn btn-primary dev-btn ng-binding" value="REMOVE" onclick="Removerow(this)">';

    $('#tbl_' + Id).append('<tr>' + Rows + '</tr>');
}
function Removerow(elem) {
    // console.log($(elem).html());
    $(elem).closest('tr').remove();
    //$(".bricks").click(function () {
    //    var index = $(".bricks").index(this);

    //    alert("brick  " + index);
    //});
}

var ArrInspection = new Array();
var ArrTableHeader = new Array();


function TableHeaderAns() {
    this.AnswerId = '0';
    this.QuestionId = '0';
    this.TableId = '0';
    this.Answear = ' ';
    this.InspectionId = '0';
    this.SSO_ID = '0';
    this.UserType = '0';
}

function InspectionAns() {
    this.AnswerId = '0';
    this.InspectionId = '0';
    this.QuestionId = '0';
    this.Answear = ' ';
    this.InspectorRemark = '';
    this.IsObjection = 'false';
    this.SSO_ID = '0';
    this.CEORemark = '';
    //this.lstTableHeaderAns[];
}


var InspectionMod = angular.module('InspectionModule', ['angularUtils.directives.dirPagination']);
InspectionMod.controller("InspectionCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.DivDate = false;
    $scope.DivFilePath = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.ShowArea = false;
    $scope.Inspection = [];
    $scope.Inspection.SocietyTypeId = 0;
    $scope.Inspection.ClassId = 0;
    $scope.FYID = 0;
    //GET Load Table data
    $scope.DashboardData = function (SocietyTypeId, ClassId) {
        $http.post(SiteUrl + "/Inspection/InspectionForSocietyOfUser", { SocietyTypeId: $scope.Inspection.SocietyTypeId, ClassId: $scope.Inspection.ClassId, FYID: $scope.Inspection.Year }).
            success(function (data, status) {
                $scope.TableData = [];
                $scope.TableData = data;
                alert(JOSN.stringify(data));

                //if (data.length > 0) {
                //    $('#lblSocityName').text(data[0].SocietyName);
                //    $('#lblRegistrationNo').text(data[0].RegistrationNo);
                //    $('#lblRegistrationDate').text(data[0].RegistrationDate);
                //    $('#lblCEOName').text(data[0].Name);
                //}


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

    // START : Upload Inspection File


    $scope.UploadInspectionDoc = function (file) {
        $scope.InspectionFile = file[0];
    }

    $scope.UploadInspectionFile = function () {
        var IsChacked = true;
        if ($scope.Inspection.InspectionFileRemark == '' || $scope.Inspection.InspectionFileRemark == undefined) {
            alert('Please Enter Remark');
            IsChacked = false;
        }

        if ($scope.InspectionFile == undefined || $scope.InspectionFile == '') {
            alert('Please Upload File First');
            IsChacked = false;
        }
        if (!IsChacked)
            return false;

        $scope.Inspection.InspectionID = $('#hdfInspectionid').val();

        var formData = new FormData();
        formData.append("InspectionID", $scope.Inspection.InspectionID);
        formData.append("InspectionFile", $scope.InspectionFile);
        formData.append("InspectionFileRemark", $scope.Inspection.InspectionFileRemark);

        $http.post(SiteUrl + "/Inspection/UploadInspectionFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $('#fileremark').val('');
                if (data > 0) {
                    $scope.IsFileUploded = true;
                    $scope.IsDisable = false;
                    ShowModelAlert("Inspection Reply", "File Upload Sucessfully");
                }
            }).
            error(function (data, status) {

                ShowModelAlert("Inspection Reply", "Error File Upload Sucessfully");
            });

    };

    // END : Upload Inspection File

    // START : Save Inspection Reply
    $scope.SaveInspectionReply = function () {
        var formData = new FormData();
        formData.append("ComplinceFile", $scope.ComplinceDoc);
        formData.append("Compliance", $scope.Inspection.Compliance);
        formData.append("ReplyRemark", $scope.Inspection.ReplyRemark);

        $http.post(SiteUrl + "/Inspection/SaveInspectionReply", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data > 0) {
                    ShowModelAlert("Inspection Reply", "Inspection Reply Insert Sucessfully");
                }


            }).
            error(function (data, status) {

                ShowModelAlert("Inspection Reply", "Error While Inspection Reply");
            });

    };
    // END :Save Inspection Reply

    $scope.SaveInspectionAns = function () {
        if ($('#hdfUserType').val() != 'C') {
            var ValidationChaeck = true;

            if ($('#MainTable').children('tbody').children('tr:first').children('td').length > 5) {
                if ($scope.IsFileUploded != true) {
                    alert('Please Upload File First');
                    return false;
                }
            }

            $('#MainTable textarea').each(function () {
                if ($(this).val().length == 0) {
                    $(this).css('border', 'solid 1px red');
                    ValidationChaeck = false;
                }
                else {
                    $(this).css('border', 'solid 1px');
                }
            });

            if (ValidationChaeck == false) {
                return;
            }

            var TR = $('#MainTable >tbody > tr').find('[Id^="tr_"]')


            for (var i = 0; i < TR.prevObject.length; i++) {
                if (TR.prevObject[i].id != "") {
                    var _InspectionAns = new InspectionAns();
                    
                    _InspectionAns.InspectionId = $('#hdfInspectionid').val();
                    _InspectionAns.QuestionId = TR.prevObject[i].id.split('_')[1];

                    $('#' + TR.prevObject[i].id).each(function () {
                        _InspectionAns.InspectorRemark = $(this).find('td:eq(4)').find('textarea').val();
                        _InspectionAns.Answear = $(this).find('td:eq(3)').find('textarea').val();
                        _InspectionAns.IsObjection = $(this).find('td:eq(2)').find('input[type="checkbox"]').prop("checked");
                        _InspectionAns.Isliquideter = $('#idstrue').prop("checked") == true ? true : false;
                        _InspectionAns.Isliquideterremark = $('#txtarealiq').val();
                        if ($('#MainTable').children('tbody').children('tr:first').children('td').length > 5) {
                            _InspectionAns.AnswerId = $(this).find('td:eq(5)').find('textarea').attr('id');
                            _InspectionAns.CEORemark = $(this).find('td:eq(5)').find('textarea').val();
                        }
                    });

                    if (TR.prevObject[i].id.split('_').length > 1) {
                        ArrInspection.push(_InspectionAns);
                    }
                }
            }

            var tables = 'tbl_';
            var File = $('#DIV1 table').find('[Id^="tbl_"]');

            for (var i = 1; i < File.prevObject.length; i++) {
                var tempCount = 0;
                var tempTableId = [];
                $('#' + File.prevObject[i].id + '>tbody > tr').each(function () {

                    var TempCountNew = 0;
                    if ($('#' + File.prevObject[i].id + '>tbody > tr').length > 1) {
                        $('#' + File.prevObject[i].id + '>tbody > tr').each(function () {
                            if (TempCountNew == 0) {
                                $(this).find('td').each(function () {
                                    tempTableId.push($(this).find('input').attr('id'));
                                });
                                TempCountNew = 1;
                            }
                        });
                    }
                    $(this).find('td').each(function () {
                        tempCount = tempCount + 1;
                        if ($(this).find('input').val() != 'जोड़ें' && $(this).find('input').val() != 'REMOVE') {
                            if ($(this).find('input').val().length > 0) {

                                //if ($(this).find('label').html() == '' || $(this).find('label').html() == undefined);
                                //{
                                //    $(this).find('label').html($('#hdfUserType').val() == 'C' ? '0' : '1');
                                //}

                                var _TableHeaderAns = new TableHeaderAns();
                                _TableHeaderAns.UserType = $(this).find('label').html();
                                _TableHeaderAns.Answear = $(this).find('input').val();
                                _TableHeaderAns.InspectionId = $('#hdfInspectionid').val();
                                _TableHeaderAns.QuestionId = File.prevObject[i].id.split('_')[1];
                                _TableHeaderAns.TableId = ($(this).find('input').attr('id') == undefined ? tempTableId[tempCount - 1] : $(this).find('input').attr('id'));
                                ArrTableHeader.push(_TableHeaderAns);
                                $(this).css('border', 'solid 1px');
                            }
                            else {
                                $(this).find('input').css('border', 'solid 1px red');
                                ValidationChaeck = false;
                            }
                        }
                    });
                });

            }
            if (ValidationChaeck == false) {
                return;
            }


        }
        else {
            var TR = $('#MainTable >tbody > tr').find('[Id^="tr_"]')

            ArrInspection = [];
            for (var i = 0; i < TR.prevObject.length; i++) {
                if (TR.prevObject[i].id != "") {
                    var _InspectionAns = new InspectionAns();
                    _InspectionAns.InspectionId = $('#hdfInspectionid').val();


                    $('#' + TR.prevObject[i].id).each(function () {
                        if ($(this).find('td:eq(5)').find('textarea').val().length > 1) {
                            $(this).find('td:eq(5)').find('textarea').css('border', 'solid 1px');
                            _InspectionAns.CEORemark = $(this).find('td:eq(5)').find('textarea').val();
                            _InspectionAns.AnswerId = $(this).find('td:eq(5)').find('textarea').attr('id');
                        }
                        else {
                            $(this).find('td:eq(5)').find('textarea').css('border', 'solid 1px red');
                            ValidationChaeck = false;
                        }

                    });

                    if (ValidationChaeck == false) {
                        return;
                    }
                    if (TR.prevObject[i].id.split('_').length > 1) {
                        ArrInspection.push(_InspectionAns);
                    }
                }
            }
            var tables = 'tbl_';
            var File = $('#DIV1 table').find('[Id^="tbl_"]');

            for (var i = 1; i < File.prevObject.length; i++) {
                var tempCount = 0;
                var tempTableId = [];
                $('#' + File.prevObject[i].id + '>tbody > tr').each(function () {

                    var TempCountNew = 0;
                    if ($('#' + File.prevObject[i].id + '>tbody > tr').length > 1) {
                        $('#' + File.prevObject[i].id + '>tbody > tr').each(function () {
                            if (TempCountNew == 0) {
                                $(this).find('td').each(function () {
                                    tempTableId.push($(this).find('input').attr('id'));
                                });
                                TempCountNew = 1;
                            }
                        });
                    }
                    $(this).find('td').each(function () {
                        tempCount = tempCount + 1;
                        if ($(this).find('input').val() != 'जोड़ें' && $(this).find('input').val() != 'REMOVE' && $(this).find('input').val() != undefined) {
                            if ($(this).find('input').val().length > 0) {
                                var _TableHeaderAns = new TableHeaderAns();
                                _TableHeaderAns.UserType = $(this).find('label').html();
                                _TableHeaderAns.Answear = $(this).find('input').val();
                                _TableHeaderAns.InspectionId = $('#hdfInspectionid').val();
                                _TableHeaderAns.QuestionId = File.prevObject[i].id.split('_')[1];
                                _TableHeaderAns.TableId = ($(this).find('input').attr('id') == undefined ? tempTableId[tempCount - 1] : $(this).find('input').attr('id'));
                                ArrTableHeader.push(_TableHeaderAns);
                                $(this).css('border', 'solid 1px');
                            }
                            else {
                                $(this).find('input').css('border', 'solid 1px red');
                                ValidationChaeck = false;
                            }
                        }
                    });
                });

            }
            if (ValidationChaeck == false) {
                return;
            }
            if ($('#txtDobm').val() == "") {
                alert('Please Select Date of board meeting')
                $('#txtDobm').focus();
                $('#txtDobm').css('border', 'solid 1px red');
                return;
            }
        }



        $.ajax({
            url: '../Inspection/SaveInspectionReplyByOfficer',
            data: JSON.stringify({
                "ObjInspectionAnsSave": {
                    "InspectionAnsList": ArrInspection,
                    "TableHeaderAnsList": ArrTableHeader,
                    "Dobm": $('#txtDobm').val().split("/").reverse().join("-")
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
                //$('[Id$="btnSubmitToDB"]').attr('disabled', true);
                //$('[Id$="btnResetFinal"]').attr('disabled', true);
            },
            success: function (data) {
                if (data.data != '-1') {
                    alert('Society Inspection Data Save Sucessfully');
                    window.location = "../Inspection/Inspection";
                }
                else {
                    ShowModelAlert("Society Inspection", 'Error While Society Inspection Data Insert');
                    //window.location = "../Inspection/Inspection";
                }

            },
            error: function (e) {
                alert(e);
            },
            complete: function () {

            }
        });


    }

    $scope.SETSocietyIdENC = function (SocietyIdEnc, InspOfficerSocietyId, FYNAME, FYID) {
        $scope.Inspection.SocietyIdEnc = SocietyIdEnc;
        $scope.Inspection.InspOfficerSocietyId = InspOfficerSocietyId;
        $scope.GetInspectionDataForReply(SocietyIdEnc);
        $scope.FYearName = FYNAME;
        $scope.FYID = FYID;
    }

    $scope.GetInspectionDataForReply = function (SocietyIdEnc) {
        $scope.SocietyItem = $scope.TableData.filter(x => x.SocietyIdENC == $scope.Inspection.SocietyIdEnc);
        // console.log($scope.SocietyItem[0]);
        //$('#lblSocityName').text($scope.SocietyItem.SocietyName);
        //$('#lblStartDate').text($scope.SocietyItem.StartDate);
        //$('#lblEndDate').text($scope.SocietyItem.EndDate);
        //$('#lblAdmin').text($scope.SocietyItem.Admin);
        //$('#lblFinancial').text($scope.SocietyItem.Fincial);
        //$('#lblTradeBusiness').text($scope.SocietyItem.TrandBusiness);
        //$('#lblObservation').text($scope.SocietyItem.Remark);
        //alert($scope.SocietyItem.SocietyName);
        $('#lblSocityName').text($scope.SocietyItem[0].SocietyName);
        $('#lblRegistrationNo').text($scope.SocietyItem[0].RegistrationNo);
        $('#lblRegistrationDate').text($scope.SocietyItem[0].InsertDate);
        $('#lblCEOName').text($scope.SocietyItem[0].Name);

    }

    $scope.UploadComplinceDoc = function (file) {
        $scope.ComplinceDoc = file[0];
    }



    $scope.SaveInspection = function () {
        if ($scope.InspectionFormValidation()) {
            var data = {
                DateOfEndInspection: $scope.Inspection.DateOfEndInspection,
                DateOfStartInspection: $scope.Inspection.DateOfStartInspection,
                InspOfficerSocietyId: $scope.Inspection.InspOfficerSocietyId,
                SocietyIdEnc: $scope.Inspection.SocietyIdEnc,
                SocietyTypeId: "0",
                DateOfStartInspection: $('#txtStartInspectionDate').val().split("/").reverse().join("-"),
                DateOfEndInspection: $('#txtEndInspectionDate').val().split("/").reverse().join("-"),
                Year: $scope.FYID
            };

            $http.post(SiteUrl + "/Inspection/SaveInspection", data).
                success(function (data, status) {
                    var k = data;
                    var message = '';
                    if (k.length > 10) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                        window.location = '/Inspection/InspectionQuestions?InspectionID=' + k;
                        return;
                    }
                    else if (k == -2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    }
                    alert(message);
                    // $scope.ClearInspectionForm();
                    // $scope.DashboardData();
                    //data - dismiss="modal"

                }).
                error(function (data, status) {
                    alert("Error while Inspection Data Insert");
                });
        }
    };



    $scope.InspectionFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($('#txtStartInspectionDate').val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Date Of Start Inspection.' : '\n <br /> कृपया प्रारंभ निरीक्षण की तारीख दर्ज करें|';

        }
        if ($('#txtEndInspectionDate').val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Date Of End Inspection' : '\n <br /> कृपया अंत निरीक्षण की तारीख दर्ज करें|';

        }
        if ($scope.Inspection.Year == undefined || $scope.Inspection.Year == '' || $scope.Inspection.Year == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Financial Year.' : '\n <br /> कृपया वित्तीय वर्ष दर्ज करें|';

        }

        //if ($scope.Inspection.Admin == undefined || $scope.Inspection.Admin == '') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please enter a comment related to the admin' : '\n <br /> कृपया एडमिन से सम्बंधित टिप्पणी दर्ज करें|';

        //}
        //if ($scope.Inspection.Financial == undefined || $scope.Inspection.Admin == '') {
        //    message += $scope._language == 'en-us' ? '\n <br />  Please enter comment related to the financial.' : '\n <br /> कृपया वित्तीय से सम्बंधित टिप्पणी दर्ज करें |';

        //}
        //if ($scope.Inspection.TradeBusiness == undefined || $scope.Inspection.TradeBusiness == '') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please enter comment related to the Trade & Business  ?' : '\n <br /> कृपया व्यापार से संबंधित टिप्पणी दर्ज करें ?';
        //}
        //if ($scope.Inspection.Remark == undefined || $scope.Inspection.Remark == '') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please enter a comment related to the your Observation  ?' : '\n <br /> कृपया अपने निरीक्षण से संबंधित एक टिप्पणी दर्ज करें ?';
        //}

        if ($scope.Inspection.Selfdeclaration == 'FALSE') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Check Self Authentication Check' : '\n <br /> कृपया स्व प्रमाणीकरण का चयन करें |';

        }

        if (message.length > 29) {
            var _Temp = (_language == 'en-us' ? 'Inspection Details' : 'निरीक्षण विवरण');
            ShowModelAlert(_Temp, message);
            return false;
        }
        return true;
    }

    $scope.GotoInspection = function (k) {
        window.location = '../Inspection/InspectionQuestions?InspectionID=' + k;
    }

    $scope.GotoInspectionView = function (k) {
        window.location = '../Inspection/InspectionQuestionsView?InspectionID=' + k;
    }

    $scope.ClearInspectionForm = function () {
        $scope.Inspection.DateOfStartInspection = '';
        $('#txtStartInspectionDate').val('');
        $scope.Inspection.DateOfEndInspection = '';
        $('#txtEndInspectionDate').val('');
        $scope.Inspection.Admin = '';
        $scope.Inspection.Financial = '';
        $scope.Inspection.TradeBusiness = '';
        $scope.Inspection.Year = '0';
        $scope.Inspection.Remark = '';
        $scope.Inspection.Selfdeclaration = '';
        $scope.Inspection.DateOfStartInspection = '';
        $scope.Inspection.DateOfEndInspection = '';
        //  $scope.Inspection.Year = 0;
    };

    $scope.GoBackTo = function () {
        window.location = '/Inspection/Inspection';
    };


    //End 

    $scope.GetQuestionsForInspection = function (val) {
        $http.post(SiteUrl + "/Inspection/InspectionQuestionData?InspectionID=" + val).
            success(function (data, status) {
                if (data != "-1") {
                    $('#DIV1').html(data.split('@#')[0]);
                    var respons = (data.split('@#')[1]) == 1 ? true : false;                    
                    if ((data.split('@#')[2]).indexOf('0001') < 0)
                        $('#txtDobm').val(data.split('@#')[2]);
                    if (respons == true) {
                        $("#idstrue").prop("checked", respons);
                        $scope.ShowArea = true;
                    }
                    else {
                        $("#idsfalse").prop("checked", true);
                        $scope.ShowArea = false;
                    }



                }
                else {
                    $("#idsfalse").prop("checked", true);
                    $scope.ShowArea = false;
                }



                if ($('#hdfUserType').val() == 'C') {
                    $scope.IsCityzen = false;
                    $scope.CityzenDobm = true;
                    //txtDobm
                }
                if ($('#hdfUserType').val() == 'D' && $('#MainTable').children('tbody').children('tr:first').children('td').length > 5) {
                    $scope.IsCityzen = true;
                    $scope.IsDisable = true;
                    $scope.CityzenDobm = true;
                }

            }).
            error(function (data, status) {
                $scope.TableData = [];
            });
    }

    $scope.InspectionQuestionDataForPDF = function (val) {
        $http.post(SiteUrl + "/Inspection/InspectionQuestionDataForPDF?InspectionID=" + val).
            success(function (data, status) {
                if (data != "-1") {
                    $('#DIV1').html(data.split('@#')[0]);
                    var respons = (data.split('@#')[1]) == 1 ? true : false;
                    $scope.DOM = (data.split('@#')[2]) == '' ? '' : (data.split('@#')[2]);
                    $scope.FilePath = (data.split('@#')[3]) == '' ? '' : (data.split('@#')[3]);
                    $scope.FileRemark = (data.split('@#')[4]) == '' ? '' : (data.split('@#')[4]);
                    if ((data.split('@#')[2]).indexOf('0001') < 0) {
                        $scope.DivDate = true;
                    }

                    if ($scope.FilePath.length > 0) {
                        $scope.DivFilePath = true;
                    }
                    if (respons == true) {
                        $("#idstrue").prop("checked", respons);
                        $scope.ShowArea = true;
                    }
                    else {
                        $("#idsfalse").prop("checked", true);
                        $scope.ShowArea = false;
                    }
                }
                else {
                    $("#idsfalse").prop("checked", true);
                    $scope.ShowArea = false;
                    $scope.DivDate = false;
                    $scope.DivFilePath = false;
                }

                if ($('#hdfUserType').val() == 'C') {
                    $scope.IsCityzen = false;
                }
                else if ($('#hdfUserType').val() == 'D' && $('#MainTable').children('tbody').children('tr:first').children('td').length > 5) {
                    $scope.IsCityzen = true;
                    $scope.CityzenDobm = true;
                    alert('Rajeev');
                }
                else {
                    $scope.IsCityzen = false;
                    $scope.CityzenDobm = true;
                }
                // alert($scope.IsCityzen);
            }).
            error(function (data, status) {
                $scope.TableData = [];


            });
    }

    // on page load
    $scope.OnPageLoad = function () {
        $('#txtEndInspectionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear()).toString(),
            minDate: '0'
        });
        $('#txtStartInspectionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear()).toString(),
            minDate: '0'
        });
        $('#txtDobm').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0',
        });

        $("#txtDobm").val((new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear()).toString());
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
        $(".se-pre-con").fadeOut("slow");

        //GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
                // alert(JSON.stringify(FinancialYearList));
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });


        // GET Society Type
        //$scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
        //};
        //END   
        // GET Society Class
        // $scope.GetSocietyClass = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });

        //}
        //END
    };

    $scope.ShowInspection = function () {
        $scope.DashboardData($scope.Inspection.SocietyTypeId, $scope.Inspection.ClassId, $scope.Inspection.Year);
    }

    $scope.Onclickareahide = function () {
        if (($("#idsfalse").prop("checked")) == true) {
            $scope.ShowArea = false;
        }
        else if (($("#idstrue").prop("checked")) == true) {
            $scope.ShowArea = true;
        }
    }
    $scope.OnPageLoad();
});