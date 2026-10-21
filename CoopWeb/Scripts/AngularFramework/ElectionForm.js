var Role = angular.module("ElectionRole", ['angularUtils.directives.dirPagination']);

Role.directive('onlyLettersInput', onlyLettersInput);
function onlyLettersInput() {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z ]/g, '');
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
};

Role.controller("Election", function ($scope, $http, $window) {

    $scope.todayDate = new Date().toISOString().split('T')[0];
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    var Mdata = [];
    $scope.TblGodownTimeBoundDetails = [];
    $scope.TblDetails = [];
    $scope.godown = { isGSSKVSS: "1" };
    $scope.IsVisible = false;
    
    $scope.UploadFile = function (file) {
        $scope.File = file[0];
    }      

    function ConvertDate(value) {

        if (!value)
            return null;
       
        if (typeof value === "string" && value.indexOf("/") > -1) {
            var parts = value.split('/');
            return parts[2] + "-" + parts[1] + "-" + parts[0];
        }
        
        var d = new Date(value);
        return d.getFullYear() + "-" + ("0" + (d.getMonth() + 1)).slice(-2) + "-" + ("0" + d.getDate()).slice(-2);
    }

    function parseLocalDate(value) {
        if (!value) return null;

        var d = new (value);

        d.setMinutes(d.getMinutes() + d.getTimezoneOffset());

        return d;
    }

   

    $scope.VacancyList = [{
        VacancyId : "",
        VacantPostId: "",
        VacancyDate: "",
        ReasonForVacancyId: "",
        OtherReason: ""
    }];

    $scope.AddVacancy = function () {
        $scope.VacancyList.push({
            VacantPostId: "",
            VacancyDate: "",
            ReasonForVacancyId: "",
            OtherReason: ""
        }); 
    };

    $scope.RemoveVacancy = function (index) {
        $scope.VacancyList.splice(index, 1);
    };

    $scope.IsPostSelected = function (postId, currentItem) {       
        //if (postId == 20134) {      
        //    return false;
        //}

        //live

        if (postId == 20104) {
            return false;
        }

        for (var i = 0; i < $scope.VacancyList.length; i++) {

            if ($scope.VacancyList[i] != currentItem &&
                $scope.VacancyList[i].VacantPostId == postId) {
                return true;
            }
        }

        return false;
    };

    //New
    $scope.InsertElectionForm = function () { 
        debugger;
        var formData = new FormData();

        if ($scope.election.IsNewSociety == null || $scope.election.IsNewSociety == undefined) {
            alert("कृपया 'क्या संस्था नवगठित है?' का चयन करें।");
            return;
        }

        if ($scope.election.IsVacantPost == null || $scope.election.IsVacantPost == undefined) {
            alert("कृपया 'क्या संस्था नवगठित है?' का चयन करें।");
            return;
        }

        if ($scope.election.SocietyCategoryId == '0') {
            alert('Please Select the Society Category');
            return;
        }

        if ($scope.election.SocietyID == '0') {
            alert('Please Select the Society');
            return;
        }    

        if ($scope.election.BylawsDocument == undefined) {
            alert("कृपया संस्था के उपनियम की पीडीएफ फाइल अपलोड करें।");
            return;
        }

        if ($scope.election.RecommendationDocument == undefined) {
            alert("कृपया अभ्यर्थना / अभिशंसा दस्तावेज़ अपलोड करें।");
            return;
        }

        var isUpdate = ($scope.election.ElectionId && $scope.election.ElectionId > 0);       
        formData.append("ElectionId", $scope.election.ElectionId || 0);        
        formData.append("SocietyCategoryId", $scope.election.SocietyCategoryId);
        formData.append("SocietyID", $scope.election.SocietyID);
        formData.append("RegistrationNo", $scope.election.RegistrationNo);
        formData.append("RegistrationDate", ConvertDate($scope.election.RegistrationDate));       
        formData.append("LastElectionDate", ConvertDate($scope.election.LastElectionDate));
        formData.append("ElectionDate", ConvertDate($scope.election.ElectionDate));
        formData.append("AdminAppointmentDate", ConvertDate($scope.election.AdminAppointmentDate));
        formData.append("DeputyAdminAppointmentDate", ConvertDate($scope.election.DeputyAdminAppointmentDate));        
        formData.append("ReasonForNonElection", $scope.election.ReasonForNonElection);
        formData.append("MemberId", $scope.election.MemberId);  
        formData.append("ReasonForVacancyId", $scope.election.ReasonForVacancyId);  
        formData.append("PostName", $scope.election.PostName || "");
        formData.append("ExcludedVoterListDocument", $scope.election.ExcludedVoterListDocument);
        formData.append("BylawsDocument", $scope.election.BylawsDocument); 
        formData.append("RecommendationDocument", $scope.election.RecommendationDocument);
        formData.append("IsNewSociety", $scope.election.IsNewSociety);
        formData.append("IsVacantPost", $scope.election.IsVacantPost);
        formData.append("FirstBoardNominationDate", ConvertDate($scope.election.FirstBoardNominationDate));

        var vacancyList = angular.copy($scope.VacancyList);

        angular.forEach(vacancyList, function (v) {
            if (v.VacancyDate) {
                v.VacancyDate = ConvertDate(v.VacancyDate);
            }
        });

        formData.append("VacancyList", angular.toJson(vacancyList));     

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/SaveElectionForm", formData, { withCredentials: true, headers: { 'Content-Type': undefined }, transformRequest: angular.identity }).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            console.log('data',data);
            if (parseInt(data, 10) > 0) {

                var Msg = '';

                if (isUpdate) {
                    Msg = (_language == 'en-us' ? 'Data Updated Successfully.' : 'डेटा सफलतापूर्वक अपडेट किया गया');                    
                }
                else {
                    Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');                    
                }

                alert(Msg);
                window.location.reload();
            }
            else if (parseInt(data, 10) == -2) {
                var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');                
                alert(Msg);
            }
            else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                alert(Msg);
            }
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while auditor assignment insert");
            });
    };       


    $scope.GetElectionList = function () {
        $http.get(SiteUrl + "/Society/GetElectionList")
            .success(function (data) {
                $scope.ElectionList = data;     
                console.log('$scope.ElectionList', $scope.ElectionList);
            });
    };


    $scope.GetElectionSocietyCategoryList = function () {
        $http.get(SiteUrl + "/Society/GetElectionSocietyCategoryList")
            .success(function (data) {
                $scope.ElectionSocietyCatgoryList = data;        
            });
    };


    $scope.EditElection = function (item) {
        debugger;
        $(".se-pre-con").fadeIn("slow");
        $scope.election = angular.copy(item);
        $scope.election.ReasonForNonElection = item.ReasonForNonElection;
        $scope.election.ExcludedVoterListDocument = item.ExcludedVoterListDocument;
        $scope.election.BylawsDocument = item.BylawsDocument;        
        $scope.election.IsNewSociety = (item.IsNewSociety === true || item.IsNewSociety === "True") ? true : false;
        $scope.election.IsVacantPost = (item.IsVacantPost === true || item.IsVacantPost === "True") ? true : false;
        $scope.election.RegistrationDate = item.RegistrationDate ? new Date(item.RegistrationDate) : null;
        $scope.election.LastElectionDate = item.LastElectionDate ? new Date(item.LastElectionDate) : null;
        $scope.election.FirstBoardNominationDate = item.FirstBoardNominationDate ? new Date(item.FirstBoardNominationDate) : null;
        $scope.election.ElectionDate = item.ElectionDate ? new Date(item.ElectionDate) : null;
        $scope.election.AdminAppointmentDate = item.AdminAppointmentDate ? new Date(item.AdminAppointmentDate) : null;
        $scope.election.DeputyAdminAppointmentDate = item.DeputyAdminAppointmentDate ? new Date(item.DeputyAdminAppointmentDate) : null;        
        $scope.election.SocietyCategoryId = item.SocietyCategoryId.toString();
        $scope.election.SocietyID = item.SocietyId.toString();       
        $scope.election.RecommendationDocument = item.RecommendationDocument;      
        
        $scope.SocietyTypeChanged();
        $scope.CalculateDueDate();

        $http.get(SiteUrl + "/Society/GetSocietyMemberList?SocietyId=" + $scope.election.SocietyID)
            .success(function (data) {
                $scope.MemberList = data;  

                $http.get(SiteUrl + "/Society/GetElectionVacancyList?ElectionId=" + item.ElectionId)
                    .success(function (vacancyData) {
                        if (vacancyData && vacancyData.length > 0) {                         
                            angular.forEach(vacancyData, function (v) {
                                v.VacancyId = v.VacancyId;         
                                v.VacancyDate = v.VacancyDate ? new Date(v.VacancyDate) : null;
                                v.VacantPostId = v.VacantPostId.toString();
                                v.ReasonForVacancyId = v.ReasonForVacancyId.toString();                                                            
                            });
                            $scope.VacancyList = vacancyData;
                        }
                        else {
                            $scope.VacancyList = [{
                                VacancyId : "",
                                VacantPostId: "",
                                VacancyDate: "",
                                ReasonForVacancyId: "",
                                OtherReason: ""
                            }];
                        }
                        $(".se-pre-con").fadeOut("slow");
                    })
                    .error(function () {
                        $(".se-pre-con").fadeOut("slow");
                    });

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
        $scope.isViewMode = false;
    };

    $scope.ViewElection = function (item) {
        $(".se-pre-con").fadeIn("slow");
        $scope.election = angular.copy(item);
        $scope.election.RegistrationDate = item.RegistrationDate ? new Date(item.RegistrationDate) : null;
        $scope.election.LastElectionDate = item.LastElectionDate ? new Date(item.LastElectionDate) : null;
        $scope.election.ElectionDate = item.ElectionDate ? new Date(item.ElectionDate) : null;
        $scope.election.AdminAppointmentDate = item.AdminAppointmentDate ? new Date(item.AdminAppointmentDate) : null;
        $scope.election.DeputyAdminAppointmentDate = item.DeputyAdminAppointmentDate ? new Date(item.DeputyAdminAppointmentDate) : null;   
        $scope.election.IsNewSociety = (item.IsNewSociety === true || item.IsNewSociety === "True") ? true : false;
        $scope.election.IsVacantPost =  (item.IsVacantPost === true || item.IsVacantPost === "True") ? true : false;
        $scope.election.ExcludedVoterListDocument = item.ExcludedVoterListDocument;
        $scope.election.BylawsDocument = item.BylawsDocument;
        $scope.election.SocietyCategoryId = item.SocietyCategoryId.toString();
        $scope.election.SocietyID = item.SocietyId.toString();        
        $http.get(SiteUrl + "/Society/GetSocietyMemberList?SocietyId=" + $scope.election.SocietyID)
            .success(function (data) {
                $scope.MemberList = data;                
                $http.get(SiteUrl + "/Society/GetElectionVacancyList?ElectionId=" + item.ElectionId)
                    .success(function (vacancyData) {                       
                        if (vacancyData && vacancyData.length > 0) {
                            angular.forEach(vacancyData, function (v) {
                                v.VacancyId = v.VacancyId;
                                v.VacancyDate = v.VacancyDate ? new Date(v.VacancyDate) : null;
                                v.VacantPostId = v.VacantPostId.toString();
                                v.ReasonForVacancyId = v.ReasonForVacancyId.toString();                                
                            });
                            $scope.VacancyList = vacancyData;                           
                        }
                        else {
                            $scope.VacancyList = [{
                                VacantPostId: "",
                                VacancyDate: "",
                                ReasonForVacancyId: "",
                                OtherReason: ""
                            }];
                        }
                        $(".se-pre-con").fadeOut("slow");
                    })
                    .error(function () {
                        $(".se-pre-con").fadeOut("slow");
                    });

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });

        $scope.isViewMode = true;
    };


    $scope.DeleteElection = function (ElectionId) {
        if (confirm("Are you sure you want to delete this record?")) {
            $http.post( SiteUrl + "/Society/DeleteElection?ElectionId=" + ElectionId
            ).success(function (data) {
                if (data > 0) {
                    alert("Record deleted successfully.");
                    $scope.GetElectionList();
                }
            });
        }
    };     
    
    $scope.translation = [];
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
   
    $scope.isKVSS = false;
    $scope.isGSS = false;
    $scope.getshowKvssGss = function (id) {
        if (id === 2) {
            $scope.isKVSS = true;
            $scope.isGSS = false;
        }
        else if (id === 1) {
            $scope.isGSS = true;
            $scope.isKVSS = false;
        }
    };
    $scope.isChcyes = false;
    $scope.isChcno = false;
    $scope.godown = { isGSSKVSS: "1" };
    $scope.type = "GSS";
    $scope.getshowChc = function (id) {

        if (id === 2) {
            $scope.isChcno = true;
            $scope.isChcyes = false;
        }
        else if (id === 1) {
            $scope.isChcyes = true;
            $scope.isChcno = false;
        }
    };
    $scope.isBenyes = false;
    $scope.isBenno = false;
    $scope.getshowben = function (id) {
        if (id === 2) {
            $scope.isBenno = true;
            $scope.isBenyes = false;
        }
        else if (id === 1) {
            $scope.isBenyes = true;
            $scope.isBenno = false;
        }

    };
    $scope.showGSSKvss = false;
    $scope.GetKVSSList = function (id) {
        if (id == 1) {
            //old
            $scope.type = "GSS"
            //new
            //$scope.type = ""
            $scope.showGSSKvss = true;
        }
        else if (id == 2) {
            $scope.type = "KVSS"
        }
        $scope.KvssGssList = [];
        $http.get(SiteUrl + "/Society/GetKVSSGSSData/?Type=" + $scope.type + "&DistrictId=" + $('#district_MemberAdd').val() + "&BlockId=" + ($scope.UralUrban == 1 ? $("#ddlblocki_MemberAdd").val() : 0) + "&CityId=" + ($scope.UralUrban == 1 ? 0 : $("#ddlcity_MemberAdd").val()) + "&GPId=" + ($scope.UralUrban == 1 ? $("#ddlGP_MemberAdd").val() : 0)).
            success(function (data, status) {
                $scope.KvssGssList = data;
                $scope.showGSSKvss = true;
            }).
            error(function (data, status) {
                $scope.KvssGssList = [];
                alert('Error while fetching KVSSList  data.');
            });
    };
    $scope.GetSocietyList = function () {
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Godown/GetSocietyData/?Type=" + $scope.type + "&DistrictId=" + $('#district_MemberAdd').val() + "&BlockId=" + ($scope.UralUrban == 1 ? $("#ddlblocki_MemberAdd").val() : 0) + "&CityId=" + ($scope.UralUrban == 1 ? 0 : $("#ddlcity_MemberAdd").val()) + "&GPId=" + ($scope.UralUrban == 1 ? $("#ddlGP_MemberAdd").val() : 0)).
            success(function (data, status) {
                $scope.SocietyList = data;              

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching SocietyList  data.');
            });
    };

    $scope.ReasonForVacancyList = [];
    $scope.GetReasonForVacancyList = function () {
        $http.get(SiteUrl + "/Society/GetReasonForVacancyByListId/?ListCode=ReasonForVacancyList")
            .success(function (data, status) {
                $scope.ReasonForVacancyList = data;                
            })
            .error(function () {
                $scope.ReasonForVacancyList = [];
            });
    };

    $scope.GodownLandList = [];
    $scope.GetGodownLandList = function () {
        $http.get(SiteUrl + "/Godown/GetNewLovValuesDocByListId/?ListCode=ForGodownLandDetails")
            .success(function (data, status) {
                $scope.GodownLandList = data;
            })
            .error(function () {
                $scope.GodownLandList = [];
            });
    };


    $scope.GodownVacantPostList = [];
    $scope.GetGodownVacantList = function () {
        $http.get(SiteUrl + "/Godown/GetNewLovValuesDocByListId/?ListCode=VacantPostDetails")
            .success(function (data, status) {
                $scope.GodownVacantPostList = data;
                console.log('$scope.GodownVacantPostList', $scope.GodownVacantPostList);
            })
            .error(function () {
                $scope.GodownVacantPostList = [];
            });
    };

    $scope.IsFreeze = true;    

    $scope.OnMemberChange = function () {
        $scope.election.PostName = '';
        var member = $scope.MemberList.find(function (x) {
            return x.AdministrativeId == $scope.election.MemberId;
        });      

        if (member) {
            $scope.election.PostName = member.POSTNAME;
        }
    };

    $scope.SocietyTypeChanged = function () {
        if ($scope.election.IsNewSociety) {
            $scope.election.LastElectionDate = null;
        }
        else {
            $scope.election.FirstBoardNominationDate = null;
        }
        $scope.election.ElectionDate = null;
    };

    $scope.CalculateDueDate = function () {

        if ($scope.election.IsNewSociety) {
            if ($scope.election.FirstBoardNominationDate) {
                var dt = new Date($scope.election.FirstBoardNominationDate);
                dt.setMonth(dt.getMonth() + 3);
                console.log('dt',dt);
                $scope.election.ElectionDate = dt;
            }
        } else {
            if ($scope.election.LastElectionDate) {
                var dt = new Date($scope.election.LastElectionDate);
                dt.setFullYear(dt.getFullYear() + 5);
                console.log('dt',dt);
                $scope.election.ElectionDate = dt;
            }
        }
    };

    $scope.MemberList = [];

    $scope.OnSocietyChange = function () {   
        $scope.election.RegistrationDate = "";
        $scope.election.RegistrationNo = "";

        if (!$scope.SocietyDataList || $scope.SocietyDataList.length == 0) {
            console.warn("SocietyDataList is empty");
            return;
        }

        var row = $scope.SocietyDataList.find(function (x) {
            return x.SocietyID.toString() === $scope.election.SocietyID.toString();
        });

        if (!row) {
            console.warn("No matching society found for ID:", $scope.election.SocietyID);
            return;
        }       

        $scope.election.SocietyID = row.SocietyID.toString();      
        $scope.election.RegistrationDate = row.RegistrationDate;
        $scope.election.RegistrationNo = row.RegistrationNo;    

        $http.get(SiteUrl + "/Society/GetSocietyMemberList?SocietyId=" + $scope.election.SocietyID)
            .success(function (data) {
                $scope.MemberList = data;               
            });
        
    };

    //Voter Document Upload
    $scope.SelectExcludedVoterListDocument = function (files) {

        if (files && files.length > 0) {

            if (!$scope.election) {
                $scope.election = {};
            }

            if (!$scope.election.ExcludedVoterListDocument) {
                $scope.election.ExcludedVoterListDocument = {};
            }

            $scope.election.ExcludedVoterListDocument = files[0].name;

            $scope.UploadExcludedVoterListDocument(files);
        }
    };

    $scope.UploadExcludedVoterListDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];

        var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
        if (format.test(file.name)) {
            $(this).val('');
            return;
        }

        let fd = new FormData();
        fd.append("ExcludedVoterListDocument", file);

        $http.post(SiteUrl + "/Society/UploadVoterDocument", fd, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.election.ExcludedVoterListDocument = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };

    // Related to Bylaws Document
    $scope.SelectRelatedUploadBylawsDocument = function (files) {

        if (files && files.length > 0) {

            if (!$scope.election) {
                $scope.election = {};
            }

            if (!$scope.election.BylawsDocument) {
                $scope.election.BylawsDocument = {};
            }

            $scope.election.BylawsDocument = files[0].name;

            $scope.UploadRelatedBylawsDocument(files);
        }
    };

    $scope.UploadRelatedBylawsDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];

        var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
        if (format.test(file.name)) {
            $(this).val('');
            return;
        }

        let fd = new FormData();
        fd.append("BylawsDocument", file);

        $http.post(SiteUrl + "/Society/UploadBylawsDocument", fd, { transformRequest: angular.identity, headers: { 'Content-Type': undefined }  })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.election.BylawsDocument = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };


    // Related to Recommendation Document
    $scope.SelectRecommendationDocument = function (files) {

        if (files && files.length > 0) {

            if (!$scope.election) {
                $scope.election = {};
            }

            if (!$scope.election.RecommendationDocument) {
                $scope.election.RecommendationDocument = {};
            }

            $scope.election.RecommendationDocument = files[0].name;

            $scope.UploadRecommendationDocument(files);
        }
    };

    $scope.UploadRecommendationDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];

        var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
        if (format.test(file.name)) {
            $(this).val('');
            return;
        }

        let fd = new FormData();
        fd.append("RecommendationDocument", file);

        $http.post(SiteUrl + "/Society/UploadRecommendationDocument", fd, { transformRequest: angular.identity, headers: { 'Content-Type': undefined } })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.election.RecommendationDocument = r.fileName;
                    console.log('$scope.election.RecommendationDocument', $scope.election.RecommendationDocument);
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };


    $scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/PacsUserLoginElectionDetails", { "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data.filter(x => x.IsObjection == false);
            $scope.SocietyDataList = $scope.TableData;            
            $scope.SocietyList = $scope.SocietyDataList;              
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");
        
        $scope.GetReasonForVacancyList();
        $scope.GetSocietyBySSO();
        $scope.GetElectionList();
        $scope.GetElectionSocietyCategoryList();
        $scope.GetGodownLandList();
        $scope.GetGodownVacantList();
    };

    $scope.getTranslation();
    $scope.OnPageLoad();     
});