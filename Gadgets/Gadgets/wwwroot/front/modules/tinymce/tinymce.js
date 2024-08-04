//  菜单显示异常Modifytinymce/skins/ui/oxide/skin.min.css:96 .tox-silver-sink的z-indexValue
//  http://tinymce.ax-z.cn/   中文文档

layui.define(['jquery'],function (exports) {
    var $ = layui.$

    var modFile = layui.cache.modules['tinymce'];

    var modPath = modFile.substr(0, modFile.lastIndexOf('.'))

    var setter = layui.setter || {}

    var response = setter.response || {}


    var settings = {
        base_url: modPath
        , images_upload_url: '/rest/upload'//Image upload interface, can be passed in option, or Modify here, option's Value takes precedence
        , language: 'zh_CN'//language, which can be passed in at option or Modified here, with option's Value taking precedence
        , response: {//Backend return data format setting
            statusName: response.statusName || 'code'//Returns the Status field
            , msgName: response.msgName || 'msg'//Return message field
            , dataName: response.dataName || 'data'//Returned data
            , statusCode: response.statusCode || {
                ok: 0//Data normal
            }
        }
        , success: function (res, succFun, failFun) {//Image upload completion callback
            if (res[this.response.statusName] == this.response.statusCode.ok) {
                succFun(res[this.response.dataName]);
            } else {
                failFun(res[this.response.msgName]);
            }
        }
    };


    var t = {};

    //initialization
    t.render = function (option,callback) {

        var admin = layui.admin || {}

        option.base_url = option.base_url ? option.base_url : settings.base_url

        option.language = option.language ? option.language : settings.language

        option.selector = option.selector ? option.selector : option.elem

        option.quickbars_selection_toolbar = option.quickbars_selection_toolbar ? option.quickbars_selection_toolbar : 'cut copy | bold italic underline strikethrough '

        option.plugins = option.plugins ? option.plugins : 'quickbars print preview searchreplace autolink fullscreen image link media codesample table charmap hr advlist lists wordcount imagetools indent2em';

        option.toolbar = option.toolbar ? option.toolbar : 'undo redo | forecolor backcolor bold italic underline strikethrough | indent2em alignleft aligncenter alignright alignjustify outdent indent | link bullist numlist image table codesample | formatselect fontselect fontsizeselect';

        option.resize = false;

        option.elementpath = false

        option.branding = false;

        option.contextmenu_never_use_native = true;

        option.menubar = option.menubar ? option.menubar : 'file edit insert format table';

        option.images_upload_url = option.images_upload_url ? option.images_upload_url : settings.images_upload_url;

        option.images_upload_handler = option.images_upload_handler? option.images_upload_handler : function (blobInfo, succFun, failFun) {

            var formData = new FormData();

            formData.append('target', 'edit');

            formData.append('edit', blobInfo.blob());

            var ajaxOpt = {

                url: option.images_upload_url,

                dataType: 'json',

                type: 'POST',

                data: formData,

                processData: false,

                contentType: false,

                success: function (res) {

                    settings.success(res, succFun, failFun)

                },
                error: function (res) {

                    failFun("Network Error：" + res.status);

                }
            };

            if (typeof admin.req == 'function') {

                admin.req(ajaxOpt);

            } else {

                $.ajax(ajaxOpt);

            }
        }

        option.menu = option.menu ? option.menu : {
            file: {title: 'File', items: 'newdocument | print preview fullscreen | wordcount'},
            edit: {title: 'Edit', items: 'undo redo | cut copy paste pastetext selectall | searchreplace'},
            format: {
                title: 'Format',
                items: 'bold italic underline strikethrough superscript subscript | formats | forecolor backcolor | removeformat'
            },
            table: {title: 'Table', items: 'inserttable tableprops deletetable | cell row column'},
        };
        if(typeof tinymce == 'undefined'){

            $.ajax({//Get Plugins
                url: option.base_url + '/tinymce.js',

                dataType: 'script',

                cache: true,

                async: false,
            });

        }

        layui.sessionData('layui-tinymce',{

            key:option.selector,

            value:option

        })

        tinymce.init(option);

        if(typeof callback == 'function'){

            callback.call(option)

        }

        return tinymce.activeEditor;
    };

    t.init = t.render

    // Get the editor object corresponding to the ID
    t.get = (elem) => {

        if(elem && /^#|\./.test(elem)){

            var id = elem.substr(1)

            var edit = tinymce.editors[id];

            if(!edit){

                return console.error("Editor not loaded")

            }

            return edit

        } else {

            return console.error("Elem error")

        }
    }

    //Reload
    t.reload = (option,callback) => {
        option = option || {}

        var edit = t.get(option.elem);

        var optionCache = layui.sessionData('layui-tinymce')[option.elem]

        edit.destroy()

        $.extend(optionCache,option)

        tinymce.init(optionCache)

        if(typeof callback == 'function'){

            callback.call(optionCache)

        }

        return tinymce.activeEditor;
    }


    exports('tinymce', t);
});
