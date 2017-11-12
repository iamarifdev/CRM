// This one freezes the content
function printDiv(divName) {
    var printContents = document.getElementById(divName).innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
    document.location.reload(true);
}

// This one perfectly working [100% working]
// Pure Javascript
function PrintDocumentDivById(divId, isLandscape) {
    isLandscape = isLandscape || false;
    //Works with Chome, Firefox, IE, Safari
    var headElement = document.getElementsByTagName("head")[0];
    var divElements = document.getElementById(divId).innerHTML;
    var printWindow = window.open("", "_blank", "");
    //open the window
    printWindow.document.open();
    if (isLandscape) headElement.innerHTML += '<style type="text/css" media="print">@page { size: landscape; }</style>';
    printWindow.document.write("<html>" + headElement.innerHTML + "<body>" + divElements + "</body></html>");
    printWindow.document.close();
    printWindow.focus();
    //The Timeout is ONLY to make Safari work, but it still works with FF, IE & Chrome.
    setTimeout(function () {
        printWindow.print();
        printWindow.close();
    }, 100);
}