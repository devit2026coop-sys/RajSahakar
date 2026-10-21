
var _http;
var SiteUrl = '../';
var societyid;

function OpenRatingpopup(SocietyIdENC) {//, callbackSuccess) {   
    societyid = SocietyIdENC;
    $('#modelId').modal("show");   
    $("#saveratingbtn").attr("disabled", true);
    $('textarea:input[name=message]').val('');
    //_callback = callbackSuccess;
};


var rating=0;
function changeRate(id) {
    
    rating = id;
    if (rating > 0) {
        $("#saveratingbtn").attr("disabled", false);
    }
    
}


function SaveRating() {    
    var comment = $('textarea:input[name=message]').val();
    //$('#modelAlertMessage1').empty();
    if (comment == "") {
       // $('#modelAlertMessage1').append('Please Enter Your Comment');
        //$('#Ratingmodel').show();
        ShowOkMessage("",'Please Enter Your Comment', function () { });
        return;
    } else
        if ($("#txtMobile").val() == "" || $("#txtEmail").val() == "") {
          //  $('#modelAlertMessage1').append('Please Enter Your Mobile Number');
            ShowOkMessage("",'Please Enter Your Mobile Number and Email', function () { });
            return;
        } else {
            var param = {
                rating: rating,
                societyid: societyid,
                Comment: $('textarea:input[name=message]').val(),
                Mobile: $("#txtMobile").val(),
                Email: $("#txtEmail").val()
            }
            $('.se-pre-con').show();
            $.post(SiteUrl + "/Citizen/UserRatinginsert", { _postvarfname: param }).
                success(function (data, status) {
                    $('.se-pre-con').hide();
                    if (data == 1 && status == 'success') {
                        ShowOkMessage("", 'Thanks for rating us', function () {
                            $('#modelId').modal("hide");
                        });

                    } else {
                        ShowOkMessage("", 'There is some problem while submiting your request. Please try after some time.', function () { });
                    }
                }).
                error(function (data, status) {
                    ShowOkMessage("", 'There is some problem while submiting your request. Please try after some time.', function () { });
                });
        }
}

function closepopup() {
    $('#RatingInsermodel').hide();
};

function ResetRating() {
    rating = 0;
    if (rating==0) {
        $("#saveratingbtn").attr("disabled", true);
    }    
    $('textarea:input[name=message]').val('');
    
}
function Closepopupsecuss() {
    $('#Ratingmodel').hide();
    _callback(true);
}