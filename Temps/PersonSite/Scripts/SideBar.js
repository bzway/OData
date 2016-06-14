/**
 * jQuery SideBar 1.4.3
 * 
 * Copyright (c) 2009-2015 www.bzway.com. All rights reserved.
 *
 * Licensed under the GPL license: http://www.gnu.org/licenses/gpl.txt
 * To use it on other terms please contact us at zhumingwu@bzway.com
 *
 */

(function ($) {
     // 在我们插件容器内，定义一个私有方法
     var privateFunction = function () {
         // code here
     };

     // 通过字面量创造一个对象，存储我们需要的共有方法
     var methods = {
         // 在字面量对象中定义每个单独的方法
         init: function () {
             // 为了更好的灵活性，对来自主函数，并进入每个方法中的选择器其中的每个单独的元素都执行代码
             return this.each(function () {
                 // 为每个独立的元素创建一个jQuery对象
                 var $this = $(this);
                 // 执行代码 例如：privateFunction()
             });
         },
         destroy: function () {
             // 对选择器每个元素都执行方法
             return this.each(function () {
                 // 执行代码
             });
         }
     };

     $.fn.SideBar = function () {
         var $this = $(this);
         var div = $this.children("div");
         ui = $this.width();
         $(div[0]).addClass("float-left");
         $(div[1]).addClass("float-right");

         //var width = Math.round($(div[0]).width() / $this.width() * 100);
         //if (width == 100) {
         //    width = 20;
         //}
         //$(div[0]).css("width", width + "%");
         //width = Math.round($(div[1]).size.width() / $this.width() * 100);
         //$(div[1]).css("width", width + "%");
     };

 })(jQuery);