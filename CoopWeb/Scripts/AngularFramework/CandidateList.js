var AppClasses = angular.module("CandidateListMod", ['angularUtils.directives.dirPagination']);

var ImageDataList = [];
var PDFDataList = [];
function FileDataWithRow() {
    this.File = '';
    this.RowNo = '';
    this.Flag = '';
    this.FileName = '';
}
AppClasses.controller("CandidateListCnt", function ($scope, $http, $rootScope) {
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
    $scope.ElctionVoterListData = [];
    $scope.ShowSocietyList = true;
    $scope.ElectionMasterId = 0;
    $scope.ShowVoterList = false;
    $scope.EMemberDetailsData = [];
    $scope.SearchText = "";
    $scope.IsSave = false;
    $scope.IsSubmit = false;
    $scope.IsValid = false;
    $scope.IsValidMessages = "";
    $scope.AllowEdit = false;
    //$scope.IsPublish = false;
    $scope.TotalMembers = 0;
    $scope.LastSubmistionDate = "";

    $scope.AddMoreMemberDetailsWithoutValidation = function () {
        //  $(".clsShowHide").hide();
        var rows = $('#myTable2 tr').length + 1;
        for (; typeof $("#lblElAdministrativeFile" + rows + "").html() !== "undefined";) {
            rows = rows + 1;
        }
        $('#myTable2').append('<tr>' +
            '<td>' + rows + '</td>' +
            ' <td>   <input type="checkbox" id="ckbIsProccess' + rows + '"> </td>' +
            '<td><input type= "text" disabled="disabled" id="ElDirectorsName' + rows + '" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम" ng- model="SocietyCandidate.ElDirectorsName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input type="text" disabled="disabled" id="ElContactNo' + rows + '" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर" ng-model="SocietyCandidate.ElContactNo' + rows + '" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
            '<td><input type="text" disabled="disabled" id="ElAADHAARNo' + rows + '" name="AdhareNo" class="form-control" placeholder="आधार संख्या" ng-model="SocietyCandidate.ElAADHAARNo1' + rows + '" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
            '<td><input disabled="disabled" id="txt_DOB' + rows + '" name="DOB' + rows + '" disabled="disabled" class="form-control AddDate" ng-model="SocietyCandidate.ElDOB' + rows + '" placeholder="जन्म की तारीख" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off">' +
            '<td><div style="width:100%"><label class="help-block-gender ng-binding" style= "width:40% !important;" > <input type="radio" disabled="disabled" name="ElGender' + rows + '" value="F" ng-model="SocietyCandidate.ElGender' + rows + '" class="ng-pristine ng-valid ng-empty ng-touched" style="border: 1px solid rgb(204, 204, 204);">महिला </label>&nbsp;&nbsp;<label class="help-block-gender ng-binding" style="width:40% !important;"> <input type="radio" disabled="disabled" value="M" name="ElGender' + rows + '" ng-model="SocietyCandidate.ElGender' + rows + '" class="ng-pristine ng-untouched ng-valid ng-empty">पुरुष</label></td ></div >' +
            '<td><div style="width:100%"><input type="hidden" id="MemberId' + rows + '" name="MemberId" ng-model="SocietyCandidate.MemberId' + rows + '" ><select disabled="disabled" id="ElCategoryId' + rows + '" ng- model="SocietyCandidate.ElCategoryId' + rows + '" class="form-control" ng- init="SocietyCandidate.CategoryId' + rows + '="0" " ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
            // '<td><div style="width:100%"><select id="ElPostId' + rows + '" ng-model="SocietyCandidate.ElPostId' + rows + '" class="form-control" ng-init="SocietyCandidate.PostId' + rows + '="0""><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
            '<td><label style="display:none" id="lblElAdministrativeFile' + rows + '"></label><a id="LinkPhoto' + rows + '" target="_blank">Photo</a><input disabled="disabled" type="file" id="ElFile' + rows + '" filetype="image" name="File" class="form-control" ng-model="SocietyCandidate.AdministrativeFile' + rows + '" onchange="angular.element(this).scope().ElUploadAdministrative(this.files,' + rows + ')"></td >' +
            '<td><input  disabled="disabled"  type= "text" id="ElFHName' + rows + '" name= "FHName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पिता / पति नाम" ng- model="SocietyRegistration.ElFHName' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><input  disabled="disabled"  type= "text" id="ElAddress' + rows + '" name= "Address" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "पता" ng- model="SocietyRegistration.ElAddress' + rows + '" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
            '<td><label style="display:none" id="lblElCandidatePDFForm' + rows + '"></label><a id="LinkDoc' + rows + '" target="_blank">PDF</a><input type="file" id="CandidatePDFForm' + rows + '" filetype="PDF" name="File" class="form-control" ng-model="SocietyRegistration.CandidatePDFForm' + rows + '" onchange="angular.element(this).scope().ElUploadPDF(this.files,' + rows + ')"></td >' +
            //  '<td><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
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
                        //  alert(JSON.stringify(PDFDataList));
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
                        // alert(JSON.stringify(PDFDataList));
                        return;
                    }
                }
                else {
                    $(this).val('');
                    $(this).parent().find('.filesize').text('');
                    if ($(this).attr('filetype').toLowerCase() == "pdf")
                        PDFDataList.splice(PDFDataList.length - 1);
                    else
                        ImageDataList.splice(ImageDataList.length - 1);
                    // alert(JSON.stringify(PDFDataList));
                    return;
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

 
    $scope.FileType = "";

    $scope.AddFile1 = function (type,id) {
        $scope.FileType = type;
       // $scope.AddNewMember.IsUploadFileName = true;
       $scope.AddNewMember.IsUpload = false;
    }
    $scope.ChangeFile1 = function (type,id ) {
        if (type == 1) {
            $scope.AddNewMember.IsUploadFileName = false;
            $scope.AddNewMember.FileName = "";
           $scope.AddNewMember.IsUpload = false;
        }
    }
  
    $scope.UploadFile_new = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("MemberFile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Election/UploadNominationForm", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr1 = function (url, data, type) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        if (type == 1) {
                            $scope.AddNewMember.FileName = request.responseText;
                            $scope.AddNewMember.IsUpload = true;
                        }
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
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }



    $scope.SelectCandidate = function (th) {

        //alert(th.val());
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
    $scope.ShowNominationFormModal = function (item) {
     
        $("#addNominationFformModal").modal("show");
        $scope.SocietyIdForAdministrativeDetails = item.SocietyIdEnc;
        $scope.AddNewMember.Name = item.Name;
        $scope.AddNewMember.ContactNo = item.ContactNo;
        $scope.AddNewMember.AadharNo = item.AadharNo2;
        $scope.AddNewMember.DOB = item.DOB;

        $scope.AddNewMember.Gender = item.Gender;
        $scope.AddNewMember.CategoryId = item.CategoryId;
        $scope.AddNewMember.ElectionMasterId = item.ElectionMasterId;

        $scope.AddNewMember.Photo = item.Photo;
        $scope.AddNewMember.FHName = item.FHName;
        $scope.AddNewMember.Address = item.Address;
        $scope.AddNewMember.MembershipNo = item.MembershipNo;
        $scope.AddNewMember.MemberPDFForm = item.MemberPDFForm;
        $scope.AddNewMember.IsDelete = item.IsDelete;
        $scope.AddNewMember.AddRemarks = item.AddRemark;
        $scope.AddNewMember.DeleteRemarks = item.DeleteRemark;
        $scope.AddNewMember.MemberId = item.MemberId;

        $scope.AddNewMember.IsUploadFileName = false;
        $scope.AddNewMember.FileName = "";
        $scope.AddNewMember.IsUpload = false;


    }

    $scope.EditBasicDetailsForm = function (SocietyIdEnc, ElectionMasterId) {
      
        $scope.ShowSocietyList = false;
        $scope.ElectionMasterId = ElectionMasterId;
        $scope.ShowVoterList = true;
        $scope.SocietyIdEnc = SocietyIdEnc;
        $(".blockUI").fadeIn("slow");



        $scope.GetElectionDate().then(function (DateData) {
            $http.post(SiteUrl + "/Election/GetVoterMemberDetailsBySocietyId", { "SocietyId": $scope.SocietyIdEnc, "IsCEO": false, "ElectionMasterId": ElectionMasterId, StepType: 3 }).
                success(function (data, status) {
                    // var MemberDetails = [];
                    var EMemberDetails = [];
                    EMemberDetails = data;
                 
                    $scope.EMemberDetailsData = data;//.filter(x => x.PostId>0);
                    $scope.AddCalender();
                                   

                    $scope.TotalMembers = $scope.EMemberDetailsData.length;
                    $("#addExcelModal").modal("hide");
                    $scope.ReSetAddModel();
                    $scope.AddCalender();
                    $(".blockUI").fadeOut("slow");                    
                }).
                error(function (data, status) {
                    alert("Error while fetching Society Details.");
                    $(".blockUI").fadeOut("slow");
                });

        });



    }



    $scope.GetElectionDate = function () {

        return $http.post(SiteUrl + "/Election/GetElectionCandidateDate", { "SocietyId": $scope.SocietyIdEnc, "IsCEO": false, "ElectionMasterId": $scope.ElectionMasterId, StepType: 3 }).
            success(function (data, status) {

                $scope.DateData = JSON.parse(data.DateData);
                $scope.DateData.forEach(function (item, indx) {
           
                    $scope.IsValid = true;
                    //item.IsCandidate = item.IsCandidate.toLowerCase() == 'true' ? true : false;
                    //item.IsSubmit = item.IsSubmit.toLowerCase() == 'true' ? true : false;
                    //item.IsValid = item.IsValid.toLowerCase() == 'true' ? true : false;
                    ////item.IsSubmitCandidate = item.IsSubmitCandidate.toLowerCase() == 'true' ? true : false;
                    //item.IsSave = item.IsSave.toLowerCase() == 'true' ? true : false;
                    if (indx == 0) {
                        $scope.SocietyDirectory = item.SocietyDirectory;
                        $scope.IsSubmit = item.IsSubmit;
                        $scope.LastSubmistionDate = item.LastDate + ' ' + item.StartTime + ' to ' + item.LastTime;
                        $scope.IsSave = item.IsSave;
                        $scope.AllowEdit = item.AllowAdd;
                        if (!item.IsValid) {
                            $scope.IsValid = false;
                            $scope.IsValidMessages = $scope._language == 'en-us' ? 'Submisstion date has paased, so you are not allow to submit member list.' : 'सबमिशन की तारीख निकल चुकी है, इसलिए आपको सदस्य सूची जमा करने की अनुमति नहीं है। |';
                        }
                    }
                });  

                //return JSON.stringify(data);
            }).
            error(function (data, status) {
                return "";
              
            });

    }


    $scope.SaveSocietyAdministrativeDetails = function (IsSubmit) {
     
        if ($scope.TableValiDate(3)) {
            var CandidatePDFFormfile = false;
            var message = "";
            var SocietyCandidateData = [];
            var formData = new FormData();
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            if (IsSubmit) {
                var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
                if (!confirm(message)) {
                    return;
                }
            }
            $(".blockUI").fadeIn("slow");
            formData.append("IsSubmit", IsSubmit);
            formData.append("StepType", "3");
            var imgindx = 0;
            $("#myTable2").find('input[type="file"]').each(function (indx) {
                if ($scope.EMemberDetailsData[indx].IsCandidate && $scope.EMemberDetailsData[indx].IsCandidate != 'False') {

                    var fileUpload = $(this).get(0);
                    var files = fileUpload.files;

                    formData.append("MemberPDFFile_" + (imgindx + 1), files[0]);
                    ++imgindx;
                }
            });
          
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.EMemberDetailsData.forEach(function (item, index) {
              
                if ($scope.EMemberDetailsData[index].IsCandidate == true) {
                
                    if (item.CandidatePDFForm != null) {
                        if (IsSubmit == true) {
                            $scope.ActivityStatus = 'Candidate Publish ' + item.Name + '  by Election Officer';
                        }
                        else {
                            $scope.ActivityStatus = 'Candidate Added ' + item.Name + '  by Election Officer';
                        }
                        SocietyCandidateData.push(
                            {

                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "Name": item.Name,
                                "MemberId": item.MemberId,
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
                                "IsCandidate": item.IsCandidate,
                                "CandidatePDFForm": item.CandidatePDFForm == undefined ? "" : item.CandidatePDFForm,
                                "ActivityStatus": $scope.ActivityStatus
                                //"StepType": 3

                            });
                    }
                    else {
                        CandidatePDFFormfile = true;
                        message += $scope._language == 'en-us' ? 'Please upload nomination form of '+' '+item.Name+' ' + '.<br />' : '<br />कृपया ' + item.Name + 'का  नामांकन प्रपत्र अपलोड करें ।';
                    }
                }
                  
                  });



            if (CandidatePDFFormfile == true) {
                $(".blockUI").fadeOut("slow");
                if (message.length > 29) {
                    ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
                    return false;
                }
                return true;
            }
            else {
                formData.append("DataArray", JSON.stringify(SocietyCandidateData));
                $http.post(SiteUrl + "/Election/UpdateSocietyMemberDetails", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                        if (data.indexOf("-2") > -1) {
                            alert("You can't change the member detail.");
                            location.reload();
                        }
                        else if (data.indexOf("-1") > -1) {
                            alert("Some technical error occur");
                        }
                        else {
                            alert("Data saved successfully");
                            location.reload();
                        }
                    }).
                    error(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                    });

            }
            //alert(JSON.stringify(SocietyCandidateData));
        
            // }
        }

    }

    $scope.TableValiDate = function (tableflag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';



        //$("#myTable2").find('input[type="file"]').removeClass("notValid");
        //if ($("#myTable2").find('input[type="checkbox"]:checked').length == 0) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Candidate.' : '\n <br />कृपया उम्मीदवार का चयन करें।';
        //}

        $scope.EMemberDetailsData.forEach(function (item, index) {
            if (item.IsCandidate) {
                if ($("#myTable2").find('input[type="file"]').eq(index).val() == "" && item.CandidatePDFForm == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please upload nomination form' : 'कृपया नामांकन फॉर्म अपलोड करें') < 0) {
                        message += $scope._language == 'en-us' ? '\n <br /> Please upload nomination form.' : '\n <br />कृपया नामांकन फॉर्म अपलोड करें।';
                    }
                    $("#myTable2").find("tr").eq(index).addClass("notValidTr");
                }

                //if (item.Photo == '' && item.Photo1 == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please upload member photo' : 'कृपया सदस्य फोटो अपलोड करें') < 0) {
                //        message += $scope._language == 'en-us' ? '\n <br /> Please upload member photo.' : '\n <br />कृपया सदस्य फोटो अपलोड करें।';
                //    }
                //    $("#myTable2").find("tr").eq(index).addClass("notValidTr");
                //}


            }

        });


        if (message.length > 29) {
            ShowModelAlert("Candidate List", message);
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
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        //Get Chairman Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.ChairmanPosts = data;
                //  $scope.EditBasicDetailsForm(1);
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

        $(".se-pre-con").fadeOut("slow");
        // END : Post List for dropdown

        $scope.GetElectionSocietyListData();
    }


    $scope.GetElectionSocietyListData = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Election/GetElectionSocietyListData_New", JSON.stringify({ Details: jsonParameters, StepType: 3 })).
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

    $scope.AddMoreMemberDetails = function () {
        //if ($scope.TableValiDate(3)) {
        //  if ($scope.ValidateSaveData()) {
        $("#addModal").modal("show");
        $scope.ReSetAddModel();
        $scope.AddCalender();
        return;
        // }
    };

    $scope.AddExcelModalShow = function () {
        //if ($scope.TableValiDate(3)) {
        //  if ($scope.ValidateSaveData()) {
        $("#addExcelModal").modal("show");
        $scope.ReSetAddModel();
        $scope.AddCalender();
        return;
        // }
    };

    $scope.ReSetAddModel = function () {

        $(".validateAddMember").removeClass("notValid");

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

    $scope.AddNewMemberRow = function () {
 
        $(".validateAddMember").removeClass("notValid");
        if ($scope.AddMemberValiDate()) {
           // $scope.AddNewMember.DOB = $("#txtAddDOB2").val();
            $scope.AddNewMember.CODEID = $("#txtAddDOB2").val();
            $scope.AddNewMember.AadharNo2 = $scope.AddNewMember.AadharNo;
            $scope.AddNewMember.Delete = true;
            $scope.AddNewMember.SrNo = $scope.EMemberDetailsData.length + 1;
            $scope.AddNewMember.Photo1 = newMemberPhoto;
            $scope.AddNewMember.PhotoURL = newMemberPhoto;
            $scope.AddNewMember.ElectionMasterId = $scope.ElectionMasterId;
            //$scope.EMemberDetailsData.push($scope.AddNewMember);



            //
            $("#addModal").find('input[type="file"]').each(function (indx) {
           
                if ($(this).attr('filetype').toLowerCase() == "pdf") {
                    if ($(this)[0].files.length > 0) {
                        $scope.ElUploadPDF(this, $scope.EMemberDetailsData.length, $(this).attr('dat-val'), indx);
                    }
                } else {
                    if ($(this)[0].files.length > 0) {
                        $scope.ElUploadAdministrative(this, $scope.EMemberDetailsData.length);
                    }
                }
            });

            $scope.AddCandidate(false);


            //$("#addModal").modal("hide");
            //$scope.ReSetAddModel();
        }
    }

    $scope.AddCandidate = function (IsSubmit) {

       
        //  if (confirm("You can't update data after submit")) {
        var SocietyRegistrationData = [];
        var formData = new FormData();
        $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
        var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
      
        if (IsSubmit) {
            var message = $scope._language == 'en-us' ? 'Are you sure you want to submit member list. ! After submit you are not able to add or edit member.' : 'क्या आप वाकई सदस्य सूची जमा करना चाहते हैं? ! सबमिट करने के बाद आप सदस्य को जोड़ने या संपादित करने में सक्षम नहीं हैं। |';
            if (!confirm(message)) {
                return;
            }
        }
        $(".blockUI").fadeIn("slow");

        var Count = 0;
        var tempRowNo = 1;
        for (var Y = 0; Y < ImageDataList.length; Y++) {

            formData.append("MemberFile_1", ImageDataList[Y].File);
            tempRowNo++;
        }
        var _data2 = PDFDataList.sort(function (a, b) { return a.RowNo - b.RowNo });

        Count = 0;
        tempRowNo = 1;
        for (var Y = 0; Y < PDFDataList.length; Y++) {

            formData.append("MemberPDFFile_1", PDFDataList[Y].File);
            tempRowNo++;
        }


        formData.append("SocietyDirectory", $scope.SocietyDirectory);
        formData.append("IsSubmit", IsSubmit);
        // $scope.EMemberDetailsData.forEach(function (item, index) {

        $scope.ActivityStatus = $scope.AddNewMember.Name + ' ' +'Nomination form upload by election officer';


        SocietyRegistrationData.push(
            {
                "MemberId": $scope.AddNewMember.MemberId,
                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                "Name": $scope.AddNewMember.Name,
                "IsActive": true,
                "ContactNo": $scope.AddNewMember.ContactNo,
                "AADHAARNo": $scope.AddNewMember.AadharNo2,
                "DOB": $scope.AddNewMember.DOB.split("/").reverse().join("-"),
                "Gender": $scope.AddNewMember.Gender,
                "CategoryId": $scope.AddNewMember.CategoryId,
                "ElectionMasterId": $scope.AddNewMember.ElectionMasterId,

                "UserType": "5",//Nominated member
                "AdministratorDesignationId": "-1",
                "IsCEO": false,
                "RowNO": 0,
                "Photo": $scope.AddNewMember.Photo,
                "FHName": $scope.AddNewMember.FHName,
                "Address": $scope.AddNewMember.Address,
                "MembershipNo": $scope.AddNewMember.MembershipNo,
                "MemberPDFForm": $scope.AddNewMember.MemberPDFForm == undefined ? "" : $scope.AddNewMember.MemberPDFForm,
                "IsDelete": $scope.AddNewMember.IsDelete == undefined ? false : $scope.AddNewMember.IsDelete,
                "AddRemarks": $scope.AddNewMember.AddRemarks == undefined ? "" : $scope.AddNewMember.AddRemarks,
                "DeleteRemarks": $scope.AddNewMember.DeleteRemarks == undefined ? "" : $scope.AddNewMember.DeleteRemarks,

                "Presentedby": $scope.AddNewMember.Presentedby,
                "NewMemberName": $scope.AddNewMember.NewMemberName,
                "NewRemark": $scope.AddNewMember.NewRemark,
                "MemberPDFForm": $scope.AddNewMember.FileName,
                "ActivityStatus": $scope.ActivityStatus

            });
        //});

        formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
        $http.post(SiteUrl + "/Election/InsertElectionCandidateDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                if (data.indexOf("-2") > -1) {

                }
                else if (data.indexOf("-1") > -1) {
                    ShowOkMessage("", "Some technical error occur", function () {

                    });
                }
                else {
                    $(".blockUI").fadeOut("slow");
                    $("#addModal").modal("hide");
                    $scope.ReSetAddModel();
                    ShowOkMessage("", "Data saved successfully", function () {
                        ImageDataList = [];
                        PDFDataList = [];
                        $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);
                        $("#addNominationFformModal").modal("hide");
                    });

                }
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
        //  }


    }


    $scope.AddMemberValiDate = function () {
      
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var Table_Name = "";
        $("#addModal").find(".dvgender").removeClass("notValid");
        $(".validateAddMember").removeClass("notValid");
        if ($scope.AddNewMember.NewMemberName == '' || $scope.AddNewMember.NewMemberName == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Name' : '\n <br /> कृपया नाम दर्ज करें |';
            $("#AddNewMemberName").addClass("notValid");
        }
      

        //if ($scope.AddNewMember.FileName == '' || $scope.AddNewMember.FileName == undefined || $scope.AddNewMember.FileName=="") {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please upload Nomination form' : '\n <br /> कृपया नामांकन फॉर्म अपलोड करें |';
        //}
        if ($scope.AddNewMember.IsUpload != true) {
            message += $scope._language == 'en-us' ? '\n <br /> Please upload Nomination form' : '\n <br /> कृपया नामांकन फॉर्म अपलोड करें |';
          //  $scope.AddNewMember.IsUploadFileName = true;
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

        //if ($scope.AddNewMember.AadharNo == '' || $scope.AddNewMember.AadharNo == undefined || $scope.AddNewMember.AadharNo.length != 12) {

        //    message += $scope._language == 'en-us' ? '\n <br /> Please Enter valid Aadhaar Number' : '\n <br /> कृपया सही आधार संख्या दर्ज करें |';
        //    $("#AddNewMemberAadharNo2").addClass("notValid");
        //}
         //   else {

        //    let data = $scope.EMemberDetailsData.find(function (ob) { return ob['AadharNo2'] === $scope.AddNewMember.AadharNo });
        //    if ($scope.AddNewMember.MemberId == '0') {
        //        if (data != null) {
        //            message += $scope._language == 'en-us' ? '\n <br /> Aadhaar Number Already added' : '\n <br /> आधार नंबर पहले से ही जोड़ा गया है |';
        //            $("#AddNewMemberAadharNo2").addClass("notValid");
        //        }
        //    }

        //}
      

        if ($scope.AddNewMember.CategoryId == "0" || $scope.AddNewMember.CategoryId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Category' : '\n <br /> कृपया वर्ग का चयन करें |';
            $("#AddNewMemberCategoryId").addClass("notValid");
        }


        if ($scope.AddNewMember.Address == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्जा करे |';
            $("#AddNewMemberAddress").addClass("notValid");
        }




        //if ($("#AddMemberPhoto").val() == "" && $scope.AddNewMember.Photo == '') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Photo of Member.' : '\n <br /> कृपया सदस्य का फोटो चुनें |';
        //    $("#AddMemberPhoto").addClass("notValid");
        //}

        //if ($("#AddMemberPDFForm1").val() == "" && $scope.AddNewMember.AddDocument == '') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please upload nomination form' : '\n <br /> कृपया नामांकन फॉर्म अपलोड करें';
        //    $("#AddMemberPDFForm1").addClass("notValid");
        //}

        if ($scope.AddNewMember.Gender == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Gender Type.' : '\n <br /> कृपया लिंग का चयन करे |';
            $("#addModal").find(".dvgender").addClass("notValid");
        }
        if ($scope.AddNewMember.Presentedby == "" || $scope.AddNewMember.Presentedby == undefined || $scope.AddNewMember.Presentedby == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Presented by.' : '\n <br /> कृपया किसके द्वारा प्रस्तुत करना है  चयन करें |';
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


    $scope.OnPageLoad();
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

        var SocietyRegistrationData =
            {
                "SocietyIdEnc": $scope.SocietyIdEnc,

                "ElectionMasterId": $scope.ElectionMasterId,

            };

        formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        $http.post(SiteUrl + "/Election/InsertSocityVoterMemberByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.status == "1") {
                    ShowOkMessage("", "Data saved successfully", function () {
                        ImageDataList = [];
                        PDFDataList = [];
                        $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);
                    });
                } else if (data.status == "-2") {
                    ShowOkMessage("Warning", data.data.split('@'), function () {

                    });
                } else {
                    ShowOkMessage("", data.data, function () {

                    });
                }

            }).
            error(function (data, status) {
                ShowModelAlert("Bulk Insert of Socity Voter Member", "Error While Upload Excel Sheet");
            });
    };

    $scope.EditCandidate = function (item) {

        $scope.ReSetAddModel();
        $scope.AddNewMember.MemberId = item.MemberId;
        $scope.AddNewMember.Name = item.Name;
        $scope.AddNewMember.ContactNo = item.ContactNo;
        $scope.AddNewMember.AadharNo = item.AadharNo2;
        $scope.AddNewMember.DOB = item.DOB;//.split("/").reverse().join("-"),
        $scope.AddNewMember.Gender = item.Gender;
        $scope.AddNewMember.CategoryId = item.CategoryId;
        $scope.AddNewMember.ElectionMasterId = item.ElectionMasterId;
        $scope.AddNewMember.FHName = item.FHName;
        $scope.AddNewMember.Address = item.Address;
        $scope.AddNewMember.AddDocument = item.CandidatePDFForm;
        $scope.AddNewMember.Photo = item.Photo;

        $("#addModal").modal("show");

        $scope.AddCalender();


    }


    $scope.RemoveCandidate = function (item) {
        var message = $scope._language == 'en-us' ? 'Are you sure you want to remove candidate from list ? ' : 'क्या आप वाकई उम्मीदवार को सूची से हटाना चाहते हैं ।';


        ShowAlertMessage("Warning", message, function () {
            var deleteData = {
                "SocietyIdEnc": item.SocietyIdEnc,
                "AADHAARNo": item.AadharNo3,
                "ElectionMasterId": item.ElectionMasterId,

            }


            $(".blockUI").fadeIn("slow");
            $http.post(SiteUrl + "/Election/DeleteElectionCandidate", JSON.stringify({ CandidateData: deleteData })).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data == "-1") {
                        ShowOkMessage("", "Some technical error occur", function () {

                        });
                    }
                    else {
                        ShowOkMessage("", "Data successfully deleted", function () {
                            $scope.EditBasicDetailsForm($scope.SocietyIdEnc, $scope.ElectionMasterId);
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                });
        });

    }

});

AppClasses.filter('split', function () {
    return function (input, splitChar, splitIndex) {
        // do some bounds checking here to ensure it has that index
        return input.split(splitChar)[splitIndex];
    }
});


AppClasses.directive('validfile1', function validFile() {
    var validFormats = ['jpg', 'gif'];
    
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                  
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 

                        if (file >= 4096) {
                            
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                            scope.AddNewMember.IsUploadFileName = false;
                        } else {
                            if (scope.FileType == 1) {
                                
                                scope.AddNewMember.IsUploadFileName = true;
                                scope.AddNewMember.FileName = "";
                            }
                            scope.$apply();
                        }
                    }
                   

                });
            };
        }
    };



});

//{ { test | split: ',': 0 } }
//{ { test | split: ',': 0 } }