window.downloadFile = function (fileName, base64Data) {
    var anchor = document.createElement('a');
    anchor.download = fileName;
    anchor.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64Data;
    anchor.target = "_blank";
    document.body.appendChild(anchor);
    anchor.click();
    document.body.removeChild(anchor);
}