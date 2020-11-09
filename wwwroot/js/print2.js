
/// Detection ///
function findPrinter(query, set) {
    qz.printers.find(query).then(function (data) {
        if (set) { setPrinter(data); }
    }).catch(displayError);
}

function findDefaultPrinter(set) {
    qz.printers.getDefault().then(function (data) {
        if (set) { setPrinter(data); }
    }).catch(displayError);
}

function findPrinters() {
    //get active printer
    //TODO

    //list all printers
    qz.printers.find().then(function (data) {
        var list = '';
        for (var i = 0; i < data.length; i++) {
            list += "<a href=\"#\" class=\"list-group-item list-group-item-action\">" + data[i] + "</a>";
        }
        $('#printer-list').html(list);
    }).catch(displayError);
}

//function detailPrinters() {
//    qz.printers.details().then(function (data) {
//        var list = '';
//        for (var i = 0; i < data.length; i++) {
//            list += "<li>" + (data[i].default ? "* " : "") + data[i].name + "<ul>" +
//                "<li><strong>Driver:</strong> " + data[i].driver + "</li>" +
//                "<li><strong>Density:</strong> " + data[i].density + "dpi</li>" +
//                "<li><strong>Connection:</strong> " + data[i].connection + "</li>" +
//                (data[i].trays ? "<li><strong>Trays:</strong> " + data[i].trays + "</li>" : "") +
//                "</ul></li>";
//        }

//        pinMessage("<strong>Printer details:</strong><br/><ul>" + list + "</ul>");
//    }).catch(displayError);
//}

function displayError(err) {
    console.error(err);
    alert(err);
}

/// QZ Config ///
var cfg = null;
function getUpdatedConfig(cleanConditions) {
    if (cfg == null) {
        cfg = qz.configs.create(null);
    }

    updateConfig(cleanConditions || {});
    return cfg
}

function updateConfig(cleanConditions) {
    var pxlSize = null;
    if (isChecked($("#pxlSizeActive"), cleanConditions['pxlSizeActive'])) {
        pxlSize = {
            width: $("#pxlSizeWidth").val(),
            height: $("#pxlSizeHeight").val()
        };
    }

    var pxlBounds = null;
    if (isChecked($("#pxlBoundsActive"), cleanConditions['pxlBoundsActive'])) {
        pxlBounds = {
            x: $("#pxlBoundX").val(),
            y: $("#pxlBoundY").val(),
            width: $("#pxlBoundWidth").val(),
            height: $("#pxlBoundHeight").val()
        };
    }

    var pxlDensity = includedValue($("#pxlDensity"));
    if (isChecked($("#pxlDensityAsymm"), cleanConditions['pxlDensityAsymm'])) {
        pxlDensity = {
            cross: $("#pxlCrossDensity").val(),
            feed: $("#pxlFeedDensity").val()
        };
    }

    var pxlMargins = includedValue($("#pxlMargins"));
    if (isChecked($("#pxlMarginsActive"), cleanConditions['pxlMarginsActive'])) {
        pxlMargins = {
            top: $("#pxlMarginsTop").val(),
            right: $("#pxlMarginsRight").val(),
            bottom: $("#pxlMarginsBottom").val(),
            left: $("#pxlMarginsLeft").val()
        };
    }

    var copies = 1;
    var jobName = null;
    if ($("#rawTab").hasClass("active")) {
        copies = includedValue($("#rawCopies"));
        jobName = includedValue($("#rawJobName"));
    } else {
        copies = includedValue($("#pxlCopies"));
        jobName = includedValue($("#pxlJobName"));
    }

    cfg.reconfigure({
        altPrinting: includedValue($("#rawAltPrinting"), isChecked($("#rawAltPrinting"), cleanConditions['rawAltPrinting'])),
        encoding: includedValue($("#rawEncoding")),
        endOfDoc: includedValue($("#rawEndOfDoc")),
        perSpool: includedValue($("#rawPerSpool")),

        bounds: pxlBounds,
        colorType: includedValue($("#pxlColorType")),
        copies: copies,
        density: pxlDensity,
        duplex: includedValue($("#pxlDuplex")),
        interpolation: includedValue($("#pxlInterpolation")),
        jobName: jobName,
        margins: pxlMargins,
        orientation: includedValue($("#pxlOrientation")),
        paperThickness: includedValue($("#pxlPaperThickness")),
        printerTray: includedValue($("#pxlPrinterTray")),
        rasterize: includedValue($("#pxlRasterize"), isChecked($("#pxlRasterize"), cleanConditions['pxlRasterize'])),
        rotation: includedValue($("#pxlRotation")),
        scaleContent: includedValue($("#pxlScale"), isChecked($("#pxlScale"), cleanConditions['pxlScale'])),
        size: pxlSize,
        units: includedValue($("input[name='pxlUnits']:checked"))
    });
}

function getUpdatedOptions(onlyPixel) {
    if (onlyPixel) {
        return {
            pageWidth: $("#pPxlWidth").val(),
            pageHeight: $("#pPxlHeight").val()
        };
    } else {
        return {
            language: 'EPL',
            x: $("#pX").val(),
            y: $("#pY").val(),
            dotDensity: $("#pDotDensity").val(),
            xmlTag: $("#pXml").val(),
            pageWidth: $("#pRawWidth").val(),
            pageHeight: $("#pRawHeight").val()
        };
    }
}

function getSerialOptions() {
    var options = {
        baudRate: $("#serialBaud").val(),
        dataBits: $("#serialData").val(),
        stopBits: $("#serialStop").val(),
        parity: $("#serialParity").val(),
        flowControl: $("#serialFlow").val(),
        encoding: $("#serialEncoding").val(),
        rx: {
            start: $("#serialStart").val(),
            end: $("#serialEnd").val() || null,
            width: $("#serialWidth").val() || null,
            untilNewline: $("#serialNewline").prop('checked'),
            lengthBytes: null,
            crcBytes: null,
            includeHeader: $("#serialHeader").prop('checked'),
            encoding: $("#serialRespEncoding").val()
        }
    };
    if (isChecked($("#serialLengthActive"))) {
        options.rx.lengthBytes = {
            index: $("#serialLenIndex").val(),
            length: $("#serialLenLength").val(),
            endian: $("input[name='serialLenEndian']:checked").val()
        };
    }
    if (isChecked($("#serialCrcActive"))) {
        options.rx.crcBytes = {
            index: $("#serialCrcIndex").val(),
            length: $("#serialCrcLength").val()
        };
    }

    return options;
}

function setPrintFile() {
    setPrinter({ file: $("#askFile").val() });
    $("#askFileModal").modal('hide');
}

function setPrintHost() {
    setPrinter({ host: $("#askHost").val(), port: $("#askPort").val() });
    $("#askHostModal").modal('hide');
}

function setPrinter(printer) {
    var cf = getUpdatedConfig();
    cf.setPrinter(printer);

    if (printer && typeof printer === 'object' && printer.name == undefined) {
        var shown;
        if (printer.file != undefined) {
            shown = "<em>FILE:</em> " + printer.file;
        }
        if (printer.host != undefined) {
            shown = "<em>HOST:</em> " + printer.host + ":" + printer.port;
        }

        $("#configPrinter").html(shown);
    } else {
        if (printer && printer.name != undefined) {
            printer = printer.name;
        }

        if (printer == undefined) {
            printer = 'NONE';
        }
        $("#configPrinter").html(printer);
    }
}