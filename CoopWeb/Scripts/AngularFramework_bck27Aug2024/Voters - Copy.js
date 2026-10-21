var AppClasses = angular.module("VotersElectionMod", ['angularUtils.directives.dirPagination']);

var ImageDataList = [];
var PDFDataList = [];
function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
    this.FileName = '';
}
AppClasses.controller("VotersElectionCnt", function ($scope, $http, $rootScope) {
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 10,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }  
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'SubmissionDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }        
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
       $scope.GetElectionSocietyListData();        
    }
    /*------------End Paging and Filter-----------------*/


    $scope.SocietyIdEnc = '';
    $scope.ElectionMasterId = 0;
    $scope.ElctionVoterListData = [];
    $scope.EMemberDetailsData = [];
    $scope.ShowSocietyList = true;
    $scope.ShowVoterList = false;
  

    $scope.IsSubmit = false;
    $scope.IsValid = false;
    $scope.IsValidMessages = "";
    //$scope.IsPublish = false;
    $scope.TotalMembers = 0;
    $scope.LastSubmistionDate = "";
    $scope.ReSetAddModel = function () {
        $scope.AddNewMember = {
            SrNo: 0,
            MemberId: '0',
            Name: '',
            ContactNo: '',
            AadharNo: '',
            DOB: '',
            Gender: '',
            CODEID: '',
            FHName: '',
            Address: '',
            Photo: '',
            IsDelete: false,
            AddRemarks: '',
            DeleteDocument: '',
            DeleteRemarks: '',
            AddDocument: '',
            SocietyIdEnc: $scope.SocietyIdForAdministrativeDetails,
            IsActive: true,
            ElectionMasterId: 0,
            UserType: 5,
            AdministratorDesignationId: -1,
            IsCEO: true,
            CategoryId: "0",
            AadharNo2: '',
            Index: 0,
            IsShow: true,
            Photo1: ""
        }
        newMemberPhoto = "";
        $("#addModal").find('input[type="file"]').val("");
        $('#addNewDelete').prop('checked', false);
        $('.addNewIsDelete').hide();
        $("#MemberDeletePDFForm1").val("");
    }
    $scope.AddNewMember = {
        SrNo: 0,
        MemberId: '0',
        Name: '',
        ContactNo: '',
        AadharNo: '',
        DOB: '',
        Gender: '',
        CODEID: '',
        FHName: '',
        Address: '',
        Photo: '',
        IsDelete: false,
        AddRemarks: '',
        DeleteDocument: '',
        DeleteRemarks: '',
        AddDocument: '',
        SocietyIdEnc: $scope.SocietyIdForAdministrativeDetails,
        IsActive: true,
        ElectionMasterId: 0,
        UserType: 5,
        AdministratorDesignationId: -1,
        IsCEO: true,
        CategoryId: "0",
        AadharNo2: '',
        Index: 0,
        IsShow: true,
        Photo1: ""
    }

    $scope.AddMoreMemberDetails = function () {
        //if ($scope.TableValiDate(3)) {
        if ($scope.ValidateSaveData()) {
            $("#addModal").modal("show");
            $scope.ReSetAddModel();
            $scope.AddCalender();
            return;            
        }
    };

    $scope.AddNewMemberRow = function () {
        $(".validateAddMember").removeClass("notValid");
        if ($scope.AddMemberValiDate()) {
            $scope.AddNewMember.DOB = $("#txtAddDOB2").val();
            $scope.AddNewMember.CODEID = $("#txtAddDOB2").val();
            $scope.AddNewMember.AadharNo2 = $scope.AddNewMember.AadharNo;
            $scope.AddNewMember.Delete = true;
            $scope.AddNewMember.SrNo = $scope.EMemberDetailsData.length + 1;
            $scope.AddNewMember.Photo1 = newMemberPhoto;
            $scope.AddNewMember.PhotoURL = newMemberPhoto;
            $scope.EMemberDetailsData.push($scope.AddNewMember);
            if ($("#addNewDelete").is(":checked")) {
                $scope.AddNewMember.IsDelete = true;
                //$scope.AddNewMember.DeleteRemark = $("#txtAddDOB2").val();
            }
            //$scope.AddNewMember.AddRemark = $("#txtAddDOB2").val();
            $scope.AddNewMember.ElectionMasterId = $scope.EMemberDetailsData[0].ElectionMasterId;

            debugger;
            $("#addModal").find('input[type="file"]').each(function (indx) {
                //alert($(this)[0].files.length);
                debugger;



                if ($(this).attr('filetype').toLowerCase() == "pdf") {
                    if ($(this)[0].files.length > 0) {
                        $scope.ElUploadPDF(this, $scope.EMemberDetailsData.length, $(this).attr('dat-val'), indx);
                    }
                } else {
                    if ($(this)[0].files.length > 0) {
                        $scope.ElUploadAdministrative(this, $scope.EMemberDetailsData.length);
                    }

                }
                //$scope.ElUploadAdministrative

            });

            $("#addModal").modal("hide");

            $scope.ReSetAddModel();
        }
    }

    $scope.DeleteMember = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#MemberDeletePDFForm1").removeClass("notValid");
        $("#MemberDeleteRemarks").removeClass("notValid");


        if ($("#MemberDeletePDFForm1").val() == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select pdf for delete Member.' : '\n <br /> कृपया सदस्य हटाने के लिए पीडीएफ का चयन करें |';
            $("#MemberDeletePDFForm1").addClass("notValid");
        }
        if ($scope.AddNewMember.DeleteRemarks == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter remark for member delete.' : '\n <br /> कृपया सदस्य हटाने के लिए टिप्पणी दर्ज करें |';
            $("#MemberDeleteRemarks").addClass("notValid");
        }

        //if ($scope.EMemberDetailsData.length = index + 1) {
        if (message.length > 29) {
            ShowModelAlert("Add Member", message);

        } else {
            debugger;
            $scope.EMemberDetailsData[$scope.AddNewMember.Index].IsDelete = true;
            $scope.EMemberDetailsData[$scope.AddNewMember.Index].DeleteRemarks = $scope.AddNewMember.DeleteRemarks;
            $scope.EMemberDetailsData[$scope.AddNewMember.Index].Delete = false;

            if ($("#MemberDeletePDFForm1")[0].files.length > 0) {
                $scope.ElUploadPDF($("#MemberDeletePDFForm1"), $scope.AddNewMember.Index + 1, "Delete", $scope.AddNewMember.Index + 1);
            }
            var srno = 1;
            $scope.EMemberDetailsData.forEach(function (itm, idnx) {
                itm.IsShow = true;
                if (itm.Delete)
                    itm.SrNo = srno++;
            });
            $("#deleteModal").modal("hide");
        }
    }


    $scope.AddMoreMemberDetailsWithoutValidation = function () {
        //  $(".clsShowHide").hide();
        var rows = $('#myTable2 tr').length + 1;
        for (; typeof $("#lblElAdministrativeFile" + rows + "").html() !== "undefined";) {
            rows = rows + 1;
        }
        $('#myTable2').append('<tr>' +
            '<td>' + rows + '</td>' +
            '<td><input type= "text" id="ElDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyRegistration.ElDirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input type="text" id="ElContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyRegistration.ElContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
            '<td><input type="text" id="ElAADHAARNo' + rows + '" name="AdhareNo" class="form-control" placeholder="आधार संख्या" ng-model="SocietyRegistration.ElAADHAARNo1' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
            '<td><input id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyRegistration.ElDOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
            '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" name="ElGender' + rows + '" value="F" ng-model="SocietyRegistration.ElGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" value="M" name="ElGender' + rows + '" ng-model="SocietyRegistration.ElGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
            '<td><div style="width:100%"><select id="ElCategoryId' + rows + '" ng- model="SocietyRegistration.ElCategoryId' + rows + '" class="form-control" ng- init="SocietyRegistration.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
            // '<td><div style="width:100%"><select id="ElPostId' + rows + '" ng-model="SocietyRegistration.ElPostId' + rows + '" class="form-control" ng-init="SocietyRegistration.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
            '<td><label style="display:none" id="lblElAdministrativeFile' + rows + '"></label><a id="LinkPhoto' + rows + '" target="_blank">Photo</a><input type="file" id="ElFile' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyRegistration.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
            '<td><input type= "text" id="ElFHName' + rows + '" name= "FHName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति नाम" ng- model="SocietyRegistration.ElFHName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input type= "text" id="ElAddress' + rows + '" name= "Address" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पता" ng- model="SocietyRegistration.ElAddress' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><label style="display:none" id="lblElMemberPDFForm' + rows + '"></label><a id="LinkDoc' + rows + '" target="_blank">PDF</a><input type="file" id="MemberPDFForm' + rows + '" filetype="PDF" name="File" class="form-control" ng-model="SocietyRegistration.MemberPDFForm' + rows + '" onchange="angular.element(this).scope().ElUploadPDF(this.files,' + rows + ')"></td >' +
            '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
            '</tr>');
        $scope.AddCalender();

        //FIll Category DropDown
        // alert(JSON.stringify($scope.CategoryList));
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language) {
                //  alert(item.CODEID);
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                // alert(item.CODEID);
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        //  alert(ddHTML);
        $("#ElCategoryId" + rows).html(ddHTML);
        //  alert($("#ElCategoryId" + rows).html());
        //END
        //FIll Post DropDown
        var ddHTML = _language ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        //angular.forEach($scope.ChairmanPosts, function (item) {
        //    if (_language) {
        //        ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
        //    }
        //    else {
        //        ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
        //    }
        //});
        //  $("#ElPostId" + rows).html(ddHTML);
        //END

        $('input[type="file"]').unbind('change').bind('change', function () {
            if ($(this)[0].files.length > 0) {
                if (checkExtension($(this).val(), $(this).attr('filetype'))) {
                    var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
                    if ($(this)[0].files[0].name.match(format)) {
                        alert(_language ? "Invalid File" : "अवैध फाइल");
                        $(this).val('');
                        $(this).parent().find('.filesize').text('');
                        if ($(this).attr('filetype').toLowerCase() == "pdf")
                            PDFDataList.splice(PDFDataList.length - 1);
                        else
                            ImageDataList.splice(ImageDataList.length - 1);
                        return;
                    }
                    if (!checkFileLength($(this)[0].files[0].size)) {
                        $(this).parent().find('.filesize').text(formateBitRates($(this)[0].files[0].size));
                    }
                    else {
                        alert(_language ? "File size exceeded.\n Please select files have size less then 1 MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 1 MB कम करें|");
                        $(this).val('');
                        $(this).parent().find('.filesize').text('');
                        if ($(this).attr('filetype').toLowerCase() == "pdf")
                            PDFDataList.splice(PDFDataList.length - 1);
                        else
                            ImageDataList.splice(ImageDataList.length - 1);
                    }
                }
                else {
                    $(this).val('');
                    $(this).parent().find('.filesize').text('');
                    if ($(this).attr('filetype').toLowerCase() == "pdf")
                        PDFDataList.splice(PDFDataList.length - 1);
                    else
                        ImageDataList.splice(ImageDataList.length - 1);
                }
            }
            else {
                alert(_language ? "Please select file." : "फ़ाइल का चयन करें|");
                $(this).parent().find('.filesize').text('');
            }
        });

    };
    $scope.SearchVoter = function () {
        if ($scope.SearchText != '') {
            // $("#myTable2").html("");
            // $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);

            //let data = $scope.EMemberDetailsData.find(ob => ob['AadharNo2'] === $scope.AddNewMember.AadharNo);
            var srno1 = 1;
            $scope.EMemberDetailsData.forEach(function (itm, idnx) {
                if ($scope.SearchText == itm.AadharNo2 && itm.Delete) {
                    itm.IsShow = true;
                    itm.SrNo = srno1++;
                }
                else {
                    itm.IsShow = false;
                }
            });


        } else {
            $scope.EMemberDetailsData.forEach(function (itm, idnx) {
                itm.IsShow = true;
            });
        }
    }

    $scope.CleaSearchVoter = function () {
        $scope.SearchText = "";
        //$("#myTable2").html("");
        //$scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);
        var srno2 = 1;
        $scope.EMemberDetailsData.forEach(function (itm, idnx) {
            itm.IsShow = true;
            if (itm.Delete)
                itm.SrNo = srno2++;

        });
    }
    $scope.EditBasicDetailsForm = function (SocietyIdEnc, ElectionMasterId) {
        $scope.ShowSocietyList = false;
        $scope.ShowVoterList = true;
        $scope.ShowVoterListExcel = false;
        $scope.SocietyIdEnc = SocietyIdEnc;
        $scope.ElectionMasterId = ElectionMasterId;
        //  alert(SocietyIdEnc);
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetVoterMemberDetailsBySocietyId", { "SocietyId": $scope.SocietyIdEnc, "IsCEO": false, "ElectionMasterId": ElectionMasterId, "SearchText": $scope.SearchText }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var EMemberDetails = [];

                EMemberDetails = data;
                EMemberDetails.sort(function (a, b) {
                    return a.UserType - b.UserType
                })
                $scope.EMemberDetailsData = EMemberDetails;
                $scope.TotalMembers = $scope.EMemberDetailsData.length;
                debugger;

                // $scope.IsSubmit = false;
                $scope.IsValid = true;
                $scope.EMemberDetailsData.forEach(function (item, indx) {
                    debugger;
                    item.IsCandidate = item.IsCandidate == true ? true : false;
                    item.IsSubmit = item.IsSubmit == true ? true : false;
                    item.IsValid = item.IsValid == true ? true : false;
                    item.IsSubmitCandidate = item.IsSubmitCandidate == true ? true : false;
                    // item.Photo1 = item.Photo;
                    if (indx == 0) {
                        $scope.SocietyDirectory = item.SocietyDirectory;
                        $scope.IsSubmit = item.IsSubmit;
                        $scope.LastSubmistionDate = item.LastDate + ' ' + item.LastTime;
                        if (!item.IsValid) {
                            // $scope.IsSubmit = false;
                            $scope.IsValid = false;
                            $scope.IsValidMessages = $scope._language == 'en-us' ? 'Submisstion date has paased, so you are not allow to submit member list.' : 'सबमिशन की तारीख निकल चुकी है, इसलिए आपको सदस्य सूची जमा करने की अनुमति नहीं है। |';
                        }
                    }
                });

                $scope.AddCalender();
                // $scope.$apply();

                //for (var k = 0; k < EMemberDetails.length; k++) {

                //    if (k == 0) {

                //        $scope.SocietyRegistration.ElDirectorsName1 = EMemberDetails[k].Name;
                //        $scope.SocietyDirectory = EMemberDetails[k].SocietyDirectory;
                //        $scope.SocietyRegistration.ElContactNo1 = EMemberDetails[k].ContactNo;
                //        $scope.SocietyRegistration.ElAADHAARNo1 = EMemberDetails[k].AadharNo2;
                //        $('#txt_DOB').val(EMemberDetails[k].DOB);
                //        $scope.SocietyRegistration.ElGender1 = EMemberDetails[k].Gender;
                //        $scope.SocietyRegistration.ElCategoryId1 = EMemberDetails[k].CategoryId;
                //        $scope.SocietyRegistration.Address1 = EMemberDetails[k].Address;
                //        $scope.SocietyRegistration.FHName1 = EMemberDetails[k].FHName;

                //        $('#lblElAdministrativeFile1').html(EMemberDetails[k].Photo);
                //        $('#lblMemberPDFForm1').html(EMemberDetails[k].MemberPDFForm);
                //        $('#ElectionMasterId1').val(EMemberDetails[k].ElectionMasterId);

                //        if (EMemberDetails[k].PhotoURL != '') {
                //            $('#LinkPhoto1').attr("href", EMemberDetails[k].PhotoURL);
                //        }
                //        else {
                //            $("#LinkPhoto1").css("display", "none")
                //        }
                //        if (EMemberDetails[k].MemberPDFFormURL != '') {
                //            $('#LinkDoc1').attr("href", EMemberDetails[k].MemberPDFFormURL);
                //        }
                //        else {
                //            $("#LinkDoc1").css("display", "none")
                //        }                     
                //    }
                //    else {
                //        $scope.AddMoreMemberDetailsWithoutValidation();

                //        $('#ElDirectorsName' + (k + 1)).val(EMemberDetails[k].Name);
                //        $('#ElContactNo' + (k + 1)).val(EMemberDetails[k].ContactNo);
                //        $('#ElAADHAARNo' + (k + 1)).val(EMemberDetails[k].AadharNo2);

                //            $('#txt_DOB' + (k + 1)).val(EMemberDetails[k].DOB);
                //            $('input:radio[name=ElGender' + (k + 1) + ']')[0].checked = EMemberDetails[k].Gender == "F" ? true : false;
                //            $('input:radio[name=ElGender' + (k + 1) + ']')[1].checked = EMemberDetails[k].Gender == "M" ? true : false;

                //            $('#ElCategoryId' + (k + 1)).val(EMemberDetails[k].CategoryId);
                //            $('#ElAddress' + (k + 1)).val(EMemberDetails[k].Address);
                //            $('#ElFHName' + (k + 1)).val(EMemberDetails[k].FHName);
                //            $('#lblElMemberPDFForm' + (k + 1)).html(EMemberDetails[k].MemberPDFForm);

                //            $('#lblElAdministrativeFile' + (k + 1)).html(EMemberDetails[k].Photo);
                //            $('#ElectionMasterId' + (k + 1)).val(EMemberDetails[k].ElectionMasterId);
                //            if (EMemberDetails[k].PhotoURL!=''){
                //                $('#LinkPhoto' + (k + 1)).attr("href", EMemberDetails[k].PhotoURL);
                //            }
                //            else {
                //                $('#LinkPhoto' + (k + 1)).css("display", "none")
                //            }

                //            if (EMemberDetails[k].MemberPDFFormURL != '') {
                //                $('#LinkDoc' + (k + 1)).attr("href", EMemberDetails[k].MemberPDFFormURL);
                //            }
                //            else {
                //                $("#LinkDoc" + (k + 1)).css("display", "none")
                //            }

                //    }

                //}
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");

            });
        $scope.AddCalender();
    }

    $scope.SaveSocietyAdministrativeDetails = function (IsSubmit) {
        if ($scope.ValidateSaveData()) {
            //alert(1);
            //  if (confirm("You can't update data after submit")) {
            var SocietyRegistrationData = [];
            var formData = new FormData();
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            // alert(JSON.stringify(ImageDataList));
            if (IsSubmit) {
                var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
                if (!confirm(message)) {
                    return;
                }  
            }
            $(".blockUI").fadeIn("slow");
            //debugger;
            //  var Count = 0;
            //  var tempRowNo = 1;
            //  for (var Y = 0; Y < ImageDataList.length; Y++) {
            //      for (; tempRowNo < ImageDataList[Y].RowNo; tempRowNo++) {
            //          formData.append("MemberTempFile" + (tempRowNo + Count), ImageDataList[Y].File);
            //      }
            //      formData.append("MemberFile" + (ImageDataList[Y].RowNo + Count), ImageDataList[Y].File);
            //      tempRowNo++;
            //  }

            //  var _data2 = PDFDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            ////  alert(JSON.stringify(PDFDataList));

            //   Count = 0;
            //   tempRowNo = 1;
            //  for (var Y = 0; Y < PDFDataList.length; Y++) {
            //      for (; tempRowNo < PDFDataList[Y].RowNo; tempRowNo++) {
            //          formData.append("MemberTempPDFFile" + (tempRowNo + Count), PDFDataList[Y].File);
            //      }
            //      formData.append("MemberPDFFile" + (PDFDataList[Y].RowNo + Count), PDFDataList[Y].File);
            //      tempRowNo++;
            //  }
            var Count = 0;
            var tempRowNo = 1;
            for (var Y = 0; Y < ImageDataList.length; Y++) {
                //for (; tempRowNo < ImageDataList[Y].RowNo; tempRowNo++) {
                //    formData.append("MemberTempFile" + (tempRowNo + Count), ImageDataList[Y].File);
                //}
                //formData.append("MemberFile" + (ImageDataList[Y].RowNo + Count), ImageDataList[Y].File);
                formData.append("MemberFile_" + (ImageDataList[Y].RowNo), ImageDataList[Y].File);
                tempRowNo++;
            }
            var _data2 = PDFDataList.sort(function (a, b) { return a.RowNo - b.RowNo });

            Count = 0;
            tempRowNo = 1;
            for (var Y = 0; Y < PDFDataList.length; Y++) {
                //for (; tempRowNo < PDFDataList[Y].RowNo; tempRowNo++) {
                //    formData.append("MemberTempPDFFile" + (tempRowNo + Count), PDFDataList[Y].File);
                //}
                //formData.append("MemberPDFFile" + (PDFDataList[Y].RowNo + Count), PDFDataList[Y].File);
                formData.append("MemberPDFFile_" + (PDFDataList[Y].RowNo) + "_" + PDFDataList[Y].Type, PDFDataList[Y].File);
                tempRowNo++;
            }
            // alert($scope.SocietyDirectory);

            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            formData.append("IsSubmit", IsSubmit);
            $scope.EMemberDetailsData.forEach(function (item, index) {

                SocietyRegistrationData.push(
                    {
                        //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                        "Name": item.Name,
                        "IsActive": true,
                        "ContactNo": item.ContactNo,
                        "AADHAARNo": item.AadharNo2,
                        "DOB": item.DOB.split("/").reverse().join("-"),
                        "Gender": item.Gender,
                        "CategoryId": item.CategoryId,
                        "ElectionMasterId": item.ElectionMasterId,
                        //"PostId": $(this).find('td:eq(7)').find('select').val(),
                        "UserType": "5",//Nominated member
                        "AdministratorDesignationId": "-1",
                        "IsCEO": false,
                        "RowNO": index,
                        "Photo": item.Photo,
                        "FHName": item.FHName,
                        "Address": item.Address,
                        "MembershipNo": item.MembershipNo,
                        "MemberPDFForm": item.MemberPDFForm == undefined ? "" : item.MemberPDFForm,
                        "IsDelete": item.IsDelete == undefined ? false : item.IsDelete,
                        "AddRemarks": item.AddRemarks == undefined ? "" : item.AddRemarks,
                        "DeleteRemarks": item.DeleteRemarks == undefined ? "" : item.DeleteRemarks
                    });

                //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
            });
            //alert(JSON.stringify(SocietyRegistrationData));
            formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
            $http.post(SiteUrl + "/Election/InsertSocietyMemberDetails", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.indexOf("-2") > -1) {
                        //alert("You can't change the member detail.");
                        //location.reload();
                    }
                    else if (data.indexOf("-1") > -1) {


                        ShowOkMessage("", "Some technical error occur", function () {

                        });
                    }
                    else {
                        $(".blockUI").fadeOut("slow");
                        //alert("Data saved successfully676765756ujytutyuy767");
                        ShowOkMessage("", "Data saved successfully", function () {
                            window.location = "../Election/Voters";
                        });
                        //window.location = "../Election/Voters";
                        //location.reload();
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                });
            //  }
        }

    }
    $scope.AddMemberValiDate = function () {
        debugger;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var Table_Name = "";
        $("#addModal").find(".dvgender").removeClass("notValid");
        $(".validateAddMember").removeClass("notValid");
        if ($scope.AddNewMember.Name == '' || $scope.AddNewMember.Name == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Director Name' : '\n <br /> कृपया निदेशक का नाम दर्ज करें |';
            $("#AddNewMemberName").addClass("notValid");
        }

        if ($scope.AddNewMember.FHName == '' || $scope.AddNewMember.FHName == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Father / Husband Name' : '\n <br /> कृपया पिता / पति नाम दर्ज करें |';
            $("#AddNewMemberFHName").addClass("notValid");
        }

        if ($scope.AddNewMember.ContactNo == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
            $("#AddNewMemberContactNo").addClass("notValid");
        }

        if ($scope.AddNewMember.ContactNo != '') {
            if ($scope.AddNewMember == undefined || $scope.AddNewMember.ContactNo.toString().length != 10) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Mobile Number' : '\n <br /> कृपया सही मोबाइल नंबर दर्ज करें |';
                $("#AddNewMemberContactNo").addClass("notValid");
            }
        }

        // alert($scope.AddNewMember.AadharNo2.length);

        if ($scope.AddNewMember.AadharNo == '' || $scope.AddNewMember.AadharNo == undefined || $scope.AddNewMember.AadharNo.length != 12) {

            message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Aadhaar Number' : '\n <br /> कृपया सही आधार संख्या दर्ज करें |';
            $("#AddNewMemberAadharNo2").addClass("notValid");
        } else {
            let data = $scope.EMemberDetailsData.find(ob => ob['AadharNo2'] === $scope.AddNewMember.AadharNo);
            if (data != null) {
                message += $scope._language == 'en-us' ? '\n <br /> Aadhaar Number Already added' : '\n <br /> आधार नंबर पहले से ही जोड़ा गया है |';
                $("#AddNewMemberAadharNo2").addClass("notValid");
            }

        }
        //$scope.EMemberDetailsData

        if ($scope.AddNewMember.CategoryId == "0" || $scope.AddNewMember.CategoryId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Category' : '\n <br /> कृपया वर्ग का चयन करें |';
            $("#AddNewMemberCategoryId").addClass("notValid");
        }


        if ($scope.AddNewMember.Address == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्जा करे |';
            $("#AddNewMemberAddress").addClass("notValid");
        }


        if ($("#AddMemberPhoto").val() == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Photo of Member.' : '\n <br /> कृपया सदस्य का फोटो चुनें |';
            $("#AddMemberPhoto").addClass("notValid");
        }

        if ($("#AddMemberPDFForm1").val() == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select pdf for Add Member.' : '\n <br /> कृपया सदस्य जोड़ें के लिए पीडीएफ का चयन करें |';
            $("#AddMemberPDFForm1").addClass("notValid");
        }

        if ($scope.AddNewMember.Gender == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Gender Type.' : '\n <br /> कृपया लिंग का चयन करे |';
            $("#addModal").find(".dvgender").addClass("notValid");
        }

        if ($("#txtAddDOB2").val() == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Date Of Birth.' : '\n <br /> कृपया जन्म की तारीख का चयन करे |';
            $("#txtAddDOB2").addClass("notValid");
        }

        if ($("#addNewDelete").is(":checked")) {
            if ($scope.AddNewMember.DeleteRemarks == "") {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter remark for member delete.' : '\n <br /> कृपया सदस्य हटाने के लिए टिप्पणी दर्ज करें |';
                $("#addDeleteMember").addClass("notValid");
            }
            if ($("#addDeleteMember").val() == "") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select pdf for delete Member.' : '\n <br /> कृपया सदस्य हटाने के लिए पीडीएफ का चयन करें |';
                $("#AddNewMemberDeleteRemarks").addClass("notValid");
            }
        }


        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        }
        return true;
    }

    $scope.ValidateSaveData = function (tableflag) {
       // debugger;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#myTable2").find("input,select").removeClass("notValid");
        $("#myTable2").find(".dvgender").removeClass("notValid");
        $scope.EMemberDetailsData.forEach(function (item, index) {
          //  debugger;
            if (item.IsDelete == false || item.IsDelete == 'False') {
                if (item.Name == '' || item.Name == undefined) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Director Name' : 'कृपया निदेशक का नाम दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Director Name' : '\n <br /> कृपया निदेशक का नाम दर्ज करें |';
                    }
                    $("#myTable2").find("tr").eq(index).find("input").eq(0).addClass("notValid");
                }

                if (item.FHName == '' || item.FHName == undefined) {

                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Father / Husband Name' : 'कृपया पिता / पति नाम दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Father / Husband Name' : '\n <br /> कृपया पिता / पति नाम दर्ज करें |';
                    }

                    $("#myTable2").find("tr").eq(index).find("input").eq(7).addClass("notValid");
                }

                if (item.ContactNo == '') {

                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Mobile Number' : 'कृपया मोबाइल नंबर दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
                    }

                    $("#myTable2").find("tr").eq(index).find("input").eq(1).addClass("notValid");
                }

                if (item.ContactNo != '') {
                    if (item.ContactNo == undefined || item.ContactNo.toString().length != 10) {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter valid Mobile Number' : 'कृपया सही मोबाइल नंबर दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Mobile Number' : '\n <br /> कृपया सही मोबाइल नंबर दर्ज करें |';
                        }
                        $("#myTable2").find("tr").eq(index).find("input").eq(1).addClass("notValid");
                    }
                }

                //alert(item.AadharNo2);

                if (item.AadharNo == '' || item.AadharNo == undefined || item.AadharNo.toString().length != 12) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter valid Aadhaar Number' : 'कृपया सही आधार संख्या दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Aadhaar Number' : '\n <br /> कृपया सही आधार संख्या दर्ज करें |';
                    }
                    $("#myTable2").find("tr").eq(index).find("input").eq(2).addClass("notValid");
                }


                if (item.CategoryId == "0" || item.CategoryId == undefined) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Category' : 'कृपया वर्ग का चयन करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Category' : '\n <br /> कृपया वर्ग का चयन करें |';
                    }
                    $("#myTable2").find("tr").eq(index).find("select").eq(0).addClass("notValid");
                }


                if (item.Address == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Address' : 'कृपया पता दर्जा करे') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्जा करे |';
                    }
                    $("#myTable2").find("tr").eq(index).find("input").eq(8).addClass("notValid");
                }
                if (item.Gender == "") {
                    if (message.indexOf($scope._language == 'en-us' ? ' Please Select Gender Type' : 'कृपया लिंग का चयन करे') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Gender Type.' : '\n <br /> कृपया लिंग का चयन करे |';
                    }
                    $("#myTable2").find("tr").eq(index).find(".dvgender").addClass("notValid");
                    //$(".validateAddMember").addClass("notValid");dvgender
                }
            }
            //if ($scope.EMemberDetailsData.length = index + 1) {
            //    if (message.length > 29) {
            //        ShowModelAlert("Add Member", message);
            //        return false;
            //    } else {
            //        ShowModelAlert("Add Member", "hjk");
            //        return true;
            //    }
            //}
        });
        //if ($scope.EMemberDetailsData.length = index + 1) {
        if (message.length > 29) {
            ShowModelAlert("Add Member", message);
            return false;
        } //else {
        //ShowModelAlert("Add Member", "hjk");
        return true;
        //}
        //}
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

            if ($(this).find('td:eq(8)').find('input').val() == '' || $(this).find('td:eq(8)').find('input').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Father / Husband Name' : '\n <br /> कृपया पिता / पति नाम दर्ज करें |';

                var jElement = $(this).find('td:eq(8)').find('input');
                jElement.css('border', 'solid 1px red');
            }
            if ($(this).find('td:eq(9)').find('input').val() == '' || $(this).find('td:eq(9)').find('input').val() == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्ज करें |';

                var jElement = $(this).find('td:eq(9)').find('input');
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


                //var Post = $(this).find('td:eq(8)');
                //var PostId = Post.find('option:selected').val();

                //if (PostId == 0) {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Post' : '\n <br /> कृपया पद का चयन करें |';

                //    var jElement = $(this).find('td:eq(8)').find('select');
                //    jElement.css('border', 'solid 1px red');
                //}
                //else {
                //    var jElement = $(this).find('td:eq(8)').find('select');
                //    jElement.css('border', '1px solid #ccc');
                //}
            }
            else {
                //var Post = $(this).find('td:eq(7)');
                //var PostId = Post.find('option:selected').val();

                //if (PostId == 0) {
                //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Post' : '\n <br /> कृपया पद का चयन करें |';

                //    var jElement = $(this).find('td:eq(7)').find('select');
                //    jElement.css('border', 'solid 1px red');
                //}
                //else {
                //    var jElement = $(this).find('td:eq(7)').find('select');
                //    jElement.css('border', '1px solid #ccc');
                //}
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
    $scope.OnPageLoad = function () {
        $scope.AddCalender();
        $(".blockUI").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    alert("Error while translation");
                });
        }
        $scope.ClearClassForm = function () {
            $scope.Classes = '';
        };
        $('#txt_DOB').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D',
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
            yearRange: "-100:-18"
        });

        //Get Chairman Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Post list")

            });
        //Get Chairman Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.ChairmanPosts = data;
                //  $scope.EditBasicDetailsForm(1);
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown


        //Get Board Member Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-2").
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.BoardPosts = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown


        // END : Post List for dropdown

        $scope.GetElectionSocietyListData();
    }


    $scope.GetElectionSocietyListData = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = {"TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Election/GetElectionSocietyListData_New", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $scope.ElectionSocietyListData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }

    $scope.ElUploadAdministrative = function (file, row) {
        // alert(JSON.stringify(ImageDataList));
        //for (var temRowNo = 0; temRowNo < ImageDataList.length; temRowNo++) {
        //    if (ImageDataList[temRowNo].RowNo == row) {
        //        ImageDataList.splice(temRowNo);
        //    }
        //}
        var fileUpload = $(file).get(0);
        var files = fileUpload.files;

        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = files[0];
        FileDataWithRowObj.RowNo = row;
        ImageDataList.push(FileDataWithRowObj);
        //  alert(JSON.stringify(ImageDataList));
    }

    $scope.ElUploadPDF = function (file, row, type, index) {
        //  alert(JSON.stringify(PDFDataList));
        debugger;
        //for (var temRowNo1 = 0; temRowNo1 < PDFDataList.length; temRowNo1++) {
        //    if (PDFDataList[temRowNo1].RowNo == row) {
        //        PDFDataList.splice(temRowNo1);
        //    }
        //}

        var fileUpload = $(file).get(0);
        var files = fileUpload.files;



        var FileDataWithRowObj = new FileDataWithRow();
        FileDataWithRowObj.File = files[0];// file[0];
        FileDataWithRowObj.RowNo = row;
        FileDataWithRowObj.Type = type;
        FileDataWithRowObj.Index = index;
        PDFDataList.push(FileDataWithRowObj);
        // alert(JSON.stringify(PDFDataList));
    }

    $scope.OnPageLoad();
    $scope.RemoveRowAdminiAct2001 = function (index) {
        //$scope.BulkInsertFile = file[0];
        $scope.ReSetAddModel();
        $("#deleteModal").modal("show");

        $scope.AddNewMember.Index = index;
    };

    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    }
    // START : Save Bulk Insert
    $scope.SaveBulkInsert = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFile == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Bulk Insert", message);
                return false;
            }
        }
        // alert(11);
        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        $http.post(SiteUrl + "/Election/InsertSocityVoterMemberByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //   alert(1);
                ////////if (data == '1') {
                ////////    ShowModelAlert("Bulk Insert of Socity Voter Member", 'ExcelData Save Sucessfully');
                ////////    $('#fluBulkInsert').val('');
                ////////    $('#fluBulkInsert').parent().find('.filesize').text('');
                ////////}
                //////////ShowModelAlert("Bulk Insert", data);
                // alert(JSON.stringify(data));
                EMemberDetails = data;
                //EMemberDetails = data.filter(x => x.AddressFor == '0');
                //EMemberDetails.sort(function (a, b) {
                //    return a.UserType - b.UserType
                //})

                $scope.ShowSocietyList = false;
                $scope.ShowVoterList = true;
                $scope.ShowVoterListExcel = false;
                //   alert(1);
                $("#myTable2").html("");
                //  alert(2);
                $("#myTable2 > tbody").html("");
                //   alert(3);
                for (var k = 0; k < EMemberDetails.length; k++) {

                    $scope.AddNewMember.Name = EMemberDetails[k].Name;
                    $scope.AddNewMember.ContactNo = EMemberDetails[k].ContactNo;
                    $scope.AddNewMember.AadharNo = EMemberDetails[k].AadharNo;
                    $scope.AddNewMember.DOB = EMemberDetails[k].SmallDOB;
                    $scope.AddNewMember.Gender = EMemberDetails[k].Gender;
                    $scope.AddNewMember.Category = EMemberDetails[k].Category;
                    $scope.AddNewMember.Photo = EMemberDetails[k].Photo;
                    $scope.AddNewMember.ElectionMasterId = $scope.ElectionMasterId;
                    $scope.AddNewMember.Address = EMemberDetails[k].Address;
                    $scope.AddNewMember.FHName = EMemberDetails[k].FHName;
                    $scope.AddNewMember.DOB = EMemberDetails[k].MemberPDFForm;
                    $scope.AddNewMember.DOB = $("#txtAddDOB2").val();
                    $scope.AddNewMember.DOB = $("#txtAddDOB2").val();
                    $scope.AddNewMember.DOB = $("#txtAddDOB2").val();
                    $scope.AddNewMember.DOB = $("#txtAddDOB2").val();

                    $scope.AddNewMember.DOB = $("#txtAddDOB2").val();
                    $scope.AddNewMember.CODEID = $("#txtAddDOB2").val();
                    $scope.AddNewMember.AadharNo2 = $scope.AddNewMember.AadharNo;
                    $scope.AddNewMember.Delete = true;
                    $scope.AddNewMember.SrNo = $scope.EMemberDetailsData.length + 1;
                    $scope.AddNewMember.ElectionMasterId = $scope.EMemberDetailsData[0].ElectionMasterId;
                    $scope.EMemberDetailsData.push($scope.AddNewMember);
                    $scope.ReSetAddModel();



                    $scope.AddMoreMemberDetailsWithoutValidation();
                    //  alert(EMemberDetails[k].Name);
                    $('#ElDirectorsName' + (k + 1)).val(EMemberDetails[k].Name);
                    $('#ElContactNo' + (k + 1)).val(EMemberDetails[k].ContactNo);
                    $('#ElAADHAARNo' + (k + 1)).val(EMemberDetails[k].AadharNo);
                    //    alert($('#ElDirectorsName' + (k + 1)).val());
                    //    alert(EMemberDetails[k].SmallDOB + "---" + EMemberDetails[k].DOB);
                    $('#txt_DOB' + (k + 1)).val(EMemberDetails[k].SmallDOB);
                    $('input:radio[name=ElGender' + (k + 1) + ']')[0].checked = EMemberDetails[k].Gender == "F" ? true : false;
                    $('input:radio[name=ElGender' + (k + 1) + ']')[1].checked = EMemberDetails[k].Gender == "M" ? true : false;

                    if (EMemberDetails[k].Category == "ST") {
                        $('#ElCategoryId' + (k + 1)).val(1);
                    }
                    else if (EMemberDetails[k].Category == "SC") {
                        $('#ElCategoryId' + (k + 1)).val(2);
                    }
                    else if (EMemberDetails[k].Category == "GEN") {
                        $('#ElCategoryId' + (k + 1)).val(3);
                    }
                    else if (EMemberDetails[k].Category == "OBC") {
                        $('#ElCategoryId' + (k + 1)).val(4);
                    }
                    //  $('#ElPostId' + (k + 1)).val(EMemberDetails[k].PostId);
                    $('#lblElAdministrativeFile' + (k + 1)).html(EMemberDetails[k].Photo);
                    $('#ElectionMasterId' + (k + 1)).val($scope.ElectionMasterId);
                    $('#ElAddress' + (k + 1)).val(EMemberDetails[k].Address);
                    $('#ElFHName' + (k + 1)).val(EMemberDetails[k].FHName);
                    $('#lblElMemberPDFForm' + (k + 1)).html(EMemberDetails[k].MemberPDFForm);
                    $("#LinkDoc" + (k + 1)).css("display", "none");
                    $("#LinkPhoto" + (k + 1)).css("display", "none");
                    //// }

                }

            }).
            error(function (data, status) {
                ShowModelAlert("Bulk Insert of Socity Voter Member", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Society Profiling

    $scope.ShowUploadExcel = function () {
        $scope.ShowSocietyList = false;
        $scope.ShowVoterList = false;
        $scope.ShowVoterListExcel = true;
        //  alert($scope.SocietyIdEnc);
        $scope.SocietyIdEnc = $scope.SocietyIdEnc;
    }
     
});


AppClasses.directive('callcalender', function ($timeout) {
    return function (scope, element, attrs) {
        if (scope.$last) {
            //scope.AddCalender();
            $('.AddDate').datepicker({
                showOn: "both",
                changeMonth: true,
                buttonImage: "../images/calender.png",
                changeYear: true,
                dateFormat: "dd/mm/yy",
                maxDate: '0D',
                defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
                yearRange: "-100:-18",
                onSelect: function (date) {
                    //ngModelCtrl.$setViewValue(date);
                    scope.$apply();
                }
            });
        }
    };
});

AppClasses.filter('split', function () {
    return function (input, splitChar, splitIndex) {
        // do some bounds checking here to ensure it has that index
        return input.split(splitChar)[splitIndex];
    }
});
//{ { test | split: ',': 0 } }
//{ { test | split: ',': 0 } }