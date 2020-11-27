$(document).ready(function () {

    qz.security.setSignatureAlgorithm("SHA512"); // Since 2.1
    qz.security.setSignaturePromise(function (toSign) {
        return function (resolve, reject) {
            //$.ajax({
            //    url: '/fr/SignMessage/SignRequest?request=' + toSign,
            //    type: "GET",
            //    //dataType: "html",
            //    async: false,
            //    success: function (response) {
            //        alert(response)

            //    },
            //    error: function (xhr, status, error) {
            //        alert('Error');
            //    }
            //});

            fetch("/fr/SignMessage/SignRequest?request=" + toSign, { cache: 'no-store', headers: { 'Content-Type': 'text/plain' } })
                .then((data) => { data.ok ? resolve(data.text()) : reject(data.text()); });
        };
    });

    qz.security.setCertificatePromise((resolve, reject) => {
        fetch("/private/digital-certificate.txt", { cache: 'no-store', headers: { 'Content-Type': 'text/plain' } })
            .then((data) => { data.ok ? resolve(data.text()) : reject(data.text()); });
    });
});