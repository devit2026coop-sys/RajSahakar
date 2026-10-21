function loadScript(src, callback) {
    const script = document.createElement('script'); // Create a script element
    script.src = src; // Set the source to the script URL
    script.type = 'text/javascript'; // Set the type (optional, defaults to 'text/javascript')
    script.async = true; // Load the script asynchronously (optional)

    // Script load success
    script.onload = () => {
        console.log(`${src} has been loaded.`);
        if (callback) callback(); // Call the callback if provided
    };

    // Script load error
    script.onerror = () => {
        console.error(`Failed to load script: ${src}`);
    };

    document.head.appendChild(script); // Append the script to the <head>
}

// Usage Example
//loadScript('https://aadhaarauth.rajasthan.gov.in/service/aadhaarauth.js', () => {
//    console.log('file1.js is loaded. You can now use its functions.');
//});

function AadhaarAuthEsign(http, callbackSuccess) {
    debugger;
    //alert(1);
    _callback = callbackSuccess;
    _transactionId = "";
    _http = http;
    
    GetEncryptedData();
}

function GetEncryptedData() {

    _http.get(SiteUrl + "/ApplicationFlow/GenerateAadhaarOTPEsign_Test/")
        .then(function (response) {
            console.log(response.data);
            EncData = response.data.EncData;
            ReqId = response.data.RequestID;
         //   EncData = response.data;
            generateOTPEsign();
        })
        .catch(function (error) {
            console.error('Error:', error);
        });
};

function generateOTPEsign() {
    loadScript('https://aadhaarauth.rajasthan.gov.in/service/aadhaarauth.js', () => {

        const functionName = 'AadhaarAuth'; // Function name as a string

        if (typeof window[functionName] === 'function') {
            $(".se-pre-con").fadeOut("slow");
            const authInstance = new window[functionName]({
                AppCode: 'PRAVL22868ABFAP012',
                Data: EncData
            });


            $(".se-pre-con").fadeOut("slow");
            authInstance.open({
                handler: (response) => {
                    debugger;
                    $(".se-pre-con").fadeOut("slow");
                    console.log("Authentication successful", response);
                    if (response && response.Data != null && response.Data != '') {
                        GetDecryptedResponse(response.Data);    
                       
                     
                    } else {
                        console.error("Authentication failed or response invalid", response);
                    }

                }
            });

            setTimeout(function () {
                $('#aadhaar_auth_container').addClass('thirdPrtyModel');
            }, 500)


        } else {
            console.error(`${functionName} is not defined.`);
        }
    });
}

function GetDecryptedResponse(response) {
    _http.get(SiteUrl + "/ApplicationFlow/GenerateAadhaarOTPDec_Test/?encData=" + encodeURIComponent(response))
        .then(function (response) {
            console.log(response.data);
            // EncResponse = response.data;
            var authData = {
                TransId: response.data
            }            
            if (ReqId != "" && ReqId == response.data);
            {        
                _callback(authData);
            }           

        })
        .catch(function (error) {
            console.error('Error:', error);
        });
};


