$(document).ready(function () {

    qz.security.setSignatureAlgorithm("SHA512"); // Since 2.1
    qz.security.setSignaturePromise(function (toSign) {
        return function (resolve, reject) {
            fetch("/fr/SignMessage/SignMessage?request=" + toSign, { cache: 'no-store', headers: { 'Content-Type': 'text/plain' } })
                .then((data) => { data.ok ? resolve(data.text()) : reject(data.text()); });
        };
    });

    qz.security.setCertificatePromise((resolve, reject) => {
        fetch("/private/digital-certificate.txt", { cache: 'no-store', headers: { 'Content-Type': 'text/plain' } })
            .then((data) => { data.ok ? resolve(data.text()) : reject(data.text()); });
    });
});