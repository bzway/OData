CKEDITOR.plugins.add('wechatnews', {
    icons: 'wechatnews',
    init: function (editor) {
        if (false) {
            CKEDITOR.dialog.add('wechatnewsDialog', this.path + 'dialogs/wechatnews.js');
            editor.addCommand('wechatnews', new CKEDITOR.dialogCommand('wechatnewsDialog'));

        } else {
            var cmd = {
                exec: function (editor) {
                    globalEditor = editor;
                    //todo
                    getResource();
                    //editor.insertHtml('<p>111</p>');
                    //editor.insertHtml('<img src="http://hdtest.abc-myclub.com//MemberSite/images/logo.png" />');
                    //editor.insertHtml('<p>222</p>');
                }
            };
            editor.addCommand('wechatnews', cmd);
        }

        editor.ui.addButton('wechatnews', {
            label: '微信素材',
            command: 'wechatnews'
        });
    }
});
var result;
var globalEditor;



function getResource() {
    var token = getToken();
    $.ajax({
        url: "/CmsContainer/GetResource?token=" + token,
        type: "post",
        success: function (data) {
            if (data["result"] == 0) {
                $("#content").empty();

                var content = "<ul>";
                $.each(data["data"], function (i, item) {
                    content += "<li><img style='width: 100px;height: 100px;' src='" + item.FilePath + "' alt='" + item.Version + "'/>" +
                        "<input type='radio' name='resourceImg' value='" + item.Version + "'/>Select</li>";
                });
                content += "<ul>";

                $("#imgcontent").append(content);
                showImgWindow();
                result = data["data"];

                //$("#btnSelect").on("click", data["data"][0], onChoosed);

            }
        }
    });
}

function showImgWindow() {
    $("#bg").css({
        display: "block", height: $(document).height()
    });

    var $box = $('#imgBox');
    $box.css({
        //设置弹出层距离左边的位置
        left: ($("body").width() - $box.width()) / 2 - 20 + "px",
        //设置弹出层距离上面的位置
        top: ($(window).height() - $box.height()) / 2 + $(window).scrollTop() + "px",
        display: "block"
    });

    $("input[name='resourceImg']").click(function () {
        $("#VersionP").show();
        $("#VersionSel").empty();
        var maxVersion = parseInt($("input[name='resourceImg']:checked").val());

        var options = "";

        for (var i = 1; i < maxVersion + 1; i++) {
            options += "<option>" + i + "</option>";
        }

        $("#VersionSel").append(options);
    });
}

$(function () {
    var imgButtonSel = function () {
        var radios = $("input[name='resourceImg']");

        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked == true) {
                // alert("i:" + result[i].FilePath);
                //alert("i:" + i);
                onImgChoosed(result[i]);
            }
        }
    }


    var onImgChoosed = function (value) {
        var version = $("#VersionSel").val();


        var url = "/CmsContainer/GetWeixinImgUrl";

        //如果该素材不存在微信的图片链接，调用接口并生成
        $.get(url, { contentId: value.ID, version: version, w: Math.random() },
            function (data) {
                if (data["errCode"] == 0) {
                    var html = "<img src='" + data["url"] + "' data-content-id='" + value.ID + "' />";
                    globalEditor.insertHtml(html);
                } else {
                    alert("system error");
                }
                
            } ,"json");
    }

    $("#btnImgSelect").on("click", imgButtonSel);
});