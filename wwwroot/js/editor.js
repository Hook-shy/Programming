 // 窗口加载事件仅用于窗口高度取决于图像 
 $(window).bind("load", function() { 
    var footerHeight = 0, 
    footerTop = 0, 
    $footer = $("#apeak_footer"); 
    positionFooter(); 
　　//定义positionFooter function 
　　function positionFooter() { 
　　//取到div#footer高度 
　　footerHeight = $footer.height(); 
　　 //div#footer离屏幕顶部的距离 
　　footerTop = ($(window).scrollTop()+$(window).height()-footerHeight)+"px"; 
　　/* DEBUGGING STUFF 
　　console.log("Document height: ", $(document.body).height()); 
　　console.log("Window height: ", $(window).height()); 
　　 console.log("Window scroll: ", $(window).scrollTop()); 
　　console.log("Footer height: ", footerHeight); 
　　console.log("Footer top: ", footerTop); 
　　console.log("-----------") 
　　*/ 
　　//如果页面内容高度小于屏幕高度，div#footer将绝对定位到屏幕底部，否则div#footer保留它的正常静态定位 
　　　　if ( ($(document.body).height()+footerHeight) < $(window).height()) { 
  　　 $footer.css({ 
       　　 position: "absolute" 
   　　 }).stop().animate({ 
       　　 top: footerTop 
   　　 }); 
　　　　 }else { 
  　　　　 $footer.css({ 
      　　 position: "static" 
    　　　　}); 
　　　　} 
　　} 
$(window).scroll(positionFooter).resize(positionFooter); 
});