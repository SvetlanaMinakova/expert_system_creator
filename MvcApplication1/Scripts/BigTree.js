//main parametres
//node colors
var rootcolor = "magenta";
var propertycolor = "pink";
var questioncolor = "red";
var answercolor = "blue";
var selectorcolor = "yellow";
var linkcolor = "blue";
//link colors
var roottoproplinkcolor = "magenta";
var proptoquestionlinkcolor = "magenta";
var answertochildquestioncolor = "black";
var quesiontoanswercolor = "red";
var answertoselectorcolor = "green";

//picture's elements
var paper;
var circles = [];
var bigCircles = [];
var nodeLables = [];
var linkLabels = [];
var linkLines = [];

var Trees = [];
var drawPaper = function (width,height)
{ paper = Raphael(document.getElementById("placeholder"), width, height); }


//drawing functions
var ShowProperties = function (data) {
    var size = 30;
    var x = 200;
    var y = 300;
    var deltax = 500;
    var PropertyNumber = data.propertiesnumber;
    var curProperty;
    var mid = PropertyNumber / 2;
    var sumHeight = 100;
    var sumWidth;
    var RootSize = 40;
    var RootX = x + (deltax + size) * (PropertyNumber - 1) / 2 - RootSize;
    var RootY = 100;

    //add root
    addNode("Root", RootX, RootY, RootSize, rootcolor, "Root", "#")

    //add properties
    for (var i = 0; i < PropertyNumber; i++) {

        //correcting the tree's height
        if (i != 0) {
            if (i <= mid)
                sumHeight = sumHeight + Trees[i - 1].LevelsNumber * 300 + 200;
            else
                sumHeight = sumHeight - Trees[i - 1].LevelsNumber * 300 - 200;
        }
        //current property
        curProperty = data.properties[i];
        //link property node with root node
        addLink(x, y, RootX, RootY, roottoproplinkcolor);
        //link property with childtree
        addLink(x, y, x, y + sumHeight, proptoquestionlinkcolor);
        //add property node
        addNode(curProperty.id, x, y, size, propertycolor, curProperty.text, curProperty.link);
        //add child tree
        addTree(x, y + sumHeight, curProperty);
        //change-x coordinate
        x = x + deltax;
    }
}

    var ShowQuestions = function (Qx, deltax, Qy, deltay,property) {
    var size = 10;
    var curQuestion;
    var x = Qx;
    var y = Qy;

    for (var j = 0; j < property.levelsnumber; j++) {
        for (var i = 0; i < property.levels[j].qnumberonlevel; i++)
        {
            curQuestion = property.levels[j].questions[i];
            addNode(curQuestion.id, x, y, size, questioncolor, curQuestion.text, curQuestion.link);

            ShowAnswers(x, deltax, y, deltay,curQuestion,property.id,j);
            
            x = x + deltax;
            y += deltay;
        }
        y = y + deltay / 2;
        x = Qx;
    }

    }

    var ShowAnswers = function (x, deltax, y, deltay, curQuestion,propId,levelid) {
        var size = 10;
        var AX = x;
        var AY=y+50;
        var deltaAX = 150;
        var curChildQ;
        var chQx;
        var chQy;
        for (var i = 0; i < curQuestion.answers.length; i++)
        {
            curAnswer = curQuestion.answers[i];
            addNode(curAnswer.id, AX, AY, size, answercolor, curAnswer.text, curAnswer.link);
            addLink(x, y, AX, AY,quesiontoanswercolor);

            //link with child question(answer can have 0 or 1 child questions)
            curChildQ = curAnswer.questions[0];
            if (curChildQ != null) {
                chQx = 200 + (propId - 1) * 500 + deltax * (curAnswer.id - 1) - deltax*(levelid%2);
                chQy= AY + deltay / 2 + 150 + 200 * (curAnswer.id-1);
                addLink(AX, AY, chQx, chQy, answertochildquestioncolor);
            }

            ShowSelectors(AX, deltaAX, AY, curAnswer);
            AX = AX + deltaAX;
        }
    }

    var ShowSelectors = function (AX, deltaAX, AY, curAnswer) {
        var SELX = AX;
        var SELY = AY+50;
        var curSel;
        var size = 10;

        for (var i = 0; i < curAnswer.selectors.length;i++)
        {
            curSelector = curAnswer.selectors[i];
            addLink(AX, AY, SELX, SELY, answertoselectorcolor);
            addNode(curSelector.id, SELX, SELY, size, selectorcolor, curSelector.text, curSelector.link);

            SELX = SELX + deltaAX / 2;
            SELY = SELY + 15;
        }

    }


//picture's elements structure functions
var acircle = function (id, x, y, size, Href) {
    this.BigCircle = paper.circle(x, y, size * 2).attr("fill", "grey");
    this.BigCircle.attr("opacity", ".30");
    this.circle = paper.circle(x, y, size).attr("fill", "red");
    this.id = id;
    this.circle[0].onclick = function () { document.location.href = Href };
}

var alable = function (id, x, y, txt, Href) {
    this.lable = paper.text(x, y, txt);
    this.id = id;
    this.lable[0].onclick = function () { document.location.href = Href };
    this.lable.attr("class", "TreeText");
    this.lable.attr("name", "TreeText");
}

var aline = function (x1, y1, x2, y2, color)
{ this.path = paper.path({ stroke: color }).moveTo(x1, y1).lineTo(x2, y2); }

var addNode = function (id, x, y, size, color, txt, href) {
    circles.push(new acircle(id, x, y, size, href).circle);
    circles[circles.length - 1].attr("fill", color);
    nodeLables.push(new alable(id, x, y - size, txt, href).lable);
}

var addLink = function (x1, y1, x2, y2, color)
{ linkLines.push(new aline(x1, y1, x2, y2, color).path); }

var AddQuestion = function (id, x, y, size, color, txt, href) {
    circles.push(new acircle(id, x, y, size, href).circle);
    circles[circles.length - 1].attr("fill", color);
    nodeLables.push(new alable(id, x, y - size, txt, href).lable);
}

var addTree = function (tx, ty, property)
{ Trees.push(new Tree(tx, ty, property)); }

var Tree = function (tx, ty, property) {

    this.Id;
    this.Height;
    this.Width;
    this.LevelsNumber = property.levelsnumber;
    this.Property = property;
    //show questions
    ShowQuestions(tx, 200, ty, 200, property);
}











