

function PrintReport(printer, json, dsjson) { 
    document.ActiveXPrint.PrintReport(printer, json, dsjson);
}

function PrintReport2(printer, repcode, dsjson) {
    document.ActiveXPrint.PrintReport2(printer, repcode, dsjson);
}

function PrintBarCode(printer, json) {
    document.ActiveXPrint.PrintBarCode(printer, json);
}

function PrintOrderNumAndUserName(printer, json) {
    document.ActiveXPrint.PrintOrderNumAndUserName(printer, json);
}

function test() {
    try {
        document.ActiveXPrint.test();
    } catch (e) {
        location.href ="../report/DownLoadPrintActivex.aspx";  
    }
}
        

