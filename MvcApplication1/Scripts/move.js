/*
аЁаКбаИаПб move (аПаЕбаЕаМаЕбаЕаНаИаЕ): 11.12.2014
ааВбаОб: ааЛаЕаКбаЕаЙ ааОаНаАаН
ааОаКбаМаЕаНбаАбаИб: http://akonan.ru/?id=24
*/
if(!("konan" in window))konan={};
konan.move=[];
konan.Move=function(id){
var t=this;
var e=t.element=typeof(id)=='string'?document.getElementById(id):id;
for(var n in konan.move)if(konan.move[n].element==e)return konan.move[n];
konan.move.push(t);
t.dt=40;
t.v=t.a=0;
t.position=e.currentStyle?e.currentStyle["position"]:window.getComputedStyle(e,"").getPropertyValue("position");
t.margin=function(){
  var l=e.currentStyle?e.currentStyle["marginLeft"]:window.getComputedStyle(e,"").getPropertyValue("margin-left");
  var t=e.currentStyle?e.currentStyle["marginTop"]:window.getComputedStyle(e,"").getPropertyValue("margin-top");
  l=isNaN(parseInt(l))?0:parseInt(l);
  t=isNaN(parseInt(t))?0:parseInt(t);
  return {'left':l,'top':t};
  }
t.padding=function(){
  var p=e.parentElement;
  var l=p.currentStyle?p.currentStyle["paddingLeft"]:window.getComputedStyle(p,"").getPropertyValue("padding-left");
  var t=p.currentStyle?p.currentStyle["paddingTop"]:window.getComputedStyle(p,"").getPropertyValue("padding-top");
  l=isNaN(parseInt(l))?0:parseInt(l);
  t=isNaN(parseInt(t))?0:parseInt(t);
  return {'left':l,'top':t};
  }
t.client=function(){
  var b=document.body;
  var d=document.documentElement;
  return {
    'scrollTop':d.scrollTop>b.scrollTop?d.scrollTop:b.scrollTop,
    'scrollLeft':d.scrollLeft>b.scrollLeft?d.scrollLeft:b.scrollLeft,
    };
  }
t.move=function(){
  var dt=t.dt/1000;
  t.v=t.v+t.a*dt;
  var x=t.x-t.marginLeft;
  var y=t.y-t.marginTop;
  var s=Math.sqrt(x*x+y*y);
  var ds=t.v*dt+t.a*dt*dt/2;
  if(ds>s||(t.v==0&&t.a==0))ds=s;
  if(s>0)t.marginLeft+=x*ds/s;
  if(s>0)t.marginTop+=y*ds/s;
  t.element.style.marginLeft=Math.round(t.marginLeft)+"px";
  t.element.style.marginTop=Math.round(t.marginTop)+"px";
  if(ds<s)t.timer=setTimeout(function(){t.move()},t.dt);
  else if(t.fn)return t.fn.apply(t.fn,t.arg);
  }
}
function move(id,a,fn){
var u=undefined;
var d=new konan.Move(id);
var m=d.margin();
var p=d.padding();
var r=d.element.parentElement.getBoundingClientRect();
d.marginLeft=m.left;
d.marginTop=m.top;
if(a.v!=u)d.v=a.v;
if(a.a!=u)d.a=a.a;
if(fn!=u)d.fn=fn;
d.arg=Array.prototype.slice.call(arguments,3);
if(a.position=='absolute'){
  d.x=a.x-r.left-d.client().scrollLeft;
  d.y=a.y-r.top-d.client().scrollTop;
  }
else if(a.position=='relative'){
  d.x=a.x;
  d.y=a.y;
  }
d.x-=p.left;
d.y-=p.top;
if(a.position==u){
  d.x=a.x+m.left;
  d.y=a.y+m.top;
  }
clearTimeout(d.timer);
d.move();
}