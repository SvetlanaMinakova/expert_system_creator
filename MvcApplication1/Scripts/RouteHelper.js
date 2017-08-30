var changeWayfunc = function (routerId, infokeeperId)
{ document.getElementById("\""+routerId+"\"").attributes.getNamedItem('href').value += "/" + document.getElementById("\""+infokeeperId+"\"").value; }
