$(function () {
    $.get("/ExpertSystem/GetJsonTree/" + document.getElementById("curEsysId").textContent, function (data) {
        var maxlevnumber = 0;
        var curlevnumber;
        for (var i = 0; i < data.propertiesnumber; i++)
        {curlevnumber=data.properties[i].levelsnumber;
        if (maxlevnumber < curlevnumber)
            maxlevnumber = curlevnumber;
        }
        drawPaper((data.propertiesnumber + 1) * 500, 500 + data.propertiesnumber*100 + (200 * maxlevnumber * 2));
        ShowProperties(data);
    });

})
