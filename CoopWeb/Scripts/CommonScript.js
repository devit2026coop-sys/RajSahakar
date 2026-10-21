var SiteUrl = '../';
$(document).ready(function () {
    //json date to dd/mm/yyyy format
    var baseUrl = '../';
    String.prototype.toMSDate = function () {
        var date = this.replace('/Date(', '').replace(')/', ''); //CHANGED LINE    
        date = new Date(parseInt(date, 10));
        var mm = (date.getMonth() + 1).toString(); // getMonth() is zero-based
        var dd = date.getDate().toString();
        var date = (dd[1] ? dd : "0" + dd[0]) + "/" + (mm[1] ? mm : "0" + mm[0]) + "/" + date.getFullYear().toString();
        return date;
    }

    //$('input[type="file"]').after('<span class="filesize" ></span>');
    //$('input[type="file"]').unbind('change').bind('change', function () {
    //    debugger;
    //    var ischek = $(this).attr("dat-val") != 'nocheck';
    //    if (ischek) {
    //        if ($(this)[0].files.length > 0) {
    //            if (checkExtension($(this).val(), $(this).attr('filetype'))) {
    //                var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
    //                var dotCount = ($(this)[0].files[0].name.match(/\./g) || []).length;
    //                if ($(this)[0].files[0].name.match(format)) {
    //                    alert(_language ? "Invalid File(File name should not contain special characters)" : "अवैध फाइल(फ़ाइल नाम में विशेष वर्ण'special characters' नहीं होने चाहिए)");
    //                    $(this).val('');
    //                    $(this).parent().find('.filesize').text('');
    //                    return;
    //                }

    //                if (dotCount !== 1) {
    //                    alert('File name must contain only one dot (extension).');
    //                    return;
    //                }
    //                if (!$(this).attr('checkmaxsize').indexOf("und") > -1) {
    //                    if (!(parseInt($(this).attr('checkmaxsize')).toString().indexOf("NaN") > -1)) {
    //                        if ($(this)[0].files[0].size > (parseInt($(this).attr('checkmaxsize')) * 1024)) {     // value of checkmaxsize is 1024 = 1 MB
    //                            var ValMB = (parseInt($(this).attr('checkmaxsize')) / 1024);
    //                            alert(_language ? "File size exceeded.\n Please select files have size less then " + ValMB + " MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार " + ValMB + " MB कम करें|");
    //                            $(this).val('');
    //                            $(this).parent().find('.filesize').text('');
    //                        }
    //                    }
    //                    else if (!checkFileLength($(this)[0].files[0].size)) {
    //                        $(this).parent().find('.filesize').text(formateBitRates($(this)[0].files[0].size));
    //                    }
    //                    else {
    //                        alert(_language ? "File size exceeded.\n Please select files have size less then 1 MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 1 MB कम करें|");
    //                        $(this).val('');
    //                        $(this).parent().find('.filesize').text('');
    //                    }
    //                }
    //                else if (!checkFileLength($(this)[0].files[0].size)) {
    //                    $(this).parent().find('.filesize').text(formateBitRates($(this)[0].files[0].size));
    //                }
    //                else {
    //                    alert(_language ? "File size exceeded.\n Please select files have size less then 1 MB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 1 MB कम करें|");
    //                    $(this).val('');
    //                    $(this).parent().find('.filesize').text('');
    //                }
    //            }
    //            else {
    //                $(this).val('');
    //                $(this).parent().find('.filesize').text('');
    //            }
    //        }
    //        else {
    //            alert(_language ? "Please select file." : "फ़ाइल का चयन करें|");
    //            $(this).parent().find('.filesize').text('');
    //        }
    //    }
    //});

    $('input[type="file"]').after('<span class="filesize" ></span>');
    $(document).on('change', 'input[type="file"]', function () {

        var ischek = $(this).attr("dat-val") !== 'nocheck';

        if (!ischek) return;

        if (this.files.length === 0) {
            alert(_language ? "Please select file." : "फ़ाइल का चयन करें|");
            return;
        }

        var file = this.files[0];
        var fileName = file.name;

        var format = /[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/;
        if (format.test(fileName)) {
            alert(_language
                ? "Invalid File (File name should not contain special characters)"
                : "अवैध फाइल (फ़ाइल नाम में विशेष वर्ण नहीं होने चाहिए)");
            $(this).val('');
            return;
        }

        var dotCount = (fileName.match(/\./g) || []).length;
        if (dotCount !== 1) {
            alert('File name must contain only one dot (extension).');
            $(this).val('');
            return;
        }

        var maxSizeKB = parseInt($(this).attr('checkmaxsize'));
        if (!isNaN(maxSizeKB)) {
            if (file.size > maxSizeKB * 1024) {
                alert("File size exceeded. Please upload smaller file.");
                $(this).val('');
                return;
            }
        }

    });
    

});

function validateFile(input) {

    if (input.files.length === 0) {
        alert("Please select file");
        return false;
    }

    var file = input.files[0];
    var fileName = file.name;

    if (/[!@#$%^&*()+\-=\[\]{};':"\\|,<>\/?]/.test(fileName)) {
        alert("Invalid file name");
        return false;
    }

    if ((fileName.match(/\./g) || []).length !== 1) {
        alert("File name must contain only one dot");
        return false;
    }

    var maxSizeKB = input.getAttribute('checkmaxsize');
    if (maxSizeKB && file.size > maxSizeKB * 1024) {
        alert("File too large");
        return false;
    }

    return true; // ✅ VALID
}

function ShowModelAlert(HeaderMSG, ModelAlertMessage) {
    $('#modelHeader').text(HeaderMSG);
    $('#modelAlertMessage').html(ModelAlertMessage);
    $('#modelBTN').click();

    // $('#myModalAlert').show();
}
function HideModelAlert() {
    $('#modelHeader').text('');
    $('#modelAlertMessage').html('');
    $('#myModalAlert').hide();
}
function checkFileLength(fileSized) {
    //    if (Max_fileSize > fileSized * 1000) {
    //        return true;
    //    }
    if (10000000 < fileSized) {
        return true;
    }

    return false;
}
function formateBitRates(bits) {
    if (typeof bits !== 'number') {
        return '';
    }
    if (bits >= 1000000000) {
        return (bits / 1000000000).toFixed(2) + ' GBs';
    }
    if (bits >= 1000000) {
        return (bits / 1000000).toFixed(2) + ' MBs';
    }
    if (bits >= 1000) {
        return (bits / 1000).toFixed(2) + 'KBs';
    }
    return bits.toFixed(2) + 'bits';
}
function LoaderStart() { $('#preloader').show(); }
function LoaderStop() { $("#preloader").css("display", "none"); }
function ScrollPage() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
}

function checkExtension(filename, filetype) {
    var isValid = false;
    var message = '';
    var extension = filename.split("").reverse().join("");
    extension = extension.substring(0, extension.indexOf('.'));
    extension = extension.split("").reverse().join("").toLowerCase();
    if (!filetype) {
        filetype = '';
    }
    switch (filetype.toLowerCase()) {
        case '1':
        case 'image':
            if (extension == 'bmp' || extension == 'jpg' || extension == 'gif' || extension == 'jpeg' || extension == 'png')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .bmp,.jpg,.gif,.jpeg,.png .' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .bmp,.jpg,.gif,.jpeg,.png |';
            break;
        case '2':
        case 'pdf':
            if (extension == 'pdf')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .pdf .' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf |';
            break;
        case '3':
        case 'worddocument':
            if (extension == 'doc' || extension == 'docx' || extension == 'txt')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .doc,.docx,.txt .' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .doc,.docx,.txt |';
            break;
        case '4':
        case 'document':
            if (extension == 'pdf' || extension == 'doc' || extension == 'docx' || extension == 'txt')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.doc,.docx,.txt .' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.doc,.docx,.txt |';
            break;
        case '5':
        case 'all':
            if (extension == 'pdf' || extension == 'doc' || extension == 'docx' || extension == 'txt' || extension == 'xls' || extension == 'xlsx' || extension == 'bmp' || extension == 'jpg' || extension == 'gif' || extension == 'jpeg' || extension == 'png')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.doc,.docx,.txt,.xls,.xlsx,.bmp,.jpg,.gif,.jpeg,.png .' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.doc,.docx,.txt,.xls,.xlsx,.bmp,.jpg,.gif,.jpeg,.png |';
            break;
        case '6':
        case 'onlyimagepdf':
            if (extension == 'pdf' || extension == 'jpg' || extension == 'jpeg' || extension == 'png')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.jpg,.jpeg,.png' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.jpg,.jpeg,.png |';
            break;
        case '7':
        case 'excel':
            if (extension == 'xlsx' || extension == 'xls')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .xlsx,.xls' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .xls,.xlsx |';
            break;
        case '8':
        case 'upload':
            isValid = true;
            break;
        case 'audio':
        case 'video':
            isValid = true;
            break;
        case '9':
        default:
            if (extension == 'pdf' || extension == 'bmp' || extension == 'jpg' || extension == 'gif' || extension == 'jpeg' || extension == 'png' || extension == 'doc' || extension == 'docx' || extension == 'txt')
                isValid = true;
            else
                message = _language ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.bmp,.jpg,.gif,.jpeg,.png,.doc,.docx ,.txt .' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.bmp,.jpg,.gif,.jpeg,.png,.doc,.docx ,.txt |';
            break;

    }
    if (!isValid)
        alert(message);

    return isValid;
}


var secretKey = 'W78K87BJ';

function encryptData(data) {

    if (data.indexOf('Encrypt:') !== -1) {
        //console.log("The string contains 'Encrypt:'.");
        return data;
    } 

    var hashedKey = CryptoJS.SHA256(secretKey).toString(CryptoJS.enc.Hex);
    var iv = CryptoJS.lib.WordArray.random(16);
    var encrypted = CryptoJS.AES.encrypt(data, CryptoJS.enc.Hex.parse(hashedKey), {
        iv: iv,
        padding: CryptoJS.pad.Pkcs7,
        mode: CryptoJS.mode.CBC
    });
    var encryptedData = iv.concat(encrypted.ciphertext).toString(CryptoJS.enc.Base64);
    return "Encrypt:"+encryptedData;
}

function decryptData(encryptedData) {

    if (encryptedData.indexOf('Encrypt:') == -1) {
        //console.log("The string not contains 'Encrypt:'.");
        return encryptedData;
    }
    else {
        encryptedData = encryptedData.replace("Encrypt:", "");
    }

    var hashedKey = CryptoJS.SHA256(secretKey).toString(CryptoJS.enc.Hex);
    var encryptedBytes = CryptoJS.enc.Base64.parse(encryptedData);
    var iv = CryptoJS.lib.WordArray.create(encryptedBytes.words.slice(0, 4), 16);
    var ciphertext = CryptoJS.lib.WordArray.create(
        encryptedBytes.words.slice(4),
        encryptedBytes.sigBytes - 16
    );
    var decrypted = CryptoJS.AES.decrypt(
        { ciphertext: ciphertext },
        CryptoJS.enc.Hex.parse(hashedKey),
        {
            iv: iv,
            padding: CryptoJS.pad.Pkcs7,
            mode: CryptoJS.mode.CBC
        }
    );
    var plaintext = decrypted.toString(CryptoJS.enc.Utf8);
    return plaintext;
}