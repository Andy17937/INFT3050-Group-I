window.rootPath = (function (src) {
    src = document.scripts[document.scripts.length - 1].src;
	console.log(src.substring(0, src.lastIndexOf("/") + 1))
    return src.substring(0, src.lastIndexOf("/") + 1);
})();

layui.config({
    base: rootPath,
    version: true
}).extend({
    http: 'http/http', 			//  Web Request Interface Extension
    layarea: 'layarea/layarea',// Provincial and municipal city linkage Optioner
    tinymce: 'tinymce/tinymce' 	// rich text editor
});