$(function () {
    $("#check").click(function (e) {
        $.get("/ExpertSystem/CheckJsonTree/" + document.getElementById("curEsysId").textContent, function (checkresults) {
        var result = $('#checkres'); // get result element
        var resultcontent = $('#checkrescontent'); // get result content element
        resultcontent.empty(); //clear result element
        if(checkresults.isTreeCorrect)
            resultcontent.append('<p class="correct">  Tree is correct </p>'); // add data to result
        for(var i=0;i<checkresults.Errors.length;i++)
            resultcontent.append('<p class="error"> Error: ' + checkresults.Errors[i] + '</p>');
        for (var i = 0; i < checkresults.Warnings.length; i++)
            resultcontent.append('<p class="warning"> Warning: ' + checkresults.Warnings[i] + '</p>');
            //show result
        result.attr("class", "checkresult")
    });
    });

    $("#hidecheck").click(function (e) {
            var result = $('#checkres'); // get result element
            var resultcontent = $('#checkrescontent'); // get result content element
            resultcontent.empty(); //clear result element
            //hide window
            result.attr("class", "checkresulthidden")
    });
})