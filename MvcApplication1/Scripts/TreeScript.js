
var paper = Raphael(document.getElementById("placeholder"), 1500, 1500);
var circles = [];
var bigCircles = [];
var nodeLables = [];
var linkLabels = [];
var linkLines = [];
var LevelsNumber = document.getElementById("levelsNumber").textContent;
var QnumberOnLevel = [];
var AnumberOnLevel = [];

var acircle = function (id,x, y,size, Href)
{
    this.BigCircle = paper.circle(x, y, size * 2).attr("fill", "grey");
    this.BigCircle.attr("opacity", ".30");
    this.circle = paper.circle(x, y, size).attr("fill", "red");
    this.id = id;
    this.circle[0].onclick = function () { document.location.href = Href };
}

var alable = function (id,x, y, txt,Href)
{
    this.lable = paper.text(x, y, txt);
    this.id = id;
    this.lable[0].onclick = function () { document.location.href = Href };
    this.lable.attr("class", "TreeText");
}

var aline = function (x1, y1, x2, y2, color)
{ this.path = paper.path({ stroke: color }).moveTo(x1, y1).lineTo(x2, y2); }

var addNode = function (id,x, y, size, color, txt,href)
{
    circles.push(new acircle(id, x, y, size, href).circle);
    circles[circles.length - 1].attr("fill", color);
    nodeLables.push(new alable(id,x,y-size, txt,href).lable);
}

var AddQuestion = function(id,x, y, size, color, txt,href)
{
    circles.push(new acircle(id,x,y, size,href).circle);
    circles[circles.length - 1].attr("fill", color);
    nodeLables.push(new alable(id,x,y-size, txt,href).lable);
}

var addLink = function (x1, y1, x2, y2, color)
{ linkLines.push(new aline(x1, y1, x2, y2, color).path); }


var GetQNumberOnLevel = function ()
{
    for (var i = 1; i <= LevelsNumber; i++)
    {
        var curContext = document.getElementById("level" + i.toString()).textContent;
        QnumberOnLevel[i-1]=curContext;
    }
}

var ShowSelectors = function(AX,deltaAX,AY,curAnswerId)
{
    //добавляем все возможные селекторы
    var SELX;
    var SELY;
    var SELcounter;
    var curSel;
    var curHTMLLink;
    var SELcounter = 1;
    var size = 10;
    var color = "yellow";

    var curSelectorId = curAnswerId + "S" + SELcounter.toString();
    var    curSelector = document.getElementById(curSelectorId);

    SELX = AX;
    SELY = AY + 50;
   
    while (curSelector != null) {
        curSel = curSelector.textContent;
        curHTMLLink = curSelector.attributes.getNamedItem('href').value;
        addNode(curSelectorId, SELX, SELY, size,color, curSel, curHTMLLink);
        addLink(AX, AY, SELX, SELY, "blue")

        SELcounter = SELcounter + 1;
        curSelectorId = curAnswerId + "S" + SELcounter.toString();
        curSelector = document.getElementById(curSelectorId);
        SELX = SELX + deltaAX / 2;
        SELY = SELY + 15;

    }

}


var ShowAnswers = function (x,deltax,y,deltay,curQuestionId,curLevelId)
{
    var size = 10;
    var color = "blue";
    var AX;
    var AY;
    var deltaAX = 200
    var curHTMLLink;
    var curAnsw;
    var curChildQId;
    var curChildQ;
    var Acounter = 1;

    var curAnswerId = curQuestionId + "A" + Acounter.toString();
    var curAnswer = document.getElementById(curAnswerId);

    AX = x;
    AY = y + 50;
    while (curAnswer != null) {
        curAnsw = curAnswer.textContent;
        curHTMLLink = curAnswer.attributes.getNamedItem('href').value;
        addNode(curAnswerId, AX, AY, size, color, curAnsw, curHTMLLink);
        addLink(x, y, AX, AY, "black");
        //добавляем связь с дочерним вопросом если он есть; j=level
        //если уровень-не последний
        if (curLevelId != levelsNumber - 1) {
            curChildQId = curAnswerId + "CQ";
            curChildQ = document.getElementById(curChildQId);
            if (curChildQ != null) {
                addLink(AX, AY, 200 + deltax * curChildQ.textContent, AY + deltay/2 + 150 + 200 * (curChildQ.textContent), "red");
            }
        }

        //добавляем все возможные селекторы
        ShowSelectors(AX, deltaAX, AY, curAnswerId);

        Acounter = Acounter + 1;
        curAnswerId = curQuestionId + "A" + Acounter.toString();
        curAnswer = document.getElementById(curAnswerId);
        AX = AX + deltaAX;

    }


}

var ShowQuestions = function ()
{
    var size = 10;
    var color = "red";
    var x = 200;
    var y=100;
    var deltax=300;
    var deltay=200;
    var curQuestionId;
    var curQuestion;
    var curQuestionTxt;
    var curHTMLLink;


    //добавляем все вопросы
    for (var j = 0; j<LevelsNumber; j++ )
    {
        for (var i = 0; i < QnumberOnLevel[j]; i++)
        {
            curQuestionId = "L" + (j + 1).toString() + "Q" + ((j + 1) * 100 + i + 1).toString();
            curQuestion = document.getElementById(curQuestionId);
            curQuestionTxt = curQuestion.textContent;
            curHTMLLink = curQuestion.attributes.getNamedItem('href').value;

            addNode(curQuestionId,x, y, size, color, curQuestionTxt.toString(), curHTMLLink);

            //добавляем ответы
            ShowAnswers(x, deltax, y, deltay, curQuestionId,j);

            x = x + deltax;
            y += deltay;
        }
        y = y + deltay/2;
        x = 200;
    }

}

    
