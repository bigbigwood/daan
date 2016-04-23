//网页功能控制，如禁止刷新，后退等
function document.oncontextmenu(){event.returnValue=false;}//屏蔽鼠标右键    
function window.onhelp(){return false} //屏蔽F1帮助    
function document.onkeydown()    
{    
    if ((window.event.altKey)&&    
        ((window.event.keyCode==37)|| //屏蔽 Alt+ 方向键 ←    
        (window.event.keyCode==39))) //屏蔽 Alt+ 方向键 →    
        {    
            event.returnValue=false;    
        }        
    if ((event.keyCode==116)|| //屏蔽 F5 刷新键    
        (event.ctrlKey && event.keyCode==82)) //Ctrl + R    
        {    
            event.keyCode=0;    
            event.returnValue=false;    
        }    
    if (event.keyCode==122){event.keyCode=0;event.returnValue=false;}   //屏蔽F11    
    if (event.ctrlKey && event.keyCode==78) event.returnValue=false;    //屏蔽 Ctrl+n    
    if (event.shiftKey && event.keyCode==121)event.returnValue=false;   //屏蔽 shift+F10    
    if (window.event.srcElement.tagName == "A" && window.event.shiftKey) window.event.returnValue = false;//屏蔽 shift 加鼠标左键新开一网页
}    
