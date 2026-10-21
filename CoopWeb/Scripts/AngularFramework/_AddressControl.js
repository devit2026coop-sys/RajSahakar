var _callback;
var _http
var _Addressinfoval = '';

var Divisionslist;
var Districtlist;

var Citylist;
var WardList;

var BlockDataList;
var GramPancyatList;
var VillageList;

var stateid;
var districtid;
var divisionid;
var cityid;
var blockid;
var wardid;
var panchayatid;
var villageid;
var translation;
function getTranslation(id) {
   // _language = 'hi-in';
    //scope._language = _language;
    $(".HOUSE_BUILDING_NUMBER_TXT").text(translation.HouseBuildingNo);
    $(".LANE_STREET_NAME_TXT").text(translation.LaneStreetName);
    $(".LOCALITY_TXT").text(translation.Locality);
    $(".STATE_TXT").text(translation.State);
    $(".DIVISION_TXT").text(translation.DivisionHeading);
    $(".DISTRICT_TXT").text(translation.District);
    $(".URBAN_RURAL_TXT").text(translation.UrbanRural);
    $(".URBAN_TXT").text(translation.Urban);
    $(".RURAL_TXT").text(translation.Rural);
    $(".BLOCK_TXT").text(translation.BlockHeading);
    $(".GRAMPANCHAYAT_TXT").text(translation.GramPanchayatHeading);
    $(".VILLAGE_TXT").text(translation.Village);
    $(".CITY_TXT").text(translation.City);
    $(".WARD_TXT").text(translation.Ward);
    $(".PIN_TXT").text(translation.PIN);
    $("#addr_"+id).find('select').find("option").eq(0).text(_language == 'en-us' ? '--- Select ---' : '--- चुनें ---');
}

var Addressgetinfo = {};

function GetAddressinfo(id) {
   
    var aa = Districtlist;

    return Addressgetinfo = {
        HouseBuildingNo: $("#HouseBuildingNo_" + id).val(),
        LaneStreetName: $("#LaneStreetName_" + id).val(),
        Locality: $("#Locality_" + id).val(),
        Pincode: $("#pincode_" + id).val(),
        Stateid: $('#ddlstate_' + id).val(),
        DivisionId: $('#ddlDivision_' + id).val(),
        DistrictId: $('#district_' + id).val(),
        CityId: $('#ddlcity_' + id).val(),
        WardId: $('#ddlward_' + id).val(),
        BlockId: $('#ddlblocki_' + id).val(),
        PanchayatId: $('#ddlGP_' + id).val(),
        VillageId: $('#ddlVillage_' + id).val(),
        Checkboxvalue: $("input[name='Rurals']:checked").val(),

        State_Name: $('#ddlstate_' + id + " option:selected").text(),
        Division_Name: $('#ddlDivision_' + id + " option:selected").text(),
        District_Name: $('#district_' + id + " option:selected").text(),
        City_Name: $('#ddlcity_' + id + " option:selected").text(),
        Ward_Name: $('#ddlward_' + id + " option:selected").text(),
        Block_Name: $('#ddlblocki_' + id + " option:selected").text(),
        Panchayat_Name: $('#ddlGP_' + id + " option:selected").text(),
        Village_Name: $('#ddlVillage_' + id + " option:selected").text(),
        Urban_Rural: $("input[name='Rurals']:checked").val() == "2" ? "Rurals" : $("input[name='Rurals']:checked").val() == "1" ? "Urban" : ""


    }
}



function GetAddressDispplay(id) {
    
    var aa = Districtlist;

    var Address = translation.HouseBuildingNo + " : " + $("#HouseBuildingNo_" + id).val() + ",  " + translation.LaneStreetName + " : " + $("#LaneStreetName_" + id).val() + ", " + translation.Locality + " : " + $("#Locality_" + id).val()
        + ", " + translation.State + " : " + $('#ddlstate_' + id + " option:selected").text() + ", " + translation.DivisionHeading + " : " + $('#ddlDivision_' + id + " option:selected").text() + ", " + translation.District + " : " + $('#district_' + id + " option:selected").text() +
     ($("input[name='Rurals']:checked").val() == 1 ? ", " + translation.City + " : " + $('#ddlcity_' + id + " option:selected").text() + ", " + translation.Ward+" : " + $('#ddlward_' + id + " option:selected").text() :
        ", " + translation.BlockHeading + " : " + $('#ddlblocki_' + id + " option:selected").text() + ", " + translation.GramPanchayatHeading + " : " + $('#ddlGP_' + id + " option:selected").text() + ", " + translation.Village+" : " + $('#ddlVillage_' + id + " option:selected").text())+
    ", " + translation.PIN+" : " + $("#pincode_" + id).val();

    return Address;
    //return Addressgetinfo = {
    //    HouseBuildingNo: $("#HouseBuildingNo_" + id).val(),
    //    LaneStreetName: $("#LaneStreetName_" + id).val(),
    //    Locality: $("#Locality_" + id).val(),
    //    Pincode: $("#pincode_" + id).val(),
    //    Stateid: $('#ddlstate_' + id).val(),
    //    DivisionId: $('#ddlDivision_' + id).val(),
    //    DistrictId: $('#district_' + id).val(),
    //    CityId: $('#ddlcity_' + id).val(),
    //    WardId: $('#ddlward_' + id).val(),
    //    BlockId: $('#ddlblocki_' + id).val(),
    //    PanchayatId: $('#ddlGP_' + id).val(),
    //    VillageId: $('#ddlVillage_' + id).val(),
    //    Checkboxvalue: $("input[name='Rurals']:checked").val()
    //}
}





var Addressinfo1;

function SetAddressinfo(_translation, id, Addressinfo, http) {
    
    translation = _translation;
    getTranslation(id);
    _http = http;
   
  return  GetDistrict().then(function () {

        Addressinfo1 = Addressinfo;
        $(".cityward_MemberAdd").hide();
        $(".Rurals_MemberAdd").hide();
        $("#HouseBuildingNo_" + id).val(Addressinfo.Housebuilding);
        $("#LaneStreetName_" + id).val(Addressinfo.Lanestreet);
        $("#pincode_" + id).val(Addressinfo.Pincode);
        $("#Locality_" + id).val(Addressinfo.Locality);

        //getdist();
        var aa = Districtlist;
        var District_Id = 1;
        var District = Addressinfo.District.split(" ");
        var DistrictName = District[1] == undefined ? "" : District[1];
        var District_Id = "0";
        var DIVISION_ID = "0";
        if ($.isNumeric(Addressinfo.District.split(" ")[1])) {
            District_Id = Addressinfo.District.split(" ")[1];
            DIVISION_ID = Districtlist.filter(function (Districtlist) { return Districtlist.DISTRICT_ID == District_Id }).length > 0 ? Districtlist.filter(function (Districtlist) { return Districtlist.DISTRICT_ID == District_Id })[0].DIVISION_ID : "";
        } else {
            District_Id = Districtlist.filter(function (Districtlist) { return Districtlist.DISTRICT_ENG == DistrictName }).length > 0 ? Districtlist.filter(function (Districtlist) { return Districtlist.DISTRICT_ENG == DistrictName })[0].DISTRICT_ID : "";
            DIVISION_ID = Districtlist.filter(function (Districtlist) { return Districtlist.DISTRICT_ENG == DistrictName }).length > 0 ? Districtlist.filter(function (Districtlist) { return Districtlist.DISTRICT_ENG == DistrictName })[0].DIVISION_ID : "";
        }


        cityid = Addressinfo.CITY_ID;
        blockid = Addressinfo.BLOCK_ID;
        wardid = Addressinfo.WARD_Id;
        panchayatid = Addressinfo.GP_ID;
        villageid = Addressinfo.VILLAGE_ID;

        $("#district_" + id).val(parseInt(District_Id));
        $("#ddlcity_" + id).val(Addressinfo.CITY_ID);
        $("#ddlblocki_" + id).val(Addressinfo.BLOCK_ID);
        $("#ddlGP_" + id).val(Addressinfo.GP_ID);
        $("#ddlward_" + id).val(Addressinfo.WARD_Id);
        if (Addressinfo.IsRurals) {
            $("#sdf").prop("checked", true);
            getRural(id);
        } else {
            //$('input[name=Rurals]').val("2");
            $("#sdfs").prop("checked", true);
            getUrban(id);
        }
        $("#ddlstate_" + id).val("6");
        $("#ddlDivision_" + id).val(parseInt(DIVISION_ID));
        // getdist();

    });
}

function getdist() {
    var ddlFruits = document.getElementById("ddlDivision_MemberAdd");
    var selectedText = ddlDivision_MemberAdd.options[ddlDivision_MemberAdd.selectedIndex].innerHTML;
    var selectedValue = ddlDivision_MemberAdd.value;
    var divisionid = parseInt(selectedValue)
    //GetDistrictFill(divisionid)
     GetDistrict()

};

function GetDivision() {
    
    _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        success(function (data, status) {
            Divisionslist = data;
        }).
        error(function (data, status) {
            alert('nostate');
        });
}

$(function () {
    $("#ddlDivision_MemberAdd").change(function () {
        
        var ddlFruits = document.getElementById("ddlDivision_MemberAdd");
        var selectedText = ddlDivision_MemberAdd.options[ddlDivision_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = ddlDivision_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        GetDistrict(divisionid)
        GetDistrict()
       
    });
});

function GetDistrict() {
    
    $('#district_MemberAdd').empty();
  return  _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
            success(function (data, status) {
            Districtlist = data;
           
                $('#district_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');              

            for (var i = 0; i < Districtlist.length; i++) {
                $('#district_MemberAdd').append('<option value="' + Districtlist[i].DISTRICT_ID + '">' + Districtlist[i].DISTRICT_ENG + '</option>');               
            }
        }).
        error(function (data, status) {
            Districtlist = data;
        });
    //  _callback(Addressgetinfo);
}

//function GetDistrictFill(selectedValue1) {
//    
//    $('#district_MemberAdd').empty();
//    //$("#ddlFruits option").remove();
//    _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + selectedValue1).
//        success(function (data, status) {
//            Districtlist = data;
//            // 
//            for (var i = 0; i < Districtlist.length; i++) {

//                $('#district_MemberAdd').append('<option value="' + Districtlist[i].DISTRICT_ID + '">' + Districtlist[i].DISTRICT_ENG + '</option>');
//            }
//        }).
//        error(function (data, status) {
//            Districtlist = data;
//        });
//    //  _callback(Addressgetinfo);
//}


$(function () {
    $("#district_MemberAdd").change(function () {
        var ddlFruits = document.getElementById("district_MemberAdd");
        var selectedText = district_MemberAdd.options[district_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = district_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        loadcity(divisionid)

    });
});

function loadcity(disid) {
    $('#ddlcity_MemberAdd').empty();
  return  _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + disid).
        success(function (data, status) {
            Citylist = data;
            $('#ddlcity_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');  
            for (var i = 0; i < Citylist.length; i++) {

                $('#ddlcity_MemberAdd').append('<option value="' + Citylist[i].CITY_ID + '">' + Citylist[i].CITY_ENG + '</option>');
            }
            // _callback(Addressgetinfo);
        }).
        error(function (data, status) {
            Citylist = [];
        });
}


$(function () {
    $("#ddlcity_MemberAdd").change(function () {
        
        var ddlFruits = document.getElementById("ddlcity_MemberAdd");
        var selectedText = ddlcity_MemberAdd.options[ddlcity_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = ddlcity_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        getward(divisionid)
    });
});


function getward(cityid) {
  
    $('#ddlward_MemberAdd').empty();
  return  _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + cityid).
        success(function (data, status) {
            WardList = data;
       
            $('#ddlward_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');  

            for (var i = 0; i < WardList.length; i++) {

                $('#ddlward_MemberAdd').append('<option value="' + WardList[i].WARD_ID + '">' + WardList[i].WARD_ENG + '</option>');
            }
            //  _callback(Addressgetinfo);
        }).
        error(function (data, status) {
            WardList = [];
        });
}



function getblock(disid) {
 
    $('#ddlblocki_MemberAdd').empty();
    return _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + disid).
        success(function (data, status) {
            BlockDataList = data;
            $('#ddlblocki_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');  
            for (var i = 0; i < BlockDataList.length; i++) {

                $('#ddlblocki_MemberAdd').append('<option value="' + BlockDataList[i].BLOCK_ID + '">' + BlockDataList[i].BLOCK_ENG + '</option>');
                //BLOCK_MANGAL
            }
            // _callback(Addressgetinfo);
        }).
        error(function (data, status) {
            BlockDataList = [];
        });
}

function getGrampanchyat(divisionid) {
 
    $('#ddlGP_MemberAdd').empty();
   return  _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + divisionid).
        success(function (data, status) {

            GramPancyatList = data;
            $('#ddlGP_MemberAdd').append('<option value=""  class="ng-binding">--- Select ---</option>');  
           
            for (var i = 0; i < GramPancyatList.length; i++) {

                $('#ddlGP_MemberAdd').append('<option value="' + GramPancyatList[i].GP_ID + '">' + GramPancyatList[i].GP_ENG + '</option>');
            }//GP_MANGAL
            // _callback(Addressgetinfo);
        }).
        error(function (data, status) {
            GramPancyatList = [];

        });
}

function getvillage(pnchid) {
   
    //VillageList = [];
    $('#ddlVillage_MemberAdd').empty();
  return  _http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + pnchid).
        success(function (data, status) {
            VillageList = data;
            $('#ddlVillage_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');  
          
            for (var i = 0; i < VillageList.length; i++) {

                $('#ddlVillage_MemberAdd').append('<option value="' + VillageList[i].VILLAGE_ID + '">' + VillageList[i].VILLAGE_ENG + '</option>');
            }//VILLAGE_MANGAL
            // _callback(Addressgetinfo);
        }).
        error(function (data, status) {
            $scope.Villages = [];
        });
}



//start


$(function () {


    $("#ddlblocki_MemberAdd").change(function () {
     
        var ddlFruits = document.getElementById("ddlblocki_MemberAdd");
        var selectedText = ddlblocki_MemberAdd.options[ddlblocki_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = ddlblocki_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        getGrampanchyat(divisionid)
    });


});

$(function () {

    $("#ddlGP_MemberAdd").change(function () {
    
        var ddlFruits = document.getElementById("ddlGP_MemberAdd");
        var selectedText = ddlGP_MemberAdd.options[ddlGP_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = ddlGP_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        getvillage(divisionid)
    });
});
//end



function getUrban(id) {
   
    var radioValue = $("input[name='Rurals']:checked").val();
    if (radioValue == 1) {
        $(".Rurals_MemberAdd").hide();
        $(".cityward_MemberAdd").show();
        var ddlFruits = document.getElementById("district_MemberAdd");
        //var selectedText = district_MemberAdd.options[district_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = district_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        loadcity(divisionid).then(function () {
            $("#ddlcity_" + id).val(cityid);
            getward(cityid).then(function () {
                $("#ddlward_" + id).val(wardid);
            });
        });
    }
}

function getRural(id) {
 
    $('#ddlGP_MemberAdd').empty();
    $('#ddlGP_MemberAdd').append('<option value=""  class="ng-binding">--- Select ---</option>');
    var radioValue = $("input[name='Rurals']:checked").val();
    if (radioValue == 2) {
        $(".Rurals_MemberAdd").show();
        $(".cityward_MemberAdd").hide();
        var ddlFruits = document.getElementById("district_MemberAdd");
       // var selectedText = district_MemberAdd.options[district_MemberAdd.selectedIndex].innerHTML;
        var selectedValue = district_MemberAdd.value;
        var divisionid = parseInt(selectedValue)
        getblock(divisionid).then(function () {    
            $("#ddlblocki_" + id).val(blockid);
            getGrampanchyat(blockid).then(function () {
                $("#ddlGP_" + id).val(panchayatid);
                getvillage(panchayatid).then(function () {
                    $("#ddlVillage_" + id).val(villageid);
                });
            });           
        })
    }
}










